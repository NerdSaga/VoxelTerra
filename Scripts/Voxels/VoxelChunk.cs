using Godot;
using System;
using System.Threading;
using VoxelTerra.Registries;
using VoxelTerra.TerrainGeneration;

namespace VoxelTerra.Voxels;

/// <summary>
/// Contains a column of voxels with a collision, and mesh. The voxel size of a chunk is 16 * 256 * 16.
/// </summary>
public partial class VoxelChunk : StaticBody3D
{
    public UInt16[] Blocks {get;} = new UInt16[16 * 16 * 256];
    public VoxelMesh VMesh;
    public VoxelCollision VCollision;
    public VoxelChunk[] Neighbors;
    /// <summary>
    /// The world position of a chunk in square chunk-size units: 16 x 16
    /// </summary>
    public Vector2I ChunkUnitPosition = Vector2I.Zero;
    public Vector3I WorldPosition
    {
        get
        {
            return new Vector3I(ChunkUnitPosition.X * 16, 0, ChunkUnitPosition.Y * 16);
        }
    }

    /// <summary>
    /// Contains static functions that are relivant to mesh and collision generation.
    /// </summary>
    private static class ChunkUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chunk"></param>
        /// <param name="block"></param>
        /// <param name="localPosition"></param>
        /// <returns></returns>
        public static bool[] GetMeshFaces(VoxelChunk chunk, BlockRegistryItem block, Vector3I localPosition, BlockRegistryItem[] blockNeighbors)
        {
            switch (block.RenderType)
            {
                case BlockRegistryItem.BlockRenderType.VOID:
                    return new bool[] {false, false, false, false, false, false};
                
                case BlockRegistryItem.BlockRenderType.ALL_FACES:
                    return new bool[] {true, true, true, true, true, true};
            }

            switch (block.RenderType)
            {
                case BlockRegistryItem.BlockRenderType.SOLID:
                    return new bool[]
                    {
                        blockNeighbors[0].RenderType != BlockRegistryItem.BlockRenderType.SOLID,
                        blockNeighbors[1].RenderType != BlockRegistryItem.BlockRenderType.SOLID,
                        blockNeighbors[2].RenderType != BlockRegistryItem.BlockRenderType.SOLID,
                        blockNeighbors[3].RenderType != BlockRegistryItem.BlockRenderType.SOLID,
                        blockNeighbors[4].RenderType != BlockRegistryItem.BlockRenderType.SOLID,
                        blockNeighbors[5].RenderType != BlockRegistryItem.BlockRenderType.SOLID,
                    };
                
                case BlockRegistryItem.BlockRenderType.TRANSPARENT:
                    return new bool[]
                    {
                        block.ID != blockNeighbors[0].ID && blockNeighbors[0].RenderType != BlockRegistryItem.BlockRenderType.SOLID,
                        block.ID != blockNeighbors[1].ID && blockNeighbors[1].RenderType != BlockRegistryItem.BlockRenderType.SOLID,
                        block.ID != blockNeighbors[2].ID && blockNeighbors[2].RenderType != BlockRegistryItem.BlockRenderType.SOLID,
                        block.ID != blockNeighbors[3].ID && blockNeighbors[3].RenderType != BlockRegistryItem.BlockRenderType.SOLID,
                        block.ID != blockNeighbors[4].ID && blockNeighbors[4].RenderType != BlockRegistryItem.BlockRenderType.SOLID,
                        block.ID != blockNeighbors[5].ID && blockNeighbors[5].RenderType != BlockRegistryItem.BlockRenderType.SOLID,
                    };
            }

            return null;
        }

