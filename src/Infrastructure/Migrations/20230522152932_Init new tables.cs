using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initnewtables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookGuid",
                keyValue: new Guid("cb961ea8-3605-45dd-b590-7ce3a255ac6c"));

            migrationBuilder.RenameColumn(
                name: "ISBN",
                table: "Books",
                newName: "Language");

            migrationBuilder.AddColumn<string>(
                name: "BookFakeName",
                table: "Books",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileURL",
                table: "Books",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ISBN10",
                table: "Books",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ISBN13",
                table: "Books",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryGuid",
                keyValue: new Guid("c6debc11-73f4-4bed-8c94-9dff15ceee17"),
                column: "CategoryName",
                value: "Computers - Programming");

            migrationBuilder.UpdateData(
                table: "Publishers",
                keyColumn: "PublisherGuid",
                keyValue: new Guid("8441c32a-8f7c-4c72-8b38-ff2a35a43284"),
                column: "PublisherName",
                value: "Manning Publications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookFakeName",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "FileURL",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "ISBN10",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "ISBN13",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "Language",
                table: "Books",
                newName: "ISBN");

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookGuid", "BookName", "BookPictureURL", "CategoryGuid", "ISBN", "PagesCount", "PublishYear", "PublisherGuid" },
                values: new object[] { new Guid("cb961ea8-3605-45dd-b590-7ce3a255ac6c"), "Thinking Fast and Slow", null, new Guid("c6debc11-73f4-4bed-8c94-9dff15ceee17"), "9780606275644", 499, "2011", new Guid("8441c32a-8f7c-4c72-8b38-ff2a35a43284") });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryGuid",
                keyValue: new Guid("c6debc11-73f4-4bed-8c94-9dff15ceee17"),
                column: "CategoryName",
                value: "Non-fiction");

            migrationBuilder.UpdateData(
                table: "Publishers",
                keyColumn: "PublisherGuid",
                keyValue: new Guid("8441c32a-8f7c-4c72-8b38-ff2a35a43284"),
                column: "PublisherName",
                value: "Farrar, Straus and Giroux");
        }
    }
}
