using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Packets.GameServer;
using Serilog;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class ChannelsPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.Channels;

        private readonly ILogger _logger;

        public ChannelsPacketProcessor(
            ILogger logger)
        {
            _logger = logger;
        }

        public Task Process(GameClient client, byte[] packetData)
        {
            _logger.Debug($"Getting available channels...");
            //var channels = await _sender.Send(new ChannelsByMapIdQuery(client.Tamer.Location.MapId));
            var channels = new Dictionary<byte, byte>
            {
                { 0, 30 }
            };

            if (!client.DungeonMap)
            {
                _logger.Debug($"Sending available channels packet...");
                client.Send(new AvailableChannelsPacket(channels));
            }

            return Task.CompletedTask;
        }
    }
}