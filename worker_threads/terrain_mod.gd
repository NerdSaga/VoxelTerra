extends WorkerThread
class_name TerrainMod

static var instance = self

func set_block_local(chunk: VoxelChunk, local_position: Vector3, block: int):
	
	var chunk_block_index := local_position.x
	chunk_block_index += local_position.y * 256
	chunk_block_index += local_position.z * 16
	
	chunk.set_block(chunk_block_index, block)
	
	var job := TerrainModJob.new(chunk)	
	queue_job(job)
