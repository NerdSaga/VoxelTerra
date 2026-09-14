

using Godot;

namespace VoxelTerra.Registries.Blocks;

public class Stone : BlockRegistryItem
{
    
    public Stone(string name) : base(name)
    {
        // SetVariants(new BlockVariant[]
        // {
        //     new BlockVariant()
        //         .SetFaceAtlasTilePositions(Vector2I.Zero, Vector2I.Zero, Vector2I.Zero, Vector2I.Zero, Vector2I.Zero, Vector2I.Zero)
        // });
    }
}