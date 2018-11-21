using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WolontariuszPlus.Data.Migrations
{
    public partial class Finishedmodelsdesign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "AppUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    AddressId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    City = table.Column<string>(nullable: true),
                    Street = table.Column<string>(nullable: true),
                    BuildingNumber = table.Column<int>(nullable: false),
                    ApartmentNumber = table.Column<int>(nullable: false),
                    PostalCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.AddressId);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Tags = table.Column<string>(nullable: true),
                    RequiredPoints = table.Column<int>(nullable: false),
                    OrganizerAppUserId = table.Column<int>(nullable: true),
                    AdressAddressId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_Events_Addresses_AdressAddressId",
                        column: x => x.AdressAddressId,
                        principalTable: "Addresses",
                        principalColumn: "AddressId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Events_AppUsers_OrganizerAppUserId",
                        column: x => x.OrganizerAppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VolunteersOnEvent",
                columns: table => new
                {
                    VolunteerOnEventId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AmountOfMoneyCollected = table.Column<double>(nullable: false),
                    PointsReceived = table.Column<int>(nullable: false),
                    OpinionAboutVolunteer = table.Column<string>(nullable: true),
                    OpinionAboutEvent = table.Column<string>(nullable: true),
                    EventRate = table.Column<string>(nullable: false),
                    EventId = table.Column<int>(nullable: true),
                    VolunteerAppUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolunteersOnEvent", x => x.VolunteerOnEventId);
                    table.ForeignKey(
                        name: "FK_VolunteersOnEvent_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VolunteersOnEvent_AppUsers_VolunteerAppUserId",
                        column: x => x.VolunteerAppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_AddressId",
                table: "AppUsers",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_AdressAddressId",
                table: "Events",
                column: "AdressAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_OrganizerAppUserId",
                table: "Events",
                column: "OrganizerAppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteersOnEvent_EventId",
                table: "VolunteersOnEvent",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteersOnEvent_VolunteerAppUserId",
                table: "VolunteersOnEvent",
                column: "VolunteerAppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_Addresses_AddressId",
                table: "AppUsers",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_Addresses_AddressId",
                table: "AppUsers");

            migrationBuilder.DropTable(
                name: "VolunteersOnEvent");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_AppUsers_AddressId",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "AppUsers");
        }
    }
}
