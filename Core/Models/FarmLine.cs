namespace Core
{
    public record FarmLine(string Name, int Owned, int Needed, DropLocation? BestLocation);
}