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
    public class TraRemoveItemPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.TradeRemoveItem;

        private readonly MapServer _mapServer;
        private readonly ILogger _logger;


        public TraRemoveItemPacketProcessor(
            MapServer mapServer,
            ILogger logger)
        {
            _mapServer = mapServer;
            _logger = logger;

        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            var packet = new GamePacketReader(packetData);
            var tradeSlot = packet.ReadShort();


            var targetClient = _mapServer.FindClientByTamerHandle(client.Tamer.TargetTradeGeneralHandle);



            var Item = client.Tamer.TradeInventory.FindItemByTradeSlot(tradeSlot);

            if (Item != null)
            {

                client.Tamer.TradeInventory.RemoveOrReduceItem(Item, Item.Amount);

                client.Send(new TradeRemoveItemPacket(client.Tamer.GeneralHandler, (byte)tradeSlot));
                targetClient.Send(new TradeRemoveItemPacket(client.Tamer.GeneralHandler, (byte)tradeSlot));
            }

            //_logger.Verbose($"Character {client.TamerId} and {targetClient.TamerId} inventory unlock "); ;

        }


    }
}

