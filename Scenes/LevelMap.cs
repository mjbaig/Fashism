using Godot;

namespace Fashism.Scenes
{
    public abstract class LevelMap: Node2D
    {
        public abstract void ReceiveInput(Direction direction);
        public abstract void TileSelected(int PlayerNumber);
    }
    
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight,
        None,
    }
    
    
}