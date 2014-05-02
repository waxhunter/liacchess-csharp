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

    public class Piece
    {
        public int color { get; set; }
        public string type { get; set; }
        public List<int> position;

        public virtual List<Move> PossibleMovements(Board board)
        {
            List<Move> movements = new List<Move>();
            
            return movements;
        }
    }

    public class Pawn : Piece
    {
        public const int BLACK_PAWN_START = 1;
        public const int WHITE_PAWN_START = 6;

        public Pawn(List<int> position, int color)
        {
            this.position = position;
            this.color = color;
            this.type = "Pawn";
        }

        public override List<Move> PossibleMovements(Board board)
        {
            List<Move> movements = new List<Move>();

            List<int> oneFront, twoFront, left, right, leftDiagonal, rightDiagonal;
                
            if (this.color == Player.COLOR_BLACK)
            {
                oneFront = new List<int>() { this.position[0] - 1, this.position[1] };
                twoFront = new List<int>() { this.position[0] - 2, this.position[1] };
                left = new List<int>() { this.position[0], this.position[1] - 1 };
                right = new List<int>() { this.position[0], this.position[1] + 1 };
                leftDiagonal = new List<int>() { this.position[0] - 1, this.position[1] - 1 };
                rightDiagonal = new List<int>() { this.position[0] - 1, this.position[1] + 1 };
            }
            else
            {
                oneFront = new List<int>() { this.position[0] + 1, this.position[1] };
                twoFront = new List<int>() { this.position[0] + 2, this.position[1] };
                left = new List<int>() { this.position[0], this.position[1] - 1};
                right = new List<int>() { this.position[0], this.position[1] + 1 };
                leftDiagonal = new List<int>() { this.position[0] + 1, this.position[1] - 1 };
                rightDiagonal = new List<int>() { this.position[0] + 1, this.position[1] + 1 };
            }

            // move uma para frente
            if (Board.IsValidPosition(oneFront))
            {
                if (board.PieceInPosition(oneFront) == null)
                {
                    movements.Add(new Move(this.position, oneFront));
                }
            }

            // move duas para frente
                
            if (Board.IsValidPosition(twoFront))
            {
                if (board.PieceInPosition(twoFront) == null)
                {
                    if(this.color == Player.COLOR_BLACK && this.position[0] == BLACK_PAWN_START)
                    {
                        movements.Add(new Move(this.position, twoFront));
                    }
                    else if(this.color == Player.COLOR_WHITE && this.position[0] == WHITE_PAWN_START)
                    {
                        movements.Add(new Move(this.position, twoFront));
                    }
                }
            }

            // move na diagonal esquerda frontal, tomando peça inimiga

            if (Board.IsValidPosition(leftDiagonal))
            {
                if (board.PieceInPosition(leftDiagonal) != null)
                {
                    if (board.PieceInPosition(leftDiagonal).color != this.color)
                    {
                        movements.Add(new Move(this.position, leftDiagonal));
                    }
                }
            }

            // move na diagonal direita frontal, tomando peça inimiga

            if (Board.IsValidPosition(rightDiagonal))
            {
                if (board.PieceInPosition(rightDiagonal) != null)
                {
                    if (board.PieceInPosition(rightDiagonal).color != this.color)
                    {
                        movements.Add(new Move(this.position, rightDiagonal));
                    }
                }
            }

            // en passant
            if (board.GetBoardState().enpassant != null)
            {

                // falta implementar...
            }
            
            return movements;
        }
    }

    public class Bishop : Piece
    {
        public  Bishop(List<int> position, int color)
        {
            this.position = position;
            this.color = color;
            this.type = "Bishop";
        }

        public override List<Move> PossibleMovements(Board board)
        {
            List<Move> movements = new List<Move>();

            List<int> leftUpper;
            List<int> rightUpper;
            List<int> leftLower;
            List<int> rightLower;

            bool validMovement = false;
            int i = 1;
            do
            {
                validMovement = false;
                leftUpper = new List<int>() { this.position[0] + i, this.position[1] - i };
                rightUpper = new List<int>() { this.position[0] + i, this.position[1] + i };
                leftLower = new List<int>() { this.position[0] - i, this.position[1] - i };
                rightLower = new List<int>() { this.position[0] - i, this.position[1] + i };

                if (Board.IsValidPosition(leftUpper))
                {
                    if (board.PieceInPosition(leftUpper) == null)
                    {
                        movements.Add(new Move(this.position, leftUpper));
                        validMovement = true;
                    }
                    else
                    {
                        if (board.PieceInPosition(leftUpper).color != this.color)
                        {
                            movements.Add(new Move(this.position, leftUpper));
                            validMovement = true;
                        }
                    }
                }

                if (Board.IsValidPosition(rightUpper))
                {
                    if (board.PieceInPosition(rightUpper) == null)
                    {
                        movements.Add(new Move(this.position, rightUpper));
                        validMovement = true;
                    }
                    else
                    {
                        if (board.PieceInPosition(rightUpper).color != this.color)
                        {
                            movements.Add(new Move(this.position, rightUpper));
                            validMovement = true;
                        }
                    }
                }

                if (Board.IsValidPosition(leftLower))
                {
                    if (board.PieceInPosition(leftLower) == null)
                    {
                        movements.Add(new Move(this.position, leftLower));
                        validMovement = true;
                    }
                    else
                    {
                        if (board.PieceInPosition(leftLower).color != this.color)
                        {
                            movements.Add(new Move(this.position, leftLower));
                            validMovement = true;
                        }
                    }
                }

                if (Board.IsValidPosition(rightLower))
                {
                    if (board.PieceInPosition(rightLower) == null)
                    {
                        movements.Add(new Move(this.position, rightLower));
                        validMovement = true;
                    }
                    else
                    {
                        if (board.PieceInPosition(rightLower).color != this.color)
                        {
                            movements.Add(new Move(this.position, rightLower));
                            validMovement = true;
                        }
                    }
                }

                i = i + 1;
            }
            while (validMovement == true);

            return movements;
        }
    }

    public class Rook : Piece
    {
        public Rook(List<int> position, int color)
        {
            this.position = position;
            this.color = color;
            this.type = "Rook";
        }

        public override List<Move> PossibleMovements(Board board)
        {
            List<Move> movements = new List<Move>();

            List<int> left;
            List<int> right;
            List<int> up;
            List<int> down;

            bool validMovement = false;
            int i = 1;
            do
            {
                validMovement = false;
                left = new List<int>() { this.position[0], this.position[1] - i};
                right = new List<int>() { this.position[0], this.position[1] + i};
                up = new List<int>() { this.position[0] + i, this.position[1] };
                down = new List<int>() { this.position[0] - i, this.position[1] };

                if (Board.IsValidPosition(left))
                {
                    if (board.PieceInPosition(left) == null)
                    {
                        movements.Add(new Move(this.position, left));
                        validMovement = true;
                    }
                    else
                    {
                        if (board.PieceInPosition(left).color != this.color)
                        {
                            movements.Add(new Move(this.position, left));
                            validMovement = true;
                        }
                    }
                }

                if (Board.IsValidPosition(right))
                {
                    if (board.PieceInPosition(right) == null)
                    {
                        movements.Add(new Move(this.position, right));
                        validMovement = true;
                    }
                    else
                    {
                        if (board.PieceInPosition(right).color != this.color)
                        {
                            movements.Add(new Move(this.position, right));
                            validMovement = true;
                        }
                    }
                }

                if (Board.IsValidPosition(up))
                {
                    if (board.PieceInPosition(up) == null)
                    {
                        movements.Add(new Move(this.position, up));
                        validMovement = true;
                    }
                    else
                    {
                        if (board.PieceInPosition(up).color != this.color)
                        {
                            movements.Add(new Move(this.position, up));
                            validMovement = true;
                        }
                    }
                }

                if (Board.IsValidPosition(down))
                {
                    if (board.PieceInPosition(down) == null)
                    {
                        movements.Add(new Move(this.position, down));
                        validMovement = true;
                    }
                    else
                    {
                        if (board.PieceInPosition(down).color != this.color)
                        {
                            Console.WriteLine("Movimento valido: torre de " + this.position[0] + "," + this.position[1] + " para " + down[0] + "," + down[1]); 
                            movements.Add(new Move(this.position, down));
                            validMovement = true;
                        }
                    }
                }

                i = i + 1;
            }
            while (validMovement == true);

            return movements;
        }
    }
}
