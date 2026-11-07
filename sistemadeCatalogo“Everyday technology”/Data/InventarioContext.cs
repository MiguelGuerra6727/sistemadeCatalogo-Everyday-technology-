using sistemadeCatalogo_Everyday_technology_.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace sistemadeCatalogo_Everyday_technology_.Data
{
    public class InventarioContext : DbContext
    {
        public InventarioContext() : base("name= InventarioConexion") 
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Seccion> Seccions  { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<pedido> pedidos { get; set; }
        public DbSet<PedidoItem> PedidoItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Rol>().ToTable("Roles");
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<Seccion>().ToTable("Secciones");
            modelBuilder.Entity<Producto>().ToTable("Productos");
            modelBuilder.Entity<pedido>().ToTable("Pedidos");
            modelBuilder.Entity<PedidoItem>().ToTable("PedidoItems");
            //Para usuario
            modelBuilder.Entity<Usuario>()
                .HasRequired(u => u.Rol).WithMany(r => r.Usuarios)
                .HasForeignKey(p => p.RolId).WillCascadeOnDelete(false);
            //Producto
            modelBuilder.Entity<Producto>()
                .HasRequired(p => p.Seccion).WithMany(s => s.Productos)
                .HasForeignKey(u => u.SeccionId).WillCascadeOnDelete(false);
            //Pedido items
            modelBuilder.Entity<PedidoItem>()
                .HasRequired(i => i.Producto).WithMany()
                .HasForeignKey(i=> i.ProductoId ).WillCascadeOnDelete(false);
            //Si se borra un pedido se borrara sus items devolviendo a stocks
            modelBuilder.Entity<PedidoItem>()
                .HasRequired(i => i.Pedido).WithMany(p=> p.Items)
                .HasForeignKey(i => i.PedidoId).WillCascadeOnDelete (true);
        }
    } //fin del public class
}