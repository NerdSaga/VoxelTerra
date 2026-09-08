using Godot;
using System;

using VoxelTerra.CollisionObjects;
using VoxelTerra.MeshObjects;
using VoxelTerra.TerrainGeneration;
using VoxelTerra.Common;
public partial class Main : Node3D
{

    VoxelChunk chunk;
    VoxelChunkBuilder cb;

    public override void _Ready() {

        chunk = VoxelChunk.Create();
        AddChild(chunk);

        cb = new("chunk_builder");
        AddChild(cb);

        // cb.QueueJob(new VoxelChunkBuilderJob(chunk));
        // cb.QueueFree();
        // cb = null;

        Timer timer = new();
        timer.Timeout += timeout;
        timer.Autostart = true;
        timer.WaitTime = 0.01;
        AddChild(timer);
    }

    int chunkBlockIndex = 0;
    public void timeout()
    {
        chunkBlockIndex++;
        chunk.SetBlock(chunkBlockIndex, 1);
        cb.QueueJob(new VoxelChunkBuilderJob(chunk));
    }
}
