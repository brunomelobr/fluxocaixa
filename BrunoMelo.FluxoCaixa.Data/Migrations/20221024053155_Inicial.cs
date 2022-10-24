using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrunoMelo.FluxoCaixa.Data.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Manutencao");

            migrationBuilder.EnsureSchema(
                name: "Seguranca");

            migrationBuilder.EnsureSchema(
                name: "Apoio");

            migrationBuilder.EnsureSchema(
                name: "Operacional");

            migrationBuilder.CreateTable(
                name: "Categoria",
                schema: "Manutencao",
                columns: table => new
                {
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.CategoriaId);
                });

            migrationBuilder.CreateTable(
                name: "Conta",
                schema: "Manutencao",
                columns: table => new
                {
                    ContaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conta", x => x.ContaId);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "Seguranca",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoTransacao",
                schema: "Apoio",
                columns: table => new
                {
                    TipoTransacaoId = table.Column<short>(type: "smallint", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoTransacao", x => x.TipoTransacaoId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Seguranca",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SistemaId = table.Column<short>(type: "smallint", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityRoleClaim",
                schema: "Seguranca",
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
                    table.PrimaryKey("PK_IdentityRoleClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdentityRoleClaim_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Seguranca",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transacao",
                schema: "Operacional",
                columns: table => new
                {
                    TransacaoId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoTransacaoId = table.Column<short>(type: "smallint", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    ContaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacao", x => x.TransacaoId);
                    table.ForeignKey(
                        name: "FK_Transacao_Categoria_CategoriaId",
                        column: x => x.CategoriaId,
                        principalSchema: "Manutencao",
                        principalTable: "Categoria",
                        principalColumn: "CategoriaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transacao_Conta_ContaId",
                        column: x => x.ContaId,
                        principalSchema: "Manutencao",
                        principalTable: "Conta",
                        principalColumn: "ContaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transacao_TipoTransacao_TipoTransacaoId",
                        column: x => x.TipoTransacaoId,
                        principalSchema: "Apoio",
                        principalTable: "TipoTransacao",
                        principalColumn: "TipoTransacaoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IdentityUserClaim",
                schema: "Seguranca",
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
                    table.PrimaryKey("PK_IdentityUserClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdentityUserClaim_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Seguranca",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityUserLogin",
                schema: "Seguranca",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserLogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_IdentityUserLogin_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Seguranca",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityUserToken",
                schema: "Seguranca",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserToken", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_IdentityUserToken_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Seguranca",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                schema: "Seguranca",
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
                        principalSchema: "Seguranca",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Seguranca",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Manutencao",
                table: "Categoria",
                columns: new[] { "CategoriaId", "Nome" },
                values: new object[,]
                {
                    { 1, "Alimentação" },
                    { 2, "Lazer" },
                    { 3, "Transporte" },
                    { 4, "Saúde" },
                    { 5, "Salário" },
                    { 6, "Dividendos" }
                });

            migrationBuilder.InsertData(
                schema: "Manutencao",
                table: "Conta",
                columns: new[] { "ContaId", "Nome" },
                values: new object[,]
                {
                    { 1, "Conta Corrente" },
                    { 2, "Carteira" }
                });

            migrationBuilder.InsertData(
                schema: "Seguranca",
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, "1c8c30c9-6aad-46c3-8a6b-553397299ef9", "Administrador", "Administrador" },
                    { 2, "483bc2e0-7956-4ad9-863c-b29c721e1084", "Usuário", "Usuário" }
                });

            migrationBuilder.InsertData(
                schema: "Apoio",
                table: "TipoTransacao",
                columns: new[] { "TipoTransacaoId", "Nome" },
                values: new object[,]
                {
                    { (short)1, "Crédito" },
                    { (short)2, "Débito" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categoria",
                schema: "Manutencao",
                table: "Categoria",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Conta",
                schema: "Manutencao",
                table: "Conta",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityRoleClaim_RoleId",
                schema: "Seguranca",
                table: "IdentityRoleClaim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUserClaim_UserId",
                schema: "Seguranca",
                table: "IdentityUserClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUserLogin_UserId",
                schema: "Seguranca",
                table: "IdentityUserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "Seguranca",
                table: "Role",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TipoTransacao",
                schema: "Apoio",
                table: "TipoTransacao",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transacao_CategoriaId",
                schema: "Operacional",
                table: "Transacao",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacao_ContaId",
                schema: "Operacional",
                table: "Transacao",
                column: "ContaId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacao_TipoTransacaoId",
                schema: "Operacional",
                table: "Transacao",
                column: "TipoTransacaoId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "Seguranca",
                table: "User",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "Seguranca",
                table: "User",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                schema: "Seguranca",
                table: "UserRole",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityRoleClaim",
                schema: "Seguranca");

            migrationBuilder.DropTable(
                name: "IdentityUserClaim",
                schema: "Seguranca");

            migrationBuilder.DropTable(
                name: "IdentityUserLogin",
                schema: "Seguranca");

            migrationBuilder.DropTable(
                name: "IdentityUserToken",
                schema: "Seguranca");

            migrationBuilder.DropTable(
                name: "Transacao",
                schema: "Operacional");

            migrationBuilder.DropTable(
                name: "UserRole",
                schema: "Seguranca");

            migrationBuilder.DropTable(
                name: "Categoria",
                schema: "Manutencao");

            migrationBuilder.DropTable(
                name: "Conta",
                schema: "Manutencao");

            migrationBuilder.DropTable(
                name: "TipoTransacao",
                schema: "Apoio");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "Seguranca");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Seguranca");
        }
    }
}
