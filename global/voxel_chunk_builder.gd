extends Object
class_name VoxelChunkBuilder

static var _instance: VoxelChunkBuilder
static var _running := true
static var _working_tasks: Dictionary = {}
static var _working_tasks_mutex := Mutex.new()

static var _working_chunks: Dictionary = {}
static var _working_chunks_mutex := Mutex.new()

static var _working_tasks_obj: Dictionary = {}
static var _working_tasks_obj_mutex := Mutex.new()

static func build_chunk(chunk: VoxelChunk) -> void:

    _working_tasks_mutex.lock()
    _working_chunks_mutex.lock()

    var duplicate_task_id = _working_chunks.get(chunk.name)

    if duplicate_task_id != null: # If the chunk is already being worked.
        WorkerThreadPool.wait_for_task_completion(duplicate_task_id) # Wait for the chunk to be finished.

    _working_tasks_obj_mutex.lock()
    # Remember the newly created task.
    var task_obj := _VoxelChunkBuilderTask.new(chunk)
    var task_id: int = WorkerThreadPool.add_task(func(): task_obj._working())
    _working_chunks.set(chunk.name, task_id)
    _working_tasks.set(str(task_id), task_id)
    _working_tasks_obj.set(str(task_id), task_obj)

    _working_tasks_obj_mutex.unlock()
    _working_chunks_mutex.unlock()
    _working_tasks_mutex.unlock()

static func START() -> void:
    _instance = VoxelChunkBuilder.new()

static func END() -> void:
    _running = false

    for task_name: String in _working_tasks:
        _working_tasks_mutex.lock()
        var task_id: int = _working_tasks.get(task_name)
        _working_tasks_mutex.unlock()

        _working_tasks_obj_mutex.lock()
        var task_obj: _VoxelChunkBuilderTask = _working_tasks_obj.get(task_name)
        _working_tasks_obj_mutex.unlock()
        task_obj._chunk_build_continue.post()
        
        WorkerThreadPool.wait_for_task_completion(task_id)
    
    _instance = null


class _VoxelChunkBuilderTask:

    var _chunk: VoxelChunk
    var _chunk_build_continue := Semaphore.new()

    func _working() -> void:
        call_deferred("_chunk_begin_build")
        _chunk_build_continue.wait()
        if VoxelChunkBuilder._running != true: return

        _chunk.build()

        call_deferred("_chunk_commit_build")
        _chunk_build_continue.wait()

        VoxelChunkBuilder._working_chunks_mutex.lock()
        VoxelChunkBuilder._working_chunks.erase(_chunk.name)
        VoxelChunkBuilder._working_chunks_mutex.unlock()

        VoxelChunkBuilder._working_tasks_mutex.lock()
        var task_id = WorkerThreadPool.get_caller_task_id()
        VoxelChunkBuilder._working_tasks.erase(str(task_id))
        VoxelChunkBuilder._working_tasks_mutex.unlock()

        VoxelChunkBuilder._working_tasks_obj_mutex.lock()
        VoxelChunkBuilder._working_tasks_obj.erase(str(task_id))
        VoxelChunkBuilder._working_tasks_obj_mutex.unlock()

    func _chunk_begin_build() -> void:
        if _chunk != null:
            _chunk.begin_build()
        _chunk_build_continue.post()


    func _chunk_commit_build() -> void:
        if _chunk != null:
            _chunk.commit_build()
        _chunk_build_continue.post()
    
    func _init(chunk: VoxelChunk) -> void:
        _chunk = chunk





