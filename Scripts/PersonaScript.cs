using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.IO;
using System.Linq;

public partial class PersonaScript : Node
{
    

    // Declare Select Containers
    private HBoxContainer fullCont;
    private HBoxContainer pngCont;
    private HBoxContainer txtCont;

    // Declare Subtitle
    private Label subtitle;

    // Declare Select Display
    private TextureRect sprite;
    private Label text;


    // Function for button presses
    public void BehaviourSelect(int val)
    {
        switch (val)
        {
            case 0:
                // Add Personality Traits
                List<string> personalityMatrix = new() { "EI", "SN", "TF", "JP" };
                for (int i = 0; i < personalityMatrix.Count; i++)
                {
                    /*
                    // Cancel if a button hasn't been selected
                    if (scales[i].ButtonSelected == 0)
                    {
                        GD.Print("Verify Failed: Button Unselected");
                        return;
                    }

                    // Log trait
                    newPersona.Add(personalityMatrix[i], scales[i].ButtonSelected.ToString());
                    */
                }

                // Add to Entity Data List
                Global.Data.roster.Add(newPersona);

                // Return to the main menu
                GetTree().ChangeSceneToFile("Scenes/MainMenu.tscn");
                break;

            case 1:
                // Return to the main menu
                GetTree().ChangeSceneToFile("Scenes/MainMenu.tscn");
                break;

        }
    }

    // Function for button presses
    public void PersonaShift(int val)
    {
        switch (val)
        {
            // Add Data
            case 0:
                // Add to the new data point
                string newKey = Global.Data.database[newPersona.traits.Count].name;
                string newVal = Global.Data.database[newPersona.traits.Count].opts[selectNum].name;
                newPersona.traits.Add(new Global.trait(newKey, newVal));

                // If the new category exists, update the screen to load the new directory
                if (newPersona.traits.Count < Global.Data.database.Count)
                {
                    updateFullScreen();
                }
                // Else, go to the character menu
                else
                {
                    // Alter visiblity
                    GetNode<Control>("SelectMenu").Visible = false;
                    GetNode<Control>("PersonalityMenu").Visible = true;

                    // Edit Sub-Title
                    subtitle = GetNode<Label>("Sub-Title");
                    subtitle.Text = "Personality Menu";
                    break;
                }

                break;
            // Exit
            case 1:
                // Return to the main menu
                GetTree().ChangeSceneToFile("Scenes/MainMenu.tscn");
                break;

        }
    }

    // Function for button presses
    public void SelectShift(bool shiftUp)
    {
        // Determine current values
        int currTrait = newPersona.traits.Count;

        // Shift
        if (shiftUp)
        {
            --selectNum;
        }
        else
        {
            ++selectNum;
        }

        // Lower Bound Shift
        if (selectNum < 0)
            selectNum = Global.Data.database[currTrait].opts.Count - 1;

        // Upper Bound Shift
        if (selectNum >= Global.Data.database[currTrait].opts.Count)
            selectNum = 0;

        // Update the Sub Screen
        updateSubScreen();
    }

    // Update the entire screen
    private void updateFullScreen()
    {
        // Determine current values
        int currTrait = newPersona.traits.Count;

        // Bound by Directory
        if (currTrait >= Global.Data.database.Count)
            return;

        // Edit Sub-Title
        subtitle = GetNode<Label>("Sub-Title");
        subtitle.Text = Global.Data.database[currTrait].name + " Category Menu";

        // Reset the select
        selectNum = 0;

        // Update the Sub Screen
        updateSubScreen();
    }

    // Update the file section of the screen
    private void updateSubScreen()
    {
        // Determine current values
        int currTrait = newPersona.traits.Count;

        // Bound by File
        if (selectNum >= Global.Data.database[currTrait].opts.Count)
            return;

        // Determine directory and file
        string currDir = Global.Data.database[currTrait].name;
        string currFile = Global.Data.database[currTrait].opts[selectNum].name;

        // Reset Containers
        fullCont.Visible = false;
        pngCont.Visible = false;
        txtCont.Visible = false;

        // Determine Screen Style
        int screenStyle = 0;

        screenStyle += Global.Data.database[currTrait].opts[selectNum].png ? 2 : 0;
        screenStyle += Global.Data.database[currTrait].opts[selectNum].txt ? 1 : 0;

        switch (screenStyle)
        {
            // PNG + TXT
            case 3:
                fullCont.Visible = true;
                sprite = GetNode<TextureRect>("SelectMenu/FullContainer/TextureRect");
                sprite.SetTexture((Texture2D)GD.Load("res://Input/" + currDir + "/" + currFile + ".png"));
                text = GetNode<Label>("SelectMenu/FullContainer/Label");
                text.SetText(currFile + "\n\n" + File.ReadAllText("Input/" + currDir + "/" + currFile + ".txt"));
                break;
            // PNG
            case 2:
                pngCont.Visible = true;
                sprite = GetNode<TextureRect>("SelectMenu/PngContainer/TextureRect");
                sprite.SetTexture((Texture2D)GD.Load("res://Input/" + currDir + "/" + currFile + ".png"));
                break;
            // TXT
            case 1:
                txtCont.Visible = true;
                text = GetNode<Label>("SelectMenu/TxtContainer/Label");
                text.SetText(currFile + "\n\n" + File.ReadAllText("Input/" + currDir + "/" + currFile + ".txt"));
                break;
        }
    }

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

        public int selectedButtonId;
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
                GD.Print("Add");
                break;
            case 1: // TBD: Edit
                GD.Print("Edit");
                break;
            case 2: // Exit
                // Return to the main menu
                GetTree().ChangeSceneToFile("Scenes/MainMenu.tscn");
                break;
        }
    }

    // Node
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
        }

        private void updateScreen()
        {
            var currDir = Global.Data.database[directoryInd].name;
            var currFile = Global.Data.database[directoryInd].opts[fileInd].name;
            GD.Print(currFile);
            fullCont.Visible = true;

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
            case 0: // TBD: Previous
                GD.Print("Previous");
                sp.ChangeFile(false);
                break;
            case 1: // TBD: Next
                GD.Print("Next");
                sp.ChangeFile(true);
                break;
            case 2: // TBD: Add To Data
                GD.Print("Add To Data");
                // Log trait
                newPersona.Add(Global.Data.database[sp.directoryInd].opts[sp.fileInd].name, Global.Data.database[sp.directoryInd].name);

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

    // Declare Pages
    viewPage vp = new viewPage();
    personalityPage pp = new personalityPage();
    selectPage sp = new selectPage();

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


        // Initialize Persona
        newPersona = new Global.persona();

        // Declare Containers
        fullCont = GetNode<HBoxContainer>("SelectMenu/FullContainer");
        pngCont = GetNode<HBoxContainer>("SelectMenu/PngContainer");
        txtCont = GetNode<HBoxContainer>("SelectMenu/TxtContainer");

        // Set initial screen
        updateFullScreen();

        // Create a button for each persona
        foreach (var member in Global.Data.roster)
        {
            vp.AddContainerButton(member);
        }

        // Set Visibile Menu to View
        vp.Visible(true);
        pp.Visible(false);
        sp.Visible(false);
    }
}
