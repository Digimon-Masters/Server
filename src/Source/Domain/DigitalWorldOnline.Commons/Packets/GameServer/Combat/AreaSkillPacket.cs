﻿using DigitalWorldOnline.Commons.Models.Config;
using DigitalWorldOnline.Commons.Models.Summon;
using DigitalWorldOnline.Commons.Writers;

namespace DigitalWorldOnline.Commons.Packets.GameServer.Combat
{
    public class AreaSkillPacket : PacketWriter
    {
        private const int PacketNumber = 1116;

        /// <summary>
        /// Casts the partner area skill.
        /// </summary>
        /// <param name="attackerHandler">Partner handler</param>
        /// <param name="attackerHpRate">Partner HP Rate</param>
        /// <param name="targets">Skill affected mobs</param>
        /// <param name="skillSlot">Skill index</param>
        /// <param name="finalDamage">Skill damage</param>
        public AreaSkillPacket(int attackerHandler, byte attackerHpRate, List<MobConfigModel> targets, byte skillSlot, int finalDamage)
        {
            Type(PacketNumber);
            WriteInt(attackerHandler);
            WriteShort((short)targets.Count);
            WriteByte(0);
            WriteInt(skillSlot);
            WriteByte(attackerHpRate);

            foreach (var target in targets)
            {
                WriteInt(target.GeneralHandler);
                WriteByte(target.CurrentHpRate);
                WriteByte(4); //HP
                WriteInt(finalDamage * -1);
                WriteByte(byte.MaxValue); //Previous HP Rate. Verificar necessidade
            }

            var deadTargets = targets.Where(x => x.Dead);
            WriteShort((short)deadTargets.Count());

            foreach (var deadTarget in deadTargets)
            {
                WriteInt(deadTarget.GeneralHandler);
                WriteInt(finalDamage);
            }
        }
        public AreaSkillPacket(int attackerHandler, byte attackerHpRate, List<SummonMobModel> targets, byte skillSlot, int finalDamage)
        {
            Type(PacketNumber);
            WriteInt(attackerHandler);
            WriteShort((short)targets.Count);
            WriteByte(0);
            WriteInt(skillSlot);
            WriteByte(attackerHpRate);

            foreach (var target in targets)
            {
                WriteInt(target.GeneralHandler);
                WriteByte(target.CurrentHpRate);
                WriteByte(4); //HP
                WriteInt(finalDamage * -1);
                WriteByte(byte.MaxValue); //Previous HP Rate. Verificar necessidade
            }

            var deadTargets = targets.Where(x => x.Dead);
            WriteShort((short)deadTargets.Count());

            foreach (var deadTarget in deadTargets)
            {
                WriteInt(deadTarget.GeneralHandler);
                WriteInt(finalDamage);
            }
        }
    }
}