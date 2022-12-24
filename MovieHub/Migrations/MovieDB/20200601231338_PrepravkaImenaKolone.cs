using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieHub.Migrations.MovieDB
{
    public partial class PrepravkaImenaKolone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Glumnci",
                table: "Film");

            migrationBuilder.AddColumn<string>(
                name: "Glumci",
                table: "Film",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Glumci",
                table: "Film");

            migrationBuilder.AddColumn<string>(
                name: "Glumnci",
                table: "Film",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
