namespace Core.Tests
{
    public class FarmPlanBuilderTests
    {
        [Fact]
        public void Build_ReturnsSortedLines_WithNeededCountsAndLocations()
        {
            var recipes = new Dictionary<string, Recipe>
            {
                ["Ash"] = new Recipe(new List<Ingredient>
                {
                    new Ingredient("Ash Neuroptics Blueprint", 1),
                    new Ingredient("Ash Chassis Blueprint", 1)
                })
            };

            var locations = new Dictionary<string, DropLocation>
            {
                ["Ash Chassis Blueprint"] = new DropLocation("Pavlov (Lua)", 11.28, "Rare")
            };

            var plan = FarmPlanBuilder.Build(
                target: "Ash",
                quantity: 1,
                inventory: new Dictionary<string, int>(),
                recipes: recipes,
                locations: locations);

            Assert.Equal("Ash", plan.Target);

            Assert.Collection(plan.Lines,
                line =>
                {
                    Assert.Equal("Ash Chassis Blueprint", line.Name);
                    Assert.Equal(0, line.Owned);
                    Assert.Equal(1, line.Needed);
                    Assert.Equal("Pavlov (Lua)", line.BestLocation?.Location);
                },
                line =>
                {
                    Assert.Equal("Ash Neuroptics Blueprint", line.Name);
                    Assert.Equal(0, line.Owned);
                    Assert.Equal(1, line.Needed);
                    Assert.Null(line.BestLocation);
                });
        }
    }
}