using InventoryManagement.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Api.Data;

public class InventoryRecordConfig : IEntityTypeConfiguration<InventoryRecord>
{
    public void Configure(EntityTypeBuilder<InventoryRecord> builder)
    {
        builder.ToTable("inventory_records");
        builder.HasKey(ir => ir.Id);
        builder.Property(ir => ir.Reason).IsRequired().HasMaxLength(1024);

        // many to one rel to product
        builder
            .HasOne(ir => ir.Product)
            .WithMany(p => p.InventoryRecords)
            .HasForeignKey(ir => ir.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
