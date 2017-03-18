using System.IO;

namespace PopcornTimeRemote
{
    public class SendPacket : MemoryStream
    {
        public SendPacket() : base() { }

        protected void WriteS(string s)
        {
            WriteByte((byte)s.Length);
            for (int i = 0; i < s.Length; i++) WriteByte((byte)s[i]);
        }

        protected void WriteValue(object i)
        {
            WriteS(i.ToString());
        }
    }
}
