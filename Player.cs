using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace LIAC_CHESS
{
    public class PlayerName
    {
        public string name { get; set; }
    }

    public class Player
    {
        public const int COLOR_WHITE = 1;
        public const int COLOR_BLACK = -1;

        public PlayerName pName { get; set; }
        public int color { get; set; }

        public List<Piece> PlayerPiecesInBoard(Board board)
        {
            List<Piece> pieceList = new List<Piece>();
            foreach (Piece piece in board.pieceList)
            {
                if (piece.color == this.color)
                {
                    pieceList.Add(piece);
                }
            }
            return pieceList;
        }

        public List<Move> PossibleMovements(Board board)
        {
            List<Move> moveList = new List<Move>();
            
            if (board.GetBoardState().draw == true || board.GetBoardState().winner != 0)
            {
                // Game is over, therefore no movements can be made
                return moveList;
            }
            else
            {
                List<Piece> pieceList = PlayerPiecesInBoard(board);

                foreach (Piece piece in pieceList)
                {
                    List<Move> pieceMovementsList = piece.PossibleMovements(board);
                    moveList.AddRange(pieceMovementsList);
                }
            }

            return moveList;
        }

        public Move GenerateMovement(Board board)
        {
            List<Move> possibleMoves = PossibleMovements(board);

            Random rand = new Random();
            Move movement = possibleMoves[rand.Next(possibleMoves.Count - 1)];

            // Calculate minimax algorithm here

            return movement;
        }
    }
}
