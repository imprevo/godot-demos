using Godot;
using System;

public class SnakeUtils
{
    static public Vector2 Follow(Vector2 velocity, Vector2 position, Vector2 targetPosition, float maxSpeed, float mass)
    {
        var nextVelocity = (targetPosition - position).Normalized() * maxSpeed;
        var steering = (nextVelocity - velocity) / mass;
        return velocity + steering;
    }

    static public Vector2 ArriveTo(Vector2 velocity, Vector2 position, Vector2 targetPosition, float maxSpeed, float slowRadius, float mass)
    {
        var nextVelocity = (targetPosition - position).Normalized() * maxSpeed;
        var distanceToTarget = position.DistanceTo(targetPosition);
        if (distanceToTarget < slowRadius)
        {
            nextVelocity *= (distanceToTarget / slowRadius) * (float)(0.9) + (float)(0.1);
        }
        var steering = (nextVelocity - velocity) / mass;
        return velocity + steering;
    }
}
