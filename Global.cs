using Godot;
using System;
using System.Collections.Generic;

public partial class Global : Node
{
    // Create an instance
    public static Global Data { get; private set; }

    public override void _Ready()
    {
        Data = this;
    }

    public List<List<int>> dataMatrix = new List<List<int>>();

    public int num = 12;
}
