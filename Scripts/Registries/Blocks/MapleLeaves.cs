using Godot;

namespace VoxelTerra.Registries.Blocks;

class MapleLeaves : BlockRegistryItem, IVTRegistryItem
{
    public static string ItemName => "maple_leaves";

    public MapleLeaves() : base(ItemName)
    {
        SurfaceID = 1;
        RenderType = BlockRenderType.ALL_FACES;
        CollisionType = BlockCollisionType.SOLID;
        SetVariants(new BlockVariant[]
        {
            new BlockVariant
            {
                FaceAtlasTilePositions = new BlockVariant.BlockFaceAtlasTilePositions
                {
                    Top = new Vector2I(6, 0),
                    Bottom = new Vector2I(6, 0),
                    North = new Vector2I(6, 0),
                    South = new Vector2I(6, 0),
                    East = new Vector2I(6, 0),
                    West = new Vector2I(6, 0),
                }
            }
        });
    }
}