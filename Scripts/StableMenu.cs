using Godot;
using System;
using System.Runtime.ConstrainedExecution;

public partial class StableMenu : Control
{
    Label dataName, dataMembers;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var container = GetNode<VBoxContainer>("ScrollContainer/VBoxContainer");
        dataName = GetNode<Label>("DataBox/Container/Name");
        dataMembers = GetNode<Label>("DataBox/Container/Members");

        int totalCount = 0;

        foreach (var member in Global.Data.groups)
        {
            var temp = new Button();
            int currInd = totalCount++;
            temp.Text = "Person: " + member.name;
            temp.Pressed += () => ButtonPress(currInd);
            container.AddChild(temp);
        }

        GD.Print("File written.");
    }

    private void ButtonPress(int i)
    {
        dataName.Text = Global.Data.groups[i].name;

        string outText = "";
        foreach(var name in Global.Data.groups[i].member)
        {
            outText += name + "\n";
        }
        dataMembers.Text = outText;
    }

}
