using DigitalWorldOnline.Commons.Enums.Account;
using MediatR;

namespace DigitalWorldOnline.Application.Admin.Commands
{
    public class UpdateAccountCommand : IRequest
    {
        public long Id { get; }
        public string Username { get; }
        public string Email { get; }
        public AccountAccessLevelEnum AccessLevel { get; }
        public int Premium { get; }
        public int Silk { get; }

        public UpdateAccountCommand(
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