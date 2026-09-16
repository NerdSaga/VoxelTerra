namespace VoxelTerra.Registries;

public partial class BlockRegistry : VTRegistry
{
    protected override void init()
    {
        PREFIX = "block";
        
        register(new Blocks.Empty());
        register(new Blocks.DefaultBlock());
        register(new Blocks.Stone());
        register(new Blocks.GrassBlock());
        register(new Blocks.Sand());
        register(new Blocks.DirtBlock());
        register(new Blocks.MapleLog());
    }

    public static BlockRegistryItem GetItem(string blockName)
    {
        return instance.getItem(blockName) as BlockRegistryItem;
    }

    public static BlockRegistryItem GetItem(int blockID)
    {
        return instance.getItem(blockID) as BlockRegistryItem;
    }

}