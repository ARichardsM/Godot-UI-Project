using Godot;
using System;

public partial class CharacterScript : Node2D
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
        GD.Print(Global.Data.num);
        Global.Data.num += 1;
        switch (val)
        {
            case 0:
                GD.Print("Left Button");
                ScaleSelection w = this.GetNode<ScaleSelection>("Selection1");
                GD.Print("Group 1: " + w.ButtonSelected);
                w = this.GetNode<ScaleSelection>("Selection2");
                GD.Print("Group 2: " + w.ButtonSelected);
                w = this.GetNode<ScaleSelection>("Selection3");
                GD.Print("Group 3: " + w.ButtonSelected);
                w = this.GetNode<ScaleSelection>("Selection4");
                GD.Print("Group 4: " + w.ButtonSelected);
                break;

            case 1:
                GD.Print("Right Button");
                GetTree().ChangeSceneToFile("MainMenu.tscn");
                break;

        }
    }
}
