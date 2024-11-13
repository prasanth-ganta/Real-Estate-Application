using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateApp.Database.Migrations
{
    /// <inheritdoc />
    public partial class Migrations3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Documents");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PropertySubTypes",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Documents",
                newName: "FileName");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$rE4I9icgwcrfGGQoI9jOteBLcRu4I56Hp8AETNZ1v2o1x8KAbtROy");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$jvc5UhOd.x/wRZaO2DQgxO0U8.CB1WMtG6Cm3v0p169ec.4Iq0zi.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "PropertySubTypes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Documents",
                newName: "Url");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Documents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$Ly0t/pj4AHbNpTy9W7wMaOT8FwDQUALX1x7MC9FAhIxicOdfHHMV2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$9KIX6OI9zu9x8t21o2ZyNumc9O8RIHhhzl4jcjcPX/f3p8SNYjtSW");
        }
    }
}
