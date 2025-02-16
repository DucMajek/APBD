using Exercise8.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exercise8.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly MyDbContext _context;

        public ValuesController(MyDbContext context)
        { 
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Doctors.Select(e => new 
            { 
                IdDoctor = e.IdDoctor,
                Firstname = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,  
            
            }).ToListAsync());
        }
    }
}
