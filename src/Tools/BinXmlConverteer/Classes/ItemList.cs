using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using static BinXmlConverter.Classes.ItemList;

namespace BinXmlConverter.Classes
{
    public class ItemList
    {

        #region Classes
        public class Item
        {
            public int s_dwItemID;
            public string s_szName;
            public int s_nIcon;
            public string s_szComment;
            public string s_cNif;
            public ushort Item_Class;
            public string s_szTypeComment;
            public ushort s_nType_L;
            public ushort s_nType_S;
            public int s_nTypeValue;
            public int Section;
            public ushort s_nSellType;
            public ushort s_nUseTimeGroup;
            public ushort s_nOverlap;
            public ushort s_nTamerReqMinLevel;
            public ushort s_nTamerReqMaxLevel;
            public ushort s_nDigimonReqMinLevel;
            public ushort s_nDigimonReqMaxLevel;
            public ushort s_nPossess;
            public ushort s_nEquipSeries;
            public ushort s_nUseCharacter;
            public ushort s_nDrop;
            public ushort s_dwEventItemPrice;
            public ushort s_dwDigiCorePrice;
            public ushort Item_Digicore;
            public int s_dwScanPrice;
            public int s_dwSale;
            public string s_cModel_Nif;
            public string s_cModel_Effect;
            public byte s_bModel_Loop;
            public byte s_bModel_Shader;
            public ushort s_nSkillCodeType;
            public int s_dwSkill;
            public byte s_btApplyRateMax;
            public byte s_btApplyRateMin;
            public byte s_btApplyElement;
            public byte unknow;
            public short unknow1;
            public short unknow2;
            public short unknow3;
            public int unknow4;
            public ushort s_nSocketCount;
            public ushort s_dwSoundID;
            public byte item_Belonging;
            public int s_nQuest1;
            public int s_nQuest2;
            public int s_nQuest3;
            public int s_nDigiviceSkillSlot;
            public int s_nDigiviceChipsetSlot;
            public short s_nQuestRequire;
            public byte unknow5;
            public short unknow6;
            public byte unknow7;
            public byte Use_Item_Type;
            public int UseTimeMinutes;
            public byte s_nUseBattle;
            public short s_nDoNotUseType;
            public byte s_bUseTimeType;
            public int unknow8;
        }
        public class ItemTap
        {
            public ushort s_nSellClass;
            public string s_szName;

        }
        public class ItemCoolTime
        {
            public int s_nGroupID;
            public byte[] TimeGroup;
        }
        public class ItemDisplay
        {
            public int nItemS;
            public int dwDispID;


        }
        public class ItemTypeName
        {
            public int s_szId;
            public string s_szName;
        }
        public class ItemRank // Container
        {
            public int ID;
            public short Drop_Class;
            public short Drop_Count;

        }
        public class ElementItem
        {
            public int s_dwItemID;
        }
        public class ItemExchange
        {
            public int nullo;

            public int ItemExchangeNo;
            public int s_dwNpcID;
            public short s_dwItemIndex;
            public short unk;
            public int s_dwItemID;
            public int s_dwExchange_Code_A;
            public int s_dwExchange_Code_B;
            public int s_dwExchange_Code_C;
            public int s_dwExchange_Code_D;
            public short s_dwPropertyA_Price;
            public short s_dwPropertyB_Price;
            public short s_dwPropertyC_Price;
            public short s_dwPropertyD_Price;
            public ushort s_dwCount;
            public short unk1;
            public int unk2;

            public bool IsNullItem { get; internal set; }
            public void ReadNullItem()
            {
                IsNullItem = true;
                // Lógica para tratar o campo nullo como um item nulo
                // Pode-se criar uma lógica específica aqui para tratamento do item nulo
            }
        }
        public class Accessory
        {
            public int index_Accessory;
            public int index_Accessory1;
            public short Gain_Option;
            public short Changeable_Option_Number;
            public List<AccessoryOption> Option = new List<AccessoryOption>(16);
        }
        public class AccessoryOption
        {

            public short s_nOptIdx;        //옵션 종류
            public uint s_nMin;            //해당 옵션 최소값
            public uint s_nMax;         //해당 옵션 최대값
            public short unknow;
        }
        public class AccessoryEnchant
        {
            public int ID;
            public int Index_Enchant;
            public short Enchant_Option;
            public short Factor1;
        }
        public struct ItemMaking
        {
            public int count;
            public NPC[] index;
        }

        public struct NPC
        {
            public int m_dwNpcIdx;
            public int m_mapMainCategoty;
            public Abar[] index;
        }

        public struct Abar
        {
            public int ID;
            public int ID1;
            public int CarteSize;
            public string Abaname;
            public int size_mapSubCategoty;
            public SubCategoty[] index;
        }

        public struct SubCategoty
        {
            public int ID;
            public int ID1;
            public int SizeNameCate;
            public string Name;
            public int fcount;
            public itemMake[] index;
        }

        public struct itemMake
        {
            public int m_nUniqueIdx;
            public int m_dwItemIdx;
            public int m_nItemNum;
            public int m_nProbabilityofSuccess;
            public int ink;
            public int unk;
            public int Valor;
            public int m_dwItemCost;
            public MaterialList[] index;
        }

        public struct MaterialList
        {
            public int m_dwItemIdx;
            public int m_nItemNum;
        }

        public class ItemMakingGroup
        {
            public int Index;
            public int Type_No;
            public int Item_Num;
            public int Item_Code;
            public int Item_Num1;
        }
        public class XaiItem
        {
            public int ItemID;
            public int XGauge;
            public byte MaxCrystal;
        }
        public class ItemRankEffect
        {

            public int nItemCode;
            public int nInterval;
            public List<ItemRankEffectInterval> Interval = new();
            public int dwItemCode;
            public int IconNo;
            public int Rank;
        }
        public class ItemRankEffectInterval
        {
            public int dwItemCode;
            public int IconNo;
            public int Rank;
        }
        public class LookItem
        {
            public int Item_Code;
            public int Di_No;
            public int Change_Type;
            public int NameSize;
            public int nameSize;
            public string File_Name;

        }
        #endregion


        public struct ITEM
        {
            public int icount;
            public struct sINFO
            {
                public uint s_dwItemID;
                public string s_szName;
                public uint s_nIcon;
                public string s_szComment;
                public string s_cNif;
                public ushort s_nClass;
                public string s_szTypeComment;
                public byte s_btCodeTag;
                public byte unkt;
                public ushort s_nType_L;
                public ushort s_nType_S;
                public uint s_nTypeValue;
                public uint s_nSection;
                public ushort s_nSellType;
                public byte s_nUseMode;
                public byte unkr;
                public ushort s_nUseTimeGroup;
                public ushort s_nOverlap;
                public ushort s_nTamerReqMinLevel;
                public ushort s_nTamerReqMaxLevel;
                public ushort s_nDigimonReqMinLevel;
                public ushort s_nDigimonReqMaxLevel;
                public ushort s_nPossess;
                public ushort s_nEquipSeries;
                public ushort s_nUseCharacter;
                public byte s_bDummy;
                public byte unk;
                public ushort s_nDrop;
                public ushort ukteste1;
                public uint s_nEventItemType;
                public ushort s_dwEventItemPrice;
                public ushort s_dwDigiCorePrice;
                public uint s_dwScanPrice;
                public uint s_dwSale;
                public string s_cModel_Nif;
                public string s_cModel_Effect;
                public byte s_bModel_Loop;
                public byte s_bModel_Shader;
                public ushort s_nSkillCodeType;
                public uint s_dwSkill;
                public byte s_btApplyRateMax;
                public byte s_btApplyRateMin;
                public byte s_btApplyElement;
                public byte uktest;
                public ushort s_nSocketCount;
                public ushort s_dwSoundID;
                public byte s_nBelonging;
                public byte[] unk2;
                public uint s_nQuest1;
                public uint s_nQuest2;
                public uint s_nQuest3;
                public byte s_nDigiviceSkillSlot;
                public byte s_nDigiviceChipsetSlot;
                public byte[] unk3;
                public uint s_nQuestRequire;
                public byte s_btUseTimeType;
                public byte[] unk4;
                public uint s_nUseTime_Min;
                public byte s_nUseBattle;
                public byte unks;
                public ushort s_nDoNotUseType;
                public byte s_bUseTimeType;
                public byte[] unkss;
            }

            public sINFO[] index;
        }


