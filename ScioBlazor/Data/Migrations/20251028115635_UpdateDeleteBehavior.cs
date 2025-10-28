using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScioBlazor.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_ShareLinks_ShareLinkId",
                table: "Meetings");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_ShareLinks_ShareLinkId",
                table: "Meetings",
                column: "ShareLinkId",
                principalTable: "ShareLinks",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
