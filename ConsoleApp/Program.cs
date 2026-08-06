using System.Net.Http;
using Core;

string url = "https://raw.githubusercontent.com/WFCD/warframe-items/master/data/json/Warframes.json";

using var http = new HttpClient();

Console.WriteLine("Fetching Warframe data...");
string json = await http.GetStringAsync(url);

var recipes = WarframeDataAdapter.Parse(json);
Console.WriteLine($"Loaded {recipes.Count} warframe recipes.");

Console.Write("Which warframe do you want to farm? ");
string target = Console.ReadLine() ?? "";

var inventory = new Dictionary<string, int>(); // own nothing for now

var plan = Planner.Plan(target, 1, inventory, recipes);

Console.WriteLine($"\nTo build {target}, farm:");
foreach (var item in plan)
{
    Console.WriteLine($"  {item.Value}x {item.Key}");
}