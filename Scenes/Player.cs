using Godot;
using System;

public class Player : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    
    private AnimationPlayer AnimationPlayer { get; set; }
    
    
    public override void _Ready()
    {
        AnimationPlayer = (AnimationPlayer)GetNode("AnimationPlayer");
    }

    public enum PlayerAnimation
    {
        WalkDown,
        WalkUp,
        WalkLeft,
        WalkRight,
        Idle
    }

    public void SetAnimation(PlayerAnimation animation)
    {
        switch (animation)
        {
            case PlayerAnimation.WalkDown:
                AnimationPlayer.Play("Walk-Down");
                break;
            case PlayerAnimation.WalkUp:
                AnimationPlayer.Play("Walk-Down");
                break;
            case PlayerAnimation.WalkLeft:
                AnimationPlayer.Play("Walk-Down");
                break;
            case PlayerAnimation.WalkRight:
                AnimationPlayer.Play("Walk-Down");
                break;
            case PlayerAnimation.Idle:
                AnimationPlayer.Play("Walk-Down");
                break;
        }
    }


    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
