using Microsoft.EntityFrameworkCore.Migrations;

namespace StockMarket.Domain.Migrations
{
    public partial class AddJsonFieldsOnLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RequestJson",
                table: "RequestLog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ResponseJson",
                table: "RequestLog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "RequestLog",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestJson",
                table: "RequestLog");

            migrationBuilder.DropColumn(
                name: "ResponseJson",
                table: "RequestLog");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "RequestLog");
        }
    }
}
