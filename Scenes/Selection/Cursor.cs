using System;
using System.Collections.Generic;
using Fashism.Scenes;
using Fashism.Scenes.Selection;
using Godot;

public class Cursor : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    private AnimationPlayer _animationPlayer;

    private HashSet<LevelMap> _levelMaps = new HashSet<LevelMap>();

    private bool _isConfigured;

    private string _up;

    private string _down;

    private string _left;

    private string _right;

    private int _playerNumber;

    private float _clock;

    private float _lastInputTime;

    private bool _isMovementReady = true;

    private Timer _movementDelay;

    public override async void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        await ToSignal(GetTree().CreateTimer(1), "timeout");
        _animationPlayer.Play("Idle");

        _movementDelay = GetNode<Timer>("MovementDelay");
    }

    public override void _Process(float delta)
    {

        _clock += delta;
        
        if (!_isConfigured)
        {
            GD.PrintErr("cursor has no configuration");
            return;
        }
        
        HandleAcceptInput();
        
        if (Math.Abs(_lastInputTime - _clock) > 0.1f)
        {
            HandleMovementInput();
            _lastInputTime = _clock;
        }
        
    }

    private void HandleAcceptInput()
    {
        if (Input.IsActionJustPressed("ui_accept"))
        {
            foreach (var levelMap in _levelMaps)
            {
                levelMap.TileSelected(_playerNumber);
            }
        }
    }

    private void HandleMovementInput()
    {
        Direction direction;
        
        if (Input.IsActionPressed(_up) && Input.IsActionPressed(_right))
        {
            direction = Direction.UpRight;
            GD.Print(direction);
        }
        else if (Input.IsActionPressed(_up) && Input.IsActionPressed(_left))
        {
            direction = Direction.UpLeft;
            GD.Print(direction);
        }
        else if (Input.IsActionPressed(_down) && Input.IsActionPressed(_right))
        {
            direction = Direction.DownRight;
            GD.Print(direction);
        }
        else if (Input.IsActionPressed(_down) && Input.IsActionPressed(_left))
        {
            direction = Direction.DownLeft;
            GD.Print(direction);
        }
        else if (Input.IsActionPressed(_up))
        {
            direction = Direction.Up;
            GD.Print(direction);
        }
        else if (Input.IsActionPressed(_down))
        {
            direction = Direction.Down;
            GD.Print(direction);
        }
        else if (Input.IsActionPressed(_left))
        {
            direction = Direction.Left;
            GD.Print(direction);
        }
        else if (Input.IsActionPressed(_right))
        {
            direction = Direction.Right;
            GD.Print(direction);
        }
        else if (Input.IsActionPressed(_right))
        {
            direction = Direction.Right;
            GD.Print(direction);
        }
        else
        {
            direction = Direction.None;
        }

        foreach (var levelMap in _levelMaps)
        {
            levelMap.ReceiveInput(direction);
        }

        if (direction != Direction.None)
        {
            _isMovementReady = false;
            _lastInputTime = _clock;
        }
    }

    public void setMovementReady()
    {
        _isMovementReady = true;
    }

    public void Subscribe(LevelMap level, CursorConfig cursorConfig)
    {
        _levelMaps.Add(level);
        _isConfigured = true;
        _playerNumber = cursorConfig.PlayerNumber;
        _up = cursorConfig.CursorDirections.Up;
        _down = cursorConfig.CursorDirections.Down;
        _left = cursorConfig.CursorDirections.Left;
        _right = cursorConfig.CursorDirections.Right;
    }
}