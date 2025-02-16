using Exercise3.Models;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Exercise3.Services
{
    public interface IFileDbService
    {
        public IEnumerable<Student> Students { get; set; }
        Task SaveChanges();
    }

    public class FileDbService : IFileDbService
    {
        private readonly string _pathToFileDatabase;

        public IEnumerable<Student> Students { get; set; } = new List<Student>();
        public FileDbService(IConfiguration configuration)
        {
            _pathToFileDatabase = configuration.GetConnectionString("Default") ?? throw new ArgumentNullException(nameof(configuration));
            Initialize();
        }

        private void Initialize()
        {
            if (!File.Exists(_pathToFileDatabase))
            {
                throw new FileNotFoundException(nameof(_pathToFileDatabase));
            }
            var lines = File.ReadLines(_pathToFileDatabase);

            var students = new List<Student>();                  
          


            //Tutaj należy przeparsować dane ze zmiennej lines, tak jak w drugim zadaniu
            lines.ToList().ForEach(line =>
            {
                var splittedLine = line.Split(',');
                var student = new Student
                {
                    IndexNumber = splittedLine[2],
                    FirstName = splittedLine[0],
                    LastName = splittedLine[1],
                    BirthDate = splittedLine[3],
                    Email = splittedLine[6],
                    MothersName = splittedLine[7],
                    FathersName = splittedLine[8],
                    StudyName = splittedLine[4],
                    StudyMode = splittedLine[5],
                };

                students.Add(student);
            });
         

            

            Students = students;
            
        }

        public async Task SaveChanges()
        {
          

            await File.WriteAllLinesAsync(
                _pathToFileDatabase,
                Students.Select(e => $"{e.FirstName},{e.LastName},{e.IndexNumber},{e.BirthDate},{e.StudyName},{e.StudyMode},{e.Email},{e.FathersName},{e.MothersName}")
                
                //tutaj należy zapewnić listę stringów zawierającą odpowiednio sformatowane dane studentów
                     // np. Jan,Kowalski,s1234,3/20/1991,Informatyka,Dzienne,kowalski@wp.pl,Jan,Anna

                );
        }

       
    }
}
