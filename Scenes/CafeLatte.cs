using Godot;
using System;

public class CafeLatte : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    
    public AnimationPlayer AnimationPlayer { get; set; }
    
    public Sprite _sprite { get; set; }

    public override void _Ready()
    {
        
        _sprite = (Sprite)GetNode("Sprite");

        AnimationPlayer = (AnimationPlayer)GetNode("AnimationPlayer");
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
