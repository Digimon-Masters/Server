using DigitalWorldOnline.Application.Separar.Queries;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.Character;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.GameHost;
using MediatR;
using Serilog;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class GuildInviteSendPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.GuildInvite;

        private readonly MapServer _mapServer;
        private readonly ILogger _logger;
        private readonly ISender _sender;

        public GuildInviteSendPacketProcessor(
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
            if (client.Tamer.Guild != null)
            {
                var packet = new GamePacketReader(packetData);

                var targetName = packet.ReadString();

                _logger.Debug($"Searching character by name {targetName}...");
                var targetCharacter = await _sender.Send(new CharacterByNameQuery(targetName));

                if (targetCharacter == null || targetCharacter.State != CharacterStateEnum.Ready)
                {
                    _logger.Verbose($"Character {client.TamerId} sent guild invite to {targetCharacter?.Id} {targetName} which was not connected.");

                    _logger.Debug($"Sending guild invite fail packet for character id {client.TamerId}...");
                    client.Send(new GuildInviteFailPacket(GuildInviteFailEnum.TargetNotConnected, targetName));
                }
                else
                {
                    _logger.Debug($"Searching guild by character id {targetCharacter.Id}...");
                    var targetGuild = await _sender.Send(new GuildByCharacterIdQuery(targetCharacter.Id));
                    if (targetGuild != null)
                    {
                        _logger.Verbose($"Character {client.TamerId} sent guild invite to {targetCharacter.Id} which was in another guild.");

                        _logger.Debug($"Sending guild invite fail packet for character id {client.TamerId}...");
                        client.Send(new GuildInviteFailPacket(GuildInviteFailEnum.TargetInAnotherGuild, targetName));
                    }
                    else
                    {
                        _logger.Verbose($"Character {client.TamerId} sent guild invite to {targetCharacter.Id}.");

                        _logger.Debug($"Sending guild invite success packet for character id {targetCharacter.Id}...");
                        _mapServer.BroadcastForUniqueTamer(targetCharacter.Id, new GuildInviteSuccessPacket(client.Tamer).Serialize());
                    }
                }
            }
        }
    }
}