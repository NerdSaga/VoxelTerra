namespace VoxelTerra.Registries.Blocks;

public class DirtBlock : BlockRegistryItem, IVTRegistryItem
{
    public static string ItemName => "dirt_block";

    public DirtBlock() : base(ItemName)
    {
        
    }
}