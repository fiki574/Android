namespace PopcornTimeRemoteServer
{
    public class Command : ReceivePacket, IPacket
    {
        public void Handle(Client client)
        {
            Position = 0;
            string command = ReadS();
            System.Console.WriteLine("Received Command packet: " + command);
            try
            {
                if (command == "pause")
                    Program.SendKeystroke((ushort)System.Windows.Forms.Keys.Space);
                else if (command == "screen")
                    Program.SendKeystroke((ushort)System.Windows.Forms.Keys.Enter);
                else if (command == "connect")
                    client.Send(new Response("con_success"));
                else
                    client.Send(new Response("unk_cmd"));
            }
            catch(System.Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
                client.Send(new Response("exception"));
            }
        }
    }
}
