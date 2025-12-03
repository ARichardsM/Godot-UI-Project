using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

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

    public override void _Ready()
    {
        Data = this;

        // Pull input directories and nested files
        foreach (string dirpath in Directory.GetDirectories("Input"))
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

        // Read data csv
    }

    public List<List<int>> dataMatrix = new List<List<int>>();

    public List<userDirectory> database = new List<userDirectory>();

    public List<List<string>> dataEntries = new List<List<string>>();
    public List<string> newData = new List<string>();
    public int databaseNum = 0;
}
