using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class DirectoryManager
{
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

}
