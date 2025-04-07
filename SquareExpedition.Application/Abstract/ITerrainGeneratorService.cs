using SquareExpedition.Data.Terrains;

namespace SquareExpedition.Application.Abstract;

public interface ITerrainGeneratorService
{
    Terrain GenerateNewTerrain(TerrainSize size);
}