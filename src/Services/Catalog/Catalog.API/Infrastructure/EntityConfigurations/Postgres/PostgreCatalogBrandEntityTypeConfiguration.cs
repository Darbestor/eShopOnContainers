namespace Catalog.API.Infrastructure.EntityConfigurations.Postgres;

class PostgreCatalogBrandEntityTypeConfiguration
    : IEntityTypeConfiguration<CatalogBrand>
{
    public void Configure(EntityTypeBuilder<CatalogBrand> builder)
    {
        builder.ToTable("catalog-brand");

        builder.HasKey(ci => ci.Id);

        NpgsqlPropertyBuilderExtensions.UseHiLo(builder.Property(ci => ci.Id), "catalog_brand_hilo")
            .IsRequired();

        builder.Property(cb => cb.Brand)
            .IsRequired()
            .HasMaxLength(100);
    }
}
