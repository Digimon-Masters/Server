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
    public class TradeAddItemPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.TradeAddItem;

        private readonly MapServer _mapServer;
        private readonly ILogger _logger;


        public TradeAddItemPacketProcessor(
            MapServer mapServer,
            ILogger logger)
        {
            _mapServer = mapServer;
            _logger = logger;

        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            var packet = new GamePacketReader(packetData);
            var inventorySlot = packet.ReadShort();
            var amount = packet.ReadShort();
            var tradeSlot = -1;

            var targetClient = _mapServer.FindClientByTamerHandle(client.Tamer.TargetTradeGeneralHandle);


            var Item = client.Tamer.Inventory.FindItemBySlot(inventorySlot);


            var NewItem = (ItemModel)Item.Clone();
            NewItem.Amount = amount;

            client.Tamer.TradeInventory.AddItem(NewItem);

            foreach (var item in client.Tamer.TradeInventory.Items)
            {
                tradeSlot++;
                if (item.Id == NewItem.Id)
                {
                    item.SetTradeSlot(tradeSlot);
                    break;
                }
            }


            targetClient.Send(new TradeInventoryUnlockPacket(client.Tamer.TargetTradeGeneralHandle));
            client.Send(new TradeInventoryUnlockPacket(client.Tamer.TargetTradeGeneralHandle));

            client.Send(new TradeAddItemPacket(client.Tamer.GeneralHandler, NewItem.ToArray(), (byte)tradeSlot, inventorySlot));
            targetClient.Send(new TradeAddItemPacket(client.Tamer.GeneralHandler, NewItem.ToArray(), (byte)tradeSlot, inventorySlot));

            //_logger.Verbose($"Character {client.TamerId} and {targetClient.TamerId} inventory unlock "); ;

        }
    }
}

