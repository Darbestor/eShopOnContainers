namespace Catalog.API.Infrastructure.EntityConfigurations.SqlServer;

class PostgreCatalogTypeEntityTypeConfiguration
    : IEntityTypeConfiguration<CatalogType>
{
    public void Configure(EntityTypeBuilder<CatalogType> builder)
    {
        builder.ToTable("catalog-type");

        builder.HasKey(ci => ci.Id);

        NpgsqlPropertyBuilderExtensions.UseHiLo(builder.Property(ci => ci.Id), "catalog_type_hilo")
            .IsRequired();

        builder.Property(cb => cb.Type)
            .IsRequired()
            .HasMaxLength(100);
    }
}
