class_name PauseMenu
extends Control

signal resume_requested
signal save_requested
signal load_requested
signal restart_requested

@onready var resume_btn: Button = $CenterContainer/PanelContainer/MarginContainer/VBoxContainer/ResumeBtn
@onready var save_btn: Button = $CenterContainer/PanelContainer/MarginContainer/VBoxContainer/SaveBtn
@onready var load_btn: Button = $CenterContainer/PanelContainer/MarginContainer/VBoxContainer/LoadBtn
@onready var restart_btn: Button = $CenterContainer/PanelContainer/MarginContainer/VBoxContainer/RestartBtn
@onready var main_menu_btn: Button = $CenterContainer/PanelContainer/MarginContainer/VBoxContainer/MainMenuBtn
@onready var quit_btn: Button = $CenterContainer/PanelContainer/MarginContainer/VBoxContainer/QuitBtn

func _ready() -> void:
	process_mode = Node.PROCESS_MODE_ALWAYS
	visible = false
	resume_btn.pressed.connect(_on_resume_pressed)
	save_btn.pressed.connect(_on_save_pressed)
	load_btn.pressed.connect(_on_load_pressed)
	restart_btn.pressed.connect(_on_restart_pressed)
	main_menu_btn.pressed.connect(_on_main_menu_pressed)
	quit_btn.pressed.connect(_on_quit_pressed)

func _unhandled_input(event: InputEvent) -> void:
	if event.is_action_pressed("ui_cancel"):
		if visible:
			close_menu()
		else:
			open_menu()
		get_viewport().set_input_as_handled()

func open_menu() -> void:
	visible = true
	get_tree().paused = true
	load_btn.disabled = not SaveSystem.has_save()

func close_menu() -> void:
	visible = false
	get_tree().paused = false
	resume_requested.emit()

func _on_resume_pressed() -> void:
	close_menu()

func _on_save_pressed() -> void:
	save_requested.emit()

func _on_load_pressed() -> void:
	load_requested.emit()

func _on_restart_pressed() -> void:
	get_tree().paused = false
	restart_requested.emit()

func _on_main_menu_pressed() -> void:
	get_tree().paused = false
	get_tree().change_scene_to_file("res://scenes/main_menu.tscn")

func _on_quit_pressed() -> void:
	get_tree().quit()
