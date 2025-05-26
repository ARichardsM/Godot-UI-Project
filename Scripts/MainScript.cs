using Godot;
using System;
using System.Collections.Generic;

public partial class MainScript : Node2D
{
    // Function for button presses
    public void ButtonPressed(int val)
    {
        switch (val)
        {
            // Switch to character menu
            case 0:
                GetTree().ChangeSceneToFile("Scenes/CharacterMenu.tscn");
                break;

            // Switch to file menu
            case 1:
                GetTree().ChangeSceneToFile("Scenes/FileMenu.tscn");
                break;

            // Exit
            case 2:
                GetTree().Quit();
                break;

        }

        
    }
}
