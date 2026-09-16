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


        int block = 3501;
        int variant = 6;

        variant = variant << 4 * 3;
        UInt16 blockBytes = (UInt16)(variant | block);

        block = blockBytes & 0x0fff;
        variant = (variant & 0xf000) >> 4 * 3;
        GD.Print(block);
        GD.Print(variant);

        // blockBytes = 0b0000-0000-0001-1111;

        AddChild(Terrain.Init());

        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                chunks.Add(Terrain.LoadChunk(new Vector2I(x, y)));
            }
        }


        // foreach (VoxelChunk chunk in chunks)
        // {
        //     int blockID = BlockRegistry.GetItem(VoxelTerra.Registries.Blocks.GrassBlock.ItemName).ID;
        //     Terrain.SetBlockLocal(chunk, new Vector3I(1, 0, 1), blockID, 0);
        //     Terrain.SetBlockLocal(chunk, new Vector3I(1, 1, 1), blockID, 0);
        //     Terrain.SetBlockLocal(chunk, new Vector3I(1, 2, 1), blockID, 0);
        //     Terrain.SetBlockLocal(chunk, new Vector3I(0, 1, 1), blockID, 0);
        //     Terrain.SetBlockLocal(chunk, new Vector3I(2, 1, 1), blockID, 0);
        //     Terrain.SetBlockLocal(chunk, new Vector3I(1, 1, 0), blockID, 0);
        //     Terrain.SetBlockLocal(chunk, new Vector3I(1, 1, 2), blockID, 0);
        // }


        Godot.Timer timer = new();
        timer.Timeout += timeout;
        timer.Autostart = true;
        timer.WaitTime = 0.5;
        AddChild(timer);

        BlockRegistry.PrintItems();

        // var block = BlockRegistry.GetItem(0);

    }

    Vector3I pos = Vector3I.Zero;
    int current = 4;
    
    public async void timeout()
    {
        current = BlockRegistry.GetItem(VoxelTerra.Registries.Blocks.MapleLog.ItemName).ID;
        
        foreach (VoxelChunk chunk in chunks)
        {
            Terrain.SetBlockLocal(chunk, pos, current, 2);
        }

        pos.X += 2;
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

    public override void _ExitTree()
    {
        Terrain.Quit();
    }

}
