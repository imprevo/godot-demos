using Godot;
using System;

public class BallTrigger : Area2D
{
    public Ball GetBall()
    {
        return GetParent<Ball>();
    }
}
