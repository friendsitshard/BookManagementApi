using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookManagmentApi.Migrations
{
    /// <inheritdoc />
    public partial class Newindexinbookstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Books_ViewsCount",
                table: "Books");

            migrationBuilder.CreateIndex(
                name: "IX_Books_IsDeleted_ViewsCount_PublicationYear",
                table: "Books",
                columns: new[] { "IsDeleted", "ViewsCount", "PublicationYear" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Books_IsDeleted_ViewsCount_PublicationYear",
                table: "Books");

            migrationBuilder.CreateIndex(
                name: "IX_Books_ViewsCount",
                table: "Books",
                column: "ViewsCount");
        }
    }
}
