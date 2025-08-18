using Godot;
using System;
using System.Drawing;
using System.IO;

public partial class CategoryScript : Node
{
    // Declare variables
    public int fileSel = 0;

    // Function for button presses
    public void ButtonPressed(int val)
    {
        GD.Print(val);
        switch (val)
        {
            // Load the previous file
            case 0:
                if (fileSel > 0) {
                    updateSprite(--fileSel);
                    updateText(fileSel);
                }

                break;
            // Load the next file
            case 1:
                int max = Global.Data.database[Global.Data.databaseNum].opts.Count - 1;

                if (fileSel < max) {
                    updateSprite(++fileSel);
                    updateText(fileSel);
                }

                break;
            // Exit
            case 2:
                break;

        }


    }

    // Update the file sprite
    public void updateSprite(int val)
    {
        // Bound check val
        if ((val < 0) || (val >= Global.Data.database[Global.Data.databaseNum].opts.Count))
            return;

        // Check for .PNG
        if (!Global.Data.database[Global.Data.databaseNum].opts[val].png)
            return;

        // Determine values
        string currDir = Global.Data.database[Global.Data.databaseNum].name;
        string currFile = Global.Data.database[Global.Data.databaseNum].opts[fileSel].name;

        // Set sprite
        var sprite = GetNode<TextureRect>("HBoxContainer/TextureRect");
        var spriteTexture = GD.Load("res://Input/" + currDir +"/" + currFile + ".png");
        sprite.SetTexture((Texture2D) spriteTexture);
    }

    // Update the file text
    public void updateText(int val)
    {
        // Bound check val
        if ((val < 0) || (val >= Global.Data.database[Global.Data.databaseNum].opts.Count))
            return;

        // Check for .TXT
        if (!Global.Data.database[Global.Data.databaseNum].opts[val].txt)
            return;

        // Determine values
        string currDir = Global.Data.database[Global.Data.databaseNum].name;
        string currFile = Global.Data.database[Global.Data.databaseNum].opts[fileSel].name;

        // Set text
        var text = GetNode<Label>("HBoxContainer/Label");
        string fileText = File.ReadAllText("Input/" + currDir + "/" + currFile + ".txt");
        text.SetText(fileText);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        // Determine initial values
        string currDir = Global.Data.database[Global.Data.databaseNum].name;

        // Edit Sub-Title
        var subtitle = GetNode<Label>("Sub-Title");
		subtitle.Text = currDir + " Category Menu";

        // Set initial sprite
        updateSprite(fileSel);
        updateText(fileSel);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	} 
}
