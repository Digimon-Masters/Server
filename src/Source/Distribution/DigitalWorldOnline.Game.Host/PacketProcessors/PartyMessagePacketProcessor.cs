using DigitalWorldOnline.Application.Separar.Commands.Create;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Models.Chat;
using DigitalWorldOnline.Commons.Packets.Chat;
using DigitalWorldOnline.Game.Managers;
using DigitalWorldOnline.GameHost;
using MediatR;
using Serilog;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class PartyMessagePacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.PartyMessage;

        private readonly PartyManager _partyManager;
        private readonly MapServer _mapServer;
        private readonly DungeonsServer _dungeonServer;
        private readonly ILogger _logger;
        private readonly ISender _sender;

        public PartyMessagePacketProcessor(
            PartyManager partyManager,
            MapServer mapServer,
            DungeonsServer dungeonsServer,
            ILogger logger,
            ISender sender)
        {
            _partyManager = partyManager;
            _mapServer = mapServer;
            _dungeonServer = dungeonsServer;
            _logger = logger;
            _logger = logger;
            _sender = sender;
        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            var packet = new GamePacketReader(packetData);
            var message = packet.ReadString();

            var party = _partyManager.FindParty(client.TamerId);
            if (party == null)
            {
                client.Send(new SystemMessagePacket($"You need to be in a party to send party messages."));
                _logger.Warning($"Character {client.TamerId} sent party message but was not in a party.");
                return;
            }

            _mapServer.BroadcastForTargetTamers(party.GetMembersIdList(),
                new PartyMessagePacket(client.Tamer.Name, message).Serialize());

            _dungeonServer.BroadcastForTargetTamers(party.GetMembersIdList(),
              new PartyMessagePacket(client.Tamer.Name, message).Serialize());

            _logger.Verbose($"Character {client.TamerId} sent chat to party {party.Id} with message {message}.");

            await _sender.Send(new CreateChatMessageCommand(ChatMessageModel.Create(client.TamerId, message)));
        }
    }
}