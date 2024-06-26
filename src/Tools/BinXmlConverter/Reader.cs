using System.Text;

namespace DSO.BinXmlConverter
{
    public class BitReader : IDisposable
    {
        private BinaryReader m_stream = null;

        public BitReader(Stream stream)
        {
            m_stream = new BinaryReader(stream);
        }

        public BinaryReader InnerStream
        {
            get { return m_stream; }
        }

        public void Skip(int bytes)
        {
            m_stream.BaseStream.Seek(bytes, SeekOrigin.Current);
        }

        public void Seek(int bytes)
        {
            m_stream.BaseStream.Seek(bytes, SeekOrigin.Begin);
        }

        public int ReadInt()
        {
            return BitConverter.ToInt32(m_stream.ReadBytes(4), 0);
        }

        public short ReadShort()
        {
            return BitConverter.ToInt16(m_stream.ReadBytes(2), 0);
        }

        public long ReadLong()
        {
            return BitConverter.ToInt64(m_stream.ReadBytes(8), 0);
        }

        public uint ReadUInt()
        {
            return BitConverter.ToUInt32(m_stream.ReadBytes(4), 0);
        }

        public ushort ReadUShort()
        {
            return BitConverter.ToUInt16(m_stream.ReadBytes(2), 0);
        }

        public ulong ReadUInt64()
        {
            return BitConverter.ToUInt64(m_stream.ReadBytes(8), 0);
        }

        public float ReadFloat()
        {
            return BitConverter.ToSingle(m_stream.ReadBytes(4), 0);
        }

        public double ReadDouble()
        {
            return BitConverter.ToDouble(m_stream.ReadBytes(8), 0);
        }

        public byte ReadByte()
        {
            return m_stream.ReadByte();
        }

        public string ReadString(int sizeLen, bool isNullTerminated)
        {
            int len = 0;
            if (sizeLen == 1)
                len = ReadByte();
            else if (sizeLen == 2)
                len = ReadShort();
            else if (sizeLen == 4)
                len = ReadInt();
            byte[] buffer = m_stream.ReadBytes(len + (isNullTerminated ? 1 : 0));
            return Encoding.ASCII.GetString(buffer).Replace("\0", "");
        }

        public string ReadString(int sizeLen, bool isNullTerminated, Encoding encoding)
        {
            int len = 0;
            if (sizeLen == 1)
                len = ReadByte();
            else if (sizeLen == 2)
                len = ReadShort();
            else if (sizeLen == 4)
                len = ReadInt();
            if (encoding is UnicodeEncoding) len *= 2;
            byte[] buffer = m_stream.ReadBytes(len + (isNullTerminated ? 1 : 0));
            return encoding.GetString(buffer).Replace("\0", "");
        }

        public string ReadZString(Encoding encoding)
        {
            int nullsEnc = 0;
            int nullLimit = 1;
            if (encoding is UnicodeEncoding) nullLimit *= 2;
            List<byte> buffer = new List<byte>();
            while (nullsEnc < nullLimit)
            {
                byte b = m_stream.ReadByte();
                if (b == 0)
                    nullsEnc++;
                else
                    nullsEnc = 0;
                if (nullsEnc > nullLimit) break;
                buffer.Add(b);
            }
            if (buffer.Count % 2 != 0) buffer.Add(0);
            return encoding.GetString(buffer.ToArray()).Replace("\0", "");
        }

        public string ReadZString(Encoding encoding, int size)
        {
            int nullsEnc = 0;
            int nullLimit = 1;
            long Position = m_stream.BaseStream.Position;
            if (encoding is UnicodeEncoding) nullLimit = 3;
            List<byte> buffer = new List<byte>();
            while (nullsEnc < nullLimit)
            {
                byte b = m_stream.ReadByte();
                if (b == 0)
                    nullsEnc++;
                else
                    nullsEnc = 0;
                buffer.Add(b);
            }
            if (m_stream.BaseStream.Position - Position < size)
                m_stream.ReadBytes((int)(size - (m_stream.BaseStream.Position - Position)));
            if (buffer.Count % 2 != 0) buffer.Add(0);
            return encoding.GetString(buffer.ToArray()).Replace("\0", "");
        }


        public void Dispose()
        {
            m_stream.Dispose();
        }
    }
}
