using Microsoft.EntityFrameworkCore.Migrations;

namespace WolontariuszPlus.Data.Migrations
{
    public partial class ImplementedAssociationsAndConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_Addresses_AddressId",
                table: "AppUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Addresses_AdressAddressId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_AppUsers_OrganizerAppUserId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteersOnEvent_Events_EventId",
                table: "VolunteersOnEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteersOnEvent_AppUsers_VolunteerAppUserId",
                table: "VolunteersOnEvent");

            migrationBuilder.DropIndex(
                name: "IX_Events_AdressAddressId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "PointsReceived",
                table: "VolunteersOnEvent");

            migrationBuilder.DropColumn(
                name: "AdressAddressId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CreatedEventsCount",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "AppUsers");

            migrationBuilder.RenameColumn(
                name: "VolunteerAppUserId",
                table: "VolunteersOnEvent",
                newName: "VolunteerId");

            migrationBuilder.RenameIndex(
                name: "IX_VolunteersOnEvent_VolunteerAppUserId",
                table: "VolunteersOnEvent",
                newName: "IX_VolunteersOnEvent_VolunteerId");

            migrationBuilder.RenameColumn(
                name: "OrganizerAppUserId",
                table: "Events",
                newName: "OrganizerId");

            migrationBuilder.RenameIndex(
                name: "IX_Events_OrganizerAppUserId",
                table: "Events",
                newName: "IX_Events_OrganizerId");

            migrationBuilder.AlterColumn<string>(
                name: "OpinionAboutVolunteer",
                table: "VolunteersOnEvent",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OpinionAboutEvent",
                table: "VolunteersOnEvent",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "VolunteersOnEvent",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Events",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "Events",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "PESEL",
                table: "AppUsers",
                maxLength: 11,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "AppUsers",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AppUsers",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdentityUserId",
                table: "AppUsers",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AppUsers",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "AppUsers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Street",
                table: "Addresses",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                table: "Addresses",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Addresses",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_AddressId",
                table: "Events",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_Addresses_AddressId",
                table: "AppUsers",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Addresses_AddressId",
                table: "Events",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_AppUsers_OrganizerId",
                table: "Events",
                column: "OrganizerId",
                principalTable: "AppUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteersOnEvent_Events_EventId",
                table: "VolunteersOnEvent",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteersOnEvent_AppUsers_VolunteerId",
                table: "VolunteersOnEvent",
                column: "VolunteerId",
                principalTable: "AppUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_Addresses_AddressId",
                table: "AppUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Addresses_AddressId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_AppUsers_OrganizerId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteersOnEvent_Events_EventId",
                table: "VolunteersOnEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteersOnEvent_AppUsers_VolunteerId",
                table: "VolunteersOnEvent");

            migrationBuilder.DropIndex(
                name: "IX_Events_AddressId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "VolunteerId",
                table: "VolunteersOnEvent",
                newName: "VolunteerAppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_VolunteersOnEvent_VolunteerId",
                table: "VolunteersOnEvent",
                newName: "IX_VolunteersOnEvent_VolunteerAppUserId");

            migrationBuilder.RenameColumn(
                name: "OrganizerId",
                table: "Events",
                newName: "OrganizerAppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Events_OrganizerId",
                table: "Events",
                newName: "IX_Events_OrganizerAppUserId");

            migrationBuilder.AlterColumn<string>(
                name: "OpinionAboutVolunteer",
                table: "VolunteersOnEvent",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OpinionAboutEvent",
                table: "VolunteersOnEvent",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "VolunteersOnEvent",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "PointsReceived",
                table: "VolunteersOnEvent",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Events",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500);

            migrationBuilder.AddColumn<int>(
                name: "AdressAddressId",
                table: "Events",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PESEL",
                table: "AppUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 11,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "AppUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AppUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "IdentityUserId",
                table: "AppUsers",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AppUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "AppUsers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "CreatedEventsCount",
                table: "AppUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "AppUsers",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Street",
                table: "Addresses",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                table: "Addresses",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Addresses",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.CreateIndex(
                name: "IX_Events_AdressAddressId",
                table: "Events",
                column: "AdressAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_Addresses_AddressId",
                table: "AppUsers",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Addresses_AdressAddressId",
                table: "Events",
                column: "AdressAddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_AppUsers_OrganizerAppUserId",
                table: "Events",
                column: "OrganizerAppUserId",
                principalTable: "AppUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteersOnEvent_Events_EventId",
                table: "VolunteersOnEvent",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteersOnEvent_AppUsers_VolunteerAppUserId",
                table: "VolunteersOnEvent",
                column: "VolunteerAppUserId",
                principalTable: "AppUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
