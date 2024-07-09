using Microsoft.AspNetCore.Mvc;
using InventoryManagement.Data;
using InventoryManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly InventoryDbContext _context;

        public LogsController(InventoryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ErrorLog>>> GetLogs()
        {
            return await _context.ErrorLogs.ToListAsync();
        }
    }
}
