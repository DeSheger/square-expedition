using SquareExpedition.Data.Objects.Characters;
using SquareExpedition.Data.Terrains;

namespace SquareExpedition.Data.World;

/// <summary>
/// Represent instance of single game, that can be shared using Server to multiplayer
/// </summary>
public abstract class World
{
    public Guid Id { get; set; }

    public Terrain? Terrain { get; set; }

    public List<Character> Players { get; set; } = [];
    
    public List<Character> Mobs { get; set; } = [];
    
    public string? HostName { get; set; }
    
    public string? HostAddress { get; set; }
}