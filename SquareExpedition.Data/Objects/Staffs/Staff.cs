using Microsoft.Xna.Framework;
using SquareExpedition.Data.Forms;
using SquareExpedition.Data.Interactions;
using SquareExpedition.Data.Physics;

namespace SquareExpedition.Data.Objects.Staffs;

public abstract class Staff : IGameObject
{
    public Guid Id { get; set; }
    public Form? Form { get; set; }
    public bool IsEditable { get; init; }
    public Vector3[] Corners { get; set; } = [];
    public ICollection<Interaction> Interactions { get; set; } = [];
    public ICollection<Physic> Physics { get; set; } = [];
    public Localization? Localization { get; set; }
    public Vector3[] GetCorners() => Corners;
}