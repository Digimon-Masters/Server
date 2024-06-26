namespace DigitalWorldOnline.Commons.Models.Events
{
    public sealed partial class AttendanceRewardModel
    {
      public bool ReedemRewards => LastRewardDate.Date < DateTime.UtcNow.Date;

        public void SetLastRewardDate()
        {
            LastRewardDate = DateTime.Now;  
        }

        public void IncreaseTotalDays(byte amount = 1)
        {
            TotalDays += amount;
        }
    }
}