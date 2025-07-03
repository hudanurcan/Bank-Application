using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bankApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerNoToBankAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerNo",
                table: "BankAccounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerNo",
                table: "BankAccounts");
        }
    }
}
