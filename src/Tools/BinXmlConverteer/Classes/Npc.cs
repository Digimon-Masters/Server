using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Text.RegularExpressions;
using static BinXmlConverter.Classes.NPC;

namespace BinXmlConverter.Classes
{
    public class NPC
    {
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
            public static (NPCs[], ModelNpc[], EventNpc[]) ExportNpcToXml(string xmlPath)
            {

                using (BitReader read = new BitReader(File.Open(xmlPath, FileMode.Open)))
                {

                    int dcount = read.ReadInt();
                    NPCs[] npcs = new NPCs[dcount];

                    for (int i = 0; i < dcount; i++)
                    {
                        NPCs npcl = new NPCs();
                        npcl.NpcID = read.ReadInt();
                        npcl.MapID = read.ReadInt();
                        npcl.NPCType = read.ReadInt();
                        npcl.NPCMOVE = read.ReadInt();
                        npcl.s_nDisplayPlag = read.ReadInt();
                        npcl.Model = read.ReadInt();
                        npcl.NPCTag = read.ReadZString(Encoding.Unicode, 64);
                        npcl.NPCName = read.ReadZString(Encoding.Unicode, 64);
                        npcl.NPCDesc = read.ReadZString(Encoding.Unicode, 1024);

                        if(npcl.NPCType == 25)
                        {
                            npcl.NPCType = (int)eNPC_TYPE.NT_PROPERTY_STORE;
                        }
                        else if(npcl.NPCType == 26)
                        {
                            npcl.NPCType = (int)eNPC_TYPE.NT_ITEM_PRODUCTION_NPC;
                        }

                        switch (npcl.NPCType)
                        {
                            case (int)eNPC_TYPE.NT_NONE:
                            case (int)eNPC_TYPE.NT_DIGITAMA_TRADE:
                            case (int)eNPC_TYPE.NT_MAKE_TACTICS:
                            case (int)eNPC_TYPE.NT_ELEMENT_ITEM:
                            case (int)eNPC_TYPE.NT_WAREHOUSE:
                            case (int)eNPC_TYPE.NT_TACTICSHOUSE:
                            case (int)eNPC_TYPE.NT_CAPSULE_MACHINE:
                            case (int)eNPC_TYPE.NT_SKILL:
                            case (int)eNPC_TYPE.NT_DATS_PORTAL:
                            case (int)eNPC_TYPE.NT_ITEM_PRODUCTION_NPC:
                            case (int)eNPC_TYPE.NT_BATTLE_REGISTRANT_NPC:
                            case (int)eNPC_TYPE.NT_INFINITEWAR_MANAGER_NPC:
                            case (int)eNPC_TYPE.NT_INFINITEWAR_NOTICEBOARD_NPC:
                            case (int)eNPC_TYPE.NT_EXTRA_EVOLUTION_NPC:
                            case (int)eNPC_TYPE.NT_GOTCHA_MACHINE:
                            case (int)eNPC_TYPE.NT_MYSTERY_MACHINE:
                            case (int)eNPC_TYPE.NT_SPIRIT_EVO:
                            case (int)eNPC_TYPE.NT_Unknow1:
                                break;
                            case (int)eNPC_TYPE.NT_TRADE:
                            case (int)eNPC_TYPE.NT_GUILD:
                            case (int)eNPC_TYPE.NT_DIGICORE:
                            case (int)eNPC_TYPE.NT_EVENT_STORE:
                            case (int)eNPC_TYPE.NT_PROPERTY_STORE:
                            case (int)eNPC_TYPE.NT_Unknow:
                                {
                                    int cnt = read.ReadInt();

                                    for (int n = 0; n < cnt; ++n)
                                    {
                                        var ItemId = read.ReadInt();
                                        npcl.ItemId.Add(ItemId);
                                    }
                                }
                                break;
                            case (int)eNPC_TYPE.NT_PORTAL:
                                {
                                    sNPC_TYPE_PORTAL TypePortal = new();
                                    TypePortal.s_nPortalType = read.ReadInt();
                                    TypePortal.s_nPortalCount = read.ReadInt();
                                    for (int p = 0; p < TypePortal.s_nPortalCount; ++p)
                                    {
                                        PortalNpc portal = new();
                                        portal.s_dwEventID = read.ReadInt();

                                        for (int t = 0; t < 3; t++)
                                        {
                                            sPORTAL_REQ Req = new();
                                            Req.s_eEnableType = read.ReadInt();
                                            Req.s_nEnableID = read.ReadInt();
                                            Req.s_nEnableCount = read.ReadInt();
                                            portal.ReqArray.Add(Req);
                                        }
                                        TypePortal.portals.Add(portal);
                                    }

                                    npcl.NpcTypePortal.Add(TypePortal);
                                }
                                break;
                            case (int)eNPC_TYPE.NT_MASTERS_MATCHING:
                                {
                                    int cnt = read.ReadInt();

                                    for (int n = 0; n < cnt; ++n)
                                    {
                                        var ItemId = read.ReadInt();
                                        npcl.MastersMatchItemIds.Add(ItemId);
                                    }
                                }
                                break;
                            case (int)eNPC_TYPE.NT_SPECIAL_EVENT:
                                {
                                    npcl.nvType = read.ReadInt();

                                    switch (npcl.nvType)
                                    {
                                        case (int)sNPC_TYPE_SPECIAL_EVENT.EVENT_CARDGAME:       // 피에몬 카드 게임
                                            break;
                                        case (int)sNPC_TYPE_SPECIAL_EVENT.EVENT_PINOKIMON:  // 피노키몬의 카드게임(2014 겨울이벤트)
                                            {
                                                int nItemCnt = read.ReadInt();

                                                for (int x = 0; x < nItemCnt; x++)
                                                {
                                                    npcl.SpecialEventItems.Add(read.ReadInt());
                                                }
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;

                            default:
                                break;
                        }

                        npcl.nExtraData = read.ReadInt();
                        for (int x = 0; x < npcl.nExtraData; x++)
                        {
                            npcl.ExtraType = read.ReadInt();
                            if (npcl.ExtraType == 0)
                            {
                                ExtraQuest quest = new();
                                quest.s_nEInitSate = read.ReadInt();
                                quest.nActcnt = read.ReadInt();
                                for (int w = 0; w < quest.nActcnt; w++)
                                {
                                    ExtraAction action = new();
                                    action.ActionType = read.ReadInt();
                                    action.ECompState = read.ReadInt();
                                    action.QuestCount = read.ReadInt();
                                    action.QuestIds = new int[action.QuestCount];
                                    for (int z = 0; z < action.QuestCount; z++)
                                    {
                                        action.QuestIds[z] = read.ReadInt();
                                    }
                                    quest.extraActions.Add(action);
                                }

                                npcl.npcQuests.Add(quest);
                            }
                        }
                        npcs[i] = npcl;
                    }

                    int scount = read.ReadInt();
                    ModelNpc[] modelNpc = new ModelNpc[scount];
                    for (int s = 0; s < scount; s++)
                    {
                        ModelNpc model = new();
                        model.s_nModelID = read.ReadInt();
                        model.s_nOffset = read.ReadShort();
                        model.s_nOffset1 = read.ReadShort();
                        model.s_nOffset2 = read.ReadShort();
                        byte[] commentBytes = read.ReadBytes(128);
                        string comment = Encoding.Unicode.GetString(commentBytes, 0, commentBytes.Length);
                        comment = comment.Trim('\0');
                        comment = comment.Replace("\0s", string.Empty);
                        comment = comment.Replace("\0", string.Empty);
                        model.s_szComment = comment;
                        model.unknowvalue = read.ReadShort();
                        modelNpc[s] = model;
                    }

                    int wcount = read.ReadInt();

                    EventNpc[] Event = new EventNpc[wcount];
                    for (int i = 0; i < wcount; i++)
                    {
                        EventNpc ev = new();
                        ev.s_nNpcID = read.ReadInt();
                        ev.s_nTry = read.ReadInt();
                        ev.s_nExhaustMoney = read.ReadInt();
                        ev.s_dwExhaustItem = read.ReadInt();
                        ev.s_unItemCount = read.ReadInt();
                        for (int z = 0; z < ev.s_unItemCount; z++)
                        {
                            ItemCountMax max = new();
                            max.ItemId = read.ReadInt();
                            max.ItemCount = read.ReadInt();
                            ev.s_maxItems.Add(max);
                        }
                        Event[i] = ev;
                    }

                    return (npcs, modelNpc, Event);
                }

            }
            public static void ExportNPCsToBinary(NPCs[] npcs, ModelNpc[] modelnpcs, EventNpc[] eventNpcs, string binaryPath)
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(binaryPath, FileMode.Create)))
                {
                    writer.Write(npcs.Length);
                    foreach (NPCs npc in npcs)
                    {
                        writer.Write(npc.NpcID);
                        writer.Write(npc.MapID);
                        writer.Write(npc.NPCType);
                        writer.Write(npc.NPCMOVE);
                        writer.Write(npc.s_nDisplayPlag);
                        writer.Write(npc.Model);

                        // Grava as strings NPCTag, NPCName e NPCDesc
                        WriteFixedString(writer, npc.NPCTag, 32);
                        WriteFixedString(writer, npc.NPCName, 32);
                        WriteFixedString(writer, npc.NPCDesc, 512);

                        switch (npc.NPCType)
                        {
                            case (int)eNPC_TYPE.NT_NONE:
                            case (int)eNPC_TYPE.NT_DIGITAMA_TRADE:
                            case (int)eNPC_TYPE.NT_MAKE_TACTICS:
                            case (int)eNPC_TYPE.NT_ELEMENT_ITEM:
                            case (int)eNPC_TYPE.NT_WAREHOUSE:
                            case (int)eNPC_TYPE.NT_TACTICSHOUSE:
                            case (int)eNPC_TYPE.NT_CAPSULE_MACHINE:
                            case (int)eNPC_TYPE.NT_SKILL:
                            case (int)eNPC_TYPE.NT_DATS_PORTAL:
                            case (int)eNPC_TYPE.NT_ITEM_PRODUCTION_NPC:
                            case (int)eNPC_TYPE.NT_BATTLE_REGISTRANT_NPC:
                            case (int)eNPC_TYPE.NT_INFINITEWAR_MANAGER_NPC:
                            case (int)eNPC_TYPE.NT_INFINITEWAR_NOTICEBOARD_NPC:
                            case (int)eNPC_TYPE.NT_EXTRA_EVOLUTION_NPC:
                            case (int)eNPC_TYPE.NT_GOTCHA_MACHINE:
                            case (int)eNPC_TYPE.NT_MYSTERY_MACHINE:
                            case (int)eNPC_TYPE.NT_SPIRIT_EVO:
                            case (int)eNPC_TYPE.NT_Unknow1:
                                break;
                            case (int)eNPC_TYPE.NT_TRADE:
                            case (int)eNPC_TYPE.NT_GUILD:
                            case (int)eNPC_TYPE.NT_DIGICORE:
                            case (int)eNPC_TYPE.NT_EVENT_STORE:
                            case (int)eNPC_TYPE.NT_PROPERTY_STORE:
                            case (int)eNPC_TYPE.NT_Unknow:
                                {
                                    writer.Write(npc.ItemId.Count);
                                    foreach (int itemId in npc.ItemId)
                                    {
                                        writer.Write(itemId);
                                    }
                                }
                                break;
                            case (int)eNPC_TYPE.NT_PORTAL:
                                {
                                    foreach (sNPC_TYPE_PORTAL portal in npc.NpcTypePortal)
                                    {
                                        writer.Write(portal.s_nPortalType);
                                        writer.Write(portal.s_nPortalCount);

                                        foreach (PortalNpc portalNpc in portal.portals)
                                        {
                                            writer.Write(portalNpc.s_dwEventID);
                                            foreach (sPORTAL_REQ req in portalNpc.ReqArray)
                                            {
                                                writer.Write(req.s_eEnableType);
                                                writer.Write(req.s_nEnableID);
                                                writer.Write(req.s_nEnableCount);
                                            }
                                        }
                                    }
                                }
                                break;
                            case (int)eNPC_TYPE.NT_MASTERS_MATCHING:
                                {
                                    writer.Write(npc.MastersMatchItemIds.Count);
                                    foreach (int itemId in npc.MastersMatchItemIds)
                                    {
                                        writer.Write(itemId);
                                    }
                                }
                                break;
                            case (int)eNPC_TYPE.NT_SPECIAL_EVENT:
                                {
                                    writer.Write(npc.nvType);

                                    switch (npc.nvType)
                                    {
                                        case (int)sNPC_TYPE_SPECIAL_EVENT.EVENT_CARDGAME:       // 피에몬 카드 게임
                                            break;
                                        case (int)sNPC_TYPE_SPECIAL_EVENT.EVENT_PINOKIMON:  // 피노키몬의 카드게임(2014 겨울이벤트)
                                            {

                                                writer.Write(npc.SpecialEventItems.Count);
                                                foreach (int itemId in npc.SpecialEventItems)
                                                {
                                                    writer.Write(itemId);
                                                }
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;

                            default:
                                break;
                        }


                        writer.Write(npc.nExtraData);
                        for (int x = 0; x < npc.nExtraData; x++)
                        {
                            writer.Write(npc.ExtraType);
                            if (npc.ExtraType == 0)
                            {
                                foreach (var quest in npc.npcQuests)
                                {



                                    writer.Write(quest.s_nEInitSate);
                                    writer.Write(quest.nActcnt);
                                    for (int w = 0; w < quest.nActcnt; w++)
                                    {

                                        writer.Write(quest.extraActions[w].ActionType);
                                        writer.Write(quest.extraActions[w].ECompState);
                                        writer.Write(quest.extraActions[w].QuestCount);
                                        for (int z = 0; z < quest.extraActions[w].QuestCount; z++)
                                        {
                                            writer.Write(quest.extraActions[w].QuestIds[z]);
                                        }

                                    }
                                }


                            }
                        }

                    }

                    writer.Write(modelnpcs.Length);
                    foreach (ModelNpc model in modelnpcs)
                    {
                        writer.Write(model.s_nModelID);
                        writer.Write(model.s_nOffset);
                        writer.Write(model.s_nOffset1);
                        writer.Write(model.s_nOffset2);

                        int commentLength = 64;

                        string fixedComment = model.s_szComment.PadRight(commentLength, '\0');
                        WriteFixedString(writer, fixedComment, 64);
                        writer.Write((short)model.unknowvalue);
                    }

                    writer.Write(eventNpcs.Length);
                    foreach (EventNpc events in eventNpcs)
                    {
                        writer.Write(events.s_nNpcID);
                        writer.Write(events.s_nTry);
                        writer.Write(events.s_nExhaustMoney);
                        writer.Write(events.s_dwExhaustItem);

                        writer.Write(events.s_unItemCount);
                        for (int i = 0; i < events.s_maxItems.Count; i++)
                        {
                            writer.Write(events.s_maxItems[i].ItemId);
                            writer.Write(events.s_maxItems[i].ItemCount);
                        }
                    }
                }

            }
            public static void ExportNPCsToXml(string xmlPath, NPCs[] npcs)
            {
                string EncodeSpecialCharacters(string input)
                {
                    StringBuilder encodedString = new StringBuilder();
                    foreach (char c in input)
                    {
                        if (XmlConvert.IsXmlChar(c))
                        {
                            encodedString.Append(c);
                        }
                        else
                        {
                            encodedString.AppendFormat("&#x{0:X};", (int)c);
                        }
                    }
                    return encodedString.ToString();
                }
                XDocument doc = new XDocument();
                XElement rootElement = new XElement("NPCs");
                doc.Add(rootElement);

                foreach (var npc in npcs)
                {
                    XElement npcElement = new XElement("NPC");
                    npcElement.Add(new XElement("NpcID", npc.NpcID));
                    npcElement.Add(new XElement("MapID", npc.MapID));
                    npcElement.Add(new XElement("NPCType", npc.NPCType));
                    npcElement.Add(new XElement("NPCMOVE", npc.NPCMOVE));
                    npcElement.Add(new XElement("s_nDisplayPlag", npc.s_nDisplayPlag));
                    string encodedNPCTag = EncodeSpecialCharacters(npc.NPCTag);
                    npcElement.Add(new XElement("NPCTag", new XCData(encodedNPCTag)));
                    string encodedNPCName = EncodeSpecialCharacters(npc.NPCName);
                    npcElement.Add(new XElement("NPCName", new XCData(encodedNPCName)));
                    npcElement.Add(new XElement("Model", npc.Model));

                    // Codificar caracteres especiais na descrição do NPC
                    string encodedNPCDesc = EncodeSpecialCharacters(npc.NPCDesc);
                    npcElement.Add(new XElement("NPCDesc", new XCData(encodedNPCDesc)));

                    switch (npc.NPCType)
                    {
                        case (int)eNPC_TYPE.NT_TRADE:
                        case (int)eNPC_TYPE.NT_GUILD:
                        case (int)eNPC_TYPE.NT_DIGICORE:
                        case (int)eNPC_TYPE.NT_EVENT_STORE:
                        case (int)eNPC_TYPE.NT_PROPERTY_STORE:
                            case(int)eNPC_TYPE.NT_Unknow:
                            XElement itemIdsElement = new XElement("ItemIDs");
                            foreach (var itemId in npc.ItemId)
                            {
                                itemIdsElement.Add(new XElement("ItemID", itemId));
                            }
                            npcElement.Add(itemIdsElement);
                            break;
                        case (int)eNPC_TYPE.NT_PORTAL:
                            XElement portalsElement = new XElement("Portals");
                            npcElement.Add(portalsElement);

                            foreach (var portal in npc.NpcTypePortal)
                            {
                                XElement portalElement = new XElement("Portal");
                                portalElement.Add(new XElement("s_nPortalType", portal.s_nPortalType));
                                portalElement.Add(new XElement("s_nPortalCount", portal.s_nPortalCount));

                                XElement portalsTypeElement = new XElement("PortalsType");
                                portalElement.Add(portalsTypeElement);

                                foreach (var portais in portal.portals)
                                {
                                    XElement portalTypeElement = new XElement("PortalType");
                                    portalTypeElement.Add(new XElement("s_dwEventID", portais.s_dwEventID));

                                    XElement reqElement = new XElement("Req");
                                    portalTypeElement.Add(reqElement);

                                    foreach (var req in portais.ReqArray)
                                    {
                                        XElement reqItemElement = new XElement("ReqItem");
                                        reqItemElement.Add(new XElement("s_eEnableType", req.s_eEnableType));
                                        reqItemElement.Add(new XElement("s_nEnableID", req.s_nEnableID));
                                        reqItemElement.Add(new XElement("s_nEnableCount", req.s_nEnableCount));
                                        reqElement.Add(reqItemElement);
                                    }

                                    portalsTypeElement.Add(portalTypeElement);
                                }

                                portalsElement.Add(portalElement);
                            }

                            break;
                        case (int)eNPC_TYPE.NT_MASTERS_MATCHING:
                            XElement matchItemIdsElement = new XElement("MatchItemIDs");
                            foreach (var itemId in npc.MastersMatchItemIds)
                            {
                                matchItemIdsElement.Add(new XElement("ItemID", itemId));
                            }
                            npcElement.Add(matchItemIdsElement);
                            break;

                        case (int)eNPC_TYPE.NT_SPECIAL_EVENT:
                            {


                                XElement specialEventItemsElement = new XElement("SpecialEventItems");
                                specialEventItemsElement.Add(new XElement("nvType", npc.nvType));
                                switch (npc.nvType)
                                {

                                    case (int)sNPC_TYPE_SPECIAL_EVENT.EVENT_CARDGAME:       // 피에몬 카드 게임
                                        break;
                                    case (int)sNPC_TYPE_SPECIAL_EVENT.EVENT_PINOKIMON:  // 피노키몬의 카드게임(2014 겨울이벤트)
                                        {
                                            foreach (var itemId in npc.SpecialEventItems)
                                            {
                                                specialEventItemsElement.Add(new XElement("ItemID", itemId));
                                            }
                                            npcElement.Add(specialEventItemsElement);
                                        }
                                        break;

                                    default:
                                        break;
                                }

                            }
                            break;
                    }

                    npcElement.Add(new XElement("nExtraData", npc.nExtraData));
                    rootElement.Add(npcElement);

                    if (npc.npcQuests != null && npc.npcQuests.Count > 0)
                    {
                        foreach (ExtraQuest quest in npc.npcQuests)
                        {
                            XElement questElement = new XElement("Quest");
                            questElement.Add(new XElement("s_nEInitSate", quest.s_nEInitSate));
                            questElement.Add(new XElement("nActcnt", quest.nActcnt));

                            foreach (ExtraAction action in quest.extraActions)
                            {
                                XElement actionElement = new XElement("Action");
                                actionElement.Add(new XElement("ActionType", action.ActionType));
                                actionElement.Add(new XElement("ECompState", action.ECompState));
                                actionElement.Add(new XElement("QuestCount", action.QuestCount));

                                XElement questIdsElement = new XElement("QuestIds");
                                foreach (int questId in action.QuestIds)
                                {
                                    questIdsElement.Add(new XElement("QuestId", questId));
                                }

                                actionElement.Add(questIdsElement);
                                questElement.Add(actionElement);
                            }

                            npcElement.Add(questElement);
                        }
                    }

                }

                doc.Save(xmlPath);

            }
            public static NPCs[] ImportNPCsFromXML(string xmlPath)
            {

                List<NPCs> npcs = new List<NPCs>();

                XDocument doc = XDocument.Load(xmlPath);
                XElement rootElement = doc.Element("NPCs");

                foreach (XElement npcElement in rootElement.Elements("NPC"))
                {
                    NPCs npc = new NPCs();
                    npc.NpcID = (int)npcElement.Element("NpcID");
                    npc.MapID = (int)npcElement.Element("MapID");
                    npc.NPCType = (int)npcElement.Element("NPCType");
                    npc.NPCMOVE = (int)npcElement.Element("NPCMOVE");
                    npc.s_nDisplayPlag = (int)npcElement.Element("s_nDisplayPlag");
                    npc.NPCTag = (string)npcElement.Element("NPCTag");
                    npc.NPCName = (string)npcElement.Element("NPCName");
                    npc.Model = (int)npcElement.Element("Model");
                    npc.NPCDesc = (string)npcElement.Element("NPCDesc");

                    // Handle different NPCType cases
                    switch (npc.NPCType)
                    {


                        case (int)eNPC_TYPE.NT_TRADE:
                        case (int)eNPC_TYPE.NT_GUILD:
                        case (int)eNPC_TYPE.NT_DIGICORE:
                        case (int)eNPC_TYPE.NT_EVENT_STORE:
                        case (int)eNPC_TYPE.NT_PROPERTY_STORE:
                        case (int)eNPC_TYPE.NT_Unknow:
                            XElement itemIdsElement = npcElement.Element("ItemIDs");
                            if (itemIdsElement != null)
                            {
                                npc.ItemId = new List<int>();
                                foreach (XElement itemIdElement in itemIdsElement.Elements("ItemID"))
                                {
                                    int itemId = (int)itemIdElement;
                                    npc.ItemId.Add(itemId);
                                }
                            }
                            break;

                        case (int)eNPC_TYPE.NT_PORTAL:
                            XElement portalsElement = npcElement.Element("Portals");
                            if (portalsElement != null)
                            {
                              
                                npc.NpcTypePortal = new List<sNPC_TYPE_PORTAL>();
                                foreach (XElement portalElement in portalsElement.Elements("Portal"))
                                {
                                    sNPC_TYPE_PORTAL portal = new();
                                    portal.s_nPortalType = (int)portalElement.Element("s_nPortalType");
                                    portal.s_nPortalCount = (int)portalElement.Element("s_nPortalCount");

                                    foreach (XElement portaisElement in portalElement.Elements("PortalsType"))
                                    {
                                        foreach (XElement portais in portaisElement.Elements("PortalType"))
                                        {
                                            PortalNpc portalNpc = new();
                                            portalNpc.s_dwEventID = (int)portais.Element("s_dwEventID");

                                            foreach (XElement portaisReq in portais.Elements("Req"))
                                            {
                                                foreach (XElement ReqElement in portaisReq.Elements("ReqItem"))
                                                {

                                                    sPORTAL_REQ req = new();
                                                    req.s_eEnableType = (int)ReqElement.Element("s_eEnableType");
                                                    req.s_nEnableID = (int)ReqElement.Element("s_nEnableID");
                                                    req.s_nEnableCount = (int)ReqElement.Element("s_nEnableCount");
                                                    portalNpc.ReqArray.Add(req);
                                                }
                                            }

                                            portal.portals.Add(portalNpc);
                                       }
                            
                                    }
                                    npc.NpcTypePortal.Add(portal);
                                }
                            }

                            break;

                        case (int)eNPC_TYPE.NT_MASTERS_MATCHING:
                            XElement matchItemIdsElement = npcElement.Element("MatchItemIDs");
                            if (matchItemIdsElement != null)
                            {
                                npc.MastersMatchItemIds = new List<int>();
                                foreach (XElement itemIdElement in matchItemIdsElement.Elements("ItemID"))
                                {
                                    int itemId = (int)itemIdElement;
                                    npc.MastersMatchItemIds.Add(itemId);
                                }
                            }
                            break;

                        case (int)eNPC_TYPE.NT_SPECIAL_EVENT:
                            XElement specialEventItemsElement = npcElement.Element("SpecialEventItems");
                            
                            if (specialEventItemsElement != null)
                            {
                                npc.nvType = int.Parse(specialEventItemsElement.Element("nvType").Value);
                                npc.SpecialEventItems = new List<int>();
                                foreach (XElement itemIdElement in specialEventItemsElement.Elements("ItemID"))
                                {
                                    int itemId = (int)itemIdElement;
                                    npc.SpecialEventItems.Add(itemId);
                                }
                            }
                            break;
                    }


                    npc.nExtraData = (int?)npcElement.Element("nExtraData") ?? 0;
                    if (npc.nExtraData != 0)
                    {
                        npc.npcQuests = new List<ExtraQuest>();
                        foreach (XElement questElement in npcElement.Elements("Quest"))
                        {
                            ExtraQuest extraQuest = new ExtraQuest();
                            extraQuest.s_nEInitSate = (int)questElement.Element("s_nEInitSate");
                            extraQuest.nActcnt = (int)questElement.Element("nActcnt");
                            foreach (XElement actionElement in questElement.Elements("Action"))
                            {
                                ExtraAction extraAction = new ExtraAction();
                                extraAction.ActionType = (int)actionElement.Element("ActionType");
                                extraAction.ECompState = (int)actionElement.Element("ECompState");
                                extraAction.QuestCount = (int)actionElement.Element("QuestCount");
                                extraAction.QuestIds = new int[extraAction.QuestCount];

                                int count = 0; // Start the index at 0
                                foreach (XElement questIdElement in actionElement.Elements("QuestIds").Elements("QuestId"))
                                {
                                    extraAction.QuestIds[count] = (int)questIdElement;
                                    count++;
                                }
                                extraQuest.extraActions.Add(extraAction);
                            }
                            npc.npcQuests.Add(extraQuest);
                        }
                    }

                    npcs.Add(npc);
                }



                return npcs.ToArray();
            }

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

        public class ModelNpc
        {
            public int s_nModelID;
            public short s_nOffset;
            public short s_nOffset1;
            public short s_nOffset2;
            public string s_szComment;
            public int unknowvalue;

            public static void ExportModelNpcToXml(ModelNpc[] npcList, string xmlPath)
            {
                XElement rootElement = new XElement("NPCs");

                foreach (ModelNpc npc in npcList)
                {
                    XElement npcElement = new XElement("NPC");

                    npcElement.Add(new XElement("s_nModelID", npc.s_nModelID));
                    npcElement.Add(new XElement("s_nOffset", npc.s_nOffset));
                    npcElement.Add(new XElement("s_nOffset1", npc.s_nOffset1));
                    npcElement.Add(new XElement("s_nOffset2", npc.s_nOffset2));

                    string invalidCharsPattern = "[^\\p{L}\\p{N}\\s\\\\]";
                    string cleanedComment = Regex.Replace(npc.s_szComment, invalidCharsPattern, "");

                    npcElement.Add(new XElement("s_szComment", cleanedComment));
                    npcElement.Add(new XElement("unknowvalue", npc.unknowvalue));

                    rootElement.Add(npcElement);
                }

                XDocument doc = new XDocument(rootElement);
                doc.Save(xmlPath);
            }
            public static ModelNpc[] ImportModelNpcFromXml(string xmlPath)
            {
                List<ModelNpc> npcList = new List<ModelNpc>();

                XDocument doc = XDocument.Load(xmlPath);
                XElement rootElement = doc.Element("NPCs");

                foreach (XElement npcElement in rootElement.Elements("NPC"))
                {
                    ModelNpc npc = new ModelNpc();

                    npc.s_nModelID = (int)npcElement.Element("s_nModelID");
                    npc.s_nOffset = (short)npcElement.Element("s_nOffset");
                    npc.s_nOffset1 = (short)npcElement.Element("s_nOffset1");
                    npc.s_nOffset2 = (short)npcElement.Element("s_nOffset2");
                    npc.s_szComment = (string)npcElement.Element("s_szComment");
                    npc.unknowvalue = (int)npcElement.Element("unknowvalue");

                    npcList.Add(npc);
                }

                return npcList.ToArray();
            }
        }

        public class EventNpc
        {

            public int s_nNpcID;         // npc id
            public int s_nTry;               // 도전 횟수
            public int s_nExhaustMoney;  // 소모 bit
            public int s_dwExhaustItem;  // 소모 아이템 코드 번호
            public int s_unItemCount;   // 소모 아이템 갯수
            public List<ItemCountMax> s_maxItems = new();
            public static void ExportEventNpcToXml(string xmlPath, EventNpc[] npcArray)
            {
                XElement rootElement = new XElement("NPCs");

                foreach (EventNpc npc in npcArray)
                {
                    XElement npcElement = new XElement("NPC");

                    npcElement.Add(new XElement("s_nNpcID", npc.s_nNpcID));
                    npcElement.Add(new XElement("s_nTry", npc.s_nTry));
                    npcElement.Add(new XElement("s_nExhaustMoney", npc.s_nExhaustMoney));
                    npcElement.Add(new XElement("s_dwExhaustItem", npc.s_dwExhaustItem));
                    npcElement.Add(new XElement("s_unItemCount", npc.s_unItemCount));

                    XElement maxItemsElement = new XElement("s_maxItems");
                    foreach (ItemCountMax item in npc.s_maxItems)
                    {
                        XElement itemElement = new XElement("Item");
                        itemElement.Add(new XElement("s_nItemID", item.ItemId));
                        itemElement.Add(new XElement("s_nMaxCount", item.ItemCount));

                        maxItemsElement.Add(itemElement);
                    }

                    npcElement.Add(maxItemsElement);

                    rootElement.Add(npcElement);
                }

                XDocument doc = new XDocument(rootElement);
                doc.Save(xmlPath);
            }
            public static EventNpc[] ImportEventNpcFromXml(string xmlPath)
            {
                List<EventNpc> npcList = new List<EventNpc>();

                XDocument doc = XDocument.Load(xmlPath);
                XElement rootElement = doc.Element("NPCs");

                foreach (XElement npcElement in rootElement.Elements("NPC"))
                {
                    EventNpc npc = new EventNpc();

                    npc.s_nNpcID = (int)npcElement.Element("s_nNpcID");
                    npc.s_nTry = (short)npcElement.Element("s_nTry");
                    npc.s_nExhaustMoney = (int)npcElement.Element("s_nExhaustMoney");
                    npc.s_dwExhaustItem = (int)npcElement.Element("s_dwExhaustItem");
                    npc.s_unItemCount = (int)npcElement.Element("s_unItemCount");

                    XElement maxItemsElement = npcElement.Element("s_maxItems");
                    if (maxItemsElement != null)
                    {
                        foreach (XElement itemElement in maxItemsElement.Elements("Item"))
                        {
                            ItemCountMax item = new ItemCountMax();
                            item.ItemId = (int)itemElement.Element("s_nItemID");
                            item.ItemCount = (int)itemElement.Element("s_nMaxCount");

                            npc.s_maxItems.Add(item);
                        }
                    }

                    npcList.Add(npc);
                }

                return npcList.ToArray();
            }
        }

        public class ItemCountMax
        {
            public int ItemId;
            public int ItemCount;
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
        private static void WriteFixedString(BinaryWriter writer, string value, int length)
        {
            // Preenche a string com espaços em branco ou trunca se necessário
            string paddedValue = value.PadRight(length, '\0');

            // Converte a string para um array de bytes usando a codificação UTF-16
            byte[] stringBytes = Encoding.Unicode.GetBytes(paddedValue);

            // Verifica se o tamanho do array é maior do que o tamanho desejado
            if (stringBytes.Length > length * 2)
            {
                // Trunca o array para o tamanho desejado
                Array.Resize(ref stringBytes, length * 2);
            }
            else if (stringBytes.Length < length * 2)
            {
                // Preenche o array com bytes nulos se for menor do que o tamanho desejado
                Array.Resize(ref stringBytes, length * 2);
                Array.Clear(stringBytes, value.Length * 2, (length - value.Length) * 2);
            }

            // Grava o array de bytes no arquivo
            writer.Write(stringBytes);
        }

        private static string DecodeSpecialCharacters(string encodedString)
        {
            string decodedString = encodedString
                .Replace("&lt;", "<")
                .Replace("&gt;", ">")
                .Replace("&quot;", "\"")
                .Replace("&apos;", "'")
                .Replace("&amp;", "&");

            return decodedString;
        }
    }
}
