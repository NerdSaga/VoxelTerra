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
    protected override void generateLand(VoxelChunk chunk, Vector3I localPosition)
    {
        Vector3I worldPosition = toWorldPosition(localPosition, chunk);
        int terrainHeight = (int)sampleTerrainHeight(new Vector2I(worldPosition.X, worldPosition.Z));

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
    protected override void generateFeatures(VoxelChunk chunk, Vector3I localPosition)
    {
        Vector3I worldPosition = toWorldPosition(localPosition, chunk);
        int terrainHeight = (int)(sampleTerrainHeight(new Vector2I(worldPosition.X, worldPosition.Z)) * 50);

        distributeTree(chunk, localPosition);
    }
    protected override void init()
    {
        foreach (FastNoiseLite layer in terrainHeightLayers)
        {
            layer.Seed = Seed;
        }
    }

    private FastNoiseLite[] terrainHeightLayers =
    {
        new FastNoiseLite
        {
            NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex,
        }
    };

    private float sampleTerrainHeight(Vector2I position)
    {
        float noise = (terrainHeightLayers[0].GetNoise2D(position.X, position.Y) + 1) / 2;
        return noise * 50;
    }

    private void distributeTree(VoxelChunk chunk, Vector3I localPosition)
    {
        Vector3I worldPosition = toWorldPosition(localPosition, chunk);
        int terrainHeight = (int)sampleTerrainHeight(new Vector2I(worldPosition.X, worldPosition.Z));

        Features.Tree tree = new();
        if (sampleRandom2D(new Vector2I(worldPosition.X, worldPosition.Z)) < 0.01)
        {
            tree.Build(chunk, localPosition + new Vector3I(0, terrainHeight, 0));
        }
    }


}