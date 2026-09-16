using Godot;

namespace VoxelTerra.Common;

public static class Rotations
{
    public static float R0 {get;} = 0.0f;
    public static float R180 {get;} = Mathf.Pi;
    public static float R90 {get;} = Mathf.Pi / 2.0f;
    public static float R270 {get;} = Mathf.Pi / 2.0f + Mathf.Pi;
}