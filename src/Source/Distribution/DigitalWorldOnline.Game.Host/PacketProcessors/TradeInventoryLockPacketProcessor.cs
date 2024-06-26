using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.Character;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.GameHost;
using Serilog;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class TradeInventorylockPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.TradeInventorylock;

        private readonly MapServer _mapServer;
        private readonly ILogger _logger;


        public TradeInventorylockPacketProcessor(
            MapServer mapServer,
            ILogger logger)
        {
            _mapServer = mapServer;
            _logger = logger;

        }

        public async Task Process(GameClient client, byte[] packetData)
        {

            var targetClient = _mapServer.FindClientByTamerHandle(client.Tamer.TargetTradeGeneralHandle);

            targetClient.Send(new TradeInventorylockPacket(client.Tamer.GeneralHandler));
            client.Send(new TradeInventorylockPacket(client.Tamer.GeneralHandler));
            _logger.Verbose($"Character {client.TamerId} inventory lock "); ;

        }

    }
}

