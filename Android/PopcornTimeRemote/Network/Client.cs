using System.Net;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace PopcornTimeRemote
{
    public class Client
    {
        public TcpClient client;
        private Thread clientThread;
        private NetworkStream stream;
        private IPEndPoint address;
        public bool liveConnection = false;

        public Client(string ip, int Port)
        {
            address = new IPEndPoint(IPAddress.Parse(ip), Port);
            client = new TcpClient();
            connect(address);
            stream = client.GetStream();
            clientThread = new Thread(new ThreadStart(handleServer));
            clientThread.Start();
        }

        private void connect(IPEndPoint serverEndPoint)
        {
            try
            {
                client.Connect(serverEndPoint);
                liveConnection = true;
            }
            catch (SocketException)
            {
                liveConnection = false;
            }
        }

        private void handleServer()
        {
            byte[] message = new byte[4096];
            int bytesRead;
            while (true)
            {
                bytesRead = 0;
                try
                {
                    bytesRead = stream.Read(message, 0, 4096);
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
                    case 0x12:
                        packet = new Response();
                        break;
                }
                packet.Write(message, 1, message.Length - 1);
                packet.Handle();
            }
            client.Close();
            stream.Dispose();
        }

        public void Send(SendPacket packet)
        {
            byte[] array = packet.ToArray();
            stream.Write(array, 0, array.Length);
            stream.Flush();
        }
    }
}
