using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SquareExpedition.Application.Abstract;
using SquareExpedition.Data.Areas;
using SquareExpedition.Data.Objects.Blocks;
using Vector3 = System.Numerics.Vector3;

namespace SquareExpedition.Application.Services;

public class TerrainGeneratorService
{
    public Area GenerateNewTerrain(Area area, Game game, BasicEffect effect, Matrix projectionMatrix, Matrix viewMatrix, Matrix worldMatrix)
    {
        var areaWithTerrain = GenerateBlocks(area, game, effect, projectionMatrix, viewMatrix, worldMatrix);

        return areaWithTerrain;
    }

    private Area GenerateBlocks(Area area, Game game, BasicEffect effect, Matrix projectionMatrix, Matrix viewMatrix, Matrix worldMatrix)
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

            var block = new Block(game, effect, projectionMatrix, viewMatrix, worldMatrix, loc, false, Color.Black);
            
            loc.SetGameObject(block);
        }

        return area;
    }
}