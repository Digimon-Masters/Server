using System.Globalization;
using System.Xml.Serialization;
using static DataImporterTool.Form1;

namespace DataImporterTool
{
    public partial class Form1 : Form
    {
        #region Quest
        [XmlRoot("QuestInfos")]
        public class QuestInfosList
        {
            [XmlElement("QuestInfo")]
            public List<QuestInfo> Quests { get; set; } = new();
        }

        public class QuestInfo
        {
            [XmlElement("UniqID")]
            public int QuestId { get; set; }

            [XmlElement("Type")]
            public int QuestType { get; set; }

            [XmlElement("Target")]
            public int Target { get; set; }

            [XmlElement("TargetValue")]
            public int TargetValue { get; set; }

            [XmlElement("QuestItems")]
            public QuestItems QuestItems { get; set; }

            [XmlElement("QuestConditions")]
            public QuestConditions QuestConditions { get; set; }

            [XmlElement("QuestGoals")]
            public QuestGoals QuestGoals { get; set; }

            [XmlElement("RewardQuantities")]
            public QuestRewards QuestRewards { get; set; }

            [XmlElement("Event")]
            public QuestEvents QuestEvents { get; set; }
        }

        public class QuestEvents
        {
            [XmlElement("EventId")]
            public List<int> EventIds { get; set; }
        }

        public class QuestRewards
        {
            [XmlElement("RewardQuantity")]
            public List<QuestReward> QuestReward { get; set; }
        }

        public class QuestReward
        {
            [XmlElement("Reward")]
            public int Reward { get; set; }

            [XmlElement("RewardType")]
            public int RewardType { get; set; }

            [XmlElement("QuestRewardMoney")]
            public QuestRewardMoney QuestRewardMoney { get; set; }

            [XmlElement("QuestRewardItems")]
            public QuestRewardItems QuestRewardItems { get; set; }
        }

        public class QuestRewardItems
        {
            [XmlElement("QuestRewardItemsItem")]
            public List<QuestRewardItemsItem> QuestRewardItemsItems { get; set; }
        }

        public class QuestRewardItemsItem
        {
            [XmlElement("RewardItem")]
            public int ItemIdOrExp { get; set; }

            [XmlElement("RewardAmount")]
            public int Amount { get; set; }
        }

        public class QuestRewardMoney
        {
            [XmlElement("QuestRewardMoneyItem")]
            public List<QuestRewardMoneyItem> QuestRewardMoneyItems { get; set; }
        }

        public class QuestRewardMoneyItem
        {
            [XmlElement("RewardMoney")]
            public int Amount { get; set; }
        }

        public class QuestGoals
        {
            [XmlElement("QuestGoal")]
            public List<QuestGoal> QuestGoal { get; set; }
        }

        public class QuestGoal
        {
            [XmlElement("GoalType")]
            public int GoalType { get; set; }

            [XmlElement("GoalId")]
            public int GoalId { get; set; }

            [XmlElement("goalAmount")]
            public int GoalAmount { get; set; }

            [XmlElement("CurTypeCount")]
            public int CurTypeCount { get; set; }

            [XmlElement("SubValue")]
            public int SubValue { get; set; }

            [XmlElement("SubValue1")]
            public int SubValueTwo { get; set; }
        }

        public class QuestConditions
        {
            [XmlElement("QuestCondition")]
            public List<QuestCondition> QuestCondition { get; set; }
        }

        public class QuestCondition
        {
            [XmlElement("ConditionType")]
            public int ConditionType { get; set; }

            [XmlElement("ConditionId")]
            public int ConditionId { get; set; }

            [XmlElement("ConditionCount")]
            public int ConditionCount { get; set; }
        }

        public class QuestItems
        {
            [XmlElement("QuestItem")]
            public List<QuestItem> QuestItem { get; set; }
        }

        public class QuestItem
        {
            [XmlElement("ItemgivenType")]
            public int ItemType { get; set; }

            [XmlElement("ItemgivenAmount")]
            public int Amount { get; set; }
        }
        #endregion

        #region DigimonEvo
        [XmlRoot("DigimonList")]
        public class DigimonEvoList
        {
            [XmlElement("Digimon")]
            public List<DigimonEvoObject> DigimonEvos { get; set; } = new();
        }

        public class DigimonEvoObject
        {
            [XmlElement("digiId")]
            public int Type { get; set; }

            [XmlElement("BattleType")]
            public int EvolutionRank { get; set; }

            [XmlElement("Evolution")]
            public List<DigimonEvoLine> EvolutionLines { get; set; }
        }

        public class DigimonEvoLine
        {
            [XmlElement("digiId")]
            public int Type { get; set; }

            [XmlElement("EvolutionType")]
            public List<DigimonEvoStage> EvolutionStages { get; set; }

            [XmlElement("Level")]
            public byte SlotLevel { get; set; }

            [XmlElement("m_nOpenLevel")]
            public byte UnlockLevel { get; set; }

            [XmlElement("m_nOpenQuest")]
            public short UnlockQuestId { get; set; }

