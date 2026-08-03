namespace Core
{
    public class Planner
    {
        public static Dictionary<string, int> Plan(string target, int quantity, Dictionary<string, int> inventory)
        {
            var result = new Dictionary<string, int>();

            int owned = inventory.GetValueOrDefault(target);
            int need = Math.Max(0, quantity - owned);

            result[target] = need;
            return result;
        }
    }
}
