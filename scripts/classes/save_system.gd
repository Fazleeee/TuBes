class_name SaveSystem
extends RefCounted

const SAVE_PATH: String = "user://save_data.json"

static func get_default_data() -> Dictionary:
	return {
		"high_score": 0,
		"current_score": 0,
		"health": 100,
		"max_health": 100,
		"wave": 1,
		"player_pos_x": 0.0,
		"player_pos_y": 0.0,
		"timestamp": "",
		"bgm_volume": 0.8,
		"sfx_volume": 0.8
	}

static func has_save() -> bool:
	return FileAccess.file_exists(SAVE_PATH)

static func load_data() -> Dictionary:
	var defaults: Dictionary = get_default_data()
	if not FileAccess.file_exists(SAVE_PATH):
		return defaults
	var file: FileAccess = FileAccess.open(SAVE_PATH, FileAccess.READ)
	if file == null:
		return defaults
	var raw_text: String = file.get_as_text()
	file.close()
	var parsed: Variant = JSON.parse_string(raw_text)
	if typeof(parsed) != TYPE_DICTIONARY:
		return defaults
	var result: Dictionary = defaults.duplicate()
	for key in parsed.keys():
		result[key] = parsed[key]
	return result

static func save_data(data: Dictionary) -> bool:
	var current: Dictionary = load_data()
	for key in data.keys():
		current[key] = data[key]
	var score: int = int(current.get("current_score", 0))
	var high: int = int(current.get("high_score", 0))
	if score > high:
		current["high_score"] = score
	var time_dict: Dictionary = Time.get_datetime_dict_from_system()
	current["timestamp"] = "%04d-%02d-%02d %02d:%02d:%02d" % [
		time_dict["year"], time_dict["month"], time_dict["day"],
		time_dict["hour"], time_dict["minute"], time_dict["second"]
	]
	var file: FileAccess = FileAccess.open(SAVE_PATH, FileAccess.WRITE)
	if file == null:
		return false
	file.store_string(JSON.stringify(current, "\t"))
	file.close()
	return true

static func delete_save() -> bool:
	if not FileAccess.file_exists(SAVE_PATH):
		return false
	var err: Error = DirAccess.remove_absolute(ProjectSettings.globalize_path(SAVE_PATH))
	return err == OK
