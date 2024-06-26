using DigitalWorldOnline.Application.Separar.Commands.Update;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.GameHost;

using MediatR;
using Serilog;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class GuildNoticeUpdatePacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.GuildNoticeUpdate;

        private readonly MapServer _mapServer;
        private readonly DungeonsServer _dungeonServer;
        private readonly ISender _sender;
        private readonly ILogger _logger;

        public GuildNoticeUpdatePacketProcessor(
            MapServer mapServer,
            ISender sender,
            ILogger logger,
            DungeonsServer dungeonServer)
        {
            _mapServer = mapServer;
            _sender = sender;
            _logger = logger;
            _dungeonServer = dungeonServer;
        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            var packet = new GamePacketReader(packetData);

            var newMessage = packet.ReadString();

            if (string.IsNullOrEmpty(newMessage))
                return;

            if (client.Tamer.Guild != null)
            {
                client.Tamer.Guild.SetNotice(newMessage);

                client.Tamer.Guild.Members
                    .ToList()
                    .ForEach(guildMember =>
                    {
                        _logger.Debug($"Sending guild notice update packet for character {guildMember.CharacterId}...");
                        _mapServer.BroadcastForUniqueTamer(guildMember.CharacterId, new GuildNoticeUpdatePacket(newMessage).Serialize());
                        _dungeonServer.BroadcastForUniqueTamer(guildMember.CharacterId, new GuildNoticeUpdatePacket(newMessage).Serialize());
                    });

                await _sender.Send(new UpdateGuildNoticeCommand(client.Tamer.Guild.Id, newMessage));
            }
        }
    }
}