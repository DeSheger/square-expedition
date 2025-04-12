using SquareExpedition.Data.Areas;
using SquareExpedition.Data.Terrains;

namespace SquareExpedition.Application.Abstract;

public interface ITerrainGeneratorService
{
    Area GenerateNewTerrain(Area area);
}