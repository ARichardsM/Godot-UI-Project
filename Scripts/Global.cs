using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class Global : Node
{
    // Create a global instance
    public static Global Data { get; private set; }

    public struct file
    {
        public file(string n)
        {
            name = n;
            txt = false;
            png = false;
        }

        public string name;
        public bool txt;
        public bool png;
    }

    public struct directory
    {
        public directory(string n) 
        { 
            name = n;
            opts = new List<file>();
        }

        public string name;
        public List<file> opts;
    }

    public override void _Ready()
    {
        Data = this;

        // Pull input directories and nested files
        foreach (string f in Directory.GetDirectories("Input"))
        {
            directory newDir = new directory(Path.GetFileName(f));

            foreach (string z in Directory.GetFiles(f))
            {
                newDir.opts.Add(new file(Path.GetFileName(z)));
            }

            CharacterDir.Add(newDir); 
        }

        // Print Data to Console
        foreach (directory f in CharacterDir)
        {
            GD.Print("Directory " + f.name);
            foreach (file z in f.opts)
            {
                GD.Print("- File " + z.name);
            }
        } 
    }

    public List<List<int>> dataMatrix = new List<List<int>>();
    public List<directory> CharacterDir = new List<directory>();
}
