using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntreEspeciesNuevo.Migrations
{
    /// <inheritdoc />
    public partial class prueba : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categorias",
                columns: table => new
                {
                    Id_Categoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom_Categoria = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Estado = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__categori__CB90334926310FE0", x => x.Id_Categoria);
                });

            migrationBuilder.CreateTable(
                name: "clientes",
                columns: table => new
                {
                    Documento_Cliente = table.Column<int>(type: "int", nullable: false),
                    Tipo_Docu = table.Column<string>(type: "varchar(35)", unicode: false, maxLength: 35, nullable: true),
                    Nombre_Cliente = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Direccion = table.Column<string>(type: "varchar(35)", unicode: false, maxLength: 35, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correo = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__clientes__B64AC97392296B2D", x => x.Documento_Cliente);
                });

            migrationBuilder.CreateTable(
                name: "permisos",
                columns: table => new
                {
                    Id_Permiso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom_Permiso = table.Column<string>(type: "varchar(70)", unicode: false, maxLength: 70, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__permisos__153CFB6D328BCFAA", x => x.Id_Permiso);
                });

            migrationBuilder.CreateTable(
                name: "proveedores",
                columns: table => new
                {
                    Id_Proveedor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nit_Proveedor = table.Column<int>(type: "int", nullable: false),
                    Nom_Proveedor = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Correo = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    contacto = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__proveedo__477B858E1EDA75DB", x => x.Id_Proveedor);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    Id_Rol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom_Rol = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__roles__55932E86C5A7B610", x => x.Id_Rol);
                });

            migrationBuilder.CreateTable(
                name: "servicios",
                columns: table => new
                {
                    Id_Servicio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom_Servico = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false),
                    Categoria = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false),
                    Precio = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__servicio__5B1345F0BD5626CF", x => x.Id_Servicio);
                });

            migrationBuilder.CreateTable(
                name: "productos",
                columns: table => new
                {
                    Id_Producto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Categoria = table.Column<int>(type: "int", nullable: false),
                    Nom_producto = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: false),
                    Disponibilidad = table.Column<int>(type: "int", unicode: false, fixedLength: true, maxLength: 15, nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Fecha_Ven = table.Column<DateTime>(type: "date", nullable: true),
                    Precio = table.Column<int>(type: "int", nullable: false),
                    Foto = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__producto__2085A9CF5B8C577C", x => x.Id_Producto);
                    table.ForeignKey(
                        name: "FK__productos__Id_Ca__7E37BEF6",
                        column: x => x.Id_Categoria,
                        principalTable: "categorias",
                        principalColumn: "Id_Categoria",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mascotas",
                columns: table => new
                {
                    Id_Mascota = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Documento_Cliente = table.Column<int>(type: "int", nullable: false),
                    Nombre_Mascota = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false),
                    Fecha_Nacimiento = table.Column<DateTime>(type: "date", nullable: false),
                    Color_Mascota = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    Especie = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Raza = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Genero = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    Inf_Mascota = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__mascotas__C7A382FEB5C42430", x => x.Id_Mascota);
                    table.ForeignKey(
                        name: "FK__mascotas__Inf_Ma__4BAC3F29",
                        column: x => x.Documento_Cliente,
                        principalTable: "clientes",
                        principalColumn: "Documento_Cliente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ventas",
                columns: table => new
                {
                    Id_Venta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Categoria = table.Column<int>(type: "int", nullable: true),
                    Documento_Cliente = table.Column<int>(type: "int", nullable: true),
                    Fecha_Venta = table.Column<DateTime>(type: "datetime", nullable: true),
                    Forma_Pago = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    Total = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ventas__B3C9EABD9CF5DD9D", x => x.Id_Venta);
                    table.ForeignKey(
                        name: "FK__productos__Id_Ca__7E37BEF9",
                        column: x => x.Id_Categoria,
                        principalTable: "categorias",
                        principalColumn: "Id_Categoria");
                    table.ForeignKey(
                        name: "FK__ventas__Document__74AE54BC",
                        column: x => x.Documento_Cliente,
                        principalTable: "clientes",
                        principalColumn: "Documento_Cliente");
                });

            migrationBuilder.CreateTable(
                name: "compras",
                columns: table => new
                {
                    Id_Compra = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_proveedor = table.Column<int>(type: "int", nullable: true),
                    Id_Categoria = table.Column<int>(type: "int", nullable: true),
                    Fecha_Compra = table.Column<DateTime>(type: "date", nullable: true),
                    Forma_Pago = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Total = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__compras__661E0ED012267630", x => x.Id_Compra);
                    table.ForeignKey(
                        name: "FK__compras__Id_pr__7D439ABD",
                        column: x => x.Id_proveedor,
                        principalTable: "proveedores",
                        principalColumn: "Id_Proveedor");
                    table.ForeignKey(
                        name: "FK__productos__Id_Ca__7E37BEF8",
                        column: x => x.Id_Categoria,
                        principalTable: "categorias",
                        principalColumn: "Id_Categoria");
                });

            migrationBuilder.CreateTable(
                name: "configuracion",
                columns: table => new
                {
                    Id_Configuracion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Rol = table.Column<int>(type: "int", nullable: true),
                    Id_Permiso = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__configur__73F5BB5AD557B58D", x => x.Id_Configuracion);
                    table.ForeignKey(
                        name: "FK__configura__Id_Pe__5BE2A6F2",
                        column: x => x.Id_Permiso,
                        principalTable: "permisos",
                        principalColumn: "Id_Permiso");
                    table.ForeignKey(
                        name: "FK__configura__Id_Ro__5AEE82B9",
                        column: x => x.Id_Rol,
                        principalTable: "roles",
                        principalColumn: "Id_Rol");
                });

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    Id_Usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Rol = table.Column<int>(type: "int", nullable: true),
                    Nombre = table.Column<string>(type: "varchar(35)", unicode: false, maxLength: 35, nullable: false),
                    Tipo_Doc = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    Documento = table.Column<int>(type: "int", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Correo = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false),
                    Contraseña = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: true),
                    Estado = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    Direccion = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    ResetToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetTokenExpiration = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__usuario__63C76BE2073CA765", x => x.Id_Usuario);
                    table.ForeignKey(
                        name: "FK__usuario__Id_Rol__5EBF139D",
                        column: x => x.Id_Rol,
                        principalTable: "roles",
                        principalColumn: "Id_Rol");
                });

            migrationBuilder.CreateTable(
                name: "cita_internas",
                columns: table => new
                {
                    Id_Cita_Int = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Mascota = table.Column<int>(type: "int", nullable: true),
                    Documento_Cliente = table.Column<int>(type: "int", nullable: false),
                    Id_Servicio = table.Column<int>(type: "int", nullable: false),
                    Nom_Cita = table.Column<string>(type: "varchar(35)", unicode: false, maxLength: 35, nullable: true),
                    PersonaCargo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Fecha_Hora = table.Column<DateTime>(type: "datetime", nullable: false),
                    Estado = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Precio = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__cita_int__499F1189AA330D6D", x => x.Id_Cita_Int);
                    table.ForeignKey(
                        name: "FK__cita_inte__Docum__5DCAEF64",
                        column: x => x.Documento_Cliente,
                        principalTable: "clientes",
                        principalColumn: "Documento_Cliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__cita_inte__Id_Ma__5CD6CB2B",
                        column: x => x.Id_Mascota,
                        principalTable: "mascotas",
                        principalColumn: "Id_Mascota");
                    table.ForeignKey(
                        name: "FK__cita_inte__Id_Ser__5CD6CB2C",
                        column: x => x.Id_Servicio,
                        principalTable: "servicios",
                        principalColumn: "Id_Servicio",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "deta_compras",
                columns: table => new
                {
                    Id_Deta_Compra = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Compra = table.Column<int>(type: "int", nullable: false),
                    Id_Producto = table.Column<int>(type: "int", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: true),
                    Precio_Compra = table.Column<int>(type: "int", nullable: true),
                    Subtotal = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__deta_com__9C865E39DEF429E1", x => x.Id_Deta_Compra);
                    table.ForeignKey(
                        name: "FK__deta_comp__Id_Co__09A971A2",
                        column: x => x.Id_Compra,
                        principalTable: "compras",
                        principalColumn: "Id_Compra",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__deta_comp__Id_Pr__0A9D95DB",
                        column: x => x.Id_Producto,
                        principalTable: "productos",
                        principalColumn: "Id_Producto");
                });

            migrationBuilder.CreateTable(
                name: "deta_venta",
                columns: table => new
                {
                    Id_detalle_V = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Venta = table.Column<int>(type: "int", nullable: false),
                    Id_Producto = table.Column<int>(type: "int", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: true),
                    id_cita = table.Column<int>(type: "int", nullable: true),
                    Sub_Total_Pro = table.Column<int>(type: "int", nullable: true),
                    Sub_Total_Ser = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__deta_ven__540046FFEEFC1F39", x => x.Id_detalle_V);
                    table.ForeignKey(
                        name: "FK__deta_vent__Id_Ci__4BAC3F28",
                        column: x => x.id_cita,
                        principalTable: "cita_internas",
                        principalColumn: "Id_Cita_Int");
                    table.ForeignKey(
                        name: "FK__deta_vent__Id_Pr__04E4BC85",
                        column: x => x.Id_Producto,
                        principalTable: "productos",
                        principalColumn: "Id_Producto");
                    table.ForeignKey(
                        name: "FK__deta_vent__Id_Ve__02FC7413",
                        column: x => x.Id_Venta,
                        principalTable: "ventas",
                        principalColumn: "Id_Venta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cita_internas_Documento_Cliente",
                table: "cita_internas",
                column: "Documento_Cliente");

            migrationBuilder.CreateIndex(
                name: "IX_cita_internas_Id_Mascota",
                table: "cita_internas",
                column: "Id_Mascota");

            migrationBuilder.CreateIndex(
                name: "IX_cita_internas_Id_Servicio",
                table: "cita_internas",
                column: "Id_Servicio");

            migrationBuilder.CreateIndex(
                name: "IX_compras_Id_Categoria",
                table: "compras",
                column: "Id_Categoria");

            migrationBuilder.CreateIndex(
                name: "IX_compras_Id_proveedor",
                table: "compras",
                column: "Id_proveedor");

            migrationBuilder.CreateIndex(
                name: "IX_configuracion_Id_Permiso",
                table: "configuracion",
                column: "Id_Permiso");

            migrationBuilder.CreateIndex(
                name: "IX_configuracion_Id_Rol",
                table: "configuracion",
                column: "Id_Rol");

            migrationBuilder.CreateIndex(
                name: "IX_deta_compras_Id_Compra",
                table: "deta_compras",
                column: "Id_Compra");

            migrationBuilder.CreateIndex(
                name: "IX_deta_compras_Id_Producto",
                table: "deta_compras",
                column: "Id_Producto");

            migrationBuilder.CreateIndex(
                name: "IX_deta_venta_id_cita",
                table: "deta_venta",
                column: "id_cita");

            migrationBuilder.CreateIndex(
                name: "IX_deta_venta_Id_Producto",
                table: "deta_venta",
                column: "Id_Producto");

            migrationBuilder.CreateIndex(
                name: "IX_deta_venta_Id_Venta",
                table: "deta_venta",
                column: "Id_Venta");

            migrationBuilder.CreateIndex(
                name: "IX_mascotas_Documento_Cliente",
                table: "mascotas",
                column: "Documento_Cliente");

            migrationBuilder.CreateIndex(
                name: "IX_productos_Id_Categoria",
                table: "productos",
                column: "Id_Categoria");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_Id_Rol",
                table: "usuario",
                column: "Id_Rol");

            migrationBuilder.CreateIndex(
                name: "IX_ventas_Documento_Cliente",
                table: "ventas",
                column: "Documento_Cliente");

            migrationBuilder.CreateIndex(
                name: "IX_ventas_Id_Categoria",
                table: "ventas",
                column: "Id_Categoria");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "configuracion");

            migrationBuilder.DropTable(
                name: "deta_compras");

            migrationBuilder.DropTable(
                name: "deta_venta");

            migrationBuilder.DropTable(
                name: "usuario");

            migrationBuilder.DropTable(
                name: "permisos");

            migrationBuilder.DropTable(
                name: "compras");

            migrationBuilder.DropTable(
                name: "cita_internas");

            migrationBuilder.DropTable(
                name: "productos");

            migrationBuilder.DropTable(
                name: "ventas");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "proveedores");

            migrationBuilder.DropTable(
                name: "mascotas");

            migrationBuilder.DropTable(
                name: "servicios");

            migrationBuilder.DropTable(
                name: "categorias");

            migrationBuilder.DropTable(
                name: "clientes");
        }
    }
}