        public static bool[] GetCollisionFaces(VoxelChunk chunk, BlockRegistryItem block, Vector3I localPosition, BlockRegistryItem[] neighbors)
        {
            switch (block.CollisionType)
            {
                case BlockRegistryItem.BlockCollisionType.VOID:
                    return new bool[] {false, false, false, false, false, false};
                    
                
                case BlockRegistryItem.BlockCollisionType.ALL_FACES:
                    return new bool[] {true, true, true, true, true, true};
            }

            // bool[] faces = null;

            switch (block.CollisionType)
            {
                case BlockRegistryItem.BlockCollisionType.SOLID:
                    return new bool[]
                    {
                        neighbors[0].CollisionType != BlockRegistryItem.BlockCollisionType.SOLID,
                        neighbors[1].CollisionType != BlockRegistryItem.BlockCollisionType.SOLID,
                        neighbors[2].CollisionType != BlockRegistryItem.BlockCollisionType.SOLID,
                        neighbors[3].CollisionType != BlockRegistryItem.BlockCollisionType.SOLID,
                        neighbors[4].CollisionType != BlockRegistryItem.BlockCollisionType.SOLID,
                        neighbors[5].CollisionType != BlockRegistryItem.BlockCollisionType.SOLID,
                    };
                
                case BlockRegistryItem.BlockCollisionType.TRANSPARENT:
                    return new bool[]
                    {
                        // Not the same block \\ or // Not next to solid
                        block.ID != neighbors[0].ID && neighbors[0].CollisionType != BlockRegistryItem.BlockCollisionType.SOLID,
                        block.ID != neighbors[1].ID && neighbors[1].CollisionType != BlockRegistryItem.BlockCollisionType.SOLID,
                        block.ID != neighbors[2].ID && neighbors[2].CollisionType != BlockRegistryItem.BlockCollisionType.SOLID,
                        block.ID != neighbors[3].ID && neighbors[3].CollisionType != BlockRegistryItem.BlockCollisionType.SOLID,
                        block.ID != neighbors[4].ID && neighbors[4].CollisionType != BlockRegistryItem.BlockCollisionType.SOLID,
                        block.ID != neighbors[5].ID && neighbors[5].CollisionType != BlockRegistryItem.BlockCollisionType.SOLID,
                    };
            }
            
            return null;
        }

        public static BlockRegistryItem[] GetBlockNeighbors(VoxelChunk chunk, Vector3I localPosition)
        {
            return new BlockRegistryItem[]
            {
                BlockRegistry.GetItem(GetBlockID(chunk, localPosition + new Vector3I(+0, +1, +0))),
                BlockRegistry.GetItem(GetBlockID(chunk, localPosition + new Vector3I(+0, -1, +0))),
                BlockRegistry.GetItem(GetBlockID(chunk, localPosition + new Vector3I(+1, +0, +0))),
                BlockRegistry.GetItem(GetBlockID(chunk, localPosition + new Vector3I(-1, +0, +0))),
                BlockRegistry.GetItem(GetBlockID(chunk, localPosition + new Vector3I(+0, +0, +1))),
                BlockRegistry.GetItem(GetBlockID(chunk, localPosition + new Vector3I(+0, +0, -1))),
            };
        }

        public static int GetBlockID(VoxelChunk chunk, Vector3I localPosition)
        {
            if (localPosition.Y < 0 || localPosition.Y >= 256)
            {
                return 0;
            }

            VoxelChunk workingChunk = chunk;

            if (GetNeighborFromLocalPosition(localPosition, chunk, out VoxelChunk neighbor))
            {
                if (localPosition.X < 0)
                {
                    localPosition.X += 16;
                }
                else if (localPosition.X >= 16)
                {
                    localPosition.X -= 16;
                }

                if (localPosition.Z < 0)
                {
                    localPosition.Z += 16;
                }
                else if (localPosition.Z >= 16)
                {
                    localPosition.Z -= 16;
                }

                workingChunk = neighbor;
            }
            // if (localPosition.Z < 0 || localPosition.Z >= 16)
            // {
            //     return 0;
            // }

            int chunkBlockIndex = localPosition.X;
            chunkBlockIndex += localPosition.Y * 256;
            chunkBlockIndex += localPosition.Z * 16;

            if (chunkBlockIndex < 0 || chunkBlockIndex > 16 * 16 * 256)
            {
                return 0;
            }

            UInt16 blockBytes = workingChunk.Blocks[chunkBlockIndex];
            int blockID = blockBytes & 0x0fff;
            return blockID;
        }

