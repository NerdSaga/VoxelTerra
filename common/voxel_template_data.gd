extends Object
class_name VoxelTemplateData

static var VERTEX: Array[Vector3] = [
	# Up (+Y)
	Vector3(1, 1, 0),
	Vector3(1, 1, 1),
	Vector3(0, 1, 0),
	Vector3(0, 1, 1),
	
	# Down (-Y)
	Vector3(1, 0, 1),
	Vector3(1, 0, 0),
	Vector3(0, 0, 1),
	Vector3(0, 0, 0),
	
	# North (+X)
	Vector3(1, 1, 1),
	Vector3(1, 1, 0),
	Vector3(1, 0, 1),
	Vector3(1, 0, 0),
	
	# South (-X)
	Vector3(0, 1, 0),
	Vector3(0, 1, 1),
	Vector3(0, 0, 0),
	Vector3(0, 0, 1),
	
	# East (+Z)
	Vector3(0, 1, 1),
	Vector3(1, 1, 1),
	Vector3(0, 0, 1),
	Vector3(1, 0, 1),
	
	Vector3(1, 1, 0),
	Vector3(0, 1, 0),
	Vector3(1, 0, 0),
	Vector3(0, 0, 0),
]

static var NORMAL: Array[Vector3] = [
	
	Vector3(0, 1, 0).normalized(),
	Vector3(0, 1, 0).normalized(),
	Vector3(0, 1, 0).normalized(),
	Vector3(0, 1, 0).normalized(),

	Vector3(0, -1, 0).normalized(),
	Vector3(0, -1, 0).normalized(),
	Vector3(0, -1, 0).normalized(),
	Vector3(0, -1, 0).normalized(),

	Vector3(1, 0, 0).normalized(),
	Vector3(1, 0, 0).normalized(),
	Vector3(1, 0, 0).normalized(),
	Vector3(1, 0, 0).normalized(),

	Vector3(-1, 0, 0).normalized(),
	Vector3(-1, 0, 0).normalized(),
	Vector3(-1, 0, 0).normalized(),
	Vector3(-1, 0, 0).normalized(),

	Vector3(0, 0, 1).normalized(),
	Vector3(0, 0, 1).normalized(),
	Vector3(0, 0, 1).normalized(),
	Vector3(0, 0, 1).normalized(),

	Vector3(0, 0, -1).normalized(),
	Vector3(0, 0, -1).normalized(),
	Vector3(0, 0, -1).normalized(),
	Vector3(0, 0, -1).normalized(),
]

static var UV: Array[Vector2] = [
	
	Vector2(0, 0),
	Vector2(1, 0),
	Vector2(0, 1),
	Vector2(1, 1),

	Vector2(0, 0),
	Vector2(1, 0),
	Vector2(0, 1),
	Vector2(1, 1),
	
	Vector2(0, 0),
	Vector2(1, 0),
	Vector2(0, 1),
	Vector2(1, 1),
	
	Vector2(0, 0),
	Vector2(1, 0),
	Vector2(0, 1),
	Vector2(1, 1),
	
	Vector2(0, 0),
	Vector2(1, 0),
	Vector2(0, 1),
	Vector2(1, 1),
	
	Vector2(0, 0),
	Vector2(1, 0),
	Vector2(0, 1),
	Vector2(1, 1),
]

static var COLOR: Array[Color] = [
	
	Color(0xffffffff),
	Color(0xffffffff),
	Color(0xffffffff),
	Color(0xffffffff),

	Color(0xffffffff),
	Color(0xffffffff),
	Color(0xffffffff),
	Color(0xffffffff),

	Color(0xffffffff),
	Color(0xffffffff),
	Color(0xffffffff),
	Color(0xffffffff),

	Color(0xffffffff),
	Color(0xffffffff),
	Color(0xffffffff),
	Color(0xffffffff),

	Color(0xffffffff),
	Color(0xffffffff),
	Color(0xffffffff),
	Color(0xffffffff),

	Color(0xffffffff),
	Color(0xffffffff),
	Color(0xffffffff),
	Color(0xffffffff),
]

static var FACES_ALL: Array[bool] = [
	true, true, true, true, true, true
]

static var FACES_NONE: Array[bool] = [
	false, false, false, false, false, false
]
