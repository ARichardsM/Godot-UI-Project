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
            templateData.Add("N/A");
        }
            
        outText += "EI,SN,TF,JP";

        List<string> personalityMatrix = new() { "EI", "SN", "TF", "JP" };
        foreach (string pers in personalityMatrix)
        {
            headerData.Add(pers);
            templateData.Add("N/A");
        }

        // Convert each observation into lists of string
        foreach (List<Global.trait> savedObs in Global.Data.entityData)
        {
            List<string> tempData = new List<string>(templateData);

            foreach (Global.trait aspect in savedObs)
            {
                int index = headerData.IndexOf(aspect.key);

                tempData[index] = aspect.val;
            }

            data.Add(tempData);
        }

        // Stringify each observation
        foreach (List<string> savedObs in data)
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
            // Button-T
            case 0:
                // Switch to category menu
                GetTree().ChangeSceneToFile("Scenes/CategoryMenu.tscn");
                break;

            // Button-ML
            case 1:
                // Print to Screen
                GD.Print(outputData());
                break;

            // Button-MR
            case 2:
                // Print to File
                System.IO.File.WriteAllText("Output/Data.csv", outputData());

                // Report to GD Console
                GD.Print("File written.");
                break;

            // Button-B
            case 3:
                // Exit
                GetTree().Quit();
                break;
        } 
    }
}
