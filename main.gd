extends Node3D

var terrain = Terrain.INIT()
var chunks: Array[VoxelChunk] = []

func _ready() -> void:

	add_child(terrain)
	add_child(FreeCamera.CREATE(Vector3(0, 0, 5)))

	

	for y: int in range(4):
		for x: int in range(4):
			var chunk := Terrain.load_chunk(Vector2i(x, y))
			Terrain.set_block_local(chunk, Vector3i(0, 0, 0), 1)
			chunks.append(chunk)

	var timer := Timer.new()
	timer.autostart = true
	timer.wait_time = 1
	timer.timeout.connect(set_block)
	add_child(timer)


var index := 0
func set_block():
	for chunk in chunks:
		Terrain.set_block_local(chunk, Vector3i(index, 0, 0), 1)
	index += 1