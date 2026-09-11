extends Node3D
class_name Terrain

static var _instance: Terrain

static func set_block_local(chunk: VoxelChunk, local_position: Vector3i, block_id: int, variant: int = 0) -> void:
	var chunk_block_index := local_position.x
	chunk_block_index += local_position.y * 256
	chunk_block_index += local_position.z * 16

	var block = (variant << 4 * 3) & block_id

	# var v = block >> 4 * 3
	# var b = block & 0x00ff
	# print("v = " + str(v) + ", b = " + str(b))

	chunk.set_block(chunk_block_index, block_id)

	chunk.begin_build()
	chunk.build()
	chunk.commit_build()
	# VoxelChunkBuilder.build_chunk(chunk)


static func load_chunk(chunk_unit_position: Vector2i) -> VoxelChunk:
	var chunk = VoxelChunk.CREATE(chunk_unit_position)
	_instance.add_child(chunk)
	return chunk


static func INIT() -> Terrain:

	if _instance != null:
		push_error("Created multiple instances of Terrain")

	var terrain := Terrain.new()
	_instance = terrain
	# VoxelChunkBuilder.START()
	return terrain



func _enter_tree() -> void:
	name = "Terrain"

func _exit_tree() -> void:
	pass
	# VoxelChunkBuilder.END()




# static var running = true
# static var _instance: TerrainMod
# static var _tasks: Dictionary = {}
# static var _tasks_mutex := Mutex.new()
# static var _mutex := Mutex.new()

# static func start() -> TerrainMod:
# 	_instance = TerrainMod.new()
# 	running = true
# 	return _instance


# static func set_block_local(chunk: VoxelChunk, local_position: Vector3, block: int) -> void:
# 	var task := WorkerThreadPool.add_task(func(): _set_block_local(chunk, local_position, block))

# 	_tasks_mutex.lock()
# 	_tasks.set(str(task), task)
# 	_tasks_mutex.unlock()

# static func _set_block_local(chunk: VoxelChunk, local_position: Vector3i, block: int) -> void:
# 	var chunk_block_index := local_position.x
# 	chunk_block_index += local_position.y * 256
# 	chunk_block_index += local_position.z * 16

# 	_mutex.lock()
# 	chunk.set_block(chunk_block_index, block)
# 	_instance._chunk_build(chunk)
# 	_mutex.unlock()
# 	_end_task()


# static func _end_task():
# 	_tasks_mutex.lock()
# 	var task_id = WorkerThreadPool.get_caller_task_id()
# 	_tasks.erase(str(task_id))
# 	_tasks_mutex.unlock()


# func _exit_tree() -> void:
# 	running = false
# 	_chunk_build_continue.post()
# 	for task: String in _tasks:
# 		WorkerThreadPool.wait_for_task_completion(_tasks.get(task))






# static var _chunk_build_continue := Semaphore.new()


# func _chunk_build(chunk: VoxelChunk) -> void:
# 	call_deferred("_chunk_begin_build", chunk)
# 	_chunk_build_continue.wait()
# 	if !running: return

# 	chunk.build()

# 	call_deferred("_chunk_commit_build", chunk)
# 	_chunk_build_continue.wait()
	

# func _chunk_begin_build(chunk: VoxelChunk) -> void:
# 	chunk.begin_build()
# 	_chunk_build_continue.post()


# func _chunk_commit_build(chunk: VoxelChunk) -> void:
# 	chunk.commit_build()
# 	_chunk_build_continue.post()