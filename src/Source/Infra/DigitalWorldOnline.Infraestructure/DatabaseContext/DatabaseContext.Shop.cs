using DigitalWorldOnline.Infraestructure.ContextConfiguration.Shop;
using Microsoft.EntityFrameworkCore;

namespace DigitalWorldOnline.Infraestructure
{
    public partial class DatabaseContext : DbContext
    {
        //TODO: Tamershop, Cashshop
        internal static void ShopEntityConfiguration(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ConsignedShopConfiguration());
            builder.ApplyConfiguration(new ConsignedShopLocationConfiguration());
        }
    }
}