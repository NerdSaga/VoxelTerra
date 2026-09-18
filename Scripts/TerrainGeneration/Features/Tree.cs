using Godot;
using VoxelTerra.Registries;
using VoxelTerra.Voxels;

namespace VoxelTerra.TerrainGeneration.Features;

public class Tree : TerrainFeature
{
    int mapleLog = BlockRegistry.GetItem(Registries.Blocks.MapleLog.ItemName).ID;
    int mapleLeaves = BlockRegistry.GetItem(Registries.Blocks.MapleLeaves.ItemName).ID;
    int monkeyBlock = BlockRegistry.GetItem(Registries.Blocks.MonkeyBlock.ItemName).ID;
    public override void Build(VoxelChunk chunk, Vector3I localPosition)
    {
        setBlock(chunk, localPosition, mapleLog, 0);
        setBlock(chunk, localPosition + new Vector3I(0, 1, 0), mapleLog, 0);
        setBlock(chunk, localPosition + new Vector3I(0, 2, 0), mapleLog, 0);
        setBlock(chunk, localPosition + new Vector3I(0, 3, 0), mapleLog, 0);
        setBlock(chunk, localPosition + new Vector3I(0, 4, 0), mapleLog, 0);
        // setBlock(chunk, localPosition + new Vector3I(0, 5, 0), mapleLeaves, 0);

        fillBlocks(chunk, localPosition + new Vector3I(-2, 5, -2), localPosition + new Vector3I(2, 6, 2), mapleLeaves, 0);
        fillBlocks(chunk, localPosition + new Vector3I(-1, 7, -1), localPosition + new Vector3I(1, 8, 1), mapleLeaves, 0);

        // setBlock(chunk, localPosition + new Vector3I(0, 10, 0), monkeyBlock, 0);
    }
}