            [XmlElement("m_nOpenItemTypeS")]
            public int UnlockItemSection { get; set; }

            [XmlElement("m_nOpenItemNum")]
            public int UnlockItemSectionAmount { get; set; }
        }

        public class DigimonEvoStage
        {
            [XmlElement("nSlot")]
            public int Value { get; set; }

            [XmlElement("dwDigimonID")]
            public int Type { get; set; }
        }
        #endregion

        #region Seal
        [XmlRoot("MasterCards")]
        public class SealList
        {
            [XmlElement("Card")]
            public List<SealObject> Seals { get; set; } = new();
        }

        public class SealObject
        {
            [XmlElement("s_nScale")]
            public int Id { get; set; }

            [XmlElement("s_nID")]
            public short Sequential { get; set; }

            [XmlElement("GradeInfo")]
            public SealStatusObject SealStatusObject { get; set; }
        }

        public class SealStatusObject
        {
            [XmlElement("GradeInfoItem")]
            public List<SealStatus> SealStatusList { get; set; }
        }

        public class SealStatus
        {
            [XmlElement("s_nEff1")]
            public int Type { get; set; }

            [XmlElement("s_nEff1val")]
            public int Amount { get; set; }
        }

        #endregion

        #region DigimonBase
        [XmlRoot("DigimonList")]
        public class DigimonBaseInfoList
        {
            [XmlElement("Digimon")]
            public List<DigimonBaseInfoObject> DigimonBaseList { get; set; } = new();
        }

        public class DigimonBaseInfoObject
        {
            [XmlElement("Species")]
            public int Type { get; set; }

            [XmlElement("Model")]
            public int Model { get; set; }

            [XmlElement("BaseLevel")]
            public byte Level { get; set; }

            [XmlElement("DisplayName")]
            public string Name { get; set; }

            [XmlElement("DigimonType")]
            public byte ScaleType { get; set; }

            [XmlElement("AttributeType")]
            public int Attribute { get; set; }

            [XmlElement("NatureType")]
            public int Element { get; set; }

            [XmlElement("Family1")]
            public int Family1 { get; set; }

            [XmlElement("Family2")]
            public int Family2 { get; set; }

            [XmlElement("Family3")]
            public int Family3 { get; set; }

            [XmlElement("s_nBaseStat_AS")]
            public int ASValue { get; set; }

            [XmlElement("s_nBaseStat_AR")]
            public int ARValue { get; set; }

            [XmlElement("s_nBaseStat_AT")]
            public int ATValue { get; set; }

            public int BLValue { get; set; }

            [XmlElement("s_nBaseStat_CR")]
            public int CTValue { get; set; }

            [XmlElement("s_nBaseStat_DE")]
            public int DEValue { get; set; }

            [XmlElement("s_nBaseStat_DS")]
            public int DSValue { get; set; }

            [XmlElement("s_nBaseStat_EV")]
            public int EVValue { get; set; }

            [XmlElement("s_nBaseStat_HP")]
            public int HPValue { get; set; }

            [XmlElement("s_nBaseStat_HT")]
            public int HTValue { get; set; }

            [XmlElement("s_nBaseStat_MS")]
            public int MSValue { get; set; }

            [XmlElement("WakkLen")]
            public int WSValue { get; set; }

            [XmlElement("EvolutionType")]
            public int EvolutionType { get; set; }

            [XmlElement("Skill1_ID")]
            public int Skill1 { get; set; }

            [XmlElement("Skill2_ID")]
            public int Skill2 { get; set; }

            [XmlElement("Skill3_ID")]
            public int Skill3 { get; set; }

            [XmlElement("Skill4_ID")]
            public int Skill4 { get; set; }
        }
        #endregion

        #region MonsterBase
        [XmlRoot("Monsters")]
        public class MonsterBaseInfoList
        {
            [XmlElement("Monster")]
            public List<MonsterBaseInfoObject> MonsterBaseList { get; set; } = new();
        }

        public class MonsterBaseInfoObject
        {
            [XmlElement("MonsterID")]
            public int Type { get; set; }

            [XmlElement("ModelDigimon")]
            public int Model { get; set; }

            [XmlElement("Level")]
            public byte Level { get; set; }

            [XmlElement("Name")]
            public string Name { get; set; }

            [XmlElement("Sight")]
            public int ViewRange { get; set; }

            [XmlElement("HuntRange")]
            public int HuntRange { get; set; }

            [XmlElement("Battle")]
            public int ReactionType { get; set; }

            [XmlElement("AttributeType")]
            public int Attribute { get; set; } = 1;

            [XmlElement("NatureType")]
            public int Element { get; set; }

            [XmlElement("Family1")]
            public int Family1 { get; set; }

            [XmlElement("Family2")]
            public int Family2 { get; set; }

            [XmlElement("Family3")]
            public int Family3 { get; set; }

            [XmlElement("AS")]
            public int ASValue { get; set; }

            [XmlElement("AR")]
            public int ARValue { get; set; }

            [XmlElement("AT")]
            public int ATValue { get; set; }

            public int BLValue { get; set; }

            [XmlElement("CT")]
            public int CTValue { get; set; }

