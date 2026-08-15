using System.Text.Json;
using System.Text.Json.Serialization;

namespace Core
{
    public class MissionRewardsFileDto
    {
        [JsonPropertyName("missionRewards")]
        public Dictionary<string, Dictionary<string, NodeDto>> MissionRewards { get; set; } = new();
    }

    public class NodeDto
    {
        [JsonPropertyName("gameMode")]
        public string? GameMode { get; set; }

        [JsonPropertyName("rewards")]
        public JsonElement Rewards { get; set; }
    }

    public class RewardDto
    {
        [JsonPropertyName("itemName")]
        public string ItemName { get; set; } = "";

        [JsonPropertyName("chance")]
        public double Chance { get; set; }

        [JsonPropertyName("rarity")]
        public string Rarity { get; set; } = "";
    }
}