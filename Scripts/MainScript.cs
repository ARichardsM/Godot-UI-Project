using Godot;
using System;
using System.Collections.Generic;

public partial class MainScript : Node2D
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
            // Switch to character menu
            case 0:
                GetTree().ChangeSceneToFile("Scenes/CharacterMenu.tscn");
                break;

            // Switch to file menu
            case 1:
                GetTree().ChangeSceneToFile("Scenes/FileMenu.tscn");
                break;

            // Exit
            case 2:
                GetTree().Quit();
                break;

        }

        
    }
}
