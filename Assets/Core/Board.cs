using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JungleCore
{
    public struct Board
    {
        private const int Width  = 7;
        private const int Height = 9;

        public Side ActiveSide;
        
        private PieceInfo[] enemies;
        private PieceInfo[] allies;
        

        public PieceInfo[] PlayerTeam
        {
            get => allies;
        }
        public PieceInfo[] EnemyTeam
        {
            get => enemies;
        }
        public PieceInfo[] OppositeTeam
        {
            get => ActiveSide == Side.Player ? enemies : allies;
        }
        public PieceInfo[] ActiveTeam
        {
            get => ActiveSide == Side.Player ? allies : enemies;
        }


        public Board(Side first)
        {
            allies = new[]
            {
                new PieceInfo(6, 2), //mouse
                new PieceInfo(1, 1), //cat
                new PieceInfo(5, 1), //dog
                new PieceInfo(2, 2), //wolf
                new PieceInfo(4, 2), //panthera
                new PieceInfo(0, 0), //Tiger
                new PieceInfo(6, 0), //Lion
                new PieceInfo(0, 2), //Elephant
            };
            
            enemies = new[]
            {
                new PieceInfo(0, 6), //mouse
                new PieceInfo(5, 7), //cat
                new PieceInfo(1, 7), //dog
                new PieceInfo(4, 6), //wolf
                new PieceInfo(2, 6), //panthera
                new PieceInfo(6, 8), //Tiger
                new PieceInfo(0, 8), //Lion
                new PieceInfo(6, 6), //Elephant
            };
            ActiveSide = first;
        }


        public List<Point> AccessiblePositions(Rank rank)
        {
            var res = new List<Point>(4);
            AddPoint(res, rank, Direction.Down);
            AddPoint(res, rank, Direction.Up);
            AddPoint(res, rank, Direction.Left);
            AddPoint(res, rank, Direction.Right);
            return res;
        }

        private void AddPoint(ICollection<Point> res, Rank rank, Direction direction)
        {
            var piecePos = ActiveTeam[(int)rank].Pos;

            var dest = piecePos.Shift(direction);
                
            if (rank == Rank.Lion || rank == Rank.Tiger)
            {
                while (dest.InWater)
                {
                    if (allies[0].Pos == dest || enemies[0].Pos == dest)
                        return; // there is a mouse in the lake
                    
                    dest = dest.Shift(direction);
                } 
            }
            
            if (dest == ActiveSide.BasePosition())
                return; // cannot enter own base
            
            if (dest.X < 0 || dest.X >= Width || dest.Y < 0 || dest.Y >= Height)
                return; // cannot leave the board

            if (rank != Rank.Mouse && dest.InWater)
                return; // cannot enter the lake if is not a mouse

            if (ActiveTeam.Any(n => n.IsAlive && n.Pos == dest))
                return; // cannot step on allie

            if (OpponentAt(dest, out var enemyRank))
            {
                if (piecePos.InWater != dest.InWater)
                    return; // cannot attack from the coast

                if (ActiveSide.Opposite().Trap(dest))
                    res.Add(dest); // definitely win if enemy is in trap

                switch (rank)
                {
                    case Rank.Mouse:
                        if (enemyRank == Rank.Mouse || enemyRank == Rank.Elephant)
                            res.Add(dest);
                        break;
                    case Rank.Elephant:
                        if (enemyRank > Rank.Mouse)
                            res.Add(dest);
                        break;
                    default:
                        if (enemyRank <= rank)
                            res.Add(dest);
                        break;
                }
            }
            else // the cell is free
            {
                res.Add(dest);
            }
        }
        
        private bool OpponentAt(Point position, out Rank enemyRank)
        {
            for (var i = 0; i < OppositeTeam.Length; i++)
            {
                var piece = OppositeTeam[i];
                if (piece.IsAlive && piece.Pos == position)
                {
                    enemyRank = (Rank) i;
                    return true;
                }
            }

            enemyRank = Rank.Mouse;
            return false;
        }

        public Side Winner()
        {
            if (enemies.Any(n => n.Pos == Side.Player.BasePosition()))
                return Side.Enemy;
            else if (allies.Any(n => n.Pos == Side.Enemy.BasePosition()))
                return Side.Player;
            else
                return Side.Nobody;
        }

        public double Score
        {
            get => throw new NotImplementedException();
        }

        private int DistanceToBase(Side side)
        {
            var oppositeBase = side.Opposite().BasePosition();

            return (side == Side.Player ? allies : enemies)
                .Select(n => n.Pos.DistanceTo(oppositeBase))
                .Min();
        }

        public Board Move(Rank rank, Point dest)
        {
            var res = new Board
            {
                enemies = enemies.ToArray(),
                allies = allies.ToArray(),
                ActiveSide = this.ActiveSide,
            };

            for (var i = 0; i < 8; i++)
            {
                if (res.OppositeTeam[i].IsAlive && res.OppositeTeam[i].Pos == dest)
                {
                    res.OppositeTeam[i].IsAlive = false;
                    break;
                }
            }
            res.ActiveTeam[(int) rank].Pos = dest;

            res.ActiveSide = res.ActiveSide.Opposite();

            return res;
        }

        public IEnumerable<Move> AllPosibleMoves
        {
            get
            {
                for (var i = 0; i < 8; i++)
                    if (ActiveTeam[i].IsAlive)
                        foreach (var destination in AccessiblePositions((Rank)i))
                            yield return new Move(i, destination, Move((Rank)i, destination));
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("  -- PLAYER --");
            for (var i = 0; i < 8; i++)
                if (PlayerTeam[i].IsAlive)
                    sb.AppendLine($"{PlayerTeam[i].Pos}\t{(Rank) i,-10}");
            
            sb.AppendLine("  -- ENEMY --");
            for (var i = 0; i < 8; i++)
                if (EnemyTeam[i].IsAlive)
                    sb.AppendLine($"{EnemyTeam[i].Pos}\t{(Rank) i,-10}");

            return sb.ToString();
        }
    }
}