            [XmlElement("DE")]
            public int DEValue { get; set; }

            [XmlElement("DS")]
            public int DSValue { get; set; }

            [XmlElement("EV")]
            public int EVValue { get; set; }

            [XmlElement("HP")]
            public int HPValue { get; set; }

            [XmlElement("HT")]
            public int HTValue { get; set; }

            [XmlElement("MS")]
            public int MSValue { get; set; }

            public int WSValue { get; set; } = 450;

            [XmlElement("Class")]
            public int Class { get; set; }
        }
        #endregion

        #region MonsterSkill
        [XmlRoot("MonsterSkills")]
        public class MonsterSkillList
        {
            [XmlElement("MonsterSkill")]
            public List<MonsterSkillObject> MonsterSkillObjectList { get; set; } = new();
        }

        public class MonsterSkillObject
        {
            [XmlElement("Skill_IDX")]
            public int SkillId { get; set; }

            [XmlElement("MonsterID")]
            public int Model { get; set; }

            [XmlElement("CoolTime")]
            public int SkillCooldown { get; set; }

            [XmlElement("Target_MinCnt")]
            public int TargetCount { get; set; }
            //TODO: falta só area e dano (maioria do dano é 0 mesmo). Para a area, fazer join no SkillTerms

            [XmlElement("Skill_Type")]
            public short SkillType { get; set; }
        }
        #endregion

        #region Item
        [XmlRoot("ITEM")]
        public class ItemInfoList
        {
            [XmlElement("index")]
            public ItemInfoObject ItemInfo { get; set; } = new();
        }

        public class ItemInfoObject
        {
            [XmlElement("sINFO")]
            public List<ItemInfo> ItemList { get; set; } = new();
        }

        public class ItemInfo
        {
            [XmlElement("s_dwItemID")]
            public int ItemId { get; set; }

            [XmlElement("s_szName")]
            public string Name { get; set; }

            [XmlElement("s_nClass")]
            public int Class { get; set; }

            [XmlElement("s_nType_L")]
            public int Type { get; set; }

            [XmlElement("s_nTypeValue")]
            public int TypeN { get; set; }

            [XmlElement("s_btApplyRateMin")]
            public short ApplyValueMin { get; set; }

            [XmlElement("s_btApplyRateMax")]
            public short ApplyValueMax { get; set; }

            [XmlElement("s_btApplyElement")]
            public short ApplyElement { get; set; }


            [XmlElement("s_nSection")]
            public int Section { get; set; }

            [XmlElement("s_nSellType")]
            public int SellType { get; set; }

            [XmlElement("s_nBelonging")]
            public int BoundType { get; set; }

            [XmlElement("s_nType_S")]
            public int UseTimeType { get; set; }

            [XmlElement("s_nUseCharacter")]
            public int Target { get; set; }

            [XmlElement("s_dwSkill")]
            public long SkillCode { get; set; }

            [XmlElement("s_nTamerReqMinLevel")]
            public byte TamerMinLevel { get; set; }

            [XmlElement("s_nTamerReqMaxLevel")]
            public byte TamerMaxLevel { get; set; }

            [XmlElement("s_nDigimonReqMinLevel")]
            public byte DigimonMinLevel { get; set; }

            [XmlElement("s_nDigimonReqMaxLevel")]
            public byte DigimonMaxLevel { get; set; }

            [XmlElement("s_dwSale")]
            public long SellPrice { get; set; }

            [XmlElement("s_dwScanPrice")]
            public int ScanPrice { get; set; }

            [XmlElement("s_dwDigiCorePrice")]
            public int DigicorePrice { get; set; }

            [XmlElement("s_nEventItemType")]
            public int EventItemId { get; set; }

            [XmlElement("s_dwEventItemPrice")]
            public int EventItemAmount { get; set; }

            [XmlElement("s_nUseTime_Min")]
            public int UsageTimeMinutes { get; set; }

            [XmlElement("s_nOverlap")]
            public int Overlap { get; set; }
        }
        #endregion

        #region Hatch
        [XmlRoot("TDBTactics")]
        public class TacticsInfoList
        {
            [XmlElement("TDBTactic")]
            public List<TacticsInfoObject> HatchList { get; set; } = new();
        }

        public class TacticsInfoObject
        {
            [XmlElement("ItemId")]
            public int ItemId { get; set; }

            [XmlElement("s_nDigimonID")]
            public int HatchType { get; set; }

            [XmlElement("s_nReqItemS_Type")]
            public int LowClassDataSection { get; set; }

            [XmlElement("s_nReqItemS_Type1")]
            public int MidClassDataSection { get; set; }

            [XmlElement("s_nReqItemCount")]
            public int LowClassDataAmount { get; set; }

            [XmlElement("s_nReqItemCount1")]
            public int MidClassDataAmount { get; set; }

            [XmlElement("s_nViewWarning")]
            public int LowClassBreakPoint { get; set; }

            [XmlElement("s_nViewWarning1")]
            public int MidClassBreakPoint { get; set; }
        }
        #endregion

