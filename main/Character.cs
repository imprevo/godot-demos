using Godot;

namespace Main
{
    public class Character : KinematicBody2D
    {
        public Navigation2D navigation;

        private int _pathIndex = 0;
        private int _speed = 150;
        private int _targetThreshold = 5;
        private Vector2[] _path = new Vector2[0];
        private InteractiveObject _interactWith;
        private AnimationTree _animationTree;
        private AnimationNodeStateMachinePlayback _animationStateMachine;
        private Vector2 _velocity = Vector2.Down;

        public override void _Ready()
        {
            _animationTree = GetNode<AnimationTree>("AnimationTree");
            _animationStateMachine = (AnimationNodeStateMachinePlayback)_animationTree.Get("parameters/playback");
            _animationTree.Active = true;
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
            _velocity = MoveAndSlide(velocity, Vector2.Zero);
            RunAnimation("run");

            if (targetPosition.DistanceTo(GlobalPosition) < _targetThreshold)
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

        private void RunAnimation(string name)
        {
            _animationStateMachine.Travel(name);
            _animationTree.Set("parameters/idle/blend_position", _velocity);
            _animationTree.Set("parameters/run/blend_position", _velocity);
        }
    }
}
