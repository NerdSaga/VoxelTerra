extends MeshInstance3D
class_name VoxelMesh

@export var materials: Array[Material] = []

var _surfaces: Array[SurfaceTool] = []
var _surface_vertex_counts: Array[int] = []
var _mesh := ArrayMesh.new()

func begin() -> void:
	
	_surface_vertex_counts.fill(0)
	for i: int in range(_surfaces.size()):
		var surface: SurfaceTool = _surfaces[i]
		surface.clear()
		surface.begin(Mesh.PRIMITIVE_TRIANGLES)
		surface.set_material(materials[i])

func commit() -> void:
	
	_mesh.clear_surfaces()
	for i: int in range(_surfaces.size()):
		var surface: SurfaceTool = _surfaces[i]
		surface.commit(_mesh)

func place_mesh() -> void:
	pass

func place_voxel(material_id: int, local_position: Vector3, uv: Array[Vector2], color: Array[Color], faces: Array[bool]) -> void:
	
	var surface := _surfaces[material_id]
	var vertex_count = _surface_vertex_counts[material_id]
	
	for i: int in range(6):
		
		if faces[i] == false:
			continue
		
		var index: int = i * 4;
		surface.set_normal(VoxelTemplateData.NORMAL[index])
	
		surface.set_uv(uv[index + 0])
		surface.set_color(color[index + 0])
		surface.add_vertex(VoxelTemplateData.VERTEX[index + 0] + local_position)
	
		surface.set_uv(uv[index + 1])
		surface.set_color(color[index + 1])
		surface.add_vertex(VoxelTemplateData.VERTEX[index + 1] + local_position)
		
		surface.set_uv(uv[index + 2])
		surface.set_color(color[index + 2])
		surface.add_vertex(VoxelTemplateData.VERTEX[index + 2] + local_position)

		surface.set_uv(uv[index + 3])
		surface.set_color(color[index + 3])
		surface.add_vertex(VoxelTemplateData.VERTEX[index + 3] + local_position)
		
		surface.add_index(vertex_count + 0)
		surface.add_index(vertex_count + 1)
		surface.add_index(vertex_count + 2)
		surface.add_index(vertex_count + 2)
		surface.add_index(vertex_count + 1)
		surface.add_index(vertex_count + 3)
		vertex_count += 4
	
	_surface_vertex_counts[material_id] = vertex_count

func _ready() -> void:
	
	_surfaces.resize(materials.size())
	_surface_vertex_counts.resize(_surfaces.size())
	_surface_vertex_counts.fill(0)
	
	for i: int in range(_surfaces.size()):
		_surfaces[i] = SurfaceTool.new()
	
	mesh = _mesh

static var SCENE: PackedScene = load("uid://be3ec2g14fuie")
static func CREATE() -> VoxelMesh:
	var voxel_mesh: VoxelMesh = SCENE.instantiate()
	return voxel_mesh
