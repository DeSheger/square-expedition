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
    
    private IEnumerable<Block> GenerateBlocks(Terrain terrain)
    {
        int totalBlocks = (int)terrain.Size;
        
        int dimension = (int)Math.Sqrt(totalBlocks);
        
        int offset = dimension / 4;

        for (int x = 0; x < dimension; x++)
        {
            for (int z = 0; z < dimension; z++)
            {
                var minPos = new Vector3(x - offset, 0, z - offset);
                
                var maxPos = minPos + new Vector3(BlockProperties.DefaultBlockSize, BlockProperties.DefaultBlockSize, BlockProperties.DefaultBlockSize);

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
    }
    
    private Terrain GetPrimaryTerrainInfo(TerrainSize size)
        => new Terrain
        {
            Id = new Guid(),
            Size = size,
            Randomizer = new Random().Next(1, Int32.MaxValue)
        };
}