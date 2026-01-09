using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WholeSalers.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class EditManufacturer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InLocation",
                table: "Manufacturers",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InstaLink",
                table: "Manufacturers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MapLocation",
                table: "Manufacturers",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MobilePhone",
                table: "Manufacturers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TikTokLink",
                table: "Manufacturers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebSiteUrl",
                table: "Manufacturers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WhatsappPhone",
                table: "Manufacturers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YoutubeLink",
                table: "Manufacturers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ManufacturerImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ManufacturerId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManufacturerImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ManufacturerImages_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturerImages_ManufacturerId",
                table: "ManufacturerImages",
                column: "ManufacturerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ManufacturerImages");

            migrationBuilder.DropColumn(
                name: "InLocation",
                table: "Manufacturers");

            migrationBuilder.DropColumn(
                name: "InstaLink",
                table: "Manufacturers");

            migrationBuilder.DropColumn(
                name: "MapLocation",
                table: "Manufacturers");

            migrationBuilder.DropColumn(
                name: "MobilePhone",
                table: "Manufacturers");

            migrationBuilder.DropColumn(
                name: "TikTokLink",
                table: "Manufacturers");

            migrationBuilder.DropColumn(
                name: "WebSiteUrl",
                table: "Manufacturers");

            migrationBuilder.DropColumn(
                name: "WhatsappPhone",
                table: "Manufacturers");

            migrationBuilder.DropColumn(
                name: "YoutubeLink",
                table: "Manufacturers");
        }
    }
}
