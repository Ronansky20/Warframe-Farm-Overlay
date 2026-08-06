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
                    ingredients.Add(new Ingredient(name, component.ItemCount));
                }

                recipes[warframe.Name] = new Recipe(ingredients);
            }

            return recipes;
        }

        private static string ResolveName(ComponentDto component, string frameName)
        {
            // 1. Has drops → use the drop's type (e.g. "Ash Chassis Blueprint").
            if (component.Drops.Count > 0)
                return component.Drops[0].Type;

            // 2. Main blueprint (no drops) → prefix with frame name (e.g. "Ash Blueprint").
            if (component.Name == "Blueprint")
                return $"{frameName} {component.Name}";

            // 3. Otherwise it's a resource (e.g. "Orokin Cell") → use its own name.
            return component.Name;
        }
    }
}