


using System.Collections.Concurrent;
using System.Threading;
using Godot;

namespace VoxelTerra.WorkerThreads;

public partial class WorkerThread : Node
{
    
    Thread thread;
    protected ConcurrentQueue<WorkerThreadJob> jobQueue = new();
    private volatile bool running = false;
    protected AutoResetEvent JobsAvailable = new(false);


    public void Work()
    {
        while (running)
        {
            JobsAvailable.WaitOne();

            if (!running) { continue; }

            while (jobQueue.TryDequeue(out WorkerThreadJob job))
            {
                job.JobMain();
            }
        }
    }

    public override void _EnterTree()
    {
        running = true;
        thread = new(Work);
        thread.Start();
    }

    public override void _ExitTree()
    {
        running = false;
        JobsAvailable.Set();
        thread.Join();
        thread = null;
        JobsAvailable.Dispose();
    }

}