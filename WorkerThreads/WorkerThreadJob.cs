
namespace VoxelTerra.WorkerThreads;

public abstract class WorkerThreadJob
{
    public abstract void PreJob();

    public abstract void MainJob();

    public abstract void PostJob();
}