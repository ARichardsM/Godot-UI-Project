using Godot;
using System;
using System.Globalization;
using System.Numerics;

public partial class ScaleSelection : Node2D
{
    // Prepare Variables
    [Export]
    public string LeftLabelText = "";
    [Export]
    public string RightLabelText = "";

    public int ButtonSelected = 0;
	private Theme selected = GD.Load<Theme>("res://Asset/SelectedButton.tres");
    private Theme unselected = GD.Load<Theme>("res://Asset/UnselectedButton.tres");

    public override void _Ready()
    {
        // Initialize Labels
        GetNode<Label>("LabelLeft").Text = LeftLabelText;
        GetNode<Label>("LabelRight").Text = RightLabelText;
        GetNode<Label>("LabelBottom").Text = "";
    }

    /// <summary>
    /// Translate the returned Signal variable into the name of the pressed button.
    /// </summary>
    /// <param name="val"> Int - Signal variable. Determines the button string. </param>
    /// <returns> String - Name of the button node. </returns>
    private string TranslateButton(int val)
	{
        string[] LeftButtons = { "Button-1L", "Button-2L", "Button-3L", "Button-4L" };
        string[] RightButtons = { "Button-1R", "Button-2R", "Button-3R", "Button-4R" };

		if (val < 0)
		{
            return LeftButtons[Math.Abs(val) - 1];
        }
        else
        {
            return RightButtons[Math.Abs(val) - 1];
        }

        return "";

	}

    public void ButtonPressed(int val)
	{
        string[] valString = { "Slightly ", "Somewhat ", "", "Very " };
        string Button;

        if (ButtonSelected != 0)
        {
            Button = TranslateButton(ButtonSelected);

            GetNode<Button>(Button).Theme = unselected;
        }
        
        Button = TranslateButton(val);

        GetNode<Button>(Button).Theme = selected;

        if (val < 0)
        {
            GetNode<Label>("LabelBottom").Text = valString[Math.Abs(val) - 1] + LeftLabelText;
        }
        else
        {
            GetNode<Label>("LabelBottom").Text = valString[Math.Abs(val) - 1] + RightLabelText;
        }

        ButtonSelected = val;
    }

}
