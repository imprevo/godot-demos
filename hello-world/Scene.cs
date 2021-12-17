using Godot;
using System;
using Core;

namespace HelloWorld
{
    public class Scene : Node2D
    {
        private int _count = 0;

        private Random _random = new Random();

        private float _RandRange(float min, float max)
        {
            return (float)_random.NextDouble() * (max - min) + min;
        }

        private void _OnButtonPressed()
        {
            _count++;
            GetNode<Label>("Label").Text = "Pressed " + _count + " times";

            var scene = GD.Load<PackedScene>("res://hello-world/ball.tscn");
            var node = (Node2D)scene.Instance();
            node.Position = new Vector2(_RandRange(300, 700), 50);
            AddChild(node);
        }

        private void OnExitButtonPressed()
        {
            SceneManager.manager.ChangeScene(Core.Scene.MAIN);
        }
    }
}
