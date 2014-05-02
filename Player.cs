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
                    moveList.Concat(PossibleMovements(board));
                }
            }

            return moveList;
        }

        public Move GenerateMovement(Board board)
        {
            Move movement = new Move(new List<int>() { 4, 1 }, new List<int>() { 4, 2 });

            foreach (Piece piece in board.pieceList)
            {
                if (piece.color == this.color)
                {
                    if (piece.PossibleMovements(board).Count > 0)
                    {
                        movement = piece.PossibleMovements(board)[0];
                    }
                    break;
                }
            }

            // Calculate minimax algorithm here

            return movement;
        }
    }
}
