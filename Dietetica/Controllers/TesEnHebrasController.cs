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
    public class TesEnHebrasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TesEnHebrasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TesEnHebras
        public async Task<IActionResult> Index(string busqNombre, int? proveedorId, int pagina = 1)
        {
            paginador paginador = new paginador()
            {
                pagActual = pagina,
                regXpag = 5
            };

            var consulta = _context.tesEnHebras.Include(a => a.proveedor).Include(a => a.tipoVenta).Select(a => a);
            if (!string.IsNullOrEmpty(busqNombre))
            {
                consulta = consulta.Where(e => e.nombre.Contains(busqNombre));
            }

            if (proveedorId.HasValue)
            {
                consulta = consulta.Where(e => e.IdProveedor == proveedorId);
            }

            paginador.cantReg = consulta.Count();

            var datosAmostrar = consulta
                .Skip((paginador.pagActual - 1) * paginador.regXpag)
                .Take(paginador.regXpag);
            foreach(var dato in datosAmostrar)
            {
                dato.proveedor = _context.proveedores.Where(x => x.Id == dato.IdProveedor).FirstOrDefault();
                dato.tipoVenta = _context.tiposVentas.Where(x => x.Id == dato.IdTipoVenta).FirstOrDefault();
            }
            foreach (var item in Request.Query)
            {
                paginador.ValoresQueryString.Add(item.Key, item.Value);
            }
            TeEnHebrasViewModel Datos = new TeEnHebrasViewModel()
            {
                ListaTesEnHebras = datosAmostrar.ToList(),
                ListaProveedores = new SelectList(_context.proveedores, "Id", "nombre", proveedorId),
                ListaTiposVentas = new SelectList(_context.tiposVentas, "Id", "tipoDeVenta"),
                busqNombre = busqNombre,
                paginador = paginador
            };

            return View(Datos);
        }

        // GET: TesEnHebras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teEnHebras = await _context.tesEnHebras
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teEnHebras == null)
            {
                return NotFound();
            }

            teEnHebras.proveedor = _context.proveedores.Where(x => x.Id == teEnHebras.IdProveedor).FirstOrDefault();
            teEnHebras.tipoVenta = _context.tiposVentas.Where(x => x.Id == teEnHebras.IdTipoVenta).FirstOrDefault();
            return View(teEnHebras);
        }

        // GET: TesEnHebras/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["ProveedorList"] = new SelectList(_context.proveedores, "Id", "nombre");
            ViewData["TiposList"] = new SelectList(_context.tiposVentas, "Id", "tipoDeVenta");
            return View();
        }

        // POST: TesEnHebras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,nombre,precioX100gr,IdTipoVenta,IdProveedor")] TeEnHebras teEnHebras)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teEnHebras);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teEnHebras);
        }

        // GET: TesEnHebras/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teEnHebras = await _context.tesEnHebras.FindAsync(id);
            if (teEnHebras == null)
            {
                return NotFound();
            }

            ViewData["idProveedor"] = new SelectList(_context.proveedores, "Id", "nombre", teEnHebras.IdProveedor);
            ViewData["idTipoVenta"] = new SelectList(_context.tiposVentas, "Id", "tipoDeVenta", teEnHebras.IdTipoVenta);
            return View(teEnHebras);
        }

        // POST: TesEnHebras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,nombre,precioX100gr,IdTipoVenta,IdProveedor")] TeEnHebras teEnHebras)
        {
            if (id != teEnHebras.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teEnHebras);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeEnHebrasExists(teEnHebras.Id))
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
            return View(teEnHebras);
        }

        // GET: TesEnHebras/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teEnHebras = await _context.tesEnHebras
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teEnHebras == null)
            {
                return NotFound();
            }

            teEnHebras.proveedor = _context.proveedores.Where(x => x.Id == teEnHebras.IdProveedor).FirstOrDefault();
            teEnHebras.tipoVenta = _context.tiposVentas.Where(x => x.Id == teEnHebras.IdTipoVenta).FirstOrDefault();
            return View(teEnHebras);
        }

        // POST: TesEnHebras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teEnHebras = await _context.tesEnHebras.FindAsync(id);
            _context.tesEnHebras.Remove(teEnHebras);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeEnHebrasExists(int id)
        {
            return _context.tesEnHebras.Any(e => e.Id == id);
        }
    }
}
