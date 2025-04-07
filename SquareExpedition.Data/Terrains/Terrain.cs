using SquareExpedition.Data.Objects.Blocks;

namespace SquareExpedition.Data.Terrains;

public class Terrain
{
    public Guid Id { get; set; }

    public long Randomizer { get; set; }

    public TerrainSize Size { get; set; }

    public ICollection<Block> Blocks { get; set; } = [];
}