        public static bool GetNeighborFromLocalPosition(Vector3I localPosition, VoxelChunk chunk, out VoxelChunk neighbor)
        {

            if (localPosition.X < 0)
            {
                if (localPosition.Z < 0)
                {
                    // Sw
                    neighbor = chunk.Neighbors[(int)NeighborDirection.SW];
                }

                else if (localPosition.Z >= 16)
                {
                    // Se
                    neighbor = chunk.Neighbors[(int)NeighborDirection.SE];
                }

                // S
                neighbor = chunk.Neighbors[(int)NeighborDirection.S];

                return true;
            }

            if (localPosition.X >= 16)
            {
                if (localPosition.Z < 0)
                {
                    // NW
                    neighbor = chunk.Neighbors[(int)NeighborDirection.NW];
                }

                if (localPosition.Z >= 16)
                {
                    // NE
                    neighbor = chunk.Neighbors[(int)NeighborDirection.NE];
                }

                // N
                neighbor = chunk.Neighbors[(int)NeighborDirection.N];

                return true;
            }

            if (localPosition.Z < 0)
            {
                // W
                neighbor = chunk.Neighbors[(int)NeighborDirection.W];
                return true;
            }
            else if (localPosition.Z >= 16)
            {
                // E
                neighbor = chunk.Neighbors[(int)NeighborDirection.E];
                return true;
            }

            neighbor = null;

            return false;
        }

        // public static Vector3I GlobalizeLocalPosition(VoxelChunk chunk, Vector3I localPosition)
        // {
        //     return new Vector3I(chunk.ChunkUnitPosition.X * 16, localPosition.Y, chunk.ChunkUnitPosition.Y * 16);
        // }
        public static BlockRegistryItem GetBlock(VoxelChunk chunk, Vector3I localPosition)
        {
            int blockID = GetBlockID(chunk, localPosition);
            return BlockRegistry.GetItem(blockID);
        }
    }

    /// <summary>
    /// Used as indicies for neighbors array.
    /// </summary>
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

    /// <summary>
    /// Sets a block at a index within the chunk.
    /// </summary>
    /// <param name="chunkBlockIndex"></param>
    /// <param name="blockID"></param>
    /// <param name="blockVariant"></param>
    public void SetBlock(int chunkBlockIndex, int blockID, int blockVariant)
    {
        blockVariant = blockVariant << 4 * 3;
        UInt16 blockBytes = (UInt16)(blockVariant | blockID);
        Blocks[chunkBlockIndex] = blockBytes;
    }

    /// <summary>
    /// Sets a block at a local position within the chunk.
    /// </summary>
    /// <param name="localPosition"></param>
    /// <param name="blockID"></param>
    /// <param name="blockVariant"></param>
    public void SetBlock(Vector3I localPosition, int blockID, int blockVariant)
    {
        int chunkBlockIndex = localPosition.X;
        chunkBlockIndex += localPosition.Y * 256;
        chunkBlockIndex += localPosition.Z * 16;
        SetBlock(chunkBlockIndex, blockID, blockVariant);
    }

    /// <summary>
    /// Gets a neighbor of the chunk that is in a specific compass direction.
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
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

            if (blockID == 0) { return 0; }

            BlockVariant blockVariant = block.GetVariant(blockVariantID);
            BlockRegistryItem[] blockNeighbors = ChunkUtils.GetBlockNeighbors(chunk, new Vector3I(x, y, z));

            if (blockVariant is BlockVariantVoxel bvVoxel)
            {
                bool[] meshFaces = ChunkUtils.GetMeshFaces(chunk, block, new Vector3I(x, y, z), blockNeighbors);
                bool[] collisionFaces = ChunkUtils.GetMeshFaces(chunk, block, new Vector3I(x, y, z), blockNeighbors);

                if (block.RenderType != BlockRegistryItem.BlockRenderType.VOID)
                {
                    chunk.VMesh.AddVoxel(block.SurfaceID, new Vector3(x, y, z), bvVoxel.GenerateBlockUV(), VoxelTemplateData.COLOR, meshFaces);
                }

                if (block.CollisionType != BlockRegistryItem.BlockCollisionType.VOID)
                {
                    chunk.VCollision.AddVoxel(new Vector3(x, y, z), collisionFaces);
                }
            }
            else if (blockVariant is BlockVariantMesh bvMesh)
            {
                bool[] collisionFaces = ChunkUtils.GetCollisionFaces(chunk, block, new Vector3I(x, y, z), blockNeighbors);
                chunk.VMesh.AddMeshArrays(block.SurfaceID, bvMesh.MeshArrays, new Vector3(x, y, z), bvMesh.Rotation);
                chunk.VCollision.AddVoxel(new Vector3(x, y, z), collisionFaces);
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
        chunk.ChunkUnitPosition = chunkUnitPosition;
        chunk.Position = new Vector3(chunkUnitPosition.X * 16, 0, chunkUnitPosition.Y * 16);
        chunk.Name = Terrain.GetChunkName(chunkUnitPosition);
        return chunk;
    }
}
