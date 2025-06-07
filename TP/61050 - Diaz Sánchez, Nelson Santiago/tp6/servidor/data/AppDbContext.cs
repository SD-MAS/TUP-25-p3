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
    new Producto { Id = 1, Nombre = "Heladera", Descripcion = "Heladera con freezer", Imagen = "heladera.jpg", Precio = 150000.0, Cantidad = 10 },
    new Producto { Id = 2, Nombre = "Lavarropas", Descripcion = "Lavarropas automático carga frontal", Imagen = "lavarropas.jpg", Precio = 120000.0, Cantidad = 8 },
    new Producto { Id = 3, Nombre = "Microondas", Descripcion = "Microondas digital 20L", Imagen = "microondas.jpg", Precio = 45000.0, Cantidad = 15 },
    new Producto { Id = 4, Nombre = "Aire Acondicionado", Descripcion = "Aire acondicionado split frío/calor", Imagen = "aire.jpg", Precio = 180000.0, Cantidad = 6 },
    new Producto { Id = 5, Nombre = "Horno eléctrico", Descripcion = "Horno eléctrico 45 litros", Imagen = "horno.jpg", Precio = 60000.0, Cantidad = 12 },
    new Producto { Id = 6, Nombre = "Batidora", Descripcion = "Batidora de mano 5 velocidades", Imagen = "batidora.jpg", Precio = 20000.0, Cantidad = 20 },
    new Producto { Id = 7, Nombre = "Licuadora", Descripcion = "Licuadora de vaso 1.5L", Imagen = "licuadora.jpg", Precio = 25000.0, Cantidad = 18 },
    new Producto { Id = 8, Nombre = "Cafetera", Descripcion = "Cafetera eléctrica 12 tazas", Imagen = "cafetera.jpg", Precio = 30000.0, Cantidad = 14 },
    new Producto { Id = 9, Nombre = "Plancha", Descripcion = "Plancha a vapor con suela antiadherente", Imagen = "plancha.jpg", Precio = 18000.0, Cantidad = 25 },
    new Producto { Id = 10, Nombre = "Tostadora", Descripcion = "Tostadora 2 rebanadas con regulador", Imagen = "tostadora.jpg", Precio = 15000.0, Cantidad = 22 }
);

    }
}
}