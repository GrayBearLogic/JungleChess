using System;
using System.Collections.Generic;

namespace JungleCore
{
    public class Game
    {
        public event DeathEventHandler someoneDead;
        public event Action<Side>      matchOver;

        public Board Board;

        public Game()
        {
            Board = new Board(Side.Player);
        }

        public List<Point> MovesFor(Rank rank)
        {
            return Board.AccessiblePositions(rank);
        }

        public void Move(Rank rank, Point destination)
        {
            var res = Board.Move(rank, destination);

            for (var i = 0; i < 8; i++)
            {
                if (Board.OppositeTeam[i].IsAlive && res.ActiveTeam[i].IsAlive == false)
                {
                    someoneDead?.Invoke(this, new DeathEventArgs((Rank) i, res.ActiveSide));
                    break;
                }
            }

            Board = res;

            if (Board.Winner() != Side.Nobody)
                matchOver?.Invoke(Board.Winner());
        }
    }
}