using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OBeefSoup.Migrations
{
    /// <inheritdoc />
    public partial class AddAboutSectionFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AboutFeatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IconClass = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutFeatures", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AboutFeatures",
                columns: new[] { "Id", "CreatedDate", "DisplayOrder", "IconClass", "IsActive", "Title", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 2, 21, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(5706), 1, "bi-clock-history", true, "Ninh 12 Tiếng", null },
                    { 2, new DateTime(2026, 2, 21, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(5708), 2, "bi-gem", true, "Nguyên Liệu", null },
                    { 3, new DateTime(2026, 2, 21, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(5710), 3, "bi-stars", true, "Đẳng Cấp", null },
                    { 4, new DateTime(2026, 2, 21, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(5712), 4, "bi-heart-fill", true, "Phục Vụ", null }
                });

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

            migrationBuilder.InsertData(
                table: "SiteSettings",
                columns: new[] { "Id", "CreatedDate", "Description", "IsActive", "Key", "UpdatedDate", "Value" },
                values: new object[,]
                {
                    { 3, new DateTime(2026, 2, 21, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(5667), "Tiêu đề nằm trên ảnh phần Giới thiệu", true, "AboutTitle1", null, "Tinh hoa TRUYỀN THỐNG & HIỆN ĐẠI hòa quyện" },
                    { 4, new DateTime(2026, 2, 21, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(5668), "Tên thương hiệu phần Giới thiệu", true, "AboutTitle2", null, "O' BeefSoup" },
                    { 5, new DateTime(2026, 2, 21, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(5670), "Tiêu đề phụ phần Giới thiệu", true, "AboutSubtitle", null, "CÂU CHUYỆN THƯƠNG HIỆU" },
                    { 6, new DateTime(2026, 2, 21, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(5672), "Mô tả chi tiết phần Giới thiệu", true, "AboutDescription", null, "Nơi hội tụ tinh hoa ẩm thực truyền thống Việt Nam với không gian hiện đại, sang trọng bậc nhất." },
                    { 7, new DateTime(2026, 2, 21, 17, 59, 9, 258, DateTimeKind.Local).AddTicks(5674), "Ảnh đại diện phần Giới thiệu", true, "AboutImageUrl", null, "/images/gioithieu.jpg" }
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutFeatures");

            migrationBuilder.DeleteData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.UpdateData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 16, 27, 12, 156, DateTimeKind.Local).AddTicks(3919));

            migrationBuilder.UpdateData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 16, 27, 12, 156, DateTimeKind.Local).AddTicks(3921));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 16, 16, 27, 12, 156, DateTimeKind.Local).AddTicks(4332));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 19, 16, 27, 12, 156, DateTimeKind.Local).AddTicks(4339));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 16, 27, 12, 156, DateTimeKind.Local).AddTicks(4341));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 16, 27, 12, 156, DateTimeKind.Local).AddTicks(3826));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 16, 27, 12, 156, DateTimeKind.Local).AddTicks(3830));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 16, 27, 12, 156, DateTimeKind.Local).AddTicks(3833));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 16, 27, 12, 156, DateTimeKind.Local).AddTicks(3835));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 16, 27, 12, 156, DateTimeKind.Local).AddTicks(3837));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 16, 27, 12, 156, DateTimeKind.Local).AddTicks(3840));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 16, 27, 12, 156, DateTimeKind.Local).AddTicks(3842));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 16, 27, 12, 156, DateTimeKind.Local).AddTicks(3844));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 16, 27, 12, 156, DateTimeKind.Local).AddTicks(3846));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 16, 27, 12, 156, DateTimeKind.Local).AddTicks(3848));

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 16, 27, 12, 156, DateTimeKind.Local).AddTicks(3949));

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 16, 27, 12, 156, DateTimeKind.Local).AddTicks(3951));

            migrationBuilder.UpdateData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2026, 2, 6, 16, 27, 12, 156, DateTimeKind.Local).AddTicks(4404));

            migrationBuilder.UpdateData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 2,
                column: "Date",
                value: new DateTime(2026, 2, 13, 16, 27, 12, 156, DateTimeKind.Local).AddTicks(4406));

            migrationBuilder.UpdateData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 3,
                column: "Date",
                value: new DateTime(2026, 2, 18, 16, 27, 12, 156, DateTimeKind.Local).AddTicks(4408));
        }
    }
}
