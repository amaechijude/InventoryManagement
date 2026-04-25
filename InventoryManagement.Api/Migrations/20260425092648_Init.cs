using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InventoryManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    SKU = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", precision: 16, scale: 2, nullable: false),
                    StockLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "inventory_records",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProductId = table.Column<Guid>(type: "TEXT", nullable: false),
                    QuantityChanged = table.Column<int>(type: "INTEGER", nullable: false),
                    Reason = table.Column<string>(type: "TEXT", maxLength: 1024, nullable: false),
                    RecordedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_records", x => x.Id);
                    table.ForeignKey(
                        name: "FK_inventory_records_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "Name", "Price", "SKU", "StockLevel" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567001"), new DateTimeOffset(new DateTime(2024, 1, 10, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, "Wireless Mouse", 29.99m, "WM-001", 150 },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567002"), new DateTimeOffset(new DateTime(2024, 1, 12, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, "Mechanical Keyboard", 89.99m, "MK-002", 75 },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567003"), new DateTimeOffset(new DateTime(2024, 1, 15, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, "USB-C Hub", 49.99m, "UH-003", 200 },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567004"), new DateTimeOffset(new DateTime(2024, 2, 1, 11, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, "27\" Monitor", 349.99m, "MN-004", 40 },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567005"), new DateTimeOffset(new DateTime(2024, 2, 5, 14, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, "Laptop Stand", 39.99m, "LS-005", 120 },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567006"), new DateTimeOffset(new DateTime(2024, 2, 10, 9, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, "Webcam 1080p", 79.99m, "WC-006", 90 },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567007"), new DateTimeOffset(new DateTime(2024, 2, 14, 13, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, "Noise Cancelling Headset", 129.99m, "NH-007", 60 },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567008"), new DateTimeOffset(new DateTime(2024, 3, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, "Desk Lamp", 34.99m, "DL-008", 180 },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567009"), new DateTimeOffset(new DateTime(2024, 3, 8, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, "External SSD 1TB", 109.99m, "ES-009", 55 },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567010"), new DateTimeOffset(new DateTime(2024, 3, 15, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, "Ergonomic Chair", 499.99m, "EC-010", 25 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_inventory_records_ProductId",
                table: "inventory_records",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_products_SKU",
                table: "products",
                column: "SKU",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inventory_records");

            migrationBuilder.DropTable(
                name: "products");
        }
    }
}
