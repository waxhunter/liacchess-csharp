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

        public int RemainingTypePieces(Board board, string type)
        {
            int pieces = 0;

            foreach (Piece piece in PlayerPiecesInBoard(board))
            {
                if (piece.type == type)
                {
                    pieces++;
                }
            }
            return pieces;
        }

        public List<Move> PossibleMovements(Board board)
        {
            List<Move> moveList = new List<Move>();
            
            List<Piece> pieceList = PlayerPiecesInBoard(board);

            foreach (Piece piece in pieceList)
            {
                    List<Move> pieceMovementsList = piece.PossibleMovements(board);
                    moveList.AddRange(pieceMovementsList);
            }

            return moveList;
        }

        public Move GenerateMovement(Board board)
        {
            Board initialTestBoard = new Board()
            {
                pieceList = new List<Piece>()
                {
                    new Pawn( new List<int> { 6, 0 }, COLOR_BLACK ),
                    new Pawn( new List<int> { 4, 1 }, COLOR_BLACK ),
                    new Pawn( new List<int> { 6, 2 }, COLOR_BLACK ),
                    new Pawn( new List<int> { 7, 7 }, COLOR_WHITE ),
                    new Pawn( new List<int> { 6, 6 }, COLOR_WHITE ),
                }
            };

            Board currentTestBoard = new Board()
            {
                pieceList = new List<Piece>()
                {
                    new Pawn( new List<int> { 6, 0 }, COLOR_BLACK ),
                    new Pawn( new List<int> { 2, 1 }, COLOR_BLACK ),
                }
            };

            Board lastBoard = Board.GenerateMovement(currentTestBoard, new Move(new List<int> { 6,0 }, new List<int> { 0,1 }));

            //Console.WriteLine("Peao mais avancado na pos: " + MostAdvancedPawnPosition(currentTestBoard));
            //Console.WriteLine("Posicoes avancadas: " + MoveResult.PositionsAdvanced(initialTestBoard, currentTestBoard, this));
            //Console.WriteLine("Peoes perdidos: " + MoveResult.PiecesLost(initialTestBoard, currentTestBoard, this, "Pawn"));
            //Console.WriteLine("Peoes tomados: " + MoveResult.PiecesTaken(initialTestBoard, currentTestBoard, this, "Pawn"));
            //Console.WriteLine("Peao mais avancado na pos: " + MostAdvancedPawnPosition(lastBoard));
            //Console.WriteLine("Tabuleiros diferentes 1 : " + lastBoard.IsDifferent(currentTestBoard));
            //Console.WriteLine("Tabuleiros diferentes 2 : " + lastBoard.IsDifferent(initialTestBoard));
            //Console.WriteLine("Tabuleiros diferentes 3 : " + currentTestBoard.IsDifferent(initialTestBoard));
            //Console.WriteLine("Tabuleiros diferentes 4 : " + lastBoard.IsDifferent(lastBoard));

            List<Move> possibleMoves = PossibleMovements(board);

            //Random rand = new Random();
            //Move movement = possibleMoves[rand.Next(possibleMoves.Count - 1)];

            Move movement = Minimax.DecideMovement(board, this);

            Board newBoard = Board.GenerateMovement(board, movement);

            int pawnsTaken = MoveResult.PiecesTaken(board, newBoard, this, "Pawn");
            int pawnsLost = MoveResult.PiecesLost(board, newBoard, this, "Pawn");
            int value = Minimax.CalculateMoveValue(board, newBoard, this);

            if(pawnsTaken > 0) Console.WriteLine("  == Pawns Taken: " + pawnsTaken);
            if(pawnsLost > 0) Console.WriteLine("  == Pawns Lost: " + pawnsLost);
            Console.WriteLine("  == Tabuleiro diferente: " + board.IsDifferent(newBoard));
            if(value > 0) Console.WriteLine("  == Valor da jogada: " + value);

            //Console.WriteLine("Peoes restantes: " + this.RemainingTypePieces(board, "Pawn"));
            //Console.WriteLine("Bispos restantes: " + this.RemainingTypePieces(board, "Bishop"));
            //Console.WriteLine("Torres restantes: " + this.RemainingTypePieces(board, "Rook"));

            return movement;
        }

        public Player GetEnemy()
        {
            int enemyColor;

            if (this.color == COLOR_BLACK)
                enemyColor = COLOR_WHITE;
            else
                enemyColor = COLOR_BLACK;

            Player enemy = new Player()
            {
                pName = new PlayerName() { name = "whatever" },
                color = enemyColor
            };
            return enemy;
        }

        public int MostAdvancedPawnPosition(Board board)
        {
            int mostAdvanced;

            if (this.color == COLOR_BLACK)
                mostAdvanced = 6;
            else
                mostAdvanced = 1;

            foreach (Piece piece in PlayerPiecesInBoard(board))
            {
                if (piece.IsPawn())
                {
                    if (this.color == COLOR_BLACK)
                    {
                        if (piece.verticalPosition() < mostAdvanced)
                        {
                            mostAdvanced = piece.verticalPosition();
                        }
                    }
                    else
                    {
                        if (piece.verticalPosition() > mostAdvanced)
                        {
                            mostAdvanced = piece.verticalPosition();
                        }
                    }
                }
            }
            return mostAdvanced;
        }

        public int ObjectivePosition()
        {
            if (this.color == COLOR_BLACK)
                return 0;
            else
                return 7;
        }

        public int DistanceToObjective(Board board)
        {
            return Math.Abs(ObjectivePosition() - MostAdvancedPawnPosition(board));
        }
    }
}
