using BinXmlConverter.Classes;
using static BinXmlConverter.Classes.Achieve;
using static BinXmlConverter.Classes.Digimon_Book;
using static BinXmlConverter.Classes.Mastercard;
using static BinXmlConverter.Classes.MapNpc;
using static BinXmlConverter.Classes.UIText;
using static BinXmlConverter.Classes.MapMonsters;
using static BinXmlConverter.Classes.MapData;
using static BinXmlConverter.Classes.QuestInfos;
using static BinXmlConverter.Classes.ExtraExchanges;
using static BinXmlConverter.Classes.Tactics.TDBTactic;
using static BinXmlConverter.Classes.Portal;
using static BinXmlConverter.Classes.TalkGlobal;
using static BinXmlConverter.Classes.WorldMap;
using static BinXmlConverter.Classes.MapObject;
using static BinXmlConverter.Classes.CashShop;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static BinXmlConverter.Classes.Tactics;
using static BinXmlConverter.Classes.ItemList;
using static BinXmlConverter.Classes.Arak;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using System.Drawing;
using System.Xml.Linq;
using System;

namespace BinXmlConverter
{
    public partial class BinToXml2 : Form
    {
        public BinToXml2()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void BinToXml2_Load(object sender, EventArgs e)
        {

        }

        private void MastercardToXml_Click(object sender, EventArgs e)
        {
            try
            {

                var MastercardInput = "DATA\\MasterCard.bin";
                var MastercardOutput = "XML\\";
                // Lê as classes MasterCards, Leader, LeaderAbility, DigimonImgPath, PlatePath, Elemental e Attribute
                (MasterCards[], Leader[], LeaderAbility[], DigimonImgPath[], PlatePath[], Elemental[], SealAttribute[], UnknowInformation[] unknow) data = ExportMastercardFromBinary(MastercardInput);

                MasterCards[] masterCards = data.Item1;
                Leader[] leaders = data.Item2;
                LeaderAbility[] leaderAbilities = data.Item3;
                DigimonImgPath[] digimonImgPaths = data.Item4;
                PlatePath[] platePaths = data.Item5;
                Elemental[] elementals = data.Item6;
                SealAttribute[] attributes = data.Item7;
                UnknowInformation[] unknows = data.Item8;
                // Use os arrays de dados como necessário
                ExportMastercardToXml(MastercardOutput, data.Item1, data.Item2, data.Item3, data.Item4, data.Item5, data.Item6, data.Item7, data.Item8);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
                throw;
            }
        }

