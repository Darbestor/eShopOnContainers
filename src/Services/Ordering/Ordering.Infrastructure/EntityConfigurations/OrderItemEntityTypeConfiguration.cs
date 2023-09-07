namespace Microsoft.eShopOnContainers.Services.Ordering.Infrastructure.EntityConfigurations;

class OrderItemEntityTypeConfiguration
    : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> orderItemConfiguration)
    {
        orderItemConfiguration.ToTable("order-items");

        orderItemConfiguration.HasKey(o => o.Id);

        orderItemConfiguration.Ignore(b => b.DomainEvents);

        orderItemConfiguration.Property(o => o.Id)
            .UseHiLo("orderitemseq");

        // Shadow property
        // Foreign key for 1 to many relationship with order,
        // because Order contains list of OrderItem
        orderItemConfiguration.Property<int>("OrderId")
            .IsRequired();

        orderItemConfiguration
            .Property<decimal>("_discount")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("discount")
            .IsRequired();

        orderItemConfiguration.Property<int>("ProductId")
            .IsRequired();

        orderItemConfiguration
            .Property<string>("_productName")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("product_name")
            .IsRequired();

        orderItemConfiguration
            .Property<decimal>("_unitPrice")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("unit_price")
            .IsRequired();

        orderItemConfiguration
            .Property<int>("_units")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("units")
            .IsRequired();

        orderItemConfiguration
            .Property<string>("_pictureUrl")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("picture_url")
            .IsRequired(false);
    }
}
