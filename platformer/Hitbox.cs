using Godot;
using System;


public class Hitbox : Area2D
{
    [Signal]
    delegate void HitSignal(int damage);

    public void Hit(int damage)
    {
        EmitSignal(nameof(HitSignal), damage);
    }
}
