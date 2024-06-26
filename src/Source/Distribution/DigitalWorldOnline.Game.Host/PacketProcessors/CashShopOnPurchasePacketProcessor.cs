using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;

namespace DigitalWorldOnline.Game.PacketProcessors;

public class CashShopOnPurchasePacketProcessor : IGamePacketProcessor
{
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
        for (var i = 0; i < amount; i++)
        {
            uniqueIds[i] = packet.ReadInt();
        }
    }

}