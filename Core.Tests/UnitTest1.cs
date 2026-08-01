namespace Core.Tests
{
    public class PlannerTests
    {
        [Fact]
        public void AskingForABaseMaterial_TellsYouToFarmThatMaterial()
        {
            var plan = Planner.Plan("Neurodes", 5);

            Assert.Equal(5, plan["Neurodes"]);
        }
    }
}
