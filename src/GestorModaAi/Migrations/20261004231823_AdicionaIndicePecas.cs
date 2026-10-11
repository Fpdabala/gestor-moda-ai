using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestorModaAi.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaIndicePecas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Pecas_Nome",
                table: "Pecas",
                column: "Nome");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pecas_Nome",
                table: "Pecas");
        }
    }
}
