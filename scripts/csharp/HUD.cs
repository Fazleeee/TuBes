using Godot;
using System;

public partial class HUD : Control
{
    [Signal]
    public delegate void PauseRequestedEventHandler();

    private ProgressBar _healthBar;
    private Label _healthLabel;
    private Label _scoreLabel;
    private Label _waveLabel;
    private Button _pauseBtn;
    private PanelContainer _toastPanel;
    private Label _toastLabel;
    private Button _damageBtn;
    private Button _healBtn;
    private Button _scoreBtn;
    private Tween _toastTween;

    public override void _Ready()
    {
        _healthBar = GetNode<ProgressBar>("TopBar/MarginContainer/HBoxContainer/HealthSection/HealthBar");
        _healthLabel = GetNode<Label>("TopBar/MarginContainer/HBoxContainer/HealthSection/HealthLabel");
        _scoreLabel = GetNode<Label>("TopBar/MarginContainer/HBoxContainer/StatsSection/ScoreLabel");
        _waveLabel = GetNode<Label>("TopBar/MarginContainer/HBoxContainer/StatsSection/WaveLabel");
        _pauseBtn = GetNode<Button>("TopBar/MarginContainer/HBoxContainer/PauseBtn");
        _toastPanel = GetNode<PanelContainer>("ToastPanel");
        _toastLabel = GetNode<Label>("ToastPanel/MarginContainer/ToastLabel");
        _damageBtn = GetNode<Button>("QuickControls/VBoxContainer/DamageBtn");
        _healBtn = GetNode<Button>("QuickControls/VBoxContainer/HealBtn");
        _scoreBtn = GetNode<Button>("QuickControls/VBoxContainer/ScoreBtn");

        _toastPanel.Modulate = new Color(1, 1, 1, 0);

        _pauseBtn.Pressed += OnPausePressed;
        _damageBtn.Pressed += OnDamagePressed;
        _healBtn.Pressed += OnHealPressed;
        _scoreBtn.Pressed += OnScorePressed;
    }

    public void UpdateHealth(int current, int max)
    {
        _healthBar.MaxValue = max;
        _healthBar.Value = current;
        _healthLabel.Text = $"HP: {current} / {max}";
    }

    public void UpdateScore(int score, int high)
    {
        _scoreLabel.Text = $"Score: {score} | High: {high}";
    }

    public void UpdateWave(int wave)
    {
        _waveLabel.Text = $"Wave: {wave}";
    }

    public void ShowToast(string message)
    {
        _toastLabel.Text = message;
        if (_toastTween != null && _toastTween.IsValid())
        {
            _toastTween.Kill();
        }
        _toastTween = CreateTween();
        _toastTween.TweenProperty(_toastPanel, "modulate:a", 1.0f, 0.2f);
        _toastTween.TweenInterval(1.8f);
        _toastTween.TweenProperty(_toastPanel, "modulate:a", 0.0f, 0.4f);
    }

    private void OnPausePressed()
    {
        EmitSignal(SignalName.PauseRequested);
    }

    private void OnDamagePressed()
    {
        var parent = GetParent();
        if (parent.HasMethod("apply_damage"))
        {
            parent.Call("apply_damage", 20);
        }
    }

    private void OnHealPressed()
    {
        var parent = GetParent();
        if (parent.HasMethod("apply_heal"))
        {
            parent.Call("apply_heal", 20);
        }
    }

    private void OnScorePressed()
    {
        var parent = GetParent();
        if (parent.HasMethod("add_score"))
        {
            parent.Call("add_score", 50);
        }
    }
}
