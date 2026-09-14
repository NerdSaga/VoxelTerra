using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using VoxelTerra.Voxels;

namespace VoxelTerra.TerrainGeneration;

public partial class Terrain : Node3D
{

    private static Terrain instance;
    private VoxelChunkBuilder chunkBuilder;


    public static void SetBlockLocal(VoxelChunk chunk, Vector3I localPosition, UInt16 block)
    {
        chunk.SetBlock(localPosition, block);

        VoxelChunkBuilder.BuildChunk(chunk);
        // chunk.BeginBuild();
        // chunk.Build();
        // chunk.CommitBuild();
    }

    public static VoxelChunk LoadChunk(Vector2I chunkUnitPosition)
    {
        VoxelChunk chunk = VoxelChunk.Create(chunkUnitPosition);
        instance.AddChild(chunk);
        return chunk;
    }


    public static Terrain Init()
    {
        Terrain terrain = new();
        terrain.Name = "Terrain";
        return terrain;
    }

    public static void Quit()
    {
        if (instance == null) return;
        instance.QueueFree();
    }

    public override void _EnterTree()
    {
        instance = this;
        chunkBuilder = new VoxelChunkBuilder();
        instance.AddChild(chunkBuilder);
    }

    public override void _ExitTree()
    {
        instance = null;
        chunkBuilder.QueueFree();
    }
}