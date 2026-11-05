using Godot;
using System;

public partial class PauseMenu : Control
{
    [Signal]
    public delegate void ResumeRequestedEventHandler();

    [Signal]
    public delegate void SaveRequestedEventHandler();

    [Signal]
    public delegate void LoadRequestedEventHandler();

    [Signal]
    public delegate void RestartRequestedEventHandler();

    private Button _resumeBtn;
    private Button _saveBtn;
    private Button _loadBtn;
    private Button _restartBtn;
    private Button _mainMenuBtn;
    private Button _quitBtn;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;
        Visible = false;

        _resumeBtn = GetNode<Button>("CenterContainer/PanelContainer/MarginContainer/VBoxContainer/ResumeBtn");
        _saveBtn = GetNode<Button>("CenterContainer/PanelContainer/MarginContainer/VBoxContainer/SaveBtn");
        _loadBtn = GetNode<Button>("CenterContainer/PanelContainer/MarginContainer/VBoxContainer/LoadBtn");
        _restartBtn = GetNode<Button>("CenterContainer/PanelContainer/MarginContainer/VBoxContainer/RestartBtn");
        _mainMenuBtn = GetNode<Button>("CenterContainer/PanelContainer/MarginContainer/VBoxContainer/MainMenuBtn");
        _quitBtn = GetNode<Button>("CenterContainer/PanelContainer/MarginContainer/VBoxContainer/QuitBtn");

        _resumeBtn.Pressed += OnResumePressed;
        _saveBtn.Pressed += OnSavePressed;
        _loadBtn.Pressed += OnLoadPressed;
        _restartBtn.Pressed += OnRestartPressed;
        _mainMenuBtn.Pressed += OnMainMenuPressed;
        _quitBtn.Pressed += OnQuitPressed;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
            if (Visible)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
            GetViewport().SetInputAsHandled();
        }
    }

    public void OpenMenu()
    {
        Visible = true;
        GetTree().Paused = true;
        _loadBtn.Disabled = !SaveSystem.HasSave();
    }

    public void CloseMenu()
    {
        Visible = false;
        GetTree().Paused = false;
        EmitSignal(SignalName.ResumeRequested);
    }

    private void OnResumePressed()
    {
        CloseMenu();
    }

    private void OnSavePressed()
    {
        EmitSignal(SignalName.SaveRequested);
    }

    private void OnLoadPressed()
    {
        EmitSignal(SignalName.LoadRequested);
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

    private void OnQuitPressed()
    {
        GetTree().Quit();
    }
}