        #region Maps
        [XmlRoot("MapData")]
        public class MapInfoList
        {
            [XmlElement("Map")]
            public List<MapInfoObject> MapList { get; set; } = new();
        }

        public class MapInfoObject
        {
            [XmlElement("MapID")]
            public int MapId { get; set; }

            [XmlElement("MapDescription_Eng")]
            public string Name { get; set; }

            [XmlElement("MapRegionID")]
            public byte Region { get; set; }
        }
        #endregion

        #region Clone
        [XmlRoot("TranscenderList")]
        public class CloneInfoList
        {
            [XmlElement("Transcender")]
            public List<CloneInfoObject> CloneList { get; set; } = new();
        }

        public class CloneInfoObject
        {
            [XmlElement("dwItemCode")]
            public int ItemSection { get; set; }

            [XmlElement("nLowLevel")]
            public byte MinLevel { get; set; }

            [XmlElement("nHighLevel")]
            public byte MaxLevel { get; set; }

            [XmlElement("dwNeedMoney")]
            public long Bits { get; set; }
        }
        #endregion

        #region Clone Value
        [XmlRoot("EnchantRankInfos")]
        public class CloneValueList
        {
            [XmlElement("EnchantRankInfo")]
            public List<CloneValueObject> CloneList { get; set; } = new();
        }

        public class CloneValueObject
        {
            [XmlElement("nStatIdx")]
            public int Type { get; set; }

            [XmlElement("enchantValueInfos")]
            public CloneSubValueObject SubValue { get; set; }
        }

        public class CloneSubValueObject
        {
            [XmlElement("EnchantValueInfo")]
            public List<CloneSubValueObjects> List { get; set; } = new();
        }

        public class CloneSubValueObjects
        {
            [XmlElement("nLowEnchantLv")]
            public byte MinLevel { get; set; }

            [XmlElement("nHighEnchantLv")]
            public byte MaxLevel { get; set; }

            [XmlElement("nNormalEnchantMinValue")]
            public int MinValue { get; set; }

            [XmlElement("nNormalEnchantMaxValue")]
            public int MaxValue { get; set; }
        }
        #endregion

        [XmlRoot("AccessoryList")]
        public class AccessoryList
        {
            [XmlElement("Accessory")]
            public List<Accessory> Accessories { get; set; } = new();
        }

        public class Accessory
        {
            [XmlAttribute("ItemId")]
            public int ItemId { get; set; }

            [XmlAttribute("Name")]
            public string Name { get; set; }

            [XmlElement("StatusList")]
            public StatusList StatusList { get; set; }
        }

        public class StatusList
        {
            [XmlAttribute("Amount")]
            public int Amount { get; set; }

            [XmlAttribute("MaxRoll")]
            public int MaxRoll { get; set; }

            [XmlElement("Status")]
            public List<Status> Statuses { get; set; } = new();
        }

        public class Status
        {
            [XmlAttribute("Type")]
            public int Type { get; set; }

            [XmlAttribute("Description")]
            public string Description { get; set; }

            [XmlElement("MinValue")]
            public int MinValue { get; set; }

            [XmlElement("MaxValue")]
            public int MaxValue { get; set; }
        }

        #region NPCs
        [XmlRoot("NPCs")]
        public class NpcInfoList
        {
            [XmlElement("NPC")]
            public List<NpcInfoObject> NpcList { get; set; } = new();
        }

        public class NpcInfoObject
        {
            [XmlElement("NpcID")]
            public int NpcId { get; set; }

            [XmlElement("MapID")]
            public int MapId { get; set; }

            [XmlElement("ItemIDs")]
            public NpcItemList NpcItemList { get; set; }
        }

        public class NpcItemList
        {
            [XmlElement("ItemID")]
            public List<int> ItemIdList { get; set; } = new();
        }
        #endregion

        #region Fruits
        [XmlRoot("FruitList")]
        public class FruitInfoList
        {
            [XmlElement("Fruit")]
            public List<FruitInfoObject> FruitList { get; set; } = new();
        }

        public class FruitInfoObject
        {
            [XmlAttribute("Id")]
            public int ItemId { get; set; }

            [XmlAttribute("Section")]
            public int ItemSection { get; set; }

            [XmlElement("IncreaseList")]
            public FruitIncreaseInfoObject IncreaseInfo { get; set; }
        }

        public class FruitIncreaseInfoObject
        {
            [XmlElement("Increase")]
            public List<FruitIncreaseList> IncreaseList { get; set; }
        }

        public class FruitIncreaseList
        {
            [XmlAttribute("Grade")]
            public int Grade { get; set; }

            [XmlElement("SizeChanceList")]
            public FruitSizeInfoObject SizeInfo { get; set; }
        }

        public class FruitSizeInfoObject
        {
            [XmlElement("SizeChance")]
            public List<FruitSize> SizeList { get; set; }
        }

        public class FruitSize
        {
            [XmlAttribute("Size")]
            public double Size { get; set; }

            [XmlText]
            public double Chance { get; set; }
        }
        #endregion

