using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateApp.Database.Migrations
{
    /// <inheritdoc />
    public partial class Migrations5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Properties_PropertyId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_ReceiverId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_SenderId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_ApprovalStatuses_ApprovalStatusId",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Locations_LocationId",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_PropertyStatuses_PropertyStatusId",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_PropertySubTypes_SubPropertyTypeId",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_PropertyTypes_PropertyTypeId",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Users_OwnerId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Properties");

            migrationBuilder.RenameColumn(
                name: "SubPropertyTypeId",
                table: "Properties",
                newName: "SubPropertyTypeID");

            migrationBuilder.RenameColumn(
                name: "PropertyTypeId",
                table: "Properties",
                newName: "PropertyTypeID");

            migrationBuilder.RenameColumn(
                name: "PropertyStatusId",
                table: "Properties",
                newName: "PropertyStatusID");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Properties",
                newName: "OwnerID");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "Properties",
                newName: "LocationID");

            migrationBuilder.RenameColumn(
                name: "ApprovalStatusId",
                table: "Properties",
                newName: "ApprovalStatusID");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_SubPropertyTypeId",
                table: "Properties",
                newName: "IX_Properties_SubPropertyTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_PropertyTypeId",
                table: "Properties",
                newName: "IX_Properties_PropertyTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_PropertyStatusId",
                table: "Properties",
                newName: "IX_Properties_PropertyStatusID");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_OwnerId",
                table: "Properties",
                newName: "IX_Properties_OwnerID");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_LocationId",
                table: "Properties",
                newName: "IX_Properties_LocationID");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_ApprovalStatusId",
                table: "Properties",
                newName: "IX_Properties_ApprovalStatusID");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Messages",
                newName: "SenderID");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "Messages",
                newName: "ReceiverID");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                newName: "IX_Messages_SenderID");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_ReceiverId",
                table: "Messages",
                newName: "IX_Messages_ReceiverID");

            migrationBuilder.RenameColumn(
                name: "PropertyId",
                table: "Documents",
                newName: "PropertyID");

            migrationBuilder.RenameIndex(
                name: "IX_Documents_PropertyId",
                table: "Documents",
                newName: "IX_Documents_PropertyID");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$9MXf7CpycH.NeSkAzRG9o.Rt5NJF8ie3O6O8.m0AHERSpQKGKWM8e");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$3YE1ezajrzyUc1BlEbRjN.wl4l/nykCP6Zm9NLmCIseOcyruDN4GO");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Properties_PropertyID",
                table: "Documents",
                column: "PropertyID",
                principalTable: "Properties",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_ReceiverID",
                table: "Messages",
                column: "ReceiverID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_SenderID",
                table: "Messages",
                column: "SenderID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_ApprovalStatuses_ApprovalStatusID",
                table: "Properties",
                column: "ApprovalStatusID",
                principalTable: "ApprovalStatuses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Locations_LocationID",
                table: "Properties",
                column: "LocationID",
                principalTable: "Locations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_PropertyStatuses_PropertyStatusID",
                table: "Properties",
                column: "PropertyStatusID",
                principalTable: "PropertyStatuses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_PropertySubTypes_SubPropertyTypeID",
                table: "Properties",
                column: "SubPropertyTypeID",
                principalTable: "PropertySubTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_PropertyTypes_PropertyTypeID",
                table: "Properties",
                column: "PropertyTypeID",
                principalTable: "PropertyTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Users_OwnerID",
                table: "Properties",
                column: "OwnerID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Properties_PropertyID",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_ReceiverID",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_SenderID",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_ApprovalStatuses_ApprovalStatusID",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Locations_LocationID",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_PropertyStatuses_PropertyStatusID",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_PropertySubTypes_SubPropertyTypeID",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_PropertyTypes_PropertyTypeID",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Users_OwnerID",
                table: "Properties");

            migrationBuilder.RenameColumn(
                name: "SubPropertyTypeID",
                table: "Properties",
                newName: "SubPropertyTypeId");

            migrationBuilder.RenameColumn(
                name: "PropertyTypeID",
                table: "Properties",
                newName: "PropertyTypeId");

            migrationBuilder.RenameColumn(
                name: "PropertyStatusID",
                table: "Properties",
                newName: "PropertyStatusId");

            migrationBuilder.RenameColumn(
                name: "OwnerID",
                table: "Properties",
                newName: "OwnerId");

            migrationBuilder.RenameColumn(
                name: "LocationID",
                table: "Properties",
                newName: "LocationId");

            migrationBuilder.RenameColumn(
                name: "ApprovalStatusID",
                table: "Properties",
                newName: "ApprovalStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_SubPropertyTypeID",
                table: "Properties",
                newName: "IX_Properties_SubPropertyTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_PropertyTypeID",
                table: "Properties",
                newName: "IX_Properties_PropertyTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_PropertyStatusID",
                table: "Properties",
                newName: "IX_Properties_PropertyStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_OwnerID",
                table: "Properties",
                newName: "IX_Properties_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_LocationID",
                table: "Properties",
                newName: "IX_Properties_LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_ApprovalStatusID",
                table: "Properties",
                newName: "IX_Properties_ApprovalStatusId");

            migrationBuilder.RenameColumn(
                name: "SenderID",
                table: "Messages",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "ReceiverID",
                table: "Messages",
                newName: "ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_SenderID",
                table: "Messages",
                newName: "IX_Messages_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_ReceiverID",
                table: "Messages",
                newName: "IX_Messages_ReceiverId");

            migrationBuilder.RenameColumn(
                name: "PropertyID",
                table: "Documents",
                newName: "PropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_Documents_PropertyID",
                table: "Documents",
                newName: "IX_Documents_PropertyId");

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Properties_PropertyId",
                table: "Documents",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_ReceiverId",
                table: "Messages",
                column: "ReceiverId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_SenderId",
                table: "Messages",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_ApprovalStatuses_ApprovalStatusId",
                table: "Properties",
                column: "ApprovalStatusId",
                principalTable: "ApprovalStatuses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Locations_LocationId",
                table: "Properties",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_PropertyStatuses_PropertyStatusId",
                table: "Properties",
                column: "PropertyStatusId",
                principalTable: "PropertyStatuses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_PropertySubTypes_SubPropertyTypeId",
                table: "Properties",
                column: "SubPropertyTypeId",
                principalTable: "PropertySubTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_PropertyTypes_PropertyTypeId",
                table: "Properties",
                column: "PropertyTypeId",
                principalTable: "PropertyTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Users_OwnerId",
                table: "Properties",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
