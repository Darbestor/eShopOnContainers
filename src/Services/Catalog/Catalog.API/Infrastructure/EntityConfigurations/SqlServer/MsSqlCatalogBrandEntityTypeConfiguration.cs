﻿namespace Microsoft.eShopOnContainers.Services.Catalog.API.Infrastructure.EntityConfigurations.SqlServer;

class MsSqlCatalogBrandEntityTypeConfiguration
    : IEntityTypeConfiguration<CatalogBrand>
{
    public void Configure(EntityTypeBuilder<CatalogBrand> builder)
    {
        builder.ToTable("CatalogBrand");

        builder.HasKey(ci => ci.Id);

        SqlServerPropertyBuilderExtensions.UseHiLo(builder.Property(ci => ci.Id), "catalog_brand_hilo")
            .IsRequired();

        builder.Property(cb => cb.Brand)
            .IsRequired()
            .HasMaxLength(100);
    }
}