        #region SkillInfo
        [XmlRoot("SkillDataArray")]
        public class SkillInfoList
        {
            [XmlElement("SkillData")]
            public List<SkillInfoObject> SkillData { get; set; } = new();
        }

        public class SkillInfoObject
        {

            [XmlElement("s_dwID")]
            public int SkillId { get; set; }

            [XmlElement("s_szComment")]
            public string Comment { get; set; }

            [XmlElement("s_szName")]
            public string Name { get; set; }

            [XmlElement("s_nUseDS")]
            public int DSUsage { get; set; }

            [XmlElement("s_nUseHP")]
            public int HPUsage { get; set; }

            [XmlElement("s_fCastingTime")]
            public CustomCastingTime CastingTime { get; set; }

            [XmlElement("s_fCooldownTime")]
            public int Cooldown { get; set; }

            [XmlElement("s_nMaxLevel")]
            public byte MaxLevel { get; set; }

            [XmlElement("s_nLevelupPoint")]
            public byte RequiredPoints { get; set; }

            [XmlElement("s_nTarget")]
            public byte Target { get; set; }

            [XmlElement("s_nAttSphere")]
            public float AreaOfEffect { get; set; }

            [XmlElement("s_fAttRange_MinDmg")]
            public float AoEMinDamage { get; set; }

            [XmlElement("s_fAttRange_MaxDmg")]
            public float AoEMaxnDamage { get; set; }

            [XmlElement("s_fAttRange")]
            public int Range { get; set; }

            [XmlElement("s_nLimitLevel")]
            public byte UnlockLevel { get; set; }

            [XmlElement("s_nMemorySkill")]
            public int MemoryChips { get; set; }

            [XmlElement("s_nFamilyType")]
            public byte FamilyType { get; set; }

            [XmlElement("SkillApply")]
            public SkillApply skillApply { get; set; }

        }
        public class SkillInfoApply
        {
            [XmlElement("IncreaseApply")]
            public List<IncreaseInfoApply> IncreaseInfoApplies { get; set; }
        }

        public class IncreaseInfoApply
        {
            [XmlElement("s_nA")]
            public int S_nA { get; set; }

            [XmlElement("s_nInvoke_Rate")]
            public int S_nInvoke_Rate { get; set; }

            [XmlElement("s_nB")]
            public int S_nB { get; set; }

            [XmlElement("s_nC")]
            public int S_nC { get; set; }

            [XmlElement("s_nBuffCode")]
            public int S_nBuffCode { get; set; }

            [XmlElement("s_nID")]
            public int S_nID { get; set; }

            [XmlElement("s_nIncrease_B_Point")]
            public int S_nIncrease_B_Point { get; set; }
        }

        [XmlRoot("s_fCastingTime")]
        public class CustomCastingTime
        {
            [XmlText]
            public float Value { get; set; }

            public float ToFloat()
            {

                return Value;
            }
        }
        #endregion

        #region SkillCode
        [XmlRoot("SkillDataArray")]
        public class SkillCodeInfoList
        {
            [XmlElement("SkillData")]
            public List<SkillDataInfoObject> SkillData { get; set; } = new();
        }

        public class SkillDataInfoObject
        {

            [XmlElement("s_dwID")]
            public int SkillCode { get; set; }

            [XmlElement("s_szComment")]
            public string Comment { get; set; }


            [XmlElement("SkillApply")]
            public SkillApply skillApply { get; set; }

        }
        public class SkillApply
        {
            [XmlElement("IncreaseApply")]
            public List<IncreaseApply> IncreaseApplies { get; set; }
        }

        public class IncreaseApply
        {
            [XmlElement("s_nA")]
            public int S_nA { get; set; }

            [XmlElement("s_nInvoke_Rate")]
            public int S_nInvoke_Rate { get; set; }

            [XmlElement("s_nB")]
            public int S_nB { get; set; }

            [XmlElement("s_nC")]
            public int S_nC { get; set; }

            [XmlElement("s_nBuffCode")]
            public int S_nBuffCode { get; set; }

            [XmlElement("s_nID")]
            public int S_nID { get; set; }

            [XmlElement("s_nIncrease_B_Point")]
            public int S_nIncrease_B_Point { get; set; }
        }
        #endregion

        #region MonsterSkill
        [XmlRoot("MonsterSkills")]
        public class MonsterSkillsList
        {
            [XmlElement("MonsterSkill")]
            public List<MonsterSkillsInfoObject> SkillData { get; set; } = new();
        }

        public class MonsterSkillsInfoObject
        {
            [XmlElement("MonsterID")]
            public int Type { get; set; }

            [XmlElement("Skill_IDX")]
            public int SkillId { get; set; }

            [XmlElement("CoolTime")]
            public int Cooldown { get; set; }

            [XmlElement("CastTime")]
            public int CastingTime { get; set; }


            [XmlElement("Target_Cnt")]
            public byte TargetCount { get; set; }

            [XmlElement("Target_MinCnt")]
            public byte TargetMin { get; set; }

            [XmlElement("Target_MaxCnt")]
            public byte TargetMax { get; set; }

            [XmlElement("UseTerms")]
            public byte UseTerms { get; set; }

