using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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

        List<string> rosterNames = new List<string>();

        bool[,] stableRelations;
        int[,] personaRelations;

        // On grid button press
        public void ContainerButtonPressed(int ID)
        {
            // Find which button was selected based it's set ID
            int left = (int)Math.Floor((double)ID / Global.Data.roster.Count);
            int right = (int)Math.Floor((double)ID % Global.Data.roster.Count);

            // Show the two characters (Row and Column)
            name.Text = Global.Data.roster[left].name + " - " + Global.Data.roster[right].name;
        }

        public void LoadStable()
        {
            stableRelations = new bool[Global.Data.roster.Count, Global.Data.roster.Count];

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

                        stableRelations[leftLoc, rightLoc] = true;
                        stableRelations[rightLoc, leftLoc] = true;
                    }
                }
            }
        }

        public void LoadPersona(string fileAddress)
        {
            personaRelations = new int[Global.Data.roster.Count, Global.Data.roster.Count];

            // Read the data file
            string fileText = File.ReadAllText(fileAddress);

            // Split the string
            string[] fileCon = fileText.Split("\n");
            string[] fileRows = fileCon[0].Trim('\r').Split(",").Skip(1).ToArray();
            List<string> fileCols = new List<string>();
            List<string> fileCont = new List<string>();

            for (int i = 1; i < fileCon.Length; i++)
            {
                string[] fileLine = fileCon[i].Split(",", 2);
                if (fileLine[0] != "")
                {
                    fileCols.Add(fileLine[0]);
                    fileCont.Add(fileLine[1].Trim('\r'));

                }
            }

            for (int i = 0; i < fileCols.Count(); i++)
            {
                string[] value = fileCont[i].Split(",");
                
                for (int j = 0; j < fileRows.Length; j++)
                {
                    if (value[j] == "")
                        continue;

                    // Skip if column member isn't in the roster
                    List<int> colLoc = new List<int>();
                    while (colLoc.Count < 2)
                    {
                        int col = rosterNames.IndexOf(fileCols[i], colLoc.LastOrDefault(-1) + 1);

                        if (col == -1)
                            break;

                        colLoc.Add(col);
                    }
                    if (colLoc.Count == 0)
                        continue;


                    // Skip if row member isn't in the roster
                    List<int> rowLoc = new List<int>();
                    while (rowLoc.Count < 2)
                    {
                        int row = rosterNames.IndexOf(fileRows[j], rowLoc.LastOrDefault(-1) + 1);

                        if (row == -1)
                            break;

                        rowLoc.Add(row);
                    }
                    if (rowLoc.Count == 0)
                        continue;

                    foreach (int row in rowLoc)
                    {
                        foreach (int col in colLoc)
                        {
                            personaRelations[row, col] = int.Parse(value[j]);
                            personaRelations[col, row] = int.Parse(value[j]);
                        }
                    }
                }
            }
        }

        public void PaintGrid()
        {
            // Same Stable (Negative, Neutral, Positive)
            Color[] sameStable = { new Color("Orange") * 3, new Color("Blue") * 3, new Color("Purple") * 3 };
            // Different Stable (Negative, Neutral, Positive)
            Color[] diffStable = { new Color("Red") * 3, new Color(0,0,0), new Color("Green") * 3 };

            for (int left = 0; left < Global.Data.roster.Count; left++)
            {
                for (int right = 0; right < left; right++)
                {
                    int level = Math.Sign(personaRelations[left, right]) + 1;

                    int locA = (right * Global.Data.roster.Count) + left;
                    int locB = (left * Global.Data.roster.Count) + right;

                    if (stableRelations[left, right])
                    {
                        container.GetChild<Button>(locA).Modulate = sameStable[level];
                        container.GetChild<Button>(locB).Modulate = sameStable[level];
                    }
                    else if (level != 1)
                    {
                        container.GetChild<Button>(locA).Modulate = diffStable[level];
                        container.GetChild<Button>(locB).Modulate = diffStable[level];
                    }
                }
            }
        }

        // Prepare and add buttons to the grid container
        public void ReadyGrid()
        {
            // Add a column for every persona
            container.Columns = Global.Data.roster.Count;

            // Add all persona names in the roster
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
                    // Create the 50x50 button with a unique ID
                    var temp = new Button();
                    int ID = idCount++;
                    temp.CustomMinimumSize = new Vector2(50, 50);

                    // Add signal to button 
                    temp.Pressed += () => ContainerButtonPressed(ID);
                    container.AddChild(temp);
                }
            }
        }
    }

    public void ViewPageButtons(int buttonID)
    {
        switch (buttonID)
        {
            case 0:
                GD.Print("TBD: Wipe");
                break;
            case 1:
                GD.Print("TBD: Edit");
                break;
            case 2: // Exit
                // Return to the main menu
                GetTree().ChangeSceneToFile("Scenes/MainMenu.tscn");
                break;
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
        // Initialize ViewPage Features
        vp.container = GetNode<GridContainer>("ViewMenu/DataBox/GridContainer");
        vp.name = GetNode<Label>("ViewMenu/Name");

        // Run Initial ViewPage Functions
        vp.ReadyGrid();
        vp.LoadStable();
        Global.Data.SafeLoad("Input/RelationData.csv", vp.LoadPersona);
        vp.PaintGrid();
    }

}
