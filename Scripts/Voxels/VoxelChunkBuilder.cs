using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Godot;
using VoxelTerra.Voxels;

public partial class VoxelChunkBuilder : Node
{

    private static VoxelChunkBuilder instance;
    private Thread thread;
    private volatile bool running = false;
    private AutoResetEvent chunksAvailable = new(false);
    private AutoResetEvent continueBuild = new(false);
    private ConcurrentDictionary<string, VoxelChunk> chunksToBuild = new();

    public static void BuildChunk(VoxelChunk chunk)
    {
        instance.chunksToBuild[chunk.Name] = chunk;
        instance.chunksAvailable.Set();
    }

    public static void BuildChunks(VoxelChunk[] chunks)
    {
        foreach (VoxelChunk chunk in chunks)
        {
            instance.chunksToBuild[chunk.Name] = chunk;
        }
        instance.chunksAvailable.Set();
    }

    private void ThreadMain()
    {
        while (running)
        {
            chunksAvailable.WaitOne();

            KeyValuePair<string, VoxelChunk>[] chunkKeyValues = chunksToBuild.ToArray();

            for (int i = 0; i < chunkKeyValues.Length; i++)
            {
                if (!chunksToBuild.Remove(chunkKeyValues[i].Key, out VoxelChunk chunk)) return;

                CallDeferred(VoxelChunkBuilder.MethodName.chunkBeginBuild, chunk);
                continueBuild.WaitOne();

                if (!running) return;
                chunk.Build();

                
                CallDeferred(VoxelChunkBuilder.MethodName.chunkCommitBuild, chunk);
                continueBuild.WaitOne();
                if (!running) return;
            }
        }
    }

    private void chunkBeginBuild(VoxelChunk chunk)
    {
        if (chunk != null)
        {
            chunk.BeginBuild();
        }

        continueBuild.Set();
    }

    private void chunkCommitBuild(VoxelChunk chunk)
    {
        if (chunk != null)
        {
            chunk.CommitBuild();
        }
        
        continueBuild.Set();
    }

    public override void _EnterTree()
    {
        Name = "VoxelChunkBuilder";
        instance = this;
        running = true;
        thread = new(ThreadMain);
        thread.Start();
    }

    public override void _ExitTree()
    {
        instance = null;
        running = false;
        chunksAvailable.Set();
        continueBuild.Set();
        thread.Join();
    }
}