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
        var container = GetNode<VBoxContainer>("ViewMenu/ScrollContainer/VBoxContainer");
        dataName = GetNode<Label>("ViewMenu/DataBox/Container/Name");
        dataMembers = GetNode<Label>("ViewMenu/DataBox/Container/Members");

        int totalCount = 0;

        foreach (var member in Global.Data.groups)
        {
            var temp = new Button();
            int currInd = totalCount++;
            temp.Text = "Person: " + member.name;
            temp.Pressed += () => ButtonPress(currInd);
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
