using DigitalWorldOnline.Application;
using DigitalWorldOnline.Application.Separar.Commands.Update;
using DigitalWorldOnline.Application.Separar.Queries;
using DigitalWorldOnline.Commons.DTOs.Shop;
using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Models.Base;
using DigitalWorldOnline.Commons.Packets.Items;
using DigitalWorldOnline.Commons.Writers;
using MediatR;

namespace DigitalWorldOnline.Game.PacketProcessors;

public class CashShopOnPurchasePacketProcessor : IGamePacketProcessor
{
    private readonly ISender _sender;
    private readonly AssetsLoader _assetsLoader;

    public CashShopOnPurchasePacketProcessor(ISender sender, AssetsLoader assetsLoader)
    {
        _sender = sender;
        _assetsLoader = assetsLoader;
    }

    public GameServerPacketEnum Type => GameServerPacketEnum.CashShopPurchase;
    
    public async Task Process(GameClient client, byte[] packetData)
    {
        var packet = new GamePacketReader(packetData);
        var amount = packet.ReadByte();
        var price = packet.ReadInt();
        client.AddPremium(-(price));
        var type = packet.ReadInt(); //premium or silk
        var unknown1 = packet.ReadInt();
        var uniqueIds = new int[amount];
        var cashItemsList = new List<CashShopDTO>();
        for (var i = 0; i < amount; i++)
        {
            uniqueIds[i] = packet.ReadInt();
            var item = await _sender.Send(new GetCashShopItem(uniqueIds[i]));
            cashItemsList.Add(item);
        }
        
        foreach (var cashShopItem in cashItemsList)
        {
            foreach (var item in cashShopItem.CashShopItemsNavigation)
            {
                var purchasingItem = new ItemModel();
                purchasingItem.SetItemId(item.ItemId); 
                purchasingItem.SetAmount(item.Quantity);
                purchasingItem.SetItemInfo(_assetsLoader.ItemInfo.First(x => x.ItemId == item.ItemId));
                client.Tamer.Inventory.AddItem(purchasingItem);
            }
        }

        await _sender.Send(new UpdateItemsCommand(client.Tamer.Inventory));
        
        client.Send(new LoadInventoryPacket(client.Tamer.Inventory, InventoryTypeEnum.Inventory));
        client.Send(new CashShopSuccessPurchasePacket(client.Premium, (short)client.Silk));
    }

}