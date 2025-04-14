using Microsoft.Xna.Framework;
using SquareExpedition.Data.Forms;
using SquareExpedition.Data.Interactions;
using SquareExpedition.Data.Physics;

namespace SquareExpedition.Data.Objects.Characters;

public abstract class Character : GameObject
{
    protected Character(Game game) : base(game)
    {
    }
}