using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SquareExpedition.Data.Forms;
using SquareExpedition.Data.Interactions;
using SquareExpedition.Data.Physics;

namespace SquareExpedition.Data.Objects.Characters;

public abstract class Character : GameObject
{
    protected Character(Game game, BasicEffect basicEffect, Matrix projectionMatrix, Matrix viewMatrix, Matrix worldMatrix) : base(game, basicEffect, projectionMatrix, viewMatrix, worldMatrix)
    {
    }
}