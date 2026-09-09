extends WorkerJob
class_name TerrainModJob

var should_continute := Semaphore.new()
var chunk: VoxelChunk

func _init(chunk: VoxelChunk) -> void:
	self.chunk = chunk

func _work() -> void:
	call_deferred("_on_start")
	should_continute.wait()
	if chunk == null: return
	chunk.build()
	call_deferred("_on_finish")

func _on_start() -> void:
	if chunk == null: 
		should_continute.post()
		return
	
	chunk.begin_build()
	should_continute.post()

func _on_finish():
	if chunk == null:
		should_continute.post()
		return
	
	chunk.commit_build()
	should_continute.post()
