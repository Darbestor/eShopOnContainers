namespace Microsoft.eShopOnContainers.Services.Ordering.Infrastructure.EntityConfigurations;

class PaymentMethodEntityTypeConfiguration
    : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> paymentConfiguration)
    {
        paymentConfiguration.ToTable("paymentmethods");

        paymentConfiguration.HasKey(b => b.Id);

        paymentConfiguration.Ignore(b => b.DomainEvents);

        paymentConfiguration.Property(b => b.Id)
            .UseHiLo("paymentseq");

        paymentConfiguration.Property<int>("BuyerId")
            .IsRequired();

        paymentConfiguration
            .Property<string>("_cardHolderName")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("card_holder_name")
            .HasMaxLength(200)
            .IsRequired();

        paymentConfiguration
            .Property<string>("_alias")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("alias")
            .HasMaxLength(200)
            .IsRequired();

        paymentConfiguration
            .Property<string>("_cardNumber")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("card_number")
            .HasMaxLength(25)
            .IsRequired();

        paymentConfiguration
            .Property<DateTime>("_expiration")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("expiration")
            .HasMaxLength(25)
            .IsRequired();

        paymentConfiguration
            .Property<int>("_cardTypeId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("card_type_id")
            .IsRequired();

        paymentConfiguration.HasOne(p => p.CardType)
            .WithMany()
            .HasForeignKey("_cardTypeId");
    }
}
