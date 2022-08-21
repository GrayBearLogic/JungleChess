using System;

namespace JungleCore
{
    public enum Side
    {
        Nobody,
        Player,
        Enemy
    }

    public static class SideEx
    {
        public static Side Opposite(this Side side)
        {
            switch (side)
            {
                case Side.Player:
                    return Side.Enemy;
                case Side.Enemy:
                    return Side.Player;
                default:
                    return Side.Nobody;
            }
        }

        public static Point BasePosition(this Side side)
        {
            switch (side)
            {
                case Side.Player:
                    return new Point(3, 0);
                case Side.Enemy:
                    return new Point(3, 8);
                default:
                    throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }
        }
        
        public static bool Trap(this Side side, Point p)
        {
            switch (side)
            {
                case Side.Player:
                    return p == new Point(2, 8) ||
                           p == new Point(3, 7) ||
                           p == new Point(4, 8);
                case Side.Enemy:
                    return p == new Point(2, 0) ||
                           p == new Point(3, 1) ||
                           p == new Point(4, 0);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}