using Godot;
using System;

public class Stats : Node
{
    [Signal]
    delegate void StatsUpdatedSignal();

    [Export]
    public int HP = 1;
    public int maxHP;

    public override void _Ready()
    {
        maxHP = HP;
    }

    public bool IsAlive()
    {
        return HP > 0;
    }

    public bool IsMaxHP()
    {
        return HP == maxHP;
    }

    public void Hit(int damage)
    {
        if (damage <= 0)
        {
            throw new Exception("Damage must be greater than 0");
        }
        HP = Math.Max(0, HP - damage);
        EmitSignal();
    }

    public void Heal(int health)
    {
        if (health <= 0)
        {
            throw new Exception("Health must be greater than 0");
        }
        this.HP = Math.Min(this.maxHP, this.HP + health);
        EmitSignal();
    }

    public void EmitSignal()
    {
        EmitSignal(nameof(StatsUpdatedSignal));
    }
}
