using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.GameHost;
using Serilog;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class PartnerRideModeStopPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.PartnerRideModeStop;

        private readonly MapServer _mapServer;
        private readonly ILogger _logger;

        public PartnerRideModeStopPacketProcessor(
            MapServer mapServer,
            ILogger logger)
        {
            _mapServer = mapServer;
            _logger = logger;
        }

        public Task Process(GameClient client, byte[] packetData)
        {
            client.Tamer.StopRideMode();

            _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                new UpdateMovementSpeedPacket(client.Tamer).Serialize());

            _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                new RideModeStopPacket(client.Tamer.GeneralHandler, client.Partner.GeneralHandler).Serialize());

            _logger.Verbose($"Character {client.TamerId} ended riding mode with " +
                $"{client.Partner.Id} ({client.Partner.CurrentType}).");

            return Task.CompletedTask;
        }
    }
}