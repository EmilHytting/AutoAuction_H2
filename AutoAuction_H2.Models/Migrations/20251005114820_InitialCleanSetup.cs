using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoAuction_H2.Models.Migrations
{
    /// <inheritdoc />
    public partial class InitialCleanSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ZipCode = table.Column<int>(type: "int", nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    CreditLimit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Mileage = table.Column<double>(type: "float", nullable: false),
                    TowBar = table.Column<bool>(type: "bit", nullable: false),
                    MotorSize = table.Column<double>(type: "float", nullable: false),
                    FuelEfficiency = table.Column<double>(type: "float", nullable: false),
                    FuelType = table.Column<int>(type: "int", nullable: false),
                    VehicleType = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    BusEntity_Height = table.Column<double>(type: "float", nullable: true),
                    BusEntity_Length = table.Column<double>(type: "float", nullable: true),
                    BusEntity_Width = table.Column<double>(type: "float", nullable: true),
                    Doors = table.Column<int>(type: "int", nullable: true),
                    BusEntity_Seats = table.Column<int>(type: "int", nullable: true),
                    HasToilet = table.Column<bool>(type: "bit", nullable: true),
                    CarEntity_Seats = table.Column<int>(type: "int", nullable: true),
                    CarEntity_TrunkSize = table.Column<int>(type: "int", nullable: true),
                    CarEntity_Isofix = table.Column<bool>(type: "bit", nullable: true),
                    PrivateCarEntity_Seats = table.Column<int>(type: "int", nullable: true),
                    Isofix = table.Column<bool>(type: "bit", nullable: true),
                    Seats = table.Column<int>(type: "int", nullable: true),
                    TrunkSize = table.Column<int>(type: "int", nullable: true),
                    SafetyBar = table.Column<bool>(type: "bit", nullable: true),
                    ProfessionalCarEntity_LoadCapacity = table.Column<double>(type: "float", nullable: true),
                    LoadCapacity = table.Column<double>(type: "float", nullable: true),
                    Height = table.Column<double>(type: "float", nullable: true),
                    Length = table.Column<double>(type: "float", nullable: true),
                    Width = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Auctions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    SellerId = table.Column<int>(type: "int", nullable: false),
                    HighestBidderId = table.Column<int>(type: "int", nullable: true),
                    MinPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CurrentBid = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsSold = table.Column<bool>(type: "bit", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auctions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auctions_Users_HighestBidderId",
                        column: x => x.HighestBidderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Auctions_Users_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Auctions_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bids",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuctionId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bids", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bids_Auctions_AuctionId",
                        column: x => x.AuctionId,
                        principalTable: "Auctions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bids_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_HighestBidderId",
                table: "Auctions",
                column: "HighestBidderId");

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_SellerId",
                table: "Auctions",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_VehicleId",
                table: "Auctions",
                column: "VehicleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bids_AuctionId",
                table: "Bids",
                column: "AuctionId");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_UserId",
                table: "Bids",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bids");

            migrationBuilder.DropTable(
                name: "Auctions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Vehicles");
        }
    }
}
