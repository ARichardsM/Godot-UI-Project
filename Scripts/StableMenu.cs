using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.IO;
using static Global;

public partial class StableMenu : Control
{
    int selected = -1;
    Label dataName, dataMembers;
    Control ViewMenu, AspectMenu, MemberMenu;
    List<aspect> aspectInfo = new List<aspect>();

    // Internal aspect class for the stable aspect data file
    public class aspect
    {
        public string type;
        public string key;
        public string desc;

        public aspect(string type, string key, string desc)
        {
            this.type = type;
            this.key = key;
            this.desc = desc;
        }
        public aspect(string key, string desc)
        {
            this.key = key;
            this.desc = desc;
        }

        public aspect()
        {
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Set control, container, and labels
        ViewMenu = GetNode<Control>("ViewMenu");
        AspectMenu = GetNode<Control>("AspectMenu");
        MemberMenu = GetNode<Control>("MemberMenu");
        var container = GetNode<VBoxContainer>("ViewMenu/ScrollContainer/VBoxContainer");
        dataName = GetNode<Label>("ViewMenu/DataBox/Container/Name");
        dataMembers = GetNode<Label>("ViewMenu/DataBox/Container/Members");

        // Begin group count (for ButtonPress int)
        int totalCount = 0;

        // Create a button for each group
        foreach (var member in Global.Data.groups)
        {
            var temp = new Button();
            int ID = totalCount++;

            // If group is unnamed, set the button text to "unnamed"
            if (string.IsNullOrEmpty(member.name))
                temp.Text = "Unnamed";
            else
                temp.Text = member.name;

            // Add signal to button 
            temp.Pressed += () => ButtonPress(ID);

            // Add button to the container
            container.AddChild(temp);
        }

        Global.Data.SafeLoad("Input/StableAspectData.csv", LoadAspects);
    }

    // Read and prepare the Aspect Database
    private void LoadAspects(string fileAddress)
    {
        // Read the data file
        string fileText = File.ReadAllText(fileAddress);

        // Split the string
        string[] fileCon = fileText.Split("\n");
        string[] fileHead = fileCon[0].Split(",");

        // Add each observation to the global data
        for (int i = 1; i < fileCon.Length; i++)
        {
            aspect currData = new aspect();
            string[] line = fileCon[i].Split(",");

            // Verify the number of values match the number of keys
            if (line.Length != fileHead.Length)
                continue;

            // Convert obs to trait format and save
            for (int j = 0; j < line.Length; j++)
            {
                // Get current header
                string currHeader = fileHead[j].Trim('\r');

                // Add data
                switch (currHeader)
                {
                    case "Name":
                        currData.key = line[j].Trim('\r');
                        break;
                    case "Description":
                        currData.desc = line[j].Trim('\r');
                        break;
                    case "Type":
                        if (line[j].Trim('\r') != "")
                            currData.type = line[j].Trim('\r');
                        break;
                }
            }

            // Save the data
            aspectInfo.Add(currData);
        }
    }

    private void SwitchToAspects()
    {
        ViewMenu.Visible = false;
        AspectMenu.Visible = true;
    }

    private void SwitchToMembers()
    {
        ViewMenu.Visible = false;
        MemberMenu.Visible = true;
    }

    private void ButtonPress(int i)
    {
        // Set header name
        dataName.Text = Global.Data.groups[i].name;

        // Set selected for edit functions
        selected = i;

        string outText = "";

        // Record aspects
        outText += "Aspects\n";
        foreach (var name in Global.Data.groups[i].aspects)
        {
            outText += name.key + "\n";
        }

        // Record members
        outText += "\nMembers\n";
        foreach (var name in Global.Data.groups[i].member)
        {
            outText += name + "\n";
        }

        // Write to window
        dataMembers.Text = outText;
    }

    private void MenuPress(int val)
    {
        switch (val)
        {
            case 0:
                // Prepare and go to the edit member menu
                if (selected == -1)
                    GD.Print("No stable selected.");
                else
                    SwitchToMembers();
                break;

            case 1:
                // Prepare and go to the edit aspect menu
                if (selected == -1)
                    GD.Print("No stable selected.");
                else
                    SwitchToAspects();
                break;

            case 2:
                // Return to the main menu
                ReturnPress(0);
                break;
        }
    }

    private void ReturnPress(int val)
    {
        switch (val)
        {
            case 0:
                // Return to the main menu
                GetTree().ChangeSceneToFile("Scenes/MainMenu.tscn");
                break;
            case 1:
                ViewMenu.Visible = true;
                AspectMenu.Visible = false;
                break;
            case 2:
                ViewMenu.Visible = true;
                MemberMenu.Visible = false;
                break;
        }
    }

    private void AspectButtons(int val)
    {
        switch (val)
        {
            case 0:
                // Return to the main menu
                GD.Print("Enter");
                break;
            case 1:
                GD.Print("Add");
                break;
            case 2:
                GD.Print("Remove");
                break;
        }
    }
}
