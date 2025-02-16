// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

Console.WriteLine("Hello, World!");

var csvPath = args[0];
var output = args[1];
var logsPath = args[2];
var format = args[3];

if (args.Length > 4) 
{
    throw new ArgumentOutOfRangeException();
}

if (!File.Exists(csvPath)) 
{
    throw new FileNotFoundException();
}

if (!Directory.Exists(output))
{
    throw new DirectoryNotFoundException();
}

if (!File.Exists(logsPath))
{
    throw new FileNotFoundException();
}

if (format != "json")
{
    throw new InvalidOperationException();
}

var csvContent = File.ReadLines(csvPath);
var students = new List<Student>();
var dict = new Dictionary<string, int>();
var logs = File.CreateText("./logs.txt");

csvContent.ToList().ForEach(line =>
{
    var splittedLine = line.Split(',');
    if (splittedLine.Length != 9)
    {
        logs.WriteLine($"Wiersz nie posiada odpowiedniej ilości kolumn: {line}");
        return;
    }
    if (splittedLine.Any(e => e.Trim() == ""))
    {
        logs.WriteLine($"Wiersz nie może posiadać pustych kolumn: {line}");
        return;
    }

    var studies = new Studies
    {
        Name = splittedLine[2],
        Mode = splittedLine[3],
    };

    var student = new Student
    {
        IndexNumber = splittedLine[4],
        Fname = splittedLine[0],
        Lname = splittedLine[1],
        Birthdate = DateTime.Parse(splittedLine[5]),
        Email = splittedLine[6],
        MothersName = splittedLine[7],
        FathersName = splittedLine[8],
        Studies = studies
    };

    if (students.Any(e => e.IndexNumber == student.IndexNumber && e.Lname == student.Lname && e.Fname == student.Fname))
    {
        logs.WriteLine($"Duplikat: {line}");
        return;
    }

    dict[studies.Name] = !dict.ContainsKey(studies.Name) ? 1 : dict[studies.Name] + 1;
    students.Add(student);
});

File.WriteAllText("./uczelnia.json", JsonSerializer.Serialize(
    new UczelniaWrapper
    {
        Uczelnia = new Uczelnia
        {
            CreatedAt = DateTime.Now,
            Author = "ko",
            Students = students,
            ActiveStudies = dict.Select(e => new ActiveStudies { Name = e.Key, NumberOfStudents = e.Value })
        }
    }, new JsonSerializerOptions
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        //camelCase

    }

));

class Studies
{
    public string Name { get; set; }
    public string Mode { get; set; }
}

class Uczelnia
{
    public DateTime CreatedAt { get; set; }
    public string Author { get; set; }
    public IEnumerable<Student> Students { get; set; }
    public IEnumerable<ActiveStudies> ActiveStudies { get; set; }
}

class UczelniaWrapper
{
    public Uczelnia Uczelnia { get; set; }
}

class ActiveStudies
{
    public string Name { get; set; }
    public int NumberOfStudents { get; set; }
}