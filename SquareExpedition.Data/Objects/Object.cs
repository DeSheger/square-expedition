using SquareExpedition.Data.Forms;
using SquareExpedition.Data.Interactions;
using SquareExpedition.Data.Physics;

namespace SquareExpedition.Data.Objects;

public abstract class Object
{
    public Guid Id { get; set; }
    
    public Form? Form { get; set; }

    public List<Interaction> Interactions { get; set; } = [];
    
    public List<Physic> Physics { get; set; } = [];

    public Localization? Localization { get; set; }
}