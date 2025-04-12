using Microsoft.Xna.Framework;
using SquareExpedition.Data.Interactions;
using SquareExpedition.Data.Physics;

namespace SquareExpedition.Data.Objects;

public interface IGameObject
{
    public Guid Id { get; set; }
    
    public bool IsEditable { get; init; }

    public Vector3[] Corners { get; set; }

    public ICollection<Interaction> Interactions { get; set; }

    public ICollection<Physic> Physics { get; set; }

    public Localization? Localization { get; set; }

    public Vector3[] GetCorners();
}