
using System;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using VoxelTerra.TerrainGeneration;

namespace VoxelTerra.WorkerThreads;

public partial class BlockPlacer : Node
{

    public static BlockPlacer Instance;

    public static async Task SetBlockLocal(VoxelChunk chunk, Vector3I position, ushort block)
    {
        int chunkBlockIndex = position.X;
        chunkBlockIndex += position.Y * 256;
        chunkBlockIndex += position.Z * 16;

        chunk.Blocks[chunkBlockIndex] = block;

        await Instance.regenChunk(chunk);
    }

    private async Task regenChunk(VoxelChunk chunk)
    {
        chunk.BeginBuild();
        await Task.Run(chunk.Build);
        chunk.CommitBuild();
    }

    public override void _EnterTree()
    {
        Name = "BlockPlacer";
        Instance = this;
    }

    public override void _ExitTree()
    {
        Instance = null;
    }


}
