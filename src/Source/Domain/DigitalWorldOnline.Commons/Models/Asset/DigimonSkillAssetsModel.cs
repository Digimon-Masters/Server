namespace DigitalWorldOnline.Commons.Models.Asset
{
    public sealed class DigimonSkillAssetModel
    {
        /// <summary>
        /// Unique sequential identifier
        /// </summary>
        public long Id { get; private set; }

        /// <summary>
        /// Digimon type/model
        /// </summary>
        public int Type { get; private set; }

        /// <summary>
        /// Skill slot (1=F1, 2=F2... x=Fx)
        /// </summary>
        public byte Slot { get; private set; }

        /// <summary>
        /// Skill id.
        /// </summary>
        public int SkillId { get; set; }

        /// <summary>
        /// Detailed skill information.
        /// </summary>
        public SkillInfoAssetModel SkillInfo { get; private set; }

        //TODO: Behavior
        public void SetSkillInfo(SkillInfoAssetModel skillInfo) => SkillInfo ??= skillInfo;

        public int TimeForCrowdControl()
        {
            switch (SkillId)
            {
                case 7571431:
                case 7111031:
                    return 3;

                case 7110731:
                    return 4;
            }

            return 0;
        }
        }
}