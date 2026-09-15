using System;
using Godot;
using VoxelTerra.Debugging;

namespace VoxelTerra.Registries;


public abstract class BlockRegistryItem : VTRegistryItem
{
    private BlockVariant[] variants = new BlockVariant[] {new BlockVariant()};


    public void SetVariants(BlockVariant[] blockVariants)
    {
        variants = blockVariants;
    }

    public BlockVariant GetVariant(int variantIndex)
    {
        return variants[variantIndex];
    }

    public BlockRegistryItem(string itemName) : base(itemName)
    {
    }
}