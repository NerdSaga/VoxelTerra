extends Node3D

var chunk = VoxelChunk.CREATE()
var terr = TerrainMod.new()

func _ready() -> void:
	var player := FreeCamera.CREATE(Vector3(0, 0, 10))
	add_child(player)
	
	terr.start()
	
	
	add_child(chunk)
	
	
	var timer := Timer.new()
	timer.autostart = true
	timer.wait_time = 0.1
	timer.timeout.connect(set_new_block)
	add_child(timer)
	


var index := 0
func set_new_block():
	
	terr.set_block_local(chunk, Vector3(index, 0, 0), 1)
	
	
	index += 1

func _exit_tree() -> void:
	terr.stop()
