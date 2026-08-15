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

        [Fact]
        public void Build_CountsWhatHeAlreadyOwns_TowardTheTarget()
        {
            var recipes = new Dictionary<string, Recipe>
            {
                ["Nekros Prime"] = new Recipe(new List<Ingredient>
                {
                    new Ingredient("Nekros Prime Systems Blueprint", 1),
                    new Ingredient("Orokin Cell", 3)
                })
            };

            var inventory = new Dictionary<string, int> { ["Orokin Cell"] = 1 };

            var plan = FarmPlanBuilder.Build(
                "Nekros Prime", 1, inventory, recipes, new Dictionary<string, DropLocation>());

            var cell = plan.Lines.Single(line => line.Name == "Orokin Cell");
            Assert.Equal(1, cell.Owned);
            Assert.Equal(3, cell.Needed);

            var systems = plan.Lines.Single(line => line.Name == "Nekros Prime Systems Blueprint");
            Assert.Equal(0, systems.Owned);
            Assert.Equal(1, systems.Needed);
        }

        [Fact]
        public void Build_WhenHeOwnsMoreThanNeeded_ShowsTheLineAsComplete()
        {
            var recipes = new Dictionary<string, Recipe>
            {
                ["Nekros Prime"] = new Recipe(new List<Ingredient>
                {
                    new Ingredient("Orokin Cell", 3)
                })
            };

            var inventory = new Dictionary<string, int> { ["Orokin Cell"] = 10 };

            var plan = FarmPlanBuilder.Build(
                "Nekros Prime", 1, inventory, recipes, new Dictionary<string, DropLocation>());

            var cell = Assert.Single(plan.Lines);
            Assert.Equal(3, cell.Owned);   // NOT 10 — owned *toward this target*
            Assert.Equal(3, cell.Needed);
        }
    }
}