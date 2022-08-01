using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Educ8IT.AspNetCore.SimpleApi.Identity.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiClaims",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(maxLength: 250, nullable: false),
                    ClaimValue = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApiRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RoleName = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApiUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LinkedId = table.Column<Guid>(nullable: true),
                    DisplayName = table.Column<string>(maxLength: 100, nullable: true),
                    UserName = table.Column<string>(maxLength: 100, nullable: false),
                    EmailAddress = table.Column<string>(maxLength: 250, nullable: false),
                    EmailAddressIsConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    AccessFailedAttemptsTotal = table.Column<int>(nullable: false),
                    AccessFailedAttemptsCurrent = table.Column<int>(nullable: false),
                    LockoutUntil = table.Column<DateTime>(nullable: true),
                    Enabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApiRoleClaims",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(nullable: false),
                    ClaimId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiRoleClaims", x => new { x.RoleId, x.ClaimId });
                    table.ForeignKey(
                        name: "FK_ApiRoleClaims_ApiClaims_ClaimId",
                        column: x => x.ClaimId,
                        principalTable: "ApiClaims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApiRoleClaims_ApiRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "ApiRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApiMfas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Method = table.Column<int>(nullable: false),
                    Parameters = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiMfas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApiMfas_ApiUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApiUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApiUserClaims",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    ClaimId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiUserClaims", x => new { x.UserId, x.ClaimId });
                    table.ForeignKey(
                        name: "FK_ApiUserClaims_ApiClaims_ClaimId",
                        column: x => x.ClaimId,
                        principalTable: "ApiClaims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApiUserClaims_ApiUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApiUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApiUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_ApiUserRoles_ApiRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "ApiRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApiUserRoles_ApiUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApiUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApiUserTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Token = table.Column<string>(nullable: false),
                    TokenType = table.Column<string>(nullable: false),
                    ValidFrom = table.Column<DateTime>(nullable: false),
                    ValidUntil = table.Column<DateTime>(nullable: true),
                    ExtendedDataInDb = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiUserTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApiUserTokens_ApiUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApiUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiMfas_UserId",
                table: "ApiMfas",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiRoleClaims_ClaimId",
                table: "ApiRoleClaims",
                column: "ClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiUserClaims_ClaimId",
                table: "ApiUserClaims",
                column: "ClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiUserRoles_RoleId",
                table: "ApiUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiUserTokens_UserId",
                table: "ApiUserTokens",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiMfas");

            migrationBuilder.DropTable(
                name: "ApiRoleClaims");

            migrationBuilder.DropTable(
                name: "ApiUserClaims");

            migrationBuilder.DropTable(
                name: "ApiUserRoles");

            migrationBuilder.DropTable(
                name: "ApiUserTokens");

            migrationBuilder.DropTable(
                name: "ApiClaims");

            migrationBuilder.DropTable(
                name: "ApiRoles");

            migrationBuilder.DropTable(
                name: "ApiUsers");
        }
    }
}
