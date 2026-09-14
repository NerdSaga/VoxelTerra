namespace VoxelTerra.Registries;

public partial class BlockRegistry : VTRegistry
{
    public static BlockRegistryItem GetItem(string blockName)
    {
        return instance.getItem(blockName) as BlockRegistryItem;
    }

    public static BlockRegistryItem GetItem(int blockID)
    {
        return instance.getItem(blockID) as BlockRegistryItem;
    }

    protected override void init()
    {
        PREFIX = "block";
        
        register(new Blocks.Stone("stone"));
        register(new Blocks.GrassBlock("grass_block"));
        register(new Blocks.Sand("sand_block"));
        register(new Blocks.DirtBlock("dirt_block"));
    }
}