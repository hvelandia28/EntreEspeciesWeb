using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EntreEspeciesNuevo.models;
using EntreEspeciesNuevo.Models;
using X.PagedList;

namespace EntreEspeciesNuevo.Controllers
{
    public class CitaInternasController : Controller
    {
        private readonly EntreespeciessqlContext _context;

        public CitaInternasController(EntreespeciessqlContext context)
        {
            _context = context;
        }

        // GET: CitaInternas
        public async Task<IActionResult> Index()
        {

            bool RegistrarCitasInternas = await RegistrarCitaInterna();
            ViewBag.RegistrarCitasInternas = RegistrarCitasInternas;
            bool ActualizarCitasInternas = await ActualizarCitaInterna();
            ViewBag.ActualizarCitasInternas = ActualizarCitasInternas;
            bool VerCitas = await VerCita();
            ViewBag.VerCitas = VerCitas;
            bool CambioEstadoCitas = await CambioEstadoCita();
            ViewBag.CambioEstadoCitas = CambioEstadoCitas;

            // Obtén todas las citas internas y ordénalas
            var citasInternas = await _context.CitaInternas
                .Include(c => c.DocumentoClienteNavigation)
                .Include(c => c.IdMascotaNavigation)
                .Include(c => c.IdServicioNavigation)
                .OrderBy(c => c.Estado == "Pendiente" ? 0 : 1)
                .ToListAsync();

            // Verifica si alguna cita está incumplida y actualiza su estado si es necesario
            foreach (var cita in citasInternas)
            {
                if (cita.Estado != "Realizada")
                {
                    TimeSpan diferencia = (TimeSpan)(DateTime.Now - cita.FechaHora);
                    if (diferencia.TotalHours >= 12)
                    {
                        cita.Estado = "Incumplida";
                        _context.Update(cita);
                    }
                }
            }
            await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos

            return View(citasInternas);
        }

