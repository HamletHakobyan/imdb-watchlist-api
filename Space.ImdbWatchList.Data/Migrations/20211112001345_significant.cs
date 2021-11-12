using Microsoft.EntityFrameworkCore.Migrations;

namespace Space.ImdbWatchList.Data.Migrations
{
    public partial class significant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Poster_Films_FilmId",
                table: "Poster");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Poster",
                table: "Poster");

            migrationBuilder.RenameTable(
                name: "Poster",
                newName: "Posters");

            migrationBuilder.RenameIndex(
                name: "IX_Poster_FilmId",
                table: "Posters",
                newName: "IX_Posters_FilmId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Posters",
                table: "Posters",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_WatchLists_UserId",
                table: "WatchLists",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posters_Films_FilmId",
                table: "Posters",
                column: "FilmId",
                principalTable: "Films",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posters_Films_FilmId",
                table: "Posters");

            migrationBuilder.DropIndex(
                name: "IX_WatchLists_UserId",
                table: "WatchLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Posters",
                table: "Posters");

            migrationBuilder.RenameTable(
                name: "Posters",
                newName: "Poster");

            migrationBuilder.RenameIndex(
                name: "IX_Posters_FilmId",
                table: "Poster",
                newName: "IX_Poster_FilmId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Poster",
                table: "Poster",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Poster_Films_FilmId",
                table: "Poster",
                column: "FilmId",
                principalTable: "Films",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
