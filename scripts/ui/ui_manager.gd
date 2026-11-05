class_name UIManager
extends CanvasLayer

@onready var hud: HUD = $HUD
@onready var pause_menu: PauseMenu = $PauseMenu
@onready var game_over_menu: GameOverMenu = $GameOverMenu

var current_health: int = 100
var max_health: int = 100
var current_score: int = 0
var high_score: int = 0
var wave: int = 1

func _ready() -> void:
	process_mode = Node.PROCESS_MODE_ALWAYS
	var saved: Dictionary = SaveSystem.load_data()
	high_score = int(saved.get("high_score", 0))

	hud.pause_requested.connect(_on_pause_requested)
	pause_menu.save_requested.connect(save_game)
	pause_menu.load_requested.connect(load_game)
	pause_menu.restart_requested.connect(restart_game)
	game_over_menu.restart_requested.connect(restart_game)

	update_all_ui()

func apply_damage(amount: int) -> void:
	current_health = clampi(current_health - amount, 0, max_health)
	hud.update_health(current_health, max_health)
	if current_health <= 0:
		trigger_game_over()

func apply_heal(amount: int) -> void:
	current_health = clampi(current_health + amount, 0, max_health)
	hud.update_health(current_health, max_health)

func add_score(amount: int) -> void:
	current_score += amount
	if current_score > high_score:
		high_score = current_score
	hud.update_score(current_score, high_score)

func update_all_ui() -> void:
	hud.update_health(current_health, max_health)
	hud.update_score(current_score, high_score)
	hud.update_wave(wave)

func save_game() -> void:
	var player_node: Node2D = get_tree().current_scene.find_child("Player", true, false) as Node2D
	var pos_x: float = player_node.position.x if player_node else 0.0
	var pos_y: float = player_node.position.y if player_node else 0.0
	var data: Dictionary = {
		"current_score": current_score,
		"high_score": high_score,
		"health": current_health,
		"max_health": max_health,
		"wave": wave,
		"player_pos_x": pos_x,
		"player_pos_y": pos_y
	}
	var success: bool = SaveSystem.save_data(data)
	if success:
		hud.show_toast("Game Saved Successfully!")
	else:
		hud.show_toast("Failed to Save Game!")

func load_game() -> void:
	if not SaveSystem.has_save():
		hud.show_toast("No Save File Found!")
		return
	var data: Dictionary = SaveSystem.load_data()
	current_health = int(data.get("health", 100))
	max_health = int(data.get("max_health", 100))
	current_score = int(data.get("current_score", 0))
	high_score = int(data.get("high_score", 0))
	wave = int(data.get("wave", 1))

	var player_node: Node2D = get_tree().current_scene.find_child("Player", true, false) as Node2D
	if player_node:
		var px: float = float(data.get("player_pos_x", player_node.position.x))
		var py: float = float(data.get("player_pos_y", player_node.position.y))
		player_node.position = Vector2(px, py)

	update_all_ui()
	hud.show_toast("Game Loaded Successfully!")

func trigger_game_over() -> void:
	save_game()
	game_over_menu.show_game_over(current_score, high_score)

func restart_game() -> void:
	get_tree().paused = false
	get_tree().reload_current_scene()

func _on_pause_requested() -> void:
	pause_menu.open_menu()
