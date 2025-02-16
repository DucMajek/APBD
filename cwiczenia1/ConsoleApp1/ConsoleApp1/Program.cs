using System;
using System.Net.Http;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            /*
                Strony do testów
                null wymaganie nr 1
                "hts://pjwstk.edu.pl" wymaganie nr 2
                "https://invalid-url" wymaganie nr 3
                "https://en.wikipedia.org/wiki/Main_Page" wymaganie nr 4
                "https://pjwstk.edu.pl" wymaganie nr 5
             */

            args[0] = "https://invalid-url";
            var url = new Uri(args[0]); 

            Console.WriteLine("Adres strony: " + url);
            Console.WriteLine("====================================");

            
            // warunek ktory sprawdza czy url jest null
            if (url == null)
            {
                throw new ArgumentNullException("Value cannot be null.");
            }
            // warunek sprawdza czy url jest poprawny i czy posiada poprawny uzywany protokol
            if (!Uri.TryCreate(url.AbsoluteUri, UriKind.Absolute, out Uri result) || (result.Scheme != Uri.UriSchemeHttp && result.Scheme != Uri.UriSchemeHttps))
            {
                throw new ArgumentException("Niepoprawny URL");
            }

            else
            {
                var regex = new Regex(@"[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+");

                var httpClient = new HttpClient();

                try
                {
                    var httpResult = await httpClient.GetAsync(url);

                    if (httpResult.IsSuccessStatusCode) { 
                        var httpContent = await httpResult.Content.ReadAsStringAsync();
                        var matches = regex.Matches(httpContent);

                        if (matches.Count == 0)
                        {
                            throw new Exception("Nie znaleziono adresów email");
                        }

                        else
                        {
                            foreach (Match match in matches)
                            {
                                Console.WriteLine(match.Value);
                            }

                            matches.Select(x => x.Value).Distinct().ToList().ForEach(x => Console.WriteLine(x));
                        }
                    }
                }


                catch (Exception ex)
                {
                    throw new Exception("Błąd w czasie pobierania strony");
                }

               
                
            }
        }
    }
}