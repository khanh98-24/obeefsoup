using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OBeefSoup.Migrations
{
    /// <inheritdoc />
    public partial class CompleteAboutSection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IconClass",
                table: "AboutFeatures",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "IconImageUrl",
                table: "AboutFeatures",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AboutFeatures",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "IconImageUrl" },
                values: new object[] { new DateTime(2026, 2, 21, 18, 9, 49, 964, DateTimeKind.Local).AddTicks(9858), null });

            migrationBuilder.UpdateData(
                table: "AboutFeatures",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "IconImageUrl" },
                values: new object[] { new DateTime(2026, 2, 21, 18, 9, 49, 964, DateTimeKind.Local).AddTicks(9860), null });

            migrationBuilder.UpdateData(
                table: "AboutFeatures",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "IconImageUrl" },
                values: new object[] { new DateTime(2026, 2, 21, 18, 9, 49, 964, DateTimeKind.Local).AddTicks(9862), null });

            migrationBuilder.UpdateData(
                table: "AboutFeatures",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "IconImageUrl" },
                values: new object[] { new DateTime(2026, 2, 21, 18, 9, 49, 964, DateTimeKind.Local).AddTicks(9863), null });

            migrationBuilder.UpdateData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 18, 9, 49, 964, DateTimeKind.Local).AddTicks(9792));

            migrationBuilder.UpdateData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 18, 9, 49, 964, DateTimeKind.Local).AddTicks(9795));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 16, 18, 9, 49, 965, DateTimeKind.Local).AddTicks(269));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 19, 18, 9, 49, 965, DateTimeKind.Local).AddTicks(276));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 18, 9, 49, 965, DateTimeKind.Local).AddTicks(278));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 18, 9, 49, 964, DateTimeKind.Local).AddTicks(9697));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 18, 9, 49, 964, DateTimeKind.Local).AddTicks(9701));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 18, 9, 49, 964, DateTimeKind.Local).AddTicks(9703));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 18, 9, 49, 964, DateTimeKind.Local).AddTicks(9705));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 18, 9, 49, 964, DateTimeKind.Local).AddTicks(9707));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 18, 9, 49, 964, DateTimeKind.Local).AddTicks(9709));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 18, 9, 49, 964, DateTimeKind.Local).AddTicks(9712));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 18, 9, 49, 964, DateTimeKind.Local).AddTicks(9714));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 18, 9, 49, 964, DateTimeKind.Local).AddTicks(9716));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 18, 9, 49, 964, DateTimeKind.Local).AddTicks(9718));

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 18, 9, 49, 964, DateTimeKind.Local).AddTicks(9820));

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 18, 9, 49, 964, DateTimeKind.Local).AddTicks(9822));

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 18, 9, 49, 964, DateTimeKind.Local).AddTicks(9824));

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 18, 9, 49, 964, DateTimeKind.Local).AddTicks(9825));

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 18, 9, 49, 964, DateTimeKind.Local).AddTicks(9827));

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 18, 9, 49, 964, DateTimeKind.Local).AddTicks(9828));

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 18, 9, 49, 964, DateTimeKind.Local).AddTicks(9830));

            migrationBuilder.UpdateData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2026, 2, 6, 18, 9, 49, 965, DateTimeKind.Local).AddTicks(344));

            migrationBuilder.UpdateData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 2,
                column: "Date",
                value: new DateTime(2026, 2, 13, 18, 9, 49, 965, DateTimeKind.Local).AddTicks(346));

            migrationBuilder.UpdateData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 3,
                column: "Date",
                value: new DateTime(2026, 2, 18, 18, 9, 49, 965, DateTimeKind.Local).AddTicks(349));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconImageUrl",
                table: "AboutFeatures");

            migrationBuilder.AlterColumn<string>(
                name: "IconClass",
                table: "AboutFeatures",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AboutFeatures",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(5706));

            migrationBuilder.UpdateData(
                table: "AboutFeatures",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(5708));

            migrationBuilder.UpdateData(
                table: "AboutFeatures",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(5710));

            migrationBuilder.UpdateData(
                table: "AboutFeatures",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(5712));

            migrationBuilder.UpdateData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(5615));

            migrationBuilder.UpdateData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(5618));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 16, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(6206));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 19, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(6215));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(6217));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(5504));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(5508));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(5510));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(5513));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(5515));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(5518));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(5520));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(5522));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(5526));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(5529));

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(5662));

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(5665));

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(5667));

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(5668));

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(5670));

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(5672));

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(5674));

            migrationBuilder.UpdateData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2026, 2, 6, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(6292));

            migrationBuilder.UpdateData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 2,
                column: "Date",
                value: new DateTime(2026, 2, 13, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(6295));

            migrationBuilder.UpdateData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 3,
                column: "Date",
                value: new DateTime(2026, 2, 18, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(6297));
        }
    }
}
