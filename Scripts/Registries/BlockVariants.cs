using Godot;
using VoxelTerra.Voxels;

namespace VoxelTerra.Registries;

public class BlockVariant
{

}

public class BlockVariantVoxel : BlockVariant
{
    /// <summary>
    /// Used by block faces to map textures to block atlas texture tiles. This should remain as six elements.
    /// </summary>

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

    public class BlockFaceAtlasTileRotations
    {
        public float Top = 0;
        public float Bottom = 0;
        public float North = 0;
        public float South = 0;
        public float East = 0;
        public float West = 0;

        public float[] ToArray()
        {
            return new float[]
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

    public BlockFaceAtlasTileRotations FaceAtlasTileRotations = new();

    public virtual Vector2[] GenerateBlockUV()
    {
        Vector2[] uv = new Vector2[24];
        Vector2[] atlasTilePositions = FaceAtlasTilePositions.ToArray();
        float[] atlasTileRotations = FaceAtlasTileRotations.ToArray();

        for (int i = 0; i < 6; i++)
        {
            int index = i * 4;
            Vector2 offset = atlasTilePositions[i];

            uv[index + 0] = VoxelTemplateData.UV[index + 0] - new Vector2(0.5f, 0.5f);
            uv[index + 1] = VoxelTemplateData.UV[index + 1] - new Vector2(0.5f, 0.5f);
            uv[index + 2] = VoxelTemplateData.UV[index + 2] - new Vector2(0.5f, 0.5f);
            uv[index + 3] = VoxelTemplateData.UV[index + 3] - new Vector2(0.5f, 0.5f);

            uv[index + 0] = uv[index + 0].Rotated(atlasTileRotations[i]) + new Vector2(0.5f, 0.5f);
            uv[index + 1] = uv[index + 1].Rotated(atlasTileRotations[i]) + new Vector2(0.5f, 0.5f);
            uv[index + 2] = uv[index + 2].Rotated(atlasTileRotations[i]) + new Vector2(0.5f, 0.5f);
            uv[index + 3] = uv[index + 3].Rotated(atlasTileRotations[i]) + new Vector2(0.5f, 0.5f);

            uv[index + 0] += offset;
            uv[index + 1] += offset;
            uv[index + 2] += offset;
            uv[index + 3] += offset;
        }

        return uv;
    }
}

public class BlockVariantMesh : BlockVariant
{
    public MeshArrays MeshArrays;

    public Vector3 Rotation = Vector3.Zero;
}