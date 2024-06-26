using DigitalWorldOnline.Commons.Enums.Account;

namespace DigitalWorldOnline.Commons.ViewModel.Account
{
    public class AccountUpdateViewModel
    {
        /// <summary>
        /// Unique sequential identifier.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Account username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Account e-mail.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Account access level.
        /// </summary>
        public AccountAccessLevelEnum AccessLevel { get; set; }

        /// <summary>
        /// Account premium coins.
        /// </summary>
        public int Premium { get; set; }

        /// <summary>
        /// Account silk(bônus) coins.
        /// </summary>
        public int Silk { get; set; }

        /// <summary>
        /// Flag for empty fields.
        /// </summary>
        public bool Empty => string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Email);

        public AccountUpdateViewModel() { }

        public AccountUpdateViewModel(
            long id,
            string username,
            string email,
            AccountAccessLevelEnum accessLevel,
            int premium,
            int silk)
        {
            Id = id;
            Username = username;
            Email = email;
            AccessLevel = accessLevel;
            Premium = premium;
            Silk = silk;
        }
    }
}