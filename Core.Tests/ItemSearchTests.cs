using Core;
using Xunit;

namespace Core.Tests
{
    public class ItemSearchTests
    {
        [Fact]
        public void Match_IsCaseInsensitiveAndMatchesWordStarts()
        {
            var names = new List<string> { "Nekros", "Nekros Prime", "Blade Storm", "Rhino" };

            var results = ItemSearch.Match("nek", names);

            Assert.Contains("Nekros", results);
            Assert.Contains("Nekros Prime", results);
            Assert.DoesNotContain("Rhino", results);
        }

        [Fact]
        public void Match_MatchesAWordThatIsntTheFirst()
        {
            var names = new List<string> { "Blade Storm", "Rhino" };

            var results = ItemSearch.Match("storm", names);

            Assert.Contains("Blade Storm", results);
        }
    }
}