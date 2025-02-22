﻿// <auto-generated />
using System;
using EntreEspeciesNuevo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EntreEspeciesNuevo.Migrations
{
    [DbContext(typeof(EntreespeciessqlContext))]
    partial class EntreespeciessqlContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EntreEspeciesNuevo.Models.Categoria", b =>
                {
                    b.Property<int>("IdCategoria")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_Categoria");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCategoria"));

                    b.Property<string>("Estado")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("Estado");

                    b.Property<string>("NomCategoria")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("Nom_Categoria");

                    b.HasKey("IdCategoria")
                        .HasName("PK__categori__CB90334926310FE0");

                    b.ToTable("categorias", (string)null);
                });

            modelBuilder.Entity("EntreEspeciesNuevo.Models.Cliente", b =>
                {
                    b.Property<int>("DocumentoCliente")
                        .HasColumnType("int")
                        .HasColumnName("Documento_Cliente");

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasMaxLength(40)
                        .IsUnicode(false)
                        .HasColumnType("varchar(40)");

                    b.Property<string>("Direccion")
                        .IsRequired()
                        .HasMaxLength(35)
                        .IsUnicode(false)
                        .HasColumnType("varchar(35)");

                    b.Property<string>("NombreCliente")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("Nombre_Cliente");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TipoDocu")
                        .HasMaxLength(35)
                        .IsUnicode(false)
                        .HasColumnType("varchar(35)")
                        .HasColumnName("Tipo_Docu");

                    b.HasKey("DocumentoCliente")
                        .HasName("PK__clientes__B64AC97392296B2D");

                    b.ToTable("clientes", (string)null);
                });

            modelBuilder.Entity("EntreEspeciesNuevo.Models.Compra", b =>
                {
                    b.Property<int>("IdCompra")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_Compra");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCompra"));

                    b.Property<DateTime?>("FechaCompra")
                        .HasColumnType("date")
                        .HasColumnName("Fecha_Compra");

                    b.Property<string>("FormaPago")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("Forma_Pago");

                    b.Property<int?>("IdCategoria")
                        .HasColumnType("int")
                        .HasColumnName("Id_Categoria");

                    b.Property<int?>("IdProveedor")
                        .HasColumnType("int")
                        .HasColumnName("Id_proveedor");

                    b.Property<int?>("Total")
                        .HasColumnType("int")
                        .HasColumnName("Total");

                    b.HasKey("IdCompra")
                        .HasName("PK__compras__661E0ED012267630");

                    b.HasIndex("IdCategoria");

                    b.HasIndex("IdProveedor");

                    b.ToTable("compras", (string)null);
                });

            modelBuilder.Entity("EntreEspeciesNuevo.Models.Configuracion", b =>
                {
                    b.Property<int>("IdConfiguracion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_Configuracion");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdConfiguracion"));

                    b.Property<int?>("IdPermiso")
                        .HasColumnType("int")
                        .HasColumnName("Id_Permiso");

                    b.Property<int?>("IdRol")
                        .HasColumnType("int")
                        .HasColumnName("Id_Rol");

                    b.HasKey("IdConfiguracion")
                        .HasName("PK__configur__73F5BB5AD557B58D");

                    b.HasIndex("IdPermiso");

                    b.HasIndex("IdRol");

                    b.ToTable("configuracion", (string)null);
                });

            modelBuilder.Entity("EntreEspeciesNuevo.Models.DetaCompra", b =>
                {
                    b.Property<int>("IdDetaCompra")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_Deta_Compra");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdDetaCompra"));

                    b.Property<int?>("Cantidad")
                        .HasColumnType("int");

                    b.Property<int>("IdCompra")
                        .HasColumnType("int")
                        .HasColumnName("Id_Compra");

                    b.Property<int?>("IdProducto")
                        .HasColumnType("int")
                        .HasColumnName("Id_Producto");

                    b.Property<int?>("PrecioCompra")
                        .HasColumnType("int")
                        .HasColumnName("Precio_Compra");

                    b.Property<int>("Subtotal")
                        .HasColumnType("int");

                    b.HasKey("IdDetaCompra")
                        .HasName("PK__deta_com__9C865E39DEF429E1");

                    b.HasIndex("IdCompra");

                    b.HasIndex("IdProducto");

                    b.ToTable("deta_compras", (string)null);
                });

            modelBuilder.Entity("EntreEspeciesNuevo.Models.DetaVentum", b =>
                {
                    b.Property<int>("IdDetalleV")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_detalle_V");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdDetalleV"));

                    b.Property<int?>("Cantidad")
                        .HasColumnType("int");

                    b.Property<int?>("IdCitaInterna")
                        .HasColumnType("int")
                        .HasColumnName("id_cita");

                    b.Property<int?>("IdProducto")
                        .HasColumnType("int")
                        .HasColumnName("Id_Producto");

                    b.Property<int>("IdVenta")
                        .HasColumnType("int")
                        .HasColumnName("Id_Venta");

                    b.Property<int?>("SubTotalPro")
                        .HasColumnType("int")
                        .HasColumnName("Sub_Total_Pro");

                    b.Property<int?>("SubTotalSer")
                        .HasColumnType("int")
                        .HasColumnName("Sub_Total_Ser");

                    b.HasKey("IdDetalleV")
                        .HasName("PK__deta_ven__540046FFEEFC1F39");

                    b.HasIndex("IdCitaInterna");

                    b.HasIndex("IdProducto");

                    b.HasIndex("IdVenta");

                    b.ToTable("deta_venta", (string)null);
                });

            modelBuilder.Entity("EntreEspeciesNuevo.Models.Mascota", b =>
                {
                    b.Property<int>("IdMascota")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_Mascota");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdMascota"));

                    b.Property<string>("ColorMascota")
                        .IsRequired()
                        .HasMaxLength(15)
                        .IsUnicode(false)
                        .HasColumnType("varchar(15)")
                        .HasColumnName("Color_Mascota");

                    b.Property<int?>("DocumentoCliente")
                        .IsRequired()
                        .HasColumnType("int")
                        .HasColumnName("Documento_Cliente");

                    b.Property<string>("Especie")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime?>("FechaNacimiento")
                        .IsRequired()
                        .HasColumnType("date")
                        .HasColumnName("Fecha_Nacimiento");

                    b.Property<string>("Genero")
                        .HasMaxLength(15)
                        .IsUnicode(false)
                        .HasColumnType("varchar(15)");

                    b.Property<string>("InfMascota")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("Inf_Mascota");

                    b.Property<string>("NombreMascota")
                        .IsRequired()
                        .HasMaxLength(25)
                        .IsUnicode(false)
                        .HasColumnType("varchar(25)")
                        .HasColumnName("Nombre_Mascota");

                    b.Property<string>("Raza")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.HasKey("IdMascota")
                        .HasName("PK__mascotas__C7A382FEB5C42430");

                    b.HasIndex("DocumentoCliente");

                    b.ToTable("mascotas", (string)null);
                });

            modelBuilder.Entity("EntreEspeciesNuevo.Models.Permiso", b =>
                {
                    b.Property<int>("IdPermiso")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_Permiso");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdPermiso"));

                    b.Property<string>("NomPermiso")
                        .HasMaxLength(70)
                        .IsUnicode(false)
                        .HasColumnType("varchar(70)")
                        .HasColumnName("Nom_Permiso");

                    b.HasKey("IdPermiso")
                        .HasName("PK__permisos__153CFB6D328BCFAA");

                    b.ToTable("permisos", (string)null);
                });

            modelBuilder.Entity("EntreEspeciesNuevo.Models.Producto", b =>
                {
                    b.Property<int>("IdProducto")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_Producto");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdProducto"));

                    b.Property<int?>("Cantidad")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int>("Disponibilidad")
                        .HasMaxLength(15)
                        .IsUnicode(false)
                        .HasColumnType("int")
                        .IsFixedLength();

                    b.Property<DateTime?>("FechaVen")
                        .HasColumnType("date")
                        .HasColumnName("Fecha_Ven");

                    b.Property<byte[]>("Foto")
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("IdCategoria")
                        .HasColumnType("int")
                        .HasColumnName("Id_Categoria");

                    b.Property<string>("NomProducto")
                        .IsRequired()
                        .HasMaxLength(45)
                        .IsUnicode(false)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("Nom_producto");

                    b.Property<int>("Precio")
                        .HasColumnType("int");

                    b.HasKey("IdProducto")
                        .HasName("PK__producto__2085A9CF5B8C577C");

                    b.HasIndex("IdCategoria");

                    b.ToTable("productos", (string)null);
                });

            modelBuilder.Entity("EntreEspeciesNuevo.Models.Proveedore", b =>
                {
                    b.Property<int>("IdProveedor")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_Proveedor");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdProveedor"));

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("Direccion")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<int?>("NitProveedor")
                        .IsRequired()
                        .HasColumnType("int")
                        .HasColumnName("Nit_Proveedor");

                    b.Property<string>("NomProveedor")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("Nom_Proveedor");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("contacto")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("IdProveedor")
                        .HasName("PK__proveedo__477B858E1EDA75DB");

                    b.ToTable("proveedores", (string)null);
                });

            modelBuilder.Entity("EntreEspeciesNuevo.Models.Role", b =>
                {
                    b.Property<int>("IdRol")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_Rol");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdRol"));

                    b.Property<string>("NomRol")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("Nom_Rol");

                    b.HasKey("IdRol")
                        .HasName("PK__roles__55932E86C5A7B610");

                    b.ToTable("roles", (string)null);
                });

            modelBuilder.Entity("EntreEspeciesNuevo.Models.Servicio", b =>
                {
                    b.Property<int>("IdServicio")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_Servicio");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdServicio"));

                    b.Property<string>("Categoria")
                        .IsRequired()
                        .HasMaxLength(25)
                        .IsUnicode(false)
                        .HasColumnType("varchar(25)");

                    b.Property<string>("NomServico")
                        .IsRequired()
                        .HasMaxLength(25)
                        .IsUnicode(false)
                        .HasColumnType("varchar(25)")
                        .HasColumnName("Nom_Servico");

                    b.Property<int?>("Precio")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("IdServicio")
                        .HasName("PK__servicio__5B1345F0BD5626CF");

                    b.ToTable("servicios", (string)null);
                });

            modelBuilder.Entity("EntreEspeciesNuevo.Models.Usuario", b =>
                {
                    b.Property<int>("IdUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_Usuario");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdUsuario"));

                    b.Property<string>("Contraseña")
                        .HasMaxLength(16)
                        .IsUnicode(false)
                        .HasColumnType("varchar(16)");

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasMaxLength(40)
                        .IsUnicode(false)
                        .HasColumnType("varchar(40)");

                    b.Property<string>("Direccion")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<int?>("Documento")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("Estado")
                        .HasMaxLength(15)
                        .IsUnicode(false)
                        .HasColumnType("varchar(15)");

                    b.Property<int?>("IdRol")
                        .HasColumnType("int")
                        .HasColumnName("Id_Rol");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(35)
                        .IsUnicode(false)
                        .HasColumnType("varchar(35)");

                    b.Property<string>("ResetToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ResetTokenExpiration")
                        .HasColumnType("datetime2");

                    b.Property<string>("Telefono")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TipoDoc")
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)")
                        .HasColumnName("Tipo_Doc");

                    b.HasKey("IdUsuario")
                        .HasName("PK__usuario__63C76BE2073CA765");

                    b.HasIndex("IdRol");

                    b.ToTable("usuario", (string)null);
                });

            modelBuilder.Entity("EntreEspeciesNuevo.Models.Venta", b =>
                {
                    b.Property<int>("IdVenta")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_Venta");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdVenta"));

                    b.Property<int?>("DocumentoCliente")
                        .HasColumnType("int")
                        .HasColumnName("Documento_Cliente");

                    b.Property<DateTime?>("FechaVenta")
                        .HasColumnType("datetime")
                        .HasColumnName("Fecha_Venta");

                    b.Property<string>("FormaPago")
                        .HasMaxLength(15)
                        .IsUnicode(false)
                        .HasColumnType("varchar(15)")
                        .HasColumnName("Forma_Pago");

                    b.Property<int?>("IdCategoria")
                        .HasColumnType("int")
                        .HasColumnName("Id_Categoria");

                    b.Property<int?>("Total")
                        .HasColumnType("int")
                        .HasColumnName("Total");

                    b.HasKey("IdVenta")
                        .HasName("PK__ventas__B3C9EABD9CF5DD9D");

                    b.HasIndex("DocumentoCliente");

                    b.HasIndex("IdCategoria");

                    b.ToTable("ventas", (string)null);
                });

            modelBuilder.Entity("EntreEspeciesNuevo.models.CitaInterna", b =>
                {
                    b.Property<int>("IdCitaInt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_Cita_Int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCitaInt"));

                    b.Property<int?>("DocumentoCliente")
                        .IsRequired()
                        .HasColumnType("int")
                        .HasColumnName("Documento_Cliente");

                    b.Property<string>("Estado")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime?>("FechaHora")
                        .IsRequired()
                        .HasColumnType("datetime")
                        .HasColumnName("Fecha_Hora");

                    b.Property<int?>("IdMascota")
                        .HasColumnType("int")
                        .HasColumnName("Id_Mascota");

                    b.Property<int?>("IdServicio")
                        .IsRequired()
                        .HasColumnType("int")
                        .HasColumnName("Id_Servicio");

                    b.Property<string>("NomCita")
                        .HasMaxLength(35)
                        .IsUnicode(false)
                        .HasColumnType("varchar(35)")
                        .HasColumnName("Nom_Cita");

                    b.Property<string>("PersonaCargo")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("Precio")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("IdCitaInt")
                        .HasName("PK__cita_int__499F1189AA330D6D");

                    b.HasIndex("DocumentoCliente");

                    b.HasIndex("IdMascota");

                    b.HasIndex("IdServicio");

                    b.ToTable("cita_internas", (string)null);
                });

            modelBuilder.Entity("EntreEspeciesNuevo.Models.Compra", b =>
                {
                    b.HasOne("EntreEspeciesNuevo.Models.Categoria", "IdCategoriaNavigation")
                        .WithMany("Compras")
                        .HasForeignKey("IdCategoria")
                        .HasConstraintName("FK__productos__Id_Ca__7E37BEF8");

                    b.HasOne("EntreEspeciesNuevo.Models.Proveedore", "IdProveedorNavigation")
                        .WithMany("Compras")
                        .HasForeignKey("IdProveedor")
                        .HasConstraintName("FK__compras__Id_pr__7D439ABD");

                    b.Navigation("IdCategoriaNavigation");

                    b.Navigation("IdProveedorNavigation");
                });

            modelBuilder.Entity("EntreEspeciesNuevo.Models.Configuracion", b =>
                {
                    b.HasOne("EntreEspeciesNuevo.Models.Permiso", "IdPermisoNavigation")
                        .WithMany("Configuracions")
                        .HasForeignKey("IdPermiso")
                        .HasConstraintName("FK__configura__Id_Pe__5BE2A6F2");

                    b.HasOne("EntreEspeciesNuevo.Models.Role", "IdRolNavigation")
                        .WithMany("Configuracions")
                        .HasForeignKey("IdRol")
                        .HasConstraintName("FK__configura__Id_Ro__5AEE82B9");

                    b.Navigation("IdPermisoNavigation");

                    b.Navigation("IdRolNavigation");
                });

            modelBuilder.Entity("EntreEspeciesNuevo.Models.DetaCompra", b =>
                {
                    b.HasOne("EntreEspeciesNuevo.Models.Compra", "IdCompraNavigation")
                        .WithMany("DetaCompras")
                        .HasForeignKey("IdCompra")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__deta_comp__Id_Co__09A971A2");

                    b.HasOne("EntreEspeciesNuevo.Models.Producto", "IdProductoNavigation")
                        .WithMany("DetaCompras")
                        .HasForeignKey("IdProducto")
                        .HasConstraintName("FK__deta_comp__Id_Pr__0A9D95DB");

                    b.Navigation("IdCompraNavigation");

                    b.Navigation("IdProductoNavigation");
                });

            modelBuilder.Entity("EntreEspeciesNuevo.Models.DetaVentum", b =>
                {
                    b.HasOne("EntreEspeciesNuevo.models.CitaInterna", "IdCitaInternaNavigation")
                        .WithMany("DetaVentas")
                        .HasForeignKey("IdCitaInterna")
                        .HasConstraintName("FK__deta_vent__Id_Ci__4BAC3F28");

                    b.HasOne("EntreEspeciesNuevo.Models.Producto", "IdProductoNavigation")
                        .WithMany("DetaVenta")
                        .HasForeignKey("IdProducto")
                        .HasConstraintName("FK__deta_vent__Id_Pr__04E4BC85");

                    b.HasOne("EntreEspeciesNuevo.Models.Venta", "IdVentaNavigation")
                        .WithMany("DetaVenta")
                        .HasForeignKey("IdVenta")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__deta_vent__Id_Ve__02FC7413");

                    b.Navigation("IdCitaInternaNavigation");

                    b.Navigation("IdProductoNavigation");

                    b.Navigation("IdVentaNavigation");
                });

            modelBuilder.Entity("EntreEspeciesNuevo.Models.Mascota", b =>
                {
                    b.HasOne("EntreEspeciesNuevo.Models.Cliente", "DocumentoClienteNavigation")
                        .WithMany("Mascota")
                        .HasForeignKey("DocumentoCliente")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__mascotas__Inf_Ma__4BAC3F29");

                    b.Navigation("DocumentoClienteNavigation");
                });

            modelBuilder.Entity("EntreEspeciesNuevo.Models.Producto", b =>
                {
                    b.HasOne("EntreEspeciesNuevo.Models.Categoria", "IdCategoriaNavigation")
                        .WithMany("Productos")
                        .HasForeignKey("IdCategoria")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__productos__Id_Ca__7E37BEF6");

                    b.Navigation("IdCategoriaNavigation");
                });

            modelBuilder.Entity("EntreEspeciesNuevo.Models.Usuario", b =>
                {
                    b.HasOne("EntreEspeciesNuevo.Models.Role", "IdRolNavigation")
                        .WithMany("Usuarios")
                        .HasForeignKey("IdRol")
                        .HasConstraintName("FK__usuario__Id_Rol__5EBF139D");

                    b.Navigation("IdRolNavigation");
                });

            modelBuilder.Entity("EntreEspeciesNuevo.Models.Venta", b =>
                {
                    b.HasOne("EntreEspeciesNuevo.Models.Cliente", "DocumentoClienteNavigation")
                        .WithMany("Venta")
                        .HasForeignKey("DocumentoCliente")
                        .HasConstraintName("FK__ventas__Document__74AE54BC");

                    b.HasOne("EntreEspeciesNuevo.Models.Categoria", "IdCategoriaNavigation")
                        .WithMany("Ventas")
                        .HasForeignKey("IdCategoria")
                        .HasConstraintName("FK__productos__Id_Ca__7E37BEF9");

                    b.Navigation("DocumentoClienteNavigation");

                    b.Navigation("IdCategoriaNavigation");
                });

            modelBuilder.Entity("EntreEspeciesNuevo.models.CitaInterna", b =>
                {
                    b.HasOne("EntreEspeciesNuevo.Models.Cliente", "DocumentoClienteNavigation")
                        .WithMany("CitaInternas")
                        .HasForeignKey("DocumentoCliente")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__cita_inte__Docum__5DCAEF64");

                    b.HasOne("EntreEspeciesNuevo.Models.Mascota", "IdMascotaNavigation")
                        .WithMany("CitaInternas")
                        .HasForeignKey("IdMascota")
                        .HasConstraintName("FK__cita_inte__Id_Ma__5CD6CB2B");

                    b.HasOne("EntreEspeciesNuevo.Models.Servicio", "IdServicioNavigation")
                        .WithMany("CitaInternas")
                        .HasForeignKey("IdServicio")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__cita_inte__Id_Ser__5CD6CB2C");

                    b.Navigation("DocumentoClienteNavigation");

                    b.Navigation("IdMascotaNavigation");

                    b.Navigation("IdServicioNavigation");
                });

            modelBuilder.Entity("EntreEspeciesNuevo.Models.Categoria", b =>
                {
                    b.Navigation("Compras");

                    b.Navigation("Productos");

                    b.Navigation("Ventas");
                });

            modelBuilder.Entity("EntreEspeciesNuevo.Models.Cliente", b =>
                {
                    b.Navigation("CitaInternas");

                    b.Navigation("Mascota");

                    b.Navigation("Venta");
                });

            modelBuilder.Entity("EntreEspeciesNuevo.Models.Compra", b =>
                {
                    b.Navigation("DetaCompras");
                });

            modelBuilder.Entity("EntreEspeciesNuevo.Models.Mascota", b =>
                {
                    b.Navigation("CitaInternas");
                });

            modelBuilder.Entity("EntreEspeciesNuevo.Models.Permiso", b =>
                {
                    b.Navigation("Configuracions");
                });

            modelBuilder.Entity("EntreEspeciesNuevo.Models.Producto", b =>
                {
                    b.Navigation("DetaCompras");

                    b.Navigation("DetaVenta");
                });

            modelBuilder.Entity("EntreEspeciesNuevo.Models.Proveedore", b =>
                {
                    b.Navigation("Compras");
                });

            modelBuilder.Entity("EntreEspeciesNuevo.Models.Role", b =>
                {
                    b.Navigation("Configuracions");

                    b.Navigation("Usuarios");
                });

            modelBuilder.Entity("EntreEspeciesNuevo.Models.Servicio", b =>
                {
                    b.Navigation("CitaInternas");
                });

            modelBuilder.Entity("EntreEspeciesNuevo.Models.Venta", b =>
                {
                    b.Navigation("DetaVenta");
                });

            modelBuilder.Entity("EntreEspeciesNuevo.models.CitaInterna", b =>
                {
                    b.Navigation("DetaVentas");
                });
#pragma warning restore 612, 618
        }
    }
}
