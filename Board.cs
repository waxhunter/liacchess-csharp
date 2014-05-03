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
    public class BoardState
    {
        public string board { get; set; }
        public List<int> enpassant { get; set; }
        public int who_moves { get; set; }
        public bool bad_move { get; set; }
        public int white_infractions { get; set; }
        public int black_infractions { get; set; }
        public int winner { get; set; }
        public bool move_limit { get; set; }
        public bool draw { get; set; }
    }

    public class Board
    {
        public const int MAX_PAWNS = 8;
        public const int MAX_BISHOPS = 2;
        public const int MAX_ROOKS = 2;

        private BoardState state = new BoardState();
        public List<Piece> pieceList = new List<Piece>();

        public BoardState GetBoardState()
        {
            return state;
        }
        
        public void SetBoardState(BoardState newState)
        {
            state = newState;
            LoadPieceList();
        }

        void LoadPieceList()
        {
            pieceList.Clear();
            for (int i = 0; i < state.board.Length; i++)
            {
                switch (state.board[i])
                {
                    case '.':
                        {
                            break;
                        }
                    case 'p':
                        {
                            pieceList.Add(new Pawn(IndexToPosition(i), Player.COLOR_BLACK));
                            break;
                        }
                    case 'b':
                        {
                            pieceList.Add(new Bishop(IndexToPosition(i), Player.COLOR_BLACK));
                            break;
                        }
                    case 'r':
                        {
                            pieceList.Add(new Rook(IndexToPosition(i), Player.COLOR_BLACK));
                            break;
                        }
                    case 'P':
                        {
                            pieceList.Add(new Pawn(IndexToPosition(i), Player.COLOR_WHITE));
                            break;
                        }
                    case 'B':
                        {
                            pieceList.Add(new Bishop(IndexToPosition(i), Player.COLOR_WHITE));
                            break;
                        }
                    case 'R':
                        {
                            pieceList.Add(new Rook(IndexToPosition(i), Player.COLOR_WHITE));
                            break;
                        }
                }
            }
        }

        public static bool IsValidPosition(List<int> position)
        {
            if (position[0] < 0 || position[0] > 7 || position[1] < 0 || position[1] > 7)
                return false;
            else
                return true;
        }

        public Piece PieceInPosition(List<int> position)
        {
            foreach (Piece piece in pieceList)
            {
                if (piece.position[0] == position[0] && piece.position[1] == position[1])
                {
                    return piece;
                }
            }
            return null;
        }

        public static List<int> IndexToPosition(int index)
        {
            List<int> position = new List<int>() {0, 0};

            int x = 7;
            int y = 0;

            while (index > 7)
            {
                index -= 8;
                x -= 1;
            }

            y = index;

            position[0] = x;
            position[1] = y;

            return position;
        }

        public static Board GenerateMovement(Board board, Move movement)
        {
            Board newBoard = new Board();

            foreach (Piece piece in board.pieceList)
            {
                if (piece.position[0] == movement.from[0] && piece.position[1] == movement.from[1])
                {
                    if (piece.IsPawn())
                    { 
                        Pawn newPiece = new Pawn(movement.to, piece.color);
                        newBoard.pieceList.Add(newPiece);
                    }
                    else if (piece.IsBishop())
                    {
                        Bishop newPiece = new Bishop(movement.to, piece.color);
                        newBoard.pieceList.Add(newPiece);
                    }
                    else if (piece.IsRook())
                    {
                        Rook newPiece = new Rook(movement.to, piece.color);
                        newBoard.pieceList.Add(newPiece);
                    }
                }
                else
                {
                    if(piece.position[0] != movement.to[0] || piece.position[1] != movement.to[1])
                        newBoard.pieceList.Add(piece);
                }
            }

            return newBoard;
        }

        public bool IsDifferent(Board other)
        {
            foreach (Piece otherPiece in other.pieceList)
            {
                bool found = false;
                foreach (Piece thisPiece in pieceList)
                {
                    if (thisPiece.type == otherPiece.type && thisPiece.color == otherPiece.color)
                    {
                        if (thisPiece.position[0] == otherPiece.position[0] && thisPiece.position[1] == otherPiece.position[1])
                        {
                            found = true;
                        }
                    }
                }

                if (found == false)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
