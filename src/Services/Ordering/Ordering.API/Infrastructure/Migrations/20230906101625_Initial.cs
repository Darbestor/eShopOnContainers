using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microsoft.eShopOnContainers.Services.Ordering.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "buyerseq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "orderitemseq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "orderseq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "paymentseq",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "buyers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    identity_guid = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_buyers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cardtypes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cardtypes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "orderstatus",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orderstatus", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "requests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_requests", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "paymentmethods",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    cardtypeid = table.Column<int>(name: "card-type-id", type: "integer", nullable: false),
                    buyer_id = table.Column<int>(type: "integer", nullable: false),
                    alias = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    cardholdername = table.Column<string>(name: "card-holder-name", type: "character varying(200)", maxLength: 200, nullable: false),
                    cardnumber = table.Column<string>(name: "card-number", type: "character varying(25)", maxLength: 25, nullable: false),
                    expiration = table.Column<DateTime>(type: "timestamp with time zone", maxLength: 25, nullable: false),
                    buyer_id1 = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_paymentmethods", x => x.id);
                    table.ForeignKey(
                        name: "fk_paymentmethods_buyers_buyer_id",
                        column: x => x.buyer_id1,
                        principalTable: "buyers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_paymentmethods_card_types_card_type_id",
                        column: x => x.cardtypeid,
                        principalTable: "cardtypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    address_street = table.Column<string>(type: "text", nullable: true),
                    address_city = table.Column<string>(type: "text", nullable: true),
                    address_state = table.Column<string>(type: "text", nullable: true),
                    address_country = table.Column<string>(type: "text", nullable: true),
                    address_zip_code = table.Column<string>(type: "text", nullable: true),
                    address_id = table.Column<int>(type: "integer", nullable: true),
                    orderstatusid = table.Column<int>(name: "order-status-id", type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    buyerid = table.Column<int>(name: "buyer-id", type: "integer", nullable: true),
                    orderdate = table.Column<DateTime>(name: "order-date", type: "timestamp with time zone", nullable: false),
                    paymentmethodid = table.Column<int>(name: "payment-method-id", type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orders", x => x.id);
                    table.ForeignKey(
                        name: "fk_orders_buyers_buyer_id",
                        column: x => x.buyerid,
                        principalTable: "buyers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_orders_order_status_order_status_id",
                        column: x => x.orderstatusid,
                        principalTable: "orderstatus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_orders_paymentmethods_payment_method_id",
                        column: x => x.paymentmethodid,
                        principalTable: "paymentmethods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "order-items",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    order_id = table.Column<int>(type: "integer", nullable: false),
                    discount = table.Column<decimal>(type: "numeric", nullable: false),
                    pictureurl = table.Column<string>(name: "picture-url", type: "text", nullable: true),
                    productname = table.Column<string>(name: "product-name", type: "text", nullable: false),
                    unitprice = table.Column<decimal>(name: "unit-price", type: "numeric", nullable: false),
                    units = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_items_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_buyers_identity_guid",
                table: "buyers",
                column: "identity_guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_order_items_order_id",
                table: "order-items",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_orders_buyer_id",
                table: "orders",
                column: "buyer-id");

            migrationBuilder.CreateIndex(
                name: "ix_orders_order_status_id",
                table: "orders",
                column: "order-status-id");

            migrationBuilder.CreateIndex(
                name: "ix_orders_payment_method_id",
                table: "orders",
                column: "payment-method-id");

            migrationBuilder.CreateIndex(
                name: "ix_paymentmethods_buyer_id",
                table: "paymentmethods",
                column: "buyer_id1");

            migrationBuilder.CreateIndex(
                name: "ix_paymentmethods_card_type_id",
                table: "paymentmethods",
                column: "card-type-id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "order-items");

            migrationBuilder.DropTable(
                name: "requests");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "orderstatus");

            migrationBuilder.DropTable(
                name: "paymentmethods");

            migrationBuilder.DropTable(
                name: "buyers");

            migrationBuilder.DropTable(
                name: "cardtypes");

            migrationBuilder.DropSequence(
                name: "buyerseq");

            migrationBuilder.DropSequence(
                name: "orderitemseq");

            migrationBuilder.DropSequence(
                name: "orderseq");

            migrationBuilder.DropSequence(
                name: "paymentseq");
        }
    }
}
