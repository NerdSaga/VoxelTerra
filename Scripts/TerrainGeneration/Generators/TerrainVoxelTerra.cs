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
        // heightLayerMountain.Seed = Seed;
        // heightLayerValley.Seed = Seed;
        // layerMix.Seed = Seed;
        mountainNoise.Seed = Seed;
    }

    // private FastNoiseLite heightLayerMountain = new()
    // {
    //     NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex,
    // };

    // private FastNoiseLite heightLayerValley = new()
    // {
    //     NoiseType = FastNoiseLite.NoiseTypeEnum.SimplexSmooth,
    // };

    // private FastNoiseLite layerMix = new()
    // {
    //     NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin,
    //     FractalOctaves = 1,
    //     Frequency = 0.01f,
    // };

    private FastNoiseLite mountainNoise = GD.Load<FastNoiseLite>("uid://ds02fuyuyl3qq");
    private FastNoiseLite valleyNoise = GD.Load<FastNoiseLite>("uid://bx238e5j32q5q");


    private float sampleTerrainHeight(Vector2I position)
    {


        // float mountainHeight = mountainNoise.GetNoise2D(position.X, position.Y) - 0.3f;
        // if ( mountainHeight < 0)
        // {
        //     mountainHeight = 0;
        // }

        // float mountainHeightMask = (mountainHeight - 0.5f) * -1 + 0.5f;
        float mountainSpread = 1;

        // Create mountainHeight and mountain mask.
        float mountainHeight = (mountainNoise.GetNoise2D(position.X, position.Y) - 0.5f) * mountainSpread + 0.5f;
        if (mountainHeight < 0)
        {
            mountainHeight = 0;
        }

        float mountainMask = mountainHeight;
        if (mountainMask > 0)
        {
            mountainMask = 1;
        }

        mountainHeight = mountainHeight * 30;




        // Create valley height.
        float valleyHeight = valleyNoise.GetNoise2D(position.X, position.Y) * 5;

        float worldHeight = 10 + mountainHeight + ((mountainMask * -1 + 1) * valleyHeight);

        return worldHeight;

        return valleyHeight * 10;

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