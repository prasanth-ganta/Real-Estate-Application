using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateApp.Database.Migrations
{
    /// <inheritdoc />
    public partial class Migration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 1,
                column: "UserName",
                value: "$2a$11$mev4uhc7JYVFPfTDj.SgGeBVVNGc1Xjn58QMnTbERugZ8nJgzcrzW");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$02OtJ5.d8tmglZ2076gpUOYhTjImxBCTBNfqJFxv3YVkr5uXKvm7u");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 1,
                column: "UserName",
                value: "abdul");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 2,
                column: "Password",
                value: "Prashanth@123");
        }
    }
}
