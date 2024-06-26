using DigitalWorldOnline.Application;
using DigitalWorldOnline.Application.Separar.Commands.Update;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Extensions;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Models.Base;
using DigitalWorldOnline.Commons.Packets.PersonalShop;

using MediatR;
using Serilog;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class TamerShopOpenPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.TamerShopOpen;

        private readonly AssetsLoader _assets;
        private readonly ILogger _logger;
        private readonly ISender _sender;

        public TamerShopOpenPacketProcessor(
            AssetsLoader assets,
            ILogger logger,
            ISender sender)
        {
            _assets = assets;
            _logger = logger;
            _sender = sender;
        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            var packet = new GamePacketReader(packetData);

            _logger.Debug($"Getting parameters...");
            var shopName = packet.ReadString();
            packet.Skip(1);
            var sellQuantity = packet.ReadInt();

            List<ItemModel> sellList = new(sellQuantity);

            for (int i = 0; i < sellQuantity; i++)
            {
                var sellItem = new ItemModel(packet.ReadInt(), packet.ReadInt());

                packet.Skip(64);
                sellItem.SetSellPrice(packet.ReadInt());

                packet.Skip(12);
                sellList.Add(sellItem);
            }

            _logger.Debug($"{shopName} {sellQuantity}");

            foreach (var item in sellList)
            {
                item.SetItemInfo(_assets.ItemInfo.First(x => x.ItemId == item.ItemId));
                _logger.Debug($"{item.ItemId} {item.Amount} {item.TamerShopSellPrice}");
            }

            _logger.Debug($"Updating tamer shop item list...");
            client.Tamer.TamerShop.AddItems(sellList.Clone());
            await _sender.Send(new UpdateItemsCommand(client.Tamer.TamerShop));

            _logger.Debug($"Updating tamer inventory item list...");
            client.Tamer.Inventory.RemoveOrReduceItems(sellList.Clone());
            await _sender.Send(new UpdateItemsCommand(client.Tamer.Inventory));

            client.Tamer.UpdateCurrentCondition(ConditionEnum.TamerShop);
            client.Tamer.UpdateShopName(shopName);

            //_logger.Debug($"Sending sync in condition packet...");
            //_mapServer.BroadcastForViewsAndSelf(client.TamerId, new SyncConditionPacket(client.Tamer.GeneralHandler, client.Tamer.CurrentCondition, shopName));

            _logger.Debug($"Sending tamer shop open packet...");
            client.Send(new PersonalShopPacket(client.Tamer.ShopItemId));
        }
    }
}