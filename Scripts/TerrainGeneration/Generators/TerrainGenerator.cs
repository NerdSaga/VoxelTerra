using Godot;
using VoxelTerra.Voxels;
namespace VoxelTerra.TerrainGeneration.Generators;

[GlobalClass]
public abstract partial class TerrainGenerator : Resource
{
    [Export] public int Seed { get; set; } = 0;
    protected abstract void generateLand(VoxelChunk chunk, Vector3I localPosition);
    protected abstract void generateFeatures(VoxelChunk chunk, Vector3I localPosition);
    protected abstract void init();

    /// <summary>
    /// Generates the terrain for a chunk.
    /// </summary>
    /// <param name="chunk"></param>
    public void Generate(VoxelChunk chunk)
    {
        for (int z = 0; z < 16; z++)
        {
            for (int x = 0; x < 16; x++)
            {
                generateLand(chunk, new Vector3I(x, 0, z));
            }
        }

        for (int z = -16; z < 32; z++)
        {
            for (int x = -16; x < 32; x++)
            {
                generateFeatures(chunk, new Vector3I(x, 0, z));
            }
        }
    }
    /// <summary>
    /// This function takes the place of the class constructor.
    /// This is needed because the terrain seed cannot be initialised
    /// in ths class constructor.
    /// </summary>
    public void Init()
    {
        randomNoise.Seed = Seed;
        init();
    }
    
    private FastNoiseLite randomNoise = new FastNoiseLite
    {
        NoiseType = FastNoiseLite.NoiseTypeEnum.Cellular,
        Frequency = 1,
        FractalOctaves = 1,
        CellularJitter = 0,
        CellularReturnType = FastNoiseLite.CellularReturnTypeEnum.CellValue,    
    };

    /// <summary>
    /// Gets a noise value between 0 and 1 at a position.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    protected float sampleRandom2D(Vector2I position)
    {
        float value = (randomNoise.GetNoise2D(position.X, position.Y) + 1) / 2;
        return value;
    }

    /// <summary>
    /// Converts a local position within a chunk to a world position.
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <returns></returns>
    protected Vector3I toWorldPosition(Vector3I localPosition, VoxelChunk chunk)
    {
        Vector3I worldPosition = chunk.WorldPosition + localPosition;
        return worldPosition;
    }
}