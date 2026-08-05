namespace Core
{
    public class Planner
    {
        public static Dictionary<string, int> Plan(string target, int quantity, Dictionary<string, int> inventory, Dictionary<string, Recipe> recipes)
        {
            var result = new Dictionary<string, int>();

            if (recipes.TryGetValue(target, out var recipe))
            {
                foreach(var ingredient in recipe.Ingredients)
                {
                    result[ingredient.Name] = ingredient.Count * quantity;
                }

            }
            else
            {
                int owned = inventory.GetValueOrDefault(target);
                int need = Math.Max(0, quantity - owned);

                result[target] = need;
            }

            return result;
        }
    }
}
