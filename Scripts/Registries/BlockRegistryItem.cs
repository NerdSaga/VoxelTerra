using System;
using Godot;
using VoxelTerra.Debugging;

namespace VoxelTerra.Registries;


public abstract class BlockRegistryItem : VTRegistryItem
{
    public int SurfaceID = 0;
    private BlockVariant[] variants = new BlockVariant[] {new BlockVariant()};

    /// <summary>
    /// Determines which block faces will be included in a voxel mesh when it is built.
    /// </summary>
    public enum BlockRenderType
    {
        /// <summary>
        /// The block will never render any faces.
        /// </summary>
        VOID,

        /// <summary>
        /// The block will hide faces from blocks that are also solid.
        /// </summary>
        SOLID,

        /// <summary>
        /// The block will hide faces from blocks with the same ID, are solid, and are not set to ALL_FACES.
        /// </summary>
        TRANSPARENT,

        /// <summary>
        /// The block will always render all faces.
        /// </summary>
        ALL_FACES,
    }
    public BlockRenderType RenderType = BlockRenderType.VOID;

    public enum BlockCollisionType
    {
        VOID,
        SOLID,
        TRANSPARENT,
        ALL_FACES,
    }
    public BlockCollisionType CollisionType = BlockCollisionType.VOID;

    public void SetVariants(BlockVariant[] blockVariants)
    {
        variants = blockVariants;
    }

    public BlockVariant GetVariant(int variantIndex)
    {
        return variants[variantIndex];
    }

    public BlockRegistryItem(string itemName) : base(itemName)
    {
    }
}