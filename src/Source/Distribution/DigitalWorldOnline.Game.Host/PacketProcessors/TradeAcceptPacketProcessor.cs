using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums;
using DigitalWorldOnline.Commons.Enums.Character;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.GameHost;
using Serilog;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class TradeAcceptPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.TradeRequestAccept;

        private readonly MapServer _mapServer;
        private readonly ILogger _logger;


        public TradeAcceptPacketProcessor(
            MapServer mapServer,
            ILogger logger)
        {
            _mapServer = mapServer;
            _logger = logger;

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
                    targetClient.Tamer.SetTrade(true, client.Tamer.GeneralHandler);

                    client.Tamer.SetTrade(true, targetClient.Tamer.GeneralHandler);
                    targetClient.Send(new TradeAcceptPacket(client.Tamer.GeneralHandler));
                    client.Send(new TradeAcceptPacket(TargetHandle));
                    _logger.Verbose($"Character {client.TamerId} sent trade request to {targetClient.TamerId} and the tamer accept trade"); ;

                }
            }

        }
    }
}
