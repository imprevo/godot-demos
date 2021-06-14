using Godot;
using System;

public class scene : Node2D
{
    private int count = 0;

    public override void _Ready()
    {

    }

    private void _on_Button_pressed()
    {
        count++;
        GetNode<Label>("Label").Text = "Pressed " + count + " times";
    }
}
