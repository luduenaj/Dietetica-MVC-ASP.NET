using Microsoft.EntityFrameworkCore.Migrations;

namespace Dietetica.Data.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "proveedores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(nullable: false),
                    telefono = table.Column<string>(nullable: false),
                    email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_proveedores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tiposVentas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tipoDeVenta = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tiposVentas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "frutosSecos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(nullable: true),
                    precioXKg = table.Column<int>(nullable: false),
                    idTipoVenta = table.Column<int>(nullable: false),
                    tipoVentaId = table.Column<int>(nullable: true),
                    idProveedor = table.Column<int>(nullable: false),
                    proveedorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_frutosSecos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_frutosSecos_proveedores_proveedorId",
                        column: x => x.proveedorId,
                        principalTable: "proveedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_frutosSecos_tiposVentas_tipoVentaId",
                        column: x => x.tipoVentaId,
                        principalTable: "tiposVentas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "productosEmbasados",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(nullable: true),
                    precioPorUnidad = table.Column<int>(nullable: false),
                    gramos = table.Column<int>(nullable: false),
                    foto = table.Column<string>(nullable: true),
                    IdTipoVenta = table.Column<int>(nullable: false),
                    tipoVentaId = table.Column<int>(nullable: true),
                    IdProveedor = table.Column<int>(nullable: false),
                    proveedorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productosEmbasados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_productosEmbasados_proveedores_proveedorId",
                        column: x => x.proveedorId,
                        principalTable: "proveedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_productosEmbasados_tiposVentas_tipoVentaId",
                        column: x => x.tipoVentaId,
                        principalTable: "tiposVentas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "semillas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(nullable: true),
                    precioXKg = table.Column<int>(nullable: false),
                    idTipoVenta = table.Column<int>(nullable: false),
                    tipoVentaId = table.Column<int>(nullable: true),
                    idProveedor = table.Column<int>(nullable: false),
                    proveedorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_semillas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_semillas_proveedores_proveedorId",
                        column: x => x.proveedorId,
                        principalTable: "proveedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_semillas_tiposVentas_tipoVentaId",
                        column: x => x.tipoVentaId,
                        principalTable: "tiposVentas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tesEnHebras",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(nullable: true),
                    precioX100gr = table.Column<int>(nullable: false),
                    IdTipoVenta = table.Column<int>(nullable: false),
                    tipoVentaId = table.Column<int>(nullable: true),
                    IdProveedor = table.Column<int>(nullable: false),
                    proveedorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tesEnHebras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tesEnHebras_proveedores_proveedorId",
                        column: x => x.proveedorId,
                        principalTable: "proveedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tesEnHebras_tiposVentas_tipoVentaId",
                        column: x => x.tipoVentaId,
                        principalTable: "tiposVentas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_frutosSecos_proveedorId",
                table: "frutosSecos",
                column: "proveedorId");

            migrationBuilder.CreateIndex(
                name: "IX_frutosSecos_tipoVentaId",
                table: "frutosSecos",
                column: "tipoVentaId");

            migrationBuilder.CreateIndex(
                name: "IX_productosEmbasados_proveedorId",
                table: "productosEmbasados",
                column: "proveedorId");

            migrationBuilder.CreateIndex(
                name: "IX_productosEmbasados_tipoVentaId",
                table: "productosEmbasados",
                column: "tipoVentaId");

            migrationBuilder.CreateIndex(
                name: "IX_semillas_proveedorId",
                table: "semillas",
                column: "proveedorId");

            migrationBuilder.CreateIndex(
                name: "IX_semillas_tipoVentaId",
                table: "semillas",
                column: "tipoVentaId");

            migrationBuilder.CreateIndex(
                name: "IX_tesEnHebras_proveedorId",
                table: "tesEnHebras",
                column: "proveedorId");

            migrationBuilder.CreateIndex(
                name: "IX_tesEnHebras_tipoVentaId",
                table: "tesEnHebras",
                column: "tipoVentaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "frutosSecos");

            migrationBuilder.DropTable(
                name: "productosEmbasados");

            migrationBuilder.DropTable(
                name: "semillas");

            migrationBuilder.DropTable(
                name: "tesEnHebras");

            migrationBuilder.DropTable(
                name: "proveedores");

            migrationBuilder.DropTable(
                name: "tiposVentas");
        }
    }
}
