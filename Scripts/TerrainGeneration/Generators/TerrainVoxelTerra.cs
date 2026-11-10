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
        public static int Stone = BlockRegistry.GetItem(VoxelTerra.Registries.Blocks.Stone.ItemName).ID;
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

        // Fill water.
        // for (int y = terrainHeight; y < SeaLevel; y++)
        // {
        //     Vector3I blockPosition = localPosition + new Vector3I(0, y, 0);
        //     chunk.SetBlock(blockPosition, BlockIDs.WaterBlock, 0);
        // }

        // Fill stone.
        for (int y = 0; y < terrainHeight; y++)
        {
            Vector3I blockPosition = localPosition + new Vector3I(0, y, 0);
            chunk.SetBlock(blockPosition, BlockIDs.Stone, 0);
        }
    }
    protected override void generateFeatures(VoxelChunk chunk, Vector3I localPosition)
    {
        // Vector3I worldPosition = toWorldPosition(localPosition, chunk);
        // int terrainHeight = (int)(sampleTerrainHeight(new Vector2I(worldPosition.X, worldPosition.Z)) * 50);

        // distributeTrees(chunk, localPosition);
    }

    protected override void init()
    {
        heightLayerMountain.Seed = Seed;
    }

    private FastNoiseLite heightLayerMountain = new()
    {
        NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex,
    };

    private FastNoiseLite heightLayerValley = new()
    {
        NoiseType = FastNoiseLite.NoiseTypeEnum.SimplexSmooth,
    };

    private float sampleTerrainHeight(Vector2I position)
    {
        float noiseMountain = ((heightLayerMountain.GetNoise2D(position.X, position.Y) + 1) * 0.5f) * 80;
        float noiseValley = ((heightLayerValley.GetNoise2D(position.X, position.Y) + 1) * 0.5f) * 6;
        // Use mix to mask between mountains and valleys...
        float mix = 1;
        return (noiseValley * mix) + (noiseMountain * ((mix * -1) + 1));
    }

    private void distributeTrees(VoxelChunk chunk, Vector3I localPosition)
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