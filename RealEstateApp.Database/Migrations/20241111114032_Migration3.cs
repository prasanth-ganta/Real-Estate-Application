using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateApp.Database.Migrations
{
    /// <inheritdoc />
    public partial class Migration3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "Password", "UserName" },
                values: new object[] { "$2a$11$hTU.ergWzllbpj69950Hsu/KgPWB4BFR4kkExPJknlfMqflaeJljO", "abdul" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$KpkxCo9X.Y6bhaquKA4lQOT5zb0.N5.gbCHsmL4WRN3Hh4BRjwK92");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "Password", "UserName" },
                values: new object[] { "Abdul@123", "$2a$11$mev4uhc7JYVFPfTDj.SgGeBVVNGc1Xjn58QMnTbERugZ8nJgzcrzW" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$02OtJ5.d8tmglZ2076gpUOYhTjImxBCTBNfqJFxv3YVkr5uXKvm7u");
        }
    }
}
