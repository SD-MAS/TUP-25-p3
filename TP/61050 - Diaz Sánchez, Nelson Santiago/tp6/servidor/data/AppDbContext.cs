using Microsoft.EntityFrameworkCore;
using servidor.Entidades;
namespace servidor.Data {

public class AppDbContext : DbContext
{
    public DbSet<Producto> Productos { get; set; }
    public DbSet<Compra> Compras { get; set; }
    public DbSet<ItemCompra> ItemsCompra { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Productos.db");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Producto>().HasData(
            new Producto { Id = 1, Nombre = "Producto 1", Descripcion = "Descripción del producto 1", Imagen = "imagen1.jpg", Precio = 10.0, Cantidad = 100 },
            new Producto { Id = 2, Nombre = "Producto 2", Descripcion = "Descripción del producto 2", Imagen = "imagen2.jpg", Precio = 20.0, Cantidad = 200 },
            new Producto { Id = 3, Nombre = "Producto 3", Descripcion = "Descripción del producto 3", Imagen = "imagen3.jpg", Precio = 30.0, Cantidad = 300 }
        );
    }
}
}