        public static void Main(string filepath, BitReader reader, bool fromServer)
        {


            // Ler o valor de icount
            int icount = reader.ReadInt();

            // Inicializar o array index com o tamanho icount
            ITEM.sINFO[] index = new ITEM.sINFO[icount];

            for (int i = 0; i < icount; i++)
            {
                // Ler cada campo da estrutura sINFO
                ITEM.sINFO info = new ITEM.sINFO();
                info.s_dwItemID = reader.ReadUInt();            
                byte[] s_szName = reader.ReadBytes(128);
                string s_szNameString = System.Text.Encoding.Unicode.GetString(s_szName, 0, 128);
                info.s_szName = CleanString(s_szNameString);

                if(fromServer)
                {
                    info.s_szName = StripUnicodeCharactersFromString(info.s_szName);
                }
                

                info.s_nIcon = reader.ReadUInt();
                byte[] s_szCommentBytes = reader.ReadBytes(1024);
                string s_szComment = System.Text.Encoding.Unicode.GetString(s_szCommentBytes, 0, 1024);
                info.s_szComment = CleanString(s_szComment);

                byte[] s_cNifBytes = reader.ReadBytes(64);
                string s_cNif = System.Text.Encoding.UTF8.GetString(s_cNifBytes, 0, 64);
                info.s_cNif = CleanString(s_cNif);
                info.s_nClass = reader.ReadUShort();

                byte[] s_szTypeCommentBytes = reader.ReadBytes(128);
                string s_szTypeComment = System.Text.Encoding.Unicode.GetString(s_szTypeCommentBytes, 0, 128);
                info.s_szTypeComment = CleanString(s_szTypeComment);

                info.s_btCodeTag = reader.ReadByte();
                info.unkt = reader.ReadByte();
                info.s_nType_L = reader.ReadUShort();
                info.s_nType_S = reader.ReadUShort();
                info.s_nTypeValue = reader.ReadUInt();
                info.s_nSection = reader.ReadUInt();
                info.s_nSellType = reader.ReadUShort();
                info.s_nUseMode = reader.ReadByte();
                info.unkr = reader.ReadByte();
                info.s_nUseTimeGroup = reader.ReadUShort();
                info.s_nOverlap = reader.ReadUShort();
                info.s_nTamerReqMinLevel = reader.ReadUShort();
                info.s_nTamerReqMaxLevel = reader.ReadUShort();
                info.s_nDigimonReqMinLevel = reader.ReadUShort();
                info.s_nDigimonReqMaxLevel = reader.ReadUShort();
                info.s_nPossess = reader.ReadUShort();
                info.s_nEquipSeries = reader.ReadUShort();
                info.s_nUseCharacter = reader.ReadUShort();
                info.s_bDummy = reader.ReadByte();
                info.uktest = reader.ReadByte();
                info.s_nDrop = reader.ReadUShort();
                info.ukteste1 = reader.ReadUShort();
                info.s_nEventItemType = reader.ReadUInt();
                info.s_dwEventItemPrice = reader.ReadUShort();
                info.s_dwDigiCorePrice = reader.ReadUShort();
                info.s_dwScanPrice = reader.ReadUInt();

                info.s_dwSale = reader.ReadUInt();
                byte[] s_cModel_NifBytes = reader.ReadBytes(64);
                string s_cModel_Nif = System.Text.Encoding.UTF8.GetString(s_cModel_NifBytes, 0, 64);
                info.s_cModel_Nif = CleanString(s_cModel_Nif);

                byte[] s_cModel_EffectBytes = reader.ReadBytes(64);
                string s_cModel_Effect = System.Text.Encoding.UTF8.GetString(s_cModel_EffectBytes, 0, 64);
                info.s_cModel_Effect = CleanString(s_cModel_Effect);

                info.s_bModel_Loop = reader.ReadByte();
                info.s_bModel_Shader = reader.ReadByte();
                info.s_nSkillCodeType = reader.ReadUShort();
                info.s_dwSkill = reader.ReadUInt();
                info.s_btApplyRateMax = reader.ReadByte();
                info.s_btApplyRateMin = reader.ReadByte();
                info.s_btApplyElement = reader.ReadByte();
                info.unk = reader.ReadByte();
                info.s_nSocketCount = reader.ReadUShort();
                info.s_dwSoundID = reader.ReadUShort();
                info.s_nBelonging = reader.ReadByte();
                info.unk2 = reader.ReadBytes(3);
                info.s_nQuest1 = reader.ReadUInt();
                info.s_nQuest2 = reader.ReadUInt();
                info.s_nQuest3 = reader.ReadUInt();
                info.s_nDigiviceSkillSlot = reader.ReadByte();
                info.s_nDigiviceChipsetSlot = reader.ReadByte();
                info.unk3 = reader.ReadBytes(2);
                info.s_nQuestRequire = reader.ReadUInt();
                info.s_btUseTimeType = reader.ReadByte();
                info.unk4 = reader.ReadBytes(3);
                info.s_nUseTime_Min = reader.ReadUInt();
                info.s_nUseBattle = reader.ReadByte();
                info.unks = reader.ReadByte();
                info.s_nDoNotUseType = reader.ReadUShort();
                info.s_bUseTimeType = reader.ReadByte();
                info.unkss = reader.ReadBytes(3);

                // Adicionar a sINFO ao array index
                index[i] = info;
            }

            // Criar uma instância de ITEM e atribuir os valores lidos
            ITEM item = new ITEM();
            item.icount = icount;
            item.index = index;

            // Criar um elemento raiz para o XML
            XElement rootElement = new XElement("ITEM");

            // Adicionar o elemento icount ao XML
            rootElement.Add(new XElement("icount", item.icount));

            // Criar um elemento index para armazenar os elementos sINFO
            XElement indexElement = new XElement("index");

            // Iterar sobre cada sINFO e adicionar ao elemento index
            foreach (var info in item.index)
            {
                // Criar um elemento sINFO para cada estrutura
                XElement infoElement = new XElement("sINFO");

                // Adicionar os campos da estrutura como elementos ao sINFO
                infoElement.Add(new XElement("s_dwItemID", info.s_dwItemID));
                infoElement.Add(new XElement("s_szName", info.s_szName));
                infoElement.Add(new XElement("s_nIcon", info.s_nIcon));
                infoElement.Add(new XElement("s_szComment", info.s_szComment));
                infoElement.Add(new XElement("s_cNif", info.s_cNif));
                infoElement.Add(new XElement("s_nClass", info.s_nClass));
                infoElement.Add(new XElement("s_szTypeComment", info.s_szTypeComment));
                infoElement.Add(new XElement("s_btCodeTag", info.s_btCodeTag));
                infoElement.Add(new XElement("unkt", info.unkt));
                infoElement.Add(new XElement("s_nType_L", info.s_nType_L));
                infoElement.Add(new XElement("s_nType_S", info.s_nType_S));
                infoElement.Add(new XElement("s_nTypeValue", info.s_nTypeValue));
                infoElement.Add(new XElement("s_nSection", info.s_nSection));
                infoElement.Add(new XElement("s_nSellType", info.s_nSellType));
                infoElement.Add(new XElement("s_nUseMode", info.s_nUseMode));
                infoElement.Add(new XElement("unkr", info.unkr));
                infoElement.Add(new XElement("s_nUseTimeGroup", info.s_nUseTimeGroup));
                infoElement.Add(new XElement("s_nOverlap", info.s_nOverlap));
                infoElement.Add(new XElement("s_nTamerReqMinLevel", info.s_nTamerReqMinLevel));
                infoElement.Add(new XElement("s_nTamerReqMaxLevel", info.s_nTamerReqMaxLevel));
                infoElement.Add(new XElement("s_nDigimonReqMinLevel", info.s_nDigimonReqMinLevel));
                infoElement.Add(new XElement("s_nDigimonReqMaxLevel", info.s_nDigimonReqMaxLevel));
                infoElement.Add(new XElement("s_nPossess", info.s_nPossess));
                infoElement.Add(new XElement("s_nEquipSeries", info.s_nEquipSeries));
                infoElement.Add(new XElement("s_nUseCharacter", info.s_nUseCharacter));
                infoElement.Add(new XElement("s_bDummy", info.s_bDummy));
                infoElement.Add(new XElement("ukteste1", info.ukteste1));
                infoElement.Add(new XElement("unk", info.unk));
                infoElement.Add(new XElement("s_nDrop", info.s_nDrop));
                infoElement.Add(new XElement("uktest", info.uktest));
                infoElement.Add(new XElement("s_nEventItemType", info.s_nEventItemType));
                infoElement.Add(new XElement("s_dwEventItemPrice", info.s_dwEventItemPrice));
                infoElement.Add(new XElement("s_dwDigiCorePrice", info.s_dwDigiCorePrice));
                infoElement.Add(new XElement("s_dwScanPrice", info.s_dwScanPrice));
                infoElement.Add(new XElement("s_dwSale", info.s_dwSale));
                infoElement.Add(new XElement("s_cModel_Nif", info.s_cModel_Nif));
                infoElement.Add(new XElement("s_cModel_Effect", info.s_cModel_Effect));
                infoElement.Add(new XElement("s_bModel_Loop", info.s_bModel_Loop));
                infoElement.Add(new XElement("s_bModel_Shader", info.s_bModel_Shader));
                infoElement.Add(new XElement("s_nSkillCodeType", info.s_nSkillCodeType));
                infoElement.Add(new XElement("s_dwSkill", info.s_dwSkill));
                infoElement.Add(new XElement("s_btApplyRateMax", info.s_btApplyRateMax));
                infoElement.Add(new XElement("s_btApplyRateMin", info.s_btApplyRateMin));
                infoElement.Add(new XElement("s_btApplyElement", info.s_btApplyElement));
                infoElement.Add(new XElement("unk", info.unk));
                infoElement.Add(new XElement("s_nSocketCount", info.s_nSocketCount));
                infoElement.Add(new XElement("s_dwSoundID", info.s_dwSoundID));
                infoElement.Add(new XElement("s_nBelonging", info.s_nBelonging));
                infoElement.Add(new XElement("unk2", Convert.ToBase64String(info.unk2)));
                infoElement.Add(new XElement("s_nQuest1", info.s_nQuest1));
                infoElement.Add(new XElement("s_nQuest2", info.s_nQuest2));
                infoElement.Add(new XElement("s_nQuest3", info.s_nQuest3));
                infoElement.Add(new XElement("s_nDigiviceSkillSlot", info.s_nDigiviceSkillSlot));
                infoElement.Add(new XElement("s_nDigiviceChipsetSlot", info.s_nDigiviceChipsetSlot));
                infoElement.Add(new XElement("unk3", Convert.ToBase64String(info.unk3)));
                infoElement.Add(new XElement("s_nQuestRequire", info.s_nQuestRequire));
                infoElement.Add(new XElement("s_btUseTimeType", info.s_btUseTimeType));
                infoElement.Add(new XElement("unk4", Convert.ToBase64String(info.unk4)));
                infoElement.Add(new XElement("s_nUseTime_Min", info.s_nUseTime_Min));
                infoElement.Add(new XElement("s_nUseBattle", info.s_nUseBattle));
                infoElement.Add(new XElement("unks", info.unks));
                infoElement.Add(new XElement("s_nDoNotUseType", info.s_nDoNotUseType));
                infoElement.Add(new XElement("s_bUseTimeType", info.s_bUseTimeType));
                infoElement.Add(new XElement("unkss", Convert.ToBase64String(info.unkss)));

                // Adicionar o elemento sINFO ao elemento index
                indexElement.Add(infoElement);
            }

            // Adicionar o elemento index ao elemento raiz
            rootElement.Add(indexElement);

            // Criar o documento XML
            XDocument xmlDocument = new XDocument(rootElement);

            // Salvar o documento XML no arquivo,
            if (fromServer)
            {
                xmlDocument.Save("XML\\ServerItemList.xml");
            }
            else
            {
                xmlDocument.Save("XML\\ItemList.xml");

            }

        }

        
        public static (ItemTap[], ItemCoolTime[], ItemDisplay[], ItemTypeName[], ItemRank[], ElementItem[], ElementItem[], ItemExchange[], Accessory[], AccessoryEnchant[], ItemMaking, ItemMakingGroup[], XaiItem[], ItemRankEffect[], LookItem[]) ReadItemsFromBinary(string inputFile,bool fromServer)
        {

            using (BitReader reader = new BitReader(File.OpenRead(inputFile)))
            {
                Main(inputFile, reader,fromServer);
                
                int dcount = reader.ReadInt();
                ItemTap[] itemTaps = new ItemTap[dcount];

                for (int i = 0; i < dcount; i++)
                {
                    ItemTap itemTap = new ItemTap();

                    itemTap.s_nSellClass = reader.ReadUShort();
                    itemTap.s_szName = reader.ReadZString(Encoding.Unicode, 32 * 2);

                    itemTaps[i] = itemTap;
                }

                int tcount = reader.ReadInt();
                ItemCoolTime[] CoolTime = new ItemCoolTime[tcount];

                for (int i = 0; i < tcount; i++)
                {
                    ItemCoolTime coolTime = new ItemCoolTime();

                    coolTime.s_nGroupID = reader.ReadInt();
                    coolTime.TimeGroup = new byte[12];

                    for (int x = 0; x < coolTime.TimeGroup.Length; x++)
                    {
                        coolTime.TimeGroup[x] = reader.ReadByte();
                    }

                    CoolTime[i] = coolTime;
                }

                int xcount = reader.ReadInt();
                ItemDisplay[] itemDisplay = new ItemDisplay[xcount];

                for (int i = 0; i < xcount; i++)
                {
                    ItemDisplay Display = new ItemDisplay();

                    Display.nItemS = reader.ReadInt();
                    Display.dwDispID = reader.ReadInt();

                    itemDisplay[i] = Display;
                }

                int scount = reader.ReadInt();
                ItemTypeName[] itemTypeName = new ItemTypeName[scount];

                for (int i = 0; i < scount; i++)
                {
                    ItemTypeName ItemType = new ItemTypeName();
                    ItemType.s_szId = reader.ReadInt();
                    ItemType.s_szName = reader.ReadZString(Encoding.Unicode, 128);

                    itemTypeName[i] = ItemType;
                }

                int wcount = reader.ReadInt();
                ItemRank[] ItemRank = new ItemRank[wcount];

                for (int i = 0; i < wcount; i++)
                {
                    ItemRank ItemType = new ItemRank();
                    ItemType.ID = reader.ReadInt();
                    ItemType.Drop_Class = reader.ReadShort();
                    ItemType.Drop_Count = reader.ReadShort();

                    ItemRank[i] = ItemType;
                }

                int swcount = reader.ReadInt();
                ElementItem[] Element = new ElementItem[swcount];

                for (int i = 0; i < swcount; i++)
                {
                    ElementItem ItemType = new ElementItem();
                    ItemType.s_dwItemID = reader.ReadInt();
                    Element[i] = ItemType;
                }

                int dwcount = reader.ReadInt();
                ElementItem[] Element1 = new ElementItem[dwcount];

                for (int i = 0; i < dwcount; i++)
                {
                    ElementItem ItemType = new ElementItem();
                    ItemType.s_dwItemID = reader.ReadInt();
                    Element1[i] = ItemType;
                }

                int wwcount = reader.ReadInt();
                _ = reader.ReadInt();

                ItemExchange[] Exchange = new ItemExchange[wwcount];

                for (int i = 0; i < wwcount; i++)
                {
                    ItemExchange exchange = new ItemExchange();
                    exchange.s_dwNpcID = reader.ReadInt();
                    exchange.s_dwItemIndex = reader.ReadShort();
                    exchange.unk = reader.ReadShort();
                    exchange.s_dwItemID = reader.ReadInt();
                    exchange.s_dwExchange_Code_A = reader.ReadInt();
                    exchange.s_dwExchange_Code_B = reader.ReadInt();
                    exchange.s_dwExchange_Code_C = reader.ReadInt();
                    exchange.s_dwExchange_Code_D = reader.ReadInt();
                    exchange.s_dwPropertyA_Price = reader.ReadShort();
                    exchange.s_dwPropertyB_Price = reader.ReadShort();
                    exchange.s_dwPropertyC_Price = reader.ReadShort();
                    exchange.s_dwPropertyD_Price = reader.ReadShort();
                    exchange.s_dwCount = reader.ReadUShort();
                    exchange.unk1 = reader.ReadShort();
                    exchange.unk2 = reader.ReadInt();

                    Exchange[i] = exchange;
                }

                Exchange[0].nullo = 0;



                Accessory[] Acessorys = new Accessory[184];

                for (int i = 0; i < 184; i++)
                {
                    Accessory acessory = new();
                    acessory.index_Accessory1 = reader.ReadInt();
                    acessory.index_Accessory = reader.ReadInt();
                    acessory.Gain_Option = reader.ReadShort();
                    acessory.Changeable_Option_Number = reader.ReadShort();

                    for (int x = 0; x < 16; x++) //maxList
                    {
                        AccessoryOption options = new();

                        options.s_nOptIdx = reader.ReadShort();
                        options.unknow = reader.ReadShort();
                        options.s_nMin = reader.ReadUInt();
                        options.s_nMax = reader.ReadUInt();


                        acessory.Option.Add(options);

                    }
                    Acessorys[i] = acessory;
                }

                int EnchantCount = reader.ReadInt();
                AccessoryEnchant[] Enchant = new AccessoryEnchant[EnchantCount];

                for (int i = 0; i < EnchantCount; i++)
                {
                    AccessoryEnchant enchant = new()
                    {
                        ID = reader.ReadInt(),
                        Index_Enchant = reader.ReadInt(),
                        Enchant_Option = reader.ReadShort(),
                        Factor1 = reader.ReadShort()
                    };

                    Enchant[i] = enchant;
                }

                ItemMaking list = new ItemMaking();

                list.count = reader.ReadInt();
                list.index = new NPC[list.count];

                for (int i = 0; i < list.count; i++)
                {
                    list.index[i] = new NPC();
                    list.index[i].m_dwNpcIdx = reader.ReadInt();
                    list.index[i].m_mapMainCategoty = reader.ReadInt();

                    list.index[i].index = new Abar[list.index[i].m_mapMainCategoty];

                    for (int j = 0; j < list.index[i].m_mapMainCategoty; j++)
                    {
                        list.index[i].index[j] = new Abar();
                        list.index[i].index[j].ID = reader.ReadInt();
                        list.index[i].index[j].ID1 = reader.ReadInt();
                        list.index[i].index[j].CarteSize = reader.ReadInt() * 2;

                        byte[] mname_get = new byte[list.index[i].index[j].CarteSize];

                        for (int g = 0; g < list.index[i].index[j].CarteSize; g++)
                            mname_get[g] = reader.ReadByte();

                        list.index[i].index[j].Abaname = Encoding.Unicode.GetString(mname_get).Trim();
                        list.index[i].index[j].Abaname = list.index[i].index[j].Abaname.Replace("\0", string.Empty);
                        list.index[i].index[j].size_mapSubCategoty = reader.ReadInt();
                        list.index[i].index[j].index = new SubCategoty[list.index[i].index[j].size_mapSubCategoty];

                        for (int k = 0; k < list.index[i].index[j].size_mapSubCategoty; k++)
                        {
                            list.index[i].index[j].index[k] = new SubCategoty();
                            list.index[i].index[j].index[k].ID = reader.ReadInt();
                            list.index[i].index[j].index[k].ID1 = reader.ReadInt();
                            list.index[i].index[j].index[k].SizeNameCate = reader.ReadInt() * 2;

                            byte[] mnamee_get = new byte[list.index[i].index[j].index[k].SizeNameCate];

                            for (int g = 0; g < list.index[i].index[j].index[k].SizeNameCate; g++)
                                mnamee_get[g] = reader.ReadByte();

                            list.index[i].index[j].index[k].Name = Encoding.Unicode.GetString(mnamee_get).Trim();
                            list.index[i].index[j].index[k].Name = list.index[i].index[j].index[k].Name.Replace("\0", string.Empty);
                            list.index[i].index[j].index[k].fcount = reader.ReadInt();

                            list.index[i].index[j].index[k].index = new itemMake[list.index[i].index[j].index[k].fcount];

                            for (int l = 0; l < list.index[i].index[j].index[k].fcount; l++)
                            {
                                list.index[i].index[j].index[k].index[l] = new itemMake();
                                list.index[i].index[j].index[k].index[l].m_nUniqueIdx = reader.ReadInt();
                                list.index[i].index[j].index[k].index[l].m_dwItemIdx = reader.ReadInt();
                                list.index[i].index[j].index[k].index[l].m_nItemNum = reader.ReadInt();
                                list.index[i].index[j].index[k].index[l].m_nProbabilityofSuccess = reader.ReadInt();
                                list.index[i].index[j].index[k].index[l].ink = reader.ReadInt();
                                list.index[i].index[j].index[k].index[l].unk = reader.ReadInt();
                                list.index[i].index[j].index[k].index[l].Valor = reader.ReadInt();
                                list.index[i].index[j].index[k].index[l].m_dwItemCost = reader.ReadInt();

                                int materialListLength = list.index[i].index[j].index[k].index[l].m_dwItemCost;

                                list.index[i].index[j].index[k].index[l].index = new MaterialList[materialListLength];

                                for (int m = 0; m < materialListLength; m++)
                                {
                                    list.index[i].index[j].index[k].index[l].index[m] = new MaterialList();
                                    list.index[i].index[j].index[k].index[l].index[m].m_dwItemIdx = reader.ReadInt();
                                    list.index[i].index[j].index[k].index[l].index[m].m_nItemNum = reader.ReadInt();
                                }
                            }
                        }
                    }
                }


                using (StreamWriter writer = new StreamWriter("arquivo.txt"))
                {
                    foreach (NPC npc in list.index)
                    {
                        foreach (Abar abar in npc.index)
                        {
                            foreach (SubCategoty subCategory in abar.index)
                            {
                                var valoresOrdenados = subCategory.index
                                    .OrderBy(item => item.m_nUniqueIdx)
                                    .Select(item => item.m_nUniqueIdx);

                                foreach (int valor in valoresOrdenados)
                                {
                                    writer.WriteLine(valor);
                                }
                            }
                        }
                    }
                }




                int GrouCount = reader.ReadInt();

                ItemMakingGroup[] Group = new ItemMakingGroup[GrouCount];

                for (int d = 0; d < GrouCount; d++)
                {
                    ItemMakingGroup group = new()
                    {
                        Index = reader.ReadInt(),
                        Type_No = reader.ReadInt(),
                        Item_Num = reader.ReadInt(),
                        Item_Code = reader.ReadInt(),
                        Item_Num1 = reader.ReadInt()
                    };

                    Group[d] = group;
                }

                int XAICount = reader.ReadInt();
                XaiItem[] Xai = new XaiItem[XAICount];

                for (int i = 0; i < XAICount; i++)
                {
                    XaiItem xai = new XaiItem();
                    xai.ItemID = reader.ReadInt();
                    xai.XGauge = reader.ReadInt();
                    xai.MaxCrystal = reader.ReadByte();

                    Xai[i] = xai;
                }

                int RankCount = reader.ReadInt();
                ItemRankEffect[] ItemRankEffect = new ItemRankEffect[RankCount];
                for (int q = 0; q < RankCount; q++)
                {
                    ItemRankEffect effect = new();

                    effect.nItemCode = reader.ReadInt();
                    effect.nInterval = reader.ReadInt();

                    for (int i = 0; i < effect.nInterval; i++)
                    {
                        ItemRankEffectInterval interval = new ItemRankEffectInterval();
                        interval.dwItemCode = reader.ReadInt();
                        interval.IconNo = reader.ReadInt();
                        interval.Rank = reader.ReadInt();
                        effect.Interval.Add(interval);
                    }

                    ItemRankEffect[q] = effect;
                }
                int swount = reader.ReadInt();
                LookItem[] Look = new LookItem[swcount];
                for (int u = 0; u < swount; u++)
                {
                    LookItem look = new()
                    {
                        Item_Code = reader.ReadInt(),
                        Di_No = reader.ReadInt(),
                        Change_Type = reader.ReadInt()
                    };

                    look.NameSize = reader.ReadInt();
                    byte[] mname_get = new byte[look.NameSize];

                    for (int g = 0; g < look.NameSize; g++)
                        mname_get[g] = reader.ReadByte();

                    look.File_Name = Encoding.ASCII.GetString(mname_get).Trim();

                    Look[u] = look;
                }

                return (itemTaps, CoolTime, itemDisplay, itemTypeName, ItemRank, Element, Element1, Exchange, Acessorys, Enchant, list, Group, Xai, ItemRankEffect, Look);
            }
        }
        public static void ExportItemToXml(string outputFile, Item[] items)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(outputFile, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("ItemList");

                foreach (Item item in items)
                {
                    writer.WriteStartElement("Item");

                    WriteElement(writer, "s_dwItemID", item.s_dwItemID);
                    WriteElement(writer, "s_szName", item.s_szName);
                    WriteElement(writer, "s_nIcon", item.s_nIcon);
                    WriteElement(writer, "s_szComment", item.s_szComment);
                    WriteElement(writer, "s_cNif", item.s_cNif);
                    WriteElement(writer, "Item_Class", item.Item_Class);
                    WriteElement(writer, "s_szTypeComment", item.s_szTypeComment);
                    WriteElement(writer, "unknow1", item.unknow1);
                    WriteElement(writer, "s_nType_L", item.s_nType_L);
                    WriteElement(writer, "s_nType_S", item.s_nType_S);
                    WriteElement(writer, "s_nTypeValue", item.s_nTypeValue);
                    WriteElement(writer, "Section", item.Section);
                    WriteElement(writer, "s_nSellType", item.s_nSellType);
                    WriteElement(writer, "unknow2", item.unknow2);
                    WriteElement(writer, "s_nUseTimeGroup", item.s_nUseTimeGroup);
                    WriteElement(writer, "s_nOverlap", item.s_nOverlap);
                    WriteElement(writer, "s_nTamerReqMinLevel", item.s_nTamerReqMinLevel);
                    WriteElement(writer, "s_nTamerReqMaxLevel", item.s_nTamerReqMaxLevel);
                    WriteElement(writer, "s_nDigimonReqMinLevel", item.s_nDigimonReqMinLevel);
                    WriteElement(writer, "s_nDigimonReqMaxLevel", item.s_nDigimonReqMaxLevel);
                    WriteElement(writer, "s_nPossess", item.s_nPossess);
                    WriteElement(writer, "s_nEquipSeries", item.s_nEquipSeries);
                    WriteElement(writer, "s_nUseCharacter", item.s_nUseCharacter);
                    WriteElement(writer, "unknow3", item.unknow3);
                    WriteElement(writer, "s_nDrop", item.s_nDrop);
                    WriteElement(writer, "unknow4", item.unknow4);
                    WriteElement(writer, "s_dwEventItemPrice", item.s_dwEventItemPrice);
                    WriteElement(writer, "s_dwDigiCorePrice", item.s_dwDigiCorePrice);
                    WriteElement(writer, "Item_Digicore", item.Item_Digicore);
                    WriteElement(writer, "s_dwScanPrice", item.s_dwScanPrice);
                    WriteElement(writer, "s_dwSale", item.s_dwSale);
                    WriteElement(writer, "s_cModel_Nif", item.s_cModel_Nif);
                    WriteElement(writer, "s_cModel_Effect", item.s_cModel_Effect);
                    WriteElement(writer, "s_bModel_Loop", item.s_bModel_Loop);
                    WriteElement(writer, "s_bModel_Shader", item.s_bModel_Shader);
                    WriteElement(writer, "s_nSkillCodeType", item.s_nSkillCodeType);
                    WriteElement(writer, "s_dwSkill", item.s_dwSkill);
                    WriteElement(writer, "s_btApplyRateMax", item.s_btApplyRateMax);
                    WriteElement(writer, "s_btApplyRateMin", item.s_btApplyRateMin);
                    WriteElement(writer, "s_btApplyElement", item.s_btApplyElement);
                    WriteElement(writer, "unknow", item.unknow);
                    WriteElement(writer, "s_nSocketCount", item.s_nSocketCount);
                    WriteElement(writer, "s_dwSoundID", item.s_dwSoundID);
                    WriteElement(writer, "item_Belonging", item.item_Belonging);
                    WriteElement(writer, "s_nQuest1", item.s_nQuest1);
                    WriteElement(writer, "s_nQuest2", item.s_nQuest2);
                    WriteElement(writer, "s_nQuest3", item.s_nQuest3);
                    WriteElement(writer, "s_nDigiviceSkillSlot", item.s_nDigiviceSkillSlot);
                    WriteElement(writer, "s_nDigiviceChipsetSlot", item.s_nDigiviceChipsetSlot);
                    WriteElement(writer, "s_nQuestRequire", item.s_nQuestRequire);
                    WriteElement(writer, "Use_Item_Type", item.Use_Item_Type);
                    WriteElement(writer, "unknow5", item.unknow5);
                    WriteElement(writer, "unknow6", item.unknow6);
                    WriteElement(writer, "unknow7", item.unknow7);
                    WriteElement(writer, "UseTimeMinutes", item.UseTimeMinutes);
                    WriteElement(writer, "s_nUseBattle", item.s_nUseBattle);
                    WriteElement(writer, "s_nDoNotUseType", item.s_nDoNotUseType);
                    WriteElement(writer, "s_bUseTimeType", item.s_bUseTimeType);
                    WriteElement(writer, "unknow8", item.unknow8);

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }


            void WriteElement(XmlWriter writer, string elementName, object elementValue)
            {

                writer.WriteElementString(elementName, elementValue.ToString());
            }
        }
        public static void ExportItemTapToXml(string outputFile, ItemTap[] items)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(outputFile, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("ItemTap");

                foreach (ItemTap item in items)
                {
                    writer.WriteStartElement("Item");
                    WriteElement(writer, "s_nSellClass", item.s_nSellClass);
                    WriteElement(writer, "s_szName", item.s_szName);

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            void WriteElement(XmlWriter writer, string elementName, object elementValue)
            {
                writer.WriteElementString(elementName, elementValue.ToString());
            }
        }
        public static void ExportCoolTimeToXml(string outputFile, ItemCoolTime[] items)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(outputFile, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("ItemCoolTime");

                foreach (ItemCoolTime item in items)
                {
                    if (item != null)
                    {
                        writer.WriteStartElement("Item");
                        WriteElement(writer, "s_nGroupID", item.s_nGroupID);
                        WriteByteArrayElement(writer, "TimeGroup", item.TimeGroup);

                        writer.WriteEndElement();
                    }
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        private static void WriteElement(XmlWriter writer, string elementName, object elementValue)
        {
            writer.WriteElementString(elementName, elementValue.ToString());
        }

        private static void WriteByteArrayElement(XmlWriter writer, string elementName, byte[] byteArray)
        {
            StringBuilder hexBuilder = new StringBuilder();

            foreach (byte b in byteArray)
            {
                hexBuilder.Append(b.ToString("X2"));
            }

            writer.WriteElementString(elementName, hexBuilder.ToString());
        }

        public static void ExportDisplayItemsToXml(string outputFile, ItemDisplay[] items)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(outputFile, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("ItemDisplay");

                foreach (ItemDisplay item in items)
                {
                    if (item != null)
                    {
                        writer.WriteStartElement("Item");
                        WriteElement(writer, "nItemS", item.nItemS);
                        WriteElement(writer, "dwDispID", item.dwDispID);

                        writer.WriteEndElement();
                    }

                }

                writer.WriteEndElement();
                writer.WriteEndDocument();

            }

            void WriteElement(XmlWriter writer, string elementName, object elementValue)
            {
                writer.WriteElementString(elementName, elementValue.ToString());
            }
        }
        public static void ExportItemTypeNamesToXml(string outputFile, ItemTypeName[] items)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(outputFile, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("ItemTypeName");

                foreach (ItemTypeName item in items)
                {
                    writer.WriteStartElement("Item");
                    WriteElement(writer, "s_szId", item.s_szId);
                    WriteElement(writer, "s_szName", item.s_szName);

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            void WriteElement(XmlWriter writer, string elementName, object elementValue)
            {
                writer.WriteElementString(elementName, elementValue.ToString());
            }
        }
        public static void ExportItemRankListToXml(string outputFile, ItemRank[] items)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(outputFile, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("ItemRank");

                foreach (ItemRank item in items)
                {
                    writer.WriteStartElement("Item");
                    WriteElement(writer, "ID", item.ID);
                    WriteElement(writer, "Drop_Class", item.Drop_Class);
                    WriteElement(writer, "Drop_Count", item.Drop_Count);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            void WriteElement(XmlWriter writer, string elementName, object elementValue)
            {
                writer.WriteElementString(elementName, elementValue.ToString());
            }
        }
        public static void ExportItemElementToXml(string outputFile, ElementItem[] items)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(outputFile, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("ItemElement");

                foreach (ElementItem item in items)
                {
                    writer.WriteStartElement("Item");
                    WriteElement(writer, "s_dwItemID", item.s_dwItemID);

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            void WriteElement(XmlWriter writer, string elementName, object elementValue)
            {
                writer.WriteElementString(elementName, elementValue.ToString());
            }
        }
        public static void ExportItemExchangesToXml(string outputFile, ItemExchange[] items)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(outputFile, settings))
            {


                writer.WriteStartDocument();
                writer.WriteStartElement("ItemExchange");
                WriteElement(writer, "nullo", items[0].nullo);

                foreach (ItemExchange item in items)
                {
                    writer.WriteStartElement("Item");
                    WriteElement(writer, "s_dwNpcID", item.s_dwNpcID);
                    WriteElement(writer, "s_dwItemIndex", item.s_dwItemIndex);
                    WriteElement(writer, "unk", item.unk);
                    WriteElement(writer, "s_dwItemID", item.s_dwItemID);
                    WriteElement(writer, "s_dwExchange_Code_A", item.s_dwExchange_Code_A);
                    WriteElement(writer, "s_dwExchange_Code_B", item.s_dwExchange_Code_B);
                    WriteElement(writer, "s_dwExchange_Code_C", item.s_dwExchange_Code_C);
                    WriteElement(writer, "s_dwExchange_Code_D", item.s_dwExchange_Code_D);
                    WriteElement(writer, "s_dwPropertyA_Price", item.s_dwPropertyA_Price);
                    WriteElement(writer, "s_dwPropertyB_Price", item.s_dwPropertyB_Price);
                    WriteElement(writer, "s_dwPropertyC_Price", item.s_dwPropertyC_Price);
                    WriteElement(writer, "s_dwPropertyD_Price", item.s_dwPropertyD_Price);
                    WriteElement(writer, "s_dwCount", item.s_dwCount);
                    WriteElement(writer, "unk1", item.unk1);
                    WriteElement(writer, "unk2", item.unk2);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            void WriteElement(XmlWriter writer, string elementName, object elementValue)
            {
                writer.WriteElementString(elementName, elementValue.ToString());
            }
        }
        public static void ExportAcessorysItemsToXml(string outputFile, Accessory[] items)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(outputFile, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("ItemAcessory");

                foreach (Accessory item in items)
                {
                    writer.WriteStartElement("Item");
                    WriteElement(writer, "index_Accessory1", item.index_Accessory1);
                    WriteElement(writer, "index_Accessory", item.index_Accessory);
                    WriteElement(writer, "Gain_Option", item.Gain_Option);
                    WriteElement(writer, "Changeable_Option_Number", item.Changeable_Option_Number);
                    writer.WriteStartElement("Option");
                    foreach (var option in item.Option)
                    {
                        WriteElement(writer, "s_nOptIdx", option.s_nOptIdx);
                        WriteElement(writer, "unknow", option.unknow);
                        WriteElement(writer, "s_nMin", option.s_nOptIdx);
                        WriteElement(writer, "s_nMax", option.s_nMax);
                    }
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            void WriteElement(XmlWriter writer, string elementName, object elementValue)
            {
                writer.WriteElementString(elementName, elementValue.ToString());
            }
        }
        public static void ExportAcessorysEnchantToXml(string outputFile, AccessoryEnchant[] items)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(outputFile, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("ItemEnchant");

                foreach (AccessoryEnchant item in items)
                {

                    writer.WriteStartElement("Item");
                    WriteElement(writer, "ID", item.ID);
                    WriteElement(writer, "Index_Enchant", item.Index_Enchant);
                    WriteElement(writer, "Enchant_Option", item.Enchant_Option);
                    WriteElement(writer, "Index_Enchant", item.Factor1);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            void WriteElement(XmlWriter writer, string elementName, object elementValue)
            {
                writer.WriteElementString(elementName, elementValue.ToString());
            }
        }
        public static void ExportItemMakingToXml(string fileName, ItemMaking data)
        {
            XElement xml = new XElement("ItemMaking",
                new XElement("count", data.count),
                new XElement("index",
                    from npc in data.index
                    select new XElement("NPC",
                        new XElement("m_dwNpcIdx", npc.m_dwNpcIdx),
                        new XElement("m_mapMainCategoty", npc.m_mapMainCategoty),
                        new XElement("index",
                            from abar in npc.index
                            select new XElement("Abar",
                                new XElement("ID", abar.ID),
                                 new XElement("ID1", abar.ID1),
                                new XElement("CarteSize", abar.CarteSize),
                                new XElement("Abaname", abar.Abaname),
                                new XElement("size_mapSubCategoty", abar.size_mapSubCategoty),
                                new XElement("index",
                                    from subCategory in abar.index
                                    select new XElement("SubCategoty",
                                        new XElement("ID", subCategory.ID),
                                          new XElement("ID1", subCategory.ID1),
                                        new XElement("SizeNameCate", subCategory.SizeNameCate),
                                        new XElement("Name", subCategory.Name),
                                        new XElement("fcount", subCategory.fcount),
                                        new XElement("index",
                                            from item in subCategory.index
                                            select new XElement("itemMake",
                                                new XElement("m_nUniqueIdx", item.m_nUniqueIdx),
                                                new XElement("m_dwItemIdx", item.m_dwItemIdx),
                                                new XElement("m_nItemNum", item.m_nItemNum),
                                                new XElement("m_nProbabilityofSuccess", item.m_nProbabilityofSuccess),
                                                new XElement("ink", item.ink),
                                                new XElement("unk", item.unk),
                                                new XElement("Valor", item.Valor),
                                                new XElement("m_dwItemCost", item.m_dwItemCost),
                                                new XElement("index",
                                                    from material in item.index
                                                    select new XElement("MaterialList",
                                                        new XElement("m_dwItemIdx", material.m_dwItemIdx),
                                                        new XElement("m_nItemNum", material.m_nItemNum)
                                                    )
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        )
                    )
                )
            );

            xml.Save(fileName);
        }
        public static void ExportMakingGroupsToXml(string outputFile, ItemMakingGroup[] items)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(outputFile, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("ItemMakingGroup");

                foreach (ItemMakingGroup item in items)
                {

                    writer.WriteStartElement("Item");
                    WriteElement(writer, "Index", item.Index);
                    WriteElement(writer, "Type_No", item.Type_No);
                    WriteElement(writer, "Item_Num", item.Item_Num);
                    WriteElement(writer, "Item_Code", item.Item_Code);
                    WriteElement(writer, "Item_Num1", item.Item_Num1);

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            void WriteElement(XmlWriter writer, string elementName, object elementValue)
            {
                writer.WriteElementString(elementName, elementValue.ToString());
            }
        }
        public static void ExportItemXaiToXml(string outputFile, XaiItem[] items)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(outputFile, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("ItemXai");

                foreach (XaiItem item in items)
                {

                    writer.WriteStartElement("Item");
                    WriteElement(writer, "ItemID", item.ItemID);
                    WriteElement(writer, "XGauge", item.XGauge);
                    WriteElement(writer, "MaxCrystal", item.MaxCrystal);

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            void WriteElement(XmlWriter writer, string elementName, object elementValue)
            {
                writer.WriteElementString(elementName, elementValue.ToString());
            }
        }
        public static void ExportItemRankEffectsToXml(string outputFile, ItemRankEffect[] items)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(outputFile, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("ItemRankEffect");

                foreach (ItemRankEffect item in items)
                {


                    writer.WriteStartElement("Item");
                    WriteElement(writer, "nItemCode", item.nItemCode);
                    WriteElement(writer, "nInterval", item.nInterval);
                    writer.WriteStartElement("nIntervals");
                    foreach (var intervals in item.Interval)
                    {
                        WriteElement(writer, "dwItemCode", intervals.dwItemCode);
                        WriteElement(writer, "IconNo", intervals.IconNo);
                        WriteElement(writer, "Rank", intervals.IconNo);
                    }

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            void WriteElement(XmlWriter writer, string elementName, object elementValue)
            {
                writer.WriteElementString(elementName, elementValue.ToString());
            }
        }
        public static void ExportItemLookToXml(string outputFile, LookItem[] items)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(outputFile, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("ItemLook");

                foreach (LookItem item in items)
                {

                    if (item != null)
                    {
                        writer.WriteStartElement("Item");
                        WriteElement(writer, "Item_Code", item.Item_Code);
                        WriteElement(writer, "Di_No", item.Di_No);
                        WriteElement(writer, "Change_Type", item.Change_Type);
                        WriteElement(writer, "NameSize", item.NameSize);
                        WriteElement(writer, "File_Name", item.File_Name);
                        writer.WriteEndElement();
                    }
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            void WriteElement(XmlWriter writer, string elementName, object elementValue)
            {
                writer.WriteElementString(elementName, elementValue.ToString());
            }
        }
        public static string StripUnicodeCharactersFromString(string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            StringBuilder sb = new();
            foreach (var c in s)
            {
                if ((c >= 'a' && c <= 'z') ||
                    (c >= 'A' && c <= 'Z') ||
                    (c >= '0' && c <= '9') ||
                    c == '-' ||
                    c == '(' ||
                    c == ')' ||
                    c == (char)32
                )
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }
        public static ITEM ImportFromXml(string xmlFilePath)
        {
            // Carregar o documento XML
            XDocument xmlDocument = XDocument.Load(xmlFilePath);

            // Obter o elemento raiz
            XElement rootElement = xmlDocument.Root;

            // Criar uma instância de ITEM
            ITEM item = new ITEM();

            // Ler o valor de icount
            item.icount = int.Parse(rootElement.Element("icount").Value);

            // Obter o elemento index
            XElement indexElement = rootElement.Element("index");

            // Obter todos os elementos sINFO
            var infoElements = indexElement.Elements("sINFO").ToList();

            // Inicializar o array index com o tamanho de icount
            item.index = new ITEM.sINFO[item.icount];

            // Iterar sobre cada elemento sINFO
            for (int i = 0; i < item.icount; i++)
            {
                // Obter o elemento sINFO atual
                XElement infoElement = infoElements[i];

                // Criar uma instância de sINFO
                ITEM.sINFO info = new ITEM.sINFO();

                // Ler os campos da estrutura sINFO do elemento XML
                info.s_dwItemID = uint.Parse(infoElement.Element("s_dwItemID").Value);
                info.s_szName = infoElement.Element("s_szName").Value;
                info.s_nIcon = uint.Parse(infoElement.Element("s_nIcon").Value);
                info.s_szComment = infoElement.Element("s_szComment").Value;
                info.s_cNif = infoElement.Element("s_cNif").Value;
                info.s_nClass = ushort.Parse(infoElement.Element("s_nClass").Value);
                info.s_szTypeComment = infoElement.Element("s_szTypeComment").Value;
                info.s_btCodeTag = byte.Parse(infoElement.Element("s_btCodeTag").Value);
                info.unkt = byte.Parse(infoElement.Element("unkt").Value);
                info.s_nType_L = ushort.Parse(infoElement.Element("s_nType_L").Value);
                info.s_nType_S = ushort.Parse(infoElement.Element("s_nType_S").Value);
                info.s_nTypeValue = uint.Parse(infoElement.Element("s_nTypeValue").Value);
                info.s_nSection = uint.Parse(infoElement.Element("s_nSection").Value);
                info.s_nSellType = ushort.Parse(infoElement.Element("s_nSellType").Value);
                info.s_nUseMode = byte.Parse(infoElement.Element("s_nUseMode").Value);
                info.unkr = byte.Parse(infoElement.Element("unkr").Value);
                info.s_nUseTimeGroup = ushort.Parse(infoElement.Element("s_nUseTimeGroup").Value);
                info.s_nOverlap = ushort.Parse(infoElement.Element("s_nOverlap").Value);
                info.s_nTamerReqMinLevel = ushort.Parse(infoElement.Element("s_nTamerReqMinLevel").Value);
                info.s_nTamerReqMaxLevel = ushort.Parse(infoElement.Element("s_nTamerReqMaxLevel").Value);
                info.s_nDigimonReqMinLevel = ushort.Parse(infoElement.Element("s_nDigimonReqMinLevel").Value);
                info.s_nDigimonReqMaxLevel = ushort.Parse(infoElement.Element("s_nDigimonReqMaxLevel").Value);
                info.s_nPossess = ushort.Parse(infoElement.Element("s_nPossess").Value);
                info.s_nEquipSeries = ushort.Parse(infoElement.Element("s_nEquipSeries").Value);
                info.s_nUseCharacter = ushort.Parse(infoElement.Element("s_nUseCharacter").Value);
                info.s_bDummy = byte.Parse(infoElement.Element("s_bDummy").Value);
                info.uktest = byte.Parse(infoElement.Element("uktest").Value);
                info.s_nDrop = ushort.Parse(infoElement.Element("s_nDrop").Value);
                info.ukteste1 = ushort.Parse(infoElement.Element("ukteste1").Value);
                info.s_nEventItemType = uint.Parse(infoElement.Element("s_nEventItemType").Value);
                info.s_dwEventItemPrice = ushort.Parse(infoElement.Element("s_dwEventItemPrice").Value);
                info.s_dwDigiCorePrice = ushort.Parse(infoElement.Element("s_dwDigiCorePrice").Value);
                info.s_dwScanPrice = uint.Parse(infoElement.Element("s_dwScanPrice").Value);
                info.s_dwSale = uint.Parse(infoElement.Element("s_dwSale").Value);
                info.s_cModel_Nif = infoElement.Element("s_cModel_Nif").Value;
                info.s_cModel_Effect = infoElement.Element("s_cModel_Effect").Value;
                info.s_bModel_Loop = byte.Parse(infoElement.Element("s_bModel_Loop").Value);
                info.s_bModel_Shader = byte.Parse(infoElement.Element("s_bModel_Shader").Value);
                info.s_nSkillCodeType = ushort.Parse(infoElement.Element("s_nSkillCodeType").Value);
                info.s_dwSkill = uint.Parse(infoElement.Element("s_dwSkill").Value);
                info.s_btApplyRateMax = byte.Parse(infoElement.Element("s_btApplyRateMax").Value);
                info.s_btApplyRateMin = byte.Parse(infoElement.Element("s_btApplyRateMin").Value);
                info.s_btApplyElement = byte.Parse(infoElement.Element("s_btApplyElement").Value);
                info.unk = byte.Parse(infoElement.Element("unk").Value);
                info.s_nSocketCount = ushort.Parse(infoElement.Element("s_nSocketCount").Value);
                info.s_dwSoundID = ushort.Parse(infoElement.Element("s_dwSoundID").Value);
                info.s_nBelonging = byte.Parse(infoElement.Element("s_nBelonging").Value);
                info.unk2 = Convert.FromBase64String(infoElement.Element("unk2").Value);
                info.s_nQuest1 = uint.Parse(infoElement.Element("s_nQuest1").Value);
                info.s_nQuest2 = uint.Parse(infoElement.Element("s_nQuest2").Value);
                info.s_nQuest3 = uint.Parse(infoElement.Element("s_nQuest3").Value);
                info.s_nDigiviceSkillSlot = byte.Parse(infoElement.Element("s_nDigiviceSkillSlot").Value);
                info.s_nDigiviceChipsetSlot = byte.Parse(infoElement.Element("s_nDigiviceChipsetSlot").Value);
                info.unk3 = Convert.FromBase64String(infoElement.Element("unk3").Value);
                info.s_nQuestRequire = uint.Parse(infoElement.Element("s_nQuestRequire").Value);
                info.s_btUseTimeType = byte.Parse(infoElement.Element("s_btUseTimeType").Value);
                info.unk4 = Convert.FromBase64String(infoElement.Element("unk4").Value);
                info.s_nUseTime_Min = uint.Parse(infoElement.Element("s_nUseTime_Min").Value);
                info.s_nUseBattle = byte.Parse(infoElement.Element("s_nUseBattle").Value);
                info.unks = byte.Parse(infoElement.Element("unks").Value);
                info.s_nDoNotUseType = ushort.Parse(infoElement.Element("s_nDoNotUseType").Value);
                info.s_bUseTimeType = byte.Parse(infoElement.Element("s_bUseTimeType").Value);
                info.unkss = Convert.FromBase64String(infoElement.Element("unkss").Value);

                // Adicionar a sINFO ao array index
                item.index[i] = info;
            }

            // Retornar a estrutura ITEM
            return item;
        }

        public static ItemTypeName[] ImportItemTypesFromXml(string inputFile)
        {
            List<ItemTypeName> itemTypesList = new List<ItemTypeName>();

            using (XmlReader reader = XmlReader.Create(inputFile))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Item")
                    {
                        ItemTypeName itemType = new ItemTypeName();

                        // Read the properties of the item type from the XML
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                string propertyName = reader.Name;
                                reader.Read(); // Read the value of the property

                                // Set the value of the property in the ItemTypeName object
                                SetProperty(itemType, propertyName, reader.Value);
                            }
                            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Item")
                            {
                                // Add the item type to the list when we reach the end of the "ItemType" element
                                itemTypesList.Add(itemType);
                                break;
                            }
                        }
                    }
                }
            }

            return itemTypesList.ToArray();
        }
        private static void SetProperty(ItemTypeName itemType, string propertyName, string value)
        {
            // Set the value of the property based on the property name
            switch (propertyName)
            {
                case "s_szId":
                    itemType.s_szId = int.Parse(value);
                    break;
                case "s_szName":
                    itemType.s_szName = value;
                    break;
                default:
                    // Unknown property, you can handle it according to your logic
                    break;
            }
        }
        public static ItemTap[] ImportItemTapsFromXml(string inputFile)
        {
            List<ItemTap> itemTapsList = new List<ItemTap>();

            using (XmlReader reader = XmlReader.Create(inputFile))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Item")
                    {
                        ItemTap itemTap = new ItemTap();

                        // Read the properties of the item tap from the XML
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                string propertyName = reader.Name;
                                reader.Read(); // Read the value of the property

                                // Set the value of the property in the ItemTap object
                                SetProperty(itemTap, propertyName, reader.Value);
                            }
                            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Item")
                            {
                                // Add the item tap to the list when we reach the end of the "ItemTap" element
                                itemTapsList.Add(itemTap);
                                break;
                            }
                        }
                    }
                }
            }

            return itemTapsList.ToArray();
        }
        private static void SetProperty(ItemTap itemTap, string propertyName, string value)
        {
            // Set the value of the property based on the property name
            switch (propertyName)
            {
                case "s_nSellClass":
                    itemTap.s_nSellClass = ushort.Parse(value);
                    break;
                case "s_szName":
                    itemTap.s_szName = value;
                    break;
                default:
                    // Unknown property, you can handle it according to your logic
                    break;
            }
        }
        public static ItemCoolTime[] ImportItemCoolTimesFromXml(string inputFile)
        {
            List<ItemCoolTime> itemCoolTimesList = new List<ItemCoolTime>();

            using (XmlReader reader = XmlReader.Create(inputFile))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Item")
                    {
                        ItemCoolTime itemCoolTime = new ItemCoolTime();

                        // Read the properties of the item cool time from the XML
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                string propertyName = reader.Name;
                                reader.Read(); // Read the value of the property

                                // Set the value of the property in the ItemCoolTime object
                                SetProperty(itemCoolTime, propertyName, reader.Value);
                            }
                            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Item")
                            {
                                // Add the item cool time to the list when we reach the end of the "ItemCoolTime" element
                                itemCoolTimesList.Add(itemCoolTime);
                                break;
                            }
                        }
                    }
                }
            }

            return itemCoolTimesList.ToArray();
        }

        private static void SetProperty(ItemCoolTime itemCoolTime, string propertyName, string value)
        {
            // Set the value of the property based on the property name
            switch (propertyName)
            {
                case "s_nGroupID":
                    itemCoolTime.s_nGroupID = int.Parse(value);
                    break;
                case "TimeGroup":
                    itemCoolTime.TimeGroup = ConvertHexStringToByteArray(value);
                    break;

                default:
                    // Unknown property, you can handle it according to your logic
                    break;
            }
        }

        private static byte[] ConvertHexStringToByteArray(string hexString)
        {
            int length = hexString.Length;
            byte[] byteArray = new byte[length / 2];

            for (int i = 0; i < length; i += 2)
            {
                byteArray[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }

            return byteArray;
        }

        public static ItemDisplay[] ImportItemDisplaysFromXml(string inputFile)
        {
            List<ItemDisplay> itemDisplaysList = new List<ItemDisplay>();

            using (XmlReader reader = XmlReader.Create(inputFile))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Item")
                    {
                        ItemDisplay itemDisplay = new ItemDisplay();

                        // Read the properties of the item display from the XML
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                string propertyName = reader.Name;
                                reader.Read(); // Read the value of the property

                                // Set the value of the property in the ItemDisplay object
                                SetProperty(itemDisplay, propertyName, reader.Value);
                            }
                            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Item")
                            {
                                // Add the item display to the list when we reach the end of the "ItemDisplay" element
                                itemDisplaysList.Add(itemDisplay);
                                break;
                            }
                        }
                    }
                }
            }

            return itemDisplaysList.ToArray();
        }
        private static void SetProperty(ItemDisplay itemDisplay, string propertyName, string value)
        {
            // Set the value of the property based on the property name
            switch (propertyName)
            {
                case "nItemS":
                    itemDisplay.nItemS = int.Parse(value);
                    break;
                case "dwDispID":
                    itemDisplay.dwDispID = int.Parse(value);
                    break;
                default:
                    // Unknown property, you can handle it according to your logic
                    break;
            }
        }
        public static ItemRank[] ImportItemRanksFromXml(string inputFile)
        {
            List<ItemRank> itemRankList = new List<ItemRank>();

            using (XmlReader reader = XmlReader.Create(inputFile))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Item")
                    {
                        ItemRank itemRank = new ItemRank();

                        // Read the properties of the ItemRank from XML
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                string propertyName = reader.Name;
                                reader.Read(); // Read the value of the property

                                // Set the value of the property in the ItemRank object
                                SetProperty(itemRank, propertyName, reader.Value);
                            }
                            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Item")
                            {
                                // Add the ItemRank to the list when we reach the end of the "ItemRank" element
                                itemRankList.Add(itemRank);
                                break;
                            }
                        }
                    }
                }
            }

