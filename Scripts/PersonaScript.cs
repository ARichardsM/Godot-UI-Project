using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

public partial class PersonaScript : Node
{
    // Declare variables
    //private int currentCategoryNum = Global.Data.newEntity.Count;
    private int selectNum = 0;
   // public string currHBox;
    public Global.persona newPersona;

    // Declare Select Containers
    private HBoxContainer fullCont;
    private HBoxContainer pngCont;
    private HBoxContainer txtCont;

    // Declare Subtitle
    private Label subtitle;

    // Declare Select Display
    private TextureRect sprite;
    private Label text;

    // Function for button presses
    public void ButtonPressed(int val)
    {
        switch (val)
        {
            // Load the previous file
            case 0:
                if (selectNum > 0)
                    --selectNum;
                    updateSubScreen();

                break;
            // Load the next file
            case 1:
                //int max = Global.Data.database[currentCategoryNum].opts.Count - 1;

                if (selectNum < 10) { 
                    ++selectNum;
                    updateSubScreen();
                }

                break;
            // Add Data
            case 2:
                // Add to the new data point
                //string newKey = Global.Data.database[currentCategoryNum].name;
                //string newVal = Global.Data.database[currentCategoryNum].opts[currentTraitNum].name;
                //Global.Data.newEntity.Add(new Global.trait(newKey, newVal));

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

    // Function for button presses
    public void SelectShift(bool shiftUp)
    {
        // Determine current values
        int currTrait = newPersona.traits.Count;

        if (shiftUp)
        {
            --selectNum;
        }
        else
        {
            ++selectNum;
        }

        if (selectNum < 0)
            selectNum = Global.Data.database[currTrait].opts.Count - 1;

        if (selectNum > Global.Data.database[currTrait].opts.Count)
            selectNum = 0;

        updateSubScreen();
    }

    // Update the entire screen
    private void updateFullScreen()
    {
        // Determine current values
        int currTrait = newPersona.traits.Count;

        // Bound by Directory
        if (currTrait >= Global.Data.database.Count)
            return;

        // Edit Sub-Title
        subtitle = GetNode<Label>("Sub-Title");
        subtitle.Text = Global.Data.database[currTrait].name + " Category Menu";

        // Update the Sub Screen
        updateSubScreen();
    }

    // Update the file section of the screen
    private void updateSubScreen()
    {
        // Determine current values
        int currTrait = newPersona.traits.Count;

        // Bound by File
        if (selectNum >= Global.Data.database[currTrait].opts.Count)
            return;

        // Determine directory and file
        string currDir = Global.Data.database[currTrait].name;
        string currFile = Global.Data.database[currTrait].opts[selectNum].name;

        // Reset Containers
        fullCont.Visible = false;
        pngCont.Visible = false;
        txtCont.Visible = false;

        // Determine Screen Style
        int screenStyle = 0;

        screenStyle += Global.Data.database[currTrait].opts[selectNum].png ? 2 : 0;
        screenStyle += Global.Data.database[currTrait].opts[selectNum].txt ? 1 : 0;

        switch (screenStyle)
        {
            // PNG + TXT
            case 3:
                fullCont.Visible = true;
                sprite = GetNode<TextureRect>("SelectMenu/FullContainer/TextureRect");
                sprite.SetTexture((Texture2D)GD.Load("res://Input/" + currDir + "/" + currFile + ".png"));
                text = GetNode<Label>("SelectMenu/FullContainer/Label");
                text.SetText(currFile + "\n\n" + File.ReadAllText("Input/" + currDir + "/" + currFile + ".txt"));
                break;
            // PNG
            case 2:
                pngCont.Visible = true;
                sprite = GetNode<TextureRect>("SelectMenu/PngContainer/TextureRect");
                sprite.SetTexture((Texture2D)GD.Load("res://Input/" + currDir + "/" + currFile + ".png"));
                break;
            // TXT
            case 1:
                txtCont.Visible = true;
                text = GetNode<Label>("SelectMenu/TxtContainer/Label");
                text.SetText(currFile + "\n\n" + File.ReadAllText("Input/" + currDir + "/" + currFile + ".txt"));
                break;
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        // Initialize Persona
        newPersona = new Global.persona();

        // Declare Containers
        fullCont = GetNode<HBoxContainer>("SelectMenu/FullContainer");
        pngCont = GetNode<HBoxContainer>("SelectMenu/PngContainer");
        txtCont = GetNode<HBoxContainer>("SelectMenu/TxtContainer");

        // Set initial screen
        updateFullScreen();
    }
}
