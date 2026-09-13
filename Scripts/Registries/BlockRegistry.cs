namespace VoxelTerra.Registries;

public partial class BlockRegistry : VTRegistry
{
    protected override void init()
    {
        prefix = "block";
        
        register("stone", new Blocks.Stone());
        register("grass_block", new Blocks.GrassBlock());
        register("sand", new Blocks.Sand());
        register("dirt_block", new Blocks.DirtBlock());
    }
}