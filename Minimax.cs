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
        public const int WIN_HEURISTIC_VALUE = 1000;
        public const int LOSS_HEURISTIC_VALUE = -1000;

        // GenerateMovementTree: Gera a árvore do algoritmo minimax, dado o tabuleiro inicial e profundidade desejada
        // OBSERVAÇÃO: Na primeira chamada, deve ser chamado com parâmetro previousMovement = null
        public static MinimaxTreeNode GenerateMinimaxTree(Move previousMovement, Player max, Player min, Board initial, Board current, int currentDepth, int treeDepth)
        {
            Player player;

            MinimaxTreeNode node = new MinimaxTreeNode();
            node.movement = previousMovement;

            // Decide which player will play in current turn
            if ((currentDepth % 2) == 0)
                player = min;
            else
                player = max;

            // If the function reaches the maximum tree depth, we must not
            // generate further child nodes, and calculate current nodes
            // values.
            if(currentDepth == treeDepth)
            {
                node.value = CalculateMoveValue(initial, current);
                node.childMovements = null;
            }
            else
            {
                // Generate node's child list
                foreach (Move move in player.PossibleMovements(current))
                {
                    MinimaxTreeNode childNode = GenerateMinimaxTree(move, max, min, initial, current.GenerateMovement(move), currentDepth + 1, treeDepth);
                    node.childMovements.Add(childNode);
                }

                node.value = 0;

                // In this section, we decide the node value by searching
                // the child list and applying minimum or maximum function,
                // depending on whose turn it is (MAX or MIN player)

                // If node has no child movements, then we must calculate
                // it's value.
                if (node.childMovements.Count == 0)
                    node.value = CalculateMoveValue(initial, current);
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
                            if (player == min)
                            {
                                if (childNode.value < node.value)
                                    node.value = childNode.value;
                            }
                            else
                            {
                                if (childNode.value > node.value)
                                    node.value = childNode.value;
                            }
                        }
                    }
                }
            }

            return node;
        }

        public static int CalculateMoveValue(Board initial, Board current)
        {
            // heuristic function value here
            return 0;
        }
    }
}
