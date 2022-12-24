using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieHub.Migrations.MovieDB
{
    public partial class WatchlistAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegistrovaniKorisnik",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Ime = table.Column<string>(nullable: true),
                    Prezime = table.Column<string>(nullable: true),
                    AppUsername = table.Column<string>(nullable: true),
                    DatumRodjenja = table.Column<DateTime>(nullable: false),
                    SlikaProfila = table.Column<string>(nullable: true),
                    RokPretplate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrovaniKorisnik", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Watchlist",
                columns: table => new
                {
                    WatchlistID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(maxLength: 50, nullable: false),
                    UserID = table.Column<string>(nullable: true),
                    RegistrovaniKorisnikId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Watchlist", x => x.WatchlistID);
                    table.ForeignKey(
                        name: "FK_Watchlist_RegistrovaniKorisnik_RegistrovaniKorisnikId",
                        column: x => x.RegistrovaniKorisnikId,
                        principalTable: "RegistrovaniKorisnik",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WatchlistFilm",
                columns: table => new
                {
                    WatchlistFilmID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WatchlistID = table.Column<int>(nullable: false),
                    FilmId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchlistFilm", x => x.WatchlistFilmID);
                    table.ForeignKey(
                        name: "FK_WatchlistFilm_Film_FilmId",
                        column: x => x.FilmId,
                        principalTable: "Film",
                        principalColumn: "FilmID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WatchlistFilm_Watchlist_WatchlistID",
                        column: x => x.WatchlistID,
                        principalTable: "Watchlist",
                        principalColumn: "WatchlistID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Watchlist_RegistrovaniKorisnikId",
                table: "Watchlist",
                column: "RegistrovaniKorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_WatchlistFilm_FilmId",
                table: "WatchlistFilm",
                column: "FilmId");

            migrationBuilder.CreateIndex(
                name: "IX_WatchlistFilm_WatchlistID",
                table: "WatchlistFilm",
                column: "WatchlistID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WatchlistFilm");

            migrationBuilder.DropTable(
                name: "Watchlist");

            migrationBuilder.DropTable(
                name: "RegistrovaniKorisnik");
        }
    }
}
