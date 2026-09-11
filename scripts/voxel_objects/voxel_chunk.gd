extends StaticBody3D
class_name VoxelChunk

@onready var v_mesh: VoxelMesh = $VoxelMesh
@onready var v_collision: VoxelCollision = $VoxelCollision
var blocks := PackedByteArray()
var unit_position := Vector2.ZERO

func begin_build() -> void:
	v_mesh.begin()
	v_collision.begin()

func set_block(chunk_block_index: int, block: int) -> void:
	blocks.encode_u16(chunk_block_index * 2, block)

func get_block(chunk_block_index: int) -> int:
	return blocks.decode_u16(chunk_block_index * 2)

func build() -> void:
	
	var x: int = 0
	var y: int = 0
	var z: int = 0
	
	for i: int in range(16 * 16 * 256):
		
		
		var chunk_block_index = x
		chunk_block_index += y * 256
		chunk_block_index += z * 16
		
		var block = get_block(chunk_block_index)
		
		if block > 0:
			v_mesh.place_voxel(0, Vector3(x, y, z), VoxelTemplateData.UV, VoxelTemplateData.COLOR, VoxelTemplateData.FACES_ALL)
			v_collision.place_voxel(Vector3(x, y, z), VoxelTemplateData.FACES_ALL)
		
		x += 1
		
		if (x >= 16):
			x = 0
			z += 1
		
		if z >= 16:
			z = 0
			y += 1

func commit_build() -> void:
	v_mesh.commit()
	v_collision.commit()

func _ready() -> void:
	blocks.resize(2 * 16 * 16 * 256)
	blocks.fill(0)

static var SCENE: PackedScene = load("uid://c3uwikwg8t615")
static func CREATE(chunk_unit_position: Vector2i) -> VoxelChunk:
	var chunk: VoxelChunk = SCENE.instantiate()
	chunk.position = Vector3i(chunk_unit_position.x * 16, 0, chunk_unit_position.y * 16)
	chunk.unit_position = chunk_unit_position
	chunk.name = "Chunk_" + str(chunk.unit_position.x) + "_" + str(chunk.unit_position.y)
	return chunk
