using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using VoxelTerra.TerrainGeneration;

namespace VoxelTerra.Voxels;

public partial class VoxelChunk : StaticBody3D
{
    public UInt16[] Blocks {get;} = new ushort[16 * 16 * 256];
    public VoxelMesh VMesh;
    public VoxelCollision VCollision;
    public VoxelChunk[] Neighbors;
    public enum NeighborDirection
    {
        NW = 0,
        N = 1,
        NE = 2,
        W = 3,
        E = 4,
        SW = 5,
        S = 6,
        SE = 7
    }

    public void SetBlock(int chunkBlockIndex, UInt16 block)
    {
        Blocks[chunkBlockIndex] = block;
    }

    public void SetBlock(Vector3I localPosition, UInt16 block)
    {
        int chunkBlockIndex = localPosition.X;
        chunkBlockIndex += localPosition.Y * 256;
        chunkBlockIndex += localPosition.Z * 16;
        SetBlock(chunkBlockIndex, block);
    }

    public VoxelChunk GetNeighbor(NeighborDirection direction)
    {
        return Neighbors[(int)direction];
    }

    public void BeginBuild()
    {
        VMesh.Begin();
        VCollision.Begin();
    }

    public void Build()
    {
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
                    setBlock(this, x, y, z);
                }
            }
        }
    }

    public void CommitBuild()
    {
        VMesh.Commit();
        VCollision.Commit();
    }

    public override void _Ready()
    {
        VMesh = VoxelMesh.Create();
        AddChild(VMesh);

        VCollision = VoxelCollision.Create();
        AddChild(VCollision);

        Array.Fill<UInt16>(Blocks, 0);
    }

    public override void _Process(double delta)
    {
    }


    private static PackedScene SCENE = GD.Load<PackedScene>("uid://druq8cym8v1jq");
    public static VoxelChunk Create(Vector2I chunkUnitPosition)
    {
        VoxelChunk chunk = SCENE.Instantiate<VoxelChunk>();
        chunk.Position = new Vector3(chunkUnitPosition.X * 16, 0, chunkUnitPosition.Y * 16);
        chunk.Name = Terrain.GetChunkName(chunkUnitPosition);
        return chunk;
    }
}
