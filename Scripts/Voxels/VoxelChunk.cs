using Godot;
using System;
using VoxelTerra.Registries;
using VoxelTerra.TerrainGeneration;

namespace VoxelTerra.Voxels;

public partial class VoxelChunk : StaticBody3D
{
    public UInt16[] Blocks {get;} = new ushort[16 * 16 * 256];
    public VoxelMesh VMesh;
    public VoxelCollision VCollision;
    public VoxelChunk[] Neighbors;

    private static class BlockInfo
    {
        public static bool[] GetMeshFaces(VoxelChunk chunk, BlockRegistryItem block, int chunkBlockIndex, BlockVariant blockVariant)
        {
            switch (blockVariant.RenderType)
            {
                case BlockVariant.BlockRenderType.VOID:
                    return new bool[] {false, false, false, false, false, false};
                
                case BlockVariant.BlockRenderType.ALL_FACES:
                    return new bool[] {true, true, true, true, true, true};

                case BlockVariant.BlockRenderType.SOLID:
                    // Only false next to solid block.
                break;

                case BlockVariant.BlockRenderType.TRANSPARENT:
                    // Only false if next to same block.

                    int blockidT = GetBlockID(chunk, chunkBlockIndex + 256);
                    int blockidB = GetBlockID(chunk, chunkBlockIndex - 256);
                    int blockidN = GetBlockID(chunk, chunkBlockIndex + 1);
                    int blockidS = GetBlockID(chunk, chunkBlockIndex - 1);
                    int blockidE = GetBlockID(chunk, chunkBlockIndex + 16);
                    int blockidW = GetBlockID(chunk, chunkBlockIndex - 16);

                    return new bool[]
                    {
                        block.ID != blockidT,
                        block.ID != blockidB,
                        block.ID != blockidN,
                        block.ID != blockidS,
                        block.ID != blockidE,
                        block.ID != blockidW,
                    };

            }
            return new bool[] {false, false, false, false, false, false};
        }

        public static int GetBlockID(VoxelChunk chunk, int chunkBlockIndex)
        {
            if (chunkBlockIndex < 0 || chunkBlockIndex > 16 * 16 * 256)
            {
                return 0;
            }

            UInt16 blockBytes = chunk.Blocks[chunkBlockIndex];
            int blockID = blockBytes & 0x0fff;
            return blockID;
        }

        public static BlockRegistryItem GetBlock(VoxelChunk chunk, int chunkBlockIndex)
        {
            int blockID = GetBlockID(chunk, chunkBlockIndex);
            return BlockRegistry.GetItem(blockID);
        }
    }

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

    public void SetBlock(int chunkBlockIndex, int blockID, int blockVariant)
    {
        blockVariant = blockVariant << 4 * 3;
        UInt16 blockBytes = (UInt16)(blockVariant | blockID);
        Blocks[chunkBlockIndex] = blockBytes;
    }

    public void SetBlock(Vector3I localPosition, int blockID, int blockVariant)
    {
        int chunkBlockIndex = localPosition.X;
        chunkBlockIndex += localPosition.Y * 256;
        chunkBlockIndex += localPosition.Z * 16;
        SetBlock(chunkBlockIndex, blockID, blockVariant);
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

            UInt16 blockBytes = chunk.Blocks[chunkBlockIndex];
            int blockID = blockBytes & 0x0fff;
            int blockVariantID = (blockBytes & 0xf000) >> 4 * 3;
            BlockRegistryItem block = BlockRegistry.GetItem(blockID);
            BlockVariant blockVariant = block.GetVariant(blockVariantID);

            bool[] meshFaces = BlockInfo.GetMeshFaces(chunk, block, chunkBlockIndex, blockVariant);

            if (blockID != 0)
            {
                if (blockVariant.RenderType != BlockVariant.BlockRenderType.VOID)
                {
                    chunk.VMesh.AddVoxel(0, new Vector3(x, y, z), blockVariant.GenerateBlockUV(), VoxelTemplateData.COLOR, meshFaces);
                }

                if (blockVariant.CollisionType != BlockVariant.BlockCollisionType.VOID)
                {
                    chunk.VCollision.AddVoxel(new Vector3(x, y, z), VoxelTemplateData.FACES_ALL);
                }
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
        VMesh = GetNode<VoxelMesh>("VoxelMesh");

        VCollision = GetNode<VoxelCollision>("VoxelCollision");

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
