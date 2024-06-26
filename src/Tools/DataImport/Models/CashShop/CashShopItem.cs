namespace DataImport.Models.CashShop;

public class CashShopItem
{
    public bool IsActive { get; set; }
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime? Date1 { get; set; }
    public DateTime? Date2 { get; set; }
    public int CategoryId { get; set; }
    public int UniqueId { get; set; }
    public int IconId { get; set; }
    public int SalesPercent { get; set; }
    public int PurchaseCashType { get; set; }
    public int[] Price { get; set; }
    public List<CashShopItemData> Items { get; set; }
    public int SellingPrice { get; set; }
}