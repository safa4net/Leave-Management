using Microsoft.EntityFrameworkCore.Migrations;

namespace Leave_Management.Data.Migrations
{
    public partial class AddedRequestCommentToLeaveRequestTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RequestComments",
                table: "LeaveRequests",
                maxLength: 1000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestComments",
                table: "LeaveRequests");
        }
    }
}
