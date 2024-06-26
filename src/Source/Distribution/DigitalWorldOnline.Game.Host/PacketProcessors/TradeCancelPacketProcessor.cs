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
    public class TradeCancelPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.TradeRefuse;

        private readonly MapServer _mapServer;
        private readonly ILogger _logger;


        public TradeCancelPacketProcessor(
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

            client.Tamer.ClearTrade();
            targetClient.Tamer.ClearTrade();

            targetClient.Send(new TradeCancelPacket(client.Tamer.GeneralHandler));
            client.Send(new TradeCancelPacket(TargetHandle));
            _logger.Verbose($"Character {client.TamerId} and  {targetClient.TamerId} cancel trade or refuse"); ;

        }

    }
}

