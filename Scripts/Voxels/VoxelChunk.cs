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

    private static class ChunkUtils
    {
        public static bool[] GetMeshFaces(VoxelChunk chunk, BlockRegistryItem block, Vector3I localPosition, BlockVariant blockVariant)
        {
            switch (block.RenderType)
            {
                case BlockRegistryItem.BlockRenderType.VOID:
                    return new bool[] {false, false, false, false, false, false};
                
                case BlockRegistryItem.BlockRenderType.ALL_FACES:
                    return new bool[] {true, true, true, true, true, true};

                case BlockRegistryItem.BlockRenderType.SOLID:
                {
                    // Only false if next to solid.
                    BlockRegistryItem blockT = BlockRegistry.GetItem(GetBlockID(chunk, localPosition + new Vector3I(0, 1, 0)));
                    BlockRegistryItem blockB = BlockRegistry.GetItem(GetBlockID(chunk, localPosition + new Vector3I(0, -1, 0)));
                    BlockRegistryItem blockN = BlockRegistry.GetItem(GetBlockID(chunk, localPosition + new Vector3I(1, 0, 0)));
                    BlockRegistryItem blockS = BlockRegistry.GetItem(GetBlockID(chunk, localPosition + new Vector3I(-1, 0, 0)));
                    BlockRegistryItem blockE = BlockRegistry.GetItem(GetBlockID(chunk, localPosition + new Vector3I(0, 0, 1)));
                    BlockRegistryItem blockW = BlockRegistry.GetItem(GetBlockID(chunk, localPosition + new Vector3I(0, 0, -1)));

                    return new bool[]
                    {
                        blockT.RenderType != BlockRegistryItem.BlockRenderType.SOLID,
                        blockB.RenderType != BlockRegistryItem.BlockRenderType.SOLID,
                        blockN.RenderType != BlockRegistryItem.BlockRenderType.SOLID,
                        blockS.RenderType != BlockRegistryItem.BlockRenderType.SOLID,
                        blockE.RenderType != BlockRegistryItem.BlockRenderType.SOLID,
                        blockW.RenderType != BlockRegistryItem.BlockRenderType.SOLID,
                    };
                }


                case BlockRegistryItem.BlockRenderType.TRANSPARENT:
                {
                    // Only false if next to same block.
                    int blockidT = GetBlockID(chunk, localPosition + new Vector3I(0, 1, 0));
                    int blockidB = GetBlockID(chunk, localPosition + new Vector3I(0, -1, 0));
                    int blockidN = GetBlockID(chunk, localPosition + new Vector3I(1, 0, 0));
                    int blockidS = GetBlockID(chunk, localPosition + new Vector3I(-1, 0, 0));
                    int blockidE = GetBlockID(chunk, localPosition + new Vector3I(0, 0, 1));
                    int blockidW = GetBlockID(chunk, localPosition + new Vector3I(0, 0, -1));

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

            }
            return new bool[] {false, false, false, false, false, false};
        }

        public static int GetBlockID(VoxelChunk chunk, Vector3I localPosition)
        {
            if (localPosition.Y < 0 || localPosition.Y >= 256)
            {
                return 0;
            }

            if (localPosition.X < 0 || localPosition.X >= 16)
            {
                return 0;
            }

            if (localPosition.Z < 0 || localPosition.Z >= 16)
            {
                return 0;
            }

            int chunkBlockIndex = localPosition.X;
            chunkBlockIndex += localPosition.Y * 256;
            chunkBlockIndex += localPosition.Z * 16;

            if (chunkBlockIndex < 0 || chunkBlockIndex > 16 * 16 * 256)
            {
                return 0;
            }

            UInt16 blockBytes = chunk.Blocks[chunkBlockIndex];
            int blockID = blockBytes & 0x0fff;
            return blockID;
        }

        public static BlockRegistryItem GetBlock(VoxelChunk chunk, Vector3I localPosition)
        {
            int blockID = GetBlockID(chunk, localPosition);
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

            bool[] faces = ChunkUtils.GetMeshFaces(chunk, block, new Vector3I(x, y, z), blockVariant);

            if (blockID != 0)
            {
                if (block.RenderType != BlockRegistryItem.BlockRenderType.VOID)
                {
                    chunk.VMesh.AddVoxel(0, new Vector3(x, y, z), blockVariant.GenerateBlockUV(), VoxelTemplateData.COLOR, faces);
                }

                if (block.CollisionType != BlockRegistryItem.BlockCollisionType.VOID)
                {
                    chunk.VCollision.AddVoxel(new Vector3(x, y, z), faces);
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
