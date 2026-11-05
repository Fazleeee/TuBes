using Godot;
using System;

public partial class UIManager : CanvasLayer
{
    private HUD _hud;
    private PauseMenu _pauseMenu;
    private GameOverMenu _gameOverMenu;

    public int CurrentHealth { get; set; } = 100;
    public int MaxHealth { get; set; } = 100;
    public int CurrentScore { get; set; } = 0;
    public int HighScore { get; set; } = 0;
    public int Wave { get; set; } = 1;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;

        _hud = GetNode<HUD>("HUD");
        _pauseMenu = GetNode<PauseMenu>("PauseMenu");
        _gameOverMenu = GetNode<GameOverMenu>("GameOverMenu");

        var saved = SaveSystem.LoadData();
        HighScore = saved.high_score;

        _hud.PauseRequested += OnPauseRequested;
        _pauseMenu.SaveRequested += SaveGame;
        _pauseMenu.LoadRequested += LoadGame;
        _pauseMenu.RestartRequested += RestartGame;
        _gameOverMenu.RestartRequested += RestartGame;

        UpdateAllUI();
    }

    public void ApplyDamage(int amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0, MaxHealth);
        _hud.UpdateHealth(CurrentHealth, MaxHealth);
        if (CurrentHealth <= 0)
        {
            TriggerGameOver();
        }
    }

    public void ApplyHeal(int amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
        _hud.UpdateHealth(CurrentHealth, MaxHealth);
    }

    public void AddScore(int amount)
    {
        CurrentScore += amount;
        if (CurrentScore > HighScore)
        {
            HighScore = CurrentScore;
        }
        _hud.UpdateScore(CurrentScore, HighScore);
    }

    public void UpdateAllUI()
    {
        _hud.UpdateHealth(CurrentHealth, MaxHealth);
        _hud.UpdateScore(CurrentScore, HighScore);
        _hud.UpdateWave(Wave);
    }

    public void SaveGame()
    {
        var playerNode = GetTree().CurrentScene.FindChild("Player", true, false) as Node2D;
        float px = playerNode != null ? playerNode.Position.X : 0.0f;
        float py = playerNode != null ? playerNode.Position.Y : 0.0f;

        var data = new SaveData
        {
            current_score = CurrentScore,
            high_score = HighScore,
            health = CurrentHealth,
            max_health = MaxHealth,
            wave = Wave,
            player_pos_x = px,
            player_pos_y = py
        };

        bool success = SaveSystem.Save(data);
        if (success)
        {
            _hud.ShowToast("Game Saved Successfully!");
        }
        else
        {
            _hud.ShowToast("Failed to Save Game!");
        }
    }

    public void LoadGame()
    {
        if (!SaveSystem.HasSave())
        {
            _hud.ShowToast("No Save File Found!");
            return;
        }

        var data = SaveSystem.LoadData();
        CurrentHealth = data.health;
        MaxHealth = data.max_health;
        CurrentScore = data.current_score;
        HighScore = data.high_score;
        Wave = data.wave;

        var playerNode = GetTree().CurrentScene.FindChild("Player", true, false) as Node2D;
        if (playerNode != null)
        {
            playerNode.Position = new Vector2(data.player_pos_x, data.player_pos_y);
        }

        UpdateAllUI();
        _hud.ShowToast("Game Loaded Successfully!");
    }

    public void TriggerGameOver()
    {
        SaveGame();
        _gameOverMenu.ShowGameOver(CurrentScore, HighScore);
    }

    public void RestartGame()
    {
        GetTree().Paused = false;
        GetTree().ReloadCurrentScene();
    }

    private void OnPauseRequested()
    {
        _pauseMenu.OpenMenu();
    }
}
