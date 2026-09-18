using Godot;
using VoxelTerra.Voxels;

namespace VoxelTerra.TerrainGeneration.Features;

public abstract class TerrainFeature
{
    public abstract void Build(VoxelChunk chunk, Vector3I localPosition);

    /// <summary>
    /// Returns true if a local position is inside of a chunk. Otherwise returns false.
    /// </summary>
    /// <param name="localPosition"></param>
    /// <returns></returns>
    protected bool isPositionInChunk(Vector3I localPosition)
    {
        if (localPosition.X < 0 || localPosition.X >= 16)
        {
            return false;
        }

        if (localPosition.Y < 0 || localPosition.Y >= 256)
        {
            return false;
        }

        if (localPosition.Z < 0 || localPosition.Z >= 16)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Sets a block in a chunk. This function will return without setting a block if the local position is outside of the chunk.
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <param name="blockID"></param>
    /// <param name="blockVariant"></param>
    protected void setBlock(VoxelChunk chunk, Vector3I localPosition, int blockID, int blockVariant)
    {
        if (!isPositionInChunk(localPosition))
        {
            return;
        }

        chunk.SetBlock(localPosition, blockID, blockVariant);
    }

    protected void fillBlocks(VoxelChunk chunk, Vector3I fromLocalPosition, Vector3I toLocalPosition, int blockID, int blockVariant)
    {
        for (int y = fromLocalPosition.Y; y <= toLocalPosition.Y; y++)
        {
            for (int z = fromLocalPosition.Z; z <= toLocalPosition.Z; z++)
            {
                for (int x = fromLocalPosition.X; x <= toLocalPosition.X; x++)
                {
                    setBlock(chunk, new Vector3I(x, y, z), blockID, blockVariant);
                }
            }
        }
    }
}