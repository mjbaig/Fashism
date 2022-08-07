using Godot;
using System;
using Fashism.Scenes.Character;

public class Aloo : Character
{
    private CharacterStats _characterStats;

    public Aloo()
    {
        _characterStats = new CharacterStats()
        {
            Move = 5
        };
    }

    public override CharacterStats GetCharacterStats()
    {
        return _characterStats;
    }
}