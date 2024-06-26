using DigitalWorldOnline.Application;
using DigitalWorldOnline.Application.Separar.Commands.Update;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Packets.Chat;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.Commons.Utils;
using DigitalWorldOnline.GameHost;
using MediatR;
using Serilog;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class PartnerDigiclonePacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.PartnerDigiclone;

        private readonly MapServer _mapServer;
        private readonly AssetsLoader _assets;
        private readonly DungeonsServer _dungeonServer;
        private readonly ConfigsLoader _configs;
        private readonly ISender _sender;
        private readonly ILogger _logger;

        public PartnerDigiclonePacketProcessor(
            MapServer mapServer,
            AssetsLoader assets,
            ConfigsLoader configs,
            ISender sender,
            ILogger logger,
            DungeonsServer dungeonsServer)
        {
            _mapServer = mapServer;
            _assets = assets;
            _configs = configs;
            _sender = sender;
            _logger = logger;
            _dungeonServer = dungeonsServer;
        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            var packet = new GamePacketReader(packetData);

            var cloneType = (DigicloneTypeEnum)packet.ReadInt();
            var digicloneSlot = packet.ReadInt();
            var backupSlot = packet.ReadInt();

            var digicloneItem = client.Tamer.Inventory.FindItemBySlot(digicloneSlot);
            if (digicloneItem == null)
            {
                _logger.Warning($"Invalid clone item at slot {digicloneSlot} for tamer {client.TamerId}.");
                client.Send(new SystemMessagePacket($"Invalid clone item at slot {digicloneSlot}."));
                return;
            }

            var currentCloneLevel = client.Partner.Digiclone.GetCurrentLevel(cloneType);

            var cloneConfig = _configs.Clones.FirstOrDefault(x => x.Type == cloneType && x.Level == currentCloneLevel + 1);
            if (cloneConfig == null)
            {
                _logger.Warning($"Invalid clone config with type {cloneType} and level {currentCloneLevel}.");
                client.Send(new SystemMessagePacket($"Invalid clone config."));
                return;
            }

            var clonePriceAsset = _assets.Clones.FirstOrDefault(x => x.ItemSection == digicloneItem.ItemInfo.Section);
            if (clonePriceAsset == null)
            {
                _logger.Warning($"Invalid clone price assets with item section {digicloneItem.ItemInfo.Section}.");
                client.Send(new SystemMessagePacket($"Invalid clone assets."));
                return;
            }

            var cloneAsset = _assets.CloneValues.FirstOrDefault(x => x.Type == cloneType && currentCloneLevel + 1 >= x.MinLevel && currentCloneLevel + 1 <= x.MaxLevel);
            if (cloneAsset == null)
            {
                _logger.Warning($"Invalid clone assets with type {cloneType} and level {currentCloneLevel}.");
                client.Send(new SystemMessagePacket($"Invalid clone assets."));
                return;
            }

            var cloneResult = DigicloneResultEnum.Fail;
            short value = 0;

            if (clonePriceAsset.Reinforced)
            {
                if (cloneConfig.SuccessChance >= UtilitiesFunctions.RandomDouble())
                {
                    cloneResult = DigicloneResultEnum.Success;
                    value = (short)cloneAsset.MaxValue;
                }
            }
            else if (clonePriceAsset.Mega)
            {
                cloneResult = DigicloneResultEnum.Success;
                value = UtilitiesFunctions.RandomShort((short)cloneAsset.MinValue, (short)(cloneAsset.MaxValue));
            }
            else if (clonePriceAsset.MegaReinforced)
            {
                cloneResult = DigicloneResultEnum.Success;
                value = (short)cloneAsset.MaxValue;
            }
            else if (clonePriceAsset.Low)
            {
                cloneResult = DigicloneResultEnum.Success;
                value = (short)cloneAsset.MinValue;
            }
            else
            {
                if (cloneConfig.SuccessChance >= UtilitiesFunctions.RandomDouble())
                {
                    cloneResult = DigicloneResultEnum.Success;
                    value = UtilitiesFunctions.RandomShort((short)cloneAsset.MinValue, (short)(cloneAsset.MaxValue));
                }
            }

            var backupItem = client.Tamer.Inventory.FindItemBySlot(backupSlot);

            if (cloneResult == DigicloneResultEnum.Success)
            {
                _logger.Verbose($"Character {client.TamerId} increased {client.Partner.Id} {cloneType} clon level to " +
                    $"{currentCloneLevel + 1} with value {value} using {digicloneItem.ItemId} {backupItem?.ItemId}.");

                client.Partner.Digiclone.IncreaseCloneLevel(cloneType, value);

                if (client.Partner.Digiclone.MaxCloneLevel)
                {
                    _dungeonServer.BroadcastGlobal(
                        new NeonMessagePacket(
                            NeonMessageTypeEnum.Digimon,
                            client.Tamer.Name,
                            client.Partner.CurrentType,
                            client.Partner.Digiclone.CloneLevel - 1
                        )
                    .Serialize());

                    _mapServer.BroadcastGlobal(
                        new NeonMessagePacket(
                            NeonMessageTypeEnum.Digimon,
                            client.Tamer.Name,
                            client.Partner.CurrentType,
                            client.Partner.Digiclone.CloneLevel - 1
                        )
                    .Serialize());
                }
            }
            else
            {
                if (cloneConfig.CanBreak)
                {
                    if (cloneConfig.BreakChance >= UtilitiesFunctions.RandomDouble())
                    {
                        if (backupItem == null)
                        {
                            _logger.Verbose($"Character {client.TamerId} broken {client.Partner.Id} {cloneType} clon level to " +
                                $"{currentCloneLevel - 1} using {digicloneItem.ItemId} without backup.");

                            cloneResult = DigicloneResultEnum.Break;
                            client.Partner.Digiclone.Break(cloneType);
                        }
                        else
                        {
                            _logger.Verbose($"Character {client.TamerId} failed to increase {client.Partner.Id} {cloneType} clon level to " +
                                $"{currentCloneLevel + 1} using {digicloneItem.ItemId} with backup {backupItem?.ItemId}.");

                            cloneResult = DigicloneResultEnum.Backup;
                        }
                    }
                }
                else
                {
                    _logger.Verbose($"Character {client.TamerId} failed to increase {client.Partner.Id} {cloneType} clon level to " +
                                $"{currentCloneLevel + 1} using {digicloneItem.ItemId} {backupItem?.ItemId}.");

                    cloneResult = DigicloneResultEnum.Fail;
                }
            }

            client.Send(new DigicloneResultPacket(cloneResult, client.Partner.Digiclone));
            client.Send(new UpdateStatusPacket(client.Tamer));

            client.Tamer.Inventory.RemoveBits(clonePriceAsset.Bits);
            client.Tamer.Inventory.RemoveOrReduceItem(digicloneItem, 1, digicloneSlot);
            client.Tamer.Inventory.RemoveOrReduceItem(backupItem, 1, backupSlot);

            await _sender.Send(new UpdateDigicloneCommand(client.Partner.Digiclone));
            await _sender.Send(new UpdateItemListBitsCommand(client.Tamer.Inventory));
            await _sender.Send(new UpdateItemsCommand(client.Tamer.Inventory));
        }
    }
}