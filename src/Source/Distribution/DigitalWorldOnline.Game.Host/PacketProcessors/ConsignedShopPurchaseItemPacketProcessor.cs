using AutoMapper;
using DigitalWorldOnline.Application;
using DigitalWorldOnline.Application.Separar.Commands.Delete;
using DigitalWorldOnline.Application.Separar.Commands.Update;
using DigitalWorldOnline.Application.Separar.Queries;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Extensions;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Models.Base;
using DigitalWorldOnline.Commons.Models.Character;
using DigitalWorldOnline.Commons.Models.TamerShop;
using DigitalWorldOnline.Commons.Packets.Chat;
using DigitalWorldOnline.Commons.Packets.Items;
using DigitalWorldOnline.Commons.Packets.PersonalShop;
using MediatR;
using Serilog;

namespace DigitalWorldOnline.Game.PacketProcessors
{
    public class ConsignedShopPurchaseItemPacketProcessor : IGamePacketProcessor
    {
        public GameServerPacketEnum Type => GameServerPacketEnum.ConsignedShopPurchaseItem;

        private readonly AssetsLoader _assets;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly ISender _sender;

        public ConsignedShopPurchaseItemPacketProcessor(
            AssetsLoader assets,
            ILogger logger,
            IMapper mapper,
            ISender sender)
        {
            _assets = assets;
            _logger = logger;
            _mapper = mapper;
            _sender = sender;
        }

        public async Task Process(GameClient client, byte[] packetData)
        {
            var packet = new GamePacketReader(packetData);

            _logger.Debug($"Getting parameters...");
            var shopHandler = packet.ReadInt();
            var shopSlot = packet.ReadInt();
            var boughtItemId = packet.ReadInt();
            var boughtAmount = packet.ReadInt();
            packet.Skip(60);
            var boughtUnitPrice = packet.ReadInt64();

            _logger.Debug($"{shopHandler} {shopSlot} {boughtItemId} {boughtAmount} {boughtUnitPrice}");

            _logger.Debug($"Searching consigned shop {shopHandler}...");
            var shop = _mapper.Map<ConsignedShop>(await _sender.Send(new ConsignedShopByHandlerQuery(shopHandler)));
            if (shop == null)
            {
                _logger.Debug($"Consigned shop {shopHandler} not found...");
                client.Send(new UnloadConsignedShopPacket(shopHandler));
                return;
            }

            var seller = _mapper.Map<CharacterModel>(await _sender.Send(new CharacterAndItemsByIdQuery(shop.CharacterId)));
            if (seller == null)
            {
                _logger.Debug($"Deleting consigned shop {shopHandler}...");
                await _sender.Send(new DeleteConsignedShopCommand(shopHandler));

                _logger.Debug($"Consigned shop owner {shop.CharacterId} not found...");
                client.Send(new UnloadConsignedShopPacket(shopHandler));
                return;
            }

            var totalValue = boughtUnitPrice * boughtAmount;

            _logger.Debug($"Removing {totalValue} bits...");
            client.Tamer.Inventory.RemoveBits(totalValue);

            _logger.Debug($"Updating inventory bits...");
            await _sender.Send(new UpdateItemListBitsCommand(client.Tamer.Inventory));

            var newItem = new ItemModel(boughtItemId, boughtAmount);
            newItem.SetItemInfo(_assets.ItemInfo.FirstOrDefault(x => x.ItemId == boughtItemId));

            _logger.Debug($"Adding bought item...");
            client.Tamer.Inventory.AddItems(((ItemModel)newItem.Clone()).GetList());

            _logger.Debug($"Updating item list...");
            await _sender.Send(new UpdateItemsCommand(client.Tamer.Inventory));

            var sellerClient = client.Server.FindByTamerId(shop.CharacterId);
            if (sellerClient != null && sellerClient.IsConnected)
            {
                _logger.Debug($"Sending system message packet {sellerClient.TamerId}...");
                var itemName = _assets.ItemInfo.FirstOrDefault(x => x.ItemId == boughtItemId)?.Name ?? "item";
                var message = $"You have sold x{boughtAmount} {itemName} in Consigned Store!";
                sellerClient.Send(new SystemMessagePacket(message));

                _logger.Debug($"Adding {totalValue} bits to {sellerClient.TamerId} consigned warehouse...");
                sellerClient.Tamer.ConsignedWarehouse.AddBits(totalValue);

                _logger.Debug($"Updating {sellerClient.TamerId} consigned warehouse...");
                await _sender.Send(new UpdateItemListBitsCommand(sellerClient.Tamer.ConsignedWarehouse));

                _logger.Debug($"Removing consigned shop bought item...");
                seller.ConsignedShopItems.RemoveOrReduceItems(((ItemModel)newItem.Clone()).GetList());

                _logger.Debug($"Updating {seller.Id} consigned shop items...");
                await _sender.Send(new UpdateItemsCommand(seller.ConsignedShopItems));
            }

            if (seller.ConsignedShopItems.Count == 0)
            {
                _logger.Debug($"Deleting consigned shop {shopHandler}...");
                await _sender.Send(new DeleteConsignedShopCommand(shopHandler));

                _logger.Debug($"Sending unload consigned shop packet {shopHandler}...");
                client.Send(new UnloadConsignedShopPacket(shopHandler));

                _logger.Debug($"Sending consigned shop close packet...");
                sellerClient?.Send(new ConsignedShopClosePacket());
            }
            else
            {
                seller.ConsignedShopItems.Items.ForEach(item =>
                { item.SetItemInfo(_assets.ItemInfo.FirstOrDefault(x => x.ItemId == item.ItemId)); });
            }

            _logger.Debug($"Sending load inventory packet...");
            client.Send(new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory));

            _logger.Debug($"Sending consigned shop bought item packet...");
            client.Send(new ConsignedShopBoughtItemPacket(shopSlot, boughtAmount));

            _logger.Debug($"Sending consigned shop item list view packet...");
            client.Send(new ConsignedShopItemsViewPacket(shop, seller.ConsignedShopItems, seller.Name));
        }
    }
}
