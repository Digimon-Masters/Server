using DigitalWorldOnline.Commons.Models;
using DigitalWorldOnline.Commons.Writers;

namespace DigitalWorldOnline.Commons.Packets.GameServer
{
    public class TimeRewardPacket : PacketWriter
    {
        private const int PacketNumber = 3106;

        /// <summary>
        /// Load the time reward current streak.
        /// </summary>
        /// <param name="remainingSeconds">The membership remaining seconds (UTC).</param>
        public TimeRewardPacket(TimeReward timeReward)
        {
            Type(PacketNumber);
            WriteInt(timeReward.RewardIndex.GetHashCode());
            WriteInt(timeReward.RemainingTime);
            WriteInt(timeReward.RemainingTime);
            WriteByte(1);
        }
    }
}