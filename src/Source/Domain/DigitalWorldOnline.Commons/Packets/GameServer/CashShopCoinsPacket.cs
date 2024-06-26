using DigitalWorldOnline.Commons.Writers;

namespace DigitalWorldOnline.Commons.Packets.GameServer
{
    public class CashShopCoinsPacket : PacketWriter
    {
        private const int PacketNumber = 3404;

        /// <summary>
        /// Load the account remaining membership time.
        /// </summary>
        /// <param name="remainingSeconds">The membership remaining seconds (UTC).</param>
        public CashShopCoinsPacket(int premium, int silk)
        {
            Type(PacketNumber);
            WriteInt(0);
            WriteInt(silk);
            WriteInt(premium);
        }
    }
}