using Godot;
using System;

namespace VoxelTerra.Scenes.MeshObjects;

public partial class VoxelMesh : MeshInstance3D
{
    
    private static PackedScene SCENE = GD.Load<PackedScene>("uid://ce22dvs8fbrk8");
    public static VoxelMesh Create()
    {
        VoxelMesh instance = SCENE.Instantiate<VoxelMesh>();
        return instance;
    }
}
