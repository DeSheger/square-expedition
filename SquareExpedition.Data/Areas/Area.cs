using System.Numerics;
using SquareExpedition.Data.Interactions;
using SquareExpedition.Data.Terrains;

namespace SquareExpedition.Data.Areas;

public class Area
{
    public Guid Id { get; init; }
    
    public TerrainSize Size { get; init; }
    
    public ICollection<Localization> Localizations { get; set; } = new List<Localization>();

    public Localization? GetLocalizationUsingCords(Vector3 vector) =>
        Localizations.SingleOrDefault(l => l.GetCoordinates() == vector);
}