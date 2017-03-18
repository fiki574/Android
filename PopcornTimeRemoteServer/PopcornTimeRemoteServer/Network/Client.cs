using System.Net.Sockets;

namespace PopcornTimeRemoteServer
{
    public class Client
    {
        public TcpClient tcp;

        public Client(TcpClient client)
        {
            tcp = client;
        }

        public void Send(SendPacket packet)
        {
            byte[] array = packet.ToArray();
            tcp.GetStream().Write(array, 0, array.Length);
            tcp.GetStream().Flush();
        }
    }
}
