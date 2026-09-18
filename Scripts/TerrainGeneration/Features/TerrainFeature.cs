using Godot;
using VoxelTerra.Voxels;

namespace VoxelTerra.TerrainGeneration.Features;

public abstract class TerrainFeature
{
    public abstract void Build(VoxelChunk chunk, Vector3I localPosition);

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
}