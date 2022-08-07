namespace Fashism.Scenes.Selection
{
    public class CursorConfig
    {
        public int PlayerNumber { get; set; }

        public CursorDirections CursorDirections { get; set; }
    }

    public class CursorDirections
    {
        public string Up { get; set; }
        public string Down { get; set; }
        public string Left { get; set; }
        public string Right { get; set; }
    }
}