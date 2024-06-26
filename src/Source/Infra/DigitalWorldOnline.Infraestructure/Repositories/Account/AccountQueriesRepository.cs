using DigitalWorldOnline.Commons.Enums.Character;
using DigitalWorldOnline.Commons.Extensions;
using DigitalWorldOnline.Commons.DTOs.Account;
using DigitalWorldOnline.Commons.DTOs.Character;
using Microsoft.EntityFrameworkCore;
using DigitalWorldOnline.Commons.Interfaces;
using System.Linq;

namespace DigitalWorldOnline.Infraestructure.Repositories.Account
{
    public class AccountQueriesRepository : IAccountQueriesRepository
    {
        private readonly DatabaseContext _context;

        public AccountQueriesRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<AccountDTO?> GetAccountByUsernameAsync(string username)
        {
            return await _context.Account
                .AsNoTracking()
                .Include(x => x.SystemInformation)
                .Include(x => x.AccountBlock)
                .FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task<AccountDTO?> GetAccountByIdAsync(long id)
        {
            var dto = await _context.Account
                    .AsNoTracking()
                    .Include(x => x.SystemInformation)
                    .Include(x => x.AccountBlock)
                    .Include(x => x.ItemList)
                        .ThenInclude(y => y.Items)
                            .ThenInclude(z => z.AccessoryStatus) // Incluindo AccessoryStatus dentro de Items
                    .Include(x => x.ItemList)
                        .ThenInclude(y => y.Items)
                            .ThenInclude(z => z.SocketStatus) // Incluindo SocketStatus dentro de Items
                    .FirstOrDefaultAsync(x => x.Id == id);


            dto?.ItemList.ForEach(itemList => itemList.Items = itemList.Items.OrderBy(x => x.Slot).ToList());

            return dto;
        }

        public async Task<AccountBlockDTO?> GetAccountBlockByIdAsync(long id)
        {
            return await _context.AccountBlock
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<SystemInformationDTO?> GetSystemInformationByIdAsync(long id)
        {
            return await _context.SystemInformation
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IList<AccountDTO>> GetAllAccountsAsync()
        {
            var accs = await _context.Account
                .AsNoTracking()
                .ToListAsync();

            accs.ForEach(acc =>
            {
                acc.Password = acc.Password.Base64Decrypt();
            });

            return accs;
        }

        public async Task<IList<CharacterDTO>> GetConnectedCharactersAsync()
        {
            return await _context.Character
                .AsNoTracking()
                .Where(x => x.State == CharacterStateEnum.Ready)
                .Include(x => x.Location)
                .Include(x => x.Digimons)
                .ToListAsync();
        }
    }
}