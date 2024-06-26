namespace ExcelToDatabase
{
    public partial class Form1
    {
        public class ItemFullInfoDTO
        {
            //TODO: separar + abstrair

            public string ItemGrade { get; set; }
            public long Id { get; set; }
            public string KRName { get; set; }
            public string ENName { get; set; }
            public string Icon { get; set; }//X 
            public string FileName { get; set; }//X 
            public string SkillCodeType { get; set; }//X
            public long SkillCode { get; set; }
            public string SkillCodeDescription { get; set; }//X
            public string Slot { get; set; }//X
            public string MinSkillBuffApply { get; set; }//X
            public string MaxSkillBuffApply { get; set; }//X
            public byte BuffApplyPercentage { get; set; }
            public string ItemClass { get; set; }//X
            public string ItemType { get; set; }//X
            public string ItemTypeComment { get; set; }//x
            public string ItemTypeDescription { get; set; }//x
            public long SubCategoryId { get; set; }//x
            public string SubCategoryName { get; set; }//x
            public long ExpMultiplier { get; set; }
            public string CodeValueApplication { get; set; }//x
            public string ItemTypeUnknown { get; set; }//x
            public int SellType { get; set; }//x 9 = not sell | others = sell
            public string SellItemGrade { get; set; }//x
            public byte ItemUseMode { get; set; }//x 0 = consumivel | 1 = equipavel
            public byte ItemUseSeries { get; set; }//x
            public byte ItemUseTimes { get; set; }//x
            public string ItemUseExplanation { get; set; }//x
            public short OverLap { get; set; }//x
            public byte TamerUseLevelMin { get; set; }//x
            public byte TamerUseLevelMax { get; set; }//x
            public byte PartnerUseLevelMin { get; set; }//x
            public byte PartnerUseLevelMax { get; set; }//x
            public byte ItemPossession { get; set; }//x
            public byte ItemUseDuplicatedType { get; set; }//x
            public byte ItemUseTarget { get; set; }//0 - no one; 1 tamer & digi; 2 - digi; 3 - tamer;
            public long ItemDurationInSeconds { get; set; }
            public byte ItemDropType { get; set; } //0 - normal; 1 - ao obter, some(quest); 2 - não dropa;
            public int ItemPriceInDigicore { get; set; }
            public int ItemEventExchangeId { get; set; }
            public int ItemEventExchangeCount { get; set; }
            public long ItemScanOrPurchasePrice { get; set; }
            public long ItemSellPrice { get; set; }
            public string ItemModelName { get; set; }//x
            public string ItemDropEffectDescription { get; set; }//x
            public string ItemDropLoop { get; set; }//x
            public string ItemDropOutline { get; set; }//x
            public string ItemSoundId { get; set; }//x
            public string ItemDescription { get; set; }
            public byte ItemBoundType { get; set; } //0 - Not bound; 1 - Bind when equipped; 2 - Bind ever
            public byte TamerLvConstraint { get; set; }//x
            public byte DigimonLvConstraint { get; set; }//x
            public int ItemDropQuest1Id { get; set; }//se tiver o id da quest, so dropa quando ela estiver ativa;
            public int ItemDropQuest2Id { get; set; }//se tiver o id da quest, so dropa quando ela estiver ativa;
            public int ItemDropQuest3Id { get; set; }//se tiver o id da quest, so dropa quando ela estiver ativa;
            public int ItemDropQuest4Id { get; set; }//se tiver o id da quest, so dropa quando após concluir a quest com esse id;
            public byte CashSkillSlots { get; set; }
            public byte ChipsetSlots { get; set; }
            public byte UseTimesType { get; set; } //0 - infinity; 1 - once; 2 - remove buff; 3 - remove buff and delete; 4 - timed buff and delete;
            public int UsageTimeInMinutes { get; set; }
            public byte TimeUsebased { get; set; }//x
            public byte AvailableInbattle { get; set; }
            public string CashSkillDisable { get; set; } //x
            public string UselessStrings { get; set; }//x
            public string UselessStrings2 { get; set; }//x
            public string UselessStrings3 { get; set; }//x
            public string UselessStrings4 { get; set; }//x
            public string UselessStrings5 { get; set; }//x
        }
        
        public sealed class SkillCodeDTO
        {
            public int Id { get; set; }
            public long SkillCode { get; set; }
            public string Comment { get; set; }
            public int SkillApply1 { get; set; }
            public int SkillApply1Attribute { get; set; } //A
            public int SkillApply1Value { get; set; } //B
            public int SkillApply1ExtraValue { get; set; } //C
            public int SkillApply2 { get; set; }
            public int SkillApply2Attribute { get; set; } //A
            public int SkillApply2Value { get; set; } //B
            public int SkillApply2ExtraValue { get; set; } //C
            public int SkillApply3 { get; set; }
            public int SkillApply3Attribute { get; set; } //A
            public int SkillApply3Value { get; set; } //B
            public int SkillApply3ExtraValue { get; set; } //C
        }
    }
}
