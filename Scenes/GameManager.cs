using Godot;
using System;

public class GameManager : Node2D
{
    
    public override void _Ready()
    {

        GetTree().ChangeScene("res://Scenes/TestMap.tscn");

    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  // public override void _Process(float delta)
  // {
  //     
  // }
}
