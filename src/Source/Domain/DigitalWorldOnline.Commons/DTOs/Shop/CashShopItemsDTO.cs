using DigitalWorldOnline.Commons.DTOs.Assets;

namespace DigitalWorldOnline.Commons.DTOs.Shop;

public class CashShopItemsDTO : ICloneable
{
    public int UniqueId { get; set; }
    public int ItemId { get; set; }
    public int Quantity { get; set; }
    
    public ItemAssetDTO ItemAssetNavigation { get; set; }
    public CashShopDTO CashShopNavigation { get; set; }
    
    public object Clone()
    {
        return (CashShopItemsDTO)MemberwiseClone();
    }
}