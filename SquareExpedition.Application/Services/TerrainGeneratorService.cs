using System.Numerics;
using SquareExpedition.Application.Abstract;
using SquareExpedition.Data.Areas;
using SquareExpedition.Data.Objects.Blocks;

namespace SquareExpedition.Application.Services;

public class TerrainGeneratorService : ITerrainGeneratorService
{
    public Area GenerateNewTerrain(Area area)
    {
        var areaWithTerrain = GenerateBlocks(area);

        return areaWithTerrain;
    }

    private Area GenerateBlocks(Area area)
    {
        var totalBlocks = (int)area.Size;

        var dimension = (int)Math.Sqrt(totalBlocks);

        var offset = dimension / 2;

        for (var x = 0; x < dimension; x++)
        for (var y = 0; y < 1; y++)
        for (var z = 0; z < dimension; z++)
        {
            var loc = area.GetLocalizationUsingCords(new Vector3(x-offset, y, z-offset));

            if (loc == null)
                throw new Exception("Localization not found for create terrain");

            var block = new Block()
            {
                Id = Guid.NewGuid(),
                Localization = loc,
                IsEditable = false
            };
            
            loc.SetGameObject(block);
        }

        return area;
    }
}