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


        for (int y = -2; y < 2; y++)
        {
            for (int x = -2; x < 2; x++)
            {
                chunks.Add(Terrain.LoadChunk(new Vector2I(x, y)));
            }
        }

        Godot.Timer timer = new();
        timer.Timeout += timeout;
        timer.Autostart = true;
        timer.WaitTime = 0.2;
        AddChild(timer);

        BlockRegistry.PrintItems();
    }

    Vector3I pos = Vector3I.Zero;
    int current = 4;
    
    public async void timeout()
    {
        current = BlockRegistry.GetItem(VoxelTerra.Registries.Blocks.DefaultBlock.ItemName).ID;
        
        foreach (VoxelChunk chunk in chunks)
        {
            Terrain.SetBlockLocal(chunk, pos, current, 0);
        }

        pos.X += 1;
        if (pos.X >= 16)
        {
            pos.X = 0;
            pos.Z++;
        }

        if (pos.Z >= 16)
        {
            pos.Z = 0;
            pos.Y++;
        }
    }
}
