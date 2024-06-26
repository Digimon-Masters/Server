using DigitalWorldOnline.Commons.Models.Asset;
using DigitalWorldOnline.Commons.Writers;

namespace DigitalWorldOnline.Commons.Packets.GameServer
{
    public class AddBuffPacket : PacketWriter
    {
        private const int PacketNumber = 4000;

        /// <summary>
        /// Applys a new buff to the target.
        /// </summary>
        /// <param name="handler">Target handler</param>
        /// <param name="buff">Target buff info</param>
        /// <param name="duration">Buff duration</param>
        public AddBuffPacket(int handler, BuffInfoAssetModel buff, short TypeN, int duration)
        {
            Type(PacketNumber);
            WriteUInt((uint)handler);
            WriteShort((short)buff.BuffId);
            WriteShort(TypeN);
            WriteInt(duration);
            WriteInt(buff.SkillCode);
        }
        public AddBuffPacket(int handler, BuffInfoAssetModel buff, short TypeN, uint duration)
        {
            Type(PacketNumber);
            WriteUInt((uint)handler);
            WriteShort((short)buff.BuffId);
            WriteShort(TypeN);
            WriteUInt(duration);
            WriteInt(buff.SkillCode);
        }
        public AddBuffPacket(int handler, int BuffId, int SkillCode, short TypeN, int duration)
        {
            Type(PacketNumber);
            WriteUInt((uint)handler);
            WriteShort((short)BuffId);
            WriteShort(TypeN);
            WriteInt(duration);
            WriteInt(SkillCode);
        }
        public class AddStunDebuffPacket : PacketWriter
        {
            private const int PacketNumber = 4013;
            public AddStunDebuffPacket(int handler, int BuffId, int SkillCode, int duration)
            {
                Type(PacketNumber);
                WriteUInt((uint)handler);
                WriteInt(SkillCode);
                WriteShort((short)BuffId);
                WriteInt(duration);

            }
        }
    }
}