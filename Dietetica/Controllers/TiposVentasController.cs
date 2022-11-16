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
using Microsoft.AspNetCore.Authorization;

namespace Dietetica.Controllers
{
    public class TiposVentasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TiposVentasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TiposVentas
        public async Task<IActionResult> Index(int pagina = 1)
        {
            paginador paginador = new paginador()
            {
                pagActual = pagina,
                regXpag = 5
            };
            var consulta = _context.tiposVentas.Select(a => a);

            paginador.cantReg = consulta.Count();

            var datosAmostrar = consulta
                .Skip((paginador.pagActual - 1) * paginador.regXpag)
                .Take(paginador.regXpag);

            foreach (var item in Request.Query)
                paginador.ValoresQueryString.Add(item.Key, item.Value);

            TipoVentaViewModel Datos = new TipoVentaViewModel()
            {
                ListaTiposVentas = datosAmostrar.ToList(),
                paginador = paginador
            };
            return View(Datos);
        }

        // GET: TiposVentas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoVenta = await _context.tiposVentas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoVenta == null)
            {
                return NotFound();
            }

            return View(tipoVenta);
        }

        // GET: TiposVentas/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: TiposVentas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,tipoDeVenta")] TipoVenta tipoVenta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoVenta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoVenta);
        }

        // GET: TiposVentas/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoVenta = await _context.tiposVentas.FindAsync(id);
            if (tipoVenta == null)
            {
                return NotFound();
            }
            return View(tipoVenta);
        }

        // POST: TiposVentas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,tipoDeVenta")] TipoVenta tipoVenta)
        {
            if (id != tipoVenta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoVenta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoVentaExists(tipoVenta.Id))
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
            return View(tipoVenta);
        }

        // GET: TiposVentas/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoVenta = await _context.tiposVentas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoVenta == null)
            {
                return NotFound();
            }

            return View(tipoVenta);
        }

        // POST: TiposVentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoVenta = await _context.tiposVentas.FindAsync(id);
            _context.tiposVentas.Remove(tipoVenta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoVentaExists(int id)
        {
            return _context.tiposVentas.Any(e => e.Id == id);
        }
    }
}
