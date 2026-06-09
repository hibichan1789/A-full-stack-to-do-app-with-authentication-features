using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoAuthApi.Migrations
{
    /// <inheritdoc />
    public partial class Functions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "Todos",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Summary",
                table: "Todos");
        }
    }
}
