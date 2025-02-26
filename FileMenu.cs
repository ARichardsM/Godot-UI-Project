using Godot;
using System;

public partial class FileMenu : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ButtonPressed(int val)
	{
        switch (val)
        {
            case 0:
                GD.Print("Left Button");
                break;

            case 1:
                GD.Print("Right Button");
                break;

            case 2:
                GD.Print("Bottom Button");
                break;

        }

    }
}
