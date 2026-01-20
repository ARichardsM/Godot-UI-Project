using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

public partial class Global : Node
{
    // Create a global instance
    public static Global Data { get; private set; }

    public class userFile
    {
        public userFile(string n)
        {
            name = n;
            txt = false;
            png = false;
        }

        public string name;
        public bool txt;
        public bool png;
    }

    public class userDirectory
    {
        public userDirectory(string n) 
        { 
            name = n;
            opts = new List<userFile>();
        }

        public string name;
        public List<userFile> opts;
    }

    public class trait
    {
        public string key;
        public string val;

        public trait(string key, string val)
        {
            this.key = key;
            this.val = val;
        }
    }

    // Read and prepare the Directory Database
    private void readDirectory(string fileAddress)
    {
        // Pull input directories and nested files
        foreach (string dirpath in Directory.GetDirectories(fileAddress))
        {
            userDirectory newDir = new userDirectory(Path.GetFileName(dirpath));

            foreach (string filepath in Directory.GetFiles(dirpath))
            {
                // Invalid file type check
                if (Path.GetExtension(filepath) != ".txt" && Path.GetExtension(filepath) != ".png")
                    continue;

                // Previous file check
                bool isPrev = false;

                for (int i = 0; i < newDir.opts.Count; i++)
                    if (newDir.opts[i].name == Path.GetFileNameWithoutExtension(filepath))
                    {
                        isPrev = true;

                        // Add new file type
                        if (Path.GetExtension(filepath) == ".png")
                            newDir.opts[i].png = true;
                        else if (Path.GetExtension(filepath) == ".txt")
                            newDir.opts[i].txt = true;
                    }

                if (isPrev)
                    continue;

                // Create a new file
                userFile newFil = new userFile(Path.GetFileNameWithoutExtension(filepath));

                // Set new file type
                if (Path.GetExtension(filepath) == ".png")
                    newFil.png = true;
                else if (Path.GetExtension(filepath) == ".txt")
                    newFil.txt = true;

                // Add file to directory
                newDir.opts.Add(newFil);
            }

            // Add directory to database
            database.Add(newDir);
        }
    }

    // Read and prepare the Entity Database
    private void readInputFile(string fileAddress)
    {
        // Read the data file
        string fileText = File.ReadAllText(fileAddress);

        // Split the string
        string[] fileCon = fileText.Split("\n");
        string[] fileHead = fileCon[0].Split(",");

        // Add each observation to the global data
        for (int i = 1; i < fileCon.Length; i++)
        {
            List<trait> currData = new List<trait>();
            string[] line = fileCon[i].Split(",");

            // Verify the number of values match the number of keys
            if (line.Length != fileHead.Length)
                continue;

            // Convert obs to trait format and save
            for (int j = 0; j < line.Length; j++)
            {
                currData.Add(new trait(fileHead[j].Trim('\r'), line[j]));
            }

            // Save the data
            entityData.Add(currData);
        }
    }

    public override void _Ready()
    {
        Data = this;

        // Read the Input Directory
        try
        {
            // Read the data file
            readDirectory("Input");

            // Report to GD Console
            GD.Print("Directory read and added to data.");
        }
        // On Error, Report to GD Console
        catch (IOException e)
        {
            GD.Print("The directory could not be read:");
            GD.Print(e.Message);
        }

        // Read the data csv
        try
        {
            // Read the data file
            readInputFile("Input/Data.csv");

            // Report to GD Console
            GD.Print("File read and added to data.");
        }
        // On Error, Report to GD Console
        catch (IOException e)
        {
            GD.Print("The file could not be read:");
            GD.Print(e.Message);
        }
    }

    public List<userDirectory> database = new List<userDirectory>();
    public List<List<trait>> entityData = new List<List<trait>>();
    public List<trait> newEntity = new List<trait>();
}
