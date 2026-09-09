extends CharacterBody3D
class_name FreeCamera

@onready var camera: Camera3D = $Camera3D

var _movment_multiplier = 10
var _look_delta := Vector2.ZERO
var _input_dir := Vector3.ZERO
var _target_velocity := Vector3.ZERO
var _target_look := Vector3.ZERO
var _look_sensitivity = 0.01

func toggle_mouse_mode() -> void:
	
	if Input.mouse_mode == Input.MOUSE_MODE_CAPTURED:
		Input.mouse_mode = Input.MOUSE_MODE_VISIBLE
	elif Input.mouse_mode == Input.MOUSE_MODE_VISIBLE:
		Input.mouse_mode = Input.MOUSE_MODE_CAPTURED

func _process(delta: float) -> void:
	
	_input_dir.x = Input.get_action_strength("move_right") - Input.get_action_strength("move_left")
	_input_dir.y = Input.get_action_strength("move_up") - Input.get_action_strength("move_down")
	_input_dir.z = Input.get_action_strength("move_backward") - Input.get_action_strength("move_forward")
	
	_target_look += Vector3(_look_delta.x, _look_delta.y, 0) * _look_sensitivity
	_target_look.x = clamp(_target_look.x, -PI / 2.001, PI / 2.001)

	camera.rotation = lerp(camera.rotation, _target_look, 20 * delta)
	_look_delta = Vector2.ZERO

func _physics_process(delta: float) -> void:
	
	var rl := camera.basis.x
	rl.y = 0
	rl = rl.normalized()
	_target_velocity += rl * _input_dir.x
	
	var fb := camera.basis.z
	fb.y = 0
	fb = fb.normalized()
	_target_velocity += fb * _input_dir.z
	
	var ud = Vector3(0, 1, 0)
	_target_velocity += ud * _input_dir.y
	
	velocity = lerp(velocity, _target_velocity * _movment_multiplier, 13 * delta)
	_target_velocity = Vector3.ZERO
	move_and_slide()
	
func _input(event: InputEvent) -> void:
	
	if (event.is_action_released("ui_cancel")):
		toggle_mouse_mode()
		
	if event is InputEventMouseMotion and Input.mouse_mode == Input.MOUSE_MODE_CAPTURED:
		_look_delta.y = -event.relative.x
		_look_delta.x = -event.relative.y

func _ready() -> void:
	toggle_mouse_mode()


static var SCENE:PackedScene = load("uid://10jvgsupxywo")
static func CREATE(world_position: Vector3) -> FreeCamera:
	
	var free_camera: FreeCamera = SCENE.instantiate()
	free_camera.position = world_position
	return free_camera
