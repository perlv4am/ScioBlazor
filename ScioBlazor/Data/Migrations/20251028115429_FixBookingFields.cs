using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScioBlazor.Migrations
{
    /// <inheritdoc />
    public partial class FixBookingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndUtc",
                table: "Meetings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2000, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "AttendeeName",
                table: "Meetings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShareLinkId",
                table: "Meetings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_ShareLinkId",
                table: "Meetings",
                column: "ShareLinkId",
                unique: true,
                filter: "[ShareLinkId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_ShareLinks_ShareLinkId",
                table: "Meetings",
                column: "ShareLinkId",
                principalTable: "ShareLinks",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_ShareLinks_ShareLinkId",
                table: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_Meetings_ShareLinkId",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "AttendeeName",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "EndUtc",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "ShareLinkId",
                table: "Meetings");
        }
    }
}