            return itemRankList.ToArray();
        }
        public static string CleanString(string input)
        {
            int nullIndex = input.IndexOf('\0');
            if (nullIndex >= 0)
            {
                return input.Substring(0, nullIndex);
            }
            else
            {
                return input;
            }
        }
        private static void SetProperty(ItemRank itemRank, string propertyName, string value)
        {
            // Set the value of the property based on the property name
            switch (propertyName)
            {
                case "ID":
                    itemRank.ID = int.Parse(value);
                    break;
                case "Drop_Class":
                    itemRank.Drop_Class = short.Parse(value);
                    break;
                case "Drop_Count":
                    itemRank.Drop_Count = short.Parse(value);
                    break;
                default:
                    // Unknown property, you can handle this according to your logic
                    break;
            }
        }
        public static ElementItem[] ImportElementItemsFromXml(string inputFile)
        {
            List<ElementItem> elementItemList = new List<ElementItem>();

            using (XmlReader reader = XmlReader.Create(inputFile))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Item")
                    {
                        ElementItem elementItem = new ElementItem();

                        // Lê as propriedades do ElementItem a partir do XML
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                string propertyName = reader.Name;
                                reader.Read(); // Lê o valor da propriedade

                                // Define o valor da propriedade no objeto ElementItem
                                SetProperty(elementItem, propertyName, reader.Value);
                            }
                            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Item")
                            {
                                // Adiciona o ElementItem à lista quando chegamos ao final do elemento "ElementItem"
                                elementItemList.Add(elementItem);
                                break;
                            }
                        }
                    }
                }
            }

            return elementItemList.ToArray();
        }
        private static void SetProperty(ElementItem elementItem, string propertyName, string value)
        {
            // Define o valor da propriedade com base no nome da propriedade
            switch (propertyName)
            {
                case "s_dwItemID":
                    elementItem.s_dwItemID = int.Parse(value);
                    break;
                default:
                    // Propriedade desconhecida, você pode lidar com isso de acordo com sua lógica
                    break;
            }
        }
        public static ItemExchange[] ImportItemExchangesFromXml(string inputFile)
        {
            List<ItemExchange> itemExchangeList = new List<ItemExchange>();

            using (XmlReader reader = XmlReader.Create(inputFile))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Item")
                    {
                        ItemExchange itemExchange = new ItemExchange();

                        // Lê as propriedades do ItemExchange a partir do XML
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                string propertyName = reader.Name;
                                reader.Read(); // Lê o valor da propriedade

                                if (propertyName == "nullo" && !itemExchange.IsNullItem)
                                {
                                    itemExchange.nullo = int.Parse(reader.Value);
                                    itemExchange.ReadNullItem();
                                }
                                else
                                {
                                    // Define o valor da propriedade no objeto ItemExchange
                                    SetProperty(itemExchange, propertyName, reader.Value);
                                }
                            }
                            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Item")
                            {
                                // Adiciona o ItemExchange à lista quando chegamos ao final do elemento "ItemExchange"
                                itemExchangeList.Add(itemExchange);
                                break;
                            }
                        }
                    }
                }
            }

            return itemExchangeList.ToArray();
        }
        private static void SetProperty(ItemExchange itemExchange, string propertyName, string value)
        {
            // Define o valor da propriedade com base no nome da propriedade
            switch (propertyName)
            {
                case "ItemExchangeNo":
                    itemExchange.ItemExchangeNo = int.Parse(value);
                    break;
                case "s_dwNpcID":
                    itemExchange.s_dwNpcID = int.Parse(value);
                    break;
                case "s_dwItemIndex":
                    itemExchange.s_dwItemIndex = short.Parse(value);
                    break;
                case "unk":
                    itemExchange.unk = short.Parse(value);
                    break;
                case "s_dwItemID":
                    itemExchange.s_dwItemID = int.Parse(value);
                    break;
                case "s_dwExchange_Code_A":
                    itemExchange.s_dwExchange_Code_A = int.Parse(value);
                    break;
                case "s_dwExchange_Code_B":
                    itemExchange.s_dwExchange_Code_B = int.Parse(value);
                    break;
                case "s_dwExchange_Code_C":
                    itemExchange.s_dwExchange_Code_C = int.Parse(value);
                    break;
                case "s_dwExchange_Code_D":
                    itemExchange.s_dwExchange_Code_D = int.Parse(value);
                    break;
                case "s_dwPropertyA_Price":
                    itemExchange.s_dwPropertyA_Price = short.Parse(value);
                    break;
                case "s_dwPropertyB_Price":
                    itemExchange.s_dwPropertyB_Price = short.Parse(value);
                    break;
                case "s_dwPropertyC_Price":
                    itemExchange.s_dwPropertyC_Price = short.Parse(value);
                    break;
                case "s_dwPropertyD_Price":
                    itemExchange.s_dwPropertyD_Price = short.Parse(value);
                    break;
                case "s_dwCount":
                    itemExchange.s_dwCount = ushort.Parse(value);
                    break;
                case "unk1":
                    itemExchange.unk1 = short.Parse(value);
                    break;
                case "unk2":
                    itemExchange.unk2 = int.Parse(value);
                    break;
                default:
                    // Propriedade desconhecida, você pode lidar com isso de acordo com sua lógica
                    break;
            }
        }
        public static Accessory[] ImportAccessoriesFromXml(string inputFile)
        {
            List<Accessory> accessoryList = new List<Accessory>();
            Accessory currentAccessory = null;
            AccessoryOption currentOption = null;

            using (XmlReader reader = XmlReader.Create(inputFile))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == "Item")
                        {
                            currentAccessory = new Accessory();
                            accessoryList.Add(currentAccessory);
                        }
                        else if (reader.Name == "Option" && currentAccessory != null)
                        {
                            currentOption = new AccessoryOption();
                        }
                        else if (reader.Name != "Item" && reader.Name != "ItemAcessory" && reader.Name != "Option" && reader.Name != "s_nOptIdx" && reader.Name != "s_nMin" && reader.Name != "s_nMax" && reader.Name != "unknow")
                        {
                            SetAccessoryProperty(currentAccessory, reader.Name, reader.ReadElementContentAsString());
                        }

                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "s_nOptIdx" && currentOption != null)
                        {
                            currentOption.s_nOptIdx = short.Parse(reader.ReadElementContentAsString());
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "s_nMin" && currentOption != null)
                        {
                            currentOption.s_nMin = uint.Parse(reader.ReadElementContentAsString());

                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "s_nMax" && currentOption != null)
                        {
                            currentOption.s_nMax = uint.Parse(reader.ReadElementContentAsString());
                            currentAccessory.Option.Add(currentOption);
                            currentOption = new AccessoryOption();

                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "unknow" && currentOption != null)
                        {
                            currentOption.unknow = short.Parse(reader.ReadElementContentAsString());
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Option")
                    {

                    }

                }
            }

            return accessoryList.ToArray();
        }
        private static void SetAccessoryProperty(Accessory accessory, string propertyName, string value)
        {
            switch (propertyName)
            {
                case "index_Accessory":
                    accessory.index_Accessory = int.Parse(value);
                    break;
                case "index_Accessory1":
                    accessory.index_Accessory1 = int.Parse(value);
                    break;
                case "Gain_Option":
                    accessory.Gain_Option = short.Parse(value);
                    break;
                case "Changeable_Option_Number":
                    accessory.Changeable_Option_Number = short.Parse(value);
                    break;
                default:
                    // Propriedade desconhecida, você pode lidar com isso de acordo com sua lógica
                    break;
            }
        }
        public static AccessoryEnchant[] ImportAccessoryEnchantsFromXml(string inputFile)
        {
            List<AccessoryEnchant> enchantList = new List<AccessoryEnchant>();

            using (XmlReader reader = XmlReader.Create(inputFile))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Item")
                    {
                        AccessoryEnchant enchant = new AccessoryEnchant();

                        // Lê as propriedades do AccessoryEnchant a partir do XML
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                string propertyName = reader.Name;
                                reader.Read(); // Lê o valor da propriedade

                                // Define o valor da propriedade no objeto AccessoryEnchant
                                SetProperty(enchant, propertyName, reader.Value);
                            }
                            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Item")
                            {
                                // Adiciona o AccessoryEnchant à lista quando chegamos ao final do elemento "AccessoryEnchant"
                                enchantList.Add(enchant);
                                break;
                            }
                        }
                    }
                }
            }

            return enchantList.ToArray();
        }
        private static void SetProperty(AccessoryEnchant enchant, string propertyName, string value)
        {
            // Define o valor da propriedade com base no nome da propriedade
            switch (propertyName)
            {
                case "ID":
                    enchant.ID = int.Parse(value);
                    break;
                case "Index_Enchant":
                    enchant.Index_Enchant = int.Parse(value);
                    break;
                case "Enchant_Option":
                    enchant.Enchant_Option = short.Parse(value);
                    break;
                case "Factor1":
                    enchant.Factor1 = short.Parse(value);
                    break;
                default:
                    // Propriedade desconhecida, você pode lidar com isso de acordo com sua lógica
                    break;
            }
        }
        public static ItemMaking ImportItemMakingFromXml(string fileName)
        {
            XElement xml = XElement.Load(fileName);

            ItemMaking data = new ItemMaking();

            data.count = int.Parse(xml.Element("count").Value);

            var npcElements = xml.Element("index").Elements("NPC");
            data.index = new NPC[data.count];

            int i = 0;
            foreach (var npcElement in npcElements)
            {
                NPC npc = new NPC();
                npc.m_dwNpcIdx = int.Parse(npcElement.Element("m_dwNpcIdx").Value);
                npc.m_mapMainCategoty = int.Parse(npcElement.Element("m_mapMainCategoty").Value);

                var abarElements = npcElement.Element("index").Elements("Abar");
                npc.index = new Abar[npc.m_mapMainCategoty];

                int j = 0;
                foreach (var abarElement in abarElements)
                {
                    Abar abar = new Abar();
                    abar.ID = int.Parse(abarElement.Element("ID").Value);
                    abar.ID1 = int.Parse(abarElement.Element("ID1").Value);
                    abar.CarteSize = int.Parse(abarElement.Element("CarteSize").Value);
                    abar.Abaname = abarElement.Element("Abaname").Value;
                    abar.size_mapSubCategoty = int.Parse(abarElement.Element("size_mapSubCategoty").Value);

                    var subCategoryElements = abarElement.Element("index").Elements("SubCategoty");
                    abar.index = new SubCategoty[abar.size_mapSubCategoty];

                    int k = 0;
                    foreach (var subCategoryElement in subCategoryElements)
                    {
                        SubCategoty subCategory = new SubCategoty();
                        subCategory.ID = int.Parse(subCategoryElement.Element("ID").Value);
                        subCategory.ID1 = int.Parse(subCategoryElement.Element("ID1").Value);
                        subCategory.SizeNameCate = int.Parse(subCategoryElement.Element("SizeNameCate").Value);
                        subCategory.Name = subCategoryElement.Element("Name").Value;
                        subCategory.fcount = int.Parse(subCategoryElement.Element("fcount").Value);

                        var itemMakeElements = subCategoryElement.Element("index").Elements("itemMake");
                        subCategory.index = new itemMake[subCategory.fcount];

                        int l = 0;
                        foreach (var itemMakeElement in itemMakeElements)
                        {
                            itemMake itemMake = new itemMake();
                            itemMake.m_nUniqueIdx = int.Parse(itemMakeElement.Element("m_nUniqueIdx").Value);
                            itemMake.m_dwItemIdx = int.Parse(itemMakeElement.Element("m_dwItemIdx").Value);
                            itemMake.m_nItemNum = int.Parse(itemMakeElement.Element("m_nItemNum").Value);
                            itemMake.m_nProbabilityofSuccess = int.Parse(itemMakeElement.Element("m_nProbabilityofSuccess").Value);
                            itemMake.ink = int.Parse(itemMakeElement.Element("ink").Value);
                            itemMake.unk = int.Parse(itemMakeElement.Element("unk").Value);
                            itemMake.Valor = int.Parse(itemMakeElement.Element("Valor").Value);
                            itemMake.m_dwItemCost = int.Parse(itemMakeElement.Element("m_dwItemCost").Value);

                            var materialListElements = itemMakeElement.Element("index").Elements("MaterialList");
                            itemMake.index = new MaterialList[materialListElements.Count()];

                            int m = 0;
                            foreach (var materialListElement in materialListElements)
                            {
                                MaterialList materialList = new MaterialList();
                                materialList.m_dwItemIdx = int.Parse(materialListElement.Element("m_dwItemIdx").Value);
                                materialList.m_nItemNum = int.Parse(materialListElement.Element("m_nItemNum").Value);

                                itemMake.index[m] = materialList;
                                m++;
                            }

                            subCategory.index[l] = itemMake;
                            l++;
                        }

                        abar.index[k] = subCategory;
                        k++;
                    }

                    npc.index[j] = abar;
                    j++;
                }

                data.index[i] = npc;
                i++;
            }

            return data;
        }
        public static ItemMakingGroup[] ImportItemMakingGroupsFromXml(string inputFile)
        {
            List<ItemMakingGroup> itemMakingGroupsList = new List<ItemMakingGroup>();

            using (XmlReader reader = XmlReader.Create(inputFile))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Item")
                    {
                        ItemMakingGroup itemMakingGroup = new ItemMakingGroup();

                        // Lê as propriedades do ItemMakingGroup a partir do XML
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                string propertyName = reader.Name;
                                reader.Read(); // Lê o valor da propriedade

                                SetProperty(itemMakingGroup, propertyName, reader.Value);
                            }
                            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Item")
                            {
                                // Adiciona o ItemMakingGroup à lista quando chegamos ao final do elemento "Item"
                                itemMakingGroupsList.Add(itemMakingGroup);
                                break;
                            }
                        }
                    }
                }
            }

            return itemMakingGroupsList.ToArray();
        }
        private static void SetProperty(ItemMakingGroup itemMakingGroup, string propertyName, string value)
        {
            switch (propertyName)
            {
                case "Index":
                    itemMakingGroup.Index = int.Parse(value);
                    break;
                case "Type_No":
                    itemMakingGroup.Type_No = int.Parse(value);
                    break;
                case "Item_Num":
                    itemMakingGroup.Item_Num = int.Parse(value);
                    break;
                case "Item_Code":
                    itemMakingGroup.Item_Code = int.Parse(value);
                    break;
                case "Item_Num1":
                    itemMakingGroup.Item_Num1 = int.Parse(value);
                    break;
                default:
                    // Propriedade desconhecida, você pode lidar com isso de acordo com sua lógica
                    break;
            }
        }
        public static XaiItem[] ImportXaiItemsFromXml(string inputFile)
        {
            List<XaiItem> xaiItemList = new List<XaiItem>();

            using (XmlReader reader = XmlReader.Create(inputFile))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Item")
                    {
                        XaiItem xaiItem = new XaiItem();

                        // Lê as propriedades do XaiItem a partir do XML
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                string propertyName = reader.Name;
                                reader.Read(); // Lê o valor da propriedade

                                SetProperty(xaiItem, propertyName, reader.Value);
                            }
                            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Item")
                            {
                                // Adiciona o XaiItem à lista quando chegamos ao final do elemento "Item"
                                xaiItemList.Add(xaiItem);
                                break;
                            }
                        }
                    }
                }
            }

            return xaiItemList.ToArray();
        }
        private static void SetProperty(XaiItem xaiItem, string propertyName, string value)
        {
            switch (propertyName)
            {
                case "ItemID":
                    xaiItem.ItemID = int.Parse(value);
                    break;
                case "XGauge":
                    xaiItem.XGauge = int.Parse(value);
                    break;
                case "MaxCrystal":
                    xaiItem.MaxCrystal = byte.Parse(value);
                    break;
                default:
                    // Propriedade desconhecida, você pode lidar com isso de acordo com sua lógica
                    break;
            }
        }
        public static ItemRankEffect[] ImportItemRankEffectFromXml(string inputFile)
        {
            List<ItemRankEffect> accessoryList = new List<ItemRankEffect>();
            ItemRankEffect currentAccessory = null;
            ItemRankEffectInterval currentOption = null;

            using (XmlReader reader = XmlReader.Create(inputFile))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == "Item")
                        {
                            currentAccessory = new ItemRankEffect();
                            accessoryList.Add(currentAccessory);
                        }
                        else if (reader.Name == "nIntervals" && currentAccessory != null)
                        {
                            currentOption = new ItemRankEffectInterval();
                        }
                        else if (reader.Name != "Item" && reader.Name != "ItemRankEffect" && reader.Name != "nIntervals" && reader.Name != "dwItemCode" && reader.Name != "Rank" && reader.Name != "IconNo")
                        {
                            SetProperty(currentAccessory, reader.Name, reader.ReadElementContentAsString());
                        }

                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "dwItemCode" && currentOption != null)
                        {
                            currentOption.dwItemCode = int.Parse(reader.ReadElementContentAsString());
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Rank" && currentOption != null)
                        {
                            currentOption.Rank = int.Parse(reader.ReadElementContentAsString());
                            currentAccessory.Interval.Add(currentOption);
                            currentOption = new ItemRankEffectInterval();

                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "IconNo" && currentOption != null)
                        {
                            currentOption.IconNo = int.Parse(reader.ReadElementContentAsString());
                        }

                    }


                }
            }

            return accessoryList.ToArray();
        }
        private static void SetProperty(ItemRankEffect itemRankEffect, string propertyName, string value)
        {
            switch (propertyName)
            {
                case "nItemCode":
                    itemRankEffect.nItemCode = int.Parse(value);
                    break;
                case "nInterval":
                    itemRankEffect.nInterval = int.Parse(value);
                    break;

                default:
                    // Propriedade desconhecida, você pode lidar com isso de acordo com sua lógica
                    break;
            }
        }
        public static LookItem[] ImportLookItemsFromXml(string inputFile)
        {
            List<LookItem> lookItemsList = new List<LookItem>();

            using (XmlReader reader = XmlReader.Create(inputFile))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Item")
                    {
                        LookItem lookItem = new LookItem();

                        // Lê as propriedades do LookItem a partir do XML
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                string propertyName = reader.Name;
                                reader.Read(); // Lê o valor da propriedade

                                SetProperty(lookItem, propertyName, reader.Value);
                            }
                            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Item")
                            {
                                // Adiciona o LookItem à lista quando chegamos ao final do elemento "Item"
                                lookItemsList.Add(lookItem);
                                break;
                            }
                        }
                    }
                }
            }

            return lookItemsList.ToArray();
        }
        private static void SetProperty(LookItem lookItem, string propertyName, string value)
        {
            switch (propertyName)
            {
                case "Item_Code":
                    lookItem.Item_Code = int.Parse(value);
                    break;
                case "Di_No":
                    lookItem.Di_No = int.Parse(value);
                    break;
                case "Change_Type":
                    lookItem.Change_Type = int.Parse(value);
                    break;
                case "NameSize":
                    lookItem.NameSize = int.Parse(value);
                    break;
                case "File_Name":
                    lookItem.File_Name = value;
                    break;
                default:
                    // Propriedade desconhecida, você pode lidar com isso de acordo com sua lógica
                    break;
            }
        }
        public static void ExportToBinary(string outputFile, ITEM items, ItemTap[] itemTaps, ItemCoolTime[] CoolTime, ItemDisplay[] Display, ItemTypeName[] itemTypeName, ItemRank[] ranks, ElementItem[] elementItems, ElementItem[] elementItems1, ItemExchange[] itemExchanges, Accessory[] accessories, AccessoryEnchant[] accessoryEnchants, ItemMaking data, ItemMakingGroup[] itemMakingGroups, XaiItem[] xaiItems, ItemRankEffect[] itemRankEffects, LookItem[] lookItems)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(outputFile, FileMode.Create)))
            {
                // TODO: Melhorar/Refazer

                writer.Write(items.icount);

                // Escrever cada campo da estrutura sINFO para cada elemento do array index
                foreach (var info in items.index)
                {
                    writer.Write(info.s_dwItemID);

                    for (int i = 0; i < 64; i++)
                    {
                        char c = i < info.s_szName.Length ? info.s_szName[i] : '\0';
                        writer.Write((ushort)c);
                    }

                    writer.Write(info.s_nIcon);

                    for (int i = 0; i < 512; i++)
                    {
                        char c = i < info.s_szComment.Length ? info.s_szComment[i] : '\0';
                        writer.Write((ushort)c);
                    }



                    if (info.s_cNif.Contains("\n    "))
                    {

                    }

                    char[] NifTeste = info.s_cNif.PadRight(64, '\0').ToCharArray();

                    // Converte o array de caracteres s_cModel_Effect para bytes no formato UTF-8
                    byte[] NifTestee = Encoding.UTF8.GetBytes(NifTeste);
                    writer.Write(NifTestee, 0, 64);


                    writer.Write(info.s_nClass);

                    // Grava a string s_szComment como wchar_t com tamanho fixo de 64 caracteres
                    for (int i = 0; i < 64; i++)
                    {
                        char c = i < info.s_szTypeComment.Length ? info.s_szTypeComment[i] : '\0';
                        writer.Write((ushort)c);
                    }

                    writer.Write(info.s_btCodeTag);
                    writer.Write(info.unkt);
                    writer.Write(info.s_nType_L);
                    writer.Write(info.s_nType_S);
                    writer.Write(info.s_nTypeValue);
                    writer.Write(info.s_nSection);
                    writer.Write(info.s_nSellType);
                    writer.Write(info.s_nUseMode);
                    writer.Write(info.unkr);
                    writer.Write(info.s_nUseTimeGroup);
                    writer.Write(info.s_nOverlap);
                    writer.Write(info.s_nTamerReqMinLevel);
                    writer.Write(info.s_nTamerReqMaxLevel);
                    writer.Write(info.s_nDigimonReqMinLevel);
                    writer.Write(info.s_nDigimonReqMaxLevel);
                    writer.Write(info.s_nPossess);
                    writer.Write(info.s_nEquipSeries);
                    writer.Write(info.s_nUseCharacter);
                    writer.Write(info.s_bDummy);
                    writer.Write(info.uktest);
                    writer.Write(info.s_nDrop);
                    writer.Write(info.ukteste1);
                    writer.Write(info.s_nEventItemType);
                    writer.Write(info.s_dwEventItemPrice);
                    writer.Write(info.s_dwDigiCorePrice);
                    writer.Write(info.s_dwScanPrice);
                    writer.Write(info.s_dwSale);

                    writer.Write(Encoding.UTF8.GetBytes(info.s_cModel_Nif.PadRight(64, '\0')));

                    if (info.s_cModel_Nif.Contains("\n    "))
                    {

                    }

                    writer.Write(Encoding.UTF8.GetBytes(info.s_cModel_Effect.PadRight(64, '\0')));

                    writer.Write(info.s_bModel_Loop);
                    writer.Write(info.s_bModel_Shader);
                    writer.Write(info.s_nSkillCodeType);
                    writer.Write(info.s_dwSkill);
                    writer.Write(info.s_btApplyRateMax);
                    writer.Write(info.s_btApplyRateMin);
                    writer.Write(info.s_btApplyElement);
                    writer.Write(info.unk);
                    writer.Write(info.s_nSocketCount);
                    writer.Write(info.s_dwSoundID);
                    writer.Write(info.s_nBelonging);
                    writer.Write(info.unk2);
                    writer.Write(info.s_nQuest1);
                    writer.Write(info.s_nQuest2);
                    writer.Write(info.s_nQuest3);
                    writer.Write(info.s_nDigiviceSkillSlot);
                    writer.Write(info.s_nDigiviceChipsetSlot);
                    writer.Write(info.unk3);
                    writer.Write(info.s_nQuestRequire);
                    writer.Write(info.s_btUseTimeType);
                    writer.Write(info.unk4);
                    writer.Write(info.s_nUseTime_Min);
                    writer.Write(info.s_nUseBattle);
                    writer.Write(info.unks);
                    writer.Write(info.s_nDoNotUseType);
                    writer.Write(info.s_bUseTimeType);
                    writer.Write(info.unkss);
                }

                #region ItemMain

                //writer.Write(items.Length);
                //foreach (Item item in items)
                //{
                //    writer.Write(item.s_dwItemID);

                //    // Grava a string s_szName como wchar_t com tamanho fixo de 64 caracteres
                //    for (int i = 0; i < 64; i++)
                //    {
                //        char c = i < item.s_szName.Length ? item.s_szName[i] : '\0';
                //        writer.Write((ushort)c);
                //    }

                //    writer.Write(item.s_nIcon);

                //    // Grava a string s_szComment como wchar_t com tamanho fixo de 512 caracteres
                //    for (int i = 0; i < 512; i++)
                //    {
                //        char c = i < item.s_szComment.Length ? item.s_szComment[i] : '\0';
                //        writer.Write((ushort)c);
                //    }

                //    // Grava a propriedade s_cNif

                //    if (item.s_cNif.Contains("\n    "))
                //    {
                //        item.s_cNif = string.Empty;
                //    }

                //    char[] NifTeste = item.s_cNif.PadRight(64, '\0').ToCharArray();

                //    // Converte o array de caracteres s_cModel_Effect para bytes no formato UTF-8
                //    byte[] NifTestee = Encoding.UTF8.GetBytes(NifTeste);
                //    writer.Write(NifTestee, 0, 64);

                //    writer.Write(item.Item_Class);

                //    // Grava a string s_szComment como wchar_t com tamanho fixo de 64 caracteres
                //    for (int i = 0; i < 64; i++)
                //    {
                //        char c = i < item.s_szTypeComment.Length ? item.s_szTypeComment[i] : '\0';
                //        writer.Write((ushort)c);
                //    }

                //    writer.Write(item.unknow1);
                //    writer.Write(item.s_nType_L);
                //    writer.Write(item.s_nType_S);
                //    writer.Write(item.s_nTypeValue);
                //    writer.Write(item.Section);
                //    writer.Write(item.s_nSellType);
                //    writer.Write(item.unknow2);
                //    writer.Write(item.s_nUseTimeGroup);
                //    writer.Write(item.s_nOverlap);
                //    writer.Write(item.s_nTamerReqMinLevel);
                //    writer.Write(item.s_nTamerReqMaxLevel);
                //    writer.Write(item.s_nDigimonReqMinLevel);
                //    writer.Write(item.s_nDigimonReqMaxLevel);
                //    writer.Write(item.s_nPossess);
                //    writer.Write(item.s_nEquipSeries);
                //    writer.Write(item.s_nUseCharacter);
                //    writer.Write(item.unknow3);
                //    writer.Write(item.s_nDrop);
                //    writer.Write(item.unknow4);
                //    writer.Write(item.s_dwEventItemPrice);
                //    writer.Write(item.s_dwDigiCorePrice);
                //    writer.Write(item.Item_Digicore);
                //    writer.Write(item.s_dwScanPrice);
                //    writer.Write(item.s_dwSale);


                //    if (item.s_cModel_Nif.Contains("\n    "))
                //    {
                //        item.s_cModel_Nif = string.Empty;
                //    }


                //    char[] modelNifChars = item.s_cModel_Nif.PadRight(64, '\0').ToCharArray();

                //    // Converte o array de caracteres s_cModel_Nif para bytes no formato UTF-8
                //    byte[] modelNifBytes = Encoding.UTF8.GetBytes(modelNifChars);

                //    // Grava os primeiros 64 bytes no arquivo binário
                //    writer.Write(modelNifBytes, 0, 64);

                //    if (item.s_cModel_Effect.Contains("\n    "))
                //    {
                //        item.s_cModel_Effect = string.Empty;
                //    }

                //    char[] modelEffectChars = item.s_cModel_Effect.PadRight(64, '\0').ToCharArray();

                //    // Converte o array de caracteres s_cModel_Effect para bytes no formato UTF-8
                //    byte[] modelEffectBytes = Encoding.UTF8.GetBytes(modelEffectChars);

                //    // Grava os primeiros 64 bytes no arquivo binário
                //    writer.Write(modelEffectBytes, 0, 64);

                //    writer.Write(item.s_bModel_Loop);
                //    writer.Write(item.s_bModel_Shader);
                //    writer.Write(item.s_nSkillCodeType);
                //    writer.Write(item.s_dwSkill);
                //    writer.Write(item.s_btApplyRateMax);
                //    writer.Write(item.s_btApplyRateMin);
                //    writer.Write(item.s_btApplyElement);
                //    writer.Write(item.unknow);
                //    writer.Write(item.s_nSocketCount);
                //    writer.Write(item.s_dwSoundID);
                //    writer.Write(item.item_Belonging);
                //    writer.Write(item.s_nQuest1);
                //    writer.Write(item.s_nQuest2);
                //    writer.Write(item.s_nQuest3);
                //    writer.Write(item.s_nDigiviceSkillSlot);
                //    writer.Write(item.s_nDigiviceChipsetSlot);
                //    writer.Write(item.s_nQuestRequire);
                //    writer.Write(item.Use_Item_Type);
                //    writer.Write(item.unknow5);
                //    writer.Write(item.unknow6);
                //    writer.Write(item.unknow7);
                //    writer.Write(item.UseTimeMinutes);
                //    writer.Write(item.s_nUseBattle);
                //    writer.Write(item.s_nDoNotUseType);
                //    writer.Write(item.s_bUseTimeType);
                //    writer.Write(item.unknow8);
                //}
                #endregion

                #region ItemTap
                writer.Write(itemTaps.Length);
                foreach (var item in itemTaps)
                {
                    writer.Write(item.s_nSellClass);

                    string originalString = item.s_szName;

                    // Converter a string original para um array de bytes no formato Unicode
                    byte[] originalStringBytes = Encoding.Unicode.GetBytes(originalString);

                    // Criar um novo array de bytes com o tamanho fixo de 64 bytes
                    byte[] newBytes = new byte[64];

                    // Copiar os bytes da string original para o novo array
                    Array.Copy(originalStringBytes, newBytes, Math.Min(originalStringBytes.Length, newBytes.Length));

                    // Gravar o novo array de bytes no arquivo binário
                    writer.Write(newBytes);
                }

                #endregion

                #region CoolTime
                writer.Write(CoolTime.Length);
                foreach (var item in CoolTime)
                {
                    writer.Write(item.s_nGroupID);
                    writer.Write(item.TimeGroup.ToArray());
                }
                #endregion

                #region ItemDisplay
                writer.Write(Display.Length);
                foreach (var item in Display)
                {
                    writer.Write(item.nItemS);
                    writer.Write(item.dwDispID);
                }
                #endregion

                #region ItemTypeName
                writer.Write(itemTypeName.Length);
                foreach (var item in itemTypeName)
                {
                    writer.Write(item.s_szId);
                    for (int i = 0; i < 64; i++)
                    {
                        char c = i < item.s_szName.Length ? item.s_szName[i] : '\0';
                        writer.Write((ushort)c);
                    }

                }
                #endregion

                #region ItemRank
                writer.Write(ranks.Length);
                foreach (var item in ranks)
                {


                    writer.Write(item.ID);
                    writer.Write(item.Drop_Class);
                    writer.Write(item.Drop_Count);
                }
                #endregion

                #region ElementItem
                writer.Write(elementItems.Length);
                foreach (var item in elementItems)
                {


                    writer.Write(item.s_dwItemID);

                }
                writer.Write(elementItems1.Length);
                foreach (var item in elementItems1)
                {
                    writer.Write(item.s_dwItemID);

                }
                #endregion

                #region ItemExchange
                writer.Write(itemExchanges.Length);
                foreach (var item in itemExchanges)
                {

                    writer.Write(item.ItemExchangeNo);
                    writer.Write(item.s_dwNpcID);
                    writer.Write(item.s_dwItemIndex);
                    writer.Write(item.unk);
                    writer.Write(item.s_dwItemID);
                    writer.Write(item.s_dwExchange_Code_A);
                    writer.Write(item.s_dwExchange_Code_B);
                    writer.Write(item.s_dwExchange_Code_C);
                    writer.Write(item.s_dwExchange_Code_D);
                    writer.Write(item.s_dwPropertyA_Price);
                    writer.Write(item.s_dwPropertyB_Price);
                    writer.Write(item.s_dwPropertyC_Price);
                    writer.Write(item.s_dwPropertyD_Price);
                    writer.Write(item.s_dwCount);
                    writer.Write(item.unk1);


                }
                #endregion

                #region Acessorys
                writer.Write(accessories.Length);
                foreach (var item in accessories)
                {

                    writer.Write(item.index_Accessory1);
                    writer.Write(item.index_Accessory);
                    writer.Write(item.Gain_Option);
                    writer.Write(item.Changeable_Option_Number);

                    for (int x = 0; x < item.Option.Count; x++) //maxList
                    {


                        writer.Write(item.Option[x].s_nOptIdx);
                        writer.Write(item.Option[x].unknow);
                        writer.Write(item.Option[x].s_nMin);
                        writer.Write(item.Option[x].s_nMax);

                    }
                }
                #endregion

                #region AcessorysEnchant
                writer.Write(accessoryEnchants.Length);
                foreach (var item in accessoryEnchants)
                {
                    writer.Write(item.ID);
                    writer.Write(item.Index_Enchant);
                    writer.Write(item.Enchant_Option);
                    writer.Write(item.Factor1);
                }
                #endregion

                writer.Write(data.count);
                for (int i = 0; i < data.count; i++)
                {
                    writer.Write(data.index[i].m_dwNpcIdx);
                    writer.Write(data.index[i].m_mapMainCategoty);

                    for (int j = 0; j < data.index[i].m_mapMainCategoty; j++)
                    {
                        writer.Write(data.index[i].index[j].ID);
                        writer.Write(data.index[i].index[j].ID1);
                        writer.Write(data.index[i].index[j].CarteSize / 2);

                        for (int s = 0; s < data.index[i].index[j].CarteSize / 2; s++)
                        {
                            char c = s < data.index[i].index[j].Abaname.Length ? data.index[i].index[j].Abaname[s] : '\0';
                            writer.Write((ushort)c);
                        }

                        writer.Write(data.index[i].index[j].size_mapSubCategoty);

                        for (int k = 0; k < data.index[i].index[j].size_mapSubCategoty; k++)
                        {
                            writer.Write(data.index[i].index[j].index[k].ID);
                            writer.Write(data.index[i].index[j].index[k].ID1);
                            writer.Write(data.index[i].index[j].index[k].SizeNameCate / 2);
                            for (int x = 0; x < data.index[i].index[j].index[k].SizeNameCate / 2; x++)
                            {
                                char c = x < data.index[i].index[j].index[k].Name.Length ? data.index[i].index[j].index[k].Name[x] : '\0';
                                writer.Write((ushort)c);
                            }

                            writer.Write(data.index[i].index[j].index[k].fcount);

                            for (int l = 0; l < data.index[i].index[j].index[k].fcount; l++)
                            {
                                writer.Write(data.index[i].index[j].index[k].index[l].m_nUniqueIdx);
                                writer.Write(data.index[i].index[j].index[k].index[l].m_dwItemIdx);
                                writer.Write(data.index[i].index[j].index[k].index[l].m_nItemNum);
                                writer.Write(data.index[i].index[j].index[k].index[l].m_nProbabilityofSuccess);
                                writer.Write(data.index[i].index[j].index[k].index[l].ink);
                                writer.Write(data.index[i].index[j].index[k].index[l].unk);
                                writer.Write(data.index[i].index[j].index[k].index[l].Valor);
                                writer.Write(data.index[i].index[j].index[k].index[l].m_dwItemCost);

                                for (int m = 0; m < data.index[i].index[j].index[k].index[l].index.Length; m++)
                                {
                                    writer.Write(data.index[i].index[j].index[k].index[l].index[m].m_dwItemIdx);
                                    writer.Write(data.index[i].index[j].index[k].index[l].index[m].m_nItemNum);
                                }
                            }
                        }
                    }
                }

                #region ItemMakeGroup
                writer.Write(itemMakingGroups.Length);
                foreach (var item in itemMakingGroups)
                {
                    writer.Write(item.Index);
                    writer.Write(item.Type_No);
                    writer.Write(item.Item_Num);
                    writer.Write(item.Item_Code);
                    writer.Write(item.Item_Num1);
                }
                #endregion

                #region XaiItem
                writer.Write(xaiItems.Length);
                foreach (var item in xaiItems)
                {
                    writer.Write(item.ItemID);
                    writer.Write(item.XGauge);
                    writer.Write(item.MaxCrystal);
                }
                #endregion

                #region ItemRankEffect
                writer.Write(itemRankEffects.Length);
                foreach (var item in itemRankEffects)
                {
                    writer.Write(item.nItemCode);
                    writer.Write(item.nInterval);

                    foreach (var NewItem in item.Interval)
                    {
                        writer.Write(NewItem.dwItemCode);
                        writer.Write(NewItem.IconNo);
                        writer.Write(NewItem.Rank);
                    }
                }
                #endregion

                #region LookItem
                writer.Write(lookItems.Length);
                foreach (var item in lookItems)
                {

                    writer.Write(item.Item_Code);
                    writer.Write(item.Di_No);
                    writer.Write(item.Change_Type);
                    int size = Encoding.ASCII.GetByteCount(item.File_Name);

                    // Gravar o tamanho no arquivo binário
                    writer.Write(size);

                    // Converter a string para um array de bytes no formato ASCII
                    byte[] nameBytes = Encoding.ASCII.GetBytes(item.File_Name);

                    // Gravar os bytes no arquivo binário
                    writer.Write(nameBytes);
                }
                #endregion

            }
        }

    }
}
