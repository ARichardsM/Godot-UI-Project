using Godot;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using static Global;

public partial class Global : Node
{
    // Create a global instance
    public static Global Data { get; private set; }

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

    public class aspect
    {
        public string key;
        public string desc;

        public aspect(string key, string desc)
        {
            this.key = key;
            this.desc = desc;
        }
    }

    public class persona
    {
        public string name = "";
        public List<trait> traits = new List<trait>();

        public persona()
        {

        }

        public override string ToString()
        {
            string output = "";

            // Stringify name
            if (name != "")
                output += name + " - ";

            // Stringify traits
            for (int i = 0; i < traits.Count; i++)
                if (i == 0)
                    output += traits[i].key + ": " + traits[i].val;
                else
                    output += " " + traits[i].key + ": " + traits[i].val;

            // Output string
            return output;
        }

        public void Add(string key, string val)
        {
            traits.Add(new Global.trait(key, val));
        }
    }

    public class stable
    {
        public string name;
        public List<string> member = new List<string>();
        public List<aspect> aspects = new List<aspect>();

        public void Add(string key, string desc)
        {
            aspects.Add(new Global.aspect(key, desc));
        }

        public void Add(string key)
        {
            aspects.Add(new Global.aspect(key, ""));
        }
    }

    // Read and prepare the Directory Database
    private void readDirectory(string fileAddress)
    {
        // Pull input directories and nested files
        foreach (string dirpath in Directory.GetDirectories(fileAddress))
        {
            DirectoryManager.userDirectory newDir = new DirectoryManager.userDirectory(Path.GetFileName(dirpath));

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
                DirectoryManager.userFile newFil = new DirectoryManager.userFile(Path.GetFileNameWithoutExtension(filepath));

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
    private void LoadGroup(string fileAddress)
    {
        // Read the data file
        string fileText = File.ReadAllText(fileAddress);

        // Split the string
        string[] fileCon = fileText.Split("\n");
        string[] fileHead = fileCon[0].Split(",");


        // Add each observation to the global data
        for (int i = 1; i < fileCon.Length; i++)
        {
            //List<trait> currData = new List<trait>();
            stable currData = new stable();
            string[] line = fileCon[i].Split(",");

            // Convert obs to trait format and save
            for (int j = 0; j < line.Length; j++)
            {
                // Get current header
                string currHeader = fileHead[j].Trim('\r');

                // Add Name
                if (currHeader == "Name")
                {
                    if (line[j].Trim('\r').Trim('\n') != "")
                        currData.name = line[j].Trim('\r');
                }
                // Aspect
                else if (currHeader == "Aspect")
                {
                    if (line[j].Trim('\r') != "")
                        currData.Add(line[j].Trim('\r'));
                }
                // Add Members
                else
                    if (line[j].Trim('\r') != "")
                        currData.member.Add(line[j]);
            }

            // Verify Group
            bool verify = string.IsNullOrEmpty(currData.name);
            verify = verify && currData.member.Count == 0;
            verify = verify && currData.aspects.Count == 0;
            if (verify)
                continue;

            // Save the data
            groups.Add(currData);
        }
    }

    // Read and prepare the Entity Database
    private void LoadPersona(string fileAddress)
    {
        // Read the data file
        string fileText = File.ReadAllText(fileAddress);

        // Split the string
        string[] fileCon = fileText.Split("\n");
        string[] fileHead = fileCon[0].Split(",");

        // Add each observation to the global data
        for (int i = 1; i < fileCon.Length; i++)
        {
            persona currData = new persona();
            string[] line = fileCon[i].Split(",");

            // Verify the number of values match the number of keys
            if (line.Length != fileHead.Length)
                continue;

            // Convert obs to trait format and save
            for (int j = 0; j < line.Length; j++)
            {
                // Get current header
                string currHeader = fileHead[j].Trim('\r');

                // Add Name
                if (currHeader == "Name") {
                    if (line[j].Trim('\r') != "")
                        currData.name = line[j].Trim('\r');
                }
                // Add Trait
                else 
                    currData.traits.Add(new trait(currHeader, line[j]));
            }

            // Save the data
            roster.Add(currData);
        }
    }

    // Load within a try-catch
    public void SafeLoad(string arg, Action<string> loadFunction)
    {
        try
        {
            // Read the data file
            loadFunction(arg);

            // Report to GD Console
            GD.Print(arg + " read and added to data.");
        }
        // On Error, Report to GD Console
        catch (IOException e)
        {
            GD.Print(arg + " could not be read:");
            GD.Print(e.Message);
        }
    }

    public override void _Ready()
    {
        Data = this;

        // Read the Input Directory
        SafeLoad("Input", readDirectory);

        // Load Persona CSV
        SafeLoad("Input/RosterData.csv", LoadPersona);

        // Load Group CSV
        SafeLoad("Input/StableData.csv", LoadGroup);

    }

    public List<DirectoryManager.userDirectory> database = new List<DirectoryManager.userDirectory>();

    public List<persona> roster = new List<persona>();
    public List<stable> groups = new List<stable>();
}
