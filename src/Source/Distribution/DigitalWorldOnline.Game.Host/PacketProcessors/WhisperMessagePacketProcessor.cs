using DigitalWorldOnline.Application.Separar.Commands.Create;
using DigitalWorldOnline.Application.Separar.Queries;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.Character;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Models.Chat;
using DigitalWorldOnline.Commons.Packets.Chat;
using DigitalWorldOnline.GameHost;
using MediatR;
using Serilog;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class WhisperMessagePacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.WhisperMessage;

        private readonly MapServer _mapServer;
        private readonly ILogger _logger;
        private readonly ISender _sender;

        public WhisperMessagePacketProcessor(
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

            var receiverName = packet.ReadString();
            var unk = packet.ReadByte();
            var message = packet.ReadString();

            var targetCharacter = await _sender.Send(new CharacterByNameQuery(receiverName));
            if (targetCharacter == null)
            {
                client.Send(new ChatMessagePacket(message, ChatTypeEnum.Whisper, WhisperResultEnum.NotFound, client.Tamer.Name, receiverName));
                _logger.Verbose($"Character {client.TamerId} sent whisper to {receiverName} which does not exist.");
            }
            else
            {
                var targetClient = _mapServer.FindClientByTamerId(targetCharacter.Id);
                if (targetClient != null)
                {
                    if (targetClient.Tamer.State != CharacterStateEnum.Ready)
                    {
                        client.Send(new ChatMessagePacket(message, ChatTypeEnum.Whisper, WhisperResultEnum.OnLoadScreen, client.Tamer.Name, receiverName));
                        _logger.Verbose($"Character {client.TamerId} sent whisper to {targetCharacter.Id} which was on load screen.");
                    }
                    else
                    {
                        client.Send(new ChatMessagePacket(message, ChatTypeEnum.Whisper, WhisperResultEnum.Success, client.Tamer.Name, receiverName));
                        targetClient.Send(new ChatMessagePacket(message, ChatTypeEnum.Whisper, WhisperResultEnum.Success, client.Tamer.Name, receiverName));
                        _logger.Verbose($"Character {client.TamerId} sent whisper to {targetCharacter.Id} with message {message}.");

                        await _sender.Send(new CreateChatMessageCommand(ChatMessageModel.Create(client.TamerId, message)));
                    }
                }
                else
                {
                    client.Send(new ChatMessagePacket(message, ChatTypeEnum.Whisper, WhisperResultEnum.Disconnected, client.Tamer.Name, receiverName));
                    _logger.Verbose($"Character {client.TamerId} sent whisper to {targetCharacter.Id} which was disconnected.");
                }
            }
        }
    }
}