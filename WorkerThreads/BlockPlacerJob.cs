// using System;
// using Godot;
// using VoxelTerra.Common;
// using VoxelTerra.TerrainGeneration;

// namespace VoxelTerra.WorkerThreads;

// public class BlockPlacerJob : WorkerJob
// {
//     private VoxelChunk chunk;

//     public override void JobMain()
//     {
//         chunk.Build();

//         chunk.OnFinishBuild.Set();
//     }

//     public BlockPlacerJob(VoxelChunk chunk)
//     {
//         this.chunk = chunk;
//     }
// }