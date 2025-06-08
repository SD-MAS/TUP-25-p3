[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly appdbContext _context;

    public ControladorDeProductos(AppdbContext context)
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