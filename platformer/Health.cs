using Godot;
using System;

public class Health
{
    public int maxValue;
    public int value;

    public Health(int health)
    {
        maxValue = health;
        value = health;
    }

    public void Hit(int damage)
    {
        if (damage <= 0)
        {
            throw new Exception("Damage must be greater than 0");
        }
        value = Math.Max(0, value - damage);
    }

    public void Heal(int health)
    {
        if (health <= 0)
        {
            throw new Exception("Health must be greater than 0");
        }
        value = Math.Min(maxValue, value + health);
    }
}
