using Microsoft.Data.SqlClient;
using System.Xml.Linq;
using System.Xml.Serialization;
using static DataImporterTool.Form1;

namespace DSO.DataImport.Processors
{
    internal static class NpcInfoImportProcessor
    {
        internal static void Import(string cs, string[] files)
        {
            var newDataList = ImportNPCsFromXML(files[0]);


            var mainQuery = $@" IF NOT EXISTS(SELECT 1 FROM Asset.Npc WHERE NpcId = @NpcId)
                                BEGIN
                                    INSERT INTO Asset.Npc
                                        (NpcId, MapId)
                                    VALUES
                                        (@NpcId, @MapId);
                                    SELECT SCOPE_IDENTITY();
                                END ELSE BEGIN SELECT 0; END;";
            
            var subQuery = $@"  INSERT INTO Asset.NpcItem
                                    (ItemId, NpcAssetId)
                                VALUES
                                    (@ItemId, @NpcAssetId);";

            var sub1Query = $@"  INSERT INTO Asset.NpcPortal
                                    (PortalType, PortalCount, NpcAssetId)
                                VALUES
                                    (@PortalType,@PortalCount, @NpcAssetId);";

            var sub3Query = $@"  INSERT INTO Asset.NpcPortalsAmount
                                    ( NpcAssetId)
                                VALUES
                                    (@NpcAssetId);";

            var sub2Query = $@"  INSERT INTO Asset.NpcPortals
                                    ( ItemId,Type, ResourceAmount, NpcAssetId)
                                VALUES
                                    (@ItemId,@Type,@ResourceAmount, @NpcAssetId);";

          
            var conn = new SqlConnection(cs);
            conn.Open();

            foreach (var npc in newDataList.Where( x=> x.ItemId.Count > 0 || x.NpcTypePortal.Count != 0 ))
            {
                var command = new SqlCommand(mainQuery, conn);
                command.Parameters.AddWithValue("NpcId", npc.NpcID);
                command.Parameters.AddWithValue("MapId", npc.MapID);

                var npcId = Convert.ToInt64(command.ExecuteScalar());

                if(npcId > 0)
                {
                    foreach (var item in npc.ItemId)
                    {

                        var subCommand = new SqlCommand(subQuery, conn);
                        subCommand.Parameters.AddWithValue("ItemId", item);
                        subCommand.Parameters.AddWithValue("NpcAssetId", npcId);

                        subCommand.ExecuteNonQuery();
                    }
                    foreach (var item in npc.NpcTypePortal)
                    {
                        var sub1Command = new SqlCommand(sub1Query, conn);
                        sub1Command.Parameters.AddWithValue("PortalType", item.s_nPortalType);
                        sub1Command.Parameters.AddWithValue("PortalCount", item.s_nPortalCount);
                        sub1Command.Parameters.AddWithValue("NpcAssetId", npcId);

                      
                        sub1Command.ExecuteNonQuery();

                        SqlCommand getIdCommand = new SqlCommand("SELECT IDENT_CURRENT('Asset.NpcPortal')", conn);

                        var PortalId = Convert.ToInt64(getIdCommand.ExecuteScalar());

                        foreach (var portal in item.portals)
                        {
                            var sub3Command = new SqlCommand(sub3Query, conn);
           
                            sub3Command.Parameters.AddWithValue("NpcAssetId", PortalId);
                            sub3Command.ExecuteNonQuery();

                            getIdCommand = new SqlCommand("SELECT IDENT_CURRENT('Asset.NpcPortalsAmount')", conn);

                            var PortalId1 = Convert.ToInt64(getIdCommand.ExecuteScalar());

                            foreach (var portals in portal.ReqArray)
                            {
                                var sub2Command = new SqlCommand(sub2Query, conn);
                                sub2Command.Parameters.AddWithValue("ItemId", portals.s_nEnableID);
                                sub2Command.Parameters.AddWithValue("Type", portals.s_eEnableType);
                                sub2Command.Parameters.AddWithValue("ResourceAmount", portals.s_nEnableCount);
                                sub2Command.Parameters.AddWithValue("NpcAssetId", PortalId1);
                                
                                sub2Command.ExecuteNonQuery();


                            }
                        }
                    }

                  
                }
            }

            conn.Close();
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
}