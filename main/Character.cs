using Godot;
using System;

namespace Main
{
    enum Direction
    {
        LEFT,
        RIGHT,
        UP,
        DOWN,
    }

    public class Character : KinematicBody2D
    {
        public Navigation2D navigation;

        private int _pathIndex = 0;
        private int _speed = 150;
        private Vector2[] _path = new Vector2[0];
        private InteractiveObject _interactWith;
        private AnimationNodeStateMachinePlayback _animationStateMachine;
        private AnimationPlayer _animationPlayer;
        private Direction _direction = Direction.DOWN;

        public override void _Ready()
        {
            _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        }

        public override void _PhysicsProcess(float delta)
        {
            if (_path.Length < _pathIndex + 1)
            {
                StopCharacter();
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
            _direction = GetDirection(velocity);
            RunAnimation("run");

            if (targetPosition.DistanceTo(GlobalPosition) < 5)
            {
                _pathIndex++;
            }
        }

        private void StopCharacter()
        {
            SetPhysicsProcess(false);
            _interactWith?.Interact();
            _interactWith = null;
            RunAnimation("idle");
        }

        private Vector2[] GetMovePath(Vector2 target)
        {
            return navigation.GetSimplePath(GlobalPosition, target, false);
        }

        private Direction GetDirection(Vector2 velocity)
        {
            var angle = Mathf.Rad2Deg(velocity.Angle());

            if (angle > 45 && angle <= 135)
            {
                return Direction.DOWN;
            }

            if (angle > -45 && angle <= 45)
            {
                return Direction.RIGHT;
            }

            if (angle > -135 && angle >= -45)
            {
                return Direction.LEFT;
            }

            return Direction.UP;
        }

        private string GetAnimationName(string name)
        {
            switch (_direction)
            {
                case Direction.LEFT:
                    return name + "-left";
                case Direction.RIGHT:
                    return name + "-right";
                case Direction.UP:
                    return name + "-up";
                case Direction.DOWN:
                default:
                    return name + "-down";
            }
        }

        private void RunAnimation(string name)
        {
            _animationPlayer.CurrentAnimation = GetAnimationName(name);
        }
    }
}
