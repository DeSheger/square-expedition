using Microsoft.Xna.Framework;
using SquareExpedition.Data.Interactions;
using SquareExpedition.Data.Physics;

namespace SquareExpedition.Data.Objects;

public class GameObject : DrawableGameComponent
{
    public GameObject(Game game) : base(game) {}

    public Guid? Id { get; set; }

    public bool IsEditable { get; init; } = true;

    public Vector3[]? Corners { get; set; } = [];

    public ICollection<Interaction> Interactions { get; set; } = [];

    public ICollection<Physic> Physics { get; set; } = [];

    public Localization? Localization { get; set; }

    public Vector3[]? GetCorners() => Corners?.ToArray();
}