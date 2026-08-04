using Godot;
using System;

using VoxelTerra.Scenes.CollisionObjects;
using VoxelTerra.Scenes.MeshObjects;

public partial class Main : Node3D
{

    public override void _Ready() {
        
        VoxelMesh mesh = VoxelMesh.Create();
        AddChild(mesh);

        VoxelCollision collision = VoxelCollision.Create();
        AddChild(collision);
    }
}
