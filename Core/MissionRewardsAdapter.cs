using System.Text.Json;

namespace Core
{
    public static class MissionRewardsAdapter
    {
        public static Dictionary<string, DropLocation> Parse(string json)
        {
            var file = JsonSerializer.Deserialize<MissionRewardsFileDto>(json);
            var best = new Dictionary<string, DropLocation>();

            if (file?.MissionRewards is null)
                return best;

            foreach (var (planet, nodes) in file.MissionRewards)
            {
                foreach (var (nodeName, node) in nodes)
                {
                    var location = $"{nodeName} ({planet})";

                    foreach (var reward in Flatten(node.Rewards))
                    {
                        var name = RewardNameParser.Parse(reward.ItemName).Name;

                        if (best.TryGetValue(name, out var existing)
                            && existing.Chance >= reward.Chance)
                            continue;

                        best[name] = new DropLocation(location, reward.Chance, reward.Rarity);
                    }
                }
            }

            return best;
        }

        private static IEnumerable<RewardDto> Flatten(JsonElement rewards)
        {
            if (rewards.ValueKind == JsonValueKind.Array)
                return rewards.Deserialize<List<RewardDto>>() ?? new List<RewardDto>();

            if (rewards.ValueKind == JsonValueKind.Object)
                return rewards.EnumerateObject()
                    .SelectMany(rotation =>
                        rotation.Value.Deserialize<List<RewardDto>>() ?? new List<RewardDto>());

            return Enumerable.Empty<RewardDto>();
        }
    }
}