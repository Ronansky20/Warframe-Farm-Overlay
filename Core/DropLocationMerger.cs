namespace Core
{
    public static class DropLocationMerger
    {
        public static Dictionary<string, DropLocation> Merge(
            Dictionary<string, DropLocation> primary,
            Dictionary<string, DropLocation> fallback)
        {
            var merged = new Dictionary<string, DropLocation>(fallback);

            foreach (var (name, location) in primary)
                merged[name] = location;

            return merged;
        }
    }
}