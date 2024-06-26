using DigitalWorldOnline.Commons.DTOs.Shop;

namespace DigitalWorldOnline.Commons.Interfaces;

public interface ICashShopRepository
{
    Task<CashShopDTO?> GetCashShopItemAsync(int uniqueId);
}