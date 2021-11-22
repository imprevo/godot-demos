using Godot;
using System;

namespace Main
{
    public class Main : Node2D
    {
        private Navigation2D _navigation;
        private Position2D _spawnPoint;
        private Character _character;
        private PackedScene _characterScene = GD.Load<PackedScene>("res://main/Character.tscn");

        public override void _Ready()
        {
            _navigation = GetNode<Navigation2D>("Navigation2D");
            _spawnPoint = GetNode<Position2D>("SpawnPosition");
            _character = SpawnCharacter();
        }

        public override void _Process(float delta)
        {
            if (Input.IsActionJustPressed("left_mouse"))
            {
                var target = GetGlobalMousePosition();
                _character.MoveTo(target);
            }
        }

        private Character SpawnCharacter()
        {
            var character = _characterScene.Instance<Character>();
            character.navigation = _navigation;
            character.GlobalPosition = _spawnPoint.GlobalPosition;
            AddChild(character);
            return character;
        }
    }
}
