using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OBeefSoup.Migrations
{
    /// <inheritdoc />
    public partial class AddStoreLocationToJobPosting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StoreLocationId",
                table: "JobPostings",
                type: "int",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_JobPostings_StoreLocationId",
                table: "JobPostings",
                column: "StoreLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobPostings_StoreLocations_StoreLocationId",
                table: "JobPostings",
                column: "StoreLocationId",
                principalTable: "StoreLocations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobPostings_StoreLocations_StoreLocationId",
                table: "JobPostings");

            migrationBuilder.DropIndex(
                name: "IX_JobPostings_StoreLocationId",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "StoreLocationId",
                table: "JobPostings");

            migrationBuilder.UpdateData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 16, 11, 31, 807, DateTimeKind.Local).AddTicks(7402));

            migrationBuilder.UpdateData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 16, 11, 31, 807, DateTimeKind.Local).AddTicks(7404));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 16, 16, 11, 31, 807, DateTimeKind.Local).AddTicks(7776));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 19, 16, 11, 31, 807, DateTimeKind.Local).AddTicks(7782));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 20, 16, 11, 31, 807, DateTimeKind.Local).AddTicks(7784));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 16, 11, 31, 807, DateTimeKind.Local).AddTicks(7268));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 16, 11, 31, 807, DateTimeKind.Local).AddTicks(7271));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 16, 11, 31, 807, DateTimeKind.Local).AddTicks(7273));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 16, 11, 31, 807, DateTimeKind.Local).AddTicks(7275));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 16, 11, 31, 807, DateTimeKind.Local).AddTicks(7277));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 16, 11, 31, 807, DateTimeKind.Local).AddTicks(7279));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 16, 11, 31, 807, DateTimeKind.Local).AddTicks(7281));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 16, 11, 31, 807, DateTimeKind.Local).AddTicks(7282));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 16, 11, 31, 807, DateTimeKind.Local).AddTicks(7284));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 16, 11, 31, 807, DateTimeKind.Local).AddTicks(7287));

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 16, 11, 31, 807, DateTimeKind.Local).AddTicks(7423));

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 21, 16, 11, 31, 807, DateTimeKind.Local).AddTicks(7425));

            migrationBuilder.UpdateData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2026, 2, 6, 16, 11, 31, 807, DateTimeKind.Local).AddTicks(7831));

            migrationBuilder.UpdateData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 2,
                column: "Date",
                value: new DateTime(2026, 2, 13, 16, 11, 31, 807, DateTimeKind.Local).AddTicks(7834));

            migrationBuilder.UpdateData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 3,
                column: "Date",
                value: new DateTime(2026, 2, 18, 16, 11, 31, 807, DateTimeKind.Local).AddTicks(7836));
        }
    }
}
