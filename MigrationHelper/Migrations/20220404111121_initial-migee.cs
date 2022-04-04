using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MigrationHelper.Migrations
{
    public partial class initialmigee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Registers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Firstname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PinNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccNum = table.Column<long>(type: "bigint", nullable: false),
                    AccountType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegisterId = table.Column<int>(type: "int", nullable: false),
                    AccNum = table.Column<long>(type: "bigint", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(12,5)", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    CreatDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserAccounts_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAccounts_Registers_RegisterId",
                        column: x => x.RegisterId,
                        principalTable: "Registers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "ID", "name" },
                values: new object[] { 1, "GBP" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "ID", "name" },
                values: new object[] { 2, "Euro" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "ID", "name" },
                values: new object[] { 3, "USD" });

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_CurrencyId",
                table: "UserAccounts",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_RegisterId",
                table: "UserAccounts",
                column: "RegisterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAccounts");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "Registers");
        }
    }
}
