using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LumoDevice.API.Migrations
{
    /// <inheritdoc />
    public partial class initials1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_PartnerUsers_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_PartnerUsers_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_MsureRequests_Partners_PartnersId",
                table: "MsureRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_OnboardingRequests_Partners_PartnerId",
                table: "OnboardingRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_partnersProducts_Partners_PartnerId",
                table: "partnersProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopUsers_Partners_PartnerId",
                table: "ShopUsers");

            migrationBuilder.DropIndex(
                name: "IX_ShopUsers_PartnerId",
                table: "ShopUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartnerUsers",
                table: "PartnerUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Partners",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "ConsumerKey",
                table: "ShopUsers");

            migrationBuilder.DropColumn(
                name: "ConsumerSecret",
                table: "ShopUsers");

            migrationBuilder.DropColumn(
                name: "PartnerId",
                table: "ShopUsers");

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "ShopUsers");

            migrationBuilder.RenameTable(
                name: "PartnerUsers",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "Partners",
                newName: "DevicePartners");

            migrationBuilder.AddColumn<int>(
                name: "PartnerId",
                table: "Shops",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PartnersId",
                table: "Shops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DevicePartners",
                table: "DevicePartners",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_PartnersId",
                table: "Shops",
                column: "PartnersId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_Roles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_Roles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MsureRequests_DevicePartners_PartnersId",
                table: "MsureRequests",
                column: "PartnersId",
                principalTable: "DevicePartners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OnboardingRequests_DevicePartners_PartnerId",
                table: "OnboardingRequests",
                column: "PartnerId",
                principalTable: "DevicePartners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_partnersProducts_DevicePartners_PartnerId",
                table: "partnersProducts",
                column: "PartnerId",
                principalTable: "DevicePartners",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Shops_DevicePartners_PartnersId",
                table: "Shops",
                column: "PartnersId",
                principalTable: "DevicePartners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_Roles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_Roles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_MsureRequests_DevicePartners_PartnersId",
                table: "MsureRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_OnboardingRequests_DevicePartners_PartnerId",
                table: "OnboardingRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_partnersProducts_DevicePartners_PartnerId",
                table: "partnersProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Shops_DevicePartners_PartnersId",
                table: "Shops");

            migrationBuilder.DropIndex(
                name: "IX_Shops_PartnersId",
                table: "Shops");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DevicePartners",
                table: "DevicePartners");

            migrationBuilder.DropColumn(
                name: "PartnerId",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "PartnersId",
                table: "Shops");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "PartnerUsers");

            migrationBuilder.RenameTable(
                name: "DevicePartners",
                newName: "Partners");

            migrationBuilder.AddColumn<string>(
                name: "ConsumerKey",
                table: "ShopUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ConsumerSecret",
                table: "ShopUsers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PartnerId",
                table: "ShopUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "ShopUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartnerUsers",
                table: "PartnerUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Partners",
                table: "Partners",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ShopUsers_PartnerId",
                table: "ShopUsers",
                column: "PartnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_PartnerUsers_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "PartnerUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_PartnerUsers_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "PartnerUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MsureRequests_Partners_PartnersId",
                table: "MsureRequests",
                column: "PartnersId",
                principalTable: "Partners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OnboardingRequests_Partners_PartnerId",
                table: "OnboardingRequests",
                column: "PartnerId",
                principalTable: "Partners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_partnersProducts_Partners_PartnerId",
                table: "partnersProducts",
                column: "PartnerId",
                principalTable: "Partners",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopUsers_Partners_PartnerId",
                table: "ShopUsers",
                column: "PartnerId",
                principalTable: "Partners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
