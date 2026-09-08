using Godot;
using VoxelTerra.WorkerThreads;

namespace VoxelTerra.TerrainGeneration;

public partial class VoxelChunkBuilder : WorkerThread
{

    public static VoxelChunkBuilder Instance;

    public void SetBlockLocal(VoxelChunk chunk, Vector3I localPosition, ushort block)
    {
        int chunkBlockIndex = localPosition.X;
        chunkBlockIndex += localPosition.Y * 256;
        chunkBlockIndex += localPosition.Z * 16;

        chunk.Blocks[chunkBlockIndex] = block;

        chunk.VMesh.Begin();
        chunk.VCollision.Begin();

        mainJobQueue.Enqueue(new VoxelChunkBuilderJob(chunk));
        jobAvailable.Set();
        
        chunk.OnFinishBuild.WaitOne();
        chunk.VMesh.Commit();
        chunk.VCollision.Commit();

        
    }

    public override void _EnterTree()
    {
        base._EnterTree();

        Instance = this;
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        Instance = null;
    }

    public VoxelChunkBuilder(string name) : base(name) {}
}