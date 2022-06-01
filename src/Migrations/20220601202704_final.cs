using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace zmdh.Migrations
{
    public partial class final : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vestigingen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Adress = table.Column<string>(type: "TEXT", nullable: true),
                    Plaats = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vestigingen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hulpverleners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    Adres = table.Column<string>(type: "TEXT", maxLength: 35, nullable: false),
                    Specialisatie = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false),
                    Intro = table.Column<string>(type: "TEXT", nullable: false),
                    Study = table.Column<string>(type: "TEXT", nullable: false),
                    OverJou = table.Column<string>(type: "TEXT", nullable: false),
                    Behandeling = table.Column<string>(type: "TEXT", nullable: false),
                    Foto = table.Column<string>(type: "TEXT", nullable: false),
                    VestigingId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hulpverleners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hulpverleners_Vestigingen_VestigingId",
                        column: x => x.VestigingId,
                        principalTable: "Vestigingen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Clienten",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Adres = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    Residence = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    AboveSixteen = table.Column<bool>(type: "INTEGER", nullable: false),
                    HulpverlenerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clienten", x => x.ClientId);
                    table.ForeignKey(
                        name: "FK_Clienten_Hulpverleners_HulpverlenerId",
                        column: x => x.HulpverlenerId,
                        principalTable: "Hulpverleners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Meldingen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<string>(type: "TEXT", nullable: true),
                    HulpverlenerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meldingen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Meldingen_Hulpverleners_HulpverlenerId",
                        column: x => x.HulpverlenerId,
                        principalTable: "Hulpverleners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ouders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    ClientId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ouders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ouders_Clienten_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clienten",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Aanmeldingen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Agree = table.Column<bool>(type: "INTEGER", nullable: false),
                    ClientId = table.Column<int>(type: "INTEGER", nullable: false),
                    OuderId = table.Column<int>(type: "INTEGER", nullable: true),
                    HulpverlenerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aanmeldingen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Aanmeldingen_Clienten_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clienten",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Aanmeldingen_Hulpverleners_HulpverlenerId",
                        column: x => x.HulpverlenerId,
                        principalTable: "Hulpverleners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Aanmeldingen_Ouders_OuderId",
                        column: x => x.OuderId,
                        principalTable: "Ouders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    ClientId = table.Column<int>(type: "INTEGER", nullable: true),
                    HulpverlenerId = table.Column<int>(type: "INTEGER", nullable: true),
                    OuderId = table.Column<int>(type: "INTEGER", nullable: true),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Clienten_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clienten",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Hulpverleners_HulpverlenerId",
                        column: x => x.HulpverlenerId,
                        principalTable: "Hulpverleners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Ouders_OuderId",
                        column: x => x.OuderId,
                        principalTable: "Ouders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChatFrequencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Status = table.Column<string>(type: "TEXT", nullable: true),
                    ChatId = table.Column<int>(type: "INTEGER", nullable: false),
                    OuderId = table.Column<int>(type: "INTEGER", nullable: false),
                    HulpverlenerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatFrequencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatFrequencies_Hulpverleners_HulpverlenerId",
                        column: x => x.HulpverlenerId,
                        principalTable: "Hulpverleners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatFrequencies_Ouders_OuderId",
                        column: x => x.OuderId,
                        principalTable: "Ouders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Moderator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    userId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moderator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Moderator_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    ChatFrequencyId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chats_ChatFrequencies_ChatFrequencyId",
                        column: x => x.ChatFrequencyId,
                        principalTable: "ChatFrequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserChats",
                columns: table => new
                {
                    ApplicationUserId = table.Column<string>(type: "TEXT", nullable: false),
                    ChatId = table.Column<int>(type: "INTEGER", nullable: false),
                    ApplicationUserChatStatus = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserChats", x => new { x.ApplicationUserId, x.ChatId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserChats_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserChats_Chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AuthorId = table.Column<string>(type: "TEXT", nullable: true),
                    ChatId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SelfHelpGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    AboveSixteen = table.Column<bool>(type: "INTEGER", nullable: false),
                    ChatId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelfHelpGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SelfHelpGroups_Chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    isHandled = table.Column<bool>(type: "INTEGER", nullable: false),
                    MessageId = table.Column<int>(type: "INTEGER", nullable: false),
                    HandlerId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_AspNetUsers_HandlerId",
                        column: x => x.HandlerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reports_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e8431a45-405f-4932-8819-46b179f1d3d3", "d80f1feb-9b07-4dd1-9d4c-b6420efe0d8e", "Client", "CLIENT" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ad376a8f-9eab-4bb9-9fca-30b01540f445", "3221c4c5-7527-4303-92c4-d880c48f3a93", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "21879a36-cc2c-4fb9-9213-2708b962b92e", "37f1c107-b808-413c-bfa0-46bfd0aaf882", "Hulpverlener", "HULPVERLENER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3e0f2af8-69ce-4d39-ae6a-440e388b3ca3", "d2055fd5-dd65-41fc-93ae-8f2a5d9a340d", "Ouder", "OUDER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e7c5f138-26ff-4413-a2e2-273a11c334d4", "75c1e5f4-5c51-4bd3-9cb2-d844a218f2ef", "Moderator", "MODERATOR" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ClientId", "ConcurrencyStamp", "Email", "EmailConfirmed", "HulpverlenerId", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OuderId", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "a18be9c0-aa65-4af8-bd17-00bd9344e575", 0, null, "68714c14-cd9d-447a-92b8-eea67f1bceb9", "admin@admin.com", false, null, true, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", null, "AQAAAAEAACcQAAAAEFIpkMkCpC+Uv/R84y5KJEx0qQg5DblN/pZhS7sn39FhWuINUWJrrs6hvojAecehkQ==", null, false, "f91c88c4-736a-4e9a-a4bb-52deeffdb25c", false, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ClientId", "ConcurrencyStamp", "Email", "EmailConfirmed", "HulpverlenerId", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OuderId", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "a18be9c0-aa65-4af8-bd17-00bd9344e574", 0, null, "cb9f7de4-8a7b-4793-a9fa-5969f55949ec", "admin2@admin.com", false, null, true, null, "ADMIN2@ADMIN.COM", "ADMIN2@ADMIN.COM", null, "AQAAAAEAACcQAAAAEEG1HQbAyKde8Pom+yV5EfLaBY/uxHTvbQU8SmwgxQX8U9mt/dkKwdP0mVppQeBGxQ==", null, false, "04587595-28dc-4912-bafe-bbffa4677f47", false, "admin2@admin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ClientId", "ConcurrencyStamp", "Email", "EmailConfirmed", "HulpverlenerId", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OuderId", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "a99be1c0-aq65-4af8-br17-00bd9674e571", 0, null, "7153435b-d6a9-47f4-99ca-91bf66f3d087", "mod@mod.com", false, null, true, null, "MOD@MOD.COM", "MOD@MOD.COM", null, "AQAAAAEAACcQAAAAEDUcuDFYfXfjjQe13ciaX1A4vVy+gSKFNgbXyzRzR5LL4ssCpoBgdKeIX1xkdzMpLw==", null, false, "5cc5a137-55a3-47a7-8adc-c24409220bf7", false, "mod@mod.com" });

            migrationBuilder.InsertData(
                table: "Vestigingen",
                columns: new[] { "Id", "Adress", "Name", "Plaats" },
                values: new object[] { 1, "Melis Stokelaan 156", "Vesteging Den Haag", "Den Haag" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "ad376a8f-9eab-4bb9-9fca-30b01540f445", "a18be9c0-aa65-4af8-bd17-00bd9344e575" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "ad376a8f-9eab-4bb9-9fca-30b01540f445", "a18be9c0-aa65-4af8-bd17-00bd9344e574" });

            migrationBuilder.InsertData(
                table: "Hulpverleners",
                columns: new[] { "Id", "Adres", "Behandeling", "Foto", "Intro", "Name", "OverJou", "Specialisatie", "Study", "VestigingId" },
                values: new object[] { 1, "", "Eerst moet er vastgesteld worden of je een eetstoornis hebt, en zo ja wat voor eetstoornis je hebt. Daarna kijken we hoe we jou kunnen helpen. Een eetstoornis is namelijk een psychische aandoening die niet vanzelf over gaat. We kunnen de eetstoornis het best behandelen met Cognitieve gedragstherapie. Cognitieve gedragstherapie is de meest onderzochte en effectieve psychologische behandeling bij eetstoornissen. In de behandeling wordt onder andere onderzocht welke gedachten er zijn over eten, gewicht en lichaamsvormen. Daarnaast wordt er ook onderzocht of deze gedachten kloppen. ", "Screenshot_57.png", "Ik heet Kees Closed, geboren in 1978 in Monster en de jongste van twee kinderen. Mijn ouders zijn gescheiden toen ik zes was en ik ben opgegroeid in het Westland. Ik ben heel nieuwsgierig naar het afwijkend eetgedrag van jongeren, waarom ze doen wat ze doen en wat hun verhaal is. Ik heb vroeger zelf last gehad van een eetstoornis en ben hier door middel van de juiste hulp vanaf gekomen. Dit heeft mij nieuwsgierig gemaakt naar hoe anderen dat ervaren en hoe ik hun daarmee kan helpen. ", "Kees", "Heb jij en vermoede dat je een eetstoornis hebt? Het grootste deel van de mensen heeft een verstoord lichaamsbeeld. Dit zorgt ervoor dat je bang bent om in gewicht aan te komen. Deze mensen letten vaak op wat ze eten, sporten overmatig of braken zelfs. Het gewicht wordt dan een obsessie en het lijnen, sporten of braken wordt een verslaving. ", "Eetstoornissen", "Ik heb orthopedagogiek gestudeerd aan de Universiteit van Leiden. Bij mijn afstuderen heb ik mij gespecialiseerd in de behandeling van eetstoornissen.  ", 1 });

            migrationBuilder.InsertData(
                table: "Hulpverleners",
                columns: new[] { "Id", "Adres", "Behandeling", "Foto", "Intro", "Name", "OverJou", "Specialisatie", "Study", "VestigingId" },
                values: new object[] { 2, "", "Eerst gaan wij is een goed gesprek hebben over wie jij bent en wat je doet, ik ben namelijk heel benieuwd naar je. Daarna zal ik wat tests samen met jou afnemen. Deze test bestaan vooral Uit lees- en schrijfoefeningen. Waaruit we zullen zien of je wel daadwerkelijk dyslectie hebt. Zo ja, dan zullen we meer afspraken maken. Tijdens deze afspraken ga ik jou helpen met lezen en schrijven. We gaan samen werken aan trucjes die bij jou aansluiten om je zo goed mogelijk te helpen om op het juiste niveau te komen. ", "Screenshot_58.png", "Mijn naam is Krystel Vegter, geboren in 1990 in Honselersdijk en ik heb een broer. Mijn ouders zijn gescheiden toen ik zes was en ik ben opgegroeid in het Westland bij mijn moeder. Mijn hobby’s zijn schilderen, koken en een goed boek lezen. Door het scheiden van mijn ouders ben ik al op jongen leeftijd geïnteresseerd geraakt in beweegredenen van mensen en specifiek in jonge mensen die nog volop in ontwikkeling zijn. Door mijn nieuwsgierigheid te combineren met mijn interesse heb ik mijn perfecte beroep gevonden. ", "Krystel", "Er bestaat een vermoeden dat jij dyslectie hebt. Het is je opgevallen dat lezen toch langzamer gaat dan de rest van de kinderen/jongeren om je heen. Daarnaast zijn alle taalvakken moeilijk te volgen.   Waardoor ook schrijven je niet altijd even makkelijk afgaat. Grote kans dat een van je leraren dit is opgevallen en dit heeft aangegeven bij ons. Wees maar gerust want er is nog niks zeker en al zou je het wel hebben. dyslectie hoeft je schoolcarrière zeker niet in de weg te gaan zitten, want er zijn genoeg hulpmiddelen. ", "Dyslectie", "Ik heb gestudeerd aan de universiteit van leiden, waar ik mijn bachelor in psychologie heb gehaald en een master gespecialiseerd rondom problemen bij jongeren. ", 1 });

            migrationBuilder.InsertData(
                table: "Hulpverleners",
                columns: new[] { "Id", "Adres", "Behandeling", "Foto", "Intro", "Name", "OverJou", "Specialisatie", "Study", "VestigingId" },
                values: new object[] { 3, "", "Wat we eerst doen is nagaan wat voor soort angststoornis jij te maken mee heeft. Dit doen we met een aantal testen. Jouw ouders of verzorgers mogen bij de testen zijn. Als het duidelijk is wat voor soort angststoornis het is gaan wij een behandelplan hiervoor samenstellen.   Eén van de eerste stappen is een gesprek voeren over gebeurtenissen in jouw leven. Dat is puur om uit te vogelen wat de angststoornis heeft veroorzaakt. Als we een oorzaak hebben gevonden kunnen progressieve gesprekken houden zodat jij in jouw vel zit. Je kan mij bellen of mailen voor meer informatie of vragen. We plannen gesprekken in om te zien hoe wij verder gaan erna. Ik help jouw heel graag en je bent van alle harte welkom. ", "Screenshot_59.png", "Mijn naam is Sugodies Allen, geboren in 1978 en opgegroeid in Den Haag, Ik ben de jongste in een gezin van zes kinderen. Mijn hobby’s zijn: lezen, schrijven en mensen helpen waar ik geschikt voor ben. Van jongs af aan wilde ik altijd iets doen om mensen met hun angst te helpen en overkomen. Hierdoor streef ik om een orthopedagoog te worden gespecialiseerd in cliënten helpen die te maken hebben met een angststoornis. ", "Sugodies Allen", "Angststoornis is dat je bang bent zelfs als de situatie geen gevaar geeft. Je raakt hierdoor in paniek en ook krijgen van serieuze klachten. Kenmerken van angststoornis die vaak gebeuren zijn: hoofdpijn, buikpijn, slaapproblemen en prikkelbaarheid. Maar ook bijzondere kenmerken: ademnood, hartkloppingen, tinteling of een doof gevoel in jouw ledematen. Er zijn nog vele andere kenmerken die je nog tegenaan loopt bij angststoornis. ", "Angststoornis", "Nadat ik klaar was met het VWO heb ik orthopedagogiek gestudeerd aan de Universiteit van Amsterdam. Bij het afstuderen heb ik mij gespecialiseerd in de behandelingen van angststoornissen. ", 1 });

            migrationBuilder.InsertData(
                table: "Hulpverleners",
                columns: new[] { "Id", "Adres", "Behandeling", "Foto", "Intro", "Name", "OverJou", "Specialisatie", "Study", "VestigingId" },
                values: new object[] { 4, "", "We gaan gezamenlijk uitzoeken of jij ADHD hebt. Dit doen we door middel van een paar testen. Je ouder(s)/verzorger(s) mogen hier ook bij zijn als jij dat prettig vindt. De testen doen we omdat ik namelijk erg benieuwd ben naar jou en hoe jij ADHD ervaart. Op basis van deze testen kan ik oefeningen met je doen en tips geven om uiteindelijk beter met ADHD om te gaan.   Je bent zeer welkom en ik wil je graag helpen, als je nog meer informatie wilt kan je me gerust emaillen of bellen. ", "Screenshot_60.png", "Mijn naam is Melissa Wiedegem, geboren in 1995 in Rotterdam met een tweelingzus. Ik merkte op jonge leeftijd al dat ik een passie begon op te bouwen voor het helpen van anderen. Dit groeide later uit tot het helpen van kinderen met ADHD. ", "Melissa Wiedegem", "Bij jou bestaat het vermoeden dat je ADHD hebt. Als je dit hebt, werken je hersenen net iets anders dan bij iemand zonder ADHD. De meeste mensen hebben een soort filter in hun hersenen. Die zorgt ervoor dat niet alle prikkels (zoals geluiden of dingen in de omgeving) even hard binnen komen. Dit is bij ADHD anders. Bij ADHD komen er juist te veel prikkels binnen doordat er minder goed wordt gefilterd. Daardoor heb jij misschien moeite met het focussen op de belangrijke dingen van dat moment zonder afgeleid te worden. Zo zijn er nog meer dingen waar jij tegen aan kan lopen die bij ADHD horen. ", "ADHD", "Ik heb eerder al verschillende rollen vervuld zoals leerkracht en speciale leerkracht. In 2017 ben ik afgestudeerd als orthopedagoog aan de Universiteit van Utrecht. Ik heb mij bij het afstuderen gespecialiseerd in de behandeling van ADHD. ", 1 });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ClientId", "ConcurrencyStamp", "Email", "EmailConfirmed", "HulpverlenerId", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OuderId", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "6c26c683-3d56-4b10-a671-9c0565acba97", 0, null, "bd590c96-9dbe-4800-b7d7-5bf8a5a83b28", "kees@gmail.com", false, 1, true, null, "KEES@GMAIL.COM", "KEES@GMAIL.COM", null, "AQAAAAEAACcQAAAAEEXpvyl2BKzNwW1I9LGM9OkOeqfSurn77d5PIwFsVJVqsysz2CKYDRVci476R9uLgw==", null, false, "fedf8ab5-530d-4597-b25c-d47bcd939d02", false, "kees@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ClientId", "ConcurrencyStamp", "Email", "EmailConfirmed", "HulpverlenerId", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OuderId", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "4a02ed90-00c0-4a74-b95e-a54722f1c734", 0, null, "50127f3a-067d-4fae-9a8f-671544770145", "krystel@gmail.com", false, 2, true, null, "KRYSTEL@GMAIL.COM", "KRYSTEL@GMAIL.COM", null, "AQAAAAEAACcQAAAAEK2HvsN4orPPDmqu5uSheEMKjQ0WzBtqUiSj4CsrLB4PZLe2hGa2A9TkshVetI9jvw==", null, false, "c502ff70-6e4f-4ff7-abae-80e58e450f82", false, "krystel@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ClientId", "ConcurrencyStamp", "Email", "EmailConfirmed", "HulpverlenerId", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OuderId", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "8c74234a-facb-40fb-b812-f45b1f328867", 0, null, "69fc0001-c6e9-41ea-9716-fd9c3d1178e9", "sugodies@gmail.com", false, 3, true, null, "SUGODIES@GMAIL.COM", "SUGODIES@GMAIL.COM", null, "AQAAAAEAACcQAAAAEOhPaBbQSUk6RX6jOj6nm1ZFSIRSOEMSAfwbNgCOg8+SB9LpoAnjvnq8fnogSKmSrA==", null, false, "ee208317-2d33-4270-8824-a1aa18699fcc", false, "sugodies@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ClientId", "ConcurrencyStamp", "Email", "EmailConfirmed", "HulpverlenerId", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OuderId", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "e0fd4978-7f42-42d9-a3d8-0efbdd40188b", 0, null, "e4d5cfc6-6d49-4467-ad06-7f79ce0ce8d3", "melissa@gmail.com", false, 4, true, null, "MELISSA@GMAIL.COM", "MELISSA@GMAIL.COM", null, "AQAAAAEAACcQAAAAEPrZn8/jKNzFyH8MqD6wLv9tx+u9H6uvCv2LqWtHTLZh7XK5YgpusRRGf95+5RJrJw==", null, false, "7ce06b89-9091-4820-b44a-fb0d61df4910", false, "melissa@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "21879a36-cc2c-4fb9-9213-2708b962b92e", "6c26c683-3d56-4b10-a671-9c0565acba97" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "21879a36-cc2c-4fb9-9213-2708b962b92e", "4a02ed90-00c0-4a74-b95e-a54722f1c734" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "21879a36-cc2c-4fb9-9213-2708b962b92e", "8c74234a-facb-40fb-b812-f45b1f328867" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "21879a36-cc2c-4fb9-9213-2708b962b92e", "e0fd4978-7f42-42d9-a3d8-0efbdd40188b" });

            migrationBuilder.CreateIndex(
                name: "IX_Aanmeldingen_ClientId",
                table: "Aanmeldingen",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Aanmeldingen_HulpverlenerId",
                table: "Aanmeldingen",
                column: "HulpverlenerId");

            migrationBuilder.CreateIndex(
                name: "IX_Aanmeldingen_OuderId",
                table: "Aanmeldingen",
                column: "OuderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserChats_ChatId",
                table: "ApplicationUserChats",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ClientId",
                table: "AspNetUsers",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_HulpverlenerId",
                table: "AspNetUsers",
                column: "HulpverlenerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_OuderId",
                table: "AspNetUsers",
                column: "OuderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatFrequencies_HulpverlenerId",
                table: "ChatFrequencies",
                column: "HulpverlenerId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatFrequencies_OuderId",
                table: "ChatFrequencies",
                column: "OuderId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_ChatFrequencyId",
                table: "Chats",
                column: "ChatFrequencyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clienten_HulpverlenerId",
                table: "Clienten",
                column: "HulpverlenerId");

            migrationBuilder.CreateIndex(
                name: "IX_Hulpverleners_VestigingId",
                table: "Hulpverleners",
                column: "VestigingId");

            migrationBuilder.CreateIndex(
                name: "IX_Meldingen_HulpverlenerId",
                table: "Meldingen",
                column: "HulpverlenerId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_AuthorId",
                table: "Messages",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatId",
                table: "Messages",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Moderator_userId",
                table: "Moderator",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Ouders_ClientId",
                table: "Ouders",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_HandlerId",
                table: "Reports",
                column: "HandlerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_MessageId",
                table: "Reports",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_SelfHelpGroups_ChatId",
                table: "SelfHelpGroups",
                column: "ChatId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aanmeldingen");

            migrationBuilder.DropTable(
                name: "ApplicationUserChats");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Meldingen");

            migrationBuilder.DropTable(
                name: "Moderator");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "SelfHelpGroups");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropTable(
                name: "ChatFrequencies");

            migrationBuilder.DropTable(
                name: "Ouders");

            migrationBuilder.DropTable(
                name: "Clienten");

            migrationBuilder.DropTable(
                name: "Hulpverleners");

            migrationBuilder.DropTable(
                name: "Vestigingen");
        }
    }
}
