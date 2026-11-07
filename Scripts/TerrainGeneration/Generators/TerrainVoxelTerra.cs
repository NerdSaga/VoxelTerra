using Godot;
using VoxelTerra.Registries;
using VoxelTerra.Voxels;

namespace VoxelTerra.TerrainGeneration.Generators;

[GlobalClass]
public partial class TerrainVoxelTerra : TerrainGenerator
{
    public static int SeaLevel = 50;

    /// <summary>
    /// A collection of blocks used for generating terrain.
    /// </summary>
    private static class BlockIDs
    {
        public static int DirtBlock = BlockRegistry.GetItem(VoxelTerra.Registries.Blocks.DirtBlock.ItemName).ID;
        public static int GrassBlock = BlockRegistry.GetItem(VoxelTerra.Registries.Blocks.GrassBlock.ItemName).ID;
        public static int WaterBlock = BlockRegistry.GetItem(VoxelTerra.Registries.Blocks.WaterBlock.ItemName).ID;
    }

    /// <summary>
    /// Generates the height terrain.
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    protected override void generateLand(VoxelChunk chunk, Vector3I localPosition)
    {
        Vector3I worldPosition = toWorldPosition(localPosition, chunk);
        int terrainHeight = (int)sampleTerrainHeight(new Vector2I(worldPosition.X, worldPosition.Z));
        for (int y = terrainHeight; y < SeaLevel; y++)
        {
            Vector3I blockPosition = localPosition + new Vector3I(0, y, 0);
            chunk.SetBlock(blockPosition, BlockIDs.WaterBlock, 0);
        }

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
        float noise = terrainHeightLayers[0].GetNoise2D(position.X, position.Y) * 30;
        return noise + SeaLevel;
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