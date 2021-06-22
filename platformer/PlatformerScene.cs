using Godot;
using System;

public class PlatformerScene : Node2D
{
    private HUD _hud;
    private Character _player;

    public override void _Ready()
    {
        _hud = GetNode<HUD>("HUD");
        _player = GetNode<Character>("Character");
        OnPlayerStatsUpdated();
    }

    private void OnPlayerStatsUpdated()
    {
        _hud.updatePlayerStats(_player.stats);
    }
}
