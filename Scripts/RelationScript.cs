using Godot;
using System;
using System.ComponentModel;
using System.Xml.Linq;

public partial class RelationScript : Control
{
    // Control Child Nodes
    class page
    {
        public Control self;
        public virtual void Visible(bool isVisible)
        {
            self.Visible = isVisible;
        }
    }

    class viewPage : page
    {
    }

    class editPage : page
    {
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        // Get the grid
        GridContainer container = GetNode<GridContainer>("ViewMenu/DataBox/GridContainer");

        // Add a column for every persona
        container.Columns = Global.Data.roster.Count;

        // Add a button for every persona
        foreach (var persona in Global.Data.roster)
        {
            var temp = new Button();
            temp.CustomMinimumSize = new Vector2(50, 50);
            container.AddChild(temp);
        }

        // Add a red button to the grid
        var temp2 = new Button();
        temp2.CustomMinimumSize = new Vector2(50, 50);
        temp2.Modulate = new Color(3f, 0, 0, 1);
        container.AddChild(temp2);

        // Change the second button to red
        container.GetChild<Button>(1).Modulate = new Color(3f, 0, 0, 1);
    }

}
