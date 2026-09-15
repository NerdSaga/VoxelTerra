using Godot;
using System;
using System.Threading;
using VoxelTerra.Debugging;
using VoxelTerra.Voxels;
using VoxelTerra.Registries;
using VoxelTerra.TerrainGeneration;
using System.Collections.Generic;

public partial class Main : Node3D
{

    List<VoxelChunk> chunks = new();
    Terrain terrain;

    public override void _Ready() {

        AddChild(Terrain.Init());

        for (int y = 0; y < 1; y++)
        {
            for (int x = 0; x < 2; x++)
            {
                chunks.Add(Terrain.LoadChunk(new Vector2I(x, y)));
            }
        }

        Godot.Timer timer = new();
        timer.Timeout += timeout;
        timer.Autostart = true;
        timer.WaitTime = 0.1;
        AddChild(timer);

        var block = BlockRegistry.GetItem(0);
        GD.Print(VoxelTerra.Registries.Blocks.GrassBlock.ItemName);
        GD.Print(VoxelTerra.Registries.Blocks.Sand.ItemName);
        // GD.Print(VoxelTerra.Registries.Blocks.Sand.ITEM_NAME);

        // VTDebug.ErrorAbort("LOL");

        BlockRegistry.PrintItems();

    }

    int chunkBlockIndex = 0;
    public async void timeout()
    {
        foreach (VoxelChunk chunk in chunks)
        {
            Terrain.SetBlockLocal(chunk, new Vector3I(chunkBlockIndex, 0, 0), 1);
        }
        chunkBlockIndex++;
    }

    public override void _ExitTree()
    {
        Terrain.Quit();
    }

}
