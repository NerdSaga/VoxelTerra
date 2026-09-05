using Godot;
using System;

using VoxelTerra.Scenes.CollisionObjects;
using VoxelTerra.Scenes.MeshObjects;
using VoxelTerra.Scripts.Common;
public partial class Main : Node3D
{

    private MeshArrays monkeyMesh = MeshArrays.Load("uid://e8w8j77jlx44");
    private MeshArrays stairsMesh = MeshArrays.Load("uid://cak4bdu27if0a");
    public override void _Ready() {
        
        VoxelMesh mesh = VoxelMesh.Create();
        AddChild(mesh);

        mesh.Begin();
        
        mesh.AddMeshArrays(0, stairsMesh, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
        mesh.AddMeshArrays(1, stairsMesh, new Vector3(0, 2, 0), new Vector3(0, Mathf.Pi / 2.0f, 0));
        mesh.AddMeshArrays(0, stairsMesh, new Vector3(0, 4, 0), new Vector3(0, Mathf.Pi, 0));
        mesh.AddMeshArrays(1, stairsMesh, new Vector3(0, 6, 0), new Vector3(0, Mathf.Pi + Mathf.Pi / 2.0f, 0));
        mesh.AddVoxel(0, new Vector3(2, 0, 0), VoxelTemplateData.UV, new bool[] {false, true, true, true, true, true});
        mesh.AddVoxel(1, new Vector3(4, 0, 0), VoxelTemplateData.UV, new bool[] {true, false, true, true, true, true});
        mesh.AddVoxel(0, new Vector3(6, 0, 0), VoxelTemplateData.UV, new bool[] {true, true, false, true, true, true});
        mesh.AddVoxel(1, new Vector3(8, 0, 0), VoxelTemplateData.UV, new bool[] {true, true, true, false, true, true});
        mesh.AddVoxel(0, new Vector3(10, 0, 0), VoxelTemplateData.UV, new bool[] {true, true, true, true, false, true});
        mesh.AddVoxel(1, new Vector3(12, 0, 0), VoxelTemplateData.UV, new bool[] {true, true, true, true, false, false});
        
        mesh.Commit();
    }
}
