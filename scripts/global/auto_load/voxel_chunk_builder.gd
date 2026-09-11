extends Node


# This class cannot run duplicate builds on chunks.


var _running = true
var _builder_tasks: Dictionary[String, _VoxelChunkBuilderTask] = {}
var _builder_tasks_mutex := Mutex.new()

class _VoxelChunkBuilderTask:

    var _task_id: int
    var _chunk: VoxelChunk
    var _update := Semaphore.new()


    func _working() -> void:


        VoxelChunkBuilder._builder_tasks_mutex.lock()
        var duplicate: _VoxelChunkBuilderTask = VoxelChunkBuilder._builder_tasks.get(_chunk.name)
        VoxelChunkBuilder._builder_tasks.set(_chunk.name, self)

        if duplicate != null:
            WorkerThreadPool.wait_for_task_completion(duplicate._task_id)
            if !VoxelChunkBuilder._running: return

        
        VoxelChunkBuilder._builder_tasks_mutex.unlock()

        call_deferred("_chunk_begin_build")
        _update.wait()

        if !VoxelChunkBuilder._running: return

        _chunk.build()

        call_deferred("_chunk_commit_build")
        _update.wait()

        if !VoxelChunkBuilder._running: return

        VoxelChunkBuilder._builder_tasks_mutex.lock()
        if VoxelChunkBuilder._builder_tasks.get(_chunk.name) != null:
            VoxelChunkBuilder._builder_tasks.erase(_chunk.name)
        VoxelChunkBuilder._builder_tasks_mutex.unlock()
    

    func _chunk_begin_build() -> void:
        if _chunk != null: _chunk.begin_build()
        _update.post()

    
    func _chunk_commit_build() -> void:
        if _chunk != null: _chunk.commit_build()
        _update.post()
    

    func queue(chunk: VoxelChunk) -> void:
        _chunk = chunk

        # VoxelChunkBuilder._builder_tasks_mutex.lock()
        # var duplicate: _VoxelChunkBuilderTask = VoxelChunkBuilder._builder_tasks.get(_chunk.name)
        # VoxelChunkBuilder._builder_tasks_mutex.unlock()

        # if duplicate != null:
        #     print("Found dupe")
        #     WorkerThreadPool.wait_for_task_completion(duplicate._task_id)
        
        # VoxelChunkBuilder._builder_tasks_mutex.lock()
        # VoxelChunkBuilder._builder_tasks.set(_chunk.name, self)
        # VoxelChunkBuilder._builder_tasks_mutex.unlock()

        _task_id = WorkerThreadPool.add_task(func(): self._working())


    func end():
        _update.post()
        WorkerThreadPool.wait_for_task_completion(_task_id)


func build_chunk(chunk: VoxelChunk) -> void:
    var task := _VoxelChunkBuilderTask.new()
    task.queue(chunk)


func _exit_tree() -> void:
    _running = false
    for chunk_name: String in _builder_tasks:
       _builder_tasks_mutex.lock()
       var task: _VoxelChunkBuilderTask = _builder_tasks.get(chunk_name)
       _builder_tasks_mutex.unlock()
       task.end()
       
    _builder_tasks.clear()