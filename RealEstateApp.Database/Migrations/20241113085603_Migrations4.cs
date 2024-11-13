using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateApp.Database.Migrations
{
    /// <inheritdoc />
    public partial class Migrations4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$7c7/YBfxb4MYii92esnse.9WFok1oODLis9dpepxKmEf91.1CoPs2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$iXRGtN94MqwH08PPBrYhJeUhV9aKR593v85oNCa8wXP/73Ql85JCm");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
