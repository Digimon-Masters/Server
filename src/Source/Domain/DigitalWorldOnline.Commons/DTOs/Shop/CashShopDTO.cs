using System.ComponentModel.DataAnnotations;

namespace DigitalWorldOnline.Commons.DTOs.Shop;

public class CashShopDTO : ICloneable
{
    public Guid Id { get; set; }
    
    public bool IsActive { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
   
    public DateTime? StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    public int CategoryId { get; set; }
    
    public int UniqueId { get; set; }
    
    public int IconId { get; set; }
    
    public int SalesPercent { get; set; }
    
    public int PurchaseCashType { get; set; }
    
    public int SellingPrice { get; set; }
    
    [StringLength(200)]
    public string ItemsJson { get; set; }

    public ICollection<CashShopItemsDTO> CashShopItemsNavigation { get; set; } = new HashSet<CashShopItemsDTO>();
    
    public object Clone()
    {
        return (CashShopDTO)MemberwiseClone();
    }
}