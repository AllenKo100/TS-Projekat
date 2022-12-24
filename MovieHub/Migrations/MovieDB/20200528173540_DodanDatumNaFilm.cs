using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieHub.Migrations.MovieDB
{
    public partial class DodanDatumNaFilm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DatumIzlaska",
                table: "Film",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatumIzlaska",
                table: "Film");
        }
    }
}
