using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BinXmlConverter.Classes.ExtraExchanges;
using System.Xml.Linq;
using static BinXmlConverter.Classes.GotchaMachine;
using System.Collections;
using static BinXmlConverter.Classes.Tactics;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace BinXmlConverter.Classes
{
    public class Tactics
    {
        public class TDBTactic
        {
            public int ItemId;
            public int s_nDigimonID;
            public int s_nReqItemS_Type;
            public int s_nReqItemS_Type1;
            public ushort s_nReqItemCount;
            public ushort s_nReqItemCount1;
            public byte s_nLimitLevel;
            public byte s_nLimitLevel1;
            public byte s_nViewWarning;
            public byte s_nViewWarning1;

            public static (TDBTactic[], TacticsPlain[], Transcender[], EnchantRankInfo[], TranscendDefaultCorrect[], TranscendSameType[]) ImporTacticsFromBinary(string filePath)
            {
                TDBTactic[] tdBTactics;
                TacticsPlain[] tacticsPlain;
                Transcender[] transcenders;
                EnchantRankInfo[] enchantRankInfo;
                TranscendDefaultCorrect[] transcendDefaultCorrects;
                TranscendSameType[] transcendSameType;
                TranscendenceInfo[] transcendenceInfo;

                using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
                {
                    // Ler o número de elementos em cada array
                    int tdBTacticsCount = reader.ReadInt32();

                    // Ler os dados de TDBTactic
                    tdBTactics = new TDBTactic[tdBTacticsCount];
                    for (int i = 0; i < tdBTacticsCount; i++)
                    {
                        TDBTactic tactic = new TDBTactic();
                        tactic.ItemId = reader.ReadInt32();
                        tactic.s_nDigimonID = reader.ReadInt32();
                        tactic.s_nReqItemS_Type = reader.ReadInt32();
                        tactic.s_nReqItemS_Type1 = reader.ReadInt32();
                        tactic.s_nReqItemCount = reader.ReadUInt16();
                        tactic.s_nReqItemCount1 = reader.ReadUInt16();
                        tactic.s_nLimitLevel = reader.ReadByte();
                        tactic.s_nLimitLevel1 = reader.ReadByte();
                        tactic.s_nViewWarning = reader.ReadByte();
                        tactic.s_nViewWarning1 = reader.ReadByte();

                        tdBTactics[i] = tactic;
                    }

                    // Ler os dados de TacticsPlain
                    int tacticsPlainCount = reader.ReadInt32();
                    tacticsPlain = new TacticsPlain[tacticsPlainCount];

                    for (int i = 0; i < tacticsPlainCount; i++)
                    {
                        TacticsPlain tactics = new TacticsPlain();
                        tactics.ID = reader.ReadInt32();
                        tactics.s_dwTacticsMonID = reader.ReadInt32();
                        byte[] nameBytes = reader.ReadBytes(128);
                        tactics.s_szTacticsName = CleanString(System.Text.Encoding.Unicode.GetString(nameBytes));
                        byte[] s_szTacticsExplain = reader.ReadBytes(1024);
                        tactics.s_szTacticsExplain = CleanString(System.Text.Encoding.Unicode.GetString(s_szTacticsExplain));

                        tacticsPlain[i] = tactics;
                    }

                    // Ler os dados de Transcender

                    int transcendersCount = reader.ReadInt32();
                    transcenders = new Transcender[transcendersCount];
                    for (int i = 0; i < transcendersCount; i++)
                    {
                        Transcender transcender = new Transcender();
                        transcender.dwItemCode = reader.ReadInt32();
                        transcender.nLowLevel = reader.ReadInt32();
                        transcender.nHighLevel = reader.ReadInt32();
                        transcender.dwNeedMoney = reader.ReadInt32();

                        transcenders[i] = transcender;
                    }

                    int EnchantRankInfoCount = reader.ReadInt32();
                    enchantRankInfo = new EnchantRankInfo[EnchantRankInfoCount];
                    for (int i = 0; i < EnchantRankInfoCount; i++)
                    {
                        EnchantRankInfo info = new();

                        info.nStatIdx = reader.ReadInt32();

                        var count = reader.ReadInt32();
                        for (int x = 0; x < count; x++)
                        {
                            EnchantValueInfo enchantValueInfo = new EnchantValueInfo();
                            enchantValueInfo.nLowEnchantLv = reader.ReadInt32();
                            enchantValueInfo.nHighEnchantLv = reader.ReadInt32();
                            enchantValueInfo.nDigimonGrowMinLv = reader.ReadInt32();
                            enchantValueInfo.nDigimonGrowMaxLv = reader.ReadInt32();
                            enchantValueInfo.nNormalEnchantMinValue = reader.ReadInt32();
                            enchantValueInfo.nNormalEnchantMaxValue = reader.ReadInt32();
                            enchantValueInfo.nSpecialEnchantValue = reader.ReadInt32();

                            info.enchantValueInfos.Add(enchantValueInfo);
                        }
                        enchantRankInfo[i] = info;
                    }

                    transcendDefaultCorrects = new TranscendDefaultCorrect[1];
                    TranscendDefaultCorrect transcendDefaultCorrect = new TranscendDefaultCorrect();
                    transcendDefaultCorrect.nEnchentDefaultCorrect = reader.ReadInt32();
                    transcendDefaultCorrects[0] = transcendDefaultCorrect;

                    var scount = reader.ReadInt32();
                    transcendSameType = new TranscendSameType[scount];
                    for (int i = 0; i < scount; i++)
                    {
                        TranscendSameType Same = new TranscendSameType();

                        Same.SameType = reader.ReadInt32();
                        var SubCount = reader.ReadInt32();
                        for (int x = 0; x < SubCount; x++)
                        {
                            TranscendSameInfo info = new();
                            info.Growth = reader.ReadInt32();
                            info.nValue = reader.ReadSingle();
                            Same.SameInfos.Add(info);
                        }
                        transcendSameType[i] = Same;
                    }
                    var dscount = reader.ReadInt32();
                    transcendenceInfo = new TranscendenceInfo[scount];
                    for (int i = 0; i < dscount; i++)
                    {
                        TranscendenceInfo tinfo = new();

                        tinfo.TargetGrowth = reader.ReadInt32();
                        var SubCount = reader.ReadInt32();
                        for (int x = 0; x < SubCount; x++)
                        {
                            TranscendenceList info = new();

                            info.nNeedDigimonLv = reader.ReadInt32();
                            info.nNeedEnchantLv = reader.ReadInt32();
                            info.nNeedEvoMinType = reader.ReadInt32();
                            info.nNeedEvoMaxType = reader.ReadInt32();
                            info.nNeedScale = reader.ReadInt32();
                            info.nNextDigimonGrowth = reader.ReadInt32();
                            info.nNeedHatchLvMin = reader.ReadInt32();
                            info.nNeedHatchLvMax = reader.ReadInt32();
                            info.dwTransCost = reader.ReadInt32(); 
                            info.dwMaxExp = reader.ReadInt32();
                            tinfo.transcendenceLists.Add(info);
                        }
                        transcendenceInfo[i] = tinfo;
                    }
                }



                return (tdBTactics, tacticsPlain, transcenders, enchantRankInfo, transcendDefaultCorrects, transcendSameType);
            }
            static string CleanString(string input)
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

            public static void ExportTDBTacticToXml(TDBTactic[] tacticArray, string filePath)
            {
                XElement rootElement = new XElement("TDBTactics");

                foreach (TDBTactic tactic in tacticArray)
                {
                    XElement tacticElement = new XElement("TDBTactic",
                        new XElement("ItemId", tactic.ItemId),
                        new XElement("s_nDigimonID", tactic.s_nDigimonID),
                        new XElement("s_nReqItemS_Type", tactic.s_nReqItemS_Type),
                        new XElement("s_nReqItemS_Type1", tactic.s_nReqItemS_Type1),
                        new XElement("s_nReqItemCount", tactic.s_nReqItemCount),
                        new XElement("s_nReqItemCount1", tactic.s_nReqItemCount1),
                        new XElement("s_nLimitLevel", tactic.s_nLimitLevel),
                        new XElement("s_nLimitLevel1", tactic.s_nLimitLevel1),
                        new XElement("s_nViewWarning", tactic.s_nViewWarning),
                        new XElement("s_nViewWarning1", tactic.s_nViewWarning1)
                    );

                    rootElement.Add(tacticElement);
                }

                XDocument xmlDoc = new XDocument(rootElement);
                xmlDoc.Save(filePath);
            }
            public static void ExportTacticsPlainToXml(TacticsPlain[] tacticsArray, string filePath)
            {
                XElement rootElement = new XElement("TacticsPlainList");

                foreach (TacticsPlain tactics in tacticsArray)
                {
                    XElement tacticsElement = new XElement("TacticsPlain",
                        new XElement("ID", tactics.ID),
                        new XElement("s_dwTacticsMonID", tactics.s_dwTacticsMonID),
                        new XElement("s_szTacticsName", tactics.s_szTacticsName),
                        new XElement("s_szTacticsExplain", tactics.s_szTacticsExplain)
                    );

                    rootElement.Add(tacticsElement);
                }

                XDocument xmlDoc = new XDocument(rootElement);
                xmlDoc.Save(filePath);
            }
            public static void ExportTranscenderToXml(Transcender[] transcenderArray, string filePath)
            {
                XElement rootElement = new XElement("TranscenderList");

                foreach (Transcender transcender in transcenderArray)
                {
                    XElement transcenderElement = new XElement("Transcender",
                        new XElement("dwItemCode", transcender.dwItemCode),
                        new XElement("nLowLevel", transcender.nLowLevel),
                        new XElement("nHighLevel", transcender.nHighLevel),
                        new XElement("dwNeedMoney", transcender.dwNeedMoney)
                    );

                    rootElement.Add(transcenderElement);
                }

                XDocument xmlDoc = new XDocument(rootElement);
                xmlDoc.Save(filePath);
            }
            public static void ExportEnchantRankInfoToXml(EnchantRankInfo[] enchantRankInfos, string fileName)
            {
                XElement rootElement = new XElement("EnchantRankInfos");

                foreach (var rankInfo in enchantRankInfos)
                {
                    XElement rankInfoElement = new XElement("EnchantRankInfo",
                        new XElement("nStatIdx", rankInfo.nStatIdx),
                        new XElement("enchantValueInfos",
                            rankInfo.enchantValueInfos.ConvertAll(enchantValueInfo =>
                                new XElement("EnchantValueInfo",
                                    new XElement("nLowEnchantLv", enchantValueInfo.nLowEnchantLv),
                                    new XElement("nHighEnchantLv", enchantValueInfo.nHighEnchantLv),
                                    new XElement("nDigimonGrowMinLv", enchantValueInfo.nDigimonGrowMinLv),
                                    new XElement("nDigimonGrowMaxLv", enchantValueInfo.nDigimonGrowMaxLv),
                                    new XElement("nNormalEnchantMinValue", enchantValueInfo.nNormalEnchantMinValue),
                                    new XElement("nNormalEnchantMaxValue", enchantValueInfo.nNormalEnchantMaxValue),
                                    new XElement("nSpecialEnchantValue", enchantValueInfo.nSpecialEnchantValue)
                                )
                            )
                        )
                    );

                    rootElement.Add(rankInfoElement);
                }

                // Salvar o XElement em um arquivo XML
                rootElement.Save(fileName);
            }

            public static EnchantRankInfo[] ImportEnchantRankInfoFromXml(string fileName)
            {
                XElement rootElement = XElement.Load(fileName);
                var enchantRankInfos = rootElement.Descendants("EnchantRankInfo");

                List<EnchantRankInfo> importedData = new List<EnchantRankInfo>();
                foreach (var enchantRankInfo in enchantRankInfos)
                {
                    importedData.Add(new EnchantRankInfo
                    {
                        nStatIdx = int.Parse(enchantRankInfo.Element("nStatIdx").Value),
                        enchantValueInfos = enchantRankInfo
                            .Descendants("EnchantValueInfo")
                            .Select(enchantValueInfo => new EnchantValueInfo
                            {
                                nLowEnchantLv = int.Parse(enchantValueInfo.Element("nLowEnchantLv").Value),
                                nHighEnchantLv = int.Parse(enchantValueInfo.Element("nHighEnchantLv").Value),
                                nDigimonGrowMinLv = int.Parse(enchantValueInfo.Element("nDigimonGrowMinLv").Value),
                                nDigimonGrowMaxLv = int.Parse(enchantValueInfo.Element("nDigimonGrowMaxLv").Value),
                                nNormalEnchantMinValue = int.Parse(enchantValueInfo.Element("nNormalEnchantMinValue").Value),
                                nNormalEnchantMaxValue = int.Parse(enchantValueInfo.Element("nNormalEnchantMaxValue").Value),
                                nSpecialEnchantValue = int.Parse(enchantValueInfo.Element("nSpecialEnchantValue").Value)
                            })
                            .ToList()
                    });
                }

                return importedData.ToArray();
            }
            public static TDBTactic[] ImportTDBTacticFromXml(string filePath)
            {
                List<TDBTactic> tacticList = new List<TDBTactic>();

                XDocument xmlDoc = XDocument.Load(filePath);
                XElement rootElement = xmlDoc.Root;

                foreach (XElement tacticElement in rootElement.Elements("TDBTactic"))
                {
                    TDBTactic tactic = new TDBTactic();
                    tactic.ItemId = int.Parse(tacticElement.Element("ItemId").Value);
                    tactic.s_nDigimonID = int.Parse(tacticElement.Element("s_nDigimonID").Value);
                    tactic.s_nReqItemS_Type = int.Parse(tacticElement.Element("s_nReqItemS_Type").Value);
                    tactic.s_nReqItemS_Type1 = int.Parse(tacticElement.Element("s_nReqItemS_Type1").Value);
                    tactic.s_nReqItemCount = ushort.Parse(tacticElement.Element("s_nReqItemCount").Value);
                    tactic.s_nReqItemCount1 = ushort.Parse(tacticElement.Element("s_nReqItemCount1").Value);
                    tactic.s_nLimitLevel = byte.Parse(tacticElement.Element("s_nLimitLevel").Value);
                    tactic.s_nLimitLevel1 = byte.Parse(tacticElement.Element("s_nLimitLevel1").Value);
                    tactic.s_nViewWarning = byte.Parse(tacticElement.Element("s_nViewWarning").Value);
                    tactic.s_nViewWarning1 = byte.Parse(tacticElement.Element("s_nViewWarning1").Value);

                    tacticList.Add(tactic);
                }

                return tacticList.ToArray();
            }
            public static TacticsPlain[] ImportTacticsPlainFromXml(string filePath)
            {
                List<TacticsPlain> tacticsList = new List<TacticsPlain>();

                XDocument xmlDoc = XDocument.Load(filePath);
                XElement rootElement = xmlDoc.Root;

                foreach (XElement tacticsElement in rootElement.Elements("TacticsPlain"))
                {
                    TacticsPlain tactics = new TacticsPlain();
                    tactics.ID = int.Parse(tacticsElement.Element("ID").Value);
                    tactics.s_dwTacticsMonID = int.Parse(tacticsElement.Element("s_dwTacticsMonID").Value);
                    tactics.s_szTacticsName = tacticsElement.Element("s_szTacticsName").Value;
                    tactics.s_szTacticsExplain = tacticsElement.Element("s_szTacticsExplain").Value;

                    tacticsList.Add(tactics);
                }

                return tacticsList.ToArray();
            }
            public static Transcender[] ImportTranscenderFromXml(string filePath)
            {
                List<Transcender> transcenderList = new List<Transcender>();

                XDocument xmlDoc = XDocument.Load(filePath);
                XElement rootElement = xmlDoc.Root;

                foreach (XElement transcenderElement in rootElement.Elements("Transcender"))
                {
                    Transcender transcender = new Transcender();
                    transcender.dwItemCode = int.Parse(transcenderElement.Element("dwItemCode").Value);
                    transcender.nLowLevel = int.Parse(transcenderElement.Element("nLowLevel").Value);
                    transcender.nHighLevel = int.Parse(transcenderElement.Element("nHighLevel").Value);
                    transcender.dwNeedMoney = int.Parse(transcenderElement.Element("dwNeedMoney").Value);

                    transcenderList.Add(transcender);
                }

                return transcenderList.ToArray();
            }
            public static void ExportTacticsToBinary(string filePath, TDBTactic[] tdBTactics, TacticsPlain[] tacticsPlain, Transcender[] transcenders)
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
                {
                    // Escrever o número de elementos em cada array
                    writer.Write(tdBTactics.Length);
                    // Escrever os dados de TDBTactic
                    foreach (TDBTactic tactic in tdBTactics)
                    {
                        writer.Write(tactic.ItemId);
                        writer.Write(tactic.s_nDigimonID);
                        writer.Write(tactic.s_nReqItemS_Type);
                        writer.Write(tactic.s_nReqItemS_Type1);
                        writer.Write(tactic.s_nReqItemCount);
                        writer.Write(tactic.s_nReqItemCount1);
                        writer.Write(tactic.s_nLimitLevel);
                        writer.Write(tactic.s_nLimitLevel1);
                        writer.Write(tactic.s_nViewWarning);
                        writer.Write(tactic.s_nViewWarning1);
                    }

                    writer.Write(tacticsPlain.Length);
                    foreach (TacticsPlain tactics in tacticsPlain)
                    {
                        writer.Write(tactics.ID);
                        writer.Write(tactics.s_dwTacticsMonID);

                        for (int i = 0; i < 128 / 2; i++)
                        {
                            char c = i < tactics.s_szTacticsName.Length ? tactics.s_szTacticsName[i] : '\0';
                            writer.Write((ushort)c);
                        }

                        for (int i = 0; i < 1024 / 2; i++)
                        {
                            char c = i < tactics.s_szTacticsExplain.Length ? tactics.s_szTacticsExplain[i] : '\0';
                            writer.Write((ushort)c);
                        }

                    }

                    //Escrever os dados de Transcender
                    writer.Write(transcenders.Length);
                    foreach (Transcender transcender in transcenders)
                    {
                        writer.Write(transcender.dwItemCode);
                        writer.Write(transcender.nLowLevel);
                        writer.Write(transcender.nHighLevel);
                        writer.Write(transcender.dwNeedMoney);
                    }
                }
            }
        }

        public class TacticsPlain
        {
            public int ID;
            public int s_dwTacticsMonID;
            public string s_szTacticsName;
            public string s_szTacticsExplain;

        }
        public class Transcender
        {
            public int dwItemCode;
            public int nLowLevel;
            public int nHighLevel;
            public int dwNeedMoney;
        }

        public class EnchantRankInfo
        {
            public int nStatIdx;

            public List<EnchantValueInfo> enchantValueInfos = new();
        }

        public class EnchantValueInfo
        {
            public int nLowEnchantLv;      // 사용할수 있는 최소단계
            public int nHighEnchantLv;     // 사용할수 있는 최대단계
            public int nDigimonGrowMinLv;  // 강화 할 수 있는 디지몬의 최소 단수
            public int nDigimonGrowMaxLv;  // 강화 할 수 있는 디지몬의 최대 단수
            public int nNormalEnchantMinValue; // 일반 강화시 렌덤 최소값
            public int nNormalEnchantMaxValue; // 일반 강화시 렌덤 최대값
            public int nSpecialEnchantValue;	// 궁극 강화시 최대값
        }

        public class TranscendDefaultCorrect
        {
            public int nEnchentDefaultCorrect;
        }

        public class TranscendSameType
        {
            public int SameType;
            public List<TranscendSameInfo> SameInfos = new();
        }

        public class TranscendSameInfo
        {
            public int Growth;
            public float nValue;
        }
        public class TranscendenceInfo
        {

            public int TargetGrowth;
            public List<TranscendenceList> transcendenceLists = new();
        }
        public class TranscendenceList
        {
            public int nNeedDigimonLv;     // 디지몬 레벨
            public int nNeedEnchantLv;     // 강화 레벨
            public int nNeedEvoMinType;    // 디지몬 진화 타입
            public int nNeedEvoMaxType;    // 디지몬 진화 타입
            public int nNeedScale;         // 디지몬 크기
            public int nNextDigimonGrowth; // 다음 디지몬 단수
            public int nNeedHatchLvMin;    // 디지몬 필요 부화 단계
            public int nNeedHatchLvMax;
            public int dwTransCost;      // 초월 비용
            public int dwMaxExp;			// 초월 경험치
        }
    }
}

