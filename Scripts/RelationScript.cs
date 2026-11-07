using Godot;
using System;
using System.Collections.Generic;
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
        public Label name;
        public GridContainer container;

        public void ContainerButtonPressed(int ID)
        {
            int left = (int)Math.Floor((double)ID / Global.Data.roster.Count);
            int right = (int)Math.Floor((double)ID % Global.Data.roster.Count);

            name.Text = Global.Data.roster[left].name + " - " + Global.Data.roster[right].name;
        }

        public void ReadyGrid()
        {
            // Add a column for every persona
            container.Columns = Global.Data.roster.Count;

            // Add all persona names in the roster
            List<string> rosterNames = new List<string>();
            foreach (var persona in Global.Data.roster)
            {
                rosterNames.Add(persona.name);
            }

            // Add a button for every persona
            int idCount = 0;
            foreach (var personaL in Global.Data.roster)
            {
                foreach (var personaR in Global.Data.roster)
                {
                    var temp = new Button();
                    int ID = idCount++;
                    temp.CustomMinimumSize = new Vector2(50, 50);
                    // Add signal to button 
                    temp.Pressed += () => ContainerButtonPressed(ID);
                    container.AddChild(temp);
                }
            }

            // Stables
            var arr = new int[Global.Data.roster.Count, Global.Data.roster.Count];
            foreach (var group in Global.Data.groups)
            {
                for (int left = 0; left < group.member.Count; left++)
                {
                    // Skip if member isn't in the roster
                    int leftLoc = rosterNames.IndexOf(group.member[left]);
                    if (leftLoc == -1)
                        continue;

                    for (int right = 0; right < left; right++)
                    {
                        // Skip if member isn't in the roster
                        int rightLoc = rosterNames.IndexOf(group.member[right]);
                        if (rightLoc == -1)
                            continue;

                        int locA = (rightLoc * Global.Data.roster.Count) + leftLoc;
                        int locB = (leftLoc * Global.Data.roster.Count) + rightLoc;
                        container.GetChild<Button>(locA).Modulate = new Color(3f, 0, 0, 1);
                        container.GetChild<Button>(locB).Modulate = new Color(3f, 0, 0, 1);
                    }
                }
            }
        }
    }

    class editPage : page
    {
    }

    // Declare Pages
    viewPage vp = new viewPage();
    editPage ep = new editPage();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        // Get the grid
        GridContainer container = GetNode<GridContainer>("ViewMenu/DataBox/GridContainer");
        vp.container = GetNode<GridContainer>("ViewMenu/DataBox/GridContainer");
        vp.name = GetNode<Label>("ViewMenu/Name");
        vp.ReadyGrid();
    }

}
