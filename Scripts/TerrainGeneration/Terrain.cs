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
    private Dictionary<string, VoxelChunk> chunks = new();


    public static void SetBlockLocal(VoxelChunk chunk, Vector3I localPosition, int blockID, int blockVariant)
    {
        chunk.SetBlock(localPosition, blockID, blockVariant);

        VoxelChunkBuilder.BuildChunk(chunk);
    }

    public static VoxelChunk LoadChunk(Vector2I chunkUnitPosition)
    {
        // Return the chunk if it already exists.

        Func<Vector2I, VoxelChunk> getChunk = static(Vector2I chunkUnitPosition) =>
        {
            if (instance.chunks.TryGetValue(GetChunkName(chunkUnitPosition), out VoxelChunk chunk))
            {
                return chunk;
            }

            VoxelChunk newChunk = VoxelChunk.Create(chunkUnitPosition);
            instance.chunks[newChunk.Name] = newChunk;
            instance.AddChild(newChunk);

            return newChunk;
        };

        VoxelChunk chunk = getChunk(chunkUnitPosition);
        VoxelChunk[] neighbors = // Check if neghbors are correct.
        {
            getChunk(chunkUnitPosition + new Vector2I(1, -1)),
            getChunk(chunkUnitPosition + new Vector2I(1, 0)),
            getChunk(chunkUnitPosition + new Vector2I(1, 1)),
            getChunk(chunkUnitPosition + new Vector2I(0, -1)),
            getChunk(chunkUnitPosition + new Vector2I(0, 1)),
            getChunk(chunkUnitPosition + new Vector2I(-1, -1)),
            getChunk(chunkUnitPosition + new Vector2I(-1, 0)),
            getChunk(chunkUnitPosition + new Vector2I(-1, 1)),
        };

        chunk.Neighbors = neighbors;
        
        return getChunk(chunkUnitPosition);
    }

    public static VoxelChunk GetChunk(Vector2I chunkUnitPosition)
    {
        return instance.chunks[GetChunkName(chunkUnitPosition)];
    }

    public static string GetChunkName(Vector2I chunkUnitPosition)
    {
        return $"Chunk_{chunkUnitPosition.X}_{chunkUnitPosition.Y}";
    }

    public static int GetChunkCount()
    {
        return instance.chunks.Count;
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