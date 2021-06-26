using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Musan.Entities.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "WScore");

            migrationBuilder.EnsureSchema(
                name: "Car");

            migrationBuilder.EnsureSchema(
                name: "Maintenance");

            migrationBuilder.EnsureSchema(
                name: "Identity");

            migrationBuilder.CreateTable(
                name: "CarsInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Company = table.Column<int>(type: "int", nullable: false),
                    Model = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<int>(type: "int", nullable: false),
                    CarTransmission = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarsInformation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Color",
                schema: "Car",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VARCHAR(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Color", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                schema: "Car",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VARCHAR(250)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                schema: "Maintenance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VARCHAR(250)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Device_Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    User_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Faq",
                schema: "WScore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "VARCHAR(2000)", nullable: false),
                    Answer = table.Column<string>(type: "VARCHAR(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faq", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fault",
                schema: "Maintenance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VARCHAR(250)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fault", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Language",
                schema: "WScore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VARCHAR(150)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Model",
                schema: "Car",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VARCHAR(250)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Model", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethod",
                schema: "Maintenance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VARCHAR(250)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transmission",
                schema: "Car",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VARCHAR(250)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transmission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    Created = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false),
                    VerificationToken = table.Column<string>(type: "VARCHAR(500)", nullable: false),
                    ResetToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetTokenExpires = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "VARCHAR(25)", nullable: false),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "VARCHAR(350)", nullable: false),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "VARCHAR(25)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaim",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaim_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Detail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Announcements_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chat",
                schema: "WScore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "INT", nullable: false),
                    ReceiverId = table.Column<int>(type: "INT", nullable: false),
                    Message = table.Column<string>(type: "VARCHAR(2000)", nullable: false),
                    MessageTime = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chat_Receiver",
                        column: x => x.ReceiverId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Chat_Sender",
                        column: x => x.SenderId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                schema: "WScore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "INT", nullable: false),
                    Message = table.Column<string>(type: "VARCHAR(500)", nullable: false),
                    NotificationTime = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notification_User",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Detail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    JwtId = table.Column<string>(type: "VARCHAR(150)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    ExpiryDate = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    Used = table.Column<bool>(type: "BIT", nullable: false),
                    Invalidated = table.Column<bool>(type: "BIT", nullable: false),
                    UserId = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_User",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Service",
                schema: "Maintenance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "VARCHAR(1000)", nullable: false),
                    CompanyId = table.Column<int>(type: "INT", nullable: false),
                    ModelId = table.Column<int>(type: "INT", nullable: false),
                    ColorId = table.Column<int>(type: "INT", nullable: false),
                    TransmissionId = table.Column<int>(type: "INT", nullable: false),
                    PaymentMethodId = table.Column<int>(type: "INT", nullable: false),
                    DepartmentId = table.Column<int>(type: "INT", nullable: false),
                    UserId = table.Column<int>(type: "INT", nullable: false),
                    ServiceStatus = table.Column<int>(type: "INT", nullable: false, defaultValueSql: "1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Service_Color",
                        column: x => x.ColorId,
                        principalSchema: "Car",
                        principalTable: "Color",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Service_Company",
                        column: x => x.CompanyId,
                        principalSchema: "Car",
                        principalTable: "Company",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Service_Department",
                        column: x => x.DepartmentId,
                        principalSchema: "Maintenance",
                        principalTable: "Department",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Service_Model",
                        column: x => x.ModelId,
                        principalSchema: "Car",
                        principalTable: "Model",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Service_PaymentMethod",
                        column: x => x.PaymentMethodId,
                        principalSchema: "Maintenance",
                        principalTable: "PaymentMethod",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Service_Transmission",
                        column: x => x.TransmissionId,
                        principalSchema: "Car",
                        principalTable: "Transmission",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Service_User",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserClaim",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaim_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogin",
                schema: "Identity",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogin_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPaymentMethod",
                schema: "WScore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "INT", nullable: false),
                    PaymentMethodId = table.Column<int>(type: "INT", nullable: false),
                    CardHolderName = table.Column<string>(type: "VARCHAR(150)", nullable: false),
                    CardNumber = table.Column<string>(type: "VARCHAR(25)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "DATE", nullable: false),
                    Cvc = table.Column<int>(type: "INT", nullable: false),
                    IsPreferred = table.Column<bool>(type: "BIT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPaymentMethod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPaymentMethod_PaymentMethod",
                        column: x => x.PaymentMethodId,
                        principalSchema: "Maintenance",
                        principalTable: "PaymentMethod",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserPaymentMethod_User",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                schema: "Identity",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSetting",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    EmailNotification = table.Column<bool>(type: "BIT", nullable: false),
                    AcceptRequest = table.Column<bool>(type: "BIT", nullable: false),
                    DeclineRequest = table.Column<bool>(type: "BIT", nullable: false),
                    LanguageId = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSetting_Language",
                        column: x => x.LanguageId,
                        principalSchema: "WScore",
                        principalTable: "Language",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserSetting_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserToken",
                schema: "Identity",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToken", x => new { x.LoginProvider, x.UserId, x.Name });
                    table.ForeignKey(
                        name: "FK_UserToken_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkShopImage",
                schema: "Maintenance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkShopId = table.Column<int>(type: "INT", nullable: false),
                    Photo = table.Column<byte[]>(type: "VARBINARY(MAX)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkShopImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkShopImage_WorkShop",
                        column: x => x.WorkShopId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkShopInfo",
                schema: "Maintenance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkShopId = table.Column<int>(type: "INT", nullable: false),
                    Sponsor = table.Column<string>(type: "VARCHAR(150)", nullable: true),
                    Department = table.Column<string>(type: "VARCHAR(50)", nullable: true),
                    Latitude = table.Column<decimal>(type: "NUMERIC(18,10)", nullable: true),
                    Longitude = table.Column<decimal>(type: "NUMERIC(18,10)", nullable: true),
                    Facility = table.Column<string>(type: "VARCHAR(50)", nullable: true),
                    CommercialRegister = table.Column<string>(type: "VARCHAR(50)", nullable: true),
                    ElectonicPaymentAccount = table.Column<string>(type: "VARCHAR(50)", nullable: true),
                    Photo = table.Column<byte[]>(type: "VARBINARY(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkShopInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkShopInfo_User_WorkShopId",
                        column: x => x.WorkShopId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnnouncementImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Photo = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    AnnouncementId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnouncementImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnnouncementImages_Announcements_AnnouncementId",
                        column: x => x.AnnouncementId,
                        principalTable: "Announcements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    AnswerText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Answers_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "QuestionImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Photo = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionImages_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                schema: "Maintenance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<int>(type: "INT", nullable: false),
                    WorkshopId = table.Column<int>(type: "INT", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "DATE", nullable: false),
                    ExpectedCompletionDate = table.Column<DateTime>(type: "DATE", nullable: false),
                    CompletionDate = table.Column<DateTime>(type: "DATE", nullable: true),
                    OrderNumber = table.Column<string>(type: "VARCHAR(25)", nullable: true),
                    EstimatedPrice = table.Column<decimal>(type: "NUMERIC(10,2)", nullable: false),
                    OrderStatusId = table.Column<int>(type: "INT", nullable: false),
                    OrderProgress = table.Column<int>(type: "INT", nullable: false),
                    OrderRating = table.Column<int>(type: "int", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlowStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Service",
                        column: x => x.ServiceId,
                        principalSchema: "Maintenance",
                        principalTable: "Service",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Order_WorkShop",
                        column: x => x.WorkshopId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ServiceFault",
                schema: "Maintenance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FaultId = table.Column<int>(type: "INT", nullable: false),
                    ServiceId = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceFault", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceFault_Fault",
                        column: x => x.FaultId,
                        principalSchema: "Maintenance",
                        principalTable: "Fault",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ServiceFault_Service",
                        column: x => x.ServiceId,
                        principalSchema: "Maintenance",
                        principalTable: "Service",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ServiceImage",
                schema: "WScore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Photo = table.Column<byte[]>(type: "VARBINARY(MAX)", nullable: false),
                    ServiceId = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceImage_Service",
                        column: x => x.ServiceId,
                        principalSchema: "Maintenance",
                        principalTable: "Service",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderStep",
                schema: "Maintenance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "INT", nullable: false),
                    ActionDate = table.Column<DateTime>(type: "DATE", nullable: false),
                    Title = table.Column<string>(type: "VARCHAR(150)", nullable: false),
                    OrderStepStatus = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStep", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderStep_Order",
                        column: x => x.OrderId,
                        principalSchema: "Maintenance",
                        principalTable: "Order",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncementImages_AnnouncementId",
                table: "AnnouncementImages",
                column: "AnnouncementId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_UserId",
                table: "Announcements",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_UserId",
                table: "Answers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Chat_ReceiverId",
                schema: "WScore",
                table: "Chat",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Chat_SenderId",
                schema: "WScore",
                table: "Chat",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_UserId",
                schema: "WScore",
                table: "Notification",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ServiceId",
                schema: "Maintenance",
                table: "Order",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_WorkshopId",
                schema: "Maintenance",
                table: "Order",
                column: "WorkshopId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStep_OrderId",
                schema: "Maintenance",
                table: "OrderStep",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionImages_QuestionId",
                table: "QuestionImages",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_UserId",
                table: "Questions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserId",
                schema: "Identity",
                table: "RefreshToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaim_RoleId",
                schema: "Identity",
                table: "RoleClaim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_ColorId",
                schema: "Maintenance",
                table: "Service",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_CompanyId",
                schema: "Maintenance",
                table: "Service",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_DepartmentId",
                schema: "Maintenance",
                table: "Service",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_ModelId",
                schema: "Maintenance",
                table: "Service",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_PaymentMethodId",
                schema: "Maintenance",
                table: "Service",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_TransmissionId",
                schema: "Maintenance",
                table: "Service",
                column: "TransmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_UserId",
                schema: "Maintenance",
                table: "Service",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceFault_FaultId",
                schema: "Maintenance",
                table: "ServiceFault",
                column: "FaultId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceFault_ServiceId",
                schema: "Maintenance",
                table: "ServiceFault",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceImage_ServiceId",
                schema: "WScore",
                table: "ServiceImage",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaim_UserId",
                schema: "Identity",
                table: "UserClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogin_UserId",
                schema: "Identity",
                table: "UserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPaymentMethod_PaymentMethodId",
                schema: "WScore",
                table: "UserPaymentMethod",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPaymentMethod_UserId",
                schema: "WScore",
                table: "UserPaymentMethod",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                schema: "Identity",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSetting_LanguageId",
                schema: "Identity",
                table: "UserSetting",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSetting_UserId",
                schema: "Identity",
                table: "UserSetting",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_UserId",
                schema: "Identity",
                table: "UserToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkShopImage_WorkShopId",
                schema: "Maintenance",
                table: "WorkShopImage",
                column: "WorkShopId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkShopInfo_WorkShopId",
                schema: "Maintenance",
                table: "WorkShopInfo",
                column: "WorkShopId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnnouncementImages");

            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "CarsInformation");

            migrationBuilder.DropTable(
                name: "Chat",
                schema: "WScore");

            migrationBuilder.DropTable(
                name: "DeviceTokens");

            migrationBuilder.DropTable(
                name: "Faq",
                schema: "WScore");

            migrationBuilder.DropTable(
                name: "Notification",
                schema: "WScore");

            migrationBuilder.DropTable(
                name: "OrderStep",
                schema: "Maintenance");

            migrationBuilder.DropTable(
                name: "QuestionImages");

            migrationBuilder.DropTable(
                name: "RefreshToken",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "RoleClaim",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ServiceFault",
                schema: "Maintenance");

            migrationBuilder.DropTable(
                name: "ServiceImage",
                schema: "WScore");

            migrationBuilder.DropTable(
                name: "UserClaim",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "UserLogin",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "UserPaymentMethod",
                schema: "WScore");

            migrationBuilder.DropTable(
                name: "UserRole",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "UserSetting",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "UserToken",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "WorkShopImage",
                schema: "Maintenance");

            migrationBuilder.DropTable(
                name: "WorkShopInfo",
                schema: "Maintenance");

            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DropTable(
                name: "Order",
                schema: "Maintenance");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Fault",
                schema: "Maintenance");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Language",
                schema: "WScore");

            migrationBuilder.DropTable(
                name: "Service",
                schema: "Maintenance");

            migrationBuilder.DropTable(
                name: "Color",
                schema: "Car");

            migrationBuilder.DropTable(
                name: "Company",
                schema: "Car");

            migrationBuilder.DropTable(
                name: "Department",
                schema: "Maintenance");

            migrationBuilder.DropTable(
                name: "Model",
                schema: "Car");

            migrationBuilder.DropTable(
                name: "PaymentMethod",
                schema: "Maintenance");

            migrationBuilder.DropTable(
                name: "Transmission",
                schema: "Car");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Identity");
        }
    }
}
