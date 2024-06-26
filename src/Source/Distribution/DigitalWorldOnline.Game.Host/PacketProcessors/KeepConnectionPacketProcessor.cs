using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.GameHost;
using Serilog;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class KeepConnectionPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.KeepConnection;

        private readonly MapServer _mapServer;
        private readonly ILogger _logger;

        public KeepConnectionPacketProcessor(
            MapServer mapServer,
            ILogger logger)
        {
            _mapServer = mapServer;
            _logger = logger;
        }

        public Task Process(GameClient client, byte[] packetData)
        {
            if (client.Tamer == null)
                return Task.CompletedTask;

            if ((DateTime.Now - client.Tamer.LastAfkNotification).TotalSeconds < 15)
            {
                client.Tamer.AddAfkNotifications(1);

                if (client.Tamer.SetAfk)
                {
                    client.Tamer.UpdateCurrentCondition(ConditionEnum.Away);
                    _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId, new SyncConditionPacket(client.Tamer.GeneralHandler, client.Tamer.CurrentCondition).Serialize());

                    _logger.Verbose($"Character {client.TamerId} went away from keyboard.");
                }
            }
            else
            {
                client.Tamer.ResetAfkNotifications();
            }

            return Task.CompletedTask;
        }
    }
}
