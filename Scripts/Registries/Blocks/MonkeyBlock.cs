using VoxelTerra.Voxels;

namespace VoxelTerra.Registries.Blocks;

public class MonkeyBlock : BlockRegistryItem, IVTRegistryItem
{
    private MeshArrays meshArrays = MeshArrays.Load("uid://btiveuky45x6w");
    public static string ItemName => "monkey_block";

    public MonkeyBlock() : base(ItemName)
    {
        SetVariants(new BlockVariant[]
        {
            new BlockVariantMesh
            {
                MeshArrays = meshArrays,
            }
        });
    }
}