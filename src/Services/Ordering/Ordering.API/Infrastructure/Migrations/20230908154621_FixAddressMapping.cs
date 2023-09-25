using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microsoft.eShopOnContainers.Services.Ordering.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixAddressMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "address_id",
                table: "orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "address_id",
                table: "orders",
                type: "integer",
                nullable: true);
        }
    }
}
