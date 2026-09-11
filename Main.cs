using Godot;
using System;
using System.Threading;
using VoxelTerra.Debugging;
using VoxelTerra.Voxels;

public partial class Main : Node3D
{

    VoxelChunk chunk;

    public override void _Ready() {

        chunk = VoxelChunk.Create();
        AddChild(chunk);

        Godot.Timer timer = new();
        timer.Timeout += timeout;
        timer.Autostart = true;
        timer.WaitTime = 0.1;
        AddChild(timer);

        VTDebug.ErrorAbort("LOL");
    }

    int chunkBlockIndex = 0;
    public async void timeout()
    {
        chunk.SetBlock(chunkBlockIndex, 1);
        chunk.BeginBuild();
        chunk.Build();
        chunk.CommitBuild();
        chunkBlockIndex++;
    }
}
