using Godot;
using VoxelTerra.Voxels;

namespace VoxelTerra.Registries;

public class BlockVariant
{
    /// <summary>
    /// Used by block faces to map textures to block atlas texture tiles. This should remain as six elements.
    /// </summary>
    
    public enum BlockRenderType
    {
        VOID,
        SOLID,
        TRANSPARENT,
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

    public class BlockFaceAtlasTilePositions
    {
        public Vector2I Top = Vector2I.Zero;
        public Vector2I Bottom = Vector2I.Zero;
        public Vector2I North = Vector2I.Zero;
        public Vector2I South = Vector2I.Zero;
        public Vector2I East = Vector2I.Zero;
        public Vector2I West = Vector2I.Zero;

        public Vector2[] ToArray()
        {
            return new Vector2[]
            {
                Top,
                Bottom,
                North,
                South,
                East,
                West,
            };
        }
    }
    public BlockFaceAtlasTilePositions FaceAtlasTilePositions = new BlockFaceAtlasTilePositions();

    public virtual Vector2[] GenerateBlockUV()
    {
        Vector2[] uv = new Vector2[24];
        Vector2[] atlasTilePositions = FaceAtlasTilePositions.ToArray();

        for (int i = 0; i < 6; i++)
        {
            int index = i * 4;
            Vector2 offset = atlasTilePositions[i];
            uv[index + 0] = VoxelTemplateData.UV[index + 0] + offset;
            uv[index + 1] = VoxelTemplateData.UV[index + 1] + offset;
            uv[index + 2] = VoxelTemplateData.UV[index + 2] + offset;
            uv[index + 3] = VoxelTemplateData.UV[index + 3] + offset;
        }

        return uv;
    }
}