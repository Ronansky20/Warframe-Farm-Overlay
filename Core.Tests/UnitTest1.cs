namespace Core.Tests
{
    public class PlannerTests
    {
        [Fact]
        public void AskingForABaseMaterial_TellsYouToFarmThatMaterial()
        {
            var plan = Planner.Plan("Neurodes", 5, new Dictionary<string, int>());

            Assert.Equal(5, plan["Neurodes"]);
        }

        [Fact]
        public void WhenYouAlreadyOwnSome_YouOnlyFarmTheRest()
        {
            var inventory = new Dictionary<string, int> { ["Neurodes"] = 2 };

            var plan = Planner.Plan("Neurodes", 5, inventory);

            Assert.Equal(3, plan["Neurodes"]);
        }

        [Fact]
        public void WhenYouOwnMoreThanYouNeed_YouFarmNothing()
        {
            var inventory = new Dictionary<string, int> { ["Neurodes"] = 8 };

            var plan = Planner.Plan("Neurodes", 5, inventory);

            Assert.Equal(0, plan["Neurodes"]);
        }
    }
}
