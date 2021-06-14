using Godot;
using System;

public class scene : Node2D
{
    private int _count = 0;

    private Random _random = new Random();

    public override void _Ready()
    {

    }

    private float _RandRange(float min, float max)
    {
        return (float)_random.NextDouble() * (max - min) + min;
    }

    private void _OnButtonPressed()
    {
        _count++;
        GetNode<Label>("Label").Text = "Pressed " + _count + " times";

        var scene = GD.Load<PackedScene>("res://ball.tscn");
        var node = (Node2D)scene.Instance();
        node.Position = new Vector2(_RandRange(300, 700), 50);
        AddChild(node);
    }
}
