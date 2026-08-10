using System.Text.Json;

namespace Core
{
    public static class WarframeDataAdapter
    {
        public static Dictionary<string, Recipe> Parse(string json)
        {
            var warframes = JsonSerializer.Deserialize<List<WarframeDto>>(json) ?? new();

            var recipes = new Dictionary<string, Recipe>();

            foreach (var warframe in warframes)
            {
                var ingredients = new List<Ingredient>();

                foreach (var component in warframe.Components)
                {
                    string name = ResolveName(component, warframe.Name);
                    DropLocation? best = FindBestLocation(component);
                    ingredients.Add(new Ingredient(name, component.ItemCount, best));
                }

                recipes[warframe.Name] = new Recipe(ingredients);
            }

            return recipes;
        }

        private static string ResolveName(ComponentDto component, string frameName)
        {
            if (component.Drops.Count > 0)
                return component.Drops[0].Type;

            if (component.Name == "Blueprint")
                return $"{frameName} {component.Name}";

            return component.Name;
        }

        private static DropLocation? FindBestLocation(ComponentDto component)
        {
            if (component.Drops.Count == 0)
                return null;

            DropDto best = component.Drops[0];
            foreach (var drop in component.Drops)
            {
                if (drop.Chance > best.Chance)
                    best = drop;
            }

            return new DropLocation(best.Location, best.Chance, best.Rarity);
        }
    }
}