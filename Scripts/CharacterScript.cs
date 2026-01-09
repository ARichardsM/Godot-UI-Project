using Godot;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class CharacterScript : Node2D
{
    // All on-screen selection scale
    List<ScaleSelection> scales = new List<ScaleSelection>();

    public void ButtonPressed(int val)
    {
        switch (val)
        {
            case 0:
                // Add Personality Traits
                List<string> personalityMatrix = new() { "EI", "SN", "TF", "JP" };
                for (int i = 0; i < personalityMatrix.Count; i++)
                {
                    // Cancel if a button hasn't been selected
                    if (scales[i].ButtonSelected == 0)
                    {
                        GD.Print("Verify Failed: Button Unselected");
                        return;
                    }

                    // Log trait
                    Global.Data.newEntity.Add(new Global.trait(personalityMatrix[i], scales[i].ButtonSelected.ToString()));
                }

                // Add to Entity Data List
                Global.Data.entityData.Add(new List<Global.trait> (Global.Data.newEntity));

                // Clear new entity
                Global.Data.newEntity.Clear();

                // Return to the main menu
                GetTree().ChangeSceneToFile("Scenes/MainMenu.tscn");
                break;

            case 1:
                // Clear new entity
                Global.Data.newEntity.Clear();

                // Return to the main menu
                GetTree().ChangeSceneToFile("Scenes/MainMenu.tscn");
                break;

        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Log each on-screen selection scale
        scales.Add(this.GetNode<ScaleSelection>("Selection1"));
        scales.Add(this.GetNode<ScaleSelection>("Selection2"));
        scales.Add(this.GetNode<ScaleSelection>("Selection3"));
        scales.Add(this.GetNode<ScaleSelection>("Selection4"));
    }
}
