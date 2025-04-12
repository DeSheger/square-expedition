using Microsoft.Xna.Framework;
using SquareExpedition.Data.Objects;

namespace SquareExpedition.Data.Interactions;

public class Localization
{
    public Localization(float x, float y, float z)
    {
        _centerPoint = new Vector3(x, y, z);
        Id = Guid.NewGuid();
    }

    public Guid Id { get; init; }

    private readonly Vector3 _centerPoint;

    private IGameObject? _gameObject;

    public Vector3 GetCoordinates() => _centerPoint;
    
    public IGameObject? GetGameObject() => _gameObject;

    public IGameObject? SetGameObject(IGameObject? gameObject)
    {
        if (!IsFreeSpace())
        {
            Console.WriteLine("Cannot change this object. The space is not empty");
            return null;   
        }
        
        if (!IsEditable())
        {
            Console.WriteLine("Cannot change this object. The space is not empty");
            return null;   
        }
        
        _gameObject = gameObject;

        return _gameObject;
    }

    public bool IsEditable() => _gameObject?.IsEditable ?? true;
    
    public bool IsFreeSpace() => _gameObject == null;
}