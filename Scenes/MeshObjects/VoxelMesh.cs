using Godot;
using System;
using VoxelTerra.Scripts.Common;

namespace VoxelTerra.Scenes.MeshObjects;

public partial class VoxelMesh : MeshInstance3D
{

    [Export] private Material[] materials;
    private SurfaceTool[] surfaces;
    private int[] surfaceVertexCounts;
    private ArrayMesh mesh = new();

    public void Begin()
    {
        mesh.ClearSurfaces();
        for (int i = 0; i < surfaces.Length; i++)
        {
            SurfaceTool surface = surfaces[i];
            surface.Clear();
            surface.Begin(Mesh.PrimitiveType.Triangles);
            surface.SetMaterial(materials[i]);
        }
    }

    public void Commit()
    {

        for (int i = 0; i < surfaces.Length; i++)
        {
            SurfaceTool surface = surfaces[i];
            // surface.Index();
            surface.Commit(mesh);
        }
    }

    /// <summary>
    /// Sets a mesh to a local position on the mesh.
    /// </summary>
    /// <param name="surfaceID"></param>
    /// <param name="meshData"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    public void AddMeshArrays(int surfaceID, MeshArrays meshData, Vector3 position, Vector3 rotation)
    {

        SurfaceTool surface = surfaces[surfaceID];

        for (int i = 0; i < meshData.VERTEX.Length; i++)
        {
            surface.SetNormal(meshData.NORMAL[i]);
            surface.SetUV(meshData.UV[i]);

            Vector3 vertex = meshData.VERTEX[i];
            Transform3D transform = Transform3D.Identity;
            transform = transform.Rotated(new Vector3(1, 0, 0), rotation.X);
            transform = transform.Rotated(new Vector3(0, 1, 0), rotation.Y);
            transform = transform.Rotated(new Vector3(0, 0, 1), rotation.Z);
            vertex *= transform;
            surface.AddVertex(vertex + position + new Vector3(0.5f, 0.5f, 0.5f));
        }

        for (int i = 0; i < meshData.INDEX.Length; i++)
        {
            surface.AddIndex(meshData.INDEX[i] + surfaceVertexCounts[surfaceID]);
        }

        surfaceVertexCounts[surfaceID] += meshData.VERTEX.Length;
    }

    /// <summary>
    /// Sets a single voxel to a local position on the mesh.
    /// </summary>
    /// <param name="surfaceID"></param>
    /// <param name="position"></param>
    /// <param name="uv"></param>
    /// <param name="faces"></param>
    public void AddVoxel(int surfaceID, Vector3 position, Vector2[] uv, bool[] faces)
    {
        SurfaceTool surface = surfaces[surfaceID];
        int nextIndex = surfaceVertexCounts[surfaceID];

        for (int i = 0; i < 6; i++)
        {
            if (faces[i] == false)
            {
                continue;
            }

            surface.SetNormal(VoxelTemplateData.NORMAL[0 + i * 4]);
            surface.SetUV(VoxelTemplateData.UV[0 + i * 4]);
            surface.AddVertex(VoxelTemplateData.VERTEX[0 + i * 4] + position);

            surface.SetUV(VoxelTemplateData.UV[1 + i * 4]);
            surface.AddVertex(VoxelTemplateData.VERTEX[1 + i * 4] + position);

            surface.SetUV(VoxelTemplateData.UV[2 + i * 4]);
            surface.AddVertex(VoxelTemplateData.VERTEX[2 + i * 4] + position);

            surface.SetUV(VoxelTemplateData.UV[3 + i * 4]);
            surface.AddVertex(VoxelTemplateData.VERTEX[3 + i * 4] + position);
            surface.AddIndex(nextIndex + 0);
            surface.AddIndex(nextIndex + 1);
            surface.AddIndex(nextIndex + 2);
            surface.AddIndex(nextIndex + 2);
            surface.AddIndex(nextIndex + 1);
            surface.AddIndex(nextIndex + 3);

            nextIndex += 4;
        }
        surfaceVertexCounts[surfaceID] = nextIndex;
    }
    
    private static PackedScene SCENE = GD.Load<PackedScene>("uid://ce22dvs8fbrk8");
    public static VoxelMesh Create()
    {
        VoxelMesh instance = SCENE.Instantiate<VoxelMesh>();
        return instance;
    }

    public override void _Ready()
    {
        // Populate surfaces array.
        surfaces = new SurfaceTool[materials.Length];
        for (int i = 0; i < surfaces.Length; i++)
        {
            surfaces[i] = new();
        }

        // Populate surface vertex counts
        surfaceVertexCounts = new int[surfaces.Length];
        Array.Fill<int>(surfaceVertexCounts, 0);

        // Set mesh to the custom array mesh.
        Mesh = mesh;
    }

}
