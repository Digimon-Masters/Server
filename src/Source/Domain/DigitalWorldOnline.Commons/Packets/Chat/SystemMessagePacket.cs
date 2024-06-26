using DigitalWorldOnline.Commons.Writers;

namespace DigitalWorldOnline.Commons.Packets.Chat
{
    public class SystemMessagePacket : PacketWriter
    {
        private const int PacketNumber = 1047;

        /// <summary>
        /// Sends the message to the general chat with SYSTEM prefix.
        /// </summary>
        /// <param name="message">The message to send.</param>
        public SystemMessagePacket(string message)
        {
            Type(PacketNumber);
            WriteByte(1);
            WriteString($"SYSTEM: {message}");
        }
    }
}