using System;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics.Metrics;
using System.IO;
using System.Runtime;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using BinXmlConverter.Classes;
using Microsoft.VisualBasic;
using static BinXmlConverter.Classes.DigimonEvo;
using static BinXmlConverter.Classes.DigimonData;
using static BinXmlConverter.Classes.Ride;
using static BinXmlConverter.Classes.ModelInfo;
using static BinXmlConverter.Classes.CharCreateTable;
using static BinXmlConverter.Classes.DigimonCreateTable;
using static BinXmlConverter.Classes.NPC;
using static BinXmlConverter.Classes.DMBaseInfo;
using static BinXmlConverter.Classes.SkillData;
using static BinXmlConverter.Classes.BuffData;
using static BinXmlConverter.Classes.ItemList;
using static BinXmlConverter.Classes.EventTotal;
using static BinXmlConverter.Classes.GotchaMachine;

namespace BinXmlConverter
{

    public partial class BinToXml : Form
    {

        public BinToXml()
        {
            InitializeComponent();
            BinToXml2 binToXml2 = new BinToXml2();
            binToXml2.Show();
        }

        #region[Buttons]
        private void button1_Click(object sender, EventArgs e)
        {

        }
        private void Digimon_ListToXml_Click(object sender, EventArgs e)
        {
            try
            {
                var DigimonListInput = "Data\\Digimon_List.bin";
                var DigimonListOutPut = "XML\\DigimonList.xml";

                DigimonData[] digimonDatas = new DigimonData[1];

                digimonDatas = DigimonToXml(DigimonListInput);
                ExportDigimon_ListToXml(DigimonListOutPut, digimonDatas);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
            }

        }
        private void Digimon_ListToBin_Click(object sender, EventArgs e)
        {
            try
            {
                var DigimonListInput = "XML\\DigimonList.xml";
                var DigimonListOutPut = "BIN\\\\Digimon_List.bin";

                var digimonData = ImportDigimonDataFromXml(DigimonListInput);
                DigimonData.ExportToBinary(DigimonListOutPut, digimonData);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
            }
        }
        private void ModelToXml_Click(object sender, EventArgs e)
        {
            try
            {
                var ModelInput = "DATA\\Model.dat";
                var ModelOutPut = "XML\\Model.xml";
                ModelInfo[] modelInfos = new ModelInfo[0];
                modelInfos = ModelToXml(ModelInput);
                ExportModelToXml(ModelOutPut, modelInfos);
                Sucess();

            }
            catch (Exception)
            {
                Fail();
            }

        }
        private void ModelToBin_Click(object sender, EventArgs e)
        {
            var ModelInput = "XML\\Model.xml";
            var ModelOutPut = "BIN\\Model.dat";

            ModelInfo[] modelInfos = new ModelInfo[0];
            try
            {
                modelInfos = ImportModelFromXml(ModelInput);
                ExportModelToBinary(ModelOutPut, modelInfos);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
            }

        }
        private void DigimonEvoToBin_Click(object sender, EventArgs e)
        {
            try
            {
                var DigimonEvoOutPut = "BIN\\DigimonEvo.bin";
                var DigimonEvoInput = "XML\\DigimonEvo.xml";

                Evolution[] evolutions = new Evolution[1];
                evolutions = ImportDigimonEvolutionFromXml(DigimonEvoInput);
                DigimonEvo.ExportToBinary(DigimonEvoOutPut, evolutions);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
            }

        }
        private void DigimonEvoToXml_Click(object sender, EventArgs e)
        {
            try
            {

                var DigimonEvoInput = "DATA\\DigimonEvo.bin";
                var DigimonEvoOutPut = "XML\\DigimonEvo.xml";

                Evolution[] evolutions = new Evolution[1];
                evolutions = DigimonEvoToXml(DigimonEvoInput);
                ExportDigimonEvolToXml(DigimonEvoOutPut, evolutions);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
            }
        }
        private void RideToXml_Click(object sender, EventArgs e)
        {
            try
            {
                var DigimonRideInput = "DATA\\Ride.bin";
                var DigimonRideOutPut = "XML\\Ride.xml";
                Ride[] Rides = new Ride[1];
                Rides = RideToXml(DigimonRideInput);
                ExportRidesToXml(DigimonRideOutPut, Rides);
                Sucess();
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void RideToBin_Click(object sender, EventArgs e)
        {
            try
            {
                var RideInput = "XML\\Ride.xml";
                var RideOutput = "BIN\\Ride.bin";

                Ride[] Rides = new Ride[1];
                Rides = ImportRidesFromXml(RideInput);
                ExportRidesToBinary(RideOutput, Rides);
                Sucess();
            }
            catch (Exception)
            {

                Fail();
            }
        }
        private void CharCreateTableToBin_CLick(object sender, EventArgs e)
        {
            try
            {

                var CharCreateTableOutPut = "BIN\\CharCreateTable.bin";
                var CharCreateTableInput = "XML\\CharCreateTable.xml";
                var DigiCreateTableInput = "XML\\DigimonCreateTable.xml";

                CharCreateTable[] charCreateTable = ImportCharCreateTableFromXml(CharCreateTableInput);
                DigimonCreateTable[] digiCreateTable = ImportDigimonCreateTableFromXml(DigiCreateTableInput);
                ExportCharCreateTableToBinary(CharCreateTableOutPut, charCreateTable, digiCreateTable);

                Sucess();
            }
            catch (Exception)
            {
                Fail();
            }
        }
        private void CharCreateTableToXml_Click(object sender, EventArgs e)
        {
            try
            {

                var CharCreateTableInput = "DATA\\CharCreateTable.bin";
                var CharCreateTableOutPut = "XML\\CharCreateTable.xml";
                var DigiCreateTableOutPut = "XML\\DigimonCreateTable.xml";

                (CharCreateTable[] charCreateTable, DigimonCreateTable[] digiCreateTable) = ReadCharCreateTableFromBinary(CharCreateTableInput);

                ExportCharacterCreateTableToXml(CharCreateTableOutPut, charCreateTable);
                ExportDigimonCreateTableToXml(DigiCreateTableOutPut, digiCreateTable);

                Sucess();
            }
            catch (Exception)
            {
                Fail();
            }
        }
        private void NpcToBin_Click(object sender, EventArgs e)
        {
            try
            {
                var NpcInput = "XML\\Npc.xml";
                var NpcOutput = "BIN\\Npc.bin";
                var ModelInput = "XML\\ModelNpc.xml";
                var EventNpcInput = "XML\\EventNpc.xml";


                NPCs[] npcs = NPCs.ImportNPCsFromXML(NpcInput);
                ModelNpc[] model = ModelNpc.ImportModelNpcFromXml(ModelInput);
                EventNpc[] events = EventNpc.ImportEventNpcFromXml(EventNpcInput);
                NPCs.ExportNPCsToBinary(npcs, model, events, NpcOutput);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
            }

        }
        private void NpcToXml_Click(object sender, EventArgs e)
        {
            try
            {
                var NpcInput = "DATA\\Npc.bin";
                var NpcOutput = "XML\\Npc.xml";
                var ModelNpcOutput = "XML\\ModelNpc.xml";
                var EventNpcOutput = "XML\\EventNpc.xml";

                (NPCs[] npcs, ModelNpc[] model, EventNpc[] eventNpc) = NPCs.ExportNpcToXml(NpcInput);
                NPCs.ExportNPCsToXml(NpcOutput, npcs);
                ModelNpc.ExportModelNpcToXml(model, ModelNpcOutput);
                EventNpc.ExportEventNpcToXml(EventNpcOutput, eventNpc);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
            }
        }
        private void DMBaseToBin_Click(object sender, EventArgs e)
        {
            try
            {

                var DMBaseOutput = "BIN\\DMBase.bin";
                var TamerBaseInput = "XML\\TamerBaseInfo.xml";
                var DigimonBaseInput = "XML\\DigimonBaseInfo.xml";
                var JumpBoosterInput = "XML\\JumpBooster.xml";
                var CsBaseMapInfoInput = "XML\\CsBaseMapInfo.xml";
                var PartyInput = "XML\\Party.xml";
                var GuildInput = "XML\\Guild.xml";
                var limitInput = "XML\\Limit.xml";
                var StoreInput = "XML\\Store.xml";
                var PaneltyInfoInput = "XML\\PaneltyInfo.xml";
                var EvolutionBaseApplyInput = "XML\\EvolutionBaseApply.xml";
                var DigimonEvoMaxSkillInput = "XML\\DigimonEvoMaxSkill.xml";
                var ExpansionDataInput = "XML\\ExpansionData.xml";

                DMBaseInfo[] dMBaseInfos = ImportTamerBaseFromXml(TamerBaseInput);
                DigiBaseInfo[] digiBaseInfos = ImportDigimonBaseFromXml(DigimonBaseInput);
                Jumpbooster[] jumpboosters = ImportJumpBoosterFromXml(JumpBoosterInput);
                CsBaseMapInfo[] csBaseMapInfos = ImportCsBaseMapInfoFromXml(CsBaseMapInfoInput);
                Party[] party = ImportPartyFromXml(PartyInput);
                Guild[] guild = ImportGuildFromXml(GuildInput);
                Limit[] limit = ImportLimitFromXml(limitInput);
                Store[] store = ImportStoreFromXml(StoreInput);
                PaneltyInfo[] paneltyInfos = ImportPaneltyInfoFromXml(PaneltyInfoInput);
                EvolutionBaseApply[] evolutionBaseApplies = ImportEvolutionBaseApplyFromXml(EvolutionBaseApplyInput);
                DigimonEvoMaxSkillLevel[] digimonEvoMaxSkills = ImportDigimonEvoMaxSkillLevelFromXml(DigimonEvoMaxSkillInput);
                ExpansionCondition[] expansion = ImportExpansionConditionFromXml(ExpansionDataInput);
                ExportDMBaseToBinary(DMBaseOutput, dMBaseInfos, digiBaseInfos, csBaseMapInfos, jumpboosters, party, guild, limit, store, paneltyInfos, evolutionBaseApplies, digimonEvoMaxSkills, expansion);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
                throw;
            }
        }
        private void DMBaseToXml_Click(object sender, EventArgs e)
        {
            try
            {

                var DMBaseInput = "DATA\\DMBase.bin";
                var TamerBaseOutput = "XML\\TamerBaseInfo.xml";
                var DigimonBaseOutput = "XML\\DigimonBaseInfo.xml";
                var JumpBoosterOutput = "XML\\JumpBooster.xml";
                var CsBaseMapInfoOutput = "XML\\CsBaseMapInfo.xml";
                var PartyOutput = "XML\\Party.xml";
                var GuildOutput = "XML\\Guild.xml";
                var limitOutput = "XML\\Limit.xml";
                var StoreOutput = "XML\\Store.xml";
                var PaneltyInfoOutput = "XML\\PaneltyInfo.xml";
                var EvolutionBaseApplyOutput = "XML\\EvolutionBaseApply.xml";
                var DigimonEvoMaxSkillOutput = "XML\\DigimonEvoMaxSkill.xml";
                var ExpasionConditionOutput = "XML\\ExpansionData.xml";
                DMBaseInfo[] dMBaseInfos = new DMBaseInfo[1];
                DigiBaseInfo[] digiBaseInfos = new DigiBaseInfo[1];
                Jumpbooster[] jumpboosters = new Jumpbooster[1];
                CsBaseMapInfo[] csBaseMapInfos = new CsBaseMapInfo[1];
                Party[] party = new Party[1];
                Guild[] guild = new Guild[1];
                Limit[] limit = new Limit[1];
                Store[] store = new Store[1];
                PaneltyInfo[] paneltyInfos = new PaneltyInfo[1];
                EvolutionBaseApply[] evolutionBaseApplies = new EvolutionBaseApply[1];
                DigimonEvoMaxSkillLevel[] digimonEvoMaxSkills = new DigimonEvoMaxSkillLevel[1];
                ExpansionCondition[] expansion = new ExpansionCondition[1];

                (dMBaseInfos, digiBaseInfos, csBaseMapInfos, jumpboosters, party, guild, limit, store, paneltyInfos, evolutionBaseApplies, digimonEvoMaxSkills, expansion) = ExportDMBaseToXml(DMBaseInput);
                ExportTamerBaseToXml(TamerBaseOutput, dMBaseInfos);
                ExportDigimonBaseToXml(DigimonBaseOutput, digiBaseInfos);
                ExportJumpBoosterInfoFromXml(jumpboosters, JumpBoosterOutput);
                ExportCsMapBaseToXml(csBaseMapInfos, CsBaseMapInfoOutput);
                ExportPartyToXml(party, PartyOutput);
                ExportGuildToXml(guild, GuildOutput);
                ExportLimitToXml(limit, limitOutput);
                ExportStoreToXml(store, StoreOutput);
                ExportPaneltyInfoToXml(paneltyInfos, PaneltyInfoOutput);
                ExportEvolutionBaseToXml(evolutionBaseApplies, EvolutionBaseApplyOutput);
                ExportDigimonEvoMaxSkillLevelToXml(digimonEvoMaxSkills, DigimonEvoMaxSkillOutput);
                ExportExpansionConditionToXml(ExpasionConditionOutput, expansion);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
            }
        }
        private void SkillToXml_Click(object sender, EventArgs e)
        {
            try
            {

                var SkillInput = "DATA\\Skill.bin";
                var SkillOutput = "XML\\Skill.xml";
                var TamerSkillOutput = "XML\\TamerSkill.xml";
                var AreaCheckOutput = "XML\\AreaCheck.xml";


                (SkillData[] Skill, TamerSkill[] tamer, AreaCheck[] area) = SkillToXml(SkillInput);

                ExportSkillToXml(SkillOutput, Skill);
                ExportTamerSkillToXml(tamer, TamerSkillOutput);
                ExportAreaCheckToXml(area, AreaCheckOutput);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
                throw;
            }

        }
        private void SkillToBin_Click(object sender, EventArgs e)
        {
            try
            {
                var SkillInput = "XML\\Skill.xml";
                var TamerSkillInput = "XML\\TamerSkill.xml";
                var AreaCheckInput = "XML\\AreaCheck.xml";
                var SkillOutput = "BIN\\Skill.bin";

                SkillData[] Skill = ImportSkillFromXml(SkillInput);
                TamerSkill[] tamer = ImportTamerSkillFromXml(TamerSkillInput);
                AreaCheck[] area = ImportAreaCheckFromXml(AreaCheckInput);

                ExportSkillToBinary(SkillOutput, Skill, tamer, area);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
            }
        }
        private void BuffToBinClick(object sender, EventArgs e)
        {
            try
            {
                var BuffInput = "XML\\Buff.xml";
                var BuffOutput = "BIN\\Buff.bin";

                BuffData[] Buff = ImportBuffFromXml(BuffInput);
                ExportBuffToBinary(BuffOutput, Buff);
                Sucess();
            }
            catch (Exception)
            {
                Fail();

                throw;
            }
        }
        private void BuffToXml_Click(object sender, EventArgs e)
        {
            try
            {
                var BuffInput = "DATA\\Buff.bin";
                var BuffOutput = "XML\\Buff.xml";
                BuffData[] Buff = BuffToXml(BuffInput);
                ExportBuffToXml(Buff, BuffOutput);
                Sucess();
            }
            catch (Exception)
            {
                Fail();

                throw;
            }

        }
        private void ItemListToBin_Click(object sender, EventArgs e)
        {
            try
            {


                var outputFile = "BIN\\ItemList.bin";
                var ItemTapInput = "XML\\ItemTap.xml";
                var CoolTimeInput = "XML\\ItemCoolTime.xml";
                var DisplayItemsInput = "XML\\ItemDisplay.xml";
                var ItemTypeNameInput = "XML\\ItemTypeName.xml";
                var ItemRankInput = "XML\\ItemRank.xml";
                var ElementItemInput = "XML\\ElementItem.xml";
                var Element1ItemInput = "XML\\ElementItem1.xml";
                var itemExchangesInput = "XML\\ItemExchange.xml";
                var AcessoryItemInput = "XML\\ItemAcessorys.xml";
                var AcessoryEnchantInput = "XML\\AcessorysEnchant.xml";
                var ItemMakingInput = "XML\\ItemMaking.xml";
                var ItemMakingGroupInput = "XML\\ItemMakingGroupList.xml";
                var XaiItemListInput = "XML\\ItemXai.xml";
                var ItemRankEffectListInput = "XML\\ItemRankEffectList.xml";
                var LookItemInput = "XML\\ItemLook.xml";
                var inputFile = "XML\\ItemList.xml";


                ITEM items = ImportFromXml(inputFile);
                ItemTap[] Tapitems = ImportItemTapsFromXml(ItemTapInput);
                ItemCoolTime[] Cooltimes = ImportItemCoolTimesFromXml(CoolTimeInput);
                ItemDisplay[] displays = ImportItemDisplaysFromXml(DisplayItemsInput);
                ItemTypeName[] itemTypeNames = ImportItemTypesFromXml(ItemTypeNameInput);
                ElementItem[] elementItems = ImportElementItemsFromXml(ElementItemInput);
                ElementItem[] elementItems1 = ImportElementItemsFromXml(Element1ItemInput);
                ItemRank[] itemRanks = ImportItemRanksFromXml(ItemRankInput);
                ItemExchange[] itemExchange = ImportItemExchangesFromXml(itemExchangesInput);
                Accessory[] accessories = ImportAccessoriesFromXml(AcessoryItemInput);
                AccessoryEnchant[] accessoryEnchants = ImportAccessoryEnchantsFromXml(AcessoryEnchantInput);
                ItemMaking Making = ImportItemMakingFromXml(ItemMakingInput);
                ItemMakingGroup[] itemMakingGroups = ImportItemMakingGroupsFromXml(ItemMakingGroupInput);
                XaiItem[] xaiItems = ImportXaiItemsFromXml(XaiItemListInput);
                ItemRankEffect[] itemRankEffects = ImportItemRankEffectFromXml(ItemRankEffectListInput);
                LookItem[] lookItems = ImportLookItemsFromXml(LookItemInput);
                ExportToBinary(outputFile, items, Tapitems, Cooltimes, displays, itemTypeNames, itemRanks, elementItems, elementItems1, itemExchange, accessories, accessoryEnchants, Making, itemMakingGroups, xaiItems, itemRankEffects, lookItems);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
                throw;
            }
        }
        private void ItemListToXml_Click(object sender, EventArgs e)
        {
            try
            {

                var inputFile = "DATA\\ItemList.bin";
                var ItemTapOutPut = "XML\\ItemTap.xml";
                var CoolTimeOutPut = "XML\\ItemCoolTime.xml";
                var DisplayItemsOutPut = "XML\\ItemDisplay.xml";
                var ItemTypeNameOutPut = "XML\\ItemTypeName.xml";
                var ItemRankOutPut = "XML\\ItemRank.xml";
                var ElementItemOutPut = "XML\\ElementItem.xml";
                var Element1ItemOutPut = "XML\\ElementItem1.xml";
                var itemExchangesOutPut = "XML\\ItemExchange.xml";
                var AcessoryItemOutPut = "XML\\ItemAcessorys.xml";
                var AcessoryEnchantOutPut = "XML\\AcessorysEnchant.xml";
                var ItemMakingOutPut = "XML\\ItemMaking.xml";
                var ItemMakingGroupOutPut = "XML\\ItemMakingGroupList.xml";
                var XaiItemListOutPut = "XML\\ItemXai.xml";
                var ItemRankEffectListOutPut = "XML\\ItemRankEffectList.xml";
                var ItemLookListOutPut = "XML\\ItemLook.xml";

                (ItemTap[] ItemsTap, ItemCoolTime[] CoolItems, ItemDisplay[] displayItems, ItemTypeName[] itemTypeName, ItemRank[] ItemRanks, ElementItem[] ElementItems, ElementItem[] ElementItems1, ItemExchange[] itensExchanges, Accessory[] acessories, AccessoryEnchant[] acessoryEnchants, ItemMaking MakingItems, ItemMakingGroup[] itemMakingGroups, XaiItem[] xai, ItemRankEffect[] itemRankEffects, LookItem[] itemLooks) = ReadItemsFromBinary(inputFile, false);
                (ItemTap[] ItemsTaap, ItemCoolTime[] CoolIteems, ItemDisplay[] displayIteems, ItemTypeName[] itemTypasdeName, ItemRank[] ItemRasdanks, ElementItem[] ElemedntItems, ElementItem[] ElementIasdtems1, ItemExchange[] itensExcadshanges, Accessory[] aceasdssories, AccessoryEnchant[] acessoryasdEnchants, ItemMaking MakigfdngItems, ItemMakingGroup[] itemMakingGroasdups, XaiItem[] xasdai, ItemRankEffect[] itemRaasdnkEffects, LookItem[] itemLasdooks) = ReadItemsFromBinary(inputFile, true);

                ExportItemTapToXml(ItemTapOutPut, ItemsTap);
                ExportCoolTimeToXml(CoolTimeOutPut, CoolItems);
                ExportDisplayItemsToXml(DisplayItemsOutPut, displayItems);
                ExportItemTypeNamesToXml(ItemTypeNameOutPut, itemTypeName);
                ExportItemRankListToXml(ItemRankOutPut, ItemRanks);
                ExportItemElementToXml(ElementItemOutPut, ElementItems);
                ExportItemElementToXml(Element1ItemOutPut, ElementItems1);
                ExportItemExchangesToXml(itemExchangesOutPut, itensExchanges);
                ExportAcessorysItemsToXml(AcessoryItemOutPut, acessories);
                ExportAcessorysEnchantToXml(AcessoryEnchantOutPut, acessoryEnchants);
                ExportItemMakingToXml(ItemMakingOutPut, MakingItems);
                ExportMakingGroupsToXml(ItemMakingGroupOutPut, itemMakingGroups);
                ExportItemXaiToXml(XaiItemListOutPut, xai);
                ExportItemRankEffectsToXml(ItemRankEffectListOutPut, itemRankEffects);
                ExportItemLookToXml(ItemLookListOutPut, itemLooks);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
                throw;
            }

        }
        private void EventToXml_Click(object sender, EventArgs e)
        {
            try
            {

                var EventInput = "DATA\\Event.bin";
                var AttendenceTimeOutput = "XML\\AttendenceTime.xml";
                var EventOutput = "XML\\Event.xml";
                var MensalEventOutput = "XML\\MensalEvent.xml";
                var MonthlyEventOutput = "XML\\MonthlyEvent.xml";
                var TimeEventOutput = "XML\\TimeEvent.xml";
                var Event100DaysOutput = "XML\\Event100Days.xml";
                // var JumpingOutput = "XML\\Jumping.xml";

                (Atendence[] atendences, Event[] events, AttendenceEvent[] mensalEvent, MonthlyEvent[] monthlyEvent, TimeEvent[] timeEvent,
                    Event100Days[] event100Days) = ReadEventFromBinary(EventInput);

                ExportEventsToXml(events, EventOutput);
                ExportAttendenceToXml(atendences, AttendenceTimeOutput);
                ExportMensalEventToXml(mensalEvent, MensalEventOutput);
                ExportMonthlyEventsToXml(monthlyEvent, MonthlyEventOutput);
                ExportTimeEventToXml(timeEvent, TimeEventOutput);
                ExportEvent100DaysToXml(event100Days, Event100DaysOutput);

                // ExportJumpingToXml(jumpings, JumpingOutput);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
                throw;
            }
        }
        private void EventToBin_Click(object sender, EventArgs e)
        {
            try
            {

                var EventOutput = "BIN\\Event.bin";
                var AttendenceTimeInput = "XML\\AttendenceTime.xml";
                var EventInput = "XML\\Event.xml";
                var MensalEventInput = "XML\\MensalEvent.xml";
                var MonthlyEventInput = "XML\\MonthlyEvent.xml";
                var TimeEventInput = "XML\\TimeEvent.xml";
                var Event100DaysInput = "XML\\Event100Days.xml";

                //var JumpingInput = "XML\\Jumping.xml";

                Atendence[] atendences = ImportAttendenceFromXml(AttendenceTimeInput);
                Event[] events = ImportEventsFromXml(EventInput);
                AttendenceEvent[] mensalEvent = ImportMensalEventsFromXml(MensalEventInput);
                MonthlyEvent[] monthlyEvents = ImportMonthlyEventsFromXml(MonthlyEventInput);
                TimeEvent[] timeEvent = ImportTimeEventsFromXml(TimeEventInput);
                Event100Days[] event100Days = ImportEvent100DaysFromXml(Event100DaysInput);
                //Jumping[] jumpings = ImportJumpingFromXml(JumpingInput);

                WriteEventToBinary(EventOutput, atendences, monthlyEvents, events, mensalEvent, timeEvent,
                   event100Days);

                Sucess();
            }
            catch (Exception)
            {
                Fail();
                throw;
            }
        }
        private void GotchaToXml_Click(object sender, EventArgs e)
        {
            try
            {
                var GotchaInput = "DATA\\Gotcha.bin";
                var GotchaOutput = "XML\\Gotcha.xml";
                var GotchaItemsOutput = "XML\\GotchaItems.xml";
                var GotchaRareItemsOutput = "XML\\GotchaRareItems.xml";
                var GotchaMysteryItemsOutput = "XML\\GotchaMysteryItems.xml";
                var GotchaMysteryCoinsOutput = "XML\\GotchaMysteryCoins.xml";

                (Gotcha[] gotcha, GotchaItems[] gotchaItems, GotchaRareItems[] gotchaRareItems,
                    GotchaMysteryItem[] gotchaMysteryItems,
                    GotchaMysteryCoin[] gotchaMysteryCoins) = ReadGotchaFromBinary(GotchaInput);

                ExportGotchasToXml(GotchaOutput, gotcha);
                ExportGotchaItemsToXml(GotchaItemsOutput, gotchaItems);
                ExportGotchaRareItemsToXml(gotchaRareItems, GotchaRareItemsOutput);
                ExportGotchaMysteryItemsToXml(gotchaMysteryItems, GotchaMysteryItemsOutput);
                ExportGotchaMysteryCoinsToXml(gotchaMysteryCoins, GotchaMysteryCoinsOutput);
                Sucess();
            }
            catch (Exception ex)
            {
                Fail();
            }

        }
        private void GotchaToBin_Click(object sender, EventArgs e)
        {

            try
            {
                var GotchaOutput = "BIN\\Gotcha.bin";
                var GotchaInput = "XML\\Gotcha.xml";
                var GotchaItemsInput = "XML\\GotchaItems.xml";
                var GotchaRareItemsInput = "XML\\GotchaRareItems.xml";
                var GotchaMysteryItemsInput = "XML\\GotchaMysteryItems.xml";
                var GotchaMysteryCoinsInput = "XML\\GotchaMysteryCoins.xml";


                Gotcha[] gotcha = ImportGotchasFromXml(GotchaInput);
                GotchaItems[] gotchaItems = ImportGotchaItemsFromXml(GotchaItemsInput);
                GotchaRareItems[] gotchaRareItems = ImportGotchaRareItemsFromXml(GotchaRareItemsInput);
                GotchaMysteryItem[] gotchaMysteryItems = ImportGotchaMysteryItemsFromXml(GotchaMysteryItemsInput);
                GotchaMysteryCoin[] gotchaMysteryCoins = ImportGotchaMysteryCoinsFromXml(GotchaMysteryCoinsInput);
                WriteGotchaToBinary(GotchaOutput, gotcha, gotchaItems, gotchaRareItems, gotchaMysteryItems, gotchaMysteryCoins);
                Sucess();

            }
            catch (Exception ex)
            {
                Fail();
            }
        }
        #endregion


        public void Sucess()
        {
            MessageBox.Show("Processamento concluído com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public void Fail()
        {
            MessageBox.Show(" Falha no processamento.", "Falha", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
        }


    }

}