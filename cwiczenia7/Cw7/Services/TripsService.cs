using Cw7.DTOs;
using Cw7.Models;
using Microsoft.EntityFrameworkCore;

namespace Cw7.Services
{

    public interface ITripsService
    {
        public Task<bool> Exists(String pesel);
        public Task<List<TripsGetAllResponse>> GetAll();
        public Task<bool> ExistsTrip(int id);
        public Task Add(ClientPost clientPost, int IdClient, int IdTrip);
        public Task<bool> CheckBinded(int id, int clientIDTemp);
    }
    public class TripsService : ITripsService
    {
        private readonly Cw7Context _context;
        public TripsService(Cw7Context context)
        {
            _context = context;
        }
        public async Task<List<TripsGetAllResponse>> GetAll()
        {
            return await _context.Trips.Select(e => new TripsGetAllResponse
            {
                Name = e.Name,
                Description = e.Description,
                DateFrom = e.DateFrom,
                DateTo = e.DateTo,
                MaxPeople = e.MaxPeople,
                Countries = e.IdCountries.Select(a => new TripsGetAllResponseCountry
                {
                    Name = a.Name
                }).ToList(),
                Clients = e.ClientTrips.Select(a => new TripsGetAllResponseClient
                {
                    FirstName = a.IdClientNavigation.FirstName,
                    LastName = a.IdClientNavigation.LastName,
                }).ToList()
            }).ToListAsync();
        }

        public async Task<bool> Exists(string pesel) 
        {
            var checkPesel = await _context.Clients.AnyAsync(e => e.Pesel == pesel);
            return checkPesel;
        }

        public async Task<bool> ExistsTrip(int id)
        {
            var trip = await _context.Trips.AnyAsync(e => e.IdTrip == id);
            return trip;
        }

        public async Task Add(ClientPost clientPost, int IdClient, int IdTrip)
        {
            _context.ClientTrips.Add(new ClientTrip
            {
                IdClient = IdClient,
                IdTrip = IdTrip,
                RegisteredAt = DateTime.Now,
                PaymentDate = clientPost.PaymentDate,
            });

            await _context.SaveChangesAsync();
        }
        public async Task<bool> CheckBinded(int tripId, int clientId)
        {
            using (var dbContext = new Cw7Context())
            {
                var trip = await dbContext.Trips.FindAsync(tripId);

                /*if (trip == null)
                {
                    throw new ArgumentException($"Wycieczka o identyfikatorze {tripId} nie istnieje.");
                }*/

                var isClientBinded = await dbContext.ClientTrips.AnyAsync(ct => ct.IdClient == clientId && ct.IdTrip == tripId);

                if (isClientBinded)
                {
                    throw new Exception("Klient jest już zapisany na tę wycieczkę.");
                }

                return false;
            }
        }



    }
}
