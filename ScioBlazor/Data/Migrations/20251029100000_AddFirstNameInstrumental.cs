using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScioBlazor.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFirstNameInstrumental : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstNameInstrumental",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstNameInstrumental",
                table: "AspNetUsers");
        }
    }
}
