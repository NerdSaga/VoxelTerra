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

    public override void _Ready() {




        for (int y = -10; y < 10; y++)
        {
            for (int x = -10; x < 10; x++)
            {
                chunks.Add(Terrain.LoadChunk(new Vector2I(x, y)));
            }
        }

        foreach (VoxelChunk chunk in chunks)
        {
            Terrain.GenerateChunkTerrain(chunk);
            Terrain.BuildChunk(chunk);
        }

        // Godot.Timer timer = new();
        // timer.Timeout += timeout;
        // timer.Autostart = true;
        // timer.WaitTime = 0.5;
        // AddChild(timer);

        // BlockRegistry.PrintItems();
    }

    Vector3I pos = Vector3I.Zero;
    int current = 4;
    
    // public async void timeout()
    // {
    //     current = BlockRegistry.GetItem(VoxelTerra.Registries.Blocks.Empty.ItemName).ID;
        
    //     Terrain.SetBlockLocal(chunks[0], pos, current, 0);
    //     Terrain.BuildChunk(chunks[0]);

    //     pos.X += 1;
    //     if (pos.X >= 16)
    //     {
    //         pos.X = 0;
    //         pos.Z++;
    //     }

    //     if (pos.Z >= 16)
    //     {
    //         pos.Z = 0;
    //         pos.Y++;
    //     }
    // }
}
