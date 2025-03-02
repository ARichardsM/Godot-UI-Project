using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class FileMenu : Node2D
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

                    List<List<int>> fileData = new List<List<int>>();
                    for (int i = 1; i < fileCon.Length; i++)
                    {
                        List<int> dataLine = new List<int>();
                        string[] line = fileCon[i].Split(",");

                        foreach(string num in line)
                        {
                            dataLine.Add(int.Parse(num));
                        }

                        fileData.Add(dataLine);
                    }

                    // Write the text to the console
                    GD.Print(fileText);
                    GD.Print(fileCon[0]);
                }
                catch (IOException e)
                {
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(e.Message);
                }

                GD.Print("Left Button");
                break;

            case 1:
                GD.Print("Right Button");
                break;

            case 2:
                // Return to the main menu
                GetTree().ChangeSceneToFile("MainMenu.tscn");
                break;

        }

    }
}
