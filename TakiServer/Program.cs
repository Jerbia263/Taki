using System;
using System.Net;
using System.Net.Sockets;

namespace TakiServer
{
    class Program
    {
        const int portNo = 1500;
        //private const string ipAddress = "127.0.0.1";

        static void Main(string[] args)
        {
            //System.Net.IPAddress localAdd = System.Net.IPAddress.Parse(ipAddress);

            TcpListener listener = new TcpListener(IPAddress.Any, portNo);

            Console.WriteLine("Simple XO Server");
            //Console.WriteLine("Listening to ip {0} port: {1}", ipAddress, portNo);
            Console.WriteLine("Listening to port: {0}", portNo.ToString());
            Console.WriteLine("Server is ready.");

            // Start listen to incoming connection requests            
            listener.Start();

            ServerManager serverManager = new ServerManager();

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Player newPlayer = new Player(client, serverManager);
                serverManager.Addplayer(newPlayer);
                
            }
        }
    }
}
