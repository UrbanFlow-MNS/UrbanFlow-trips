using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrbanFlow_trips.Migrations
{
    /// <inheritdoc />
    public partial class AddAgencyToStops : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AgencyId",
                table: "Stops",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Stops_AgencyId",
                table: "Stops",
                column: "AgencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stops_Agencies_AgencyId",
                table: "Stops",
                column: "AgencyId",
                principalTable: "Agencies",
                principalColumn: "AgencyId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stops_Agencies_AgencyId",
                table: "Stops");

            migrationBuilder.DropIndex(
                name: "IX_Stops_AgencyId",
                table: "Stops");

            migrationBuilder.DropColumn(
                name: "AgencyId",
                table: "Stops");
        }
    }
}
