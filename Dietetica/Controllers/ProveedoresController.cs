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
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Dietetica.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment env;

        public ProveedoresController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        public IActionResult Importar()
        {
            var archivos = HttpContext.Request.Form.Files;
            if (archivos != null && archivos.Count > 0)
            {
                var archivoImpo = archivos[0];
                var pathDestino = Path.Combine(env.WebRootPath, "importaciones");
                if (archivoImpo.Length > 0)
                {
                    var archivoDestino = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(archivoImpo.FileName);
                    string rutaCompleta = Path.Combine(pathDestino, archivoDestino);
                    using (var filestream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        archivoImpo.CopyTo(filestream);
                    };

                    using (var file = new FileStream(rutaCompleta, FileMode.Open))
                    {
                        List<string> renglones = new List<string>();
                        List<Proveedor> ProveedoresArch = new List<Proveedor>();

                        StreamReader fileContent = new StreamReader(file);
                        do
                        {
                            renglones.Add(fileContent.ReadLine());
                        }
                        while (!fileContent.EndOfStream);

                        foreach (string renglon in renglones)
                        {
                            string[] datos = renglon.Split(';');
                            Proveedor proveedortemporal = new Proveedor()
                            {
                                nombre = datos[0],
                                telefono = datos[1],
                                email = datos[2],
                            };
                            ProveedoresArch.Add(proveedortemporal);
                            
                        }
                        if (ProveedoresArch.Count > 0)
                        {
                            _context.proveedores.AddRange(ProveedoresArch);
                            _context.SaveChanges();
                        }

                        ViewBag.cantReng = ProveedoresArch.Count + " de " + renglones.Count;
                    }
                }
            }

            return View();
        }

        // GET: Proveedores
        public async Task<IActionResult> Index(string busqNombre, int pagina = 1)
        {
            paginador paginador = new paginador()
            {
                pagActual = pagina,
                regXpag = 5
            };
            var consulta = _context.proveedores.Select(a => a);
            if (!string.IsNullOrEmpty(busqNombre))
            {
                consulta = consulta.Where(e => e.nombre.Contains(busqNombre));
                //paginador.ValoresQueryString.Add("busqNombre", busqNombre);
            }
            

            paginador.cantReg = consulta.Count();

            //paginador.totalPag = (int)Math.Ceiling((decimal)paginador.cantReg / paginador.regXpag);
            var datosAmostrar = consulta
                .Skip((paginador.pagActual - 1) * paginador.regXpag)
                .Take(paginador.regXpag);

            foreach (var item in Request.Query)
                paginador.ValoresQueryString.Add(item.Key, item.Value);

            ProveedorViewModel Datos = new ProveedorViewModel()
            {
                ListaProveedores = datosAmostrar.ToList(),
                busqNombre = busqNombre,
                paginador = paginador
            };
            return View(Datos);
        }

        // GET: Proveedores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedor = await _context.proveedores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proveedor == null)
            {
                return NotFound();
            }

            return View(proveedor);
        }

        // GET: Proveedores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Proveedores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,nombre,telefono,email")] Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proveedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proveedor);
        }

        // GET: Proveedores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedor = await _context.proveedores.FindAsync(id);
            if (proveedor == null)
            {
                return NotFound();
            }
            return View(proveedor);
        }

        // POST: Proveedores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,nombre,telefono,email")] Proveedor proveedor)
        {
            if (id != proveedor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proveedor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProveedorExists(proveedor.Id))
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
            return View(proveedor);
        }

        // GET: Proveedores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedor = await _context.proveedores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proveedor == null)
            {
                return NotFound();
            }

            return View(proveedor);
        }

        // POST: Proveedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proveedor = await _context.proveedores.FindAsync(id);
            _context.proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProveedorExists(int id)
        {
            return _context.proveedores.Any(e => e.Id == id);
        }
    }
}
