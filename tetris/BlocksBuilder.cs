using Godot;
using System;
using System.Collections.Generic;

public class BlocksBuilder : Node2D
{
    private RandomNumberGenerator rng = new RandomNumberGenerator();
    private List<Block> blocks = new List<Block>();

    public BlocksBuilder()
    {
        rng.Randomize();
        blocks.Add(CreateBlockZ());
        blocks.Add(CreateBlockT());
        blocks.Add(CreateBlockI());
        blocks.Add(CreateBlockL());
        blocks.Add(CreateBlockJ());
        blocks.Add(CreateBlockO());
    }

    public Block GetBlock()
    {
        var index = rng.RandiRange(0, blocks.Count - 1);
        return blocks[index];
    }

    private Block CreateBlockZ()
    {
        List<Vector2> block = new List<Vector2>(4);
        block.Add(new Vector2(-1, -1));
        block.Add(new Vector2(0, -1));
        block.Add(new Vector2(0, 0));
        block.Add(new Vector2(1, 0));
        return new Block(block);
    }

    private Block CreateBlockT()
    {
        List<Vector2> block = new List<Vector2>(4);
        block.Add(new Vector2(-1, -1));
        block.Add(new Vector2(0, -1));
        block.Add(new Vector2(1, -1));
        block.Add(new Vector2(0, 0));
        return new Block(block);
    }

    private Block CreateBlockI()
    {
        List<Vector2> block = new List<Vector2>(4);
        block.Add(new Vector2(0, -3));
        block.Add(new Vector2(0, -2));
        block.Add(new Vector2(0, -1));
        block.Add(new Vector2(0, 0));
        return new Block(block);
    }

    private Block CreateBlockL()
    {
        List<Vector2> block = new List<Vector2>(4);
        block.Add(new Vector2(0, -2));
        block.Add(new Vector2(0, -1));
        block.Add(new Vector2(0, 0));
        block.Add(new Vector2(1, 0));
        return new Block(block);
    }

    private Block CreateBlockJ()
    {
        List<Vector2> block = new List<Vector2>(4);
        block.Add(new Vector2(1, -2));
        block.Add(new Vector2(1, -1));
        block.Add(new Vector2(1, 0));
        block.Add(new Vector2(0, 0));
        return new Block(block);
    }

    private Block CreateBlockO()
    {
        List<Vector2> block = new List<Vector2>(4);
        block.Add(new Vector2(0, -1));
        block.Add(new Vector2(1, -1));
        block.Add(new Vector2(0, 0));
        block.Add(new Vector2(1, 0));
        return new Block(block);
    }
}
