using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

public partial class MainScript : Node
{
    // Write the roster
    private void writeRoster()
    {
        // Declare Recording Variables
        List<List<string>> data = new List<List<string>>();
        string outText = "";
        List<string> headerData = new List<string>();
        List<string> templateData = new List<string>();

        // Append Name Header
        outText += "Name,";
        headerData.Add("Name");
        templateData.Add("N/A");

        // Determine maximum number of aspects
        int aspectMax = 0;
        foreach (Global.persona entity in Global.Data.roster)
            aspectMax = Math.Max(aspectMax, entity.traits.Count);

        // Append Headers
        for (int i = 0; i < aspectMax; i++)
        {
            outText += "Aspect " + (i+1) + ",";
            headerData.Add("Aspect " + (i + 1) + ",");
            templateData.Add("N/A");
        }

        // Convert each persona into a csv line
        foreach (Global.persona entity in Global.Data.roster)
        {
            List<string> tempData = new(templateData);

            // Record the observation's name
            tempData[0] = entity.name;

            // Record the observation's traits
            for (int i = 0; i < aspectMax; i++ )
            {
                if (i < entity.traits.Count)
                    tempData[i + 1] = entity.traits[i].key + " - " + entity.traits[i].val;
                else
                    tempData[i + 1] = "N/A";
            }

            // Add the observations data to the data list
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

    // Write the roster TBD: Depreciated Method
    private void writeRosterOld()
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

            // Record the observation's name
            tempData[0] = savedObs.name;

            // Record the observation's traits
            foreach (Global.trait aspect in savedObs.traits) 
            {
                int index = headerData.IndexOf(aspect.key);

                // Remove Unaccounted Traits
                if (index == -1)
                    continue;

                tempData[index] = aspect.val;
            }

            // Add the observations data to the data list
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
    public void ButtonPressed(int row,int col)
    {
        void topLayer()
        {
            switch (col)
            {
                // Left Column: Persona Menu
                case 0:
                    GetTree().ChangeSceneToFile("Scenes/PersonaMenu.tscn");
                    break;

                // Middle Column: Relation Menu
                case 1:
                    GetTree().ChangeSceneToFile("Scenes/RelationMenu.tscn");
                    break;

                // Right Column: Stable Menu
                case 2:
                    GetTree().ChangeSceneToFile("Scenes/StableMenu.tscn");
                    break;
            }
        }

        void midLayer()
        {
            switch (col)
            {
                // Left Column: Print
                case 0:
                    printRoster();
                    break;

                // Right Column: Write
                case 1:
                    // Print to File
                    writeRoster();

                    // Report to GD Console
                    GD.Print("File written.");
                    break;
            }
        }

        switch (row)
        {
            // Top Row
            case 0:
                topLayer();
                break;

            // Middle Row
            case 1:
                midLayer();
                break;

            // Bottom Row
            case 2:
                GetTree().Quit();
                break;
        }
    }
}
