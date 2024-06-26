using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace BinXmlConverter.Classes
{
    public class DMBaseInfo
    {
        public BaseInfoCharacterModel characterModel;
        public int Id;
        public ushort Level;
        public long Exp;
        public int Hp;
        public int Ds;
        public int At;
        public ushort De;
        public ushort Ev;
        public ushort Ct;
        public ushort Ht;
        public ushort Ms;
        public ushort Unknow1;
        public ushort Unknow2;
        public static (DMBaseInfo[], DigiBaseInfo[], CsBaseMapInfo[], Jumpbooster[], Party[], Guild[], Limit[], Store[], PaneltyInfo[], EvolutionBaseApply[], DigimonEvoMaxSkillLevel[], ExpansionCondition[] expasion) ExportDMBaseToXml(string xmlPath)
        {

            using (BitReader read = new BitReader(File.Open(xmlPath, FileMode.Open)))
            {

                int count = read.ReadInt();
                DMBaseInfo[] dMBaseInfos = new DMBaseInfo[count + 1];

                for (int i = 1; i < count + 1; i++)
                {
                    DMBaseInfo dmbaseInfo = new DMBaseInfo();
                    dmbaseInfo.Id = read.ReadInt();
                    dmbaseInfo.Level = read.ReadUShort();
                    dmbaseInfo.Unknow1 = read.ReadUShort();
                    dmbaseInfo.Exp = read.ReadLong();
                    dmbaseInfo.Hp = read.ReadInt();
                    dmbaseInfo.Ds = read.ReadInt();
                    dmbaseInfo.Ms = read.ReadUShort();
                    dmbaseInfo.De = read.ReadUShort();
                    dmbaseInfo.Ev = read.ReadUShort();
                    dmbaseInfo.Ct = read.ReadUShort();
                    dmbaseInfo.At = read.ReadInt();
                    dmbaseInfo.Ht = read.ReadUShort();
                    dmbaseInfo.Unknow2 = read.ReadUShort();

                    dMBaseInfos[i] = dmbaseInfo;

                }

                int dcount = read.ReadInt();

                DigiBaseInfo[] digiBaseInfos = new DigiBaseInfo[dcount + 1];
                for (int i = 1; i < dcount + 1; i++)
                {
                    DigiBaseInfo digibaseInfo = new DigiBaseInfo();
                    digibaseInfo.Id = read.ReadInt();
                    digibaseInfo.Level = read.ReadUShort();
                    digibaseInfo.Unknow1 = read.ReadUShort();
                    digibaseInfo.Exp = read.ReadLong();
                    digibaseInfo.Hp = read.ReadInt();
                    digibaseInfo.Ds = read.ReadInt();
                    digibaseInfo.Ms = read.ReadUShort();
                    digibaseInfo.De = read.ReadUShort();
                    digibaseInfo.Ev = read.ReadUShort();
                    digibaseInfo.Ct = read.ReadUShort();
                    digibaseInfo.At = read.ReadInt();
                    digibaseInfo.Ht = read.ReadUShort();
                    digibaseInfo.Unknow2 = read.ReadUShort();

                    digiBaseInfos[i] = digibaseInfo;
                }

                int scount = read.ReadInt();
                CsBaseMapInfo[] baseMapInfos = new CsBaseMapInfo[scount];

                for (int i = 0; i < scount; i++)
                {
                    CsBaseMapInfo csbasemapinfo = new CsBaseMapInfo();
                    csbasemapinfo.s_nMapID = read.ReadInt();
                    csbasemapinfo.s_nShoutSec = read.ReadInt();
                    csbasemapinfo.s_bEnableCheckMacro = read.ReadUShort();
                    csbasemapinfo.unk = read.ReadUShort();

                    baseMapInfos[i] = csbasemapinfo;
                }

                int jcount = read.ReadInt();
                Jumpbooster[] Jumpbooster = new Jumpbooster[jcount];
                for (int j = 0; j < jcount; j++)
                {
                    Jumpbooster jumpbooster = new Jumpbooster();
                    jumpbooster.dwItemID = read.ReadInt();
                    jumpbooster.mapcount = read.ReadInt();

                    for (int a = 0; a < jumpbooster.mapcount; a++)
                    {
                        JumpMaps jump = new();
                        jump.MapId = read.ReadInt();
                        jumpbooster.JumpMaps.Add(jump);
                    }

                    Jumpbooster[j] = jumpbooster;
                }


                Party[] party = new Party[1];
                Party party1 = new();
                party1.distc = read.ReadFloat();
                party[0] = party1;


                int gcount = read.ReadInt();
                Guild[] guild = new Guild[gcount];
                for (int i = 0; i < gcount; i++)
                {
                    Guild guild1 = new();
                    guild1.s_nLevel = read.ReadInt();
                    guild1.s_nFame = read.ReadUInt();
                    guild1.s_nItemNo1 = read.ReadInt();
                    guild1.s_nItemCount1 = read.ReadInt();
                    guild1.s_nItemNo2 = read.ReadInt();
                    guild1.s_nItemCount2 = read.ReadInt();
                    guild1.s_nMasterLevel = read.ReadInt();
                    guild1.s_nNeedPerson = read.ReadInt();
                    guild1.s_nMaxGuildPerson = read.ReadInt();
                    guild1.s_nIncMember = read.ReadInt();
                    guild1.s_nMaxGuild2Master = read.ReadInt();

                    guild[i] = guild1;

                }


                Limit[] limit = new Limit[1];

                Limit limit1 = new();
                limit1.s_nMaxTacticsHouse = read.ReadUShort();
                limit1.s_nMaxWareHouse = read.ReadUShort();
                limit1.s_nUnionStore = read.ReadUShort();
                limit1.s_nMaxShareStash = read.ReadUShort();
                limit1.s_nConsume_XG = read.ReadInt();
                limit1.s_nCharge_XG = read.ReadInt();
                limit[0] = limit1;



                Store[] store = new Store[1];
                Store store1 = new();
                store1.s_fPerson_Charge = read.ReadFloat();
                store1.s_fEmployment_Charge = read.ReadFloat();
                store1.s_fStoreDist = read.ReadFloat();
                int storecount = read.ReadInt();
                for (int x = 0; x < storecount; x++)
                {
                    StoreItems items = new();
                    items.s_nItemID = read.ReadInt();
                    items.s_nDigimonID = read.ReadInt();
                    items.s_fScale = read.ReadFloat();
                    items.s_nSlotCount = read.ReadInt();
                    items.s_szFileName = read.ReadZString(Encoding.Unicode, 64 * 2);

                    store1.StoreItems.Add(items);
                }

                store[0] = store1;


                int pcount = read.ReadInt();
                PaneltyInfo[] paneltyInfos = new PaneltyInfo[pcount];
                for (int i = 0; i < pcount; i++)
                {
                    PaneltyInfo paneltyInfo = new PaneltyInfo();
                    paneltyInfo.s_nPaneltyLevel = read.ReadInt();
                    paneltyInfo.s_nDrop = read.ReadInt();
                    paneltyInfo.s_nExp = read.ReadInt();
                    paneltyInfos[i] = paneltyInfo;
                }

                int evcount = read.ReadInt();
                EvolutionBaseApply[] evolutionBaseApplies = new EvolutionBaseApply[evcount];
                for (int i = 0; i < evcount; i++)
                {
                    EvolutionBaseApply evolutionBaseApply = new();
                    evolutionBaseApply.EvolutionType = read.ReadInt();
                    evolutionBaseApply.NameSize = read.ReadInt();
                    byte[] nameBytes = new byte[evolutionBaseApply.NameSize * 2];
                    for (int x = 0; x < evolutionBaseApply.NameSize * 2; x++)
                    {
                        nameBytes[x] = read.ReadByte();
                    }
                    evolutionBaseApply.EvolutionName = Encoding.Unicode.GetString(nameBytes);
                    evolutionBaseApply.EvolutionApplyValue = read.ReadInt();
                    evolutionBaseApplies[i] = evolutionBaseApply;
                }

                int SkillMaxCount = read.ReadInt();
                DigimonEvoMaxSkillLevel[] digimonEvoMaxSkillLevel = new DigimonEvoMaxSkillLevel[SkillMaxCount];
                for (int i = 0; i < SkillMaxCount; i++)
                {
                    DigimonEvoMaxSkillLevel digimonEvoMaxSkillLevel1 = new();
                    digimonEvoMaxSkillLevel1.nEvoType = read.ReadInt();
                    digimonEvoMaxSkillLevel1.s_SkillExpStartLv = read.ReadInt();
                    digimonEvoMaxSkillLevel1.nSubCount = read.ReadInt();
                    digimonEvoMaxSkillLevel1.s_SkillMaxLvs = new int[digimonEvoMaxSkillLevel1.nSubCount];
                    for (int x = 0; x < digimonEvoMaxSkillLevel1.nSubCount; x++)
                    {
                        digimonEvoMaxSkillLevel1.s_SkillMaxLvs[x] = read.ReadInt();
                    }
                    digimonEvoMaxSkillLevel[i] = digimonEvoMaxSkillLevel1;
                }

                int ExpasionCount = read.ReadInt();

                ExpansionCondition[] expasion = new ExpansionCondition[ExpasionCount];

                for (int i = 0; i < ExpasionCount; i++)
                {
                    ExpansionCondition exp = new();
                    exp.nOpenItemSubType = read.ReadInt();
                    exp.nExpansionRank = read.ReadInt();
                    int SubCount = read.ReadInt();
                    exp.nEvoType = new int[SubCount];
                    for (int z = 0; z < SubCount; z++)
                    {
                        exp.nEvoType[z] = read.ReadInt();
                    }
                    expasion[i] = exp;
                }
                return (dMBaseInfos, digiBaseInfos, baseMapInfos, Jumpbooster, party, guild, limit, store, paneltyInfos, evolutionBaseApplies, digimonEvoMaxSkillLevel, expasion);
            }

        }
        public static void ExportTamerBaseToXml(string xmlPath, DMBaseInfo[] dMBaseInfos)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(xmlPath, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("CharacterList");

                foreach (DMBaseInfo dmbaseInfo in dMBaseInfos)
                {
                    if (dmbaseInfo == null)
                        continue;

                    writer.WriteStartElement("DMBaseInfo");
                    WriteElement(writer, "Id", dmbaseInfo.Id.ToString());
                    WriteElement(writer, "Level", dmbaseInfo.Level.ToString());
                    WriteElement(writer, "Unknow1", dmbaseInfo.Unknow1.ToString());
                    WriteElement(writer, "Exp", dmbaseInfo.Exp.ToString());
                    WriteElement(writer, "Hp", dmbaseInfo.Hp.ToString());
                    WriteElement(writer, "Ds", dmbaseInfo.Ds.ToString());
                    WriteElement(writer, "Ms", dmbaseInfo.Ms.ToString());
                    WriteElement(writer, "De", dmbaseInfo.De.ToString());
                    WriteElement(writer, "Ev", dmbaseInfo.Ev.ToString());
                    WriteElement(writer, "Ct", dmbaseInfo.Ct.ToString());
                    WriteElement(writer, "At", dmbaseInfo.At.ToString());
                    WriteElement(writer, "Ht", dmbaseInfo.Ht.ToString());
                    WriteElement(writer, "Unknow2", dmbaseInfo.Unknow2.ToString());
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            void WriteElement(XmlWriter writer, string elementName, string elementValue)
            {
                writer.WriteStartElement(elementName);
                writer.WriteString(elementValue);
                writer.WriteEndElement();
            }

            return;
        }
        public static void ExportDigimonBaseToXml(string xmlPath, DigiBaseInfo[] dMBaseInfos)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(xmlPath, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("DigimonList");

                foreach (DigiBaseInfo dmbaseInfo in dMBaseInfos)
                {
                    if (dmbaseInfo == null)
                        continue;

                    writer.WriteStartElement("DigiBaseInfo");
                    WriteElement(writer, "Id", dmbaseInfo.Id.ToString());
                    WriteElement(writer, "Level", dmbaseInfo.Level.ToString());
                    WriteElement(writer, "Unknow1", dmbaseInfo.Unknow1.ToString());
                    WriteElement(writer, "Exp", dmbaseInfo.Exp.ToString());
                    WriteElement(writer, "Hp", dmbaseInfo.Hp.ToString());
                    WriteElement(writer, "Ds", dmbaseInfo.Ds.ToString());
                    WriteElement(writer, "Ms", dmbaseInfo.Ms.ToString());
                    WriteElement(writer, "De", dmbaseInfo.De.ToString());
                    WriteElement(writer, "Ev", dmbaseInfo.Ev.ToString());
                    WriteElement(writer, "Ct", dmbaseInfo.Ct.ToString());
                    WriteElement(writer, "At", dmbaseInfo.At.ToString());
                    WriteElement(writer, "Ht", dmbaseInfo.Ht.ToString());
                    WriteElement(writer, "Unknow2", dmbaseInfo.Unknow2.ToString());
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            void WriteElement(XmlWriter writer, string elementName, string elementValue)
            {
                writer.WriteStartElement(elementName);
                writer.WriteString(elementValue);
                writer.WriteEndElement();
            }

            return;
        }
        public static void ExportCsMapBaseToXml(CsBaseMapInfo[] csBaseMapInfos, string filePath)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(filePath, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("CsBaseMapInfoList");

                foreach (CsBaseMapInfo csBaseMapInfo in csBaseMapInfos)
                {
                    writer.WriteStartElement("CsBaseMapInfo");
                    WriteElement(writer, "s_nMapID", csBaseMapInfo.s_nMapID.ToString());
                    WriteElement(writer, "s_nShoutSec", csBaseMapInfo.s_nShoutSec.ToString());
                    WriteElement(writer, "s_bEnableCheckMacro", csBaseMapInfo.s_bEnableCheckMacro.ToString());
                    WriteElement(writer, "unk", csBaseMapInfo.unk.ToString());
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            void WriteElement(XmlWriter writer, string elementName, string elementValue)
            {
                writer.WriteStartElement(elementName);
                writer.WriteString(elementValue);
                writer.WriteEndElement();
            }

        }
        public static void ExportJumpBoosterInfoFromXml(Jumpbooster[] jumpboosters, string filePath)
        {
            XElement jumpboosterList = new XElement("JumpboosterList");

            foreach (Jumpbooster jumpbooster in jumpboosters)
            {
                XElement jumpboosterElement = new XElement("Jumpbooster");
                jumpboosterElement.Add(new XElement("dwItemID", jumpbooster.dwItemID));
                jumpboosterElement.Add(new XElement("mapcount", jumpbooster.mapcount));

                XElement dwMapIDsElement = new XElement("dwMapIDs");
                foreach (JumpMaps jumpMap in jumpbooster.JumpMaps)
                {
                    dwMapIDsElement.Add(new XElement("dwMapID", jumpMap.MapId));
                }
                jumpboosterElement.Add(dwMapIDsElement);

                jumpboosterList.Add(jumpboosterElement);
            }

            XDocument xmlDoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), jumpboosterList);
            xmlDoc.Save(filePath);
        }
        public static void ExportGuildToXml(Guild[] guilds, string filePath)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(filePath, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("GuildData");

                foreach (Guild guild in guilds)
                {
                    writer.WriteStartElement("Guild");
                    WriteElement(writer, "s_nLevel", guild.s_nLevel.ToString());
                    WriteElement(writer, "s_nFame", guild.s_nFame.ToString());
                    WriteElement(writer, "s_nItemNo1", guild.s_nItemNo1.ToString());
                    WriteElement(writer, "s_nItemCount1", guild.s_nItemCount1.ToString());
                    WriteElement(writer, "s_nItemNo2", guild.s_nItemNo2.ToString());
                    WriteElement(writer, "s_nItemCount2", guild.s_nItemCount2.ToString());
                    WriteElement(writer, "s_nMasterLevel", guild.s_nMasterLevel.ToString());
                    WriteElement(writer, "s_nNeedPerson", guild.s_nNeedPerson.ToString());
                    WriteElement(writer, "s_nMaxGuildPerson", guild.s_nMaxGuildPerson.ToString());
                    WriteElement(writer, "s_nIncMember", guild.s_nIncMember.ToString());
                    WriteElement(writer, "s_nMaxGuild2Master", guild.s_nMaxGuild2Master.ToString());

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            void WriteElement(XmlWriter writer, string elementName, string elementValue)
            {
                writer.WriteElementString(elementName, elementValue);
            }
        }
        public static void ExportLimitToXml(Limit[] limits, string filePath)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(filePath, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Limits");

                foreach (Limit limit in limits)
                {
                    writer.WriteStartElement("Limit");
                    WriteElement(writer, "s_nMaxTacticsHouse", limit.s_nMaxTacticsHouse.ToString());
                    WriteElement(writer, "s_nMaxWareHouse", limit.s_nMaxWareHouse.ToString());
                    WriteElement(writer, "s_nUnionStore", limit.s_nUnionStore.ToString());
                    WriteElement(writer, "s_nMaxShareStash", limit.s_nMaxShareStash.ToString());
                    WriteElement(writer, "s_nConsume_XG", limit.s_nConsume_XG.ToString());
                    WriteElement(writer, "s_nCharge_XG", limit.s_nCharge_XG.ToString());

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            void WriteElement(XmlWriter writer, string elementName, string elementValue)
            {
                writer.WriteElementString(elementName, elementValue);
            }
        }
        public static void ExportStoreToXml(Store[] stores, string filePath)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(filePath, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Stores");

                foreach (Store store in stores)
                {
                    writer.WriteStartElement("Store");

                    WriteElement(writer, "s_fPerson_Charge", store.s_fPerson_Charge.ToString());
                    WriteElement(writer, "s_fEmployment_Charge", store.s_fEmployment_Charge.ToString());
                    WriteElement(writer, "s_fStoreDist", store.s_fStoreDist.ToString());

                    writer.WriteStartElement("StoreItems");

                    foreach (StoreItems storeItem in store.StoreItems)
                    {
                        writer.WriteStartElement("StoreItem");
                        WriteElement(writer, "s_nItemID", storeItem.s_nItemID.ToString());
                        WriteElement(writer, "s_nDigimonID", storeItem.s_nDigimonID.ToString());
                        WriteElement(writer, "s_fScale", storeItem.s_fScale.ToString());
                        WriteElement(writer, "s_nSlotCount", storeItem.s_nSlotCount.ToString());
                        WriteElement(writer, "s_szFileName", storeItem.s_szFileName);

                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            void WriteElement(XmlWriter writer, string elementName, string elementValue)
            {
                writer.WriteElementString(elementName, elementValue);
            }
        }
        public static void ExportPaneltyInfoToXml(PaneltyInfo[] paneltyInfos, string filePath)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(filePath, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("PaneltyInfos");

                foreach (PaneltyInfo paneltyInfo in paneltyInfos)
                {
                    writer.WriteStartElement("PaneltyInfo");

                    WriteElement(writer, "s_nPaneltyLevel", paneltyInfo.s_nPaneltyLevel.ToString());
                    WriteElement(writer, "s_nExp", paneltyInfo.s_nExp.ToString());
                    WriteElement(writer, "s_nDrop", paneltyInfo.s_nDrop.ToString());

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            void WriteElement(XmlWriter writer, string elementName, string elementValue)
            {
                writer.WriteElementString(elementName, elementValue);
            }
        }
        public static void ExportEvolutionBaseToXml(EvolutionBaseApply[] evolutionBaseApplies, string filePath)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(filePath, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("EvolutionBaseApplies");

                foreach (EvolutionBaseApply evolutionBaseApply in evolutionBaseApplies)
                {
                    writer.WriteStartElement("EvolutionBaseApply");

                    WriteElement(writer, "EvolutionType", evolutionBaseApply.EvolutionType.ToString());
                    WriteElement(writer, "EvolutionName", evolutionBaseApply.EvolutionName);
                    WriteElement(writer, "EvolutionApplyValue", evolutionBaseApply.EvolutionApplyValue.ToString());
                    WriteElement(writer, "NameSize", evolutionBaseApply.NameSize.ToString());

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            void WriteElement(XmlWriter writer, string elementName, string elementValue)
            {
                writer.WriteElementString(elementName, elementValue);
            }
        }
        public static void ExportDigimonEvoMaxSkillLevelToXml(DigimonEvoMaxSkillLevel[] digimonEvoMaxSkillLevels, string filePath)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(filePath, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("DigimonEvoMaxSkillLevels");

                foreach (DigimonEvoMaxSkillLevel digimonEvoMaxSkillLevel in digimonEvoMaxSkillLevels)
                {
                    writer.WriteStartElement("DigimonEvoMaxSkillLevel");

                    WriteElement(writer, "nEvoType", digimonEvoMaxSkillLevel.nEvoType.ToString());
                    WriteElement(writer, "s_SkillExpStartLv", digimonEvoMaxSkillLevel.s_SkillExpStartLv.ToString());
                    WriteElement(writer, "nSubCount", digimonEvoMaxSkillLevel.nSubCount.ToString());

                    writer.WriteStartElement("s_SkillMaxLvs");
                    foreach (int skillMaxLv in digimonEvoMaxSkillLevel.s_SkillMaxLvs)
                    {
                        WriteElement(writer, "SkillMaxLv", skillMaxLv.ToString());
                    }
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            void WriteElement(XmlWriter writer, string elementName, string elementValue)
            {
                writer.WriteElementString(elementName, elementValue);
            }
        }
        public static void ExportPartyToXml(Party[] parties, string filePath)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(filePath, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Parties");

                foreach (Party party in parties)
                {
                    writer.WriteStartElement("Party");
                    WriteElement(writer, "distc", party.distc.ToString());
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            void WriteElement(XmlWriter writer, string elementName, string elementValue)
            {
                writer.WriteElementString(elementName, elementValue);
            }
        }
        public static void ExportExpansionConditionToXml(string filePath, ExpansionCondition[] expansionConditions)
        {
            var xml = new XElement("ExpansionConditions",
                expansionConditions.Select(ec =>
                    new XElement("ExpansionCondition",
                        new XElement("nOpenItemSubType", ec.nOpenItemSubType),
                        new XElement("nExpansionRank", ec.nExpansionRank),
                        new XElement("nEvoType", ec.nEvoType.Select(e => new XElement("Type", e)))
                    )
                )
            );

            xml.Save(filePath);
        }
        public static DMBaseInfo[] ImportTamerBaseFromXml(string filePath)
        {
            List<DMBaseInfo> dMBaseInfos = new List<DMBaseInfo>();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;

            using (XmlReader reader = XmlReader.Create(filePath, settings))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "DMBaseInfo")
                    {
                        DMBaseInfo dmbaseInfo = new DMBaseInfo();

                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                string propertyName = reader.Name;
                                reader.Read();

                                switch (propertyName)
                                {
                                    case "Id":
                                        dmbaseInfo.Id = int.Parse(reader.Value);
                                        break;
                                    case "Level":
                                        dmbaseInfo.Level = ushort.Parse(reader.Value);
                                        break;
                                    case "Unknow1":
                                        dmbaseInfo.Unknow1 = ushort.Parse(reader.Value);
                                        break;
                                    case "Exp":
                                        dmbaseInfo.Exp = long.Parse(reader.Value);
                                        break;
                                    case "Hp":
                                        dmbaseInfo.Hp = int.Parse(reader.Value);
                                        break;
                                    case "Ds":
                                        dmbaseInfo.Ds = int.Parse(reader.Value);
                                        break;
                                    case "Ms":
                                        dmbaseInfo.Ms = ushort.Parse(reader.Value);
                                        break;
                                    case "De":
                                        dmbaseInfo.De = ushort.Parse(reader.Value);
                                        break;
                                    case "Ev":
                                        dmbaseInfo.Ev = ushort.Parse(reader.Value);
                                        break;
                                    case "Ct":
                                        dmbaseInfo.Ct = ushort.Parse(reader.Value);
                                        break;
                                    case "At":
                                        dmbaseInfo.At = int.Parse(reader.Value);
                                        break;
                                    case "Ht":
                                        dmbaseInfo.Ht = ushort.Parse(reader.Value);
                                        break;
                                    case "Unknow2":
                                        dmbaseInfo.Unknow2 = ushort.Parse(reader.Value);
                                        break;
                                }
                            }
                            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "DMBaseInfo")
                            {
                                dMBaseInfos.Add(dmbaseInfo);
                                break;
                            }
                        }
                    }
                }
            }

            return dMBaseInfos.ToArray();
        }
        public static DigiBaseInfo[] ImportDigimonBaseFromXml(string filePath)
        {
            List<DigiBaseInfo> dMBaseInfos = new List<DigiBaseInfo>();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;

            using (XmlReader reader = XmlReader.Create(filePath, settings))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "DigiBaseInfo")
                    {
                        DigiBaseInfo dmbaseInfo = new DigiBaseInfo();

                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                string propertyName = reader.Name;
                                reader.Read();

                                switch (propertyName)
                                {
                                    case "Id":
                                        dmbaseInfo.Id = int.Parse(reader.Value);
                                        break;
                                    case "Level":
                                        dmbaseInfo.Level = ushort.Parse(reader.Value);
                                        break;
                                    case "Unknow1":
                                        dmbaseInfo.Unknow1 = ushort.Parse(reader.Value);
                                        break;
                                    case "Exp":
                                        dmbaseInfo.Exp = long.Parse(reader.Value);
                                        break;
                                    case "Hp":
                                        dmbaseInfo.Hp = int.Parse(reader.Value);
                                        break;
                                    case "Ds":
                                        dmbaseInfo.Ds = int.Parse(reader.Value);
                                        break;
                                    case "Ms":
                                        dmbaseInfo.Ms = ushort.Parse(reader.Value);
                                        break;
                                    case "De":
                                        dmbaseInfo.De = ushort.Parse(reader.Value);
                                        break;
                                    case "Ev":
                                        dmbaseInfo.Ev = ushort.Parse(reader.Value);
                                        break;
                                    case "Ct":
                                        dmbaseInfo.Ct = ushort.Parse(reader.Value);
                                        break;
                                    case "At":
                                        dmbaseInfo.At = int.Parse(reader.Value);
                                        break;
                                    case "Ht":
                                        dmbaseInfo.Ht = ushort.Parse(reader.Value);
                                        break;
                                    case "Unknow2":
                                        dmbaseInfo.Unknow2 = ushort.Parse(reader.Value);
                                        break;
                                }
                            }
                            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "DigiBaseInfo")
                            {
                                dMBaseInfos.Add(dmbaseInfo);
                                break;
                            }
                        }
                    }
                }
            }

            return dMBaseInfos.ToArray();
        }
        public static CsBaseMapInfo[] ImportCsBaseMapInfoFromXml(string filePath)
        {
            List<CsBaseMapInfo> csBaseMapInfos = new List<CsBaseMapInfo>();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;

            using (XmlReader reader = XmlReader.Create(filePath, settings))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "CsBaseMapInfo")
                    {
                        CsBaseMapInfo csBaseMapInfo = new CsBaseMapInfo();

                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                string propertyName = reader.Name;
                                reader.Read();

                                switch (propertyName)
                                {
                                    case "s_nMapID":
                                        csBaseMapInfo.s_nMapID = int.Parse(reader.Value);
                                        break;
                                    case "s_nShoutSec":
                                        csBaseMapInfo.s_nShoutSec = int.Parse(reader.Value);
                                        break;
                                    case "s_bEnableCheckMacro":
                                        csBaseMapInfo.s_bEnableCheckMacro = ushort.Parse(reader.Value);
                                        break;
                                    case "unk":
                                        csBaseMapInfo.unk = ushort.Parse(reader.Value);
                                        break;
                                }
                            }
                            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "CsBaseMapInfo")
                            {
                                csBaseMapInfos.Add(csBaseMapInfo);
                                break;
                            }
                        }
                    }
                }
            }

            return csBaseMapInfos.ToArray();
        }
        public static Jumpbooster[] ImportJumpBoosterFromXml(string filePath)
        {
            List<Jumpbooster> jumpboosters = new List<Jumpbooster>();

            XElement root = XElement.Load(filePath);
            foreach (XElement jumpboosterElement in root.Elements("Jumpbooster"))
            {
                Jumpbooster jumpbooster = new Jumpbooster();
                List<int> dwMapIDs = new List<int>();

                jumpbooster.dwItemID = int.Parse(jumpboosterElement.Element("dwItemID").Value);
                jumpbooster.mapcount = int.Parse(jumpboosterElement.Element("mapcount").Value);

                foreach (XElement dwMapIDElement in jumpboosterElement.Element("dwMapIDs").Elements("dwMapID"))
                {
                    int dwMapID = int.Parse(dwMapIDElement.Value);
                    dwMapIDs.Add(dwMapID);
                }

                jumpbooster.dwMapID = dwMapIDs.ToArray();
                jumpboosters.Add(jumpbooster);
            }

            return jumpboosters.ToArray();
        }
        public static Guild[] ImportGuildFromXml(string filePath)
        {
            List<Guild> guilds = new List<Guild>();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;

            using (XmlReader reader = XmlReader.Create(filePath, settings))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Guild")
                    {
                        Guild guild = new Guild();

                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                string propertyName = reader.Name;
                                reader.Read();

                                switch (propertyName)
                                {
                                    case "s_nLevel":
                                        guild.s_nLevel = int.Parse(reader.Value);
                                        break;
                                    case "s_nFame":
                                        guild.s_nFame = uint.Parse(reader.Value);
                                        break;
                                    case "s_nItemNo1":
                                        guild.s_nItemNo1 = int.Parse(reader.Value);
                                        break;
                                    case "s_nItemCount1":
                                        guild.s_nItemCount1 = int.Parse(reader.Value);
                                        break;
                                    case "s_nItemNo2":
                                        guild.s_nItemNo2 = int.Parse(reader.Value);
                                        break;
                                    case "s_nItemCount2":
                                        guild.s_nItemCount2 = int.Parse(reader.Value);
                                        break;
                                    case "s_nMasterLevel":
                                        guild.s_nMasterLevel = int.Parse(reader.Value);
                                        break;
                                    case "s_nNeedPerson":
                                        guild.s_nNeedPerson = int.Parse(reader.Value);
                                        break;
                                    case "s_nMaxGuildPerson":
                                        guild.s_nMaxGuildPerson = int.Parse(reader.Value);
                                        break;
                                    case "s_nIncMember":
                                        guild.s_nIncMember = int.Parse(reader.Value);
                                        break;
                                    case "s_nMaxGuild2Master":
                                        guild.s_nMaxGuild2Master = int.Parse(reader.Value);
                                        break;
                                }
                            }
                            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Guild")
                            {
                                guilds.Add(guild);
                                break;
                            }
                        }
                    }
                }
            }

            return guilds.ToArray();
        }
        public static Limit[] ImportLimitFromXml(string filePath)
        {
            List<Limit> limits = new List<Limit>();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;

            using (XmlReader reader = XmlReader.Create(filePath, settings))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Limit")
                    {
                        Limit limit = new Limit();

                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                string propertyName = reader.Name;
                                reader.Read();

                                switch (propertyName)
                                {
                                    case "s_nMaxTacticsHouse":
                                        limit.s_nMaxTacticsHouse = ushort.Parse(reader.Value);
                                        break;
                                    case "s_nMaxWareHouse":
                                        limit.s_nMaxWareHouse = ushort.Parse(reader.Value);
                                        break;
                                    case "s_nUnionStore":
                                        limit.s_nUnionStore = ushort.Parse(reader.Value);
                                        break;
                                    case "s_nMaxShareStash":
                                        limit.s_nMaxShareStash = ushort.Parse(reader.Value);
                                        break;
                                    case "s_nConsume_XG":
                                        limit.s_nConsume_XG = int.Parse(reader.Value);
                                        break;
                                    case "s_nCharge_XG":
                                        limit.s_nCharge_XG = int.Parse(reader.Value);
                                        break;
                                }
                            }
                            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Limit")
                            {
                                limits.Add(limit);
                                break;
                            }
                        }
                    }
                }
            }

            return limits.ToArray();
        }
        public static Store[] ImportStoreFromXml(string filePath)
        {
            List<Store> stores = new List<Store>();

            XDocument document = XDocument.Load(filePath);

            foreach (XElement storeElement in document.Descendants("Store"))
            {
                Store store = new Store();

                foreach (XElement propertyElement in storeElement.Elements())
                {
                    switch (propertyElement.Name.LocalName)
                    {
                        case "s_fPerson_Charge":
                            store.s_fPerson_Charge = float.Parse(propertyElement.Value);
                            break;
                        case "s_fEmployment_Charge":
                            store.s_fEmployment_Charge = float.Parse(propertyElement.Value);
                            break;
                        case "s_fStoreDist":
                            store.s_fStoreDist = float.Parse(propertyElement.Value);
                            break;
                        case "StoreItems":
                            foreach (XElement storeItemElement in propertyElement.Elements("StoreItem"))
                            {
                                StoreItems storeItem = new StoreItems();

                                foreach (XElement itemPropertyElement in storeItemElement.Elements())
                                {
                                    switch (itemPropertyElement.Name.LocalName)
                                    {
                                        case "s_nItemID":
                                            storeItem.s_nItemID = int.Parse(itemPropertyElement.Value);
                                            break;
                                        case "s_nDigimonID":
                                            storeItem.s_nDigimonID = int.Parse(itemPropertyElement.Value);
                                            break;
                                        case "s_fScale":
                                            storeItem.s_fScale = float.Parse(itemPropertyElement.Value.Replace(",", "."));
                                            break;
                                        case "s_nSlotCount":
                                            storeItem.s_nSlotCount = int.Parse(itemPropertyElement.Value);
                                            break;
                                        case "s_szFileName":
                                            storeItem.s_szFileName = itemPropertyElement.Value;
                                            break;
                                    }
                                }

                                store.StoreItems.Add(storeItem);
                            }
                            break;
                    }
                }

                stores.Add(store);
            }

            return stores.ToArray();

        }
        public static PaneltyInfo[] ImportPaneltyInfoFromXml(string filePath)
        {
            List<PaneltyInfo> paneltyInfos = new List<PaneltyInfo>();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;

            using (XmlReader reader = XmlReader.Create(filePath, settings))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "PaneltyInfo")
                    {
                        PaneltyInfo paneltyInfo = new PaneltyInfo();

                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                string propertyName = reader.Name;
                                reader.Read();

                                switch (propertyName)
                                {
                                    case "s_nPaneltyLevel":
                                        paneltyInfo.s_nPaneltyLevel = int.Parse(reader.Value);
                                        break;
                                    case "s_nExp":
                                        paneltyInfo.s_nExp = int.Parse(reader.Value);
                                        break;
                                    case "s_nDrop":
                                        paneltyInfo.s_nDrop = int.Parse(reader.Value);
                                        break;
                                }
                            }
                            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "PaneltyInfo")
                            {
                                paneltyInfos.Add(paneltyInfo);
                                break;
                            }
                        }
                    }
                }
            }

            return paneltyInfos.ToArray();
        }
        public static EvolutionBaseApply[] ImportEvolutionBaseApplyFromXml(string filePath)
        {
            List<EvolutionBaseApply> evolutionBaseApplies = new List<EvolutionBaseApply>();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;

            using (XmlReader reader = XmlReader.Create(filePath, settings))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "EvolutionBaseApply")
                    {
                        EvolutionBaseApply evolutionBaseApply = new EvolutionBaseApply();

                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                string propertyName = reader.Name;
                                reader.Read();

                                switch (propertyName)
                                {
                                    case "EvolutionType":
                                        evolutionBaseApply.EvolutionType = int.Parse(reader.Value);
                                        break;
                                    case "EvolutionName":
                                        evolutionBaseApply.EvolutionName = reader.Value;
                                        break;
                                    case "EvolutionApplyValue":
                                        evolutionBaseApply.EvolutionApplyValue = int.Parse(reader.Value);
                                        break;
                                    case "NameSize":
                                        evolutionBaseApply.NameSize = int.Parse(reader.Value);
                                        break;
                                }
                            }
                            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "EvolutionBaseApply")
                            {
                                evolutionBaseApplies.Add(evolutionBaseApply);
                                break;
                            }
                        }
                    }
                }
            }

            return evolutionBaseApplies.ToArray();
        }
        public static DigimonEvoMaxSkillLevel[] ImportDigimonEvoMaxSkillLevelFromXml(string filePath)
        {
            List<DigimonEvoMaxSkillLevel> digimonEvoMaxSkillLevels = new List<DigimonEvoMaxSkillLevel>();

            XDocument doc = XDocument.Load(filePath);
            XElement rootElement = doc.Root;

            if (rootElement != null)
            {
                foreach (XElement element in rootElement.Elements("DigimonEvoMaxSkillLevel"))
                {
                    DigimonEvoMaxSkillLevel digimonEvoMaxSkillLevel = new DigimonEvoMaxSkillLevel();

                    foreach (XElement subElement in element.Elements())
                    {
                        switch (subElement.Name.LocalName)
                        {
                            case "nEvoType":
                                digimonEvoMaxSkillLevel.nEvoType = int.Parse(subElement.Value);
                                break;
                            case "s_SkillExpStartLv":
                                digimonEvoMaxSkillLevel.s_SkillExpStartLv = int.Parse(subElement.Value);
                                break;
                            case "nSubCount":
                                digimonEvoMaxSkillLevel.nSubCount = int.Parse(subElement.Value);
                                break;
                            case "s_SkillMaxLvs":
                                digimonEvoMaxSkillLevel.s_SkillMaxLvs = subElement.Elements("SkillMaxLv")
                                    .Select(x => int.Parse(x.Value))
                                    .ToArray();
                                break;
                        }
                    }

                    digimonEvoMaxSkillLevels.Add(digimonEvoMaxSkillLevel);
                }
            }

            return digimonEvoMaxSkillLevels.ToArray();

        }
        public static Party[] ImportPartyFromXml(string filePath)
        {
            List<Party> parties = new List<Party>();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;

            using (XmlReader reader = XmlReader.Create(filePath, settings))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Party")
                    {
                        Party party = new Party();

                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                string propertyName = reader.Name;
                                reader.Read();

                                switch (propertyName)
                                {
                                    case "distc":
                                        party.distc = float.Parse(reader.Value);
                                        break;
                                }
                            }
                            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Party")
                            {
                                parties.Add(party);
                                break;
                            }
                        }
                    }
                }
            }

            return parties.ToArray();
        }
        public static ExpansionCondition[] ImportExpansionConditionFromXml(string filePath)
        {
            var xml = XDocument.Load(filePath);

            var expansionConditions = xml.Root.Elements("ExpansionCondition").Select(ec =>
                new ExpansionCondition
                {
                    nOpenItemSubType = (int)ec.Element("nOpenItemSubType"),
                    nExpansionRank = (int)ec.Element("nExpansionRank"),
                    nEvoType = ec.Element("nEvoType").Elements("Type").Select(e => (int)e).ToArray()
                }
            ).ToArray();

            return expansionConditions;
        }
        public static void ExportDMBaseToBinary(string outputFile, DMBaseInfo[] Tamerbase, DigiBaseInfo[] DigiBase, CsBaseMapInfo[] BaseMap, Jumpbooster[] jumpboosters, Party[] party, Guild[] guild, Limit[] limit, Store[] stores, PaneltyInfo[] paneltyInfos, EvolutionBaseApply[] evolutionBaseApplies, DigimonEvoMaxSkillLevel[] evoMaxSkillLevels, ExpansionCondition[] expansionConditions)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(outputFile, FileMode.Create)))
            {
                writer.Write(Tamerbase.Length);
                foreach (DMBaseInfo dmbaseInfo in Tamerbase)
                {

                    writer.Write(dmbaseInfo.Id);
                    writer.Write(dmbaseInfo.Level);
                    writer.Write(dmbaseInfo.Unknow1);
                    writer.Write(dmbaseInfo.Exp);
                    writer.Write(dmbaseInfo.Hp);
                    writer.Write(dmbaseInfo.Ds);
                    writer.Write(dmbaseInfo.Ms);
                    writer.Write(dmbaseInfo.De);
                    writer.Write(dmbaseInfo.Ev);
                    writer.Write(dmbaseInfo.Ct);
                    writer.Write(dmbaseInfo.At);
                    writer.Write(dmbaseInfo.Ht);
                    writer.Write(dmbaseInfo.Unknow2);

                }

                writer.Write(DigiBase.Length);
                foreach (DigiBaseInfo dmbaseInfo in DigiBase)
                {

                    writer.Write(dmbaseInfo.Id);
                    writer.Write(dmbaseInfo.Level);
                    writer.Write(dmbaseInfo.Unknow1);
                    writer.Write(dmbaseInfo.Exp);
                    writer.Write(dmbaseInfo.Hp);
                    writer.Write(dmbaseInfo.Ds);
                    writer.Write(dmbaseInfo.Ms);
                    writer.Write(dmbaseInfo.De);
                    writer.Write(dmbaseInfo.Ev);
                    writer.Write(dmbaseInfo.Ct);
                    writer.Write(dmbaseInfo.At);
                    writer.Write(dmbaseInfo.Ht);
                    writer.Write(dmbaseInfo.Unknow2);

                }

                writer.Write(BaseMap.Length);
                foreach (CsBaseMapInfo csbasemapinfo in BaseMap)
                {
                    writer.Write(csbasemapinfo.s_nMapID);
                    writer.Write(csbasemapinfo.s_nShoutSec);
                    writer.Write(csbasemapinfo.s_bEnableCheckMacro);
                    writer.Write(csbasemapinfo.unk);
                }

                writer.Write(jumpboosters.Length);
                foreach (Jumpbooster jumpbooster in jumpboosters)
                {
                    writer.Write(jumpbooster.dwItemID);
                    writer.Write(jumpbooster.dwMapID.Length);

                    for (int a = 0; a < jumpbooster.dwMapID.Length; a++)
                    {
                        writer.Write(jumpbooster.dwMapID[a]);
                    }
                }

                writer.Write(party[0].distc);


                writer.Write(guild.Length);
                foreach (Guild guild1 in guild)
                {
                    writer.Write(guild1.s_nLevel);
                    writer.Write(guild1.s_nFame);
                    writer.Write(guild1.s_nItemNo1);
                    writer.Write(guild1.s_nItemCount1);
                    writer.Write(guild1.s_nItemNo2);
                    writer.Write(guild1.s_nItemCount2);
                    writer.Write(guild1.s_nMasterLevel);
                    writer.Write(guild1.s_nNeedPerson);
                    writer.Write(guild1.s_nMaxGuildPerson);
                    writer.Write(guild1.s_nIncMember);
                    writer.Write(guild1.s_nMaxGuild2Master);
                }

                writer.Write(limit[0].s_nMaxTacticsHouse);
                writer.Write(limit[0].s_nMaxWareHouse);
                writer.Write(limit[0].s_nUnionStore);
                writer.Write(limit[0].s_nMaxShareStash);
                writer.Write(limit[0].s_nConsume_XG);
                writer.Write(limit[0].s_nCharge_XG);

                writer.Write(stores[0].s_fPerson_Charge);
                writer.Write(stores[0].s_fEmployment_Charge);
                writer.Write(stores[0].s_fStoreDist);

                writer.Write(stores[0].StoreItems.Count);
                foreach (StoreItems items in stores[0].StoreItems)
                {

                    writer.Write(items.s_nItemID);
                    writer.Write(items.s_nDigimonID);
                    writer.Write(items.s_fScale);
                    writer.Write(items.s_nSlotCount);

                    byte[] stringBytes = Encoding.Unicode.GetBytes(items.s_szFileName);

                    // Criar um array de bytes com o tamanho fixo desejado (128 bytes)
                    byte[] fixedSizeBytes = new byte[128];

                    // Copiar os bytes da string para o array de tamanho fixo
                    Array.Copy(stringBytes, fixedSizeBytes, Math.Min(stringBytes.Length, fixedSizeBytes.Length));

                    // Gravar o array de bytes no arquivo binário
                    writer.Write(fixedSizeBytes);

                }

                writer.Write(paneltyInfos.Length);
                foreach (PaneltyInfo paneltyInfo in paneltyInfos)
                {

                    writer.Write(paneltyInfo.s_nPaneltyLevel);
                    writer.Write(paneltyInfo.s_nDrop);
                    writer.Write(paneltyInfo.s_nExp);

                }

                writer.Write(evolutionBaseApplies.Length);
                foreach (EvolutionBaseApply evolutionBaseApply in evolutionBaseApplies)
                {

                    writer.Write(evolutionBaseApply.EvolutionType);

                    string text = evolutionBaseApply.EvolutionName;
                    byte[] stringBytes = Encoding.Unicode.GetBytes(text);

                    // Gravar o tamanho da string em bytes
                    writer.Write(stringBytes.Length / 2);

                    // Gravar os bytes da string
                    writer.Write(stringBytes);
                    writer.Write(evolutionBaseApply.EvolutionApplyValue);

                }

                writer.Write(evoMaxSkillLevels.Length);
                foreach (DigimonEvoMaxSkillLevel digimonEvoMaxSkillLevel1 in evoMaxSkillLevels)
                {

                    writer.Write(digimonEvoMaxSkillLevel1.nEvoType);
                    writer.Write(digimonEvoMaxSkillLevel1.s_SkillExpStartLv);
                    writer.Write(digimonEvoMaxSkillLevel1.s_SkillMaxLvs.Length);
                    for (int x = 0; x < digimonEvoMaxSkillLevel1.s_SkillMaxLvs.Length; x++)
                    {
                        writer.Write(digimonEvoMaxSkillLevel1.s_SkillMaxLvs[x]);
                    }
                }

                writer.Write(expansionConditions.Length);

                foreach (var expansionCondition in expansionConditions)
                {
                    writer.Write(expansionCondition.nOpenItemSubType);
                    writer.Write(expansionCondition.nExpansionRank);
                    writer.Write(expansionCondition.nEvoType.Length);

                    foreach (int evoType in expansionCondition.nEvoType)
                    {
                        writer.Write(evoType);
                    }
                }
            }
        }
   
    }

    public class DigiBaseInfo
    {
        public BaseInfoDigimonModel digimonModel;
        public int Id;
        public ushort Level;
        public long Exp;
        public int Hp;
        public int Ds;
        public int At;
        public ushort De;
        public ushort Ev;
        public ushort Ct;
        public ushort Ht;
        public ushort Ms;
        public ushort Unknow1;
        public ushort Unknow2;
    }

    public class CsBaseMapInfo
    {
        public int s_nMapID;
        public int s_nShoutSec;
        public ushort s_bEnableCheckMacro;
        public ushort unk;
    }

    public class EvolutionBaseApply
    {
        public int EvolutionType;
        public string EvolutionName;
        public int EvolutionApplyValue;
        public int NameSize;

    }

    public class Jumpbooster
    {
        public int dwItemID;
        public int mapcount;
        public int[] dwMapID;
        public int teste;
        public List<JumpMaps> JumpMaps = new();
    }

    public class JumpMaps
    {
        public int MapId;
    }
    public enum BaseInfoCharacterModel
    {
        Masaru = 80001,
        Tohma = 80002,
        Yoshino = 80003,
        Ikuto = 80004,
        Tai = 80005,
        Mimi = 80006,
        Yamato = 80007,
        Takeru = 80008,
        Hikari = 80009,
        Sora = 80010,
        Takato = 80011,
        Rika = 80012,
        Henry = 80013,
        Izzy = 80014,
        Joe = 80015,
        Jeri = 80016,
        Ryo = 80017,
        End
    }

    public enum BaseInfoDigimonModel
    {
        Agumon = 31001,
        Lalamon = 31002,
        Goamon = 31003,
        Falcomon = 31004,
        End
    }

    public class Party
    {
        public float distc;
    }

    public class Guild
    {
        public int s_nLevel;
        public uint s_nFame;
        public int s_nItemNo1;
        public int s_nItemCount1;
        public int s_nItemNo2;
        public int s_nItemCount2;
        public int s_nMasterLevel;
        public int s_nNeedPerson;
        public int s_nMaxGuildPerson;
        public int s_nIncMember;
        public int s_nMaxGuild2Master;
    }

    public class Limit
    {
        public ushort s_nMaxTacticsHouse;
        public ushort s_nMaxWareHouse;
        public ushort s_nUnionStore;
        public ushort s_nMaxShareStash;
        public int s_nConsume_XG;
        public int s_nCharge_XG;
    }

    public class Store
    {
       public float s_fPerson_Charge;
       public float s_fEmployment_Charge;
       public float s_fStoreDist;       
        public List<StoreItems> StoreItems = new();
    }

    public class StoreItems
    {
        public int s_nItemID;
        public int s_nDigimonID;
        public float s_fScale;
        public int s_nSlotCount;
        public string s_szFileName;
    }

    public class PaneltyInfo
    {
        public int s_nPaneltyLevel;
        public int s_nExp;
        public int s_nDrop;
    }

    public class DigimonEvoMaxSkillLevel
    {
        public int nEvoType;
        public int s_SkillExpStartLv;// 진화체 별로 디지몬의 스킬 경험시 시작 테이블 인덱스 번호
        public int nSubCount;
        public int[] s_SkillMaxLvs;     
        
    }

    public class ExpansionCondition
    {
        public int nOpenItemSubType;
        public int nExpansionRank;
        public int[] nEvoType;
    }
}
