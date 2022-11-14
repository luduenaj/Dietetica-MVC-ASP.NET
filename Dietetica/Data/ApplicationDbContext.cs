using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Dietetica.Models;

namespace Dietetica.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Semilla> semillas { get; set; }
        public DbSet<FrutoSeco> frutosSecos { get; set; }
        public DbSet<ProductoEmbasado> productosEmbasados { get; set; }
        public DbSet<TeEnHebras> tesEnHebras { set; get; }

        public DbSet<TipoVenta> tiposVentas { set; get; }
        public DbSet<Proveedor> proveedores { set; get; }
    }
}
