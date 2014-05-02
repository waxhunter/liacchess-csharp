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
    class Program
    {
        static void Main(string[] args)
        {
            #region Declarations
            NetworkStream stream;
            TcpClient client = new TcpClient();
            Board board = new Board();
            BoardState boardState = new BoardState();
            Player player = new Player(); 
            #endregion

            #region Connection
            string ip;
            Console.Write("Digite o ip para efetuar conexao: ");
            ip = Console.ReadLine();

            Console.Write("Digite a porta para efetuar conexao: ");
            String portstring = Console.ReadLine();
            Console.WriteLine(" ");

            int port = Convert.ToInt32(portstring);

            stream = Communication.Connect(ip, port, client);

            Console.WriteLine("Enviando informacao de nome...");
            PlayerName pName = new PlayerName { name = "King Joffrey" };
            player.pName = pName;
            Communication.SendToServer(stream, pName);
            Console.WriteLine("Nome enviado: " + pName.name + "\n"); 
            #endregion

            #region Game Loop
            bool gameEnd = false;
            while(gameEnd == false)
            {
                Communication.WaitForServer(stream);

                boardState = JsonConvert.DeserializeObject<BoardState>(Communication.ReceiveFromServer(stream));
                Console.WriteLine("Recebida mensagem do servidor!");
                board.SetBoardState(boardState);
                player.color = boardState.who_moves;
                Console.WriteLine("Carregado tabuleiro do servidor.");

                if (boardState.winner == 0 && boardState.draw == false)
                {
                    Console.WriteLine("Enviando jogada.");
                    Communication.SendToServer(stream, player.GenerateMovement(board));
                    Console.WriteLine("Jogada enviada.");
                }
                else
                {
                    gameEnd = true;
                }
            }
            #endregion
        }
    }
}
