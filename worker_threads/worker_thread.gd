extends Object
class_name WorkerThread

var _thread := Thread.new()
var _mutex := Mutex.new()
var _update_worker := Semaphore.new()
var running = true

var _jobs: Array[WorkerJob] = []

func _working() -> void:
	while (running):
		_update_worker.wait()
		if (!running): continue
		
		_mutex.lock()
		while _jobs.size() > 0:
			var job: WorkerJob = _jobs.pop_front()
			job._work()
			
		_mutex.unlock()

func queue_job(job: WorkerJob) -> void:
	_mutex.lock()
	_jobs.append(job)
	_mutex.unlock()
	_update_worker.post()

func queue_jobs(jobs: Array[WorkerJob]) -> void:
	_mutex.lock()
	_jobs.append_array(jobs)
	_mutex.unlock()
	_update_worker.post()

func start() -> void:
	_thread.start(_working)

func stop() -> void:
	running = false
	_update_worker.post()
	_thread.wait_to_finish()
