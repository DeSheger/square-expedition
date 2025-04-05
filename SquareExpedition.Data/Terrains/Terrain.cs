using SquareExpedition.Data.Objects.Blocks;

namespace SquareExpedition.Data.Terrains;

public abstract class Terrain
{
    public Guid Id { get; set; }

    public long Randomizer { get; set; }

    public List<Block> Blocks { get; set; } = [];
}