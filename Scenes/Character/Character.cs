using Godot;

namespace Fashism.Scenes.Character
{
    public abstract class Character : KinematicBody2D
    {
        public static string[] characterList = new string[]
        {
            "res://Scenes/Character/Player.tscn",
            "res://Scenes/Character/Aloo.tscn",
        };

        private AnimationPlayer AnimationPlayer { get; set; }

        public abstract CharacterStats GetCharacterStats();
        
        public override void _Ready()
        {
            AnimationPlayer = (AnimationPlayer)GetNode("AnimationPlayer");
        }

        public void SetAnimation(CharacterAnimation animation)
        {
            switch (animation)
            {
                case CharacterAnimation.WalkDown:
                    AnimationPlayer.Play("Walk-Down");
                    break;
                case CharacterAnimation.WalkUp:
                    AnimationPlayer.Play("Walk-Down");
                    break;
                case CharacterAnimation.WalkLeft:
                    AnimationPlayer.Play("Walk-Down");
                    break;
                case CharacterAnimation.WalkRight:
                    AnimationPlayer.Play("Walk-Down");
                    break;
                case CharacterAnimation.Idle:
                    AnimationPlayer.Play("Walk-Down");
                    break;
            }
        }
    }
    
    public enum CharacterAnimation
    {
        WalkDown,
        WalkUp,
        WalkLeft,
        WalkRight,
        Idle
    }

    public class CharacterStats
    {
        public int Move { get; set; }
        public int AttackRange { get; set; }
    }
}