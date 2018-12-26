namespace PopcornTimeRemote
{
    public interface IPacket
    {
        void Write(byte[] buffer, int offset, int count);
        void Handle();
    }
}
