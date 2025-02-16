using Exercise8.Models;
using Exercise8.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

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

        [HttpDelete]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var result = await _context.Doctors.FirstOrDefaultAsync(e => e.IdDoctor == id);
            if (result == null)
            {
                return NotFound();
            }

            _context.Doctors.Remove(result);
            await _context.SaveChangesAsync();
            return Ok();

        }

        [HttpPost]
        public async Task<IActionResult> AddDoctor(DoctorAdd data) 
        {
            _context.Doctors.Add(new Doctor
            {
                FirstName = data.FirstName,
                LastName = data.LastName,
                Email = data.Email,
            });

            await _context.SaveChangesAsync();
            return Created("",data);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDoctor(DoctorUpdate data) 
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(e => e.IdDoctor == data.IdDoctor);
            if (doctor == null) 
            {
                return NotFound();
            }

            doctor.FirstName = data.FirstName;
            doctor.LastName = data.LastName;
            doctor.Email = data.Email;
            await _context.SaveChangesAsync();

            return Ok(doctor);
        }
    }
}
