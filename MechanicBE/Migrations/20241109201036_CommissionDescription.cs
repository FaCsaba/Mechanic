using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MechanicBE.Migrations
{
    /// <inheritdoc />
    public partial class CommissionDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Commissions",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Commissions");
        }
    }
}
