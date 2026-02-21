using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OBeefSoup.Migrations
{
    /// <inheritdoc />
    public partial class AddMenuItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AdminUsers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AdminUsers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.CreateTable(
                name: "MenuItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuItems_MenuItems_ParentId",
                        column: x => x.ParentId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 15, 35, 6, 997, DateTimeKind.Local).AddTicks(7704));

            migrationBuilder.UpdateData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 15, 35, 6, 997, DateTimeKind.Local).AddTicks(7706));

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "DisplayOrder", "Icon", "IsActive", "ParentId", "Title", "Url" },
                values: new object[,]
                {
                    { 1, 1, null, true, null, "Trang Chủ", "/" },
                    { 2, 2, null, true, null, "Giới Thiệu", "/#about" },
                    { 3, 3, null, true, null, "Thực Đơn", "/Menu" },
                    { 4, 4, null, true, null, "Ưu Điểm", "/#why-us" },
                    { 5, 5, null, true, null, "Đánh Giá", "/#testimonials" }
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 15, 35, 6, 997, DateTimeKind.Local).AddTicks(7627));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 15, 35, 6, 997, DateTimeKind.Local).AddTicks(7629));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 15, 35, 6, 997, DateTimeKind.Local).AddTicks(7632));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 15, 35, 6, 997, DateTimeKind.Local).AddTicks(7634));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 15, 35, 6, 997, DateTimeKind.Local).AddTicks(7636));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 15, 35, 6, 997, DateTimeKind.Local).AddTicks(7638));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 15, 35, 6, 997, DateTimeKind.Local).AddTicks(7640));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 15, 35, 6, 997, DateTimeKind.Local).AddTicks(7641));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 15, 35, 6, 997, DateTimeKind.Local).AddTicks(7643));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 15, 35, 6, 997, DateTimeKind.Local).AddTicks(7646));

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "DisplayOrder", "Icon", "IsActive", "ParentId", "Title", "Url" },
                values: new object[,]
                {
                    { 6, 1, null, true, 3, "Phở Bò", "/Menu#category-1" },
                    { 7, 2, null, true, 3, "Phở Gà", "/Menu#category-2" },
                    { 8, 3, null, true, 3, "Đồ Uống", "/Menu#category-3" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_ParentId",
                table: "MenuItems",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuItems");

            migrationBuilder.InsertData(
                table: "AdminUsers",
                columns: new[] { "Id", "CreatedDate", "Email", "FullName", "IsActive", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@obeefsoup.vn", "Quản Trị Viên", true, "F65121EE9DA8E3AEEFBA5B0A795D8D851F07EF218808764179E8AEACE08EE132", "Admin", "admin" },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "manager@obeefsoup.vn", "Quản Lý Nội Dung", true, "57EB76546AEF06B06AE1941B25E76573D35CD29BCE27A66BE14271823BDC8BA4", "Manager", "manager" }
                });

            migrationBuilder.UpdateData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 10, 26, 52, 56, DateTimeKind.Local).AddTicks(3640));

            migrationBuilder.UpdateData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 10, 26, 52, 56, DateTimeKind.Local).AddTicks(3643));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 10, 26, 52, 56, DateTimeKind.Local).AddTicks(3564));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 10, 26, 52, 56, DateTimeKind.Local).AddTicks(3567));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 10, 26, 52, 56, DateTimeKind.Local).AddTicks(3569));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 10, 26, 52, 56, DateTimeKind.Local).AddTicks(3571));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 10, 26, 52, 56, DateTimeKind.Local).AddTicks(3573));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 10, 26, 52, 56, DateTimeKind.Local).AddTicks(3576));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 10, 26, 52, 56, DateTimeKind.Local).AddTicks(3578));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 10, 26, 52, 56, DateTimeKind.Local).AddTicks(3580));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 10, 26, 52, 56, DateTimeKind.Local).AddTicks(3582));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 10, 26, 52, 56, DateTimeKind.Local).AddTicks(3584));

            migrationBuilder.InsertData(
                table: "SiteSettings",
                columns: new[] { "Id", "CreatedDate", "Description", "IsActive", "Key", "UpdatedDate", "Value" },
                values: new object[] { 1, new DateTime(2026, 2, 20, 10, 26, 52, 56, DateTimeKind.Local).AddTicks(3666), "Ảnh nền cho phần thực đơn trang chủ", true, "MenuBackgroundImage", null, "/images/menu-background.jpg" });
        }
    }
}
