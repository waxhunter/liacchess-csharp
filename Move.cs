using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIAC_CHESS
{
    public class Move
    {
        public List<int> from { get; set; }
        public List<int> to { get; set; }

        public Move(List<int> from, List<int> to)
        {
            this.from = from;
            this.to = to;
        }
    }

    public class MoveResult
    {
        // ok
        public static int PiecesTaken(Board initial, Board current, Player player, string pieceType)
        {
            int initialPieces = player.GetEnemy().RemainingTypePieces(initial, pieceType);
            int currentPieces = player.GetEnemy().RemainingTypePieces(current, pieceType);

            return (initialPieces - currentPieces);
        }

        // ok
        public static int PiecesLost(Board initial, Board current, Player player, string pieceType)
        {
            int initialPieces = player.RemainingTypePieces(initial, pieceType);
            int currentPieces = player.RemainingTypePieces(current, pieceType);

            return (initialPieces - currentPieces);
        }

        public static int PawnsAdvanced(Board initial, Board current, Player player)
        {
            int pawnsAdvanced = 0;

            

            return pawnsAdvanced;
        }

        // ok
        public static int PositionsAdvanced(Board initial, Board current, Player player)
        {
            return (player.DistanceToObjective(initial) - player.DistanceToObjective(current));
        }

        public static bool GameWon(Board current, Player player)
        {
            if (player.GetEnemy().RemainingTypePieces(current, "Pawn") == 0)
            {
                return true;
            }
            else if (player.MostAdvancedPawnPosition(current) == player.ObjectivePosition())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool GameLost(Board current, Player player)
        {
            return GameWon(current, player.GetEnemy());
        }
    }
}
