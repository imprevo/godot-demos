using Godot;
using System;
using System.Collections.Generic;

public class Block
{
    public List<Vector2> cells;

    public Block(List<Vector2> _cells)
    {
        cells = _cells;
    }

    public Block Rotate()
    {
        return new Block(RotateClockwise());
    }

    private List<Vector2> RotateClockwise()
    {
        var rotated = new List<Vector2>();
        foreach (var cell in cells)
        {
            rotated.Add(new Vector2(cell.y, -cell.x));
        }
        return rotated;
    }
}
