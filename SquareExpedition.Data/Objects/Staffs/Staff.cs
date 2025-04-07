using SquareExpedition.Data.Forms;
using SquareExpedition.Data.Interactions;
using SquareExpedition.Data.Physics;

namespace SquareExpedition.Data.Objects.Staffs;

public abstract class Staff : IGameObject
{
    public Guid Id { get; set; }
    public Form? Form { get; set; }
    public List<Interaction> Interactions { get; set; } = new();
    public List<Physic> Physics { get; set; } = new();
    public Localization? Localization { get; set; }
}