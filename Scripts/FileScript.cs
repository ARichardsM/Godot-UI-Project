using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;

public partial class FileScript : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ButtonPressed(int val)
	{
        switch (val)
        {
            case 0:
                try
                {
                    // Open the text file using a stream reader
                    using StreamReader reader = new("Data/Data.csv");

                    // Read the stream as a string
                    string fileText = reader.ReadToEnd();

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

            case 1:
                foreach (List<int> s in Global.Data.dataMatrix)
                    GD.Print("Right Button");
                break;

            case 2:
                // Return to the main menu
                GetTree().ChangeSceneToFile("Scenes/MainMenu.tscn");
                break;

        }

    }
}
