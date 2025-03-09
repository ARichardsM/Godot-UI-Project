using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// Creates characters and adds them to the data matrix.
/// </summary>
public partial class CharacterScript : Node2D
{
    public void ButtonPressed(int val)
    {
        switch (val)
        {
            case 0:
                // Get each on-screen selection scale
                List<ScaleSelection> scales = new List<ScaleSelection>();
                scales.Add(this.GetNode<ScaleSelection>("Selection1"));
                scales.Add(this.GetNode<ScaleSelection>("Selection2"));
                scales.Add(this.GetNode<ScaleSelection>("Selection3"));
                scales.Add(this.GetNode<ScaleSelection>("Selection4"));

                // Selected button list
                List<int> selectedList = new List<int> ();

                // Verify all scales
                foreach (ScaleSelection currScale in scales)
                {
                    // Cancel if a button hasn't been selected
                    if (currScale.ButtonSelected == 0)
                    {
                        GD.Print("Verify Failed: Button Unselected");
                        return;
                    }

                    // Add the button value to the selected button list
                    selectedList.Add(currScale.ButtonSelected);
                }

                // Add the selected buttons to Data
                Global.Data.dataMatrix.Add(selectedList);
                break;

            case 1:
                // Return to the main menu
                GetTree().ChangeSceneToFile("Scenes/MainMenu.tscn");
                break;

        }
    }
}
