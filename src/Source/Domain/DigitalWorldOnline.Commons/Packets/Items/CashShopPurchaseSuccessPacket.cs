using DigitalWorldOnline.Commons.Writers;

namespace DigitalWorldOnline.Commons.Packets.Items;

public class CashShopSuccessPurchasePacket : PacketWriter
{
    public CashShopSuccessPurchasePacket(int premium, short silk)
    {
        Type(3413);
        WriteByte(0);
        WriteByte(0);
        WriteInt(premium); // premium
        WriteShort(silk); // silk
        WriteByte(0);
        WriteByte(0);
    }
}
