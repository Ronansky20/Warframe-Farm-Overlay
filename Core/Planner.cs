namespace Core
{
    public class Planner
    {
        public static Dictionary<string, int> Plan(string target, int quantity)
        {
            var result = new Dictionary<string, int>();
            result[target] = quantity;
            return result;
        }
    }
}
