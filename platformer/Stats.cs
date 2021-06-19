using Godot;
using System;

public class Stats : Node
{
    [Export]
    public int health = 1;
    public int maxHealth;

    public override void _Ready()
    {
        maxHealth = health;
    }

    public void Hit(int damage)
    {
        if (damage <= 0)
        {
            throw new Exception("Damage must be greater than 0");
        }
        health = Math.Max(0, health - damage);
    }

    public void Heal(int health)
    {
        if (health <= 0)
        {
            throw new Exception("Health must be greater than 0");
        }
        this.health = Math.Min(this.maxHealth, this.health + health);
    }
}
