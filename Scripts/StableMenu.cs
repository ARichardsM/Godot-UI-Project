using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;

public partial class StableMenu : Control
{
    Label dataName, dataMembers;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Set container and labels
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
    }

    private void ButtonPress(int i)
    {
        dataName.Text = Global.Data.groups[i].name;

        string outText = "";

        outText += "Aspects\n";
        foreach (var name in Global.Data.groups[i].aspects)
        {
            outText += name.key + "\n";
        }

        outText += "\nMembers\n";
        foreach (var name in Global.Data.groups[i].member)
        {
            outText += name + "\n";
        }
        dataMembers.Text = outText;
    }

    private void MenuPress(int val)
    {
        switch (val)
        {
            case 0:
                break;

            case 1:
                // Return to the main menu
                GetTree().ChangeSceneToFile("Scenes/MainMenu.tscn");
                break;
        }
    }
}
