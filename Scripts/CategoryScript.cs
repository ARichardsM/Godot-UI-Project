using Godot;
using System;
using System.Drawing;

public partial class CategoryScript : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        // Determine initial values
        string currDir = Global.Data.database[Global.Data.databaseNum].name;
        string currFile = Global.Data.database[Global.Data.databaseNum].opts[0].name;

        // Edit Sub-Title
        var subtitle = GetNode<Label>("Sub-Title");
		subtitle.Text = currDir + " Category Menu";

        var sprite = GetNode<Sprite2D>("Area2D/Sprite2D");
		//z.Texture.Set()
		var w = GD.Load("res://Input/Element/Earth.png");
		
        GD.Print(currDir);
        sprite.SetTexture((Texture2D) w);
		//float newscale = sprite.Texture.GetWidth();
        //float newscalew = sprite.Texture.GetHeight();
		float newScale = 250.0f / Math.Max(sprite.Texture.GetWidth(), sprite.Texture.GetHeight());
		//GD.Print(newscale +" " + newscalew);
		Vector2 newScaleVec = new Vector2(newScale, newScale);
		sprite.Scale = newScaleVec;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    // Function for button presses
    public void ButtonPressed(int val)
    {
        GD.Print(val);
        switch (val)
        {
            // Handle adding an entity
            case 0:
                break;
            // Switch to file menu
            case 1:
                break;
            // Exit
            case 2:
                break;

        }


    }
}
