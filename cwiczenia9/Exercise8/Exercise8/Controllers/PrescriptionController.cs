using Exercise8.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exercise8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly MyDbContext _context;

        public PrescriptionController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetDoctors(int PrescriptionId)
        {
            var prescription = await _context.Prescriptions.FirstOrDefaultAsync(e => e.IdPrescription == PrescriptionId);
            if (prescription != null)
            {
                var doctor = await _context.Doctors.FirstOrDefaultAsync(e => e.IdDoctor == prescription.IdDoctor);
                var patient = await _context.Patiens.FirstOrDefaultAsync(e => e.IdPatient == prescription.IdPatient);
                var PresciptionMedi = await _context.PrescriptionMedicaments.FirstOrDefaultAsync(e => e.IdPrescription == prescription.IdPrescription);
                List<Medicament> listTempMed = await _context.Medicaments.Where(e => e.IdMedicament == PresciptionMedi.IdMedicament).Select(e => new Medicament
                {
                    IdMedicament = e.IdMedicament,
                    Name = e.Name,
                    Description = e.Description,
                    Type = e.Type
                }).ToListAsync();

                var prescriptionGet = new PrescriptionGet
                {
                    Dose = (int)PresciptionMedi.Dose,
                    Details = PresciptionMedi.Details,
                    IdDoctor = doctor.IdDoctor,
                    FirstNameDoctor = doctor.FirstName,
                    LastNameDoctor = doctor.LastName,
                    EmailDoctor = doctor.Email,
                    IdPatient = patient.IdPatient,
                    FirstNamePatient = patient.FirstName,
                    LastNamePatient = patient.LastName,
                    BirthdatePatient = patient.BirthDate,
                    listMed = listTempMed,
                };

                return Ok(prescriptionGet);
            }

            return NotFound();
        }
    }
}
