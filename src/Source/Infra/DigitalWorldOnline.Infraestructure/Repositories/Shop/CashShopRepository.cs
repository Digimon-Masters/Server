using DigitalWorldOnline.Commons.DTOs.Shop;
using DigitalWorldOnline.Commons.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalWorldOnline.Infraestructure.Repositories.Shop;

public class CashShopRepository : ICashShopRepository
{
    private readonly DatabaseContext _context;

    public CashShopRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<CashShopDTO?> GetCashShopItemAsync(int uniqueId)
    {
        var cashShopItem = await _context.CashShopItems.Include(x => x.CashShopItemsNavigation).FirstOrDefaultAsync(x => x.UniqueId == uniqueId);
        return cashShopItem;
    }
}