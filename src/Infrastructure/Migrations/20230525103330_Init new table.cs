using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initnewtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileURL",
                table: "Books");

            migrationBuilder.AddColumn<Guid>(
                name: "UserGuid",
                table: "Books",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FavoriteBooks",
                columns: table => new
                {
                    BookId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BookEntityBookGuid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteBooks", x => new { x.BookId, x.UserId });
                    table.ForeignKey(
                        name: "FK_FavoriteBooks_Books_BookEntityBookGuid",
                        column: x => x.BookEntityBookGuid,
                        principalTable: "Books",
                        principalColumn: "BookGuid");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "bytea", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "bytea", nullable: true),
                    Role = table.Column<string>(type: "text", nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    TokenCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TokenExpires = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SocialId = table.Column<string>(type: "text", nullable: true),
                    AuthVia = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserGuid);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_UserGuid",
                table: "Books",
                column: "UserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteBooks_BookEntityBookGuid",
                table: "FavoriteBooks",
                column: "BookEntityBookGuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Users_UserGuid",
                table: "Books",
                column: "UserGuid",
                principalTable: "Users",
                principalColumn: "UserGuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Users_UserGuid",
                table: "Books");

            migrationBuilder.DropTable(
                name: "FavoriteBooks");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Books_UserGuid",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "UserGuid",
                table: "Books");

            migrationBuilder.AddColumn<string>(
                name: "FileURL",
                table: "Books",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
