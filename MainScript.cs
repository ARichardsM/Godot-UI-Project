using Godot;
using System;

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
        switch (val)
        {
            case 0:
                GD.Print("Top Button");
                break;

            case 1:
                GD.Print("Mid Button");
                break;

            case 2:
                GD.Print("Bot Button");
                break;

        }

        GetTree().ChangeSceneToFile("CharacterMenu.tscn");
    }
}
