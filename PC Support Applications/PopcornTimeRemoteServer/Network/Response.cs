namespace PopcornTimeRemoteServer
{
    public class Response : SendPacket
    {
        public Response(string response)
        {
            WriteByte(0x12);
            WriteS(response);
        }
    }
}
