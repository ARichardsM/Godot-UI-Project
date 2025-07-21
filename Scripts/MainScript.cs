using Godot;
using System;
using System.Collections.Generic;

public partial class MainScript : Node2D
{
    // Function for button presses
    public void ButtonPressed(int val)
    {
        switch (val)
        {
            // Handle adding an entity
            case 0:
                // Go through each user directory in the database
                for (int i = 0; i < Global.Data.database.Count; i++) {
                    GD.Print(Global.Data.database[i].name);
                }

                // Determine general characteristics
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
