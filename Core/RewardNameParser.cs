using System.Text.RegularExpressions;

namespace Core
{
    public static class RewardNameParser
    {
        private static readonly Regex QuantityPrefix =
            new(@"^(\d[\d,]*)X (.+)$", RegexOptions.Compiled);

        public static RewardName Parse(string raw)
        {
            var match = QuantityPrefix.Match(raw);

            if (!match.Success)
                return new RewardName(raw, 1);

            var digits = match.Groups[1].Value.Replace(",", "");

            if (!int.TryParse(digits, out var quantity))
                return new RewardName(raw, 1);

            return new RewardName(match.Groups[2].Value, quantity);
        }
    }
}