using System.Dynamic;
using Godot;

namespace VoxelTerra.MeshObjects;

public class MeshArrays
{
    
    public Vector3[] VERTEX;
    public Vector3[] NORMAL;
    public Vector2[] UV;
    public int[] INDEX;
    public bool Deindexed {get; set;} = false;

    public static MeshArrays Load(string path, bool deindex = false)
    {
        MeshArrays meshArrays = new();

        Mesh mesh = GD.Load<Mesh>(path);
        
        if (deindex)
        {
            SurfaceTool st = new();
            st.CreateFrom(mesh, 0);
            st.Deindex();
            mesh = st.Commit();
            meshArrays.Deindexed = true;
        }

        Godot.Collections.Array arrays = mesh.SurfaceGetArrays(0);

        
        meshArrays.VERTEX = (Vector3[])arrays[(int)ArrayMesh.ArrayType.Vertex];
        meshArrays.NORMAL = (Vector3[])arrays[(int)ArrayMesh.ArrayType.Normal];
        meshArrays.UV = (Vector2[])arrays[(int)ArrayMesh.ArrayType.TexUV];
        meshArrays.INDEX = (int[])arrays[(int)ArrayMesh.ArrayType.Index];

        mesh = null;
        return meshArrays;
    }
}