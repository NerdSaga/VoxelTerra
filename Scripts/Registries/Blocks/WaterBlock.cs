using Godot;

namespace VoxelTerra.Registries.Blocks;

public class WaterBlock : BlockRegistryItem, IVTRegistryItem
{
    public static string ItemName => "water_block";

    public WaterBlock() : base(ItemName)
    {
        RenderType = BlockRenderType.TRANSPARENT;
        CollisionType = BlockCollisionType.VOID;
        SurfaceID = 2;
        SetVariants(new BlockVariant[]
        {
            new BlockVariantVoxel
            {
                FaceAtlasTilePositions = new BlockVariantVoxel.BlockFaceAtlasTilePositions
                {
                    Top = new Vector2I(7, 0),
                    Bottom = new Vector2I(7, 0),
                    North = new Vector2I(7, 0),
                    South = new Vector2I(7, 0),
                    East = new Vector2I(7, 0),
                    West = new Vector2I(7, 0),
                },

                FaceAtlasTileRotations = new BlockVariantVoxel.BlockFaceAtlasTileRotations(),
            }
        });
    }
}