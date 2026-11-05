class_name MainMenu
extends Control

@onready var play_btn: Button = $VBoxContainer/PlayBtn
@onready var continue_btn: Button = $VBoxContainer/ContinueBtn
@onready var reset_btn: Button = $VBoxContainer/ResetBtn
@onready var quit_btn: Button = $VBoxContainer/QuitBtn
@onready var stats_label: Label = $VBoxContainer/StatsLabel

func _ready() -> void:
	play_btn.pressed.connect(_on_play_pressed)
	continue_btn.pressed.connect(_on_continue_pressed)
	reset_btn.pressed.connect(_on_reset_pressed)
	quit_btn.pressed.connect(_on_quit_pressed)
	refresh_stats()

func refresh_stats() -> void:
	var has_data: bool = SaveSystem.has_save()
	continue_btn.disabled = not has_data
	reset_btn.disabled = not has_data
	if has_data:
		var data: Dictionary = SaveSystem.load_data()
		stats_label.text = "High Score: %d | Last Wave: %d\nLast Saved: %s" % [
			int(data.get("high_score", 0)),
			int(data.get("wave", 1)),
			str(data.get("timestamp", "-"))
		]
	else:
		stats_label.text = "No saved game found."

func _on_play_pressed() -> void:
	get_tree().change_scene_to_file("res://node_2d.tscn")

func _on_continue_pressed() -> void:
	get_tree().change_scene_to_file("res://node_2d.tscn")

func _on_reset_pressed() -> void:
	SaveSystem.delete_save()
	refresh_stats()

func _on_quit_pressed() -> void:
	get_tree().quit()
