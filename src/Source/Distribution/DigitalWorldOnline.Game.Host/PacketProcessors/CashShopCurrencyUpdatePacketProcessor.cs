using DigitalWorldOnline.Commons.Entities;
using DigitalWorldOnline.Commons.Enums.PacketProcessor;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Packets.GameServer;

namespace DigitalWorldOnline.Game.PacketProcessors;

public class CashShopCurrencyUpdatePacketProcessor : IGamePacketProcessor
{
    public GameServerPacketEnum Type => GameServerPacketEnum.PremiumCurrencyUpdate;
    
    public async Task Process(GameClient client, byte[] packetData)
    {
        client.Send(new CashShopCoinsPacket(client.Premium, client.Silk));
    }
}
