using Godot;
using System;
using VoxelTerra.CollisionObjects;
using VoxelTerra.MeshObjects;

namespace VoxelTerra.TerrainGeneration;

public partial class VoxelChunk : StaticBody3D
{
    public ushort[] Blocks {get;} = new ushort[16 * 16 * 256];
    public VoxelMesh VMesh;
    public VoxelCollision VCollision;

    public void SetBlock(int chunkBlockIndex, ushort block)
    {
        Blocks[chunkBlockIndex] = block;
    }

    public void SetBlock(Vector3I position, ushort block)
    {
        int chunkBlockIndex = position.X;
        chunkBlockIndex += position.Y * 256;
        chunkBlockIndex += position.Z * 16;
        SetBlock(chunkBlockIndex, block);
    }


    private bool readyToBuild = false;
    public bool IsBuilding = false;
    public void Build()
    {
        readyToBuild = true;
    }

    public override void _Ready()
    {
        VMesh = VoxelMesh.Create();
        AddChild(VMesh);

        VCollision = VoxelCollision.Create();
        AddChild(VCollision);

        Array.Fill<ushort>(Blocks, 0);
    }

    public override void _Process(double delta)
    {
        if (readyToBuild && !IsBuilding)
        {
            IsBuilding = true;
            readyToBuild = false;
            VoxelChunkBuilder.Instance.QueueJob(new VoxelChunkBuilderJob(this));
            
        }
    }


    private static PackedScene SCENE = GD.Load<PackedScene>("uid://druq8cym8v1jq");
    public static VoxelChunk Create()
    {
        VoxelChunk instance = SCENE.Instantiate<VoxelChunk>();
        return instance;
    }
}
