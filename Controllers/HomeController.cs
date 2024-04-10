using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using EntreEspeciesNuevo.models;
using EntreEspeciesNuevo.Models;
using System.Diagnostics;

namespace TuProyecto.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly EntreespeciessqlContext _context;

        public HomeController(EntreespeciessqlContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {           
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Index(int? page)
        {
            bool VerDashboards = await VerDashboard();
            ViewBag.VerDashboards = VerDashboards;

            // Obtiene el primer y el último día del mes actual
            var primerDiaMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var ultimoDiaMes = primerDiaMes.AddMonths(1).AddDays(-1);

            // Realiza la consulta para obtener la cantidad de ventas por cada cita del mes actual
            var ventasPorCita = _context.CitaInternas
                                            .Where(c => c.Estado == "Realizada" && c.FechaHora >= primerDiaMes && c.FechaHora <= ultimoDiaMes)
                                            .GroupBy(c => c.IdServicioNavigation.NomServico)
                                            .Select(g => new { CitaInternas = g.Key, Cantidad = g.Count() })
                                            .OrderByDescending(x => x.Cantidad)
                                            .Take(10) // Puedes ajustar el número de citas que deseas mostrar
                                            .ToList();

            // Convierte los datos en un formato adecuado para el gráfico
            var labels = ventasPorCita.Select(x => x.CitaInternas).ToArray();
            var data = ventasPorCita.Select(x => x.Cantidad).ToArray();

            ViewBag.CitaInternas = labels;
            ViewBag.CantidadVentas = data;

            // Obtiene el primer y el último día del mes actual
            var primerDiaMesCliente = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var ultimoDiaMesCliente = primerDiaMesCliente.AddMonths(1).AddDays(-1);

            // Realiza la consulta para obtener los clientes que más han comprado
            var clientesMasCompradores = _context.Ventas
                .Where(v => v.FechaVenta >= primerDiaMesCliente && v.FechaVenta <= ultimoDiaMesCliente)
                .GroupBy(v => new { v.DocumentoCliente, v.DocumentoClienteNavigation.NombreCliente }) // Agrega el nombre del cliente a la agrupación
                .Select(g => new { Cliente = g.Key.NombreCliente, g.Key.DocumentoCliente, TotalComprado = g.Sum(x => x.Total) })
                .OrderByDescending(x => x.TotalComprado)
                .Take(10)
                .ToList();

            // Convierte los datos en un formato adecuado para el segundo gráfico
            var nombresClientes = clientesMasCompradores.Select(x => x.Cliente).ToArray();
            var totalCompradoPorCliente = clientesMasCompradores.Select(x => x.TotalComprado).ToArray();

            ViewBag.ClientesMasCompradores = nombresClientes;
            ViewBag.TotalCompradoPorCliente = totalCompradoPorCliente;



            // Obtiene el primer y el último día del mes actual
            var primerDiaMesProducto = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var ultimoDiaMesProducto = primerDiaMesProducto.AddMonths(1).AddDays(-1);

            // Realiza la consulta para obtener los productos más vendidos
            var productosMasComprados = _context.DetaVenta
                                                .Where(p => p.IdVentaNavigation.FechaVenta >= primerDiaMesProducto && p.IdVentaNavigation.FechaVenta <= ultimoDiaMesProducto)
                                                .GroupBy(p => p.IdProductoNavigation.NomProducto)
                                                .Select(g => new { Producto = g.Key, TotalComprado = g.Sum(x => x.SubTotalPro ?? 0), Cantidad = g.Sum(x => x.Cantidad ?? 0) })
                                                .OrderByDescending(x => x.TotalComprado)
                                                .Take(10)
                                                .ToList();

            // Convierte los datos en un formato adecuado para el gráfico
            var nombresProductos = productosMasComprados.Select(x => x.Producto).ToArray();
            var totalProductosComprados = productosMasComprados.Select(x => x.TotalComprado).ToArray();
            var cantidades = productosMasComprados.Select(x => x.Cantidad).ToArray();

            ViewBag.ProductosMasComprados = nombresProductos;
            ViewBag.TotalProductosComprados = totalProductosComprados;
            ViewBag.Cantidades = cantidades;

            var primerDiaMess = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var ultimoDiaMess = primerDiaMes.AddMonths(1).AddDays(-1);

            // Realiza la consulta para obtener el total de ventas del mes actual
            var totalVentasMes = _context.Ventas
                .Where(v => v.FechaVenta >= primerDiaMess && v.FechaVenta <= ultimoDiaMess)
                .Sum(v => v.Total);

            ViewBag.TotalVentasMes = totalVentasMes;



            return View();
        }

        public async Task<bool> VerDashboard()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 3)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

