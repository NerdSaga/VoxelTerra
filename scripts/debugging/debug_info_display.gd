extends Control


@onready var fps_label: Label = $FPS

func _ready() -> void:
	visible = false


func _process(_delta: float) -> void:
	fps_label.text = "FPS: " + str(Engine.get_frames_per_second())


func _input(event: InputEvent) -> void:
	
	if event is InputEventKey:
		if event.is_action_released("show_debug_info"):
			visible = !visible
