using DigitalWorldOnline.Commons.Writers;

namespace DigitalWorldOnline.Commons.Packets.GameServer
{
    public class MembershipPacket : PacketWriter
    {
        private const int PacketNumber = 3414;

        /// <summary>
        /// Load the account remaining membership time.
        /// </summary>
        public MembershipPacket(DateTime membershipExpirationDate, int utcSeconds)
        {
            var remainingSeconds = (membershipExpirationDate - DateTime.Now).TotalSeconds;

            Type(PacketNumber);
            WriteByte(Convert.ToByte(remainingSeconds > 0));
            WriteInt(utcSeconds);
        }

        public MembershipPacket()
        {
            Type(PacketNumber);
            WriteByte(0);
            WriteInt(0);
        }
    }
}