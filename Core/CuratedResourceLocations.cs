namespace Core
{
    public static class CuratedResourceLocations
    {
        public static readonly Dictionary<string, DropLocation> All = new()
        {
            ["Plastids"] = new DropLocation("Assur (Uranus) — Survival, Dark Sector", null, "Common"),
            ["Neurodes"] = new DropLocation("Hieracon (Pluto) — Excavation", null, "Rare"),
            ["Orokin Cell"] = new DropLocation("Gabii (Ceres) — Survival, Dark Sector", null, "Rare"),
        };
    }
}