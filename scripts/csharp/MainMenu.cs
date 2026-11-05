using Godot;
using System;

public partial class MainMenu : Control
{
    private Button _playBtn;
    private Button _continueBtn;
    private Button _resetBtn;
    private Button _quitBtn;
    private Label _statsLabel;

    public override void _Ready()
    {
        _playBtn = GetNode<Button>("VBoxContainer/PlayBtn");
        _continueBtn = GetNode<Button>("VBoxContainer/ContinueBtn");
        _resetBtn = GetNode<Button>("VBoxContainer/ResetBtn");
        _quitBtn = GetNode<Button>("VBoxContainer/QuitBtn");
        _statsLabel = GetNode<Label>("VBoxContainer/StatsLabel");

        _playBtn.Pressed += OnPlayPressed;
        _continueBtn.Pressed += OnContinuePressed;
        _resetBtn.Pressed += OnResetPressed;
        _quitBtn.Pressed += OnQuitPressed;

        RefreshStats();
    }

    public void RefreshStats()
    {
        bool hasData = SaveSystem.HasSave();
        _continueBtn.Disabled = !hasData;
        _resetBtn.Disabled = !hasData;

        if (hasData)
        {
            var data = SaveSystem.LoadData();
            _statsLabel.Text = $"High Score: {data.high_score} | Last Wave: {data.wave}\nLast Saved: {data.timestamp}";
        }
        else
        {
            _statsLabel.Text = "No saved game found.";
        }
    }

    private void OnPlayPressed()
    {
        GetTree().ChangeSceneToFile("res://node_2d.tscn");
    }

    private void OnContinuePressed()
    {
        GetTree().ChangeSceneToFile("res://node_2d.tscn");
    }

    private void OnResetPressed()
    {
        SaveSystem.DeleteSave();
        RefreshStats();
    }

    private void OnQuitPressed()
    {
        GetTree().Quit();
    }
}
