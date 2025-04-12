using Common.Constants;
using Microsoft.Xna.Framework;
using SquareExpedition.Data.Interactions;
using SquareExpedition.Data.Physics;

namespace SquareExpedition.Data.Objects.Blocks;

public class Block : IGameObject
{
    public Guid Id { get; set; }
    public bool IsEditable { get; init; }
    public Vector3[] Corners { get; set; } = [];
    public ICollection<Interaction> Interactions { get; set; } = [];
    public ICollection<Physic> Physics { get; set; } = [];
    
    
    private Localization? _localization;
    public Localization? Localization
    {
        get => _localization;
        set
        {
            _localization = value;
            UpdateFormImplementation();
        }
    }

    public Vector3[] GetCorners() => Corners.ToArray();

    public BoundingBox? FormImplementation { get; private set;}
    private void UpdateFormImplementation()
    {
        if (Localization != null)
        {
            // TODO: handler for allow to change localization based on terrain info and space grid 
            
            var blockRadiusLength = BlockProperties.DefaultBlockSize / 2;
            var minPos = new Vector3(
                Localization.GetCoordinates().X - blockRadiusLength,
                Localization.GetCoordinates().Y - blockRadiusLength,
                Localization.GetCoordinates().Z - blockRadiusLength);

            var maxPos = minPos + new Vector3(
                BlockProperties.DefaultBlockSize, 
                BlockProperties.DefaultBlockSize,
                BlockProperties.DefaultBlockSize);
           
            FormImplementation = new BoundingBox(minPos, maxPos);

            Corners = FormImplementation.Value.GetCorners();
        }
        else
        { 
            FormImplementation = null;
            Corners = [];
        }
    }
}