

using Godot;

namespace VoxelTerra.Registries.Blocks;

public class Stone : BlockRegistryItem, IVTRegistryItem
{

    public static string ItemName { get => "stone_block"; }

    public Stone() : base(ItemName)
    {
    }
}