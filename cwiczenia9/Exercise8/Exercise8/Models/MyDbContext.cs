using Microsoft.EntityFrameworkCore;
namespace Exercise8.Models
{
    public class MyDbContext : DbContext
    {

        public MyDbContext(DbContextOptions options): base(options) 
        { 
            
        }

        

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patiens { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>(e =>
            {
                e.HasKey(e => e.IdDoctor);
                e.Property(e => e.FirstName).HasMaxLength(120).IsRequired();
                e.Property(e => e.LastName).HasMaxLength(120).IsRequired();
                e.Property(e => e.Email).HasMaxLength(120).IsRequired();

                e.HasData(new List<Doctor> 
                { 
                    new Doctor
                    { 
                        IdDoctor = 1,
                        FirstName = "Kamil",
                        LastName = "Koko",
                        Email = "koko123@gmail.pl"
                    },

                    new Doctor
                    {
                        IdDoctor = 2,
                        FirstName = "Maciek",
                        LastName = "Kali",
                        Email = "Kali13@onet.pl"
                    }


                });
            });


            modelBuilder.Entity<Medicament>(e =>
            {
                e.HasKey(e => e.IdMedicament);
                e.Property(e => e.Name).HasMaxLength(120).IsRequired();
                e.Property(e => e.Description).HasMaxLength(220).IsRequired();
                e.Property(e => e.Type).HasMaxLength(120).IsRequired();

                e.HasData(new List<Medicament>
                { 
                    new Medicament
                    { 
                        IdMedicament = 1,
                        Name = "Apap",
                        Description = "Na ból głowy",
                        Type = "Tabletka"
                    },
                    new Medicament
                    {
                        IdMedicament = 2,
                        Name = "Gripex",
                        Description = "Gorączka",
                        Type = "Proszek"
                    }
                });
            });

            modelBuilder.Entity<Patient>(e =>
            {
                e.HasKey(e => e.IdPatient);
                e.Property(e => e.FirstName).HasMaxLength(120).IsRequired();
                e.Property(e => e.LastName).HasMaxLength(220).IsRequired();
                e.Property(e => e.BirthDate).IsRequired();

                e.HasData(new List<Patient>
                {
                    new Patient
                    {
                        IdPatient = 1,
                        FirstName = "Maciek",
                        LastName = "Musiała",
                        BirthDate = DateTime.Now
                    },
                    new Patient
                    {
                        IdPatient = 2,
                        FirstName = "Maja",
                        LastName = "Gabur",
                        BirthDate = DateTime.Now
                    }
                });
            });

            modelBuilder.Entity<Prescription>(e =>
            {
                e.HasKey(e => e.IdPrescription);
                e.Property(e => e.Date).IsRequired();
                e.Property(e => e.DueDate).IsRequired();
                e.HasOne(e => e.Doctors).WithMany(e => e.Prescriptions).HasForeignKey(e => e.IdDoctor).OnDelete(DeleteBehavior.ClientCascade);
                e.HasOne(e => e.Patients).WithMany(e => e.Prescriptions).HasForeignKey(e => e.IdPatient).OnDelete(DeleteBehavior.ClientCascade);

                e.ToTable("Prescription");

                e.HasData(new List<Prescription>
                { 
                    new Prescription
                    { 
                        IdPrescription = 1,
                        Date = DateTime.Now,
                        DueDate = DateTime.Now,
                        IdDoctor = 1,
                        IdPatient = 2,
                    },
                    new Prescription
                    {
                        IdPrescription = 2,
                        Date = DateTime.Now,
                        DueDate = DateTime.Now,
                        IdDoctor = 2,
                        IdPatient = 1,
                    }

                });
            });

            modelBuilder.Entity<PrescriptionMedicament>(e =>
            {
                e.HasKey(e => new {e.IdMedicament, e.IdPrescription });
                e.Property(e => e.Dose).IsRequired();
                e.Property(e => e.Details).HasMaxLength(220).IsRequired();
                e.HasOne(e => e.Medicament).WithMany(e => e.PerscriptionMedicaments).HasForeignKey(e => e.IdMedicament).OnDelete(DeleteBehavior.ClientCascade);
                e.HasOne(e => e.Prescription).WithMany(e => e.PrescriptionMedicaments).HasForeignKey(e => e.IdPrescription).OnDelete(DeleteBehavior.ClientCascade);

                e.ToTable("Prescription_Medicament");

                e.HasData(new List<PrescriptionMedicament>
                { 
                    new PrescriptionMedicament
                    { 
                        IdMedicament = 1,
                        IdPrescription = 1,
                        Dose = 30,
                        Details = "Czytaj ulotke"
                    },
                    new PrescriptionMedicament
                    {
                        IdMedicament = 2,
                        IdPrescription = 2,
                        Dose = 50,
                        Details = "Nie dla ciężarnych kobiet"
                    }


                });
            });
        }
    }
}
