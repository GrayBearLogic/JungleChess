using System;

namespace JungleCore
{
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left,
    }
    
    public readonly struct Point
    {
        public readonly int X;
        public readonly int Y;

        public Point(int x, int y)
        {
            this.Y = y;
            this.X = x;
        }

        public Point Shift(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:    
                    return new Point(X, Y + 1);
                case Direction.Right: 
                    return new Point(X + 1, Y);
                case Direction.Down:  
                    return new Point(X, Y - 1);
                case Direction.Left:  
                    return new Point(X - 1, Y);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool InWater
        {
            get =>
                X == 1 && Y == 3 ||
                X == 1 && Y == 4 ||
                X == 1 && Y == 5 ||
                X == 2 && Y == 3 ||
                X == 2 && Y == 4 ||
                X == 2 && Y == 5 ||

                X == 4 && Y == 3 ||
                X == 4 && Y == 4 ||
                X == 4 && Y == 5 ||
                X == 5 && Y == 3 ||
                X == 5 && Y == 4 ||
                X == 5 && Y == 5;
        }

        public int DistanceTo(Point target)
        {
            return Math.Abs(X - target.X) + Math.Abs(Y - target.Y);
        }

        private bool Equals(Point other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is Point other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public static bool operator ==(Point a, Point b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Point a, Point b)
        {
            return !(a == b);
        }

        public static implicit operator UnityEngine.Vector3(Point input)
        {
            return new UnityEngine.Vector3(input.X, input.Y, 0);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}