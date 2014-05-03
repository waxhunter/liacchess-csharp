using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIAC_CHESS
{
    class MinimaxTreeNode
    {
        public int value;
        public Move movement = null;
        public List<MinimaxTreeNode> childMovements = new List<MinimaxTreeNode>();
    }

    class Minimax
    {
        public const int MAX_HEURISTIC_VALUE = 1000;
        public const int MIN_HEURISTIC_VALUE = -1000;

        public const int PAWN_TAKEN_WEIGHT = 2;
        public const int BISHOP_TAKEN_WEIGHT = 5;
        public const int ROOK_TAKEN_WEIGHT = 6;

        public const int PAWN_ADVANCE_WEIGHT = 1;
        public const int POSITION_ADVANCE_WEIGHT = 2;

        public static Move DecideMovement(Board board, Player player)
        {
            Board newBoard = new Board();
            newBoard.pieceList = new List<Piece>(board.pieceList);

            MinimaxTreeNode minimaxTree = GenerateMinimaxTree(null, player, player.GetEnemy(), board, newBoard, 0, 3);

            List<MinimaxTreeNode> biggestNodes = new List<MinimaxTreeNode>();

            int biggestValue = MIN_HEURISTIC_VALUE;
            
            foreach (MinimaxTreeNode childNode in minimaxTree.childMovements)
            {
                if (childNode.value > biggestValue)
                {
                    biggestNodes.Clear();
                    biggestNodes.Add(childNode);
                    biggestValue = childNode.value;
                }
                else if (childNode.value == biggestValue)
                {
                    biggestNodes.Add(childNode);
                }
            }

            MinimaxTreeNode randomNode;
            Random rand = new Random();
            randomNode = biggestNodes[rand.Next(biggestNodes.Count - 1)];

            Console.WriteLine("Calculado movimento. Valor: " + randomNode.value);

            return randomNode.movement;
        }

        public static MinimaxTreeNode GenerateMinimaxTree(Move previousMovement, Player max, Player min, Board initial, Board current, int currentDepth, int treeDepth)
        {
            Player player;
            if (currentDepth % 2 == 0)
                player = max;
            else
                player = min;

            MinimaxTreeNode node = new MinimaxTreeNode();
            node.movement = previousMovement;

            if(currentDepth == treeDepth)
            {
                node.value = CalculateMoveValue(initial, current, max);
                node.childMovements = null;
            }
            else
            {
                List<Move> possibleMoves = player.PossibleMovements(current);
                foreach (Move move in possibleMoves)
                {
                    Board newBoard = new Board();
                    newBoard.pieceList = new List<Piece>(Board.GenerateMovement(current, move).pieceList);
                    MinimaxTreeNode childNode = GenerateMinimaxTree(move, max, min, initial, newBoard, currentDepth + 1, treeDepth);
                    node.childMovements.Add(childNode);
                }

                if (node.childMovements.Count == 0)
                    node.value = CalculateMoveValue(initial, current, max);
                else
                {
                    bool firstNode = true;
                    foreach (MinimaxTreeNode childNode in node.childMovements)
                    {
                        if (firstNode == true)
                        {
                            firstNode = false;
                            node.value = childNode.value;
                        }
                        else
                        {
                            if (player == max)
                            {
                                if (childNode.value > node.value)
                                    node.value = childNode.value;
                            }
                            else
                            {
                                if (childNode.value < node.value)
                                    node.value = childNode.value;
                            }
                        }
                    }
                }
            }

            return node;
        }

        public static int CalculateMoveValue(Board initial, Board current, Player player)
        {
            int value = 0;

            if (MoveResult.GameWon(current, player) == true)
            {
                value = MAX_HEURISTIC_VALUE;
            }
            else if (MoveResult.GameLost(current, player) == true)
            {
                value = MIN_HEURISTIC_VALUE;
            }
            else
            {

            //Console.WriteLine("Tabuleiro diferente: " + initial.IsDifferent(current));
                // Heuristic value is defined by:
                // - Pawns taken: PAWN_TAKEN_WEIGHT * number of pawns taken * (initial pawns number - remaining pawns number)
            value += PAWN_TAKEN_WEIGHT * MoveResult.PiecesTaken(initial, current, player, "Pawn");

                // - Bishops taken: BISHOP_TAKEN_WEIGHT * number of bishops taken * (initial bishops number - remaining bishops number)
            value += BISHOP_TAKEN_WEIGHT * MoveResult.PiecesTaken(initial, current, player, "Bishop");

                // - Rooks taken: ROOK_TAKEN_WEIGHT * number of rooks taken * (initial rooks number - remaining rooks number)
            value += ROOK_TAKEN_WEIGHT * MoveResult.PiecesTaken(initial, current, player, "Rook");

                // - Pawns lost: follows same logic as pawns taken, adjusting same value as negative for enemy
            value -= PAWN_TAKEN_WEIGHT * MoveResult.PiecesLost(initial, current, player, "Pawn");

                // - Bishops lost: follows same logic as bishops taken, negative value
            value -= BISHOP_TAKEN_WEIGHT * MoveResult.PiecesLost(initial, current, player, "Bishop");

                // - Rooks lost:
            value -= ROOK_TAKEN_WEIGHT * MoveResult.PiecesLost(initial, current, player, "Rook");

                // - Number of pawns advanced:
                //value += PAWN_ADVANCE_WEIGHT * MoveResult.PawnsAdvanced(initial, current, player);

                // - Number of positions advanced, multiplied by closeness to objective
                value += POSITION_ADVANCE_WEIGHT * MoveResult.PositionsAdvanced(initial, current, player);
            }

                    return value;
        }
    }
}
