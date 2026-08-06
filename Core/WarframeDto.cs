using System.Text.Json.Serialization;

namespace Core
{
    public class WarframeDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("components")]
        public List<ComponentDto> Components { get; set; } = new();
    }

    public class ComponentDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("itemCount")]
        public int ItemCount { get; set; }

        [JsonPropertyName("drops")]
        public List<DropDto> Drops { get; set; } = new();

        [JsonPropertyName("type")]
        public string Type { get; set; } = "";
    }

    public class DropDto
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "";

        [JsonPropertyName("location")]
        public string Location { get; set; } = "";

        [JsonPropertyName("chance")]
        public double Chance { get; set; }

        [JsonPropertyName("rarity")]
        public string Rarity { get; set; } = "";
    }
}