using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teste.Api.Migrations
{
    /// <inheritdoc />
    public partial class IndicesUnicos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Complemento",
                table: "Clientes",
                type: "TEXT",
                maxLength: 60,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_Email",
                table: "Clientes",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_Telefone",
                table: "Clientes",
                column: "Telefone",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Clientes_Email",
                table: "Clientes");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_Telefone",
                table: "Clientes");

            migrationBuilder.AlterColumn<string>(
                name: "Complemento",
                table: "Clientes",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 60,
                oldNullable: true);
        }
    }
}
