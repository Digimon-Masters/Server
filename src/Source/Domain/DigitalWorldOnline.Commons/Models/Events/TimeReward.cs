using DigitalWorldOnline.Commons.Enums;

namespace DigitalWorldOnline.Commons.Models
{
    public sealed partial class TimeReward
    {
        /// <summary>
        /// Unique sequential identifier.
        /// </summary>
        public long Id { get; private set; }

        /// <summary>
        /// Reference to the owner.
        /// </summary>
        public long CharacterId { get; private set; }

        /// <summary>
        /// The current index start time.
        /// </summary>
        public DateTime StartTime { get; private set; }

        /// <summary>
        /// The reward current index and duration.
        /// </summary>
        public TimeRewardIndexEnum RewardIndex { get; private set; }

        public TimeReward()
        {
            RewardIndex = TimeRewardIndexEnum.First;
            StartTime = DateTime.Now;
        }
    }
}