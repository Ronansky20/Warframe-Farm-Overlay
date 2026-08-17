namespace Core.Tests
{
    public class MissionRewardsAdapterTests
    {
        [Fact]
        public void Parse_ReadsBothRotationShapedAndFlatRewards()
        {
            var json = """
            {
              "missionRewards": {
                "Sedna": {
                  "Hydron": {
                    "gameMode": "Defense",
                    "isEvent": false,
                    "rewards": {
                      "A": [ { "itemName": "Neurodes", "chance": 10.0, "rarity": "Rare" } ]
                    }
                  },
                  "Berehynia": {
                    "gameMode": "Interception",
                    "isEvent": false,
                    "rewards": [ { "itemName": "Orokin Cell", "chance": 5.0, "rarity": "Uncommon" } ]
                  }
                }
              }
            }
            """;

            var locations = MissionRewardsAdapter.Parse(json);

            Assert.Equal("Hydron (Sedna)", locations["Neurodes"].Location);
            Assert.Equal(10.0, locations["Neurodes"].Chance);
            Assert.Equal("Rare", locations["Neurodes"].Rarity);

            Assert.Equal("Berehynia (Sedna)", locations["Orokin Cell"].Location);
            Assert.Equal(5.0, locations["Orokin Cell"].Chance);
        }

        [Fact]
        public void Parse_StripsQuantityPrefixesFromItemNames()
        {
            var json = """
            {
              "missionRewards": {
                "Eris": {
                  "Candiru": {
                    "gameMode": "Survival",
                    "isEvent": false,
                    "rewards": {
                      "C": [ { "itemName": "2X Neurodes", "chance": 15.49, "rarity": "Rare" } ]
                    }
                  }
                }
              }
            }
            """;

            var locations = MissionRewardsAdapter.Parse(json);

            Assert.True(locations.ContainsKey("Neurodes"));
            Assert.False(locations.ContainsKey("2X Neurodes"));
            Assert.Equal("Candiru (Eris)", locations["Neurodes"].Location);
        }
    }
}