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
    class Communication
    {
        public static void DoNothing()
        {
            return;
        }

        public static void WaitForServer(NetworkStream stream)
        {
            Console.WriteLine("Esperando mensagem do servidor...");
            while (stream.DataAvailable == false)
            {
                DoNothing();
            }
        }

        public static void SendToServer(NetworkStream stream, object information)
        {
            StreamWriter streamWriter = new StreamWriter(stream);
            string json = JsonConvert.SerializeObject(information, Formatting.Indented);
            Console.WriteLine("Mensagem enviada: " + json);
            streamWriter.Write(json);
            streamWriter.Flush();
        }

        public static string ReceiveFromServer(NetworkStream stream)
        {
            StreamReader streamReader = new StreamReader(stream);
            Console.WriteLine("Lendo mensagem...");
            byte[] data = new byte[512];
            stream.Read(data, 0, data.Length);
            char[] msg = new char[512];
            for (int i = 0; i < data.Length; i++)
            {
                if(data[i] > 0)
                msg[i] = (char)data[i];
            }
            string message = new string(msg);
            stream.Flush();
            Console.WriteLine("mensagem lida: " + message);

            return message;
        }

        public static NetworkStream Connect(string ip, int port, TcpClient client)
        {
            NetworkStream stream;

            Console.WriteLine("Conectando.....");

            try
            {
                client.Connect(ip, port);
            }
            catch (Exception e)
            {
                Console.WriteLine("Nao foi possivel conectar.\n");
                Console.WriteLine("O sistema mandou a mensagem: " + e.Message + "\n");
                Console.WriteLine("[STACK TRACE] " + e.StackTrace);

                return (NetworkStream)NetworkStream.Null;
            }

            Console.WriteLine("Conectado ao servidor com sucesso!");
            Console.WriteLine("IP: " + ip);
            Console.WriteLine("Porta: " + port + "\n");
            stream = client.GetStream();
            return stream;

        }
    }
}
