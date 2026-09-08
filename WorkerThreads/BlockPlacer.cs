using Godot;
using VoxelTerra.TerrainGeneration;

namespace VoxelTerra.WorkerThreads;

public partial class BlockPlacer : WorkerThread
{
    public static BlockPlacer Instance;

    public static void SetBlockLocal(VoxelChunk chunk, Vector3I position, ushort block)
    {
        int chunkBlockIndex = position.X;
        chunkBlockIndex += position.Y * 256;
        chunkBlockIndex += position.Z * 16;

        chunk.Blocks[chunkBlockIndex] = block;

        Instance.regenChunk(chunk);
    }

    private void regenChunk(VoxelChunk chunk)
    {
        chunk.VMesh.Begin();
        chunk.VCollision.Begin();
        jobQueue.Enqueue(new BlockPlacerJob(chunk));
        JobsAvailable.Set();
        chunk.OnFinishBuild.WaitOne();
        chunk.VMesh.Commit();
        chunk.VCollision.Commit();
    }

    // public BlockPlacer Init()
    // {
        
    // }

    public override void _EnterTree()
    {
        base._EnterTree();
        Instance = this;
        Name = "WorkerThread-BlockPlacer";
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        Instance = null;
    }
}