using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PillarBox.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inboxes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedByClientHostname = table.Column<string>(nullable: true),
                    CreatedByClientIP = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    MaxAge = table.Column<TimeSpan>(nullable: false),
                    MaxCount = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ParentInboxId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inboxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inboxes_Inboxes_ParentInboxId",
                        column: x => x.ParentInboxId,
                        principalTable: "Inboxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MessageRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageRules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedByClientHostname = table.Column<string>(nullable: true),
                    CreatedByClientIP = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    DateSent = table.Column<DateTime>(nullable: false),
                    From = table.Column<string>(nullable: true),
                    HasAttachments = table.Column<bool>(nullable: false),
                    InboxId = table.Column<Guid>(nullable: false),
                    Source = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    Summary = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Inboxes_InboxId",
                        column: x => x.InboxId,
                        principalTable: "Inboxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MessageActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ActionType = table.Column<string>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    RuleId = table.Column<Guid>(nullable: false),
                    ForwardingAddress = table.Column<string>(nullable: true),
                    DeviceToken = table.Column<string>(nullable: true),
                    PostTemplate = table.Column<string>(nullable: true),
                    TargetUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageActions_MessageRules_RuleId",
                        column: x => x.RuleId,
                        principalTable: "MessageRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MessageFilters",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    FieldName = table.Column<string>(nullable: true),
                    IsRegularExpression = table.Column<bool>(nullable: false),
                    Pattern = table.Column<string>(nullable: true),
                    RuleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageFilters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageFilters_MessageRules_RuleId",
                        column: x => x.RuleId,
                        principalTable: "MessageRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserInboxes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    InboxId = table.Column<Guid>(nullable: false),
                    Starred = table.Column<bool>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInboxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInboxes_Inboxes_InboxId",
                        column: x => x.InboxId,
                        principalTable: "Inboxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserInboxes_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    MessageId = table.Column<Guid>(nullable: false),
                    Starred = table.Column<bool>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMessages_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserMessages_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inboxes_ParentInboxId",
                table: "Inboxes",
                column: "ParentInboxId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageActions_RuleId",
                table: "MessageActions",
                column: "RuleId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageFilters_RuleId",
                table: "MessageFilters",
                column: "RuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_InboxId",
                table: "Messages",
                column: "InboxId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInboxes_InboxId",
                table: "UserInboxes",
                column: "InboxId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInboxes_UserId",
                table: "UserInboxes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMessages_MessageId",
                table: "UserMessages",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMessages_UserId",
                table: "UserMessages",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageActions");

            migrationBuilder.DropTable(
                name: "MessageFilters");

            migrationBuilder.DropTable(
                name: "UserInboxes");

            migrationBuilder.DropTable(
                name: "UserMessages");

            migrationBuilder.DropTable(
                name: "MessageRules");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Inboxes");
        }
    }
}
