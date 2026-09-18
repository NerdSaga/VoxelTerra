using Godot;
using VoxelTerra.Registries;
using VoxelTerra.Voxels;

namespace VoxelTerra.TerrainGeneration.Features;

public class Tree : TerrainFeature
{
    int mapleLog = BlockRegistry.GetItem(Registries.Blocks.MapleLog.ItemName).ID;
    public override void Build(VoxelChunk chunk, Vector3I localPosition)
    {
        if (isPositionInChunk(localPosition))
        {
            chunk.SetBlock(localPosition, mapleLog, 0);
        }
    }
}