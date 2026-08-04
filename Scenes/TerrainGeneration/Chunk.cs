using Godot;
using System;

namespace VoxelTerra.Scenes.TerrainGeneration;

public partial class Chunk : StaticBody3D
{
    private static PackedScene SCENE = GD.Load<PackedScene>("uid://druq8cym8v1jq");
    public static Chunk Create()
    {
        Chunk instance = SCENE.Instantiate<Chunk>();
        return instance;
    }
}
