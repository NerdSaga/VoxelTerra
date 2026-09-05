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

    public void AddMesh(int surfaceID, MeshArrays meshData, Vector3 position, Vector3 rotation)
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

    public void IndexSurfaces()
    {
        
        for (int i = 0; i < surfaces.Length; i++)
        {
            SurfaceTool surface = surfaces[i];
            surface.Index();
        }
    }

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

            

            // surface.AddTriangleFan(
            //     vertices: new Vector3[]
            //     {
            //         VoxelTemplateData.VERTEX[0 + i * 4] + position,
            //         VoxelTemplateData.VERTEX[1 + i * 4] + position,
            //         VoxelTemplateData.VERTEX[2 + i * 4] + position,
            //         VoxelTemplateData.VERTEX[3 + i * 4] + position,
            //     },
            //     normals: new Vector3[]
            //     {
            //         VoxelTemplateData.NORMAL[0 + i * 4],
            //         VoxelTemplateData.NORMAL[1 + i * 4],
            //         VoxelTemplateData.NORMAL[2 + i * 4],
            //         VoxelTemplateData.NORMAL[3 + i * 4],
            //     },
            //     uvs: new Vector2[]
            //     {
            //         uv[0 + i * 4],
            //         uv[1 + i * 4],
            //         uv[2 + i * 4],
            //         uv[3 + i * 4],
            //     }
            // );
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

        surfaceVertexCounts = new int[surfaces.Length];
        Array.Fill<int>(surfaceVertexCounts, 0);

        Mesh = mesh;
    }

}
