extends Node3D

var terrain = Terrain.INIT()
var chunks: Array[VoxelChunk] = []

func _ready() -> void:

	add_child(terrain)
	add_child(FreeCamera.CREATE(Vector3(0, 0, 5)))

	# var _variant = 5
	# var _block_id = 102
	# var block = (_variant << 4 * 3) | _block_id

	# var variant = block >> 4 * 3
	# var block_id = block & 0x00ff

	# print("variant: " + str(variant))
	# print("block_id: " + str(block_id))

	for y: int in range(4):
		for x: int in range(4):
			var chunk := Terrain.load_chunk(Vector2i(x, y))
			# Terrain.set_block_local(chunk, Vector3i(0, 0, 0), 1)
			chunks.append(chunk)

	var timer := Timer.new()
	timer.autostart = true
	timer.wait_time = 1
	timer.timeout.connect(set_block)
	add_child(timer)

	# Terrain.set_block_local(chunks[0], Vector3i(index, 0, 0), 1, 5)
	# Terrain.set_block_local(chunks[0], Vector3i(index, 0, 0), 102, 8)
	# Terrain.set_block_local(chunks[0], Vector3i(index, 0, 0), 23, 15)
	# Terrain.set_block_local(chunks[0], Vector3i(index, 0, 0), 1001, 1)


var index := 0
func set_block():
	# Terrain.set_block_local(chunks[0], Vector3i(index, 0, 0), 1)
	# Terrain.set_block_local(chunks[0], Vector3i(index, 1, 0), 1)
	for chunk in chunks:
		Terrain.set_block_local(chunk, Vector3i(index, 0, 0), 1)
		Terrain.set_block_local(chunk, Vector3i(index, 1, 0), 1)
	index += 1
