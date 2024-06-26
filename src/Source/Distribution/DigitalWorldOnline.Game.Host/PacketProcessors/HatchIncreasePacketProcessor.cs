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
    public class HatchIncreasePacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.HatchIncrease;

        private readonly MapServer _mapServer;
        private readonly DungeonsServer _dungeonServer;
        private readonly AssetsLoader _assets;
        private readonly ConfigsLoader _configs;
        private readonly ILogger _logger;
        private readonly ISender _sender;

        public HatchIncreasePacketProcessor(
            MapServer mapServer,
            DungeonsServer dungeonsServer,
            AssetsLoader assets,
            ConfigsLoader configs,
            ILogger logger,
            ISender sender
        )
        {
            _mapServer = mapServer;
            _dungeonServer = dungeonsServer;
            _assets = assets;
            _configs = configs;
            _logger = logger;
            _sender = sender;
        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            var packet = new GamePacketReader(packetData);

            var vipEnabled = packet.ReadByte();
            var npcId = packet.ReadInt();
            var dataTier = packet.ReadByte();

            var targetItem = client.Tamer.Incubator.EggId;
            var hatchInfo = _assets.Hatchs.FirstOrDefault(x => x.ItemId == targetItem);
            if (hatchInfo == null)
            {
                _logger.Warning($"Unknown hatch info for egg {targetItem}.");
                client.Send(new SystemMessagePacket($"Unknown hatch info for egg {targetItem}."));
                return;
            }

            var hatchConfig = _configs.Hatchs.FirstOrDefault(x => x.Type.GetHashCode() == client.Tamer.Incubator.HatchLevel + 1);
            if (hatchConfig == null)
            {
                client.Send(new HatchIncreaseFailedPacket(client.Tamer.GeneralHandler, HatchIncreaseResultEnum.Failled));
                _logger.Error($"Invalid hatch config for level {client.Tamer.Incubator.HatchLevel + 1}.");
                client.Send(new SystemMessagePacket($"Invalid hatch config for level {client.Tamer.Incubator.HatchLevel + 1}."));
                return;
            }

            if (dataTier == 0)
            {
                var success = client.Tamer.Inventory.RemoveOrReduceItemsBySection(hatchInfo.LowClassDataSection, hatchInfo.LowClassDataAmount);
                if (!success)
                {
                    client.Send(new HatchIncreaseFailedPacket(client.Tamer.GeneralHandler, HatchIncreaseResultEnum.Failled));
                    _logger.Error($"Invalid low class data amount for egg {targetItem} and section {hatchInfo.LowClassDataSection}.");
                    client.Send(new SystemMessagePacket($"Invalid low class data amount for egg {targetItem} and section {hatchInfo.LowClassDataSection}."));
                    return;
                }
            }
            else
            {
                var success = client.Tamer.Inventory.RemoveOrReduceItemsBySection(hatchInfo.MidClassDataSection, hatchInfo.MidClassDataAmount);
                if (!success)
                {
                    client.Send(new HatchIncreaseFailedPacket(client.Tamer.GeneralHandler, HatchIncreaseResultEnum.Failled));
                    _logger.Error($"Invalid mid class data amount for egg {targetItem} and section {hatchInfo.MidClassDataSection}.");
                    client.Send(new SystemMessagePacket($"Invalid mid class data amount for egg {targetItem} and section {hatchInfo.MidClassDataSection}."));
                    return;
                }
            }

            if(hatchConfig.SuccessChance >= UtilitiesFunctions.RandomDouble())
            {
                client.Tamer.Incubator.IncreaseLevel();

                if (client.DungeonMap)
                {
                    _dungeonServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                        new HatchIncreaseSucceedPacket(
                            client.Tamer.GeneralHandler,
                            client.Tamer.Incubator.HatchLevel
                        ).Serialize()
                    );
                }
                else
                {
                    _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                       new HatchIncreaseSucceedPacket(
                           client.Tamer.GeneralHandler,
                           client.Tamer.Incubator.HatchLevel
                       ).Serialize()
                   );
                }
                _logger.Verbose($"Character {client.TamerId} succeeded to increase egg {targetItem} to level {client.Tamer.Incubator.HatchLevel} " +
                    $"with data section {hatchInfo.LowClassDataSection} x{hatchInfo.LowClassDataAmount}.");
            }
            else
            {
                if (hatchConfig.BreakChance >= UtilitiesFunctions.RandomDouble())
                {
                    if (client.Tamer.Incubator.BackupDiskId > 0)
                    {
                        if (client.DungeonMap)
                        {
                            _dungeonServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                            new HatchIncreaseFailedPacket(
                                client.Tamer.GeneralHandler,
                                HatchIncreaseResultEnum.Backuped
                            ).Serialize()
                        );
                        }
                        else
                        {
                            _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                           new HatchIncreaseFailedPacket(
                               client.Tamer.GeneralHandler,
                               HatchIncreaseResultEnum.Backuped
                           ).Serialize()
                       );
                        }
                        _logger.Verbose($"Character {client.TamerId} failed to increase egg {targetItem} to level {client.Tamer.Incubator.HatchLevel + 1} " +
                            $"with data section {hatchInfo.MidClassDataSection} x{hatchInfo.MidClassDataAmount} and egg was saved by {client.Tamer.Incubator.BackupDiskId}.");
                    }
                    else
                    {
                        if (client.DungeonMap)
                        {
                            _dungeonServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                            new HatchIncreaseFailedPacket(
                                client.Tamer.GeneralHandler,
                                HatchIncreaseResultEnum.Broken
                            ).Serialize()
                        );
                        }
                        else
                        {
                            _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                            new HatchIncreaseFailedPacket(
                                client.Tamer.GeneralHandler,
                                HatchIncreaseResultEnum.Broken
                            ).Serialize()
                        );

                        }
                        _logger.Verbose($"Character {client.TamerId} failed to increase egg {targetItem} to level {client.Tamer.Incubator.HatchLevel + 1} " +
                            $"with data section {hatchInfo.MidClassDataSection} x{hatchInfo.MidClassDataAmount} and egg has broken.");

                        client.Tamer.Incubator.RemoveEgg();
                    }
                }
                else
                {
                    if (client.DungeonMap)
                    {
                        _dungeonServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                        new HatchIncreaseFailedPacket(
                            client.Tamer.GeneralHandler,
                            HatchIncreaseResultEnum.Failled
                        ).Serialize()
                    );
                    }
                    else
                    {
                        _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                        new HatchIncreaseFailedPacket(
                            client.Tamer.GeneralHandler,
                            HatchIncreaseResultEnum.Failled
                        ).Serialize()
                    );

                    }
                    _logger.Verbose($"Character {client.TamerId} failed to increase egg {targetItem} to level {client.Tamer.Incubator.HatchLevel + 1} " +
                        $"with data section {hatchInfo.MidClassDataSection} x{hatchInfo.MidClassDataAmount}.");
                }
            }

            client.Tamer.Incubator.RemoveBackupDisk();

            await _sender.Send(new UpdateItemsCommand(client.Tamer.Inventory));
            await _sender.Send(new UpdateIncubatorCommand(client.Tamer.Incubator));
        }
    }
}