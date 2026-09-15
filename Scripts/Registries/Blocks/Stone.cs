

using Godot;

namespace VoxelTerra.Registries.Blocks;

public class Stone : BlockRegistryItem, IVTRegistryItem
{

    public static string ItemName { get => "stone_block"; }

    public Stone() : base(ItemName)
    {
        var a = new BlockVariant();
        
        SetVariants(new BlockVariant[]
        {
            new BlockVariant()
            {
                FaceAtlasTilePositions = new BlockVariant.BlockFaceAtlasTilePositions()
                {
                    Top = new Vector2I(0, 0),
                    Bottom = new Vector2I(0, 0),
                    North = new Vector2I(0, 0),
                    South = new Vector2I(0, 0),
                    East = new Vector2I(0, 0),
                    West = new Vector2I(0, 0),
                },
                RenderType = BlockVariant.BlockRenderType.SOLID,
                CollisionType = BlockVariant.BlockCollisionType.SOLID
            }
        });
    }
}