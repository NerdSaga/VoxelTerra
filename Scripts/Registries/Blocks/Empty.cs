namespace VoxelTerra.Registries.Blocks;

class Empty : BlockRegistryItem, IVTRegistryItem
{
    public static string ItemName => ".empty";

    public Empty() : base(ItemName)
    {
        
    }
}