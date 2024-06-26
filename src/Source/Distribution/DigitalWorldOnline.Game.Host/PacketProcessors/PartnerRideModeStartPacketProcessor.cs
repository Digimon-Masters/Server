using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.GameHost;
using Serilog;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class PartnerRideModeStartPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.PartnerRideModeStart;

        private readonly MapServer _mapServer;
        private readonly ILogger _logger;

        public PartnerRideModeStartPacketProcessor(
            MapServer mapServer,
            ILogger logger)
        {
            _mapServer = mapServer;
            _logger = logger;
        }

        public Task Process(GameClient client, byte[] packetData)
        {
            client.Tamer.StartRideMode();

            _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                new UpdateMovementSpeedPacket(client.Tamer).Serialize());

            _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                new RideModeStartPacket(client.Tamer.GeneralHandler, client.Partner.GeneralHandler).Serialize());

            _logger.Verbose($"Character {client.TamerId} started riding mode with " +
                $"{client.Partner.Id} ({client.Partner.CurrentType}).");

            return Task.CompletedTask;
        }
    }
}