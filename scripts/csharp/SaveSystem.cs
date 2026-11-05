using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;

public class SaveData
{
    public int high_score { get; set; } = 0;
    public int current_score { get; set; } = 0;
    public int health { get; set; } = 100;
    public int max_health { get; set; } = 100;
    public int wave { get; set; } = 1;
    public float player_pos_x { get; set; } = 0.0f;
    public float player_pos_y { get; set; } = 0.0f;
    public string timestamp { get; set; } = "";
    public float bgm_volume { get; set; } = 0.8f;
    public float sfx_volume { get; set; } = 0.8f;
}

public static class SaveSystem
{
    private const string SavePath = "user://save_data.json";

    public static SaveData GetDefaultData()
    {
        return new SaveData();
    }

    public static bool HasSave()
    {
        return FileAccess.FileExists(SavePath);
    }

    public static SaveData LoadData()
    {
        if (!FileAccess.FileExists(SavePath))
        {
            return GetDefaultData();
        }

        using var file = FileAccess.Open(SavePath, FileAccess.ModeFlags.Read);
        if (file == null)
        {
            return GetDefaultData();
        }

        string rawText = file.GetAsText();
        try
        {
            var data = JsonSerializer.Deserialize<SaveData>(rawText);
            return data ?? GetDefaultData();
        }
        catch
        {
            return GetDefaultData();
        }
    }

    public static bool Save(SaveData data)
    {
        if (data.current_score > data.high_score)
        {
            data.high_score = data.current_score;
        }

        data.timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        using var file = FileAccess.Open(SavePath, FileAccess.ModeFlags.Write);
        if (file == null)
        {
            return false;
        }

        string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        file.StoreString(json);
        return true;
    }

    public static bool DeleteSave()
    {
        if (!FileAccess.FileExists(SavePath))
        {
            return false;
        }

        string globalPath = ProjectSettings.GlobalizePath(SavePath);
        return DirAccess.RemoveAbsolute(globalPath) == Error.Ok;
    }
}
