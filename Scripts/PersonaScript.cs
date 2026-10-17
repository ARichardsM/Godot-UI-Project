using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.IO;
using System.Linq;
using static Global;

public partial class PersonaScript : Node
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
    
    // ViewMenu Node
    class viewPage : page
    {
        public Label name;
        public Label aspects;

        public VBoxContainer container;

        public int selectedButtonId = -1;
        int idCount = 0;

        // Update the labels when a container button is pressed
        public void ContainerButtonPressed(int buttonID)
        {
            // Log the selected button
            selectedButtonId = buttonID;

            // Set Name Label
            name.Text = Global.Data.roster[buttonID].name;

            // Set Aspect Label
            string aspectText = "";
            foreach (var asp in Global.Data.roster[buttonID].traits)
            {
                aspectText = aspectText + asp.key + " (" + asp.val + ")\n";
            }
            aspects.Text = aspectText;
        }

        // Add a container button
        public void AddContainerButton(Global.persona member)
        {
            var temp = new Button();
            int ID = idCount++;

            // If group is unnamed, set the button text to "unnamed"
            if (string.IsNullOrEmpty(member.name))
                temp.Text = "Unnamed";
            else
                temp.Text = member.name;

            // Add signal to button 
            temp.Pressed += () => ContainerButtonPressed(ID);

            // Add button to the container
            container.AddChild(temp);
        }
    }

    // View Menu Node Buttons
    public void ViewPageButtonPressed(int buttonID)
    {
        switch (buttonID)
        {
            case 0: // Add
                vp.Visible(false);
                sp.Visible(true);
                break;
            case 1: // TBD: Edit
                if (vp.selectedButtonId != -1) {
                    vp.Visible(false);
                    ep.loadAspects(vp.selectedButtonId);
                    ep.Visible(true);
                }

                GD.Print("Edit");
                break;
            case 2: // Exit
                // Return to the main menu
                GetTree().ChangeSceneToFile("Scenes/MainMenu.tscn");
                break;
        }
    }

    // PersonalityMenu Node
    class personalityPage : page
    {
        public LineEdit name;
        // Declare Selection Scales
        public List<ScaleSelection> scales = new List<ScaleSelection>();

        public override void Visible(bool isVisible) 
        {
            base.Visible(isVisible);

            if (!isVisible)
            {
                foreach (ScaleSelection sca in scales)
                    sca.ButtonPressed(0);

                name.Text = "";
            }
        }
    }

    // Personality Menu Node Buttons
    public void PersonalityPageButtonPressed(int buttonID)
    {
        switch (buttonID)
        {
            case 0: // Add
                vp.Visible(false);
                pp.Visible(true);

                newPersona.name = pp.name.Text;
                // Add Personality Traits
                List<string> personalityMatrix = new() { "EI", "SN", "TF", "JP" };
                for (int i = 0; i < personalityMatrix.Count; i++)
                {
                    // Cancel if a button hasn't been selected
                    if (pp.scales[i].ButtonSelected == 0)
                    {
                        GD.Print("Verify Failed: Button Unselected");
                        return;
                    }

                    // Log trait
                    newPersona.Add(personalityMatrix[i], pp.scales[i].ButtonSelected.ToString());
                }

                // Add to Entity Data List
                Global.Data.roster.Add(newPersona);
                vp.AddContainerButton(newPersona);

                // Return to View Menu
                vp.Visible(true);
                pp.Visible(false);
                break;
            case 1: // Exit
                // Wipe Persona
                newPersona.traits.Clear();

                // Return to View Menu
                vp.Visible(true);
                pp.Visible(false);
                break;
        }
    }

    // SelectMenu Node
    class selectPage : page
    {
        public string name;

        // Declare Select Containers
        public HBoxContainer fullCont;
        public HBoxContainer pngCont;
        public HBoxContainer txtCont;

        // File Indices
        public int fileInd;
        public int directoryInd;

        // Declare Select Display
        public TextureRect fullSprite;
        public Label fullText;
        public TextureRect onlySprite;
        public Label onlyText;


        public override void Visible(bool isVisible)
        {
            
            base.Visible(isVisible);

            if (!isVisible)
            {
                fileInd = 0;
                directoryInd = 0;
            }
            else
            {
                this.updateScreen();
            }
        }

        private void updateScreen()
        {
            var currDir = Global.Data.database[directoryInd].name;
            var currFile = Global.Data.database[directoryInd].opts[fileInd].name;

            // Reset Containers
            fullCont.Visible = false;
            pngCont.Visible = false;
            txtCont.Visible = false;

            // Determine Screen Style
            int screenStyle = 0;

            screenStyle += Global.Data.database[directoryInd].opts[fileInd].png ? 2 : 0;
            screenStyle += Global.Data.database[directoryInd].opts[fileInd].txt ? 1 : 0;


            switch (screenStyle)
            {
                // PNG + TXT
                case 3:
                    fullCont.Visible = true;
                    fullSprite.SetTexture((Texture2D)GD.Load("res://Input/" + currDir + "/" + currFile + ".png"));
                    fullText.SetText(currFile + "\n\n" + File.ReadAllText("Input/" + currDir + "/" + currFile + ".txt"));
                    break;
                // PNG
                case 2:
                    pngCont.Visible = true;
                    onlySprite.SetTexture((Texture2D)GD.Load("res://Input/" + currDir + "/" + currFile + ".png"));
                    break;
                // TXT
                case 1:
                    txtCont.Visible = true;
                    onlyText.SetText(currFile + "\n\n" + File.ReadAllText("Input/" + currDir + "/" + currFile + ".txt"));
                    break;
            }
        }

        public void ChangeFile(bool isForward)
        {
            if (isForward && ((fileInd+1) < Global.Data.database[directoryInd].opts.Count)) { 
                fileInd++;
                updateScreen();
            }
            else if (!isForward && (fileInd > 0)) { 
                fileInd--;
                updateScreen();
            }
        }
        public void ChangeDirectory()
        {
            directoryInd++;
            fileInd = 0;

            updateScreen();
        }
    }

    // Select Menu Node Buttons
    public void SelectPageButtonPressed(int buttonID)
    {
        switch (buttonID)
        {
            case 0: // Previous
                sp.ChangeFile(false);
                break;
            case 1: // Next
                sp.ChangeFile(true);
                break;
            case 2: // Add To Data
                // Log trait
                newPersona.Add(Global.Data.database[sp.directoryInd].name, Global.Data.database[sp.directoryInd].opts[sp.fileInd].name);

                if ((sp.directoryInd + 1) < Global.Data.database.Count)
                {
                    sp.ChangeDirectory();
                }
                else
                {
                    // Move to Personality Menu
                    pp.Visible(true);
                    sp.Visible(false);
                }
                break;
            case 3: // Exit
                // Wipe Persona
                newPersona.traits.Clear();

                // Return to View Menu
                vp.Visible(true);
                sp.Visible(false);
                break;
        }
    }

    // EditMenu Node
    class editPage : page
    {
        public struct Aspect
        {
            public string type;
            public string name;
            public string desc;
        }

        public int selectedPersona;
        public List<Aspect> aspectList = new List<Aspect>();

        // Declare Edit Show Sub-Display
        public Label aspects;

        // Declare Edit Add Sub-Display
        public OptionButton addOptionButton;
        public Label addDescription;

        // Declare Edit Remove Sub-Display
        public OptionButton removeOptionButton;
        public Label removeDescription;

        public void loadAspects(int persona)
        {
            removeOptionButton.Clear();
            selectedPersona = persona;
            // Set Aspect Label
            string aspectText = "";
            foreach (var asp in Global.Data.roster[persona].traits)
            {
                aspectText = aspectText + asp.key + " (" + asp.val + ")\n";
                removeOptionButton.AddItem(asp.key);
            }
            aspects.Text = aspectText;
        }

        public void removeAspect()
        {
            int selected = removeOptionButton.Selected;

            // If no trait is selected, return
            if (selected == -1)
            {
                return;
            }

            // Remove the trait from the persona in the roster
            Global.Data.roster[selectedPersona].traits.RemoveAt(selected);

            // Reload aspects
            loadAspects(selectedPersona);
        }

        public void addAspect()
        {
            int selected = addOptionButton.Selected;

            // If no trait is selected, return
            if (selected == -1)
            {
                return;
            }

            // Create the new trait
            Global.trait newTrait = new Global.trait(aspectList[addOptionButton.Selected].name, aspectList[addOptionButton.Selected].type);

            // Add the trait to the persona in the roster
            Global.Data.roster[selectedPersona].traits.Add(newTrait);

            // Reload aspects
            loadAspects(selectedPersona);
        }

        public void loadAspectDoc(string fileName)
        {
            // Read the data file
            string fileText = File.ReadAllText(fileName);
            

            // Split the string
            string[] fileCon = fileText.Split("\n");
            string[] fileHead = fileCon[0].Split(",");

            // Add each aspect to the aspect list
            for (int i = 1; i < fileCon.Length-1; i++)
            {
                // Split the line
                string[] fileLine = fileCon[i].Split(",", 3);
                Aspect currAspect = new Aspect();

                // Assign the aspects according to header
                for (int j = 0; j < fileHead.Length; j++) {
                    char[] traillingChars = { ' ', '"', '\r' };

                    string currentLine = fileLine[j].Trim(traillingChars);

                    switch (fileHead[j].Trim(traillingChars))
                    {
                        case "Type":
                            currAspect.type = currentLine;
                            break;
                        case "Name":
                            currAspect.name = currentLine;
                            break;
                        case "Description":
                            currAspect.desc = currentLine;
                            break;
                    }
                }

                aspectList.Add(currAspect);
            }

            foreach (Aspect asp in aspectList)
            {
                addOptionButton.AddItem(asp.name);
            }
        }

        public void loadRemoveDescription()
        {
            int selected = removeOptionButton.Selected;

            List<string> aspectNames = new List<string>();

            foreach (var asp in aspectList)
            {
                aspectNames.Add(asp.name);
            }

            int aspectIndex = aspectNames.IndexOf(Global.Data.roster[selectedPersona].traits[selected].key);

            // If no trait is selected, return
            if (selected == -1)
            {
                return;
            }
            // Else, If
            else if (aspectIndex != -1)
            {
                removeDescription.Text = aspectList[aspectIndex].desc;
            }
            else
            {
                removeDescription.Text = "Ipsum";
            }

            GD.Print(addOptionButton.Selected);
        }
        public void loadAddDescription()
        {
            addDescription.Text = aspectList[addOptionButton.Selected].desc;
        }
    }

    // Edit Menu Node Buttons
    public void EditPageButtonPressed(int buttonID)
    {
        switch (buttonID)
        {
            case 0: // Wipe
                GD.Print("Wipe");
                break;
            case 1: // Exit
                vp.Visible(true);
                ep.Visible(false);
                break;
            case 2: // Add Random
                var addRand = new Random();
                ep.addOptionButton.Selected = addRand.Next(ep.aspectList.Count);
                ep.loadAddDescription();
                break;
            case 3: // Add
                ep.addAspect();
                break;
            case 4: // Remove Random
                var removeRand = new Random();
                ep.removeOptionButton.Selected = removeRand.Next(Global.Data.roster[ep.selectedPersona].traits.Count);
                ep.loadRemoveDescription();
                break;
            case 5: // Remove
                ep.removeAspect();
                break;
        }
    }

    // Edit Menu Node Buttons
    public void EditPageOptionChanged(int _)
    {
        ep.loadAddDescription();
    }

    // Edit Menu Node Buttons
    public void EditPageOptionChanged(int _, bool isAdd)
    {
        ep.loadAddDescription();
    }


    // Declare Pages
    viewPage vp = new viewPage();
    personalityPage pp = new personalityPage();
    selectPage sp = new selectPage();
    editPage ep = new editPage();

    // Declare variables
    private int selectNum = 0;
    public Global.persona newPersona;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        // Pull View Menu
        vp.self = GetNode<Control>("ViewMenu");
        vp.name = GetNode<Label>("ViewMenu/DataBox/Container/Name");
        vp.aspects = GetNode<Label>("ViewMenu/DataBox/Container/Members");
        vp.container = GetNode<VBoxContainer>("ViewMenu/ScrollContainer/VBoxContainer");
        vp.selectedButtonId = -1;

        // Pull Personality Menu
        pp.self = GetNode<Control>("PersonalityMenu");
        pp.name = GetNode<LineEdit>("PersonalityMenu/ButtonContainer/Name");
        pp.scales.Add(this.GetNode<ScaleSelection>("PersonalityMenu/Selection1"));
        pp.scales.Add(this.GetNode<ScaleSelection>("PersonalityMenu/Selection2"));
        pp.scales.Add(this.GetNode<ScaleSelection>("PersonalityMenu/Selection3"));
        pp.scales.Add(this.GetNode<ScaleSelection>("PersonalityMenu/Selection4"));

        // Pull Select Menu
        sp.self = GetNode<Control>("SelectMenu");
        sp.fullCont = GetNode<HBoxContainer>("SelectMenu/FullContainer");
        sp.pngCont = GetNode<HBoxContainer>("SelectMenu/PngContainer");
        sp.txtCont = GetNode<HBoxContainer>("SelectMenu/TxtContainer");
        sp.fullSprite = GetNode<TextureRect>("SelectMenu/FullContainer/TextureRect");
        sp.fullText = GetNode<Label>("SelectMenu/FullContainer/Label");
        sp.onlySprite = GetNode<TextureRect>("SelectMenu/PngContainer/TextureRect");
        sp.onlyText = GetNode<Label>("SelectMenu/TxtContainer/Label");

        // Pull Edit Menu
        ep.self = GetNode<Control>("EditMenu");
        ep.aspects = GetNode<Label>("EditMenu/Options/Aspects/Label");
        ep.removeOptionButton = GetNode<OptionButton>("EditMenu/Options/RemoveMenu/Options");
        ep.addOptionButton = GetNode<OptionButton>("EditMenu/Options/AddMenu/Options");
        ep.addDescription = GetNode<Label>("EditMenu/Options/AddMenu/Description/Label");
        ep.removeDescription = GetNode<Label>("EditMenu/Options/RemoveMenu/Description/Label");
        Global.Data.SafeLoad("Input/PersonaAspectData.csv", ep.loadAspectDoc);
        
        // Initialize Persona
        newPersona = new Global.persona();

        // Create a button for each persona
        foreach (var member in Global.Data.roster)
        {
            vp.AddContainerButton(member);
        }

        // Set Visibile Menu to View
        vp.Visible(true);
        pp.Visible(false);
        sp.Visible(false);
        ep.Visible(false);
    }
}
