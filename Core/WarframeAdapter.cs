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
                    string name = component.Drops.Count > 0
                        ? component.Drops[0].Type
                        : component.Name;

                    ingredients.Add(new Ingredient(name, component.ItemCount));
                }

                recipes[warframe.Name] = new Recipe(ingredients);
            }

            return recipes;
        }
    }
}