using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using static Global;

public partial class FileScript : Node2D
{
	public void ButtonPressed(int val)
	{
        switch (val)
        {
            // Pull File Information
            case 0:
                try
                {
                    // Read the data file
                    string fileText = File.ReadAllText("Input/Data.csv");

                    // Split the string
                    string[] fileCon = fileText.Split("\n");
                    string[] fileHead = fileCon[0].Split(",");
                    
                    // Add each observation to the global data
                    for (int i = 1; i < fileCon.Length; i++)
                    {
                        List<int> dataLine = new List<int>();
                        string[] line = fileCon[i].Split(",");

                        foreach(string num in line)
                        {
                            dataLine.Add(int.Parse(num));
                        }

                        Global.Data.dataMatrix.Add(dataLine);
                    }

                    // Report to GD Console
                    GD.Print("File read and added to data.");
                }
                // On Error, Report to GD Console
                catch (IOException e)
                {
                    GD.Print("The file could not be read:");
                    GD.Print(e.Message);
                }
                break;
            
            // Push Data to File
            case 1:
                // Output Variable
                string outText = "";

                // Add directory names to header
                /*
                foreach (userDirectory i in Global.Data.database)
                    outText += i.name;
                */

                // Write header
                outText += "EI, SN, TF, JP";

                // Stringify each observation
                foreach (List<int> savedObs in Global.Data.dataMatrix) {
                    outText += "\n";
                    outText += savedObs[0] + ", " + savedObs[1] + ", " + savedObs[2] + ", " + savedObs[3];
                }

                // Write to file
                File.WriteAllText("Output/Data.csv", outText);

                // Report to GD Console
                GD.Print("File written.");
                break;

            // Exit
            case 2:
                // Return to the main menu
                GetTree().ChangeSceneToFile("Scenes/MainMenu.tscn");
                break;

        }

    }
}
