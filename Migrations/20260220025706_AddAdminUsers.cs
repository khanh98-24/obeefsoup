using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OBeefSoup.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUsers", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AdminUsers",
                columns: new[] { "Id", "CreatedDate", "FullName", "IsActive", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Quản Trị Viên", true, "F65121EE9DA8E3AEEFBA5B0A795D8D851F07EF218808764179E8AEACE08EE132", "Admin", "admin" },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Quản Lý Nội Dung", true, "57EB76546AEF06B06AE1941B25E76573D35CD29BCE27A66BE14271823BDC8BA4", "Manager", "manager" }
                });

            migrationBuilder.UpdateData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 9, 57, 4, 810, DateTimeKind.Local).AddTicks(9034));

            migrationBuilder.UpdateData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 9, 57, 4, 810, DateTimeKind.Local).AddTicks(9040));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 9, 57, 4, 810, DateTimeKind.Local).AddTicks(8907));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 9, 57, 4, 810, DateTimeKind.Local).AddTicks(8911));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 9, 57, 4, 810, DateTimeKind.Local).AddTicks(8913));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 9, 57, 4, 810, DateTimeKind.Local).AddTicks(8915));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 9, 57, 4, 810, DateTimeKind.Local).AddTicks(8917));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 9, 57, 4, 810, DateTimeKind.Local).AddTicks(8919));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 9, 57, 4, 810, DateTimeKind.Local).AddTicks(8921));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 9, 57, 4, 810, DateTimeKind.Local).AddTicks(8923));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 9, 57, 4, 810, DateTimeKind.Local).AddTicks(8925));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 9, 57, 4, 810, DateTimeKind.Local).AddTicks(8927));

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 9, 57, 4, 810, DateTimeKind.Local).AddTicks(9065));

            migrationBuilder.UpdateData(
                table: "StoreLocations",
                keyColumn: "Id",
                keyValue: 1,
                column: "MapUrl",
                value: "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3919.513274291583!2d106.70119131533413!3d10.771143262222383!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x31752f40a3b49e59%3A0xe27581172a39a037!2zMTIzIMSQLiBOZ3V54buFbiBIdeG7hywgQuG6v24gTmdow6ks QuG6rW4gMSwgVGjDoW5oIHBo4buSIEjhu5MgQ2jDrSBNaW5oLCBWaWV0bmFt!5e0!3m2!1sen!2s!4v1645084931345!5m2!1sen!2s");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminUsers");

            migrationBuilder.UpdateData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 12, 0, 20, 28, 8, DateTimeKind.Local).AddTicks(8457));

            migrationBuilder.UpdateData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 12, 0, 20, 28, 8, DateTimeKind.Local).AddTicks(8459));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 12, 0, 20, 28, 8, DateTimeKind.Local).AddTicks(8378));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 12, 0, 20, 28, 8, DateTimeKind.Local).AddTicks(8382));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 12, 0, 20, 28, 8, DateTimeKind.Local).AddTicks(8385));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 12, 0, 20, 28, 8, DateTimeKind.Local).AddTicks(8387));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 12, 0, 20, 28, 8, DateTimeKind.Local).AddTicks(8389));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 12, 0, 20, 28, 8, DateTimeKind.Local).AddTicks(8391));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 12, 0, 20, 28, 8, DateTimeKind.Local).AddTicks(8393));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 12, 0, 20, 28, 8, DateTimeKind.Local).AddTicks(8395));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 12, 0, 20, 28, 8, DateTimeKind.Local).AddTicks(8397));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 12, 0, 20, 28, 8, DateTimeKind.Local).AddTicks(8399));

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 12, 0, 20, 28, 8, DateTimeKind.Local).AddTicks(8483));

            migrationBuilder.UpdateData(
                table: "StoreLocations",
                keyColumn: "Id",
                keyValue: 1,
                column: "MapUrl",
                value: "");
        }
    }
}
