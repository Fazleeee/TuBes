class_name HUD
extends Control

signal pause_requested

@onready var health_bar: ProgressBar = $TopBar/MarginContainer/HBoxContainer/HealthSection/HealthBar
@onready var health_label: Label = $TopBar/MarginContainer/HBoxContainer/HealthSection/HealthLabel
@onready var score_label: Label = $TopBar/MarginContainer/HBoxContainer/StatsSection/ScoreLabel
@onready var wave_label: Label = $TopBar/MarginContainer/HBoxContainer/StatsSection/WaveLabel
@onready var pause_btn: Button = $TopBar/MarginContainer/HBoxContainer/PauseBtn
@onready var toast_panel: PanelContainer = $ToastPanel
@onready var toast_label: Label = $ToastPanel/MarginContainer/ToastLabel
@onready var debug_damage_btn: Button = $QuickControls/VBoxContainer/DamageBtn
@onready var debug_heal_btn: Button = $QuickControls/VBoxContainer/HealBtn
@onready var debug_score_btn: Button = $QuickControls/VBoxContainer/ScoreBtn

var toast_tween: Tween

func _ready() -> void:
	toast_panel.modulate.a = 0.0
	pause_btn.pressed.connect(_on_pause_pressed)
	debug_damage_btn.pressed.connect(_on_damage_pressed)
	debug_heal_btn.pressed.connect(_on_heal_pressed)
	debug_score_btn.pressed.connect(_on_score_pressed)

func update_health(current: int, max_val: int) -> void:
	health_bar.max_value = max_val
	health_bar.value = current
	health_label.text = "HP: %d / %d" % [current, max_val]

func update_score(score: int, high: int) -> void:
	score_label.text = "Score: %d | High: %d" % [score, high]

func update_wave(wave: int) -> void:
	wave_label.text = "Wave: %d" % [wave]

func show_toast(message: String) -> void:
	toast_label.text = message
	if toast_tween and toast_tween.is_valid():
		toast_tween.kill()
	toast_tween = create_tween()
	toast_tween.tween_property(toast_panel, "modulate:a", 1.0, 0.2)
	toast_tween.tween_interval(1.8)
	toast_tween.tween_property(toast_panel, "modulate:a", 0.0, 0.4)

func _on_pause_pressed() -> void:
	pause_requested.emit()

func _on_damage_pressed() -> void:
	var root: Node = get_tree().current_scene
	var ui: Node = get_parent()
	if ui.has_method("apply_damage"):
		ui.apply_damage(20)

func _on_heal_pressed() -> void:
	var ui: Node = get_parent()
	if ui.has_method("apply_heal"):
		ui.apply_heal(20)

func _on_score_pressed() -> void:
	var ui: Node = get_parent()
	if ui.has_method("add_score"):
		ui.add_score(50)
