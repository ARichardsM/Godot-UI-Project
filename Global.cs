using Godot;
using System;

public partial class Global : Node
{
    // Create an instance
    public static Global Data { get; private set; }

    public override void _Ready()
    {
        Data = this;
    }

    public int Health { get; set; }

    

    public int num = 12;
}
