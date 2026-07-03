using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend_finanzas_grupo5.Migrations
{
    /// <inheritdoc />
    public partial class AgregarImagenYModeloVehiculo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VehImagenUrl",
                table: "Vehiculos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "VehModelo",
                table: "Vehiculos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VehImagenUrl",
                table: "Vehiculos");

            migrationBuilder.DropColumn(
                name: "VehModelo",
                table: "Vehiculos");
        }
    }
}
