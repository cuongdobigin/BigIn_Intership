using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace webApi.Migrations
{
    /// <inheritdoc />
    public partial class addcolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_TypeBook_id",
                table: "Book");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailOrder_Order_id",
                table: "DetailOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_Image_Book_id",
                table: "Image");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Account_id",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Discount_id",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_review_Account_id",
                table: "review");

            migrationBuilder.DropForeignKey(
                name: "FK_review_Book_id",
                table: "review");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCart_Account_id",
                table: "ShoppingCart");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Account_Id",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Users",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "ShoppingCart",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "ShoppingCart",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "review",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "review",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "book_Id",
                table: "review",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "Order",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Order",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "discount_id",
                table: "Order",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "Image",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "book_Id",
                table: "Image",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "DetailOrder",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "order_id",
                table: "DetailOrder",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "Book",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "type_book_Id",
                table: "Book",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_AccountId",
                table: "Users",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCart_AccountId",
                table: "ShoppingCart",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_review_AccountId",
                table: "review",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_review_book_Id",
                table: "review",
                column: "book_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_AccountId",
                table: "Order",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_discount_id",
                table: "Order",
                column: "discount_id");

            migrationBuilder.CreateIndex(
                name: "IX_Image_book_Id",
                table: "Image",
                column: "book_Id");

            migrationBuilder.CreateIndex(
                name: "IX_DetailOrder_order_id",
                table: "DetailOrder",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_Book_type_book_Id",
                table: "Book",
                column: "type_book_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_TypeBook_type_book_Id",
                table: "Book",
                column: "type_book_Id",
                principalTable: "TypeBook",
                principalColumn: "type_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DetailOrder_Order_order_id",
                table: "DetailOrder",
                column: "order_id",
                principalTable: "Order",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Book_book_Id",
                table: "Image",
                column: "book_Id",
                principalTable: "Book",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Account_AccountId",
                table: "Order",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Discount_discount_id",
                table: "Order",
                column: "discount_id",
                principalTable: "Discount",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_review_Account_AccountId",
                table: "review",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_review_Book_book_Id",
                table: "review",
                column: "book_Id",
                principalTable: "Book",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCart_Account_AccountId",
                table: "ShoppingCart",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Account_AccountId",
                table: "Users",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_TypeBook_type_book_Id",
                table: "Book");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailOrder_Order_order_id",
                table: "DetailOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_Image_Book_book_Id",
                table: "Image");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Account_AccountId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Discount_discount_id",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_review_Account_AccountId",
                table: "review");

            migrationBuilder.DropForeignKey(
                name: "FK_review_Book_book_Id",
                table: "review");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCart_Account_AccountId",
                table: "ShoppingCart");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Account_AccountId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AccountId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCart_AccountId",
                table: "ShoppingCart");

            migrationBuilder.DropIndex(
                name: "IX_review_AccountId",
                table: "review");

            migrationBuilder.DropIndex(
                name: "IX_review_book_Id",
                table: "review");

            migrationBuilder.DropIndex(
                name: "IX_Order_AccountId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_discount_id",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Image_book_Id",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_DetailOrder_order_id",
                table: "DetailOrder");

            migrationBuilder.DropIndex(
                name: "IX_Book_type_book_Id",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "ShoppingCart");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "review");

            migrationBuilder.DropColumn(
                name: "book_Id",
                table: "review");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "discount_id",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "book_Id",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "order_id",
                table: "DetailOrder");

            migrationBuilder.DropColumn(
                name: "type_book_Id",
                table: "Book");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Users",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "ShoppingCart",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "review",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "Order",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "Image",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "DetailOrder",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "Book",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_Book_TypeBook_id",
                table: "Book",
                column: "id",
                principalTable: "TypeBook",
                principalColumn: "type_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DetailOrder_Order_id",
                table: "DetailOrder",
                column: "id",
                principalTable: "Order",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Book_id",
                table: "Image",
                column: "id",
                principalTable: "Book",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Account_id",
                table: "Order",
                column: "id",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Discount_id",
                table: "Order",
                column: "id",
                principalTable: "Discount",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_review_Account_id",
                table: "review",
                column: "id",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_review_Book_id",
                table: "review",
                column: "id",
                principalTable: "Book",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCart_Account_id",
                table: "ShoppingCart",
                column: "id",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Account_Id",
                table: "Users",
                column: "Id",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
