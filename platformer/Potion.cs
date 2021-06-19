using Godot;
using System;

public class Potion : Area2D
{
    private int HP = 1;

    public void Use(Character character)
    {
        character.Heal(HP);
        QueueFree();
    }
}

