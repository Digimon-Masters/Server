﻿namespace DigitalWorldOnline.Commons.Models.Config
{ 
    public sealed partial class MobDebuffModel: Buff
    {
        /// <summary>
        /// Creates a new digimon buff.
        /// </summary>
        /// <param name="buffId">Buff id (client reference).</param>
        /// <param name="skillId">Skill id (client reference).</param>
        /// <param name="duration">Buff duration (in seconds).</param>
        public static MobDebuffModel Create(int buffId, int skillId, int TypeN = 0, int duration = 0, int Cooldown = 0)
        {
            var buff = new MobDebuffModel();
            buff.SetSkillId(skillId);
            buff.SetBuffId(buffId);
            buff.SetTypeN(TypeN);
            buff.SetCooldown(Cooldown);
            buff.IncreaseDuration(duration);
            return buff;
        }
    }
}