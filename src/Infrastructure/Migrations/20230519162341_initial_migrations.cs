using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial_migrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthorBook",
                columns: table => new
                {
                    BookGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Guid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorBook", x => new { x.AuthorGuid, x.BookGuid });
                });

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    AuthorGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorName = table.Column<string>(type: "text", nullable: false),
                    AuthorLastName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.AuthorGuid);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryGuid);
                });

            migrationBuilder.CreateTable(
                name: "Publishers",
                columns: table => new
                {
                    PublisherGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    PublisherName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publishers", x => x.PublisherGuid);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    BookName = table.Column<string>(type: "text", nullable: false),
                    ISBN = table.Column<string>(type: "text", nullable: false),
                    PagesCount = table.Column<int>(type: "integer", nullable: false),
                    BookPictureURL = table.Column<string>(type: "text", nullable: true),
                    PublishYear = table.Column<string>(type: "text", nullable: false),
                    PublisherGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryGuid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookGuid);
                    table.ForeignKey(
                        name: "FK_Books_Categories_CategoryGuid",
                        column: x => x.CategoryGuid,
                        principalTable: "Categories",
                        principalColumn: "CategoryGuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Books_Publishers_PublisherGuid",
                        column: x => x.PublisherGuid,
                        principalTable: "Publishers",
                        principalColumn: "PublisherGuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AuthorBook",
                columns: new[] { "AuthorGuid", "BookGuid", "Guid" },
                values: new object[] { new Guid("d791c427-0df6-410f-bf4a-bf623fe73888"), new Guid("cb961ea8-3605-45dd-b590-7ce3a255ac6c"), new Guid("8af1bf87-a423-4795-af25-afbb6662b35d") });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "AuthorGuid", "AuthorLastName", "AuthorName" },
                values: new object[] { new Guid("d791c427-0df6-410f-bf4a-bf623fe73888"), "Kanneman", "Daniel" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryGuid", "CategoryName" },
                values: new object[] { new Guid("c6debc11-73f4-4bed-8c94-9dff15ceee17"), "Non-fiction" });

            migrationBuilder.InsertData(
                table: "Publishers",
                columns: new[] { "PublisherGuid", "PublisherName" },
                values: new object[] { new Guid("8441c32a-8f7c-4c72-8b38-ff2a35a43284"), "Farrar, Straus and Giroux" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookGuid", "BookName", "BookPictureURL", "CategoryGuid", "ISBN", "PagesCount", "PublishYear", "PublisherGuid" },
                values: new object[] { new Guid("cb961ea8-3605-45dd-b590-7ce3a255ac6c"), "Thinking Fast and Slow", null, new Guid("c6debc11-73f4-4bed-8c94-9dff15ceee17"), "9780606275644", 499, "2011", new Guid("8441c32a-8f7c-4c72-8b38-ff2a35a43284") });

            migrationBuilder.CreateIndex(
                name: "IX_Books_CategoryGuid",
                table: "Books",
                column: "CategoryGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Books_PublisherGuid",
                table: "Books",
                column: "PublisherGuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorBook");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Publishers");
        }
    }
}
