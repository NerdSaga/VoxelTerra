using System.Dynamic;
using Godot;

namespace VoxelTerra.Scripts.Common;

public class MeshArrays
{
    
    public Vector3[] VERTEX;
    public Vector3[] NORMAL;
    public Vector2[] UV;
    public int[] INDEX;

    public static MeshArrays Load(string path)
    {
        Mesh mesh = GD.Load<Mesh>(path);
        Godot.Collections.Array arrays = mesh.SurfaceGetArrays(0);
        

        MeshArrays meshArrays = new();
        meshArrays.VERTEX = (Vector3[])arrays[(int)ArrayMesh.ArrayType.Vertex];
        meshArrays.NORMAL = (Vector3[])arrays[(int)ArrayMesh.ArrayType.Normal];
        meshArrays.UV = (Vector2[])arrays[(int)ArrayMesh.ArrayType.TexUV];
        meshArrays.INDEX = (int[])arrays[(int)ArrayMesh.ArrayType.Index];

        mesh = null;
        return meshArrays;
    }
}