using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OBeefSoup.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastOrderDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "StoreLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MapUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OpeningHours = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreLocations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DeliveryAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "DisplayOrder", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, "Các loại phở bò truyền thống", 1, true, "Phở Bò" },
                    { 2, "Phở gà thanh đạm, thơm ngon", 2, true, "Phở Gà" },
                    { 3, "Nước giải khát", 3, true, "Đồ Uống" }
                });

            migrationBuilder.InsertData(
                table: "StoreLocations",
                columns: new[] { "Id", "Address", "City", "Email", "IsActive", "MapUrl", "Name", "OpeningHours", "Phone" },
                values: new object[] { 1, "123 Nguyễn Huệ, Quận 1, TP. Hồ Chí Minh", "Hồ Chí Minh", "contact@obeefsoup.vn", true, "", "O' BeefSoup - Chi nhánh Trung tâm", "7:00 - 22:00 (Hàng ngày)", "0901 234 567" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedDate", "Description", "ImageUrl", "IsActive", "IsFeatured", "Name", "Price", "Stock", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 2, 11, 13, 19, 27, 680, DateTimeKind.Local).AddTicks(5653), "Thịt bò tái mềm, nước dùng thanh ngọt", "/images/pho-bo-tai-nam.jpg", true, true, "Phở Tái", 65000m, 100, null },
                    { 2, 1, new DateTime(2026, 2, 11, 13, 19, 27, 680, DateTimeKind.Local).AddTicks(5657), "Gầu bò béo ngậy, đậm đà truyền thống", "/images/pho-bo-chin.jpg", true, true, "Phở Gầu", 70000m, 100, null },
                    { 3, 1, new DateTime(2026, 2, 11, 13, 19, 27, 680, DateTimeKind.Local).AddTicks(5660), "Đầy đủ topping - Signature O' BeefSoup", "https://placehold.co/400x300/8B0000/FFF?text=Pho+Dac+Biet", true, true, "Phở Đặc Biệt", 85000m, 100, null },
                    { 4, 1, new DateTime(2026, 2, 11, 13, 19, 27, 680, DateTimeKind.Local).AddTicks(5664), "Nạm bò thơm ngon, mềm tan", "https://placehold.co/400x300/8B0000/FFF?text=Pho+Nam", true, false, "Phở Nạm", 68000m, 100, null },
                    { 5, 1, new DateTime(2026, 2, 11, 13, 19, 27, 680, DateTimeKind.Local).AddTicks(5666), "Bò viên giòn sần sật", "https://placehold.co/400x300/8B0000/FFF?text=Pho+Bo+Vien", true, false, "Phở Bò Viên", 60000m, 100, null },
                    { 6, 2, new DateTime(2026, 2, 11, 13, 19, 27, 680, DateTimeKind.Local).AddTicks(5876), "Gà ta thơm ngon, nước dùng thanh ngọt", "https://placehold.co/400x300/FFB347/000?text=Pho+Ga", true, false, "Phở Gà", 55000m, 100, null },
                    { 7, 2, new DateTime(2026, 2, 11, 13, 19, 27, 680, DateTimeKind.Local).AddTicks(5879), "Gà ta nguyên con, đầy đủ topping", "https://placehold.co/400x300/FFB347/000?text=Pho+Ga+Dac+Biet", true, false, "Phở Gà Đặc Biệt", 75000m, 100, null },
                    { 8, 3, new DateTime(2026, 2, 11, 13, 19, 27, 680, DateTimeKind.Local).AddTicks(5885), "Trà đá miễn phí", "https://placehold.co/400x300/5E8B7E/FFF?text=Tra+Da", true, false, "Trà Đá", 0m, 1000, null },
                    { 9, 3, new DateTime(2026, 2, 11, 13, 19, 27, 680, DateTimeKind.Local).AddTicks(5888), "Coca, Pepsi, 7Up", "https://placehold.co/400x300/FF6347/FFF?text=Nuoc+Ngot", true, false, "Nước Ngọt", 15000m, 200, null },
                    { 10, 3, new DateTime(2026, 2, 11, 13, 19, 27, 680, DateTimeKind.Local).AddTicks(5891), "Chanh tươi vắt", "https://placehold.co/400x300/FFE135/000?text=Nuoc+Chanh", true, false, "Nước Chanh", 20000m, 100, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "StoreLocations");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
