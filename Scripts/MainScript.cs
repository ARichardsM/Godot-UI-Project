using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;

public partial class MainScript : Node2D
{
    // Write the roster
    private void writeRoster()
    {
        // Declare Variables
        string outText = "";
        List<List<string>> data = new List<List<string>>();
        List<string> headerData = new List<string>();
        List<string> templateData = new List<string>();

        // Append Name Header
        outText += "Name,";
        headerData.Add("Name");
        templateData.Add("N/A");

        // Append Directory Headers
        foreach (DirectoryManager.userDirectory dir in Global.Data.database)
        {
            outText += dir.name + ",";
            headerData.Add(dir.name);
            templateData.Add("N/A");
        }

        // Append Personality Headers
        outText += "EI,SN,TF,JP";
        List<string> personalityMatrix = new() { "EI", "SN", "TF", "JP" };
        foreach (string pers in personalityMatrix)
        {
            headerData.Add(pers);
            templateData.Add("N/A");
        }

        // Convert each observation into lists of string
        foreach (Global.persona savedObs in Global.Data.roster)
        {
            List<string> tempData = new List<string>(templateData);

            tempData[0] = savedObs.name;

            foreach(Global.trait aspect in savedObs.traits) 
            {
                int index = headerData.IndexOf(aspect.key);

                // Remove Unaccounted Traits
                if (index == -1)
                    continue;

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

        // Write to File
        System.IO.File.WriteAllText("Output/Data.csv", outText);
    }

    // Print the roster
    private void printRoster()
    {
        foreach (var item in Global.Data.roster)
        {
            GD.Print(item.ToString());
        }
    }

    // Function for button presses
    public void ButtonPressed(int val)
    {
        switch (val)
        {
            // Button-T
            case 0:
                // If there are categories, go to category menu
                if (Global.Data.newEntity.Count < Global.Data.database.Count)
                {
                    GetTree().ChangeSceneToFile("Scenes/CategoryMenu.tscn");
                    break;
                }
                // Else, go to the character menu
                else
                {
                    GetTree().ChangeSceneToFile("Scenes/CharacterMenu.tscn");
                    break;
                }

            // Button-ML
            case 1:
                // Print to Screen
                printRoster();
                break;

            // Button-MR
            case 2:
                // Print to File
                writeRoster();

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
