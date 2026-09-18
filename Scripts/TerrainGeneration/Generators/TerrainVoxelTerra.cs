using Godot;
using VoxelTerra.Registries;
using VoxelTerra.Voxels;

namespace VoxelTerra.TerrainGeneration.Generators;

[GlobalClass]
public partial class TerrainVoxelTerra : TerrainGenerator
{
    private static class BlockIDs
    {
        public static int DirtBlock = BlockRegistry.GetItem(VoxelTerra.Registries.Blocks.DirtBlock.ItemName).ID;
        public static int GrassBlock = BlockRegistry.GetItem(VoxelTerra.Registries.Blocks.GrassBlock.ItemName).ID;
    }
    protected override void generateFeatures(VoxelChunk chunk, Vector3I localPosition)
    {
        Vector3I worldPosition = chunk.WorldPosition + localPosition;
        int terrainHeight = (int)(sampleTerrainHeight(new Vector2I(worldPosition.X, worldPosition.Z)) * 50);

        Features.Tree tree = new();
        if (sampleRandom2D(new Vector2I(worldPosition.X, worldPosition.Z)) > 0.9)
        {
            tree.Build(chunk, localPosition + new Vector3I(0, terrainHeight, 0));
        }
    }

    protected override void generateLand(VoxelChunk chunk, Vector3I localPosition)
    {
        Vector3I worldPosition = chunk.WorldPosition + localPosition;
        int terrainHeight = (int)(sampleTerrainHeight(new Vector2I(worldPosition.X, worldPosition.Z)) * 50);

        for (int y = 0; y < terrainHeight; y++)
        {
            Vector3I blockPosition = localPosition + new Vector3I(0, y, 0);
            if (y == terrainHeight - 1)
            {
                chunk.SetBlock(blockPosition, BlockIDs.GrassBlock, 0);
                continue;
            }
            chunk.SetBlock(blockPosition, BlockIDs.DirtBlock, 0);
        }
    }


    private FastNoiseLite[] terrainHeightLayers =
    {
        new FastNoiseLite
        {
            NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex,
        }
    };

    public float sampleTerrainHeight(Vector2I position)
    {
        return (terrainHeightLayers[0].GetNoise2D(position.X, position.Y) + 1) / 2;
    }

    protected override void init()
    {
        foreach (FastNoiseLite layer in terrainHeightLayers)
        {
            layer.Seed = Seed;
        }
    }
}