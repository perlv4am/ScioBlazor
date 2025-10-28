using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScioBlazor.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAttendeeEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AttendeeEmail",
                table: "Meetings",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttendeeEmail",
                table: "Meetings");
        }
    }
}
