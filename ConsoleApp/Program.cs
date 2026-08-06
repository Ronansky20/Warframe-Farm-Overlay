using System.Net.Http;
using Core;

string url = "https://raw.githubusercontent.com/WFCD/warframe-items/master/data/json/Warframes.json";

using var http = new HttpClient();

Console.WriteLine("Fetching Warframe data...");
string json = await http.GetStringAsync(url);

var recipes = WarframeDataAdapter.Parse(json);
Console.WriteLine($"Loaded {recipes.Count} warframe recipes.");

// Build a flat lookup: material name → its best drop location.
var locations = new Dictionary<string, DropLocation>();
foreach (var recipe in recipes.Values)
{
    foreach (var ingredient in recipe.Ingredients)
    {
        if (ingredient.BestLocation != null)
            locations[ingredient.Name] = ingredient.BestLocation;
    }
}

Console.Write("Which warframe do you want to farm? ");
string target = Console.ReadLine() ?? "";

var inventory = new Dictionary<string, int>();

var plan = Planner.Plan(target, 1, inventory, recipes);

Console.WriteLine($"\nTo build {target}, farm:");
foreach (var item in plan)
{
    if (locations.TryGetValue(item.Key, out var loc))
        Console.WriteLine($"  {item.Value}x {item.Key} — best at {loc.Location} ({loc.Chance}%)");
    else
        Console.WriteLine($"  {item.Value}x {item.Key} — no drop location");
}