using Godot;
using System;

public partial class GameOverMenu : Control
{
    [Signal]
    public delegate void RestartRequestedEventHandler();

    private Label _scoreLabel;
    private Label _highScoreLabel;
    private Button _restartBtn;
    private Button _mainMenuBtn;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;
        Visible = false;

        _scoreLabel = GetNode<Label>("CenterContainer/PanelContainer/MarginContainer/VBoxContainer/ScoreLabel");
        _highScoreLabel = GetNode<Label>("CenterContainer/PanelContainer/MarginContainer/VBoxContainer/HighScoreLabel");
        _restartBtn = GetNode<Button>("CenterContainer/PanelContainer/MarginContainer/VBoxContainer/RestartBtn");
        _mainMenuBtn = GetNode<Button>("CenterContainer/PanelContainer/MarginContainer/VBoxContainer/MainMenuBtn");

        _restartBtn.Pressed += OnRestartPressed;
        _mainMenuBtn.Pressed += OnMainMenuPressed;
    }

    public void ShowGameOver(int finalScore, int highScore)
    {
        _scoreLabel.Text = $"Final Score: {finalScore}";
        _highScoreLabel.Text = $"High Score: {highScore}";
        Visible = true;
        GetTree().Paused = true;
    }

    private void OnRestartPressed()
    {
        GetTree().Paused = false;
        EmitSignal(SignalName.RestartRequested);
    }

    private void OnMainMenuPressed()
    {
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
    }
}
