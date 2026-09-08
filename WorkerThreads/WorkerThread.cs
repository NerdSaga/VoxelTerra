using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Godot;
using VoxelTerra.Debugging;

namespace VoxelTerra.WorkerThreads;

public partial class WorkerThread : Node
{

    private volatile bool running = false;
    private Thread thread;
    public string Name;

    protected ConcurrentQueue<WorkerThreadJob> preJobQueue = new();
    protected ConcurrentQueue<WorkerThreadJob> mainJobQueue = new();
    protected ConcurrentQueue<WorkerThreadJob> postJobQueue = new();
    protected AutoResetEvent jobAvailable = new(false);

    public void QueueJob(WorkerThreadJob job)
    {
        preJobQueue.Enqueue(job);
    }

    public override void _Process(double delta)
    {
        // if (!mainJobQueue.IsEmpty)
        // {
        //     return;
        // }

        if (!postJobQueue.IsEmpty)
        {
            doJobs(postJobQueue, 2);
        }

        if (!preJobQueue.IsEmpty)
        {
            doJobs(preJobQueue, 0);
        }
    }

    private void doJobs(ConcurrentQueue<WorkerThreadJob> jobs, int jobStage)
    {
        bool working = true;
        List<WorkerThreadJob> newMainJobs = new();
        while (working)
        {
            working = jobs.TryDequeue(out WorkerThreadJob job);

            if (!working)
            {
                continue;
            }

            switch (jobStage)
            {
                case 0:
                    job.PreJob();
                    mainJobQueue.Enqueue(job);
                    jobAvailable.Set();
                    break;

                case 1:
                    // job.MainJob();
                    // postJobQueue.Enqueue(job);
                    break;
                
                case 2:
                    job.PostJob();
                    break;
            }
        }
    }

    private void _OnThreadRun()
    {
        while (running)
        {
            jobAvailable.WaitOne();
            
            if (!running)
            {
                continue;
            }

            while (mainJobQueue.TryDequeue(out WorkerThreadJob job))
            {
                job.MainJob();
                postJobQueue.Enqueue(job);
            }
        }
    }

    public override void _EnterTree()
    {
        running = true;
        thread = new(_OnThreadRun);
        thread.Name = Name;
        thread.Start();
    }

    public override void _ExitTree()
    {
        running = false;
        jobAvailable.Set();
        thread.Join();
        thread = null;
        jobAvailable.Dispose();
    }

    public WorkerThread(string name = "thread_default_name")
    {
        this.Name = name;
    }
}