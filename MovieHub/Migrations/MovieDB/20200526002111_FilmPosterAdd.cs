using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieHub.Migrations.MovieDB
{
    public partial class FilmPosterAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Poster",
                table: "Film",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Poster",
                table: "Film");
        }
    }
}
