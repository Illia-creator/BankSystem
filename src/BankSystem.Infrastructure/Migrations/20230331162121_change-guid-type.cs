using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeguidtype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AccountNumberGenerated",
                table: "Accounts",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "AccountNumberGenerated",
                table: "Accounts",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
