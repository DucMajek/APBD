using Cw7.DTOs;
using Cw7.Models;
using Cw7.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cw7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly Cw7Context _context;
        private readonly ITripsService _service;
        public TripsController(Cw7Context context, ITripsService service)
        {
            _context = context;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAll());
        }

        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> AddClientTrip(int idTrip, ClientPost clientPost)
        {
            if (!await _service.Exists(clientPost.Pesel))
            {
                var clientMaxID = _context.Clients.OrderByDescending(e => e.IdClient).FirstOrDefault();
                _context.Clients.Add(new Client
                {
                    IdClient = (clientMaxID.IdClient + 1),
                    FirstName = clientPost.FirstName,
                    LastName = clientPost.LastName,
                    Email = clientPost.Email,
                    Telephone = clientPost.Telephone,
                    Pesel = clientPost.Pesel
                });
                await _context.SaveChangesAsync();
                Console.WriteLine("Dodano nowego klienta!");
            }
            else 
            {
                return BadRequest("Klient o takim pesel już istnieje ");
            }

            int clientIDTemp = _context.Clients.FirstOrDefault(e => e.Pesel == clientPost.Pesel)?.IdClient ?? 0;

            if (await _service.CheckBinded(idTrip, clientIDTemp))
            {
                Console.WriteLine("Klient jest już zapisany!");

                return BadRequest("Klient jest już zapisany na tę wycieczkę.");
            }


            if (!await _service.ExistsTrip(idTrip))
            {
              
                return BadRequest("Wycieczka nie istnieje!");
            }

            await _service.Add(clientPost, clientIDTemp, idTrip);

            return Ok();
        }

    }

  
}
