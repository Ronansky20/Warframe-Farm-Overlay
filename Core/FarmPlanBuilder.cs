namespace Core
{
    public static class FarmPlanBuilder
    {
        public static FarmPlan Build(
            string target,
            int quantity,
            Dictionary<string, int> inventory,
            Dictionary<string, Recipe> recipes,
            Dictionary<string, DropLocation> locations)
        {
            var gross = Planner.Plan(target, quantity, new Dictionary<string, int>(), recipes);

            var net = Planner.Plan(target, quantity, inventory, recipes);

            var lines = gross
                .OrderBy(entry => entry.Key, StringComparer.OrdinalIgnoreCase)
                .Select(entry =>
                {
                    var needed = entry.Value;
                    var remaining = net.TryGetValue(entry.Key, out var value) ? value : 0;
                    var owned = needed - remaining;

                    locations.TryGetValue(entry.Key, out DropLocation? location);

                    return new FarmLine(entry.Key, owned, needed, location);
                })
                .ToList();

            return new FarmPlan(target, lines);
        }
    }
}