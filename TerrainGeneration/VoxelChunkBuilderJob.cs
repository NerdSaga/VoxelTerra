using System;
using Godot;
using VoxelTerra.Common;
using VoxelTerra.TerrainGeneration;
using VoxelTerra.WorkerThreads;

namespace VoxelTerra.TerrainGeneration;

public class VoxelChunkBuilderJob : WorkerThreadJob
{

    private VoxelChunk chunk;

    public override void PreJob()
    {
        chunk.VMesh.Begin();
        chunk.VCollision.Begin();
    }

    public override void MainJob()
    {
        // 

        Func<VoxelChunk, int, int, int, int> setBlock = static (VoxelChunk chunk, int x, int y, int z) =>
        {
            int chunkBlockIndex = x;
            chunkBlockIndex += y * 256;
            chunkBlockIndex += z * 16;

            ushort blockBytes = chunk.Blocks[chunkBlockIndex];

            if (blockBytes > 0)
            {
                chunk.VMesh.AddVoxel(0, new Vector3(x, y, z), VoxelTemplateData.UV, VoxelTemplateData.COLOR, VoxelTemplateData.FACES_ALL);
                chunk.VCollision.AddVoxel(new Vector3(x, y, z), VoxelTemplateData.FACES_ALL);
            }

            return 0;
        };

        for (int y = 0; y < 256; y++)
        {
            for (int z = 0; z < 16; z++)
            {
                for (int x = 0; x < 16; x++)
                {
                    setBlock(chunk, x, y, z);
                }
            }
        }

        chunk.OnFinishBuild.Set();
    }

    public override void PostJob()
    {
        chunk.VMesh.Commit();
        chunk.VCollision.Commit();
    }

    public VoxelChunkBuilderJob(VoxelChunk chunk)
    {
        this.chunk = chunk;
    }
}