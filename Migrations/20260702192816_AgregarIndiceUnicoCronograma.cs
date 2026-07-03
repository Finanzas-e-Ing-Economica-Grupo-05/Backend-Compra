using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend_finanzas_grupo5.Migrations
{
    /// <inheritdoc />
    public partial class AgregarIndiceUnicoCronograma : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CronogramaPagos_IdSimulacion_Mes",
                table: "CronogramaPagos",
                columns: new[] { "IdSimulacion", "Mes" },
                unique: true);

            migrationBuilder.DropIndex(
                name: "IX_CronogramaPagos_IdSimulacion",
                table: "CronogramaPagos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CronogramaPagos_IdSimulacion",
                table: "CronogramaPagos",
                column: "IdSimulacion");

            migrationBuilder.DropIndex(
                name: "IX_CronogramaPagos_IdSimulacion_Mes",
                table: "CronogramaPagos");
        }
    }
}
