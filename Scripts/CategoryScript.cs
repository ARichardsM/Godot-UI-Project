using Godot;
using System;

public partial class CategoryScript : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var z = GetNode<Sprite2D>("Area2D/Sprite2D");
		//z.Texture.Set()
		var w = GD.Load("res://Input/Element/Earth.png");
		z.SetTexture((Texture2D) w);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
