using DigitalWorldOnline.Application.Separar.Queries;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.GameHost;
using MediatR;
using Serilog;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class GuildInviteDenyPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.GuildInviteDeny;

        private readonly MapServer _mapServer;
        private readonly ILogger _logger;
        private readonly ISender _sender;

        public GuildInviteDenyPacketProcessor(
            MapServer mapServer,
            ILogger logger,
            ISender sender)
        {
            _mapServer = mapServer;
            _logger = logger;
            _sender = sender;
        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            var packet = new GamePacketReader(packetData);

            var guildId = packet.ReadInt();
            var senderName = packet.ReadString();

            _logger.Debug($"Searching character by name {senderName}...");
            var targetCharacter = await _sender.Send(new CharacterByNameQuery(senderName));
            if (targetCharacter != null)
            {
                _logger.Verbose($"Character {targetCharacter.Id} denied guild invitation from {client.Tamer.Id}.");

                _logger.Debug($"Sending guild invite deny packet for character id {targetCharacter.Id}...");
                _mapServer.BroadcastForUniqueTamer(targetCharacter.Id, new GuildInviteDenyPacket(client.Tamer.Name).Serialize());
            }
            else
                _logger.Warning($"Character not found with name {senderName}.");
        }
    }
}