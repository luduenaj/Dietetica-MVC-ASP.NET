using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dietetica.Data;
using Dietetica.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Dietetica.Models;
using Dietetica.ModelsView;
using Microsoft.AspNetCore.Authorization;

namespace Dietetica.Controllers
{
    public class ProductosEmbasadosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment env;

        public ProductosEmbasadosController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        // GET: ProductosEmbasados
        public async Task<IActionResult> Index(string busqNombre, int? proveedorId,int? tipoVentaId, int pagina = 1)
        {
            paginador paginador = new paginador()
            {
                pagActual = pagina,
                regXpag = 5
            };

            var consulta = _context.productosEmbasados.Include(a => a.proveedor).Include(a => a.tipoVenta).Select(a => a);
            if (!string.IsNullOrEmpty(busqNombre))
            {
                consulta = consulta.Where(e => e.nombre.Contains(busqNombre));
            }

            if (proveedorId.HasValue)
            {
                consulta = consulta.Where(e => e.IdProveedor == proveedorId);
            }
            
            if (tipoVentaId.HasValue)
            {
                consulta = consulta.Where(e => e.IdTipoVenta == tipoVentaId);
            }

            paginador.cantReg = consulta.Count();

            var datosAmostrar = consulta
                .Skip((paginador.pagActual - 1) * paginador.regXpag)
                .Take(paginador.regXpag);
            foreach (var dato in datosAmostrar)
            {
                dato.proveedor = _context.proveedores.Where(x => x.Id == dato.IdProveedor).FirstOrDefault();
                dato.tipoVenta = _context.tiposVentas.Where(x => x.Id == dato.IdTipoVenta).FirstOrDefault();
            }
            foreach (var item in Request.Query)
                paginador.ValoresQueryString.Add(item.Key, item.Value);

            ProductoEmbasadoViewModel Datos = new ProductoEmbasadoViewModel()
            {
                ListaProductosEmbasados = datosAmostrar.ToList(),
                ListaProveedores = new SelectList(_context.proveedores, "Id", "nombre", proveedorId),
                ListaTiposVentas = new SelectList(_context.tiposVentas, "Id", "tipoDeVenta", tipoVentaId),
                busqNombre = busqNombre,
                paginador = paginador
            };
            ViewData["proveedoresList"] = new SelectList(_context.proveedores, "id", "nombre");
            ViewData["tipoVentasList"] = new SelectList(_context.tiposVentas, "id", "tipoDeVenta");
            return View(Datos);
        }

        // GET: ProductosEmbasados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productoEmbasado = await _context.productosEmbasados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productoEmbasado == null)
            {
                return NotFound();
            }

            productoEmbasado.proveedor = _context.proveedores.Where(x => x.Id == productoEmbasado.IdProveedor).FirstOrDefault();
            productoEmbasado.tipoVenta = _context.tiposVentas.Where(x => x.Id == productoEmbasado.IdTipoVenta).FirstOrDefault();
            return View(productoEmbasado);
        }

        // GET: ProductosEmbasados/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["ProveedorList"] = new SelectList(_context.proveedores, "Id", "nombre");
            ViewData["TiposList"] = new SelectList(_context.tiposVentas, "Id", "tipoDeVenta");
            return View();
        }

        // POST: ProductosEmbasados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,nombre,precioPorUnidad,gramos,foto,IdTipoVenta,IdProveedor")] ProductoEmbasado productoEmbasado)
        {
            if (ModelState.IsValid)
            {
                var archivos = HttpContext.Request.Form.Files;
                if (archivos != null && archivos.Count > 0)
                {
                    var archivoFoto = archivos[0];
                    var pathDestino = Path.Combine(env.WebRootPath, "fotos");
                    if (archivoFoto.Length > 0)
                    {
                        var archivoDestino = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(archivoFoto.FileName);

                        using (var filestream = new FileStream(Path.Combine(pathDestino, archivoDestino), FileMode.Create))
                        {
                            archivoFoto.CopyTo(filestream);
                            productoEmbasado.foto = archivoDestino;
                        };

                    }
                }
                _context.Add(productoEmbasado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productoEmbasado);
        }

        // GET: ProductosEmbasados/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productoEmbasado = await _context.productosEmbasados.FindAsync(id);
            if (productoEmbasado == null)
            {
                return NotFound();
            }

            ViewData["idProveedor"] = new SelectList(_context.proveedores, "Id", "nombre", productoEmbasado.IdProveedor);
            ViewData["idTipoVenta"] = new SelectList(_context.tiposVentas, "Id", "tipoDeVenta", productoEmbasado.IdTipoVenta);
            return View(productoEmbasado);
        }

        // POST: ProductosEmbasados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,nombre,precioPorUnidad,gramos,foto,IdTipoVenta,IdProveedor")] ProductoEmbasado productoEmbasado)
        {
            if (id != productoEmbasado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productoEmbasado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoEmbasadoExists(productoEmbasado.Id))
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
            return View(productoEmbasado);
        }

        // GET: ProductosEmbasados/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productoEmbasado = await _context.productosEmbasados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productoEmbasado == null)
            {
                return NotFound();
            }
            productoEmbasado.proveedor = _context.proveedores.Where(x => x.Id == productoEmbasado.IdProveedor).FirstOrDefault();
            productoEmbasado.tipoVenta = _context.tiposVentas.Where(x => x.Id == productoEmbasado.IdTipoVenta).FirstOrDefault();
            return View(productoEmbasado);
        }

        // POST: ProductosEmbasados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productoEmbasado = await _context.productosEmbasados.FindAsync(id);
            _context.productosEmbasados.Remove(productoEmbasado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoEmbasadoExists(int id)
        {
            return _context.productosEmbasados.Any(e => e.Id == id);
        }
    }
}
