using DigitalWorldOnline.Application.Separar.Queries;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums;
using DigitalWorldOnline.Commons.Enums.Character;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.Game.Managers;
using DigitalWorldOnline.GameHost;
using MediatR;
using Serilog;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class TradePacketRequestSendProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.TradeRequestSend;

        private readonly MapServer _mapServer;
        private readonly ILogger _logger;
        private readonly ISender _sender;

        public TradePacketRequestSendProcessor(
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
            var TargetHandle = packet.ReadInt();


            var targetClient = _mapServer.FindClientByTamerHandle(TargetHandle);

            if (targetClient != null)
            {
                if (targetClient.Loading || targetClient.Tamer.State != CharacterStateEnum.Ready || targetClient.Tamer.CurrentCondition == ConditionEnum.Away || targetClient.Tamer.TradeCondition)
                {
                    client.Send(new TradeRequestErrorPacket(TradeRequestErrorEnum.othertransact));
                    _logger.Verbose($"Character {client.TamerId} sent trade request to {targetClient.TamerId} and the tamer was already in another transaction.");
                }
                else
                {
                    targetClient.Send(new TradeRequestSucessPacket(client.Tamer.GeneralHandler));
                    _logger.Verbose($"Character {client.TamerId} sent trade request to {targetClient.TamerId}.");

                }
            }
            else
            {
                targetClient.Send(new TradeRequestErrorPacket(TradeRequestErrorEnum.othertransact));
                _logger.Verbose($"Character {client.TamerId} sent trade request to {targetClient.TamerId} and the tamer was impossible to trade.");
            }
        }
    }
}
