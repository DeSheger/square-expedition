using SquareExpedition.Application.Abstract;
using SquareExpedition.Data.Areas;
using SquareExpedition.Data.Interactions;
using SquareExpedition.Data.Terrains;

namespace SquareExpedition.Application.Services;

public class AreaGeneratorService : IAreaGeneratorService
{
    public Area GenerateNewArea(TerrainSize size)
    {
        var area = GetPrimaryAreaInfo(size);

        area.Localizations = GenerateLocalizations(area).ToList();

        return area;
    }

    public Area GetAreaInfo()
    {
        throw new NotImplementedException();
    }

    private IEnumerable<Localization> GenerateLocalizations(Area terrain)
    {
        var totalBlocks = (int)terrain.Size;

        var dimension = (int)Math.Sqrt(totalBlocks);

        var offset = dimension / 2;

        for (var x = 0; x < dimension; x++)
            for (var y = 0; y < 10; y++)
                for (var z = 0; z < dimension; z++)
                {
                    yield return new Localization(x-offset, y, z-offset);
                }
    }

    private Area GetPrimaryAreaInfo(TerrainSize size)
    {
        return new Area()
        {
            Id = new Guid(),
            Size = size,
        };
    }
}