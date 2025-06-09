using Microsoft.AspNetCore.Mvc;
using servidor.Data; 
using System.Linq;
[ApiController]
[Route("api/[controller]")]
public class ProductosController  : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetProductos()
    {
        var productos = _context.Productos.ToList();
        return Ok(productos);
    }
}