            [XmlElement("Skill_Type")]
            public int SkillType { get; set; }

            [XmlElement("Eff_Val_Min")]
            public int MinValue { get; set; }

            [XmlElement("Eff_Val_Max")]
            public int MaxValue { get; set; }

            [XmlElement("RangeIDX")]
            public int RangeId { get; set; }

            [XmlElement("Ani_Delay")]
            public float AnimationDelay { get; set; }

            [XmlElement("Activetype")]
            public byte ActiveType { get; set; }

            [XmlElement("NoticeTime")]
            public float NoticeTime { get; set; }

        }

        #endregion

        #region TamerSkill
        [XmlRoot("TamerSkillArray")]
        public class TamerSkillList
        {
            [XmlElement("TamerSkill")]
            public List<TamerSkillInfo> Skills { get; set; } = new List<TamerSkillInfo>();
        }

        public class TamerSkillInfo
        {
            [XmlElement("SkillId")]
            public int SkillId { get; set; }

            [XmlElement("SkillCode")]
            public int SkillCode { get; set; }

            [XmlElement("Duration")]
            public int Duration { get; set; }
        }
        #endregion

        [XmlRoot("BuffDataArray")]
        public class BuffDataList
        {
            [XmlElement("BuffData")]
            public List<BuffDataInfo> Buffs { get; set; } = new List<BuffDataInfo>();
        }
        public class BuffDataInfo
        {
            [XmlElement("s_dwID")]
            public int BuffId { get; set; }

            [XmlElement("s_szName")]
            public string Name { get; set; }

            [XmlElement("s_dwSkillCode")]
            public int SkillCode { get; set; }

            [XmlElement("s_nMinLv")]
            public int MinLevel { get; set; }

            [XmlElement("s_nConditionLv")]
            public int ConditionLevel { get; set; }

            [XmlElement("s_nBuffClass")]
            public short Class { get; set; }

            [XmlElement("s_nBuffType")]
            public int Type { get; set; }

            [XmlElement("s_nBuffLifeType")]
            public int LifeType { get; set; }

            [XmlElement("s_nBuffTimeType")]
            public int TimeType { get; set; }

            [XmlElement("s_dwDigimonSkillCode")]
            public int DigimonSkillCode { get; set; }
        }

        [XmlRoot("MonthlyEvents")]
        public class MonthlyEvents
        {
            [XmlElement("MonthlyEvent")]
            public List<MonthlyEvent> Events { get; set; } = new List<MonthlyEvent>();
        }

        public class MonthlyEvent
        {
            [XmlElement("s_nTableNo")]
            public int TableNumber { get; set; }

            [XmlElement("s_szMessage")]
            public string Message { get; set; }

            [XmlElement("MonthlyItems")]
            public MonthlyItems MonthlyItems { get; set; }
        }

        public class MonthlyItems
        {
            [XmlElement("MonthlyItem")]
            public List<MonthlyItem> Items { get; set; } = new List<MonthlyItem>();
        }

        public class MonthlyItem
        {
            [XmlElement("ItemId")]
            public int ItemId { get; set; }

            [XmlElement("ItemCount")]
            public int ItemCount { get; set; }
        }

        [XmlRoot("AchieveSINFOs")]
        public class AchieveSINFOs
        {
            [XmlElement("AchieveSINFO")]
            public List<AchieveSINFO> AchieveSINFO { get; set; } = new List<AchieveSINFO>();
        }

        public class AchieveSINFO
        {
            [XmlElement("s_nQuestID")]
            public int QuestID { get; set; }

            [XmlElement("s_nIcon")]
            public int Icon { get; set; }

            [XmlElement("s_nPoint")]
            public int Point { get; set; }

            [XmlElement("s_bDisplay")]
            public int Display { get; set; }

            [XmlElement("s_bDisplay2")]
            public int Display2 { get; set; }

            [XmlElement("s_szName")]
            public string Name { get; set; }

            [XmlElement("s_szComment")]
            public string Comment { get; set; }

            [XmlElement("s_szTitle")]
            public string Title { get; set; }

            [XmlElement("s_nGroup")]
            public int Group { get; set; }

            [XmlElement("s_nSubGroup")]
            public int SubGroup { get; set; }

            [XmlElement("s_nType")]
            public int Type { get; set; }

            [XmlElement("s_nBuffCode")]
            public int BuffCode { get; set; }
        }


        [XmlRoot("SummonList")]
        public class SummonList
        {
            [XmlElement("SummonDTO")]
            public List<SummonDTO> SummonDTOs { get; set; } = new List<SummonDTO>();
        }

        public class SummonDTO
        {
            [XmlElement("ItemId")]
            public int ItemId { get; set; }

            [XmlElement("Maps")]
            public MapsDTO Maps { get; set; }

            [XmlElement("SummonedMobs")]
            public List<SummonedMobsDTO> SummonedMobs { get; set; } = new List<SummonedMobsDTO>();
        }

        public class MapsDTO
        {
            [XmlElement("Map")]
            public List<int> Map { get; set; }
        }

