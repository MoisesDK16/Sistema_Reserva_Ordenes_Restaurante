using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restaurante_Moises_Loor.Models;
using Sistema_ReservaOrdenes_Restaurante.Data;
using System.Globalization;

namespace Restaurante_Moises_Loor.Controllers
{
    public class OrdenesController : Controller
    {
        private readonly RestauranteContext _context;
        private readonly ILogger<OrdenesController> _logger;

        public OrdenesController(RestauranteContext context, ILogger<OrdenesController> logger)
        {
            _context = context;
            _logger = logger;
        }
        public IActionResult Index()
        {
            var ordenes = _context.Ordenes.Include(o => o.Usuario).ToList();
            return View(ordenes);
        }

        public IActionResult Create()
        {
            ViewData["IdUsuario"] = new SelectList(
                _context.Clientes
                    .Select(u => new { u.Id, NombreCompleto = u.Nombre + " " + u.Apellido })
                    .ToList(),
                "Id", "NombreCompleto"
            );

            var menus = _context.Menus
                .Where(m => m.Estado == true)
                .Select(m => new { m.Id, Nombre = m.Nombre, Precio = m.Precio })
                .ToList();

            ViewData["Menus"] = new SelectList(menus, "Id", "Nombre");

            ViewBag.MenusDict = menus.ToDictionary(
            m => m.Id.ToString(), 
            m => new Dictionary<string, object> 
            {
            { "Nombre", m.Nombre },
            { "Precio", m.Precio }});
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(
        [Bind("UsuarioId,MetodoPago")] Orden orden,
        [Bind("MenuId,Cantidad,Subtotal")] List<Detalle> detalles)
        {
            if (detalles == null || !detalles.Any()) ModelState.AddModelError(string.Empty, "Debe agregar al menos un detalle.");

            if (ModelState.IsValid)
            {
                var ordenToSave = new Orden
                {
                    UsuarioId = orden.UsuarioId,
                    Fecha = DateTime.Now,
                    MetodoPago = orden.MetodoPago
                };

                _context.Ordenes.Add(ordenToSave);
                await _context.SaveChangesAsync();

                var detallesToSave = detalles.Select(d =>
                {
                    decimal subtotal;
                    bool conversionExitosa = decimal.TryParse(d.Subtotal.ToString(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out subtotal);

                    return new Detalle
                    {
                        OrdenId = ordenToSave.Id,
                        MenuId = d.MenuId,
                        Cantidad = d.Cantidad,
                        Subtotal = conversionExitosa ? Math.Round(subtotal / 100, 2, MidpointRounding.AwayFromZero) : 0
                    };
                }).ToList();

                _context.Detalles.AddRange(detallesToSave);
                await _context.SaveChangesAsync();
      

                ordenToSave.Total = Math.Round(detallesToSave.Sum(d => d.Subtotal), 2, MidpointRounding.AwayFromZero);
                _context.Ordenes.Update(ordenToSave);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            _logger.LogError("Error al crear la orden");

            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    _logger.LogError(error.ErrorMessage);
                }
            }

            ViewData["UsuarioId"] = new SelectList(
                await _context.Clientes.ToListAsync(), "Id", "Nombre", orden.UsuarioId
            );

            return View(orden);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var orden = await _context.Ordenes
                .Include(o => o.Usuario)
                .Include(o => o.Detalles)
                .Include("Detalles.Menu")
                .FirstOrDefaultAsync(m => m.Id == id);

            if (orden == null) return NotFound();

            Console.WriteLine($"Orden ID: {orden.Id}, Usuario: {orden.Usuario.Nombre} {orden.Usuario.Apellido}");
            foreach (var detalle in orden.Detalles)
            {
                Console.WriteLine($"Detalle - Menu: {detalle.Menu.Nombre}, Cantidad: {detalle.Cantidad}, Subtotal: {detalle.Subtotal}");
            }

            return View(orden);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var orden = await _context.Ordenes
                .Include(o => o.Usuario)  
                .Include(o => o.Detalles) 
                .ThenInclude(d => d.Menu) 
                .FirstOrDefaultAsync(m => m.Id == id);

            if (orden == null) return NotFound();

            return View(orden);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orden = await _context.Ordenes.FindAsync(id);
            if (orden != null)
            {
                _context.Ordenes.Remove(orden);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
