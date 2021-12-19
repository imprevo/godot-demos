using Godot;

public class CharacterCamera : Godot.Camera2D
{
    private Node2D _target;
    private int _speed = 5;

    public override void _Process(float delta)
    {
        if (_target != null)
        {
            Position = new Vector2(
                Mathf.Lerp(Position.x, _target.GlobalPosition.x, _speed * delta),
                Mathf.Lerp(Position.y, _target.GlobalPosition.y, _speed * delta)
            );
        }
    }

    public void SetTarget(Node2D target)
    {
        _target = target;
        Position = target.GlobalPosition;
    }
}
