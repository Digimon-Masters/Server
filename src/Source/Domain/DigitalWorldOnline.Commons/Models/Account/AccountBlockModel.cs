using DigitalWorldOnline.Commons.Enums.Account;

namespace DigitalWorldOnline.Commons.Models.Account
{
    public class AccountBlockModel
    {
        public long Id { get; set; }

        public long AccountId { get; set; }

        public AccountBlockEnum Type { get; set; }

        public string Reason { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public uint RemainingTimeInSeconds
        {
            get
            {
                return (uint)(EndDate - StartDate).TotalSeconds;
            }
        }
    }
}
