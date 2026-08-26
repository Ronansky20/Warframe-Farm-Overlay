namespace Core.Tests
{
    public class DropLocationMergerTests
    {
        [Fact]
        public void Merge_FillsGapsFromTheFallbackSource()
        {
            var primary = new Dictionary<string, DropLocation>
            {
                ["Ash Chassis Blueprint"] = new DropLocation("Pavlov (Lua)", 11.28, "Rare")
            };

            var fallback = new Dictionary<string, DropLocation>
            {
                ["Neurodes"] = new DropLocation("Marduk (Void)", 25.29, "Rare")
            };

            var merged = DropLocationMerger.Merge(primary, fallback);

            Assert.Equal(2, merged.Count);
            Assert.Equal("Pavlov (Lua)", merged["Ash Chassis Blueprint"].Location);
            Assert.Equal("Marduk (Void)", merged["Neurodes"].Location);
        }

        [Fact]
        public void Merge_KeepsThePrimarySourceOnCollision()
        {
            var primary = new Dictionary<string, DropLocation>
            {
                ["Lith A1 Relic"] = new DropLocation("Hepit (Void)", 12.5, "Common")
            };

            var fallback = new Dictionary<string, DropLocation>
            {
                ["Lith A1 Relic"] = new DropLocation("Somewhere Else (Mars)", 99.0, "Common")
            };

            var merged = DropLocationMerger.Merge(primary, fallback);

            Assert.Equal("Hepit (Void)", merged["Lith A1 Relic"].Location);
        }

        [Fact]
        public void Merge_CuratedAdviceBeatsMissionRewardsForResources()
        {
            var missionRewards = new Dictionary<string, DropLocation>
            {
                ["Plastids"] = new DropLocation("Desdemona (Caches) (Uranus)", 15.49, "Common")
            };

            var withCurated = DropLocationMerger.Merge(CuratedResourceLocations.All, missionRewards);

            Assert.Equal("Assur (Uranus) — Survival, Dark Sector", withCurated["Plastids"].Location);
            Assert.Null(withCurated["Plastids"].Chance);
        }
    }
}