using DigitalWorldOnline.Application.Separar.Commands.Create;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Models.Chat;
using DigitalWorldOnline.Commons.Packets.Chat;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.GameHost;
using MediatR;
using Serilog;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class GuildMessagePacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.GuildMessage;

        private readonly MapServer _mapServer;
        private readonly DungeonsServer _dungeonServer;
        private readonly ILogger _logger;
        private readonly ISender _sender;

        public GuildMessagePacketProcessor(
            MapServer mapServer,
            ILogger logger,
            ISender sender,
            DungeonsServer dungeonServer)
        {
            _mapServer = mapServer;
            _logger = logger;
            _sender = sender;
            _dungeonServer = dungeonServer;
        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            var packet = new GamePacketReader(packetData);
            var message = packet.ReadString();

            if (client.Tamer.Guild != null)
            {
                _mapServer.BroadcastForTargetTamers(client.Tamer.Guild.GetGuildMembersIdList(), new GuildMessagePacket(client.Tamer.Name, message).Serialize());
                _dungeonServer.BroadcastForTargetTamers(client.Tamer.Guild.GetGuildMembersIdList(), new GuildMessagePacket(client.Tamer.Name, message).Serialize());

                _logger.Verbose($"Character {client.TamerId} sent chat to guild {client.Tamer.Guild.Id} with message {message}.");

                await _sender.Send(new CreateChatMessageCommand(ChatMessageModel.Create(client.TamerId, message)));
            }
            else
            {
                client.Send(new SystemMessagePacket($"You need to be in a guild to send guild messages."));
                _logger.Warning($"Character {client.TamerId} sent guild message but was not in a guild.");
            }
        }
    }
}