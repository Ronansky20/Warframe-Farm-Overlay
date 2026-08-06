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
    }
}