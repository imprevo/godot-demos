using Godot;
using System;

public class Hurtbox : Area2D
{
    [Export]
    private int damage = 1;

    private void OnHurtboxAreaEntered(object area)
    {
        if (area is Hitbox)
        {
            var hitbox = (Hitbox)area;
            hitbox.Hit(damage);
        }
    }
}
