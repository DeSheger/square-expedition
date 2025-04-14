using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SquareExpedition.Data.Areas;

namespace SquareExpedition.Application.Abstract;

public interface ITerrainGeneratorService
{
    Area GenerateNewTerrain(Area area, Game game, BasicEffect effect, GraphicsDevice graphicsDevice);
}