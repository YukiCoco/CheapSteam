using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChpStmScraper.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Goods",
                columns: table => new
                {
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Kind = table.Column<int>(type: "INTEGER", nullable: false),
                    SteamMarketID = table.Column<string>(type: "TEXT", nullable: false),
                    BuffID = table.Column<string>(type: "TEXT", nullable: false),
                    SteamSellNum = table.Column<int>(type: "INTEGER", nullable: false),
                    BuffSellNum = table.Column<int>(type: "INTEGER", nullable: false),
                    BuffSellPrice = table.Column<double>(type: "REAL", nullable: false),
                    BuffBuyPrice = table.Column<double>(type: "REAL", nullable: false),
                    SteamSellPrice = table.Column<double>(type: "REAL", nullable: false),
                    SteamBuyPrice = table.Column<double>(type: "REAL", nullable: false),
                    SteamSellRadio = table.Column<double>(type: "REAL", nullable: false),
                    SteamBuyRadio = table.Column<double>(type: "REAL", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goods", x => x.Name);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Goods");
        }
    }
}
