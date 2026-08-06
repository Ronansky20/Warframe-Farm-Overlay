using System.Net.Http;

string url = "https://raw.githubusercontent.com/WFCD/warframe-items/master/data/json/Warframes.json";

using var http = new HttpClient();

string json = await http.GetStringAsync(url);

Console.WriteLine($"Got {json.Length} characters of JSON.");
Console.WriteLine(json.Substring(0, 500));