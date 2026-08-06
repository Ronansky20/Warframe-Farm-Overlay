namespace Core
{
    public class Planner
    {
        public static Dictionary<string, int> Plan(string target, int quantity, Dictionary<string, int> inventory, Dictionary<string, Recipe> recipes)
        {
            var result = new Dictionary<string, int>();
            AddMaterials(target, quantity, inventory, recipes, result);
            return result;
        }

        private static void AddMaterials(string item, int quantity, Dictionary<string, int> inventory, Dictionary<string, Recipe> recipes, Dictionary<string, int> result)
        {
            if (recipes.TryGetValue(item, out var recipe))
            {
                foreach (var ingredient in recipe.Ingredients)
                {
                    AddMaterials(ingredient.Name, ingredient.Count * quantity, inventory, recipes, result);
                }
            }
            else
            {
                // Raw material → subtract what we own, then add to the tally.
                int owned = inventory.GetValueOrDefault(item);
                int need = Math.Max(0, quantity - owned);
                result[item] = result.GetValueOrDefault(item) + need;
            }
        }
    }
}
