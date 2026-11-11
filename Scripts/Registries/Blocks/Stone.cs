

using Godot;

namespace VoxelTerra.Registries.Blocks;

public class Stone : BlockRegistryItem, IVTRegistryItem
{

    public static string ItemName { get => "stone_block"; }

    public Stone() : base(ItemName)
    {
        RenderType = BlockRenderType.SOLID;
        CollisionType = BlockCollisionType.SOLID;

        SetVariants(new BlockVariant[]
        {
            new BlockVariantVoxel()
            {
                FaceAtlasTilePositions = new BlockVariantVoxel.BlockFaceAtlasTilePositions()
                {
                    Top = new Vector2I(8, 0),
                    Bottom = new Vector2I(8, 0),
                    North = new Vector2I(8, 0),
                    South = new Vector2I(8, 0),
                    East = new Vector2I(8, 0),
                    West = new Vector2I(8, 0),
                }
            }
        });
    }
}