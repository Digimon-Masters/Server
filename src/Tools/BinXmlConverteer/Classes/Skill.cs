using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BinXmlConverter.Classes
{
    public class SkillData
    {
        public int s_dwID;
        public string s_szName;
        public string s_szComment;
        public List<IncreaseApply> SkillApply = new();
        public ushort s_nLevelupPoint;
        public ushort s_nMaxLevel;
        public ushort s_nAttributeType;
        public ushort s_nNatureType;
        public ushort s_nFamilyType;
        public ushort s_nUseHP;
        public ushort s_nUseDS;
        public ushort s_nIcon;
        public ushort s_nTarget;
        public ushort s_nAttType;
        public float s_fAttRange;
        public float s_fAttRange_MinDmg;
        public float s_fAttRange_NorDmg;
        public float s_fAttRange_MaxDmg;
        public ushort s_nAttSphere;
        public float s_fCastingTime;
        public float s_fDamageTime;
        public int s_nDamageDay;
        public float s_nDistanceTime;
        public float s_fCooldownTime;
        public ushort s_nCooldownDay;
        public float s_fSkill_Velocity;
        public float s_fSkill_Accel;
        public ushort s_nSkillType;
        public ushort s_nLimitLevel;
        public ushort s_nSkillGroup;       // Verificação de sobreposição da habilidade de memória (habilidade do Digimon Cash) ??
        public ushort s_nSkillRank;        // Habilidade de Memória (Alta (3), Média (2), Baixa (1))
        public ushort s_nMemorySkill;      // Se for 1~3, é uma habilidade de memória (se for 0, é uma habilidade geral... A habilidade de memória pode ser deletada.
        public ushort s_nReq_Item;		// Número de chips de memória necessários ao usar habilidades de memória
        public ushort ink;
        public ushort unk;
        public ushort unk2;
        public static (SkillData[], TamerSkill[], AreaCheck[]) SkillToXml(string SkillInput)
        {
            using (BinaryReader read = new BinaryReader(File.OpenRead(SkillInput)))
            {
                int count = read.ReadInt32();
                SkillData[] Skill = new SkillData[count];

                for (int i = 0; i < count; i++)
                {


                    SkillData skill = new SkillData();
                    skill.s_dwID = read.ReadInt32();


                    byte[] nameBytes = read.ReadBytes(32 * 2); // Lê os bytes da string s_szName

                    StringBuilder nameBuilder = new StringBuilder();

                    for (int x = 0; x < 32; x++)
                    {
                        char character = BitConverter.ToChar(nameBytes, x * 2);

                        if (character != '\0')
                            nameBuilder.Append(character);
                        else
                            break;
                    }

                    skill.s_szName = nameBuilder.ToString();

                    byte[] commentBytes = read.ReadBytes(256 * 2); // Lê os bytes da string s_szComment

                    StringBuilder commentBuilder = new StringBuilder();

                    for (int s = 0; s < 256; s++)
                    {
                        char character = BitConverter.ToChar(commentBytes, s * 2);

                        if (character != '\0')
                            commentBuilder.Append(character);
                        else
                            break;
                    }

                    skill.s_szComment = commentBuilder.ToString();

                    for (int x = 0; x < 3; x++)
                    {
                        IncreaseApply apply = new();
                        apply.s_nA = read.ReadInt32();
                        apply.s_nInvoke_Rate = read.ReadInt32();     // Probabilidade de ativação
                        apply.s_nB = read.ReadInt32();
                        apply.s_nC = read.ReadInt32();
                        apply.s_nBuffCode = read.ReadUInt16();
                        apply.s_nID = read.ReadUInt16();
                        apply.s_nIncrease_B_Point = read.ReadInt32();
                        skill.SkillApply.Add(apply);
                    }

                    skill.s_nLevelupPoint = read.ReadUInt16();
                    skill.s_nMaxLevel = read.ReadUInt16();
                    skill.s_nAttributeType = read.ReadUInt16();
                    skill.s_nNatureType = read.ReadUInt16();
                    skill.s_nFamilyType = read.ReadUInt16();
                    skill.s_nUseHP = read.ReadUInt16();
                    skill.s_nUseDS = read.ReadUInt16();
                    skill.s_nIcon = read.ReadUInt16();
                    skill.s_nTarget = read.ReadUInt16();
                    skill.s_nAttType = read.ReadUInt16();
                    skill.s_fAttRange = read.ReadSingle();
                    skill.s_fAttRange_MinDmg = read.ReadSingle();
                    skill.s_fAttRange_NorDmg = read.ReadSingle();
                    skill.s_fAttRange_MaxDmg = read.ReadSingle();
                    skill.s_nAttSphere = read.ReadUInt16();


                    CultureInfo cultureInfo = new CultureInfo("en-US");
                    CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                    CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

                    var s_fCastingTimeString = read.ReadSingle().ToString();

                    string valorNumerico = Regex.Match(s_fCastingTimeString, @"\d+").Value;

                    if( skill.s_dwID == 5100921)
                    {

                    }
                    skill.s_fCastingTime = float.Parse(valorNumerico);
                    skill.s_fDamageTime = read.ReadSingle();


                    skill.s_nDamageDay = read.ReadInt32();
                    skill.ink = read.ReadUInt16();
                    skill.s_nDistanceTime = read.ReadSingle();
                    skill.s_fCooldownTime = read.ReadSingle();
                    skill.s_nCooldownDay = read.ReadUInt16();
                    skill.unk = read.ReadUInt16();
                    skill.s_fSkill_Velocity = read.ReadSingle();
                    skill.s_fSkill_Accel = read.ReadSingle();
                    skill.s_nSkillType = read.ReadUInt16();
                    skill.s_nLimitLevel = read.ReadUInt16();
                    skill.s_nSkillGroup = read.ReadUInt16();
                    skill.s_nSkillRank = read.ReadUInt16();
                    skill.s_nMemorySkill = read.ReadUInt16();
                    skill.s_nReq_Item = read.ReadUInt16();      //


                    Skill[i] = skill;
                }


                int tcount = read.ReadInt32();
                TamerSkill[] tamerSkill = new TamerSkill[tcount];
                for (int i = 0; i < tcount; i++)
                {
                    TamerSkill tamer = new();
                    tamer.s_nIndex = read.ReadUInt16();
                    bool IsNormal = tamer.s_nIndex >= 7;

                    if (IsNormal)
                    {
                        tamer.otherunknow = read.ReadUInt16();
                    }
                    tamer.s_dwSkillCode = read.ReadInt32();
                    if (!IsNormal)
                    {
                        tamer.unknow = read.ReadInt16();
                    }
                    tamer.s_nType = read.ReadUInt16();

                    tamer.unknow1 = read.ReadInt16();

                    tamer.s_dwFactor1 = read.ReadInt32();
                    tamer.s_dwFactor2 = read.ReadInt32();
                    tamer.s_dwTamer_SeqID = read.ReadInt32();
                    tamer.s_dwDigimon_SeqID = read.ReadInt32();
                    tamer.s_nUseState = read.ReadUInt16();
                    tamer.s_nUse_Are_Check = read.ReadUInt16();
                    tamer.s_nAvailable = read.ReadUInt16();
                    tamer.unknow3 = read.ReadInt16();


                    tamerSkill[i] = tamer;
                }

                int acount = read.ReadInt32();
                AreaCheck[] areaCheck = new AreaCheck[acount];
                for (int i = 0; i < acount; i++)
                {
                    AreaCheck area = new();
                    area.s_dwIndex = read.ReadInt32();
                    area.s_nArea = new ushort[30];
                    for (int z = 0; z < 30; z++)
                    {
                        area.s_nArea[z] = read.ReadUInt16();
                    }

                    areaCheck[i] = area;
                }

                return (Skill, tamerSkill, areaCheck);
            }
        }
        static double ConvertToMilliseconds(string scientificNotation)
        {
            if (double.TryParse(scientificNotation, out double value))
            { if( value > 0)
                {
                    long seconds = (long)(value * 1e6); // 1e9 é o mesmo que 10^9, que representa bilhões de segundos

                    if(seconds > 0)
                    {

                    }
                }
                double milliseconds = value * 1000;
                return milliseconds;
            }
            else
            {
                throw new ArgumentException("Valor em notação científica inválido.");
            }
        }
        public static void ExportSkillToXml(string filePath, SkillData[] skillDataArray)
        {
            CultureInfo cultureInfo = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            var xml = new XElement("SkillDataArray",
          skillDataArray.Select(skillData =>
              new XElement("SkillData",
                  new XElement("s_dwID", skillData.s_dwID),
                  new XElement("s_szName", skillData.s_szName),
                  new XElement("s_szComment", skillData.s_szComment),
                  new XElement("SkillApply", skillData.SkillApply.Select(sa =>
                      new XElement("IncreaseApply",
                          new XElement("s_nA", sa.s_nA),
                          new XElement("s_nInvoke_Rate", sa.s_nInvoke_Rate),
                          new XElement("s_nB", sa.s_nB),
                          new XElement("s_nC", sa.s_nC),
                          new XElement("s_nBuffCode", sa.s_nBuffCode),
                          new XElement("s_nID", sa.s_nID),
                          new XElement("s_nIncrease_B_Point", sa.s_nIncrease_B_Point)
                      )
                  )),
                  new XElement("s_nLevelupPoint", skillData.s_nLevelupPoint),
                  new XElement("s_nMaxLevel", skillData.s_nMaxLevel),
                  new XElement("s_nAttributeType", skillData.s_nAttributeType),
                  new XElement("s_nNatureType", skillData.s_nNatureType),
                  new XElement("s_nFamilyType", skillData.s_nFamilyType),
                  new XElement("s_nUseHP", skillData.s_nUseHP),
                  new XElement("s_nUseDS", skillData.s_nUseDS),
                  new XElement("s_nIcon", skillData.s_nIcon),
                  new XElement("s_nTarget", skillData.s_nTarget),
                  new XElement("s_nAttType", skillData.s_nAttType),
                  new XElement("s_fAttRange", XmlConvert.ToString(skillData.s_fAttRange)),
                  new XElement("s_fAttRange_MinDmg", XmlConvert.ToString(skillData.s_fAttRange_MinDmg)),
                  new XElement("s_fAttRange_NorDmg", XmlConvert.ToString(skillData.s_fAttRange_NorDmg)),
                  new XElement("s_fAttRange_MaxDmg", XmlConvert.ToString(skillData.s_fAttRange_MaxDmg)),
                  new XElement("s_nAttSphere", skillData.s_nAttSphere),
                  new XElement("s_fCastingTime", XmlConvert.ToString(skillData.s_fCastingTime)),
                  new XElement("s_fDamageTime", XmlConvert.ToString(skillData.s_fDamageTime)),
                  new XElement("s_nDamageDay", skillData.s_nDamageDay),
                   new XElement("ink", skillData.ink),
                  new XElement("s_nDistanceTime", XmlConvert.ToString(skillData.s_nDistanceTime)),
                  new XElement("s_fCooldownTime", XmlConvert.ToString(skillData.s_fCooldownTime)),
                  new XElement("s_nCooldownDay", XmlConvert.ToString(skillData.s_nCooldownDay)),
                    new XElement("unk", skillData.unk),
                  new XElement("s_fSkill_Velocity", XmlConvert.ToString(skillData.s_fSkill_Velocity)),
                  new XElement("s_fSkill_Accel", XmlConvert.ToString(skillData.s_fSkill_Accel)),
                  new XElement("s_nSkillType", skillData.s_nSkillType),
                  new XElement("s_nLimitLevel", skillData.s_nLimitLevel),
                  new XElement("s_nSkillGroup", skillData.s_nSkillGroup),
                  new XElement("s_nSkillRank", skillData.s_nSkillRank),
                  new XElement("s_nMemorySkill", skillData.s_nMemorySkill),
                  new XElement("s_nReq_Item", skillData.s_nReq_Item),

                  new XElement("unk2", skillData.unk2)
              )
          )
      );

            xml.Save(filePath);
        }
        public static void ExportTamerSkillToXml(TamerSkill[] tamerSkills, string filePath)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(filePath, settings))
            {
                writer.WriteStartDocument();

                writer.WriteStartElement("TamerSkillArray");

                foreach (TamerSkill tamerSkill in tamerSkills)
                {
                    writer.WriteStartElement("TamerSkill");
                    writer.WriteElementString("s_nIndex", tamerSkill.s_nIndex.ToString());

                    bool IsSkill = tamerSkill.s_nIndex >= 7;

                    if (IsSkill)
                    {
                        writer.WriteElementString("otherunknow", tamerSkill.otherunknow.ToString());
                    }

                    writer.WriteElementString("s_dwSkillCode", tamerSkill.s_dwSkillCode.ToString());

                    if (!IsSkill)
                    {
                        writer.WriteElementString("unknow", tamerSkill.unknow.ToString());
                    }

                    writer.WriteElementString("s_nType", tamerSkill.s_nType.ToString());
                    writer.WriteElementString("unknow1", tamerSkill.unknow1.ToString());
                    writer.WriteElementString("s_dwFactor1", tamerSkill.s_dwFactor1.ToString());
                    writer.WriteElementString("s_dwFactor2", tamerSkill.s_dwFactor2.ToString());
                    writer.WriteElementString("s_dwTamer_SeqID", tamerSkill.s_dwTamer_SeqID.ToString());
                    writer.WriteElementString("s_dwDigimon_SeqID", tamerSkill.s_dwDigimon_SeqID.ToString());
                    writer.WriteElementString("s_nUseState", tamerSkill.s_nUseState.ToString());
                    writer.WriteElementString("s_nUse_Are_Check", tamerSkill.s_nUse_Are_Check.ToString());
                    writer.WriteElementString("s_nAvailable", tamerSkill.s_nAvailable.ToString());
                    writer.WriteElementString("unknow3", tamerSkill.unknow3.ToString());

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

        }
        public static void ExportAreaCheckToXml(AreaCheck[] areaChecks, string filePath)
        {
            var xml = new XElement("AreaCheckArray",
                areaChecks.Select(areaCheck =>
                    new XElement("AreaCheck",
                        new XElement("s_dwIndex", areaCheck.s_dwIndex),
                        new XElement("s_nArea", areaCheck.s_nArea.Select(area => new XElement("Area", area)))
                    )
                )
            );

            xml.Save(filePath);
        }
        public static SkillData[] ImportSkillFromXml(string filePath)
        {
            var xml = XDocument.Load(filePath);
            var skillDataArray = xml.Descendants("SkillData").Select(skillDataElement =>
            {
                var skillData = new SkillData
                {
                    s_dwID = int.Parse(skillDataElement.Element("s_dwID").Value),
                    s_szName = skillDataElement.Element("s_szName").Value,
                    s_szComment = skillDataElement.Element("s_szComment").Value,
                    s_nLevelupPoint = ushort.Parse(skillDataElement.Element("s_nLevelupPoint").Value),
                    s_nMaxLevel = ushort.Parse(skillDataElement.Element("s_nMaxLevel").Value),
                    s_nAttributeType = ushort.Parse(skillDataElement.Element("s_nAttributeType").Value),
                    s_nNatureType = ushort.Parse(skillDataElement.Element("s_nNatureType").Value),
                    s_nFamilyType = ushort.Parse(skillDataElement.Element("s_nFamilyType").Value),
                    s_nUseHP = ushort.Parse(skillDataElement.Element("s_nUseHP").Value),
                    s_nUseDS = ushort.Parse(skillDataElement.Element("s_nUseDS").Value),
                    s_nIcon = ushort.Parse(skillDataElement.Element("s_nIcon").Value),
                    s_nTarget = ushort.Parse(skillDataElement.Element("s_nTarget").Value),
                    s_nAttType = ushort.Parse(skillDataElement.Element("s_nAttType").Value),
                    s_fAttRange = XmlConvert.ToSingle(skillDataElement.Element("s_fAttRange").Value),
                    s_fAttRange_MinDmg = XmlConvert.ToSingle(skillDataElement.Element("s_fAttRange_MinDmg").Value),
                    s_fAttRange_NorDmg = XmlConvert.ToSingle(skillDataElement.Element("s_fAttRange_NorDmg").Value),
                    s_fAttRange_MaxDmg = XmlConvert.ToSingle(skillDataElement.Element("s_fAttRange_MaxDmg").Value),
                    s_nAttSphere = ushort.Parse(skillDataElement.Element("s_nAttSphere").Value),
                    s_fCastingTime = XmlConvert.ToSingle(skillDataElement.Element("s_fCastingTime").Value),
                    s_fDamageTime = XmlConvert.ToSingle(skillDataElement.Element("s_fDamageTime").Value),
                    s_nDamageDay = int.Parse(skillDataElement.Element("s_nDamageDay").Value),
                    ink = ushort.Parse(skillDataElement.Element("ink").Value),
                    s_nDistanceTime = XmlConvert.ToSingle(skillDataElement.Element("s_nDistanceTime").Value),
                    s_fCooldownTime = XmlConvert.ToSingle(skillDataElement.Element("s_fCooldownTime").Value),
                    s_nCooldownDay = ushort.Parse(skillDataElement.Element("s_nCooldownDay").Value),
                    unk = ushort.Parse(skillDataElement.Element("unk").Value),
                    s_fSkill_Velocity = XmlConvert.ToSingle(skillDataElement.Element("s_fSkill_Velocity").Value),
                    s_fSkill_Accel = XmlConvert.ToSingle(skillDataElement.Element("s_fSkill_Accel").Value),
                    s_nSkillType = ushort.Parse(skillDataElement.Element("s_nSkillType").Value),
                    s_nLimitLevel = ushort.Parse(skillDataElement.Element("s_nLimitLevel").Value),
                    s_nSkillGroup = ushort.Parse(skillDataElement.Element("s_nSkillGroup").Value),
                    s_nSkillRank = ushort.Parse(skillDataElement.Element("s_nSkillRank").Value),
                    s_nMemorySkill = ushort.Parse(skillDataElement.Element("s_nMemorySkill").Value),
                    s_nReq_Item = ushort.Parse(skillDataElement.Element("s_nReq_Item").Value),
                    unk2 = ushort.Parse(skillDataElement.Element("unk2").Value)
                };

                var skillApplyElements = skillDataElement.Descendants("IncreaseApply");
                foreach (var skillApplyElement in skillApplyElements)
                {
                    var increaseApply = new IncreaseApply
                    {
                        s_nA = int.Parse(skillApplyElement.Element("s_nA").Value),
                        s_nInvoke_Rate = int.Parse(skillApplyElement.Element("s_nInvoke_Rate").Value),
                        s_nB = int.Parse(skillApplyElement.Element("s_nB").Value),
                        s_nC = int.Parse(skillApplyElement.Element("s_nC").Value),
                        s_nBuffCode = ushort.Parse(skillApplyElement.Element("s_nBuffCode").Value),
                        s_nID = ushort.Parse(skillApplyElement.Element("s_nID").Value),
                        s_nIncrease_B_Point = int.Parse(skillApplyElement.Element("s_nIncrease_B_Point").Value)
                    };

                    skillData.SkillApply.Add(increaseApply);
                }

                return skillData;
            }).ToArray();

            return skillDataArray;
        }
        public static TamerSkill[] ImportTamerSkillFromXml(string filePath)
        {
            var tamerSkills = new List<TamerSkill>();

            var xml = XDocument.Load(filePath);
            foreach (var element in xml.Root.Elements("TamerSkill"))
            {
                var tamerSkill = new TamerSkill();

                tamerSkill.s_nIndex = ushort.Parse(element.Element("s_nIndex").Value);

                bool IsSkill = tamerSkill.s_nIndex >= 7;

                if (IsSkill)
                {
                    tamerSkill.otherunknow = ushort.Parse(element.Element("otherunknow").Value);
                }

                tamerSkill.s_dwSkillCode = int.Parse(element.Element("s_dwSkillCode").Value);
                if (!IsSkill)
                {
                    tamerSkill.unknow = short.Parse(element.Element("unknow").Value);
                }
                tamerSkill.s_nType = ushort.Parse(element.Element("s_nType").Value);
                tamerSkill.unknow1 = short.Parse(element.Element("unknow1").Value);
                tamerSkill.s_dwFactor1 = int.Parse(element.Element("s_dwFactor1").Value);
                tamerSkill.s_dwFactor2 = int.Parse(element.Element("s_dwFactor2").Value);
                tamerSkill.s_dwTamer_SeqID = int.Parse(element.Element("s_dwTamer_SeqID").Value);
                tamerSkill.s_dwDigimon_SeqID = int.Parse(element.Element("s_dwDigimon_SeqID").Value);
                tamerSkill.s_nUseState = ushort.Parse(element.Element("s_nUseState").Value);
                tamerSkill.s_nUse_Are_Check = ushort.Parse(element.Element("s_nUse_Are_Check").Value);
                tamerSkill.s_nAvailable = ushort.Parse(element.Element("s_nAvailable").Value);
                tamerSkill.unknow3 = short.Parse(element.Element("unknow3").Value);

                tamerSkills.Add(tamerSkill);
            }

            return tamerSkills.ToArray();
        }
        public static AreaCheck[] ImportAreaCheckFromXml(string filePath)
        {
            var areaChecks = new List<AreaCheck>();

            var xml = XDocument.Load(filePath);
            foreach (var element in xml.Root.Elements("AreaCheck"))
            {
                var areaCheck = new AreaCheck
                {
                    s_dwIndex = int.Parse(element.Element("s_dwIndex").Value),
                    s_nArea = element.Element("s_nArea").Elements("Area").Select(area => ushort.Parse(area.Value)).ToArray()
                };

                areaChecks.Add(areaCheck);
            }

            return areaChecks.ToArray();
        }
        public static void ExportSkillToBinary(string filePath, SkillData[] skillDataArray, TamerSkill[] tamerSkills, AreaCheck[] areaChecks)
        {
            using (var writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {
                writer.Write(skillDataArray.Length);

                foreach (var skillData in skillDataArray)
                {
                    writer.Write(skillData.s_dwID);                

                    // Escrever a propriedade s_szName
                    byte[] s_szNameBytes = Encoding.Unicode.GetBytes(skillData.s_szName);
                    for (int i = 0; i < 64; i++)
                    {
                        if (i < s_szNameBytes.Length)
                        {
                            writer.Write(s_szNameBytes[i]);
                        }
                        else
                        {
                            writer.Write((byte)0); // Preencher com zeros se a string for menor que 64 bytes
                        }
                    }

                    // Escrever a propriedade s_szComment
                    byte[] s_szCommentBytes = Encoding.Unicode.GetBytes(skillData.s_szComment);
                    for (int i = 0; i < 512; i++)
                    {
                        if (i < s_szCommentBytes.Length)
                        {
                            writer.Write(s_szCommentBytes[i]);
                        }
                        else
                        {
                            writer.Write((byte)0); // Preencher com zeros se a string for menor que 512 bytes
                        }
                    }

                    foreach (var increaseApply in skillData.SkillApply)
                    {
                        writer.Write(increaseApply.s_nA);
                        writer.Write(increaseApply.s_nInvoke_Rate);
                        writer.Write(increaseApply.s_nB);
                        writer.Write(increaseApply.s_nC);
                        writer.Write(increaseApply.s_nBuffCode);
                        writer.Write(increaseApply.s_nID);
                        writer.Write(increaseApply.s_nIncrease_B_Point);
                    }

                    writer.Write(skillData.s_nLevelupPoint);
                    writer.Write(skillData.s_nMaxLevel);
                    writer.Write(skillData.s_nAttributeType);
                    writer.Write(skillData.s_nNatureType);
                    writer.Write(skillData.s_nFamilyType);
                    writer.Write(skillData.s_nUseHP);
                    writer.Write(skillData.s_nUseDS);
                    writer.Write(skillData.s_nIcon);
                    writer.Write(skillData.s_nTarget);
                    writer.Write(skillData.s_nAttType);
                    writer.Write(skillData.s_fAttRange);
                    writer.Write(skillData.s_fAttRange_MinDmg);
                    writer.Write(skillData.s_fAttRange_NorDmg);
                    writer.Write(skillData.s_fAttRange_MaxDmg);
                    writer.Write(skillData.s_nAttSphere);
                    writer.Write(skillData.s_fCastingTime);
                    writer.Write(skillData.s_fDamageTime);
                    writer.Write(skillData.s_nDamageDay);
                    writer.Write(skillData.ink);
                    writer.Write(skillData.s_nDistanceTime);
                    writer.Write(skillData.s_fCooldownTime);
                    writer.Write(skillData.s_nCooldownDay);
                    writer.Write(skillData.unk);
                    writer.Write(skillData.s_fSkill_Velocity);
                    writer.Write(skillData.s_fSkill_Accel);
                    writer.Write(skillData.s_nSkillType);
                    writer.Write(skillData.s_nLimitLevel);
                    writer.Write(skillData.s_nSkillGroup);
                    writer.Write(skillData.s_nSkillRank);
                    writer.Write(skillData.s_nMemorySkill);
                    writer.Write(skillData.s_nReq_Item);

                }

                writer.Write(tamerSkills.Length);

                foreach (var tamerSkill in tamerSkills)
                {
                    bool IsSkill = tamerSkill.s_nIndex >= 7;

                    writer.Write(tamerSkill.s_nIndex);
                    if (IsSkill)
                    {
                        writer.Write(tamerSkill.otherunknow);
                    }
                    writer.Write(tamerSkill.s_dwSkillCode);
                    if (!IsSkill)
                    {
                        writer.Write(tamerSkill.unknow);
                    }
                    writer.Write(tamerSkill.s_nType);
                    writer.Write(tamerSkill.unknow1);
                    writer.Write(tamerSkill.s_dwFactor1);
                    writer.Write(tamerSkill.s_dwFactor2);
                    writer.Write(tamerSkill.s_dwTamer_SeqID);
                    writer.Write(tamerSkill.s_dwDigimon_SeqID);
                    writer.Write(tamerSkill.s_nUseState);
                    writer.Write(tamerSkill.s_nUse_Are_Check);
                    writer.Write(tamerSkill.s_nAvailable);
                    writer.Write(tamerSkill.unknow3);
                }

                writer.Write(areaChecks.Length);

                foreach (var areaCheck in areaChecks)
                {
                    writer.Write(areaCheck.s_dwIndex);

                    // Escrever o número de áreas
                    writer.Write(areaCheck.s_nArea.Length);

                    foreach (var area in areaCheck.s_nArea)
                    {
                        writer.Write(area);
                    }
                }
            }
        }

    }

    public class IncreaseApply
    {
        public int s_nA;
        public int s_nInvoke_Rate;     // Probabilidade de ativação
        public int s_nB;
        public int s_nC;
        public ushort s_nBuffCode;      // No caso de habilidade do item, código de bônus, habilidade domador/Digimon, alvo do efeito da habilidade
        public ushort s_nID;
        public int s_nIncrease_B_Point;

    }

    public class TamerSkill
    {
        public ushort s_nIndex;                // 인덱스
        public int s_dwSkillCode;            // 스킬 코드 번호
        public ushort s_nType;             // 스킬 타입 ( 1: 공격, 2: 버프 / 디버프, 3: 스크립트 스킬 )
        public short unknow;
        public short unknow1;
        public short unknow3;
        public int s_dwFactor1;          // 스킬 타입에 따른 입력
        public int s_dwFactor2;
        public int s_dwTamer_SeqID;      // 테이머 시퀀스 아이디
        public int s_dwDigimon_SeqID;        // 디지몬 시퀀스 아이디
        public ushort s_nUseState;         // 사용 가능 상태 ( 0: 항상 사용, 1: 전투 중 사용 불가 )
        public ushort s_nUse_Are_Check;        // 지역설정 사용여부 ( 1: 스킬을 사용할 수 있는 지역을 설정하겠다, 0: 아무곳에서나 사용가능 )
        public ushort s_nAvailable;			// 가능/불가능( 1: 해당 지역에서 사용 불가능, 2: 해당 지역에서 사용 가능
        public ushort otherunknow;
    }

    public class AreaCheck
    {

        public int s_dwIndex;            // 인덱스 (스킬 코드 번호)
        public ushort[] s_nArea;		// 지역
    }
}