        // Método para cambiar el estado de la cita
        public async Task<IActionResult> CambiarEstadoCitaIncumplida(int id, string nuevoEstado)
        {
            var cita = await _context.CitaInternas.FindAsync(id);
            if (cita == null)
            {
                return NotFound();
            }

            // Cambia el estado de la cita
            cita.Estado = nuevoEstado;
            _context.Update(cita);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<bool> RegistrarCitaInterna()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 25)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }
        public async Task<bool> CambioEstadoCita()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 9)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }
        public async Task<bool> ActualizarCitaInterna()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 26)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }
        public async Task<bool> VerCita()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 27)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }




        // GET: CitaInternas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var citaInterna = await _context.CitaInternas
                .Include(c => c.IdMascotaNavigation)
                .FirstOrDefaultAsync(m => m.IdCitaInt == id);

            if (citaInterna == null)
            {
                return NotFound();
            }

            // Consulta la base de datos para obtener los datos actualizados del Cliente y la Mascota
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.DocumentoCliente == citaInterna.IdMascotaNavigation.DocumentoCliente);
            var mascota = await _context.Mascotas.FirstOrDefaultAsync(m => m.IdMascota == citaInterna.IdMascotaNavigation.IdMascota);
            var servicio = await _context.Servicios.FirstOrDefaultAsync(s => s.IdServicio == citaInterna.IdServicio);

            var viewModel = new DetalleCitaViewModel
            {
                CitaInterna = citaInterna,
                Cliente = cliente,
                Mascota = mascota,
                Servicio = servicio
            };

            return View(viewModel);
        }

        // GET: CitaInternas/Create
        public IActionResult Create()
        {
            // Cargar la lista de mascotas al ViewBag
            ViewBag.IdMascota = new SelectList(_context.Mascotas, "IdMascota", "IdMascota");
            ViewBag.IdServicio = new SelectList(_context.Servicios, "IdServicio", "NomServico");

            // Inicializar el ViewBag.DocumentoCliente con una lista vacía (se actualizará dinámicamente)
            ViewBag.DocumentoCliente = new SelectList(new List<SelectListItem>(), "Value", "Text");

            return View();
        }

        [HttpPost]
        public IActionResult VerificarDisponibilidadaaassds(DateTime fechaHora)
        {
            // Calcular el límite inferior del rango de 2 horas antes de la hora seleccionada
            var limiteInferior = fechaHora.AddMinutes(-119);

            // Calcular el límite superior del rango de 2 horas después de la hora seleccionada
            var limiteSuperior = fechaHora.AddMinutes(119);

            // Verificar la disponibilidad de citas en el rango especificado
            bool citaDisponible = !_context.CitaInternas.Any(c => c.FechaHora >= limiteInferior && c.FechaHora < limiteSuperior);

            return Json(new { citaDisponible = citaDisponible });
        }

        [HttpPost]
        public IActionResult VerificarDisponibilidad(DateTime fechaHora, int? idCitaEditar = null)
        {
            // Calcular el límite inferior del rango de 2 horas antes de la hora seleccionada
            var limiteInferior = fechaHora.AddMinutes(-119);

            // Calcular el límite superior del rango de 2 horas después de la hora seleccionada
            var limiteSuperior = fechaHora.AddMinutes(119);

            // Verificar la disponibilidad de citas en el rango especificado, excluyendo la cita actual si se está editando
            bool citaDisponible = !_context.CitaInternas
                .Where(c => c.IdCitaInt != idCitaEditar) // Excluir la cita actual si se está editando
                .Any(c => c.FechaHora >= limiteInferior && c.FechaHora < limiteSuperior);

            return Json(new { citaDisponible = citaDisponible });
        }

        public async Task<IActionResult> CambiarEstadoCita(int id, string nuevoEstado)
        {
            var cita = await _context.CitaInternas.FindAsync(id);
            if (cita == null)
            {
                return NotFound();
            }

            // Aquí es donde accedes a los datos de la cita
            var documentoCliente = cita.DocumentoCliente;
            var precio = cita.Precio;
            Venta venta = null;
            // Cambiar el estado de la cita
            cita.Estado = nuevoEstado;
            _context.Update(cita);
            await _context.SaveChangesAsync();

            // Si el nuevo estado es "Realizada", crear una nueva venta y su detalle
            if (nuevoEstado == "Realizada")
            {
                // Obtener el servicio asociado a la cita
                var servicio = await _context.Servicios.FindAsync(cita.IdServicio);
                if (servicio == null)
                {
                    return NotFound("Servicio no encontrado");
                }

                // Capturar el nombre del servicio
                var nombreServicio = servicio.NomServico; // Aquí obtienes el nombre del servicio

                // Crear una nueva venta con los datos de la cita
                venta = new Venta
                {
                    DocumentoCliente = documentoCliente, // Usar el documento del cliente de la cita
                    FechaVenta = DateTime.Now, // Asignar la fecha actual
                    FormaPago = "Efectivo", // Asignar un valor por defecto o obtenerlo de otra fuente
                    Total = precio, // Usar el precio de la cita
                };

                // Crear el detalle de la venta
                var detalleVenta = new DetaVentum
                {
                    IdVenta = venta.IdVenta,
                    IdCitaInterna = cita.IdCitaInt,
                    SubTotalSer = cita.Precio,
                    // ServicioNombre = nombreServicio // Aquí asignas el nombre del servicio al detalle de la venta
                };
                // Agregar el detalle de la venta a la colección de DetaVenta de la venta
                venta.DetaVenta.Add(detalleVenta);

                // Agregar la venta al contexto y guardar los cambios
                _context.Ventas.Add(venta);
                await _context.SaveChangesAsync();
            }

            return Json(new { success = true, message = "Estado actualizado exitosamente", nuevaVentaId = venta?.IdVenta });
        }



        // Inside your controller
        [HttpGet]
        public IActionResult VerifyClientDocument(int documentoCliente)
        {
            bool exists = _context.Clientes.Any(c => c.DocumentoCliente == documentoCliente);
            return Json(new { exists });
        }


        // Acción Ajax para obtener documentos por IdMascota
        [HttpGet]
        public IActionResult ObtenerMascotasPorDocumento(int documentoCliente)
        {
            try
            {
                var mascotas = _context.Mascotas
                    .Where(m => m.DocumentoCliente == documentoCliente)
                    .Select(m => new SelectListItem { Value = m.IdMascota.ToString(), Text = m.NombreMascota })
                    .ToList();

                return Json(mascotas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }
        [HttpGet]
        public IActionResult GetPrecioProducto(int servicioId)
        {
            try
            {
                var servicio = _context.CitaInternas.FirstOrDefault(s => s.IdCitaInt == servicioId);

                if (servicio != null)
                {
                    return Json(servicio.Precio);
                }

                return NotFound(); // O devuelve un valor predeterminado, según tus requisitos.
            }
            catch (Exception ex)
            {
                // Manejar la excepción según tus necesidades (por ejemplo, registrarla)
                return StatusCode(500, "Error interno del servidor");
            }
        }

        private List<SelectListItem> ObtenerMascotasPorDocumentoCliente(int documentoCliente)
        {
            // Lógica para obtener mascotas por DocumentoCliente
            var mascotas = _context.Clientes
                .Where(c => c.DocumentoCliente == documentoCliente)
                .SelectMany(c => c.Mascota)
                .Select(m => new SelectListItem
                {
                    Value = m.IdMascota.ToString(),
                    Text = m.NombreMascota
                })
                .ToList();

            return mascotas;


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CitaInterna citaInterna)
        {
            if (ModelState.IsValid)
            {
                // Obtener el nombre del servicio seleccionado
                var servicioSeleccionado = await _context.Servicios.FindAsync(citaInterna.IdServicio);
                if (servicioSeleccionado != null)
                {
                    // Asignar el nombre del servicio como nombre de la cita
                    citaInterna.NomCita = servicioSeleccionado.NomServico; // Asumiendo que el nombre del servicio está en una propiedad llamada Nombre en tu modelo Servicio
                }

                _context.Add(citaInterna);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(citaInterna);
        }



        // GET: CitaInternas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CitaInternas == null)
            {
                return NotFound();
            }

            var citaInterna = await _context.CitaInternas.FindAsync(id);
            if (citaInterna == null)
            {
                return NotFound();
            }

            ViewData["DocumentoCliente"] = new SelectList(_context.Clientes, "DocumentoCliente", "DocumentoCliente", citaInterna.DocumentoCliente);

            // Aquí obtén la información de la mascota, por ejemplo, su nombre
            var mascota = await _context.Mascotas.FindAsync(citaInterna.IdMascota);


            // Envía el nombre de la mascota a la vista usando ViewBag

            // También puedes enviar el ID si es necesario

            ViewBag.IdMascota = mascota.NombreMascota;
            ViewBag.IdServicio = new SelectList(_context.Servicios, "IdServicio", "NomServico");
            return View(citaInterna);
        }


        // POST: CitaInternas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCitaInt,IdMascota,DocumentoCliente,IdServicio,FechaHora,Estado,Precio,PersonaCargo")] CitaInterna citaInterna)
        {
            if (id != citaInterna.IdCitaInt)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(citaInterna);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CitaInternaExists(citaInterna.IdCitaInt))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DocumentoCliente"] = new SelectList(_context.Clientes, "DocumentoCliente", "DocumentoCliente", citaInterna.DocumentoCliente);
            ViewData["IdMascota"] = new SelectList(_context.Mascotas, "IdMascota", "NombreMascota", citaInterna.IdMascota);
            ViewData["IdServicio"] = new SelectList(_context.Servicios, "IdServicio", "IdServicio", citaInterna.IdServicio);

            return View(citaInterna);
        }

        // GET: CitaInternas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CitaInternas == null)
            {
                return NotFound();
            }

            var citaInterna = await _context.CitaInternas
                .Include(c => c.DocumentoClienteNavigation)
                .Include(c => c.IdMascotaNavigation)
                .FirstOrDefaultAsync(m => m.IdCitaInt == id);
            if (citaInterna == null)
            {
                return NotFound();
            }

            return View(citaInterna);
        }

        // POST: CitaInternas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CitaInternas == null)
            {
                return Problem("Entity set 'EntreespeciessqlpContext.CitaInternas'  is null.");
            }
            var citaInterna = await _context.CitaInternas.FindAsync(id);
            if (citaInterna != null)
            {
                _context.CitaInternas.Remove(citaInterna);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CitaInternaExists(int id)
        {
            return (_context.CitaInternas?.Any(e => e.IdCitaInt == id)).GetValueOrDefault();
        }
    }
}