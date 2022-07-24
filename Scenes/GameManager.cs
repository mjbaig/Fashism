using Godot;
using System;

public class GameManager : Node2D
{

    private CafeLatte _player;

    private TileMap _tileMap;

    private Vector2 _playerPosition = new Vector2(100, 100);
    
    public override void _Ready()
    {

        var cafeLatte = (PackedScene)ResourceLoader.Load("res://Scenes/CafeLatte.tscn");

        _tileMap = (TileMap)GetNode("TileMap");

        _player = (CafeLatte)cafeLatte.Instance();
        
        _player._Ready();

    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
      
      try
      {
          _player.AnimationPlayer.Play("Walk-Down");
          _tileMap.SetCell(1, 2, 1);
          _player.Position = _playerPosition;
          AddChild(_player);
          _playerPosition.x += 1;
          _playerPosition.y += 1;
      }
      catch (Exception e)
      {
          GD.PrintErr(e.Message);
      }

  }
}
