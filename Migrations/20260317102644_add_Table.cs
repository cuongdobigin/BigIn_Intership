using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace webApi.Migrations
{
    /// <inheritdoc />
    public partial class add_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_TypeBook_TypeBookId",
                table: "Book");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailOrder_Order_OrderId",
                table: "DetailOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_Image_Book_BookId",
                table: "Image");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Account_AccountId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Discount_DiscountId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_review_Account_AccountId",
                table: "review");

            migrationBuilder.DropForeignKey(
                name: "FK_review_Book_BookId",
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
                name: "IX_review_BookId",
                table: "review");

            migrationBuilder.DropIndex(
                name: "IX_Order_AccountId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_DiscountId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Image_BookId",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_DetailOrder_OrderId",
                table: "DetailOrder");

            migrationBuilder.DropIndex(
                name: "IX_Book_TypeBookId",
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
                name: "BookId",
                table: "review");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "DiscountId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "DetailOrder");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "BookId",
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
                name: "DiscountId",
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
                name: "BookId",
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
                name: "OrderId",
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
                name: "IX_review_BookId",
                table: "review",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_AccountId",
                table: "Order",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_DiscountId",
                table: "Order",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_BookId",
                table: "Image",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailOrder_OrderId",
                table: "DetailOrder",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Book_TypeBookId",
                table: "Book",
                column: "TypeBookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_TypeBook_TypeBookId",
                table: "Book",
                column: "TypeBookId",
                principalTable: "TypeBook",
                principalColumn: "type_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DetailOrder_Order_OrderId",
                table: "DetailOrder",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Book_BookId",
                table: "Image",
                column: "BookId",
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
                name: "FK_Order_Discount_DiscountId",
                table: "Order",
                column: "DiscountId",
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
                name: "FK_review_Book_BookId",
                table: "review",
                column: "BookId",
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
    }
}
