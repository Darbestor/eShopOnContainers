namespace Catalog.API.Infrastructure.EntityConfigurations.SqlServer;

class MsSqlCatalogTypeEntityTypeConfiguration
    : IEntityTypeConfiguration<CatalogType>
{
    public void Configure(EntityTypeBuilder<CatalogType> builder)
    {
        builder.ToTable("CatalogType");

        builder.HasKey(ci => ci.Id);

        SqlServerPropertyBuilderExtensions.UseHiLo(builder.Property(ci => ci.Id), "catalog_type_hilo")
            .IsRequired();

        builder.Property(cb => cb.Type)
            .IsRequired()
            .HasMaxLength(100);
    }
}
