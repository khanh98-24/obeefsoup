using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OBeefSoup.Migrations
{
    /// <inheritdoc />
    public partial class AddBlogPosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPosts", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 17, 5, 12, 683, DateTimeKind.Local).AddTicks(1546));

            migrationBuilder.UpdateData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 17, 5, 12, 683, DateTimeKind.Local).AddTicks(1550));

            migrationBuilder.InsertData(
                table: "BlogPosts",
                columns: new[] { "Id", "Content", "CreatedDate", "DisplayOrder", "ImageUrl", "IsActive", "Slug", "Summary", "Title", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "Nội dung chi tiết về cách ninh xương, chọn gừng, hành tím nướng và các nguyên liệu thảo mộc quý hiếm...", new DateTime(2026, 2, 15, 17, 5, 12, 683, DateTimeKind.Local).AddTicks(2829), 1, "/images/486842042_1200614522067957_8652198815515987194_n.jpg", true, "bi-quyet-nuoc-dung-pho-bo", "Linh hồn của bát phở nằm ở nước dùng. Tại O'BeefSoup, chúng tôi ninh xương ống bò trong 12 tiếng cùng các gia vị tự nhiên...", "Bí quyết tạo nên nước dùng phở bò truyền thống", null },
                    { 2, "Chào đón sự kiện khai trương tưng bừng với nhiều phần quà hấp dẫn và chương trình âm nhạc đặc sắc...", new DateTime(2026, 2, 18, 17, 5, 12, 683, DateTimeKind.Local).AddTicks(2840), 2, "/images/486748406_1200441442085265_4112327303630339444_n.jpg", true, "khai-truong-co-so-moi", "Chào đón cơ sở thứ 3 của O'BeefSoup tại Cầu Giấy. Giảm ngay 20% trên tổng hóa đơn cho khách hàng check-in tại quán...", "Khai trương cơ sở mới - Nhận ngay ưu đãi 20%", null },
                    { 3, "Từ những gánh hàng rong xưa đến các cửa hàng sang trọng ngày nay, phở luôn giữ vững vị trí độc tôn...", new DateTime(2026, 2, 19, 17, 5, 12, 683, DateTimeKind.Local).AddTicks(2843), 3, "/images/484090098_1184094750386601_4967145700946842561_n.jpg", true, "pho-van-hoa-am-thuc-viet", "Phở không chỉ là một món ăn, nó là biểu tượng của tinh thần và văn hóa Việt Nam. Cùng khám phá hành trình của phở...", "Phở - Nét văn hóa ẩm thực tinh tế của người Việt", null }
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 17, 5, 12, 683, DateTimeKind.Local).AddTicks(1387));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 17, 5, 12, 683, DateTimeKind.Local).AddTicks(1392));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 17, 5, 12, 683, DateTimeKind.Local).AddTicks(1395));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 17, 5, 12, 683, DateTimeKind.Local).AddTicks(1398));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 17, 5, 12, 683, DateTimeKind.Local).AddTicks(1400));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 17, 5, 12, 683, DateTimeKind.Local).AddTicks(1403));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 17, 5, 12, 683, DateTimeKind.Local).AddTicks(1406));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 17, 5, 12, 683, DateTimeKind.Local).AddTicks(1410));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 17, 5, 12, 683, DateTimeKind.Local).AddTicks(1412));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 17, 5, 12, 683, DateTimeKind.Local).AddTicks(1415));

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 17, 5, 12, 683, DateTimeKind.Local).AddTicks(1601));

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 17, 5, 12, 683, DateTimeKind.Local).AddTicks(1605));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogPosts");

            migrationBuilder.UpdateData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 16, 21, 41, 105, DateTimeKind.Local).AddTicks(8406));

            migrationBuilder.UpdateData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 16, 21, 41, 105, DateTimeKind.Local).AddTicks(8409));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 16, 21, 41, 105, DateTimeKind.Local).AddTicks(8276));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 16, 21, 41, 105, DateTimeKind.Local).AddTicks(8279));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 16, 21, 41, 105, DateTimeKind.Local).AddTicks(8281));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 16, 21, 41, 105, DateTimeKind.Local).AddTicks(8282));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 16, 21, 41, 105, DateTimeKind.Local).AddTicks(8285));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 16, 21, 41, 105, DateTimeKind.Local).AddTicks(8287));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 16, 21, 41, 105, DateTimeKind.Local).AddTicks(8289));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 16, 21, 41, 105, DateTimeKind.Local).AddTicks(8291));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 16, 21, 41, 105, DateTimeKind.Local).AddTicks(8293));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 16, 21, 41, 105, DateTimeKind.Local).AddTicks(8295));

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 16, 21, 41, 105, DateTimeKind.Local).AddTicks(8433));

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 16, 21, 41, 105, DateTimeKind.Local).AddTicks(8435));
        }
    }
}
