using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using VoxelTerra.Voxels;
using VoxelTerra.TerrainGeneration.Generators;

namespace VoxelTerra.TerrainGeneration;

/// <summary>
/// Contains static methods for editing and loading terrain. Must be added as a child node in the scene tree before accessing these methods.
/// </summary>
public partial class Terrain : Node3D
{

    private static Terrain instance;
    private TerrainGenerator terrainGenerator = new TerrainFlat();
    private VoxelChunkBuilder chunkBuilder;
    private Dictionary<string, VoxelChunk> chunks = new();

    public static void Configure(TerrainGenerator terrainGenerator, int seed)
    {
        instance.terrainGenerator = terrainGenerator;
        instance.terrainGenerator.Seed = seed;
        instance.terrainGenerator.Init();
    }

    /// <summary>
    /// Sets a block at a position local to a chunk.
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <param name="blockID"></param>
    /// <param name="blockVariant"></param>
    public static void SetBlockLocal(VoxelChunk chunk, Vector3I localPosition, int blockID, int blockVariant)
    {
        chunk.SetBlock(localPosition, blockID, blockVariant);
    }

    /// <summary>
    /// Loads a chunk at a chunk unit position.
    /// </summary>
    /// <param name="chunkUnitPosition"></param>
    /// <returns></returns>
    public static VoxelChunk LoadChunk(Vector2I chunkUnitPosition)
    {
        Func<Vector2I, VoxelChunk> getChunk = static(Vector2I chunkUnitPosition) =>
        {
            // If the chunk already exists return it.
            if (instance.chunks.TryGetValue(GetChunkName(chunkUnitPosition), out VoxelChunk chunk))
            {
                return chunk;
            }

            // The chunk does not exist so create a new one.
            VoxelChunk newChunk = VoxelChunk.Create(chunkUnitPosition);
            instance.chunks[newChunk.Name] = newChunk;
            instance.AddChild(newChunk);
            instance.terrainGenerator.Generate(newChunk); // Later we need to load from a file instead.

            return newChunk;
        };

        // Get the main chunk.
        VoxelChunk chunk = getChunk(chunkUnitPosition);

        // Set the main chunk's neighbors.
        chunk.Neighbors = new VoxelChunk[]
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
        
        return getChunk(chunkUnitPosition);
    }

    /// <summary>
    /// Gets a chunk at a chunk unit position.
    /// </summary>
    /// <param name="chunkUnitPosition"></param>
    /// <returns></returns>
    public static VoxelChunk GetChunk(Vector2I chunkUnitPosition)
    {
        return instance.chunks[GetChunkName(chunkUnitPosition)];
    }

    public static void GenerateChunkTerrain(VoxelChunk chunk)
    {
        instance.terrainGenerator.Generate(chunk);
    }

    public static void BuildChunk(VoxelChunk chunk)
    {
        VoxelChunkBuilder.BuildChunk(chunk);
    }

    public static string GetChunkName(Vector2I chunkUnitPosition)
    {
        return $"Chunk_{chunkUnitPosition.X}_{chunkUnitPosition.Y}";
    }

    /// <summary>
    /// Gets the number of chunks that are childeren to this node.
    /// </summary>
    /// <returns></returns>
    public static int GetChunkCount()
    {
        return instance.chunks.Count;
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