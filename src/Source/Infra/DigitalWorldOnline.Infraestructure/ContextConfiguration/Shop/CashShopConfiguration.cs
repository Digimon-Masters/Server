using DigitalWorldOnline.Commons.DTOs.Shop;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalWorldOnline.Infraestructure.ContextConfiguration.Shop;

public class CashShopConfiguration : IEntityTypeConfiguration<CashShopDTO>
{
    public void Configure(EntityTypeBuilder<CashShopDTO> builder)
    {
        builder.ToTable("CashShop", "Shop")
               .HasKey(x => x.Id);

        builder
            .Property(x => x.Name)
            .HasColumnType("varchar")
            .HasMaxLength(100);
        
        builder.Property(x => x.Description)
            .HasColumnType("varchar")
            .HasMaxLength(2000);
        
        builder.Property(x => x.ItemsJson)
               .HasColumnType("nvarchar")
               .HasMaxLength(500);
    }
}