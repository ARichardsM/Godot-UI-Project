using Godot;
using System;
using System.Collections.Generic;

public partial class MainScript : Node2D
{
    private void printToScreen()
    {
        // Output Variable
        string outText = "";

        // Write header
        foreach (Global.userDirectory dir in Global.Data.database)
            outText += dir.name + " ";
        outText += "EI SN TF JP ";

        // Stringify each observation
        foreach (List<string> savedObs in Global.Data.dataEntries)
        {
            outText += "\n";

            foreach (string obs in savedObs)
                outText += obs + " ";
        }

        // Report to GD Console
        GD.Print(outText);
    }
    
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

                for (int i = 0; i < Global.Data.newData.Count; i++)
                {
                    GD.Print(Global.Data.newData[i]);
                }

                // Determine general characteristics
                GetTree().ChangeSceneToFile("Scenes/CategoryMenu.tscn");
                break;

            // Print to Screen
            case 1:
                printToScreen();
                break;

            // TBD: Print to File
            case 2:
                GetTree().ChangeSceneToFile("Scenes/FileMenu.tscn");
                break;

            // Exit
            case 3:
                GetTree().Quit();
                break;

        }

        
    }
}
