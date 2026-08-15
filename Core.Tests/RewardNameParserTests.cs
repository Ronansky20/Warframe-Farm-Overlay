namespace Core.Tests
{
    public class RewardNameParserTests
    {
        [Theory]
        [InlineData("Neurodes", "Neurodes", 1)]
        [InlineData("2X Neurodes", "Neurodes", 2)]
        [InlineData("750X Circuits", "Circuits", 750)]
        [InlineData("1000X Nano Spores", "Nano Spores", 1000)]
        public void Parse_SplitsQuantityPrefixFromName(string raw, string expectedName, int expectedQuantity)
        {
            var parsed = RewardNameParser.Parse(raw);

            Assert.Equal(expectedName, parsed.Name);
            Assert.Equal(expectedQuantity, parsed.Quantity);
        }

        [Theory]
        [InlineData("100 Endo")]
        [InlineData("1,500 Credits Cache")]
        [InlineData("3 Day Affinity Booster")]
        [InlineData("Orokin Cell")]
        public void Parse_LeavesNamesWithoutAnXPrefixAlone(string raw)
        {
            var parsed = RewardNameParser.Parse(raw);

            Assert.Equal(raw, parsed.Name);
            Assert.Equal(1, parsed.Quantity);
        }
    }
}