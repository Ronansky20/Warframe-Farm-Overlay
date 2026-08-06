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

        [Fact]
        public void WhenCraftingMultiple_IngredientsScaleUp()
        {
            var recipes = new Dictionary<string, Recipe>
            {
                ["Reaper Handle"] = new Recipe(new List<Ingredient>
        {
            new Ingredient("Alloy Plate", 500),
            new Ingredient("Rubedo", 150)
        })
            };

            var inventory = new Dictionary<string, int>();

            var plan = Planner.Plan("Reaper Handle", 2, inventory, recipes);

            Assert.Equal(1000, plan["Alloy Plate"]); // 500 × 2
            Assert.Equal(300, plan["Rubedo"]);       // 150 × 2
        }

        [Fact]
        public void WhenIngredientIsItselfCraftable_ItBreaksDownToRawMaterials()
        {
            var recipes = new Dictionary<string, Recipe>
            {
                ["Reaper"] = new Recipe(new List<Ingredient>
        {
            new Ingredient("Reaper Handle", 1),
            new Ingredient("Reaper Blade", 1)
        }),
                ["Reaper Handle"] = new Recipe(new List<Ingredient>
        {
            new Ingredient("Alloy Plate", 500)
        }),
                ["Reaper Blade"] = new Recipe(new List<Ingredient>
        {
            new Ingredient("Alloy Plate", 300),
            new Ingredient("Rubedo", 150)
        })
            };

            var inventory = new Dictionary<string, int>();

            var plan = Planner.Plan("Reaper", 1, inventory, recipes);

            // Reaper → Handle (500 Alloy) + Blade (300 Alloy + 150 Rubedo)
            // Alloy Plate should SUM across both: 500 + 300 = 800
            Assert.Equal(800, plan["Alloy Plate"]);
            Assert.Equal(150, plan["Rubedo"]);
            Assert.False(plan.ContainsKey("Reaper Handle")); // not a raw material — shouldn't appear
        }
    }
}
