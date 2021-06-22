using Godot;
using System;

public class HUD : Control
{

    private TextureProgress _progress;

    public override void _Ready()
    {
        _progress = GetNode<TextureProgress>("HealthBar/Progress");
    }

    public void updatePlayerStats(Stats stats)
    {
        _progress.MaxValue = stats.maxHP;
        _progress.Value = stats.HP;
    }
}
