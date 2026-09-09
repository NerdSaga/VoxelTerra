


using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Godot;

namespace VoxelTerra.WorkerThreads;

public partial class Worker : Node
{
    
    Task task;
    protected ConcurrentQueue<WorkerJob> jobQueue = new();
    private volatile bool running = false;
    protected AutoResetEvent JobsAvailable = new(false);


    public void Work()
    {
        while (running)
        {
            JobsAvailable.WaitOne();

            if (!running) { continue; }

            while (jobQueue.TryDequeue(out WorkerJob job))
            {
                job.JobMain();
            }
        }
    }

    public override void _EnterTree()
    {
        running = true;
        task = Task.Run(Work);
    }

    public override async void _ExitTree()
    {
        running = false;
        JobsAvailable.Set();
        await task;
        task = null;
        JobsAvailable.Dispose();
    }

}