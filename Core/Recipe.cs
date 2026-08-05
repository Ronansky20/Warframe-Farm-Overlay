using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public record Ingredient(string Name, int Count);

    public record Recipe(List<Ingredient> Ingredients);
}
