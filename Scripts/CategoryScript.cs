using Godot;
using System;
using System.Drawing;
using System.IO;
using System.Linq;

public partial class CategoryScript : Node
{
    // Declare variables
    private int currentCategoryNum = Global.Data.newEntity.Count;
    private int currentTraitNum = 0;
    public string currHBox;

    // Function for button presses
    public void ButtonPressed(int val)
    {
        switch (val)
        {
            // Load the previous file
            case 0:
                if (currentTraitNum > 0)
                    updateScreen(--currentTraitNum);

                break;
            // Load the next file
            case 1:
                int max = Global.Data.database[currentCategoryNum].opts.Count - 1;

                if (currentTraitNum < max)
                    updateScreen(++currentTraitNum);

                break;
            // Add Data
            case 2:
                // Add to the new data point
                string newKey = Global.Data.database[currentCategoryNum].name;
                string newVal = Global.Data.database[currentCategoryNum].opts[currentTraitNum].name;
                Global.Data.newEntity.Add(new Global.trait(newKey, newVal));

                // If the new category exists, reload the tree to load the new directory
                if (Global.Data.newEntity.Count < Global.Data.database.Count)
                {
                    GetTree().ReloadCurrentScene();
                }
                // Else, go to the character menu
                else
                {
                    GetTree().ChangeSceneToFile("Scenes/CharacterMenu.tscn");
                    break;
                }

                break;
            // Exit
            case 3:
                // Clear new entity
                Global.Data.newEntity.Clear();

                // Return to the main menu
                GetTree().ChangeSceneToFile("Scenes/MainMenu.tscn");
                break;

        }
    }

    // Update the screen
    public void updateScreen(int val)
    {
        // Bound check val
        if ((val < 0) || (val >= Global.Data.database[currentCategoryNum].opts.Count))
            return;

        // Determine Screen Style
        int screenStyle = 0;

        screenStyle += Global.Data.database[currentCategoryNum].opts[val].png ? 2 : 0;
        screenStyle += Global.Data.database[currentCategoryNum].opts[val].txt ? 1 : 0;

        switch (screenStyle) {
            // PNG + TXT
            case 3:
                currHBox = "FullContainer";
                GetNode<HBoxContainer>("FullContainer").Visible = true;
                GetNode<HBoxContainer>("PngContainer").Visible = false;
                GetNode<HBoxContainer>("TxtContainer").Visible = false;
                updateSprite(val);
                updateText(val);
                break;
            // PNG
            case 2:
                currHBox = "PngContainer";
                GetNode<HBoxContainer>("FullContainer").Visible = false;
                GetNode<HBoxContainer>("PngContainer").Visible = true;
                GetNode<HBoxContainer>("TxtContainer").Visible = false;
                updateSprite(val);
                break;
            // TXT
            case 1:
                currHBox = "TxtContainer";
                GetNode<HBoxContainer>("FullContainer").Visible = false;
                GetNode<HBoxContainer>("PngContainer").Visible = false;
                GetNode<HBoxContainer>("TxtContainer").Visible = true;
                updateText(val);
                break;
        }
    }

    // Update the file sprite
    public void updateSprite(int val)
    {
        // Bound check val
        if ((val < 0) || (val >= Global.Data.database[currentCategoryNum].opts.Count))
            return;

        // Check for .PNG
        if (!Global.Data.database[currentCategoryNum].opts[val].png)
            return;

        // Determine values
        string currDir = Global.Data.database[currentCategoryNum].name;
        string currFile = Global.Data.database[currentCategoryNum].opts[currentTraitNum].name;

        // Set sprite
        var sprite = GetNode<TextureRect>(currHBox + "/TextureRect");
        var spriteTexture = GD.Load("res://Input/" + currDir +"/" + currFile + ".png");
        sprite.SetTexture((Texture2D) spriteTexture);
    }

    // Update the file text
    public void updateText(int val)
    {
        // Bound check val
        if ((val < 0) || (val >= Global.Data.database[currentCategoryNum].opts.Count))
            return;

        // Check for .TXT
        if (!Global.Data.database[currentCategoryNum].opts[val].txt)
            return;

        // Determine values
        string currDir = Global.Data.database[currentCategoryNum].name;
        string currFile = Global.Data.database[currentCategoryNum].opts[currentTraitNum].name;

        // Set text
        var text = GetNode<Label>(currHBox + "/Label");
        string fileText = currFile + "\n\n";
        fileText += File.ReadAllText("Input/" + currDir + "/" + currFile + ".txt");
        text.SetText(fileText);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        // Determine initial values
        string currDir = Global.Data.database[currentCategoryNum].name;

        // Edit Sub-Title
        var subtitle = GetNode<Label>("Sub-Title");
		subtitle.Text = currDir + " Category Menu";

        // Set initial screen
        updateScreen(currentTraitNum);
    }
}
