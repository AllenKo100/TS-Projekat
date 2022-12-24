using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieHub.Migrations.MovieDB
{
    public partial class GlumacOmiljeniFilmAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Glumac",
                columns: table => new
                {
                    GlumacId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImeIPrezime = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Glumac", x => x.GlumacId);
                });

            migrationBuilder.CreateTable(
                name: "OmiljeniFilmovi",
                columns: table => new
                {
                    OmiljeniFilmoviID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilmID = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: false),
                    RegistrovaniKorisnikId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OmiljeniFilmovi", x => x.OmiljeniFilmoviID);
                    table.ForeignKey(
                        name: "FK_OmiljeniFilmovi_Film_FilmID",
                        column: x => x.FilmID,
                        principalTable: "Film",
                        principalColumn: "FilmID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OmiljeniFilmovi_RegistrovaniKorisnik_RegistrovaniKorisnikId",
                        column: x => x.RegistrovaniKorisnikId,
                        principalTable: "RegistrovaniKorisnik",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FilmGlumac",
                columns: table => new
                {
                    FilmGlumacID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilmID = table.Column<int>(nullable: false),
                    GlumacId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmGlumac", x => x.FilmGlumacID);
                    table.ForeignKey(
                        name: "FK_FilmGlumac_Film_FilmID",
                        column: x => x.FilmID,
                        principalTable: "Film",
                        principalColumn: "FilmID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmGlumac_Glumac_GlumacId",
                        column: x => x.GlumacId,
                        principalTable: "Glumac",
                        principalColumn: "GlumacId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilmGlumac_FilmID",
                table: "FilmGlumac",
                column: "FilmID");

            migrationBuilder.CreateIndex(
                name: "IX_FilmGlumac_GlumacId",
                table: "FilmGlumac",
                column: "GlumacId");

            migrationBuilder.CreateIndex(
                name: "IX_OmiljeniFilmovi_FilmID",
                table: "OmiljeniFilmovi",
                column: "FilmID");

            migrationBuilder.CreateIndex(
                name: "IX_OmiljeniFilmovi_RegistrovaniKorisnikId",
                table: "OmiljeniFilmovi",
                column: "RegistrovaniKorisnikId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilmGlumac");

            migrationBuilder.DropTable(
                name: "OmiljeniFilmovi");

            migrationBuilder.DropTable(
                name: "Glumac");
        }
    }
}
