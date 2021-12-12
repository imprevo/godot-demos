using Godot;
using System;

namespace Main
{
    public class Character : KinematicBody2D
    {
        public Navigation2D navigation;

        private int _pathIndex = 0;
        private int _speed = 200;
        private Vector2[] _path = new Vector2[0];
        private InteractiveObject _interactWith;

        public override void _Ready()
        {
            SetPhysicsProcess(false);
        }

        public override void _PhysicsProcess(float delta)
        {
            if (_path.Length < _pathIndex + 1)
            {
                SetPhysicsProcess(false);
                _interactWith?.Interact();
                _interactWith = null;
            }
            else
            {
                PhysicsMoveCharacter();
            }
        }

        public void MoveTo(Vector2 target, InteractiveObject interactWith = null)
        {
            _path = GetMovePath(target);
            _pathIndex = 0;
            _interactWith = interactWith;
            SetPhysicsProcess(true);
        }

        private void PhysicsMoveCharacter()
        {
            var targetPosition = _path[_pathIndex];
            var velocity = (targetPosition - GlobalPosition).Normalized() * _speed;
            MoveAndSlide(velocity, Vector2.Zero);

            if (targetPosition.DistanceTo(GlobalPosition) < 10)
            {
                _pathIndex++;
            }
        }

        private Vector2[] GetMovePath(Vector2 target)
        {
            return navigation.GetSimplePath(GlobalPosition, target, false);
        }
    }
}
