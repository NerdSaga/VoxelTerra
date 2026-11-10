using Godot;

namespace VoxelTerra.Registries.Blocks;

public class SandBlock : BlockRegistryItem, IVTRegistryItem
{
    public static string ItemName => "sand_block";

    public SandBlock() : base(ItemName)
    {
        RenderType = BlockRenderType.SOLID;
        CollisionType = BlockCollisionType.SOLID;
        SetVariants(new BlockVariant[]
        {
            new BlockVariantVoxel
            {
                FaceAtlasTilePositions = new BlockVariantVoxel.BlockFaceAtlasTilePositions
                {
                    Top = new Vector2I(9, 0),
                    Bottom = new Vector2I(9, 0),
                    North = new Vector2I(9, 0),
                    South = new Vector2I(9, 0),
                    East = new Vector2I(9, 0),
                    West = new Vector2I(9, 0),
                }
            }
        });
    }
}