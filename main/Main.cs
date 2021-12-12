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
        private Area2D _interactiveObjects;
        private InteractiveObject _interactWith;

        public override void _Ready()
        {
            _navigation = GetNode<Navigation2D>("Navigation2D");
            _spawnPoint = GetNode<Position2D>("SpawnPosition");
            _character = SpawnCharacter();
            _interactiveObjects = GetNode<Area2D>("InteractiveObjects");
        }

        public override void _Process(float delta)
        {
            if (Input.IsActionJustPressed("left_mouse"))
            {
                var target = GetGlobalMousePosition();
                _character.MoveTo(target, _interactWith);
                _interactWith = null;
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

        private void OnInteractiveObjectsInputEvent(object viewport, object @event, int shape_idx)
        {
            if (@event is InputEventMouseButton)
            {
                var mouseEvent = (InputEventMouseButton)@event;
                if (mouseEvent.Pressed)
                {
                    _interactWith = _interactiveObjects.GetChild<InteractiveObject>(shape_idx);
                }
            }
        }

        private void OnInteractiveObjectsMouseEntered()
        {
            Input.SetDefaultCursorShape(Input.CursorShape.PointingHand);
        }

        private void OnInteractiveObjectsMouseExited()
        {
            Input.SetDefaultCursorShape();
        }
    }
}
