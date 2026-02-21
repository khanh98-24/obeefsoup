using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OBeefSoup.Migrations
{
    /// <inheritdoc />
    public partial class AddTestimonialsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Testimonials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Testimonials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Testimonials_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 14, 3, 44, 215, DateTimeKind.Local).AddTicks(5499));

            migrationBuilder.UpdateData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 14, 3, 44, 215, DateTimeKind.Local).AddTicks(5503));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 16, 14, 3, 44, 215, DateTimeKind.Local).AddTicks(6122));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 19, 14, 3, 44, 215, DateTimeKind.Local).AddTicks(6132));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 14, 3, 44, 215, DateTimeKind.Local).AddTicks(6136));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 14, 3, 44, 215, DateTimeKind.Local).AddTicks(5363));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 14, 3, 44, 215, DateTimeKind.Local).AddTicks(5368));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 14, 3, 44, 215, DateTimeKind.Local).AddTicks(5372));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 14, 3, 44, 215, DateTimeKind.Local).AddTicks(5375));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 14, 3, 44, 215, DateTimeKind.Local).AddTicks(5378));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 14, 3, 44, 215, DateTimeKind.Local).AddTicks(5381));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 14, 3, 44, 215, DateTimeKind.Local).AddTicks(5385));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 14, 3, 44, 215, DateTimeKind.Local).AddTicks(5388));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 14, 3, 44, 215, DateTimeKind.Local).AddTicks(5391));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 14, 3, 44, 215, DateTimeKind.Local).AddTicks(5394));

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 14, 3, 44, 215, DateTimeKind.Local).AddTicks(5536));

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 14, 3, 44, 215, DateTimeKind.Local).AddTicks(5540));

            migrationBuilder.InsertData(
                table: "Testimonials",
                columns: new[] { "Id", "Comment", "CustomerId", "CustomerName", "Date", "IsApproved", "Rating" },
                values: new object[,]
                {
                    { 1, "Nước dùng đậm đà, thịt bò tươi ngon. Đây là quán phở ngon nhất tôi từng ăn!", 1, "Nguyễn Văn A", new DateTime(2026, 2, 6, 14, 3, 44, 215, DateTimeKind.Local).AddTicks(6213), true, 5 },
                    { 2, "Không gian sang trọng, phục vụ chu đáo. Phở đặc biệt rất xứng đáng!", 1, "Trần Thị B", new DateTime(2026, 2, 13, 14, 3, 44, 215, DateTimeKind.Local).AddTicks(6217), true, 5 },
                    { 3, "Giá hơi cao nhưng chất lượng tuyệt vời. Sẽ quay lại!", 1, "Lê Minh C", new DateTime(2026, 2, 18, 14, 3, 44, 215, DateTimeKind.Local).AddTicks(6220), true, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Testimonials_CustomerId",
                table: "Testimonials",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Testimonials");

            migrationBuilder.UpdateData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 11, 22, 44, 56, DateTimeKind.Local).AddTicks(5131));

            migrationBuilder.UpdateData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 11, 22, 44, 56, DateTimeKind.Local).AddTicks(5133));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 16, 11, 22, 44, 56, DateTimeKind.Local).AddTicks(5493));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 19, 11, 22, 44, 56, DateTimeKind.Local).AddTicks(5499));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 11, 22, 44, 56, DateTimeKind.Local).AddTicks(5532));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 11, 22, 44, 56, DateTimeKind.Local).AddTicks(5049));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 11, 22, 44, 56, DateTimeKind.Local).AddTicks(5053));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 11, 22, 44, 56, DateTimeKind.Local).AddTicks(5055));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 11, 22, 44, 56, DateTimeKind.Local).AddTicks(5057));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 11, 22, 44, 56, DateTimeKind.Local).AddTicks(5059));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 11, 22, 44, 56, DateTimeKind.Local).AddTicks(5061));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 11, 22, 44, 56, DateTimeKind.Local).AddTicks(5064));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 11, 22, 44, 56, DateTimeKind.Local).AddTicks(5065));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 11, 22, 44, 56, DateTimeKind.Local).AddTicks(5068));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 11, 22, 44, 56, DateTimeKind.Local).AddTicks(5070));

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 11, 22, 44, 56, DateTimeKind.Local).AddTicks(5153));

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 11, 22, 44, 56, DateTimeKind.Local).AddTicks(5155));
        }
    }
}
