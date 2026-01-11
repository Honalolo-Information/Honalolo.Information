using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Honalolo.Information.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Regions_RegionId1",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Countries_Continents_ContinentId1",
                table: "Countries");

            migrationBuilder.DropForeignKey(
                name: "FK_Regions_Countries_CountryId1",
                table: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Regions_CountryId1",
                table: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Countries_ContinentId1",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Cities_RegionId1",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "CountryId1",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "ContinentId1",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "RegionId1",
                table: "Cities");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryId1",
                table: "Regions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContinentId1",
                table: "Countries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegionId1",
                table: "Cities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Regions_CountryId1",
                table: "Regions",
                column: "CountryId1");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_ContinentId1",
                table: "Countries",
                column: "ContinentId1");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_RegionId1",
                table: "Cities",
                column: "RegionId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Regions_RegionId1",
                table: "Cities",
                column: "RegionId1",
                principalTable: "Regions",
                principalColumn: "region_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_Continents_ContinentId1",
                table: "Countries",
                column: "ContinentId1",
                principalTable: "Continents",
                principalColumn: "continent_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Regions_Countries_CountryId1",
                table: "Regions",
                column: "CountryId1",
                principalTable: "Countries",
                principalColumn: "country_id");
        }
    }
}
