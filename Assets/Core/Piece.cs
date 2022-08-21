namespace JungleCore
{
    public enum Rank
    {
        Mouse    = 0,
        Cat      = 1,
        Dog      = 2,
        Wolf     = 3,
        Leopard  = 4,
        Tiger    = 5,
        Lion     = 6,
        Elephant = 7
    }
    
    public struct PieceInfo
    {
        public bool IsAlive { get; set; }
        public Point Pos { get; set; }

        public PieceInfo(int x, int y)
        {
            IsAlive = true;
            Pos     = new Point(x, y);
        }
    }
    
}