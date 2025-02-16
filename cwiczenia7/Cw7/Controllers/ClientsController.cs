using Cw7.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cw7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly Cw7Context _context;
        public ClientsController(Cw7Context context)
        {
            _context = context;
        }


        [HttpDelete("{idClient}")]
        public async Task<IActionResult> Remove(int idClient)
        {
            var client = await _context.Clients.Include(c => c.ClientTrips).FirstOrDefaultAsync(c => c.IdClient == idClient);
            if (client == null) return NotFound();

            if (client.ClientTrips.Any())
            {
                return BadRequest("Klient ma przypisane wycieczki. Nie można usunąć klienta.");
            }
            _context.Clients.Remove(client);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
