using Godot;
using System;
using System.Collections.Generic;
using VoxelTerra.TerrainGeneration;
using VoxelTerra.TerrainGeneration.Generators;
using VoxelTerra.Voxels;

public partial class World : Node3D
{
	[Export] int Seed = 0;
	[Export] int WorldSize = 4;
	List<VoxelChunk> chunks = new();
	public override void _Ready()
	{
		Terrain.Configure(new TerrainVoxelTerra(), Seed);

		for (int z = -WorldSize; z < WorldSize; z++)
		{
			for (int x = -WorldSize; x < WorldSize; x++)
			{
				VoxelChunk chunk = Terrain.LoadChunk(new Vector2I(x, z));
				Terrain.GenerateChunkTerrain(chunk);
				chunks.Add(chunk);
			}
		}

		foreach (VoxelChunk chunk in chunks)
		{
			Terrain.BuildChunk(chunk);
		}
	}

	private static PackedScene SCENE = GD.Load<PackedScene>("uid://dhmh3jhlkub6y");
	public static World Create(int seed)
	{
		World world = SCENE.Instantiate<World>();
		world.Seed = seed;
		return world;
	}

}
