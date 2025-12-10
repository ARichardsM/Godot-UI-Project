using Godot;
using System;
using System.Collections.Generic;

public partial class MainScript : Node2D
{
    // Return saved data as a single string
    private string outputData()
    {
        // Declare Variables
        string outText = "";
        List<List<string>> data = new List<List<string>>();
        List<string> headerData = new List<string>();
        List<string> templateData = new List<string>();


        // Write header
        foreach (Global.userDirectory dir in Global.Data.database)
        {
            outText += dir.name + ",";
            headerData.Add(dir.name);
        }
            
        outText += "EI,SN,TF,JP";

        List<string> personalityMatrix = new() { "EI", "SN", "TF", "JP" };
        foreach (string pers in personalityMatrix)
        {
            headerData.Add(pers);
        }

        headerData.Add("EI");
        headerData.Add("SN");
        headerData.Add("TF");
        headerData.Add("JP");

        // Stringify each observation
        foreach (List<string> savedObs in Global.Data.dataEntries)
        {
            outText += "\n" + savedObs[0];

            for (int i = 1; i < savedObs.Count; i++)
                outText += "," + savedObs[i];
        }

        // Return
        return outText;
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
                GD.Print(outputData());
                break;

            // Print to File
            case 2:
                System.IO.File.WriteAllText("Output/Data.csv", outputData());

                // Report to GD Console
                GD.Print("File written.");
                break;

            // Exit
            case 3:
                GetTree().Quit();
                break;
        } 
    }
}