        public class SummonedMobsDTO
        {
            [XmlElement("SummonMobDTO")]
            public List<SummonMobDTO> SummonMobs { get; set; } = new List<SummonMobDTO>();
        }

        public class SummonMobDTO
        {
            [XmlElement("Type")]
            public int Type { get; set; }

            [XmlElement("Duration")]
            public int Duration { get; set; }

            [XmlElement("Amount")]
            public int Amount { get; set; }

            [XmlElement("Model")]
            public int Model { get; set; }

            [XmlElement("Level")]
            public int Level { get; set; }

            [XmlElement("ViewRange")]
            public int ViewRange { get; set; }

            [XmlElement("HuntRange")]
            public int HuntRange { get; set; }

            [XmlElement("ReactionType")]
            public int ReactionType { get; set; }

            [XmlElement("Attribute")]
            public int Attribute { get; set; }

            [XmlElement("Class")]
            public int Class { get; set; }


            [XmlElement("Element")]
            public int Element { get; set; }

            [XmlElement("Family1")]
            public int Family1 { get; set; }

            [XmlElement("Family2")]
            public int Family2 { get; set; }

            [XmlElement("Family3")]
            public int Family3 { get; set; }

            [XmlElement("ASValue")]
            public int ASValue { get; set; }

            [XmlElement("ARValue")]
            public int ARValue { get; set; }

            [XmlElement("ATValue")]
            public int ATValue { get; set; }

            [XmlElement("BLValue")]
            public int BLValue { get; set; }

            [XmlElement("CTValue")]
            public int CTValue { get; set; }

            [XmlElement("DEValue")]
            public int DEValue { get; set; }

            [XmlElement("DSValue")]
            public int DSValue { get; set; }

            [XmlElement("EVValue")]
            public int EVValue { get; set; }

            [XmlElement("HPValue")]
            public int HPValue { get; set; }

            [XmlElement("HTValue")]
            public int HTValue { get; set; }

            [XmlElement("MSValue")]
            public int MSValue { get; set; }

            [XmlElement("WSValue")]
            public int WSValue { get; set; }

            [XmlElement("DropReward")]
            public DropRewardDTO DropReward { get; set; }

            [XmlElement("ExpReward")]
            public ExpRewardDTO ExpReward { get; set; }

            [XmlElement("Location")]
            public LocationDTO Location { get; set; }
        }

        public class DropRewardDTO
        {
            [XmlElement("MinAmount")]
            public int MinAmount { get; set; }

            [XmlElement("MaxAmount")]
            public int MaxAmount { get; set; }

            [XmlElement("Drops")]
            public DropsDTO Drops { get; set; }

            [XmlElement("BitsDrop")]
            public List<BitsDropDTO> BitsDrop { get; set; }
        }

        public class DropsDTO
        {
            [XmlElement("SummonMobItemDropDTO")]
            public List<SummonMobItemDropDTO> SummonMobItemDrop { get; set; }
        }

        public class SummonMobItemDropDTO
        {
            [XmlElement("ItemId")]
            public int ItemId { get; set; }

            [XmlElement("MinAmount")]
            public int MinAmount { get; set; }

            [XmlElement("MaxAmount")]
            public int MaxAmount { get; set; }

            [XmlElement("Chance")]
            public int Chance { get; set; }
        }

        public class BitsDropDTO
        {

            [XmlElement("MinAmount")]
            public int MinAmount { get; set; }

            [XmlElement("MaxAmount")]
            public int MaxAmount { get; set; }

            [XmlElement("Chance")]
            public int Chance { get; set; }
        }

        public class ExpRewardDTO
        {
            [XmlElement("TamerExperience")]
            public int TamerExperience { get; set; }

            [XmlElement("DigimonExperience")]
            public int DigimonExperience { get; set; }

            [XmlElement("NatureExperience")]
            public int NatureExperience { get; set; }

            [XmlElement("ElementExperience")]
            public int ElementExperience { get; set; }

            [XmlElement("SkillExperience")]
            public int SkillExperience { get; set; }
        }

        public class LocationDTO
        {
            [XmlElement("X")]
            public int X { get; set; }

            [XmlElement("Y")]
            public int Y { get; set; }
        }

        [XmlRoot("RewardList")]
        public class RewardList
        {
            [XmlElement("Reward")]
            public List<Reward> Rewards { get; set; } = new List<Reward>();
        }

        public class Reward
        {
            [XmlElement("nKey")]
            public int NKey { get; set; }

            [XmlElement("RewardInfo")]
            public List<RewardInfo> RewardInfos { get; set; } = new List<RewardInfo>();
        }

        public class RewardInfo
        {
            [XmlElement("ItemId")]
            public int ItemId { get; set; }

            [XmlElement("Amount")]
            public int Amount { get; set; }

            [XmlElement("Rewards")]
            public List<Rewards> RewardsList { get; set; } = new List<Rewards>();
        }

        public class Rewards
        {
            [XmlElement("ItemId")]
            public int ItemId { get; set; }

            [XmlElement("Amount")]
            public int Amount { get; set; }
        }


