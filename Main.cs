using Godot;
using System;

using VoxelTerra.CollisionObjects;
using VoxelTerra.MeshObjects;
using VoxelTerra.TerrainGeneration;
using VoxelTerra.Common;
using VoxelTerra.WorkerThreads;
public partial class Main : Node3D
{

    VoxelChunk chunk;
    BlockPlacer bp;

    public override void _Ready() {

        chunk = VoxelChunk.Create();
        AddChild(chunk);

        bp = new();
        AddChild(bp);

        // cb.QueueJob(new VoxelChunkBuilderJob(chunk));
        // cb.QueueFree();
        // cb = null;

        Timer timer = new();
        timer.Timeout += timeout;
        timer.Autostart = true;
        timer.WaitTime = 0.1;
        AddChild(timer);
    }

    int chunkBlockIndex = 0;
    public void timeout()
    {
        chunkBlockIndex++;
        // chunk.SetBlock(chunkBlockIndex, 1);
        BlockPlacer.SetBlockLocal(chunk, new Vector3I(chunkBlockIndex, 0, 0), 1);
    }
}
