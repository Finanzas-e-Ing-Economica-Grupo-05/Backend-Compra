using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend_finanzas_grupo5.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UsuNombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UsuApellido = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UsuDni = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UsuPassword = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UsuCorreo = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UsuNumero = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UsuPerfilSe = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IdUsuario);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Vehiculos",
                columns: table => new
                {
                    IdVehiculo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VehMarca = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VehPrecio = table.Column<double>(type: "double", nullable: false),
                    VehMoneda = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehiculos", x => x.IdVehiculo);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Simulaciones",
                columns: table => new
                {
                    IdSimulacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdVehiculo = table.Column<int>(type: "int", nullable: false),
                    FinFecInicio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FinIniMonto = table.Column<double>(type: "double", nullable: false),
                    FinBalonPorc = table.Column<double>(type: "double", nullable: false),
                    FinPlazo = table.Column<int>(type: "int", nullable: false),
                    TasaValor = table.Column<double>(type: "double", nullable: false),
                    TasaTipo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TasaCapit = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GraTipo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GraMeses = table.Column<int>(type: "int", nullable: false),
                    SegDesgrav = table.Column<double>(type: "double", nullable: false),
                    SegVehic = table.Column<double>(type: "double", nullable: false),
                    ValCok = table.Column<double>(type: "double", nullable: false),
                    ComPortes = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Simulaciones", x => x.IdSimulacion);
                    table.ForeignKey(
                        name: "FK_Simulaciones_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Simulaciones_Vehiculos_IdVehiculo",
                        column: x => x.IdVehiculo,
                        principalTable: "Vehiculos",
                        principalColumn: "IdVehiculo",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CronogramaPagos",
                columns: table => new
                {
                    IdCronograma = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdSimulacion = table.Column<int>(type: "int", nullable: false),
                    Mes = table.Column<int>(type: "int", nullable: false),
                    TipoCuota = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SaldoInicial = table.Column<double>(type: "double", nullable: false),
                    AmoPeriodo = table.Column<double>(type: "double", nullable: false),
                    IntPeriodo = table.Column<double>(type: "double", nullable: false),
                    InteresCapitalizado = table.Column<double>(type: "double", nullable: true),
                    SegDesMonto = table.Column<double>(type: "double", nullable: false),
                    SegVehMonto = table.Column<double>(type: "double", nullable: false),
                    CuoMensual = table.Column<double>(type: "double", nullable: false),
                    MontoBalonPagado = table.Column<double>(type: "double", nullable: true),
                    PagoTotal = table.Column<double>(type: "double", nullable: false),
                    SaldoFinal = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CronogramaPagos", x => x.IdCronograma);
                    table.ForeignKey(
                        name: "FK_CronogramaPagos_Simulaciones_IdSimulacion",
                        column: x => x.IdSimulacion,
                        principalTable: "Simulaciones",
                        principalColumn: "IdSimulacion",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ResultadosSimulacion",
                columns: table => new
                {
                    IdResultado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdSimulacion = table.Column<int>(type: "int", nullable: false),
                    MontoTotalFin = table.Column<double>(type: "double", nullable: false),
                    MontoCuotaBalon = table.Column<double>(type: "double", nullable: false),
                    Tem = table.Column<double>(type: "double", nullable: false),
                    ValTcea = table.Column<double>(type: "double", nullable: false),
                    ValVan = table.Column<double>(type: "double", nullable: false),
                    ValTir = table.Column<double>(type: "double", nullable: false),
                    MonInteresTot = table.Column<double>(type: "double", nullable: false),
                    MonSeguroTot = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadosSimulacion", x => x.IdResultado);
                    table.ForeignKey(
                        name: "FK_ResultadosSimulacion_Simulaciones_IdSimulacion",
                        column: x => x.IdSimulacion,
                        principalTable: "Simulaciones",
                        principalColumn: "IdSimulacion",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CronogramaPagos_IdSimulacion",
                table: "CronogramaPagos",
                column: "IdSimulacion");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadosSimulacion_IdSimulacion",
                table: "ResultadosSimulacion",
                column: "IdSimulacion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Simulaciones_IdUsuario",
                table: "Simulaciones",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Simulaciones_IdVehiculo",
                table: "Simulaciones",
                column: "IdVehiculo");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_UsuCorreo",
                table: "Usuarios",
                column: "UsuCorreo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_UsuDni",
                table: "Usuarios",
                column: "UsuDni",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CronogramaPagos");

            migrationBuilder.DropTable(
                name: "ResultadosSimulacion");

            migrationBuilder.DropTable(
                name: "Simulaciones");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Vehiculos");
        }
    }
}
