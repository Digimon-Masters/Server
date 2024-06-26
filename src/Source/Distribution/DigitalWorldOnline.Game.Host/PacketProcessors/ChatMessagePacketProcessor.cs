using DigitalWorldOnline.Application.Separar.Commands.Create;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.Account;
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
    public class ChatMessagePacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.ChatMessage;

        private readonly GameMasterCommandsProcessor _gmCommands;
        private readonly MapServer _mapServer;
        private readonly DungeonsServer _dungeonServer;
        private readonly ILogger _logger;
        private readonly ISender _sender;

        public ChatMessagePacketProcessor(
            GameMasterCommandsProcessor gmCommands,
            MapServer mapServer,
            ILogger logger,
            ISender sender,DungeonsServer dungeonServer)
        {
            _gmCommands = gmCommands;
            _mapServer = mapServer;
            _dungeonServer = dungeonServer;
            _logger = logger;
            _sender = sender;
        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            var packet = new GamePacketReader(packetData);

            string message = packet.ReadString();

            switch (client.AccessLevel)
            {
                case AccountAccessLevelEnum.Default:
                    {
                        _logger.Debug($"Tamer says \"{message}\" to NormalChat.");
                        _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId, new ChatMessagePacket(message, ChatTypeEnum.Normal, client.Tamer.GeneralHandler).Serialize());
                        await _sender.Send(new CreateChatMessageCommand(ChatMessageModel.Create(client.TamerId, message)));
                    }
                    break;

                case AccountAccessLevelEnum.Blocked:
                    break;

                //TODO: split
                case AccountAccessLevelEnum.Moderator:
                case AccountAccessLevelEnum.GameMasterOne:
                case AccountAccessLevelEnum.GameMasterTwo:
                case AccountAccessLevelEnum.GameMasterThree:
                case AccountAccessLevelEnum.Administrator:
                    {
                        if (message.StartsWith("!"))
                        {
                            _logger.Debug($"Tamer trys to execute \"{message}\".");
                            await _gmCommands.ExecuteCommand(client, message.TrimStart('!'));
                        }
                        else
                        {
                            _logger.Debug($"Tamer says \"{message}\" to NormalChat.");

                            if (client.DungeonMap)
                            {
                                _dungeonServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                                    new ChatMessagePacket(message, ChatTypeEnum.Normal, client.Tamer.GeneralHandler).Serialize());
                            }
                            else
                            {
                                _mapServer.BroadcastForTamerViewsAndSelf(client.TamerId,
                                    new ChatMessagePacket(message, ChatTypeEnum.Normal, client.Tamer.GeneralHandler).Serialize());

                            }

                            await _sender.Send(new CreateChatMessageCommand(ChatMessageModel.Create(client.TamerId, message)));
                        }
                    }
                    break;

                default:
                    _logger.Warning($"Invalid Access Level for account {client.AccountId}.");
                    break;
            }
        }
    }
}