extends CollisionShape3D
class_name VoxelCollision

var _shape := ConcavePolygonShape3D.new()
var _points: PackedVector3Array = []

func begin() -> void:
	_points.clear()

func commit() -> void:
	_shape.set_faces(_points)

func place_voxel(local_position: Vector3, faces: Array[bool]):
	
	for i: int in range(6):
		
		if (faces[i] == false): continue
		
		var index := i * 4
		_points.append(VoxelTemplateData.VERTEX[0 + index] + local_position)
		_points.append(VoxelTemplateData.VERTEX[1 + index] + local_position)
		_points.append(VoxelTemplateData.VERTEX[2 + index] + local_position)
		_points.append(VoxelTemplateData.VERTEX[2 + index] + local_position)
		_points.append(VoxelTemplateData.VERTEX[1 + index] + local_position)
		_points.append(VoxelTemplateData.VERTEX[3 + index] + local_position)

func _ready() -> void:
	shape = _shape

static var SCENE: PackedScene = load("uid://b176cy6ffh13n")
static func CREATE() -> VoxelCollision:
	var voxel_collision: VoxelCollision = SCENE.instantiate()
	return voxel_collision
