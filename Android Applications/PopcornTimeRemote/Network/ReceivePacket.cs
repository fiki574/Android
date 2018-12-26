using System;
using System.IO;
using System.Text;

namespace PopcornTimeRemote
{
    public class ReceivePacket : MemoryStream
    {
        public ReceivePacket() : base() { }

        protected string ReadS()
        {
            int length = ReadByte();
            if (length > Length - Position) return "";
            byte[] str = new byte[length];
            Read(str, 0, length);
            return Encoding.ASCII.GetString(str);
        }

        protected object ReadValue(Type t)
        {
            if (t == typeof(short)) return Convert.ToInt16(ReadS());
            else if (t == typeof(int)) return Convert.ToInt32(ReadS());
            else if (t == typeof(long)) return Convert.ToInt64(ReadS());
            else if (t == typeof(double)) return Convert.ToDouble(ReadS());
            else if (t == typeof(bool)) return Convert.ToBoolean(ReadS());
            else if (t == typeof(byte)) return Convert.ToByte(ReadS());
            else if (t == typeof(char)) return Convert.ToChar(ReadS());
            else if (t == typeof(decimal)) return Convert.ToDecimal(ReadS());
            else if (t == typeof(ushort)) return Convert.ToUInt16(ReadS());
            else if (t == typeof(uint)) return Convert.ToUInt32(ReadS());
            else if (t == typeof(ulong)) return Convert.ToUInt64(ReadS());
            else return null;
        }
    }
}
