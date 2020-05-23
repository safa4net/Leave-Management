using Microsoft.EntityFrameworkCore.Migrations;

namespace Leave_Management.Data.Migrations
{
    public partial class RenameCanceledToCancelled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Canceled",
                table: "LeaveRequests");

            migrationBuilder.AddColumn<bool>(
                name: "Cancelled",
                table: "LeaveRequests",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cancelled",
                table: "LeaveRequests");

            migrationBuilder.AddColumn<bool>(
                name: "Canceled",
                table: "LeaveRequests",
                type: "bit",
                nullable: true);
        }
    }
}
