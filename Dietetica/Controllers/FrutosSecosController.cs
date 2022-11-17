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
    public class FrutosSecosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FrutosSecosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FrutosSecos
        [AllowAnonymous]
        public async Task<IActionResult> Index(string busqNombre, int? proveedorId, int pagina = 1)
        {
            paginador paginador = new paginador()
            {
                pagActual = pagina,
                regXpag = 5
            };

            var consulta = _context.frutosSecos.Include(a => a.proveedor).Include(a => a.tipoVenta).Select(a => a);
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

            foreach(var dato in datosAmostrar)
            {
                dato.proveedor = _context.proveedores.Where(x => x.Id == dato.idProveedor).FirstOrDefault();
                dato.tipoVenta = _context.tiposVentas.Where(x => x.Id == dato.idTipoVenta).FirstOrDefault();
            }

            foreach (var item in Request.Query)
                paginador.ValoresQueryString.Add(item.Key, item.Value);

            FrutoSecoViewModel Datos = new FrutoSecoViewModel()
            {
                ListaFrutosSecos = datosAmostrar.ToList(),
                ListaProveedores = new SelectList(_context.proveedores, "Id", "nombre", proveedorId),
                ListaTiposVentas = new SelectList(_context.tiposVentas, "Id", "tipoDeVenta"),
                busqNombre = busqNombre,
                paginador = paginador
            };
            
            return View(Datos);
        }


        // GET: FrutosSecos/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var datos = _context.frutosSecos.Include(x => x.proveedor).Include(x => x.tipoVenta).ToList();
            var frutoSeco = await _context.frutosSecos.Include(x => x.proveedor).Include(x => x.tipoVenta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (frutoSeco == null)
            {
                return NotFound();
            }
            frutoSeco.proveedor = _context.proveedores.Where(x => x.Id == frutoSeco.idProveedor).FirstOrDefault();
            frutoSeco.tipoVenta = _context.tiposVentas.Where(x => x.Id == frutoSeco.idTipoVenta).FirstOrDefault();
            return View(frutoSeco);
        }

        // GET: FrutosSecos/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["ProveedorList"] = new SelectList(_context.proveedores, "Id", "nombre");
            ViewData["TiposList"] = new SelectList(_context.tiposVentas, "Id", "tipoDeVenta");
            return View();
        }

        // POST: FrutosSecos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,nombre,precioXKg,idTipoVenta,idProveedor")] FrutoSeco frutoSeco)
        {
            if (ModelState.IsValid)
            {
                _context.Add(frutoSeco);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(frutoSeco);
        }

        // GET: FrutosSecos/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var frutoSeco = await _context.frutosSecos.FindAsync(id);
            if (frutoSeco == null)
            {
                return NotFound();
            }

            ViewData["idProveedor"] = new SelectList(_context.proveedores, "Id", "nombre", frutoSeco.idProveedor);
            ViewData["idTipoVenta"] = new SelectList(_context.tiposVentas, "Id", "tipoDeVenta", frutoSeco.idTipoVenta);
            return View(frutoSeco);
        }

        // POST: FrutosSecos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,nombre,precioXKg,idTipoVenta,idProveedor")] FrutoSeco frutoSeco)
        {
            if (id != frutoSeco.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(frutoSeco);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FrutoSecoExists(frutoSeco.Id))
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
            return View(frutoSeco);
        }

        // GET: FrutosSecos/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var frutoSeco = await _context.frutosSecos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (frutoSeco == null)
            {
                return NotFound();
            }

            frutoSeco.proveedor = _context.proveedores.Where(x => x.Id == frutoSeco.idProveedor).FirstOrDefault();
            frutoSeco.tipoVenta = _context.tiposVentas.Where(x => x.Id == frutoSeco.idTipoVenta).FirstOrDefault();
            return View(frutoSeco);
        }

        // POST: FrutosSecos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var frutoSeco = await _context.frutosSecos.FindAsync(id);
            _context.frutosSecos.Remove(frutoSeco);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FrutoSecoExists(int id)
        {
            return _context.frutosSecos.Any(e => e.Id == id);
        }
    }
}
