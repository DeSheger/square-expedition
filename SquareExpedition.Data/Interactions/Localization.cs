using Microsoft.Xna.Framework;

namespace SquareExpedition.Data.Interactions;

public class Localization
{
    public Localization(float x, float y, float z)
    {
        CenterPoint = new Vector3(x, y, z);
    }
    
    public Vector3 CenterPoint { get; set; }
}