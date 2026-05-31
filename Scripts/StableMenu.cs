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

        foreach (var member in Global.Data.roster)
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
        GD.Print("Wow");
        dataName.Text = Global.Data.roster[i].name;

        string outText = "";
        foreach(var traits in Global.Data.roster[i].traits)
        {
            outText += traits.key + " " + traits.val + "\n";
        }
        dataMembers.Text = outText;
    }

}
