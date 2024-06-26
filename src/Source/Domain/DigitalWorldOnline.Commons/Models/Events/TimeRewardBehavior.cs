using DigitalWorldOnline.Commons.Enums;
using DigitalWorldOnline.Commons.Utils;

namespace DigitalWorldOnline.Commons.Models
{
    public sealed partial class TimeReward
    {
        public int RemainingTime
        {
            get
            {
                switch (RewardIndex)
                {
                    default: return -1;

                    case TimeRewardIndexEnum.First:
                    case TimeRewardIndexEnum.Second:
                    case TimeRewardIndexEnum.Third:
                    case TimeRewardIndexEnum.Fourth:
                        return (int)(StartTime - DateTime.Now).TotalSeconds;
                }
            }
        }

        public void UpdateRewardIndex()
        {
            switch (RewardIndex)
            {
                case TimeRewardIndexEnum.First:
                    RewardIndex = TimeRewardIndexEnum.Second;
                    StartTime = DateTime.Now.AddSeconds(TimeRewardDurationEnum.Second.GetHashCode());
                    break;

                case TimeRewardIndexEnum.Second:
                    RewardIndex = TimeRewardIndexEnum.Third;
                    StartTime = DateTime.Now.AddSeconds(TimeRewardDurationEnum.Third.GetHashCode());
                    break;

                case TimeRewardIndexEnum.Third:
                    RewardIndex = TimeRewardIndexEnum.Fourth;
                    StartTime = DateTime.Now.AddSeconds(TimeRewardDurationEnum.Fourth.GetHashCode());
                    break;

                case TimeRewardIndexEnum.Fourth:
                    RewardIndex = TimeRewardIndexEnum.Ended;
                    StartTime = DateTime.Now.AddSeconds(TimeRewardDurationEnum.Ended.GetHashCode());
                    break;
            }
        }
    }
}