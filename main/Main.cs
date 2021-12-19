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
        private ActiveTileMap _interiorActiveTileMap;
        private bool interactiveJustEntered = false;

        public override void _Ready()
        {
            _navigation = GetNode<Navigation2D>("Navigation2D");
            _spawnPoint = GetNode<Position2D>("SpawnPosition");
            _character = SpawnCharacter();
            _interactiveObjects = GetNode<Area2D>("InteractiveObjects");
            _interiorActiveTileMap = GetNode<ActiveTileMap>("Navigation2D/InteriorActiveTileMap");
        }

        public override void _Process(float delta)
        {
            if (Input.IsActionJustPressed("left_mouse"))
            {
                GD.PrintS(_interactWith);
                if (_interactWith == null)
                {
                    var target = GetGlobalMousePosition();
                    _character.MoveTo(target);
                }
                else
                {
                    _character.MoveTo(_interactWith.point.GlobalPosition, _interactWith);
                }
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

        private InteractiveObject GetInteractiveObject(int shapeIdx)
        {
            return _interactiveObjects.GetChild<InteractiveObject>(shapeIdx);
        }

        private void OnInteractiveObjectsInputEvent(object viewport, object @event, int shapeIdx)
        {
            if (@event is InputEventMouseButton)
            {
                var mouseEvent = (InputEventMouseButton)@event;
                if (mouseEvent.Pressed)
                {
                    _interactWith = GetInteractiveObject(shapeIdx);
                }
            }

            if (@event is InputEventMouseMotion)
            {
                if (interactiveJustEntered)
                {
                    var obj = GetInteractiveObject(shapeIdx);
                    _interiorActiveTileMap.SetActiveTile(obj.tileCell);
                }
                interactiveJustEntered = false;
            }
        }

        private void OnInteractiveObjectsMouseEntered()
        {
            interactiveJustEntered = true;
            Input.SetDefaultCursorShape(Input.CursorShape.PointingHand);
        }

        private void OnInteractiveObjectsMouseExited()
        {
            _interiorActiveTileMap.ResetActiveTile();
            Input.SetDefaultCursorShape();
        }
    }
}
