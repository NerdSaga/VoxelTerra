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
            new BlockVariant // Up facing
            {
                FaceAtlasTilePositions = new BlockVariant.BlockFaceAtlasTilePositions
                {
                    Top = new Vector2I(4, 0),
                    Bottom = new Vector2I(4, 0),
                    North = new Vector2I(5, 0),
                    South = new Vector2I(5, 0),
                    East = new Vector2I(5, 0),
                    West = new Vector2I(5, 0),
                }
            },

            new BlockVariant // North facing
            {
                FaceAtlasTilePositions = new BlockVariant.BlockFaceAtlasTilePositions
                {
                    Top = new Vector2I(5, 0),
                    Bottom = new Vector2I(5, 0),
                    North = new Vector2I(4, 0),
                    South = new Vector2I(4, 0),
                    East = new Vector2I(5, 0),
                    West = new Vector2I(5, 0),
                }
            },

            new BlockVariant // East facing
            {
                FaceAtlasTilePositions = new BlockVariant.BlockFaceAtlasTilePositions
                {
                    Top = new Vector2I(5, 0),
                    Bottom = new Vector2I(5, 0),
                    North = new Vector2I(5, 0),
                    South = new Vector2I(5, 0),
                    East = new Vector2I(4, 0),
                    West = new Vector2I(4, 0),
                }
            }
        });
    }
}