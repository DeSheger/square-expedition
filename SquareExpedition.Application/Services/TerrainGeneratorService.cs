using Common.Constants;
using Microsoft.Xna.Framework;
using SquareExpedition.Application.Abstract;
using SquareExpedition.Data.Interactions;
using SquareExpedition.Data.Objects.Blocks;
using SquareExpedition.Data.Physics;
using SquareExpedition.Data.Terrains;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace SquareExpedition.Application.Services;

public class TerrainGeneratorService : ITerrainGeneratorService
{
    public Terrain GenerateNewTerrain(TerrainSize size)
    {
        var terrain = GetPrimaryTerrainInfo(size);

        terrain.Blocks = GenerateBlocks(terrain).ToList();

        return terrain;
    }

    public Terrain GetTerrainInfo()
    {
        throw new NotImplementedException();
    }

    private IEnumerable<Block> GenerateBlocks(Terrain terrain)
    {
        var totalBlocks = (int)terrain.Size;

        var dimension = (int)Math.Sqrt(totalBlocks);

        var offset = dimension / 2;

        for (var x = 0; x < dimension; x++)
        for (var z = 0; z < dimension; z++)
        {
            var minPos = new Vector3(x - offset, 0, z - offset);

            var maxPos = minPos + new Vector3(BlockProperties.DefaultBlockSize, BlockProperties.DefaultBlockSize,
                BlockProperties.DefaultBlockSize);

            yield return new Block
            {
                Id = Guid.NewGuid(),
                BlockSize = BlockProperties.DefaultBlockSize,
                FormImplementation = new BoundingBox(minPos, maxPos),
                Form = null,
                Interactions = new List<Interaction>(),
                Physics = new List<Physic>()
            };
        }
    }

    private Terrain GetPrimaryTerrainInfo(TerrainSize size)
    {
        return new Terrain
        {
            Id = new Guid(),
            Size = size,
            Randomizer = new Random().Next(1, int.MaxValue)
        };
    }
}