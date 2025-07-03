using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bankApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddReceiverUserIdAndReceiverNameToTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReceiverUserId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverUserId",
                table: "Transactions");
        }
    }
}
