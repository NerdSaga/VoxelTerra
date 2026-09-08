using System;
using System.Collections.Generic;
using Godot;
using VoxelTerra.MeshObjects;
using VoxelTerra.Common;

namespace VoxelTerra.CollisionObjects;

public partial class VoxelCollision : CollisionShape3D
{

    private ConcavePolygonShape3D shape;
    private List<Vector3> points = new();

    public void Begin()
    {
        points.Clear();
    }

    public void Commit()
    {
        shape.SetFaces(points.ToArray());
    }

    public void AddMeshArrays(MeshArrays meshData, Vector3 position, Vector3 rotation)
    {
        if (meshData.Deindexed == false)
        {
            throw new Exception("meshData.Deindexed == false, VoxelCollision.AddMeshArrays() requires meshData to be indexed.");
        }

        for (int i = 0; i < meshData.VERTEX.Length; i++)
        {
            Vector3 vertex = meshData.VERTEX[i];
            Transform3D transform = Transform3D.Identity;
            transform = transform.Rotated(new Vector3(1, 0, 0), rotation.X);
            transform = transform.Rotated(new Vector3(0, 1, 0), rotation.Y);
            transform = transform.Rotated(new Vector3(0, 0, 1), rotation.Z);
            vertex *= transform;
            points.Add(vertex + position + new Vector3(0.5f, 0.5f, 0.5f));
        }
    }

    public void AddVoxel(Vector3 position, bool[] faces)
    {
        
        for (int i = 0; i < 6; i++)
        {
            if (faces[i] == false)
            {
                continue;
            }
            
            points.Add(VoxelTemplateData.VERTEX[0 + i * 4] + position);
            points.Add(VoxelTemplateData.VERTEX[1 + i * 4] + position);
            points.Add(VoxelTemplateData.VERTEX[2 + i * 4] + position);
            points.Add(VoxelTemplateData.VERTEX[2 + i * 4] + position);
            points.Add(VoxelTemplateData.VERTEX[1 + i * 4] + position);
            points.Add(VoxelTemplateData.VERTEX[3 + i * 4] + position);
        }
    }

    public override void _Ready()
    {
        shape = new ConcavePolygonShape3D();
        Shape = shape;
    }

    private static PackedScene SCENE = GD.Load<PackedScene>("uid://cd2tqtv7ku0nh"); 
    public static VoxelCollision Create()
    {
        VoxelCollision instance = SCENE.Instantiate<VoxelCollision>();
        return instance;
    }
}