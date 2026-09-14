using System;

namespace VoxelTerra.Registries;

public class BlockRegistryItem : VTRegistryItem
{
    private BlockVariant[] variants = new BlockVariant[] {new BlockVariant()};
    
    public void SetVariants(BlockVariant[] blockVariants)
    {
        variants = blockVariants;
    }

    public BlockRegistryItem(string name) : base(name) {}

}