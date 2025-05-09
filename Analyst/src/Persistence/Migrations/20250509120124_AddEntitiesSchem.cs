using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddEntitiesSchem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PasswordResetToken_User_UserId",
                table: "PasswordResetToken");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOnUtc",
                table: "User",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOnUtc",
                table: "User",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    IsRead = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedOnUtc = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notification_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SocialMediaPlatform",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    IconUrl = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedOnUtc = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialMediaPlatform", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsageSummary",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlatformId = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalDuration = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    SummaryDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SocialMediaPlatformId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsageSummary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsageSummary_SocialMediaPlatform_PlatformId",
                        column: x => x.PlatformId,
                        principalTable: "SocialMediaPlatform",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsageSummary_SocialMediaPlatform_SocialMediaPlatformId",
                        column: x => x.SocialMediaPlatformId,
                        principalTable: "SocialMediaPlatform",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UsageSummary_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSocialMediaUsage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlatformId = table.Column<int>(type: "INTEGER", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSocialMediaUsage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSocialMediaUsage_SocialMediaPlatform_PlatformId",
                        column: x => x.PlatformId,
                        principalTable: "SocialMediaPlatform",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserSocialMediaUsage_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserUsageGoal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlatformId = table.Column<int>(type: "INTEGER", nullable: false),
                    DailyLimit = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    WarningThresholdPercentage = table.Column<double>(type: "REAL", precision: 5, scale: 2, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedOnUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    SocialMediaPlatformId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserUsageGoal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserUsageGoal_SocialMediaPlatform_PlatformId",
                        column: x => x.PlatformId,
                        principalTable: "SocialMediaPlatform",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserUsageGoal_SocialMediaPlatform_SocialMediaPlatformId",
                        column: x => x.SocialMediaPlatformId,
                        principalTable: "SocialMediaPlatform",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserUsageGoal_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notification_UserId",
                table: "Notification",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SocialMediaPlatform_Name",
                table: "SocialMediaPlatform",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsageSummary_PlatformId",
                table: "UsageSummary",
                column: "PlatformId");

            migrationBuilder.CreateIndex(
                name: "IX_UsageSummary_SocialMediaPlatformId",
                table: "UsageSummary",
                column: "SocialMediaPlatformId");

            migrationBuilder.CreateIndex(
                name: "IX_UsageSummary_UserId_PlatformId_SummaryDate",
                table: "UsageSummary",
                columns: new[] { "UserId", "PlatformId", "SummaryDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSocialMediaUsage_PlatformId",
                table: "UserSocialMediaUsage",
                column: "PlatformId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSocialMediaUsage_UserId_PlatformId_StartTime",
                table: "UserSocialMediaUsage",
                columns: new[] { "UserId", "PlatformId", "StartTime" });

            migrationBuilder.CreateIndex(
                name: "IX_UserUsageGoal_PlatformId",
                table: "UserUsageGoal",
                column: "PlatformId");

            migrationBuilder.CreateIndex(
                name: "IX_UserUsageGoal_SocialMediaPlatformId",
                table: "UserUsageGoal",
                column: "SocialMediaPlatformId");

            migrationBuilder.CreateIndex(
                name: "IX_UserUsageGoal_UserId_PlatformId",
                table: "UserUsageGoal",
                columns: new[] { "UserId", "PlatformId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserUsageGoal_UserId_PlatformId_IsActive",
                table: "UserUsageGoal",
                columns: new[] { "UserId", "PlatformId", "IsActive" },
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.AddForeignKey(
                name: "FK_PasswordResetToken_User_UserId",
                table: "PasswordResetToken",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PasswordResetToken_User_UserId",
                table: "PasswordResetToken");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "UsageSummary");

            migrationBuilder.DropTable(
                name: "UserSocialMediaUsage");

            migrationBuilder.DropTable(
                name: "UserUsageGoal");

            migrationBuilder.DropTable(
                name: "SocialMediaPlatform");

            migrationBuilder.DropColumn(
                name: "CreatedOnUtc",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ModifiedOnUtc",
                table: "User");

            migrationBuilder.AddForeignKey(
                name: "FK_PasswordResetToken_User_UserId",
                table: "PasswordResetToken",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
