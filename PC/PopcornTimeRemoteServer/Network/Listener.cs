using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace PopcornTimeRemoteServer
{
    public class Listener
    {
        private TcpListener tcpListener;
        private Thread listenThread;
        private string IP;
        private int Port;
        private List<Client> _clients = new List<Client>();

        public Listener(string ip, int port)
        {
            IPAddress address = IPAddress.Any;
            Console.WriteLine($"Starting listener at {ip}:{port} <-- use this IP to connect through Android application");
            try
            {
                address = IPAddress.Parse(ip);
            }
            catch
            {
                throw new Exception("Failed to start listener on chosen address");
            }
            IP = address.ToString();
            Port = port;
            tcpListener = new TcpListener(address, port);
            listenThread = new Thread(new ThreadStart(listenForClients));
            listenThread.Start();
        }

        private void listenForClients()
        {
            try
            {
                tcpListener.Start();
                Console.WriteLine($"Listener started");
            }
            catch (SocketException)
            {
                listenForClients();
            }
            while (true)
            {
                TcpClient client = tcpListener.AcceptTcpClient();
                Thread clientThread = new Thread(new ParameterizedThreadStart(handleClient));
                clientThread.Start(client);
            }
        }

        private void handleClient(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            Client cclient = new Client(tcpClient);
            _clients.Add(cclient);
            Console.WriteLine($"Client connected");
            NetworkStream clientStream = tcpClient.GetStream();
            byte[] message = new byte[4096];
            int bytesRead;
            while (true)
            {
                bytesRead = 0;
                try
                {
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    break;
                }
                if (bytesRead == 0)
                {
                    break;
                }

                IPacket packet = null;
                switch(message[0])
                {
                    case 0x11:
                        packet = new Command();
                        break;
                }
                packet.Write(message, 1, message.Length - 1);
                packet.Handle(cclient);
            }
            lock (_clients)
            {
                _clients.Remove(cclient);
                Console.WriteLine($"Client disconnected");
            }
            tcpClient.Close();
        }
    }
}
