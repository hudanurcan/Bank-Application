using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bankApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionTypeColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TransactionType",
                table: "Transactions",
                nullable: false,
                defaultValue: 0); // Enum'un varsayılan değeri
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionType",
                table: "Transactions");
        }
    }
}
