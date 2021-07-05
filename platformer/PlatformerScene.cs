using Godot;
using System;

public class PlatformerScene : Node2D
{
    private HUD _hud;
    private Character _player;
    private Enemy _enemy;
    private Camera2D _camera;
    private Control _menu;

    public override void _Ready()
    {
        _hud = GetNode<HUD>("Camera2D/HUD");
        _player = GetNode<Character>("Character");
        _enemy = GetNode<Enemy>("Enemy");
        _enemy.setPlayer(_player);
        _camera = GetNode<Camera2D>("Camera2D");
        _camera.setTarget(_player);
        _menu = GetNode<Control>("Menu/Control");
        OnPlayerStatsUpdated();
    }

    private void OnPlayerStatsUpdated()
    {
        _hud.updatePlayerStats(_player.stats);
        if (!_player.stats.IsAlive())
        {
            _menu.Show();
        }
    }

    private void OnRestartButtonPressed()
    {
        GetTree().ReloadCurrentScene();
    }

    private void OnExitButtonPressed()
    {
        GetTree().Quit();
    }
}
