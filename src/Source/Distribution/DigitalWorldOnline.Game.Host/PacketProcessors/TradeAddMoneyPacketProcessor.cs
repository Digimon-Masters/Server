using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.Character;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Models.Base;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.GameHost;
using Serilog;
using System.Net.Sockets;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class TradeAddMoneyacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.TradeAddMoney;

        private readonly MapServer _mapServer;
        private readonly ILogger _logger;


        public TradeAddMoneyacketProcessor(
            MapServer mapServer,
            ILogger logger)
        {
            _mapServer = mapServer;
            _logger = logger;

        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            var packet = new GamePacketReader(packetData);
            var TargetMoney = packet.ReadInt();

            var targetClient = _mapServer.FindClientByTamerHandle(client.Tamer.TargetTradeGeneralHandle);

            client.Tamer.TradeInventory.AddBits(TargetMoney);


            targetClient.Send(new TradeInventoryUnlockPacket(client.Tamer.TargetTradeGeneralHandle));
            client.Send(new TradeInventoryUnlockPacket(client.Tamer.TargetTradeGeneralHandle));


            client.Send(new TradeAddMoneyPacket(client.Tamer.GeneralHandler, TargetMoney));
            targetClient.Send(new TradeAddMoneyPacket(client.Tamer.GeneralHandler, TargetMoney));

            //_logger.Verbose($"Character {client.TamerId} and {targetClient.TamerId} inventory unlock "); ;

        }

    }
}
