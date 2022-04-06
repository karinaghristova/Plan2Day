using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plan2Day.Infrastructure.Data.Migrations
{
    public partial class BooksMoviesAndGenresAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookGenres",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookGenres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Isbn13 = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Pages = table.Column<int>(type: "int", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MovieGenres",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieGenres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImdbOriginalId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Runtime = table.Column<int>(type: "int", nullable: false),
                    Plot = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookBookGenre",
                columns: table => new
                {
                    BooksId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GenresId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookBookGenre", x => new { x.BooksId, x.GenresId });
                    table.ForeignKey(
                        name: "FK_BookBookGenre_BookGenres_GenresId",
                        column: x => x.GenresId,
                        principalTable: "BookGenres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookBookGenre_Books_BooksId",
                        column: x => x.BooksId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieMovieGenre",
                columns: table => new
                {
                    GenresId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MoviesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieMovieGenre", x => new { x.GenresId, x.MoviesId });
                    table.ForeignKey(
                        name: "FK_MovieMovieGenre_MovieGenres_GenresId",
                        column: x => x.GenresId,
                        principalTable: "MovieGenres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieMovieGenre_Movies_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookBookGenre_GenresId",
                table: "BookBookGenre",
                column: "GenresId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieMovieGenre_MoviesId",
                table: "MovieMovieGenre",
                column: "MoviesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookBookGenre");

            migrationBuilder.DropTable(
                name: "MovieMovieGenre");

            migrationBuilder.DropTable(
                name: "BookGenres");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "MovieGenres");

            migrationBuilder.DropTable(
                name: "Movies");
        }
    }
}