        public void Sucess()
        {
            MessageBox.Show("Processamento concluído com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public void Fail()
        {
            MessageBox.Show(" Falha no processamento.", "Falha", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
        }

        private void MastercardToBin_Click(object sender, EventArgs e)
        {
            try
            {

                var MastercardOutput = "BIN\\MasterCard.bin";

                (MasterCards[], Leader[], LeaderAbility[], DigimonImgPath[], PlatePath[], Elemental[], SealAttribute[], UnknowInformation[]) data = ImportMastercardsFromXml("XML\\");

                MasterCards[] masterCards = data.Item1;
                Leader[] leaders = data.Item2;
                LeaderAbility[] leaderAbilities = data.Item3;
                DigimonImgPath[] digimonImgPaths = data.Item4;
                PlatePath[] platePaths = data.Item5;
                Elemental[] elementals = data.Item6;
                SealAttribute[] attributes = data.Item7;
                UnknowInformation[] unknows = data.Item8;
                // Use os arrays de dados como necessário
                WriteBinary(MastercardOutput, data.Item1, data.Item2, data.Item3, data.Item4, data.Item5, data.Item6, data.Item7, data.Item8);
                Sucess();
            }
            catch (Exception)
            {
                Fail();

                throw;
            }
        }
        private void AchieveToXML_Click(object sender, EventArgs e)
        {
            var Input = "DATA\\Achieve.bin";
            var AchieveOutput = "XML\\Achieve.xml";
            var AchieveUnknowOutput = "XML\\AchieveUnknow.xml";
            try
            {

                (sINFO[], AchieveSINFO[]) data = ReadAchieveFromBinary(Input);

                ExportSinfoToXml(AchieveUnknowOutput, data.Item1);
                ExportAchieveSinfoToXml(AchieveOutput, data.Item2);
                Sucess();

            }
            catch (Exception)
            {
                Fail();
                throw;
            }
        }
        private void AchieveToBin_Click(object sender, EventArgs e)
        {
            var Output = "BIN\\Achieve.bin";
            var AchieveInput = "XML\\Achieve.xml";
            var AchieveUnknowInput = "XML\\AchieveUnknow.xml";

            try
            {
                sINFO[] sINFO = ImportsInfoFromXml(AchieveUnknowInput);
                AchieveSINFO[] achieveSINFOs = ImportAchieveSinfoFromXml(AchieveInput);
                WriteAchieveToBinary(Output, sINFO, achieveSINFOs);
                Sucess();

            }
            catch (Exception)
            {
                Fail();
                throw;
            }

        }
        private void Digimon_BookToXML_Click(object sender, EventArgs e)
        {
            var input = "DATA\\Digimon_Book.bin";
            var BookInfoOutput = "XML\\BookInfo.xml";
            var EncyclopediaExceptionOutput = "XML\\EncyclopediaException.xml";
            var DeckOptionOutput = "XML\\DeckOption.xml";
            var DeckCompositionOutput = "XML\\DeckComposition.xml";
            try
            {

                (BookInfo[], EncyclopediaException[], DeckOption[], DeckComposition[]) data = ReadDigimonBookFromBinary(input);

                ExportBookInfoToXml(data.Item1, BookInfoOutput);
                ExportEncyclopediaExceptionToXml(data.Item2, EncyclopediaExceptionOutput);
                ExportDeckOptionToXml(data.Item3, DeckOptionOutput);
                ExportDeckCompositionToXml(data.Item4, DeckCompositionOutput);
                Sucess();
            }
            catch (Exception)
            {
                Fail();

                throw;
            }
        }
        private void Digimon_BookToBin_Click(object sender, EventArgs e)
        {
            var output = "BIN\\Digimon_Book.bin";
            var BookInfoInput = "XML\\BookInfo.xml";
            var EncyclopediaInput = "XML\\EncyclopediaException.xml";
            var DeckOptionInput = "XML\\DeckOption.xml";
            var DeckCompositionInput = "XML\\DeckComposition.xml";
            try
            {
                BookInfo[] books = ImportBookInfosFromXml(BookInfoInput);
                EncyclopediaException[] encyclopediaExceptions = ImportEncyclopediaExceptionsFromXml(EncyclopediaInput);
                DeckOption[] deckOptions = ImportDeckOptionsFromXml(DeckOptionInput);
                DeckComposition[] deckCompositions = ImportDeckCompositionsFromXml(DeckCompositionInput);
                WriteDigimon_BookToBinary(output, books, encyclopediaExceptions, deckOptions, deckCompositions);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
                throw;
            }
        }
        private void MapNpcToXML_Click(object sender, EventArgs e)
        {
            var input = "DATA\\MapNpc.bin";
            var Output = "XML\\MapNpc.xml";
            try
            {
                MapNPCs[] mapNpcs = ReadMapNPCFromBinary(input);
                ExportMapNPCsToXml(mapNpcs, Output);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
                throw;
            }
        }
        private void MapNpcToBin_Click(object sender, EventArgs e)
        {
            var Input = "XML\\MapNpc.xml";
            var Output = "BIN\\MapNpc.bin";
            try
            {
                MapNPCs[] map = ImportMapNPCsFromXml(Input);
                WriteMapNPCToBinary(Output, map);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
                throw;
            }
        }

        private void UitextToXml_Click(object sender, EventArgs e)
        {
            var Input = "DATA\\UIText.bin";
            var Output = "XML\\UIText.xml";

            try
            {
                UIText[] result = ReadUITextFromBinary(Input);
                ExportUitextToXml(Output, result);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
                throw;
            }
        }

        private void UitextToBin_Click(object sender, EventArgs e)
        {
            var Input = "XML\\UIText.xml";
            var Output = "BIN\\UIText.bin";
            try
            {
                UIText[] result = ImportUITextsFromXml(Input);
                WriteUItextToBinary(Output, result);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
                throw;
            }
        }

        private void MapMonstersToXml_Click(object sender, EventArgs e)
        {
            var Input = "DATA\\MapMonsterList.bin";
            var Output = "XML\\MapMonsterList.xml";
            try
            {
                MapMonsters[] mapMonsters = ReadMapMonstersFromBinary(Input);
                ExportMapMonstersToXml(mapMonsters, Output);
                Sucess();

            }
            catch (Exception)
            {
                Fail();
                throw;
            }
        }

        private void MapMonsterToBin_Click(object sender, EventArgs e)
        {
            var Output = "BIN\\MapMonsterList.bin";
            var Input = "XML\\MapMonsterList.xml";
            try
            {
                MapMonsters[] mapMonsters = ImportMapMonstersFromXml(Input);
                WriteMapMonstersToBinary(mapMonsters, Output);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
                throw;
            }
        }

        private void MonstersToXml_Click(object sender, EventArgs e)
        {
            var Input = "DATA\\Monster.bin";
            var MonstersOutput = "XML\\Monster.xml";
            var MonstersSkillOutput = "XML\\MonstersSkill.xml";
            var MonstersHitOutput = "XML\\MonsterHit.xml";
            var MonstersSkillTermsOutput = "XML\\MonstersSkillTerms.xml";
            try
            {
                (MonstersInfo.Monsters[], MonstersInfo.MonsterHit[], MonstersInfo.MonsterSkill[], MonstersInfo.MonsterSkillTerms[]) data = MonstersInfo.ReadMonstersFromBinary(Input);
                MonstersInfo.ExportMonsterToXml(data.Item1, MonstersOutput);
                MonstersInfo.ExportMonsterHitToXml(data.Item2, MonstersHitOutput);
                MonstersInfo.ExportMonsterSkillToXml(data.Item3, MonstersSkillOutput);
                MonstersInfo.ExportMonsterSkillTermsToXml(data.Item4, MonstersSkillTermsOutput);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
                throw;
            }
        }

        private void MonstersToBin_Click(object sender, EventArgs e)
        {
            var Output = "BIN\\Monster.bin";
            var MonstersInput = "XML\\Monster.xml";
            var MonstersSkillInput = "XML\\MonstersSkill.xml";
            var MonstersHitInput = "XML\\MonsterHit.xml";
            var MonstersSkillTermsInput = "XML\\MonstersSkillTerms.xml";
            try
            {
                MonstersInfo.Monsters[] monsters = MonstersInfo.ImportMonsterFromXml(MonstersInput);
                MonstersInfo.MonsterSkill[] monsterSkills = MonstersInfo.ImportMonsterSkillFromXml(MonstersSkillInput);
                MonstersInfo.MonsterHit[] monsterHits = MonstersInfo.ImportMonsterHitFromXml(MonstersHitInput);
                MonstersInfo.MonsterSkillTerms[] monsterSkillTerms = MonstersInfo.ImportMonsterSkillTermsFromXml(MonstersSkillTermsInput);
                MonstersInfo.WriteMonstersToBinary(Output, monsters, monsterHits, monsterSkills, monsterSkillTerms);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
                throw;
            }
        }

        private void MapListToXml_Click(object sender, EventArgs e)
        {
            var Input = "DATA\\MapList.bin";
            var Output = "XML\\MapList.xml";
            try
            {
                MapData[] Map = ReadMoApDataFromBinary(Input);
                ExportMapListToXml(Map, Output);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
                throw;
            }

        }

        private void QuestsToXml_Click(object sender, EventArgs e)
        {
            var Input = "DATA\\Quest.bin";
            var Output = "XML\\Quest.xml";
            try
            {
                QuestInfos[] questInfos = ReadQuestFromBinary(Input);
                ExportQuestsToXml(questInfos, Output);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
                throw;
            }
        }

        private void QuestToBin_Click(object sender, EventArgs e)
        {
            var Input = "XML\\Quest.xml";
            var Output = "BIN\\Quest.bin";
            try
            {
                QuestInfos[] questInfos = ImportQuestsFromXml(Input);
                WriteQuestToBinary(Output, questInfos);
                Sucess();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void MapListToBin_Click(object sender, EventArgs e)
        {
            var Output = "BIN\\MapList.bin";
            var Input = "XML\\MapList.xml";
            try
            {
                MapData[] Map = ImportMapListFromXml(Input);
                WriteMapListToBinary(Map, Output);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
                throw;
            }
        }

        private void ExtraExchangeToXml_Click(object sender, EventArgs e)
        {
            var Input = "DATA\\ExtraExchange.bin";
            var Output = "XML\\ExtraExchange.xml";
            try
            {
                ExtraExchangeNPC[] extraExchangeNPC = ExtraExchangesFromBinary(Input);
                ExportExtraExchangeNPCToXml(extraExchangeNPC, Output);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
                throw;
            }
        }

        private void ExtraExchangeToBin_Click(object sender, EventArgs e)
        {
            var Input = "XML\\ExtraExchange.xml";
            var Output = "BIN\\ExtraExchange.bin";
            try
            {
                ExtraExchangeNPC[] extraExchangeNPCs = ImportExtraExchangeNPCsFromXml(Input);
                WriteExtraExchangeToBinary(extraExchangeNPCs, Output);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
                throw;
            }
        }
        private void TacticsToXml_Click(object sender, EventArgs e)
        {
            var Input = "DATA\\Tactics.bin";
            var Output = "XML\\Tactics.xml";
            var TacticsPlainOutput = "XML\\TacticsPlain.xml";
            var TacticsTrancenderOutput = "XML\\TacticsTranscender.xml";
            var TacticsEnchantOutput = "XML\\EnchantClon.xml";

            try
            {
                (Tactics.TDBTactic[] tactics, Tactics.TacticsPlain[] TacticsPlains, Tactics.Transcender[] transcenders, Tactics.EnchantRankInfo[] enchantRankInfos, TranscendDefaultCorrect[] transcendDefaultCorrects, TranscendSameType[] transcendSameTypes) data = ImporTacticsFromBinary(Input);
                ExportTDBTacticToXml(data.tactics, Output);
                ExportTacticsPlainToXml(data.TacticsPlains, TacticsPlainOutput);
                ExportTranscenderToXml(data.transcenders, TacticsTrancenderOutput);
                ExportEnchantRankInfoToXml(data.enchantRankInfos, TacticsEnchantOutput);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
                throw;
            }
        }

        private void TacticsToBin_Click(object sender, EventArgs e)
        {
            var Output = "BIN\\Tactics.bin";
            var TacticsInput = "XML\\Tactics.xml";
            var TacticsPlainInput = "XML\\TacticsPlain.xml";
            var TacticsTrancenderInput = "XML\\TacticsTranscender.xml";

            try
            {
                Tactics.TDBTactic[] tactics = ImportTDBTacticFromXml(TacticsInput);
                Tactics.TacticsPlain[] tacticsPlains = ImportTacticsPlainFromXml(TacticsPlainInput);
                Tactics.Transcender[] transcenders = ImportTranscenderFromXml(TacticsTrancenderInput);
                ExportTacticsToBinary(Output, tactics, tacticsPlains, transcenders);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
                throw;
            }
        }

        private void MapPortalToXml_Click(object sender, EventArgs e)
        {
            var Input = "DATA\\MapPortal.bin";
            var Output = "XML\\MapPortal.xml";
            try
            {
                Portal[] portals = ReadPortalFromBinary(Input);
                ExportPortalToXml(portals, Output);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
                throw;
            }
        }

        private void MapPortalToBin_Click(object sender, EventArgs e)
        {
            var Input = "XML\\MapPortal.xml";
            var Output = "BIN\\MapPortal.bin";
            try
            {
                Portal[] portals = ImportPortalFromXml(Input);
                WritePortalToBinary(portals, Output);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
                throw;
            }
        }

        private void TalkToXml_Click(object sender, EventArgs e)
        {
            var Input = "DATA\\Talk.bin";
            var TalkDigimonOutput = "XML\\TalkDigimon.xml";
            var TalkEventOutput = "XML\\TalkEvent.xml";
            var TalkMessageOutput = "XML\\TalkMessage.xml";
            var TalkTipOutput = "XML\\TalkTip.xml";
            var TalkLoadingTipOutput = "XML\\TalkLoadingTip.xml";

            try
            {
                (TalkDigimon[] talkDigimons, TalkEvent[] talkEvents, TalkMessage[] talkMessages, TalkTip[] talkTips, TalkLoadingTip[] talkLoadingTips) data = ReadTalkFromBinary(Input);
                ExportTalkDigimonToXml(data.talkDigimons, TalkDigimonOutput);
                ExportTalkEventToXml(data.talkEvents, TalkEventOutput);
                ExportTalkMessageToXml(data.talkMessages, TalkMessageOutput);
                ExportTalkTipToXml(data.talkTips, TalkTipOutput);
                ExportTalkLoadingTipToXml(data.talkLoadingTips, TalkLoadingTipOutput);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
                throw;
            }
        }

        private void button26_Click(object sender, EventArgs e)
        {
            var Output = "BIN\\Talk.bin";
            var TalkDigimonInput = "XML\\TalkDigimon.xml";
            var TalkEventInput = "XML\\TalkEvent.xml";
            var TalkMessageInput = "XML\\TalkMessage.xml";
            var TalkTipInput = "XML\\TalkTip.xml";
            var TalkLoadingTipInput = "XML\\TalkLoadingTip.xml";
            try
            {
                TalkDigimon[] talkDigimons = ImportTalkDigimonFromXml(TalkDigimonInput);
                TalkEvent[] talkEvents = ImportTalkEventFromXml(TalkEventInput);
                TalkMessage[] talkMessages = ImportTalkMessageFromXml(TalkMessageInput);
                TalkTip[] talkTips = ImportTalkTipFromXml(TalkTipInput);
                TalkLoadingTip[] talkLoadingTip = ImportTalkLoadingTipFromXml(TalkLoadingTipInput);
                ExportTalkToBinary(talkDigimons, talkEvents, talkMessages, talkTips, talkLoadingTip, Output);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
                throw;
            }
        }

        private void WorldMapToXml_Click(object sender, EventArgs e)
        {
            var Input = "DATA\\WorldMap.bin";
            var WorldMapOutput = "XML\\WorldMap.xml";
            var AreaMapOutput = "XML\\AreaMap.xml";
            try
            {
                (WorldMapInfo[], AreaMapInfo[]) data = ReadWorldMapFromBinary(Input);
                ExportWorldMapInfoToXml(data.Item1, WorldMapOutput);
                ExportAreaMapInfoToXml(data.Item2, AreaMapOutput);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
                throw;
            }
        }

        private void WorlMapToBin_Click(object sender, EventArgs e)
        {
            var Output = "BIN\\WorldMap.bin";
            var WorldMapInput = "XML\\WorldMap.xml";
            var AreaMapInput = "XML\\AreaMap.xml";
            try
            {
                WorldMapInfo[] worldMapInfos = ImportWorldMapInfoFromXml(WorldMapInput);
                AreaMapInfo[] areaMapInfos = ImportAreaMapInfoFromXml(AreaMapInput);
                WriteAreaMapInfoToBinary(worldMapInfos, areaMapInfos, Output);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
                throw;
            }
        }

        private void MapObjectToXml_Click(object sender, EventArgs e)
        {
            var Input = "DATA\\MapObject.bin";
            var Output = "XML\\MapObject.xml";
            try
            {
                MapObject[] objects = ExportMapObjectFromBinary(Input);
                ExportMapObjectToXml(objects, Output);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
                throw;
            }
        }

        private void MapObjectToBin_Click(object sender, EventArgs e)
        {
            var Input = "XML\\MapObject.xml";
            var Output = "BIN\\MapObject.bin";
            try
            {
                MapObject[] objects = ImportMapObjectFromXml(Input);

                WriteMapObjectToBinary(objects, Output);
                Sucess();
            }
            catch (Exception)
            {
                Fail();
                throw;
            }
        }

        private void button31_Click(object sender, EventArgs e)
        {


            var FilePath = "DATA\\DBO_PDungeon_001_PATH.pth";
            Arak.Main(FilePath);
            
        }
        private void CashShopToXml_Click(object sender, EventArgs e)
        {
            var Input = "DATA\\CashShop.bin";
            try
            {
                CashShopToXml(Input);
                Sucess();
            }
            catch (Exception)
            {

                Fail();
            }
        }

        private void CashShopToBin_Click(object sender, EventArgs e)
        {
            try
            {

                var Output = "BIN\\CashShop.bin";

                CashShopMainInformation[] cashShopMain = new CashShopMainInformation[2];
                CashShopTamerInfo[] CashTamerInfo = new CashShopTamerInfo[2];
                CashShopDigimonInfo[] CashDigimonInfo = new CashShopDigimonInfo[2];
                CashShopAvatarInfo[] CashAvatarInfo = new CashShopAvatarInfo[2];
                CashShopPackageInfo[] CashPackageInfo = new CashShopPackageInfo[2];
                CashWebData[] cashWebDatas = new CashWebData[1];

                for (int i = 0; i < 2; i++)
                {
                    CashShopTamerInfo TamerInfo = new();
                    CashShopDigimonInfo DigimonInfo = new();
                    CashShopAvatarInfo avatarInfo = new();
                    CashShopPackageInfo packageInfo = new();
                    CashShopMainInformation cashShopMainInformation = new();
                    if (i == 0)
                    {
                        cashShopMainInformation = ImportCashShopMainInfoFromXml($"XML\\CashShop\\Main\\CashShopMainInformation.xml");
                        TamerInfo = ImportCashTamerInfoFromXml("XML\\CashShop\\TamerInfo\\CashShopTamerInfo.xml", i);
                        DigimonInfo = ImportCashDigimonInfoFromXml("XML\\CashShop\\DigimonInfo\\CashShopDigimonInfo.xml", i);
                        avatarInfo = ImportCashAvatarInfoFromXml("XML\\CashShop\\AvatarInfo\\CashShopAvatarInfo.xml", i);
                        packageInfo = ImportCashPackageInfoFromXml("XML\\CashShop\\PackageInfo\\CashShopPackageInfo.xml", i);

                        CashTamerInfo[i] = TamerInfo;
                        CashDigimonInfo[i] = DigimonInfo;
                        CashAvatarInfo[i] = avatarInfo;
                        CashPackageInfo[i] = packageInfo;
                        cashShopMain[i] = cashShopMainInformation;
                    }
                    else
                    {
                        cashShopMainInformation = ImportCashShopMainInfoFromXml($"XML\\CashShop\\Main{i}\\CashShopMainInformation.xml");
                        TamerInfo = ImportCashTamerInfoFromXml($"XML\\CashShop\\TamerInfo{i}\\CashShopTamerInfo.xml", i);
                        DigimonInfo = ImportCashDigimonInfoFromXml($"XML\\CashShop\\DigimonInfo{i}\\CashShopDigimonInfo.xml", i);
                        avatarInfo = ImportCashAvatarInfoFromXml($"XML\\CashShop\\AvatarInfo{i}\\CashShopAvatarInfo.xml", i);
                        packageInfo = ImportCashPackageInfoFromXml($"XML\\CashShop\\PackageInfo{i}\\CashShopPackageInfo.xml", i);

                        CashTamerInfo[i] = TamerInfo;
                        CashDigimonInfo[i] = DigimonInfo;
                        CashAvatarInfo[i] = avatarInfo;
                        CashPackageInfo[i] = packageInfo;
                        cashShopMain[i] = cashShopMainInformation;
                    }


                }

                cashWebDatas = ImportCashWebDataFromXml("XML\\CashShop\\WebData\\WebData.xml");
                WriteCashShopToBinary(Output, cashShopMain, CashTamerInfo, CashDigimonInfo, CashAvatarInfo, CashPackageInfo, cashWebDatas);
                Sucess();
            }
            catch
            {
                Fail();
            }

        }
    }
}
