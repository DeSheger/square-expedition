using SquareExpedition.Data.Terrains;
using SquareExpedition.Data.World;

namespace SquareExpedition.Application.Services;

public class WorldGeneratorService
{
    public World GenerateNewWorld(string name, TerrainSize size)
    {
        var world = new World()
        {
            Id = Guid.NewGuid(),
            Area = new AreaGeneratorService().GenerateNewArea(size),
            WorldName = name
        };

        world.Area = new TerrainGeneratorService().GenerateNewTerrain(world.Area);
        return world;
    }
}