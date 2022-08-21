namespace JungleCore
{
    public struct Move
    {
        public Board Board;
        public int   PieceId;
        public Point Destination;

        public Move(int pieceId, Point dest, Board board)
        {
            Board     = board;
            PieceId   = pieceId;
            Destination = dest;
        }

        public override string ToString()
        {
            return $"{Destination}";
        }
    }
}