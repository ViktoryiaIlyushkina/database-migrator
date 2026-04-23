using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestMigrator.HybridEfCoreMigrator.Migrations
{
    /// <inheritdoc />
    public partial class AddTelephoneNumberPropertyToCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TelphoneNumber",
                table: "Companies",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TelphoneNumber",
                table: "Companies");
        }
    }
}
