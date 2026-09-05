using Godot;
using System;

using VoxelTerra.Scenes.CollisionObjects;
using VoxelTerra.Scenes.MeshObjects;
using VoxelTerra.Scripts.Common;
public partial class Main : Node3D
{

    private MeshArrays monkeyMesh = MeshArrays.Load("uid://e8w8j77jlx44");
    public override void _Ready() {
        
        VoxelMesh mesh = VoxelMesh.Create();
        AddChild(mesh);

        mesh.Begin();
        
        mesh.AddMesh(0, monkeyMesh, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
        mesh.AddMesh(0, monkeyMesh, new Vector3(0, 2, 0), new Vector3(1, 0, 0));
        mesh.AddMesh(0, monkeyMesh, new Vector3(0, 4, 0), new Vector3(0, 1, 0));
        mesh.AddMesh(0, monkeyMesh, new Vector3(0, 6, 0), new Vector3(1, 0, 1));
        mesh.AddVoxel(0, new Vector3(0, 0, 0), VoxelTemplateData.UV, new bool[] {true, true, true, true, true, true});
        
        mesh.Commit();
    }
}
