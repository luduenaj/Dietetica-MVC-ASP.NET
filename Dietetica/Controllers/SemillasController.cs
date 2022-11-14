using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dietetica.Data;
using Dietetica.Models;
using Dietetica.ModelsView;

namespace Dietetica.Controllers
{
    public class SemillasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SemillasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Semillas
        public async Task<IActionResult> Index(string busqNombre, int? proveedorId, int pagina = 1)
        {
            paginador paginador = new paginador()
            {
                pagActual = pagina,
                regXpag = 5
            };

            var consulta = _context.semillas.Include(a => a.proveedor).Include(a => a.tipoVenta).Select(a => a);
            if (!string.IsNullOrEmpty(busqNombre))
            {
                consulta = consulta.Where(e => e.nombre.Contains(busqNombre));
            }

            if (proveedorId.HasValue)
            {
                consulta = consulta.Where(e => e.idProveedor == proveedorId);
            }

            paginador.cantReg = consulta.Count();

            var datosAmostrar = consulta
                .Skip((paginador.pagActual - 1) * paginador.regXpag)
                .Take(paginador.regXpag);

            foreach (var item in Request.Query)
                paginador.ValoresQueryString.Add(item.Key, item.Value);

            SemillaViewModel Datos = new SemillaViewModel()
            {
                ListaSemillas = datosAmostrar.ToList(),
                ListaProveedores = new SelectList(_context.proveedores, "Id", "nombre", proveedorId),
                ListaTiposVentas = new SelectList(_context.tiposVentas, "Id", "tipoDeVenta"),
                busqNombre = busqNombre,
                paginador = paginador
            };

            return View(Datos);
        }

        // GET: Semillas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var semilla = await _context.semillas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (semilla == null)
            {
                return NotFound();
            }

            return View(semilla);
        }

        // GET: Semillas/Create
        public IActionResult Create()
        {
            ViewData["ProveedorList"] = new SelectList(_context.proveedores, "Id", "nombre");
            ViewData["TiposList"] = new SelectList(_context.tiposVentas, "Id", "tipoDeVenta");
            return View();
        }

        // POST: Semillas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,nombre,precioXKg,idTipoVenta,idProveedor")] Semilla semilla)
        {
            if (ModelState.IsValid)
            {
                _context.Add(semilla);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(semilla);
        }

        // GET: Semillas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var semilla = await _context.semillas.FindAsync(id);
            if (semilla == null)
            {
                return NotFound();
            }

            semilla.proveedor = _context.proveedores.Where(x => x.Id == semilla.idProveedor).FirstOrDefault();
            semilla.tipoVenta = _context.tiposVentas.Where(x => x.Id == semilla.idTipoVenta).FirstOrDefault();
            return View(semilla);
        }

        // POST: Semillas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,nombre,precioXKg,idTipoVenta,idProveedor")] Semilla semilla)
        {
            if (id != semilla.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(semilla);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SemillaExists(semilla.Id))
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
            return View(semilla);
        }

        // GET: Semillas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var semilla = await _context.semillas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (semilla == null)
            {
                return NotFound();
            }

            return View(semilla);
        }

        // POST: Semillas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var semilla = await _context.semillas.FindAsync(id);
            _context.semillas.Remove(semilla);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SemillaExists(int id)
        {
            return _context.semillas.Any(e => e.Id == id);
        }
    }
}