        [XmlRoot("ArmorList")]
        public class ArmorList
        {
            [XmlElement("Item")]
            public List<Item> Items { get; set; } = new List<Item>();
        }

        public class Item
        {
            [XmlElement("ItemId")]
            public int ItemId { get; set; }

            [XmlElement("RequiredAmount")]
            public int Amount { get; set; }

            [XmlElement("Chance")]
            public int Chance { get; set; }
        }

        public class NPCs
        {
            public string NPCTag;
            public string NPCName;
            public string NPCDesc;
            public int NPCType;
            public int NPCMOVE;
            public int s_nDisplayPlag;
            public int MapID;
            public int NpcID;
            public int Model;
            public List<int> ItemId = new();
            public List<int> MastersMatchItemIds = new();
            public List<sNPC_TYPE_PORTAL> NpcTypePortal = new();
            public List<int> SpecialEventItems = new();
            public int nExtraData;
            public int nvType;
            public int ExtraType;
            public List<ExtraQuest> npcQuests = new();

        }
        public class sNPC_TYPE_PORTAL
        {
            public int s_nPortalType;
            public int s_nPortalCount;
            public List<PortalNpc> portals = new List<PortalNpc>();
        }
        public class PortalNpc
        {
            public int s_dwEventID;
            public List<sPORTAL_REQ> ReqArray = new();

        }

        public class sPORTAL_REQ
        {
            public int s_eEnableType;
            public int s_nEnableID;
            public int s_nEnableCount;
        }
        public class ExtraQuest
        {
            public int s_nEInitSate;
            public int nActcnt;
            public List<ExtraAction> extraActions = new();

        }
        public class ExtraAction
        {
            public int ActionType;
            public int ECompState;
            public int QuestCount;
            public int[] QuestIds;
        }

        public enum eNPC_TYPE : int        // 바뀌면 안된다.
        {
            NT_NONE = 0,
            NT_TRADE = 1,
            NT_DIGITAMA_TRADE = 2,
            NT_PORTAL = 3,
            NT_MAKE_TACTICS = 4,
            NT_ELEMENT_ITEM = 5,
            NT_WAREHOUSE = 6,
            NT_TACTICSHOUSE = 7,
            NT_GUILD = 8,
            NT_DIGICORE = 9,
            NT_CAPSULE_MACHINE = 10,
            NT_SKILL = 11,
            NT_EVENT_STORE = 12,
            NT_DATS_PORTAL = 13,
            NT_PROPERTY_STORE = 14,
            NT_GOTCHA_MACHINE = 15,
            NT_MASTERS_MATCHING = 16,
            NT_MYSTERY_MACHINE = 17,
            NT_SPIRIT_EVO = 18,
            NT_SPECIAL_EVENT = 19,
            NT_ITEM_PRODUCTION_NPC = 20,
            NT_BATTLE_REGISTRANT_NPC = 21,
            NT_INFINITEWAR_MANAGER_NPC = 22,        // 무한대전 진행 NPC
            NT_INFINITEWAR_NOTICEBOARD_NPC = 23,    // 무한대전 게시판 NPC
            NT_EXTRA_EVOLUTION_NPC = 24,
            NT_Unknow = 25,
            NT_Unknow1 = 26,
            Default
        };

        public enum eNPC_MOVE : int
        {
            MT_NONE = 0,
            MT_MOVE = 1,
        };

        enum eNPC_EXTRA
        {
            NE_QUEST,

            NE_MAX_CNT,
        };

        public enum sNPC_TYPE_SPECIAL_EVENT : int
        {
            EVENT_NONE = 0,
            EVENT_CARDGAME = 1, // 피에몬 카드 게임
            EVENT_PINOKIMON = 2,    // 2014 겨울(크리스마스)이벤트
        }
        public class PortalInfo
        {
            public int s_dwPortalID;
            public int s_dwPortalType;
            public int s_dwSrcMapID;
            public int s_nSrcTargetX;
            public int s_nSrcTargetY;
            public int s_nSrcRadius;
            public int s_dwDestMapID;
            public int s_nDestTargetX;
            public int s_nDestTargetY;
            public int s_nDestRadius;
            public int s_ePortalType;
            public int s_dwUniqObjectID;
            public int s_nPortalKindIndex;
            public int s_nViewTargetX;
            public int s_nViewTargetY;
        }
        public class Portal
        {
            public int pMapGroup;
            public List<PortalInfo> portalInfos = new();

        }
        public class Stages
        {
            public int dwNPCIdx;
            public List<StageInfos> StageInfo = new();
        }

        public class StageInfos
        {


            public int dwNPCIdx;
            public int nStageCount;
            public int nStage;
            public int s_nSummon_Type;         // Summon Tipo 0: Ambos, 1: Summon apenas um, 2: 2:
            public int s_dwRewardItemIdx;      // Código do item pago pelo ponto de fluxo para a vitória / derrota
            public int s_nWinPoint;            // Ponto de pagamento para a vitória
            public int s_nLosePoint;           // Ponto de pagamento para derrota
            public int s_nMonsterInfoIndex;    // Informações sobre o monstro para co

        }
    }
}
