using Godot;
using System;

using VoxelTerra.CollisionObjects;
using VoxelTerra.MeshObjects;
using VoxelTerra.TerrainGeneration;
using VoxelTerra.Common;
using VoxelTerra.WorkerThreads;
using System.Threading;
public partial class Main : Node3D
{

    VoxelChunk chunk;
    BlockPlacer bp;

    public override void _Ready() {

        GD.Print($"_Ready() {Thread.CurrentThread.ManagedThreadId}");
        chunk = VoxelChunk.Create();
        AddChild(chunk);

        bp = new();
        AddChild(bp);

        Godot.Timer timer = new();
        timer.Timeout += timeout;
        timer.Autostart = true;
        timer.WaitTime = 0.1;
        AddChild(timer);
    }

    int chunkBlockIndex = 0;
    public async void timeout()
    {
        chunkBlockIndex++;
        await BlockPlacer.SetBlockLocal(chunk, new Vector3I(chunkBlockIndex, 0, 0), 1);
        await BlockPlacer.SetBlockLocal(chunk, new Vector3I(chunkBlockIndex, 1, 0), 1);
        await BlockPlacer.SetBlockLocal(chunk, new Vector3I(chunkBlockIndex, 2, 0), 1);
    }
}
