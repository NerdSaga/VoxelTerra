using Godot;

namespace VoxelTerra.Scenes.CollisionObjects;

public partial class VoxelCollision : CollisionShape3D
{
    private static PackedScene SCENE = GD.Load<PackedScene>("uid://cd2tqtv7ku0nh");    
    public static VoxelCollision Create()
    {
        VoxelCollision instance = SCENE.Instantiate<VoxelCollision>();
        return instance;
    }
}