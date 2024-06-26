using DigitalWorldOnline.Application;
using DigitalWorldOnline.Application.Separar.Commands.Delete;
using DigitalWorldOnline.Application.Separar.Commands.Update;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Models.Base;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.Commons.Packets.Items;
using MediatR;
using Serilog;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class SpiritCraftPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.SpiritCraft;

        private readonly ILogger _logger;
        private readonly ISender _sender;
        private readonly AssetsLoader _assets;
        public SpiritCraftPacketProcessor(
            ILogger logger,
            ISender sender,
            AssetsLoader assets
        )
        {
            _logger = logger;
            _sender = sender;
            _assets = assets;
        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            var packet = new GamePacketReader(packetData);
            var slot = packet.ReadByte();
            var validation = packet.ReadString();

            var digimonId = client.Tamer.Digimons.First(x => x.Slot == slot).Id;

            var targetType = client.Tamer.Digimons.First(x => x.Slot == slot).BaseType;

            var result = client.PartnerDeleteValidation(validation);

            var NpcId = 90005; // TODO: Ajustar leitura do packet.

            var extraEvolutionNpc = _assets.ExtraEvolutions.FirstOrDefault(x => x.NpcId == NpcId);

            if (extraEvolutionNpc == null)
                return;

            var extraEvolutionInfo = extraEvolutionNpc.ExtraEvolutionInformation.FirstOrDefault(
                x => x.ExtraEvolution.Any(extra => extra.Requireds.Any(required => required.ItemId == targetType))
            )?.ExtraEvolution;

            if (extraEvolutionInfo == null)
                return;

            var extraEvolution = extraEvolutionInfo.FirstOrDefault(x => x.Requireds.Any( x=> x.ItemId == targetType));
            
            if (extraEvolution == null)
                return;

            if (result > 0)
            {

                if (!client.Tamer.Inventory.RemoveBits(extraEvolution.Price))
                {
                    //client.Send(new SystemMessagePacket($"Insuficient bits for item craft NPC id {npcId} and id {sequencialId}."));
                    //_logger.Warning($"Insuficient bits for item craft NPC id {npcId} and id {sequencialId} for tamer {client.TamerId}.");
                    return;
                }

                var craftedItem = new ItemModel(extraEvolution.DigimonId, 1);
                craftedItem.SetItemInfo(_assets.ItemInfo.FirstOrDefault(x => x.ItemId == craftedItem.ItemId));

                var tempItem = (ItemModel)craftedItem.Clone();
                
                client.Tamer.Inventory.AddItem(tempItem);

                client.Tamer.RemoveDigimon(slot);


                client.Send(new SpiritCraftPacket(slot, (int)extraEvolution.Price,extraEvolution.DigimonId));
                client.Send(new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory));

                await _sender.Send(new DeleteDigimonCommand(digimonId));
                await _sender.Send(new UpdateItemsCommand(client.Tamer.Inventory));

                _logger.Verbose($"Character {client.TamerId} deleted partner {digimonId}.");
            }
            else
            {
                client.Send(new PartnerDeletePacket(result));
                _logger.Verbose($"Character {client.TamerId} failed to deleted partner {digimonId} with invalid account information.");
            }
        }
    }
}