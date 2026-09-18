using Godot;

namespace VoxelTerra.Registries.Blocks;

public class MapleLog : BlockRegistryItem, IVTRegistryItem
{
    public static string ItemName => "maple_log";

    public MapleLog() : base(ItemName)
    {
        RenderType = BlockRenderType.SOLID;
        CollisionType = BlockCollisionType.SOLID;
        SetVariants(new BlockVariant[]
        {
            new BlockVariantVoxel // Up facing
            {
                FaceAtlasTilePositions = new BlockVariantVoxel.BlockFaceAtlasTilePositions
                {
                    Top = new Vector2I(4, 0),
                    Bottom = new Vector2I(4, 0),
                    North = new Vector2I(5, 0),
                    South = new Vector2I(5, 0),
                    East = new Vector2I(5, 0),
                    West = new Vector2I(5, 0),
                }
            },

            new BlockVariantVoxel // North facing
            {
                FaceAtlasTilePositions = new BlockVariantVoxel.BlockFaceAtlasTilePositions
                {
                    Top = new Vector2I(5, 0),
                    Bottom = new Vector2I(5, 0),
                    North = new Vector2I(4, 0),
                    South = new Vector2I(4, 0),
                    East = new Vector2I(5, 0),
                    West = new Vector2I(5, 0),
                },
                FaceAtlasTileRotations = new BlockVariantVoxel.BlockFaceAtlasTileRotations
                {
                    East = Common.Rotations.R270,
                    West = Common.Rotations.R90,
                }
            },

            new BlockVariantVoxel // East facing
            {
                FaceAtlasTilePositions = new BlockVariantVoxel.BlockFaceAtlasTilePositions
                {
                    Top = new Vector2I(5, 0),
                    Bottom = new Vector2I(5, 0),
                    North = new Vector2I(5, 0),
                    South = new Vector2I(5, 0),
                    East = new Vector2I(4, 0),
                    West = new Vector2I(4, 0),
                },

                FaceAtlasTileRotations = new BlockVariantVoxel.BlockFaceAtlasTileRotations
                {
                    Top = Common.Rotations.R270,
                    Bottom = Common.Rotations.R90,
                    North = Common.Rotations.R90,
                    South = Common.Rotations.R270,
                }
            }
        });
    }
}