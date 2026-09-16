using Godot;

namespace VoxelTerra.Common;

public static class Directions
{
    public static Vector3 UP {get;} = new Vector3(0, 1, 0);
    public static Vector3 DOWN {get;} = new Vector3(0, -1, 0);
    public static Vector3 NORTH {get;} = new Vector3(1, 0, 0);
    public static Vector3 SOUTH {get;} = new Vector3(-1, 0, 0);
    public static Vector3 EAST {get;} = new Vector3(0, 0, 1);
    public static Vector3 WEST {get;} = new Vector3(0, 0, -1);
}