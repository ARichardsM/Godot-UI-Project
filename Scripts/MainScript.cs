using Godot;
using System;
using System.Collections.Generic;

public partial class MainScript : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        GD.Print("Hello from C# to Godot :)");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

    public void ButtonPressed(int val)
    {
        GD.Print(Global.Data.num);
        Global.Data.num += 1;
        GD.Print(Global.Data.dataMatrix.Count);
        foreach (List<int> w in Global.Data.dataMatrix)
        {
            GD.Print(w[0]);
        }

        switch (val)
        {
            case 0:
                GD.Print("Character Menu");
                GetTree().ChangeSceneToFile("Scenes/CharacterMenu.tscn");
                break;

            case 1:
                GD.Print("File Menu");
                GetTree().ChangeSceneToFile("Scenes/FileMenu.tscn");
                break;

            case 2:
                GD.Print("Bot Button");
                break;

        }

        
    }
}
