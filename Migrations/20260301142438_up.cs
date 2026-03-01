using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OBeefSoup.Migrations
{
    /// <inheritdoc />
    public partial class up : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 3, 1, 21, 24, 36, 364, DateTimeKind.Local).AddTicks(1384));

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedDate",
                value: new DateTime(2026, 3, 1, 21, 24, 36, 364, DateTimeKind.Local).AddTicks(1426));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 25, 21, 30, 13, 378, DateTimeKind.Local).AddTicks(2892));

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 25, 21, 30, 13, 378, DateTimeKind.Local).AddTicks(2954));
        }
    }
}
