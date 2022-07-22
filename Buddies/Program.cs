using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Buddies
{
    class Program
    {

        static void Main(string[] args)
        {
            CallWebAPIAsync()
                .Wait();
        }
        static async Task CallWebAPIAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://swapi.dev/api/people");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(client.BaseAddress);

                var buddies = new Dictionary<string, List<string>>(); 

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsAsync<StarWarsData>();

                    foreach (var item in data.results)
                    {
                        var buddieKey = "";
                        foreach (var film in item.films)
                        {
                            var flimNum = film.Substring(film.Length - 2).Substring(0,1);

                            buddieKey += flimNum;
                        }
                        if (buddies.ContainsKey(buddieKey))
                        {
                            buddies[buddieKey].Add(item.name);
                        }
                        else
                        {
                            buddies.Add(buddieKey,new List<string>() { item.name });
                        }
                       

                    }

                    foreach (var key in buddies.Keys)
                    {
                       
                        foreach (var characterName in buddies[key])
                        {
                            Console.Write($"{characterName}, ");
                        }
                        Console.WriteLine();
                    }
                   
                    
                }
            }
        }
    }
}
