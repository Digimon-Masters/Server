using DigitalWorldOnline.Commons.DTOs.Shop;
using DigitalWorldOnline.Infraestructure.ContextConfiguration.Shop;
using Microsoft.EntityFrameworkCore;

namespace DigitalWorldOnline.Infraestructure
{
    public partial class DatabaseContext : DbContext
    {
        //TODO: Tamershop
        public DbSet<CashShopDTO> CashShopItems { get; set; }
        internal static void ShopEntityConfiguration(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ConsignedShopConfiguration());
            builder.ApplyConfiguration(new ConsignedShopLocationConfiguration());
            builder.ApplyConfiguration(new CashShopConfiguration());
            builder.ApplyConfiguration(new CashShopItemsConfiguration());
        }
    }
}