using Godot;
using System;
using Fashism.Scenes.Character;

public class Player : Character
{

    private CharacterStats _characterStats;

    public Player()
    {
        _characterStats = new CharacterStats()
        {
            Move = 5,
            AttackRange = 2,
        };
    }

    public override CharacterStats GetCharacterStats()
    {
        return _characterStats;
    }
}
