using Core;
using Xunit;

namespace Core.Tests
{
    public class WarframeDataAdapterTests
    {
        private const string AshJson = """
        [
          {
            "name": "Ash",
            "components": [
              {
                "name": "Chassis",
                "itemCount": 1,
                "drops": [
                  { "type": "Ash Chassis Blueprint", "location": "Pluto/Fenton's Field", "chance": 13.33, "rarity": "Uncommon" }
                ]
              }
            ]
          }
        ]
        """;

        [Fact]
        public void Parse_ProducesARecipeKeyedByWarframeName()
        {
            var recipes = WarframeDataAdapter.Parse(AshJson);

            Assert.True(recipes.ContainsKey("Ash"));
        }

        [Fact]
        public void Parse_NamesComponentsByDropTypeWithCorrectCount()
        {
            var recipes = WarframeDataAdapter.Parse(AshJson);

            var ash = recipes["Ash"];

            Assert.Contains(ash.Ingredients, i => i.Name == "Ash Chassis Blueprint" && i.Count == 1);
        }

        private const string TwoFramesJson = """
            [
              {
                "name": "Ash",
                "components": [
                  { "name": "Blueprint", "itemCount": 1, "drops": [] }
                ]
              },
              {
                "name": "Rhino",
                "components": [
                  { "name": "Blueprint", "itemCount": 1, "drops": [] }
                ]
              }
            ]
            """;

        private const string ResourceJson = """
            [
              {
                "name": "Ash",
                "components": [
                  { "name": "Orokin Cell", "itemCount": 1, "drops": [], "type": "Resource" }
                ]
              }
            ]
            """;

        [Fact]
        public void Parse_KeepsResourcesUnderTheirOwnName()
        {
            var recipes = WarframeDataAdapter.Parse(ResourceJson);

            Assert.Contains(recipes["Ash"].Ingredients, i => i.Name == "Orokin Cell" && i.Count == 1);
        }

        [Fact]
        public void Parse_GivesEachWarframesBlueprintAUniqueName()
        {
            var recipes = WarframeDataAdapter.Parse(TwoFramesJson);

            Assert.Contains(recipes["Ash"].Ingredients, i => i.Name == "Ash Blueprint");
            Assert.Contains(recipes["Rhino"].Ingredients, i => i.Name == "Rhino Blueprint");
        }

        private const string DropChanceJson = """
            [
              {
                "name": "Ash",
                "components": [
                  {
                    "name": "Chassis",
                    "itemCount": 1,
                    "drops": [
                      { "type": "Ash Chassis Blueprint", "location": "Pluto/Profit Margin", "chance": 4.82, "rarity": "Rare" },
                      { "type": "Ash Chassis Blueprint", "location": "Pluto/Fenton's Field", "chance": 13.33, "rarity": "Uncommon" }
                    ]
                  }
                ]
              }
            ]
            """;

        [Fact]
        public void Parse_PicksTheHighestChanceDropAsBestLocation()
        {
            var recipes = WarframeDataAdapter.Parse(DropChanceJson);

            var chassis = recipes["Ash"].Ingredients.First(i => i.Name == "Ash Chassis Blueprint");

            Assert.NotNull(chassis.BestLocation);
            Assert.Equal("Pluto/Fenton's Field", chassis.BestLocation!.Location);
        }
    }
}