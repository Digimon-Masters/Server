using DigitalWorldOnline.Commons.DTOs.Shop;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalWorldOnline.Infraestructure.ContextConfiguration.Shop;

public class CashShopItemsConfiguration : IEntityTypeConfiguration<CashShopItemsDTO>
{
    public void Configure(EntityTypeBuilder<CashShopItemsDTO> builder)
    {
        builder.ToTable("CashShopItems", "Shop").HasKey(c => new { c.UniqueId, c.ItemId });
        
        builder
                .HasOne(x => x.CashShopNavigation)
                .WithMany(x => x.CashShopItemsNavigation)
                .HasForeignKey(x => x.UniqueId)
                .HasPrincipalKey(x => x.UniqueId);


        builder.HasOne(x => x.ItemAssetNavigation)
            .WithMany(x => x.CashShopItemsNavigation)
            .HasForeignKey(x => x.ItemId)
            .HasPrincipalKey(x => x.ItemId);
    }
}