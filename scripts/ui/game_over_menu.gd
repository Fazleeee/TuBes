class_name GameOverMenu
extends Control

signal restart_requested

@onready var score_label: Label = $CenterContainer/PanelContainer/MarginContainer/VBoxContainer/ScoreLabel
@onready var high_score_label: Label = $CenterContainer/PanelContainer/MarginContainer/VBoxContainer/HighScoreLabel
@onready var restart_btn: Button = $CenterContainer/PanelContainer/MarginContainer/VBoxContainer/RestartBtn
@onready var main_menu_btn: Button = $CenterContainer/PanelContainer/MarginContainer/VBoxContainer/MainMenuBtn

func _ready() -> void:
	process_mode = Node.PROCESS_MODE_ALWAYS
	visible = false
	restart_btn.pressed.connect(_on_restart_pressed)
	main_menu_btn.pressed.connect(_on_main_menu_pressed)

func show_game_over(final_score: int, high_score: int) -> void:
	score_label.text = "Final Score: %d" % final_score
	high_score_label.text = "High Score: %d" % high_score
	visible = true
	get_tree().paused = true

func _on_restart_pressed() -> void:
	get_tree().paused = false
	restart_requested.emit()

func _on_main_menu_pressed() -> void:
	get_tree().paused = false
	get_tree().change_scene_to_file("res://scenes/main_menu.tscn")
