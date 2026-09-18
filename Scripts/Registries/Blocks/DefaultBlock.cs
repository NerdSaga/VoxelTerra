using Godot;

namespace VoxelTerra.Registries.Blocks;

public class DefaultBlock : BlockRegistryItem, IVTRegistryItem
{
    public static string ItemName => "default_block";

    public DefaultBlock() : base(ItemName)
    {
        RenderType = BlockRenderType.SOLID;
        CollisionType = BlockCollisionType.VOID;
    }
}