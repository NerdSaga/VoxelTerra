
using Godot;

namespace VoxelTerra.Registries.Blocks;

public class GrassBlock : BlockRegistryItem, IVTRegistryItem
{
    public static string ItemName => "grass_block";

    public GrassBlock() : base(ItemName)
    {

        RenderType = BlockRenderType.SOLID;
        CollisionType = BlockCollisionType.SOLID;
        SetVariants(
            new BlockVariant[]
            {
                new BlockVariant()
                {
                    FaceAtlasTilePositions = new BlockVariant.BlockFaceAtlasTilePositions
                    {
                        Top = new Vector2I(2, 0),
                        Bottom = new Vector2I(1, 0),
                        North = new Vector2I(3, 0),
                        South = new Vector2I(3, 0),
                        East = new Vector2I(3, 0),
                        West = new Vector2I(3, 0),
                    },
                },
            }
        );
    }
}