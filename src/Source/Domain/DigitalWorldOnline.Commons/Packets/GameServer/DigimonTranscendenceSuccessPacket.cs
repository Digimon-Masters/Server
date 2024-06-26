using DigitalWorldOnline.Commons.Enums;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Models.Base;
using DigitalWorldOnline.Commons.Writers;

namespace DigitalWorldOnline.Commons.Packets.GameServer
{
    public class DigimonTranscendenceSuccessPacket : PacketWriter
    {
        private const int PacketNumber = 16040;


        public DigimonTranscendenceSuccessPacket(int Result, byte targetSlot, DigimonHatchGradeEnum scale, int tamerMoney)
        {
            Type(PacketNumber);
            WriteInt(Result);
            WriteByte(targetSlot);
            WriteByte((byte)scale);
            WriteInt((int)50000);
            WriteInt(0);
            WriteInt(tamerMoney);
            WriteInt(0);
            WriteInt64(0);
        }
    }
}