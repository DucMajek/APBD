using Exercise6;

Console.WriteLine("Cwiczenia rozwiazujemy w pliku LinqTasks.cs");

var results = LinqTasks.Task6();
results.ToList().ForEach(x => Console.WriteLine(x));