using Godot;
using System;

public class TestMap : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    
    private Player _player;

    private TileMap _tileMap;

    private Vector2 _playerPosition = new Vector2(8, 8);
    
    private int _tileMaxWidth = 300;
    private int _tileMinWidth = 0;
            
    private int _tileMaxHeight = 300;
    private int _tileMinHeight = 300;
    
    public override void _Ready()
    {
        var player = (PackedScene)ResourceLoader.Load("res://Scenes/Player.tscn");
 
        _tileMap = (TileMap)GetNode("Ground");

        _player = (Player)player.Instance();
        
        _player._Ready();

        for (int x = 0; x < _tileMaxWidth; x++)
        {
            for (int y = 0; y < _tileMaxHeight; y++)
            {
                int tile = (x % 2) + (y % 2 != 0 ? 1 : 0);
                _tileMap.SetCell(x, y, tile);
            }
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
      
        try
        {
            _player.SetAnimation(Player.PlayerAnimation.WalkDown);
            _player.Position = _playerPosition;
            AddChild(_player);
            //
            // _playerPosition.x += 1;
            // _playerPosition.y += 1;

            for (var x = _tileMinWidth; x <= _tileMaxWidth; x++)
            {
                _tileMap.SetCell(x, _tileMaxHeight, 0);
                _tileMap.SetCell(x, _tileMinHeight, 0);
            }

            for (var y = _tileMinHeight; y <= _tileMaxHeight; y++)
            {
                _tileMap.SetCell(_tileMaxWidth, y, 0);
                _tileMap.SetCell(_tileMinWidth, y, 0);
            }

            _tileMaxHeight--;
            _tileMinHeight++;

            _tileMaxWidth++;
            _tileMinWidth--;

        }
        catch (Exception e)
        {
            GD.PrintErr(e.Message);
        }

    }
}
