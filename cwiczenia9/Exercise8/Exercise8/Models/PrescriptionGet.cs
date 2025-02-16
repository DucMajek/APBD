namespace Exercise8.Models
{
    public class PrescriptionGet
    {
        public int Dose { get; set; }
        public string Details { get; set; }

        public int IdDoctor { get; set; }

        public string FirstNameDoctor { get; set; } = null!;

        public string LastNameDoctor { get; set; } = null!;

        public string EmailDoctor { get; set; } = null!;


        public int IdPatient { get; set; }
        public string FirstNamePatient { get; set; } = null!;
        public string LastNamePatient { get; set; } = null!;
        public DateTime BirthdatePatient { get; set; }

        public List<Medicament> listMed { get; set; }


    }
}
