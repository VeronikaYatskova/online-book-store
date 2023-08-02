using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteBooks");

            migrationBuilder.CreateTable(
                name: "UserBooks",
                columns: table => new
                {
                    BookId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: true),
                    BookEntityBookGuid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBooks", x => new { x.BookId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserBooks_Books_BookEntityBookGuid",
                        column: x => x.BookEntityBookGuid,
                        principalTable: "Books",
                        principalColumn: "BookGuid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBooks_BookEntityBookGuid",
                table: "UserBooks",
                column: "BookEntityBookGuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBooks");

            migrationBuilder.CreateTable(
                name: "FavoriteBooks",
                columns: table => new
                {
                    BookId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    BookEntityBookGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    Id = table.Column<Guid>(type: "uuid", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteBooks_BookEntityBookGuid",
                table: "FavoriteBooks",
                column: "BookEntityBookGuid");
        }
    }
}
