using System;
using System.Collections.Generic;
using Fashism.Scenes;
using Fashism.Scenes.Character;
using Fashism.Scenes.Selection;
using Godot;

public class TestMap : LevelMap
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    // private Character _player;

    private Cursor _cursor;

    private TileMap _groundMap;

    private TileMap _selectMap;

    private TileMap _featureMap;
    
    private Vector2 _cursorPosition = new Vector2(8, 8);
    
    private int[,] _inboundMatrix;

    private Character[,] _characterMatrix;

    private Vector2 _cursorIndex = new Vector2(0, 0);

    private Boolean _isSomethingSelected;
    
    public override void _Ready()
    {
        
        _groundMap = GetNode<TileMap>("Ground");

        _featureMap = GetNode<TileMap>("Features");

        _selectMap = GetNode<TileMap>("Selected");
        
        _cursor = GetNode<Cursor>("Cursor");
        
        var rect = _groundMap.GetUsedRect();
        
        _inboundMatrix = new int[(int)rect.End.x , (int)rect.End.y];
        
        for (int y = 0; y < rect.End.y; y++)
        {
            for (int x = 0; x < rect.End.x; x++)
            {
                _inboundMatrix[x, y] = _groundMap.GetCell(x, y);
                
            }
        }

        _characterMatrix = new Character[(int)rect.End.x, (int)rect.End.y];
        
        _cursor.Position = _cursorPosition;
        var playerOneConfig = new CursorConfig
        {
            PlayerNumber = 1
        };
        var cursorDirections = new CursorDirections
        {
            Up = "ui_up",
            Down = "ui_down",
            Left = "ui_left",
            Right = "ui_right"
        };
        playerOneConfig.CursorDirections = cursorDirections;
        _cursor.Subscribe(this, playerOneConfig);
        _InitializeCharacters();
    }

    public void _InitializeCharacters()
    {
        var x = 0;
        const int y = 0;
        
        foreach (var characterResourcePath in Character.characterList)
        {
            var characterResource = ResourceLoader.Load<PackedScene>(characterResourcePath);
            var character = characterResource.Instance<Character>();
            character._Ready();
            character.SetAnimation(CharacterAnimation.WalkDown);
            character.Position = _CoordinatesToPosition(x, y);
            AddChild(character);
            _characterMatrix[x, y] = character;
            x++;
        }
        
        
    }

    private Vector2 _CoordinatesToPosition(float x, float y)
    {
        return new Vector2(x * _groundMap.CellSize.x + _groundMap.CellSize.x / 2,
            y * _groundMap.CellSize.x + _groundMap.CellSize.x / 2 );
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override async void _Process(float delta)
    {
        try
        {
        }
        catch (Exception e)
        {
            GD.PrintErr(e.Message);
        }
    }

    public override void ReceiveInput(Direction direction)
    {
        
        Vector2 position;

        if (direction == Direction.Up)
        {
            position = new Vector2(_cursor.Position.x, _cursor.Position.y - _groundMap.CellSize.x);
        }
        else if (direction == Direction.Down)
        {
            position = new Vector2(_cursor.Position.x, _cursor.Position.y + _groundMap.CellSize.x);
        }
        else if (direction == Direction.Left)
        {
            position = new Vector2(_cursor.Position.x - _groundMap.CellSize.x, _cursor.Position.y);
        }
        else if (direction == Direction.Right)
        {
            position = new Vector2(_cursor.Position.x + _groundMap.CellSize.x, _cursor.Position.y);
        }
        else if (direction == Direction.UpLeft)
        {
            position = new Vector2(_cursor.Position.x - _groundMap.CellSize.x, _cursor.Position.y - _groundMap.CellSize.x);
        }
        else if (direction == Direction.UpRight)
        {
            position = new Vector2(_cursor.Position.x + _groundMap.CellSize.x, _cursor.Position.y - _groundMap.CellSize.x);
        }
        else if (direction == Direction.DownLeft)
        {
            position = new Vector2(_cursor.Position.x - _groundMap.CellSize.x, _cursor.Position.y + _groundMap.CellSize.x);
        }
        else if (direction == Direction.DownRight)
        {
            position = new Vector2(_cursor.Position.x + _groundMap.CellSize.x, _cursor.Position.y + _groundMap.CellSize.x);
        }
        else
        {
            return;
        }

        var cursorIndex = new Vector2((position.x - _groundMap.CellSize.x / 2) / 16,
            (position.y - _groundMap.CellSize.x / 2) / 16);
        
        if ((position.x - _groundMap.CellSize.x / 2) % _groundMap.CellSize.x == 0 
            && (position.y - _groundMap.CellSize.x / 2) % _groundMap.CellSize.x == 0 
            && cursorIndex.x >= 0 
            && cursorIndex.y >= 0 
            && cursorIndex.x < _inboundMatrix.GetLength(0) 
            && cursorIndex.y < _inboundMatrix.GetLength(1) 
            && _inboundMatrix[(int)cursorIndex.x, (int)cursorIndex.y] != -1)
        {
            _cursor.Position = position;
            _cursorIndex = cursorIndex;
        }
        
    }

    public override void TileSelected(int playerNumber)
    {
        GD.Print("tile selected by player " + playerNumber + " " + _cursorIndex);

        if (_characterMatrix[(int)_cursorIndex.x, (int)_cursorIndex.y] != null)
        {
            var name = _characterMatrix[(int)_cursorIndex.x, (int)_cursorIndex.y].Name;
            GD.Print("Clicked on Character " + name);
            PlayerSelected(_characterMatrix[(int)_cursorIndex.x, (int)_cursorIndex.y], _cursorIndex);
            _isSomethingSelected = true;
        }

    }

    public override void Deselect()
    {
        var rect = _selectMap.GetUsedRect();
        
        for (int y = 0; y < rect.End.y; y++)
        {
            for (int x = 0; x < rect.End.x; x++)
            {
                _selectMap.SetCell(x, y, -1);
            }
        }

        _isSomethingSelected = false;
    }

    private void ApplyMovementRange(Character character, Vector2 characterPosition)
    {
        Queue<Thing> queue = new Queue<Thing>();

        queue.Enqueue(new Thing()
        {
            Position = characterPosition,
            StepsRemaining = character.GetCharacterStats().Move
        });

        int[][] directions =
        {
            new[] { 0, 1 },
            new[] { 0, -1 },
            new[] { 1, 0 },
            new[] { -1, 0 }
        };
        
        GD.Print(queue);

        while (queue.Count != 0)
        {
            var thing = queue.Dequeue();
            _selectMap.SetCell((int)thing.Position.x, (int)thing.Position.y, 0);

            var stepsRemaining = thing.StepsRemaining - 1;

            if (stepsRemaining > 0)
            {
                foreach (var direction in directions)
                {
                    var newX = (int)thing.Position.x + direction[0];
                    var newY = (int)thing.Position.y + direction[1];
                    
                    GD.Print(newX, " ,",newY);
                    
                    GD.Print(_selectMap.GetCell(newX, newY) , _featureMap.GetCell(newX, newY));
                    
                    if (newX >= 0
                        && newY >= 0
                        && newX < _inboundMatrix.GetLength(0)
                        && newY < _inboundMatrix.GetLength(1)
                        && _inboundMatrix[newX, newY] == 1 
                        && (_selectMap.GetCell(newX, newY) == -1 || _selectMap.GetCell(newX, newY) == 1 )
                        && _featureMap.GetCell(newX, newY) == -1)
                    {
                        queue.Enqueue(new Thing()
                        {
                            Position = new Vector2(newX, newY),
                            StepsRemaining = stepsRemaining
                        });
                    }
                    
                }
            }

        }
    }

    private void ApplyAttackRange(Character character, Vector2 characterPosition)
    {
        Queue<Thing> queue = new Queue<Thing>();

        queue.Enqueue(new Thing()
        {
            Position = characterPosition,
            StepsRemaining = character.GetCharacterStats().Move + character.GetCharacterStats().AttackRange
        });

        int[][] directions =
        {
            new[] { 0, 1 },
            new[] { 0, -1 },
            new[] { 1, 0 },
            new[] { -1, 0 }
        };
        
        GD.Print(queue);

        while (queue.Count != 0)
        {
            var thing = queue.Dequeue();
            _selectMap.SetCell((int)thing.Position.x, (int)thing.Position.y, 1);

            var stepsRemaining = thing.StepsRemaining - 1;

            if (stepsRemaining > 0)
            {
                foreach (var direction in directions)
                {
                    var newX = (int)thing.Position.x + direction[0];
                    var newY = (int)thing.Position.y + direction[1];
                    
                    GD.Print(newX, " ,",newY);
                    
                    GD.Print(_selectMap.GetCell(newX, newY) , _featureMap.GetCell(newX, newY));
                    
                    if (newX >= 0
                        && newY >= 0
                        && newX < _inboundMatrix.GetLength(0)
                        && newY < _inboundMatrix.GetLength(1)
                        && _inboundMatrix[newX, newY] == 1 
                        && _selectMap.GetCell(newX, newY) == -1)
                    {
                        queue.Enqueue(new Thing()
                        {
                            Position = new Vector2(newX, newY),
                            StepsRemaining = stepsRemaining
                        });
                    }
                    
                }
            }

        }
    }

    private void PlayerSelected(Character character, Vector2 characterPosition)
    {
        ApplyAttackRange(character, characterPosition);
        ApplyMovementRange(character, characterPosition);
    }

    private class Thing
    {
        public Vector2 Position { get; set; }
        public int StepsRemaining { get; set; }
    }


}