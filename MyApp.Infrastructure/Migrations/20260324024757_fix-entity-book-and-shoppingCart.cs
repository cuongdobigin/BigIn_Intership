using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webApi.Migrations
{
    /// <inheritdoc />
    public partial class fixentitybookandshoppingCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCart_Account_AccountId",
                table: "ShoppingCart");

            migrationBuilder.DropTable(
                name: "BookShoppingCart");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCart_AccountId",
                table: "ShoppingCart");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "ShoppingCart",
                newName: "book_Id");

            migrationBuilder.RenameColumn(
                name: "Author",
                table: "Book",
                newName: "author");

            migrationBuilder.AddColumn<int>(
                name: "account_Id",
                table: "ShoppingCart",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "ShoppingCart",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "quantity",
                table: "Book",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCart_account_Id",
                table: "ShoppingCart",
                column: "account_Id");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCart_book_Id",
                table: "ShoppingCart",
                column: "book_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCart_Account_account_Id",
                table: "ShoppingCart",
                column: "account_Id",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCart_Book_book_Id",
                table: "ShoppingCart",
                column: "book_Id",
                principalTable: "Book",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCart_Account_account_Id",
                table: "ShoppingCart");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCart_Book_book_Id",
                table: "ShoppingCart");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCart_account_Id",
                table: "ShoppingCart");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCart_book_Id",
                table: "ShoppingCart");

            migrationBuilder.DropColumn(
                name: "account_Id",
                table: "ShoppingCart");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "ShoppingCart");

            migrationBuilder.DropColumn(
                name: "quantity",
                table: "Book");

            migrationBuilder.RenameColumn(
                name: "book_Id",
                table: "ShoppingCart",
                newName: "AccountId");

            migrationBuilder.RenameColumn(
                name: "author",
                table: "Book",
                newName: "Author");

            migrationBuilder.CreateTable(
                name: "BookShoppingCart",
                columns: table => new
                {
                    BooksId = table.Column<int>(type: "integer", nullable: false),
                    ShoppingCartsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookShoppingCart", x => new { x.BooksId, x.ShoppingCartsId });
                    table.ForeignKey(
                        name: "FK_BookShoppingCart_Book_BooksId",
                        column: x => x.BooksId,
                        principalTable: "Book",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookShoppingCart_ShoppingCart_ShoppingCartsId",
                        column: x => x.ShoppingCartsId,
                        principalTable: "ShoppingCart",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCart_AccountId",
                table: "ShoppingCart",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookShoppingCart_ShoppingCartsId",
                table: "BookShoppingCart",
                column: "ShoppingCartsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCart_Account_AccountId",
                table: "ShoppingCart",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
