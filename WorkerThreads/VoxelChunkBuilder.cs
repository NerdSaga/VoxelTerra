public partial class VoxelChunkBuilder : WorkerThread
{
    public static VoxelChunkBuilder Instance;

    public override void _EnterTree()
    {
        base._EnterTree();

        Instance = this;
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        Instance = null;
    }

    public VoxelChunkBuilder(string name) : base(name) {}
}