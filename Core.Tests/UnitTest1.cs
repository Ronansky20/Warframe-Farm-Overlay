namespace Core.Tests
{
    public class PlannerTests
    {
        [Fact]
        public void AskingForABaseMaterial_TellsYouToFarmThatMaterial()
        {
            var plan = Planner.Plan("Neurodes", 5, new Dictionary<string, int>(), new Dictionary<string, Recipe>());

            Assert.Equal(5, plan["Neurodes"]);
        }

        [Fact]
        public void WhenYouAlreadyOwnSome_YouOnlyFarmTheRest()
        {
            var inventory = new Dictionary<string, int> { ["Neurodes"] = 2 };

            var plan = Planner.Plan("Neurodes", 5, inventory, new Dictionary<string, Recipe>());

            Assert.Equal(3, plan["Neurodes"]);
        }

        [Fact]
        public void WhenYouOwnMoreThanYouNeed_YouFarmNothing()
        {
            var inventory = new Dictionary<string, int> { ["Neurodes"] = 8 };

            var plan = Planner.Plan("Neurodes", 5, inventory, new Dictionary<string, Recipe>());

            Assert.Equal(0, plan["Neurodes"]);
        }

        [Fact]
        public void WhenTargetIsCraftable_YouFarmItsIngredientsInstead()
        {
            var recipes = new Dictionary<string, Recipe>
            {
                ["Reaper Handle"] = new Recipe(new List<Ingredient>
        {
            new Ingredient("Alloy Plate", 500),
            new Ingredient("Rubedo", 150)
        })
            };

            var inventory = new Dictionary<string, int>(); // own nothing

            var plan = Planner.Plan("Reaper Handle", 1, inventory, recipes);

            Assert.Equal(500, plan["Alloy Plate"]);
            Assert.Equal(150, plan["Rubedo"]);
        }
    }
}
