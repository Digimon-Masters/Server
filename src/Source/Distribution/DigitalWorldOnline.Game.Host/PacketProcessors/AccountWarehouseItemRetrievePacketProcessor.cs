using DigitalWorldOnline.Application;
using DigitalWorldOnline.Application.Separar.Commands.Update;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Models.Base;
using DigitalWorldOnline.Commons.Packets.GameServer;
using DigitalWorldOnline.Commons.Packets.GameServer.Combat;
using DigitalWorldOnline.Commons.Packets.Items;
using DigitalWorldOnline.Commons.Utils;
using MediatR;
using Serilog;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class AccountWarehouseItemRetrievePacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.RetrivieAccountWarehouseItem;

        private readonly ILogger _logger;
        private readonly ISender _sender;

        public AccountWarehouseItemRetrievePacketProcessor(
            ILogger logger,
            ISender sender)
        {
            _logger = logger;
            _sender = sender;

        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            var packet = new GamePacketReader(packetData);


            var itemSlot = packet.ReadShort();

            var targetItem = client.Tamer.AccountCashWarehouse.FindItemBySlot(itemSlot);

            if (targetItem != null)
            {

                var NewItem = (ItemModel)targetItem.Clone();

                NewItem.SetItemInfo(targetItem.ItemInfo);

                if (NewItem.IsTemporary)
                    NewItem.SetRemainingTime((uint)NewItem.ItemInfo.UsageTimeMinutes);

                if (client.Tamer.Inventory.AddItem(NewItem))
                {

                    client.Tamer.AccountCashWarehouse.RemoveOrReduceItem(targetItem,targetItem.Amount);

                    client.Tamer.AccountCashWarehouse.Sort();

                    client.Send(new AccountWarehouseItemRetrievePacket(NewItem, itemSlot));

                    client.Send(new LoadAccountWarehousePacket(client.Tamer.AccountCashWarehouse));
                    client.Send(new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory));

                    await _sender.Send(new UpdateItemsCommand(client.Tamer.Inventory));
                    await _sender.Send(new UpdateItemsCommand(client.Tamer.AccountCashWarehouse));
                }
                else
                {

                }
            }

        }

    }
}