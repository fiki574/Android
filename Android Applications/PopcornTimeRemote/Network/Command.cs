namespace PopcornTimeRemote
{
    public class Command : SendPacket
    {
        public Command(string command)
        {
            WriteByte(0x11);
            WriteS(command);
        }
    }
}