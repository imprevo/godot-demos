using Godot;
using System;

public class InteractiveObject : CollisionShape2D
{
    [Export]
    public String message;

    public void Interact()
    {
        GD.PrintS(message);
    }
}
