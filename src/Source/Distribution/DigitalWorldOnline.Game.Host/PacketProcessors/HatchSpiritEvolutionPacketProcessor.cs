﻿using DigitalWorldOnline.Application;
using DigitalWorldOnline.Application.Separar.Commands.Create;
using DigitalWorldOnline.Application.Separar.Commands.Update;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Models.Asset;
using DigitalWorldOnline.Commons.Models.Base;
using DigitalWorldOnline.Commons.Models.Digimon;
using DigitalWorldOnline.Commons.Packets.Chat;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.Commons.Packets.Items;
using DigitalWorldOnline.Commons.Utils;
using DigitalWorldOnline.Commons.Writers;
using DigitalWorldOnline.Game.Managers;
using DigitalWorldOnline.GameHost;
using MediatR;
using Serilog;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class HatchSpiritEvolutionPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.HatchSpiritEvolution;

        private readonly StatusManager _statusManager;
        private readonly MapServer _mapServer;
        private readonly DungeonsServer _dungeonServer;
        private readonly AssetsLoader _assets;
        private readonly ILogger _logger;
        private readonly ISender _sender;

        public HatchSpiritEvolutionPacketProcessor(
            StatusManager statusManager,
            MapServer mapServer,
            AssetsLoader assets,
            ILogger logger,
            ISender sender,
            DungeonsServer dungeonsServer
        )
        {
            _statusManager = statusManager;
            _mapServer = mapServer;
            _assets = assets;
            _logger = logger;
            _sender = sender;
            _dungeonServer = dungeonsServer;
        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            var packet = new GamePacketReader(packetData);

            var targetType = packet.ReadInt();

            var digiName = packet.ReadString();

            var NpcId = 90005; // TODO: Ajustar leitura do packet.

            var extraEvolutionNpc = _assets.ExtraEvolutions.FirstOrDefault(x => x.NpcId == NpcId);

            if (extraEvolutionNpc == null)
                return;

            var extraEvolutionInfo = extraEvolutionNpc.ExtraEvolutionInformation.FirstOrDefault(x => x.ExtraEvolution.Any(x => x.DigimonId == targetType))?.ExtraEvolution;


            if (extraEvolutionInfo == null)
                return;

            var extraEvolution = extraEvolutionInfo.FirstOrDefault(x => x.DigimonId == targetType);

            if (extraEvolution == null)
            {
                //_logger.Warning($"Unknown hatch info for egg {client.Tamer.Incubator.EggId}.");
                //client.Send(new SystemMessagePacket($"Unknown hatch info for egg {client.Tamer.Incubator.EggId}."));
                return;
            }        

            if (!client.Tamer.Inventory.RemoveBits(extraEvolution.Price))
            {
                //client.Send(new SystemMessagePacket($"Insuficient bits for item craft NPC id {npcId} and id {sequencialId}."));
                //_logger.Warning($"Insuficient bits for item craft NPC id {npcId} and id {sequencialId} for tamer {client.TamerId}.");
                return;
            }

            var materialToPacket = new List<ExtraEvolutionMaterialAssetModel>();
            var requiredsToPacket = new List<ExtraEvolutionRequiredAssetModel>();


            foreach (var material in extraEvolution.Materials)
            {
                var itemToRemove = client.Tamer.Inventory.FindItemById(material.ItemId);
                if (itemToRemove != null)
                {
                    materialToPacket.Add(material);
                    client.Tamer.Inventory.RemoveOrReduceItemWithoutSlot(new ItemModel(material.ItemId, material.Amount));
                 
                    break;
                }
            }

            foreach (var material in extraEvolution.Requireds)
            {
                var itemToRemove = client.Tamer.Inventory.FindItemById(material.ItemId);

                if (itemToRemove != null)
                {
                    requiredsToPacket.Add(material);
                    client.Tamer.Inventory.RemoveOrReduceItemWithoutSlot(new ItemModel(material.ItemId, material.Amount));

                    if(extraEvolution.Requireds.Count <=3)
                    {
                        break;
                    }    
                }
            }


            byte i = 0;
            while (i < client.Tamer.DigimonSlots)
            {
                if (client.Tamer.Digimons.FirstOrDefault(x => x.Slot == i) == null)
                    break;

                i++;
            }
            
            var newDigimon = DigimonModel.Create(
                digiName,
                targetType,
                targetType,
                DigimonHatchGradeEnum.Default,
                UtilitiesFunctions.GetLevelSize(3),
                i
            );

            newDigimon.NewLocation(
                client.Tamer.Location.MapId,
                client.Tamer.Location.X,
                client.Tamer.Location.Y
            );

            newDigimon.SetBaseInfo(
                _statusManager.GetDigimonBaseInfo(
                    newDigimon.BaseType
                )
            );

            newDigimon.SetBaseStatus(
                _statusManager.GetDigimonBaseStatus(
                    newDigimon.BaseType,
                    newDigimon.Level,
                    newDigimon.Size
                )
            );

            newDigimon.AddEvolutions(
                _assets.EvolutionInfo.First(x => x.Type == newDigimon.BaseType)
            );

            if (newDigimon.BaseInfo == null || newDigimon.BaseStatus == null || !newDigimon.Evolutions.Any())
            {
                _logger.Warning($"Unknown digimon info for {newDigimon.BaseType}.");
                client.Send(new SystemMessagePacket($"Unknown digimon info for {newDigimon.BaseType}."));
                return;
            }

            newDigimon.SetTamer(client.Tamer);

            client.Tamer.AddDigimon(newDigimon);


         
            if (client.Tamer.Incubator.PerfectSize(newDigimon.HatchGrade, newDigimon.Size))
            {
                _mapServer.BroadcastGlobal(new NeonMessagePacket(NeonMessageTypeEnum.Scale, client.Tamer.Name, newDigimon.BaseType, newDigimon.Size).Serialize());
                _dungeonServer.BroadcastGlobal(new NeonMessagePacket(NeonMessageTypeEnum.Scale, client.Tamer.Name, newDigimon.BaseType, newDigimon.Size).Serialize());
            }


            var digimonInfo = await _sender.Send(new CreateDigimonCommand(newDigimon));

            client.Send(new HatchFinishPacket(newDigimon, (ushort)(client.Partner.GeneralHandler + 1000), client.Tamer.Digimons.FindIndex(x => x == newDigimon)));

            client.Send(new HatchSpiritEvolutionPacket(targetType, (int)client.Tamer.Inventory.Bits, materialToPacket, requiredsToPacket));
            client.Send(new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory));

            await _sender.Send(new UpdateItemsCommand(client.Tamer.Inventory));

            if (digimonInfo != null)
            {
                newDigimon.SetId(digimonInfo.Id);
                var slot = -1;

                foreach (var digimon in newDigimon.Evolutions)
                {
                    slot++;

                    var evolution = digimonInfo.Evolutions[slot];

                    if (evolution != null)
                    {
                        digimon.SetId(evolution.Id);

                        var skillSlot = -1;

                        foreach (var skill in digimon.Skills)
                        {
                            skillSlot++;

                            var dtoSkill = evolution.Skills[skillSlot];

                            skill.SetId(dtoSkill.Id);
                        }
                    }
                }
            }

            _logger.Verbose($"Character {client.TamerId} hatched spirit {newDigimon.Id}({newDigimon.BaseType}) with grade {newDigimon.HatchGrade} and size {newDigimon.Size}.");
        }
    }
}