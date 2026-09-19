using Godot;
using VoxelTerra.Registries;
using VoxelTerra.Voxels;

namespace VoxelTerra.TerrainGeneration.Generators;

[GlobalClass]
public partial class TerrainFlat : TerrainGenerator
{
    [Export] private int terrainHeight = 5;

    protected override void generateFeatures(VoxelChunk chunk, Vector3I localPosition)
    {
    }

    protected override void generateLand(VoxelChunk chunk, Vector3I localPosition)
    {
        BlockRegistryItem dirtBlock = BlockRegistry.GetItem(VoxelTerra.Registries.Blocks.DirtBlock.ItemName);
        BlockRegistryItem grassBlock = BlockRegistry.GetItem(VoxelTerra.Registries.Blocks.GrassBlock.ItemName);

        for (int z = 0; z < 16; z++)
        {
            
            for (int x = 0; x < 16; x++)
            {
                for (int y = 0; y < terrainHeight; y++)
                {
                    if (y == terrainHeight - 1)
                    {
                        chunk.SetBlock(new Vector3I(x, y, z), grassBlock.ID, 0);
                        continue;
                    }
                    chunk.SetBlock(new Vector3I(x, y, z), dirtBlock.ID, 0);
                }
            }
        }
    }

    protected override void init()
    {
    }

}