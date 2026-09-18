using Godot;

namespace VoxelTerra.Registries.Blocks;

public class DirtBlock : BlockRegistryItem, IVTRegistryItem
{
    public static string ItemName => "dirt_block";

    public DirtBlock() : base(ItemName)
    {
        RenderType = BlockRenderType.SOLID;
        CollisionType = BlockCollisionType.SOLID;
        SetVariants(new BlockVariant[]
        {
            new BlockVariant
            {
                FaceAtlasTilePositions = new BlockVariant.BlockFaceAtlasTilePositions
                {
                    Top = new Vector2I(1, 0),
                    Bottom = new Vector2I(1, 0),
                    North = new Vector2I(1, 0),
                    South = new Vector2I(1, 0),
                    East = new Vector2I(1, 0),
                    West = new Vector2I(1, 0),
                }
            }
        });
    }
}