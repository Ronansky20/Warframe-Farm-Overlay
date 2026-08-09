using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public record DropLocation(string Location, double Chance, string Rarity);

    public record Ingredient(string Name, int Count, DropLocation? BestLocation = null);

    public record Recipe(List<Ingredient> Ingredients);

}
