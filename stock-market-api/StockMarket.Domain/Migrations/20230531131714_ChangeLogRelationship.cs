using Microsoft.EntityFrameworkCore.Migrations;

namespace StockMarket.Domain.Migrations
{
    public partial class ChangeLogRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestLog_Ticker_TickerId",
                table: "RequestLog");

            migrationBuilder.DropIndex(
                name: "IX_RequestLog_TickerId",
                table: "RequestLog");

            migrationBuilder.DropColumn(
                name: "TickerId",
                table: "RequestLog");

            migrationBuilder.CreateTable(
                name: "RequestLogTicker",
                columns: table => new
                {
                    RequestLogId = table.Column<int>(type: "int", nullable: false),
                    TickerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestLogTicker", x => new { x.RequestLogId, x.TickerId });
                    table.ForeignKey(
                        name: "FK_RequestLogTicker_RequestLog_RequestLogId",
                        column: x => x.RequestLogId,
                        principalTable: "RequestLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestLogTicker_Ticker_TickerId",
                        column: x => x.TickerId,
                        principalTable: "Ticker",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestLogTicker_TickerId",
                table: "RequestLogTicker",
                column: "TickerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestLogTicker");

            migrationBuilder.AddColumn<int>(
                name: "TickerId",
                table: "RequestLog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RequestLog_TickerId",
                table: "RequestLog",
                column: "TickerId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestLog_Ticker_TickerId",
                table: "RequestLog",
                column: "TickerId",
                principalTable: "Ticker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
