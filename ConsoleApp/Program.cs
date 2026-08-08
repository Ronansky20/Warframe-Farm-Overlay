using System.Net.Http;
using Core;

string[] urls =
{
    "https://raw.githubusercontent.com/WFCD/warframe-items/master/data/json/Warframes.json",
    "https://raw.githubusercontent.com/WFCD/warframe-items/master/data/json/Primary.json",
    "https://raw.githubusercontent.com/WFCD/warframe-items/master/data/json/Secondary.json",
    "https://raw.githubusercontent.com/WFCD/warframe-items/master/data/json/Melee.json",
};

using var http = new HttpClient();

var recipes = new Dictionary<string, Recipe>();

foreach (string url in urls)
{
    Console.WriteLine($"Fetching {url.Split('/').Last()}...");
    string json = await http.GetStringAsync(url);

    var parsed = WarframeDataAdapter.Parse(json);
    foreach (var entry in parsed)
    {
        recipes[entry.Key] = entry.Value; // add/merge into the combined set
    }
}

Console.WriteLine($"Loaded {recipes.Count} recipes total.");

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

Console.Write("What do you want to farm? ");
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