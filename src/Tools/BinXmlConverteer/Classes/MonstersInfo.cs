using System.Data;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using static BinXmlConverter.Classes.MonstersInfo;

namespace BinXmlConverter.Classes
{
    public class MonstersInfo
    {
        public class Monsters
        {
            public int Ht;
            public int Lv;
            public int HP;
            public int DS;
            public uint Activetype;
            public uint TalkID;
            public uint Range;
            public ushort DE;
            public ushort EV;
            public ushort MS;
            public ushort WS;
            public ushort CT;
            public ushort AT;
            public ushort AS;
            public ushort AR;
            public ushort HT;
            public ushort Sight;
            public ushort HuntRange;
            public ushort EXP;
            public ushort Battle;
            public short Unknown;
            public short Level;
            public string Name;
            public string Tag;
            public string Comment;
            public string Title;
            public ushort Unknown2;
            public ushort Class; // monster types
            public short Icon1;
            public short Icon2;
            public short Icon3;
            public short Icon4;
            public short Icon5;
            public short Icon6;
            public ushort ExpMin;
            public ushort ExpMax;
            public ushort Unknown3;
            public int MonsterID;
            public int ModelDigimon;
            public short Skill_Type;
            public short unk;
            public short RangeIDX;
            public short Ani_Delay;
            public short Valocity;
            public short Accel;
            public short Eff_Factor;
            public short IDX;
            public short Direction;
            public short TargetingType;
            public short RefCode;
            public float Scale;
            public short Skill_IDX;
            public short SequenceID;
            public short Eff_Val_Min;
            public short Eff_Val_Max;
            public short CoolTime;
            public short CastTime;
            public short Eff_Fact_Val;
        }
        public class MonsterHit
        {
            public int Lv;
            public int Ht;
        }
        public class MonsterSkill
        {
            public ushort Skill_IDX;
            public ushort unk;
            public int MonsterID;
            public int CoolTime;
            public int CastTime;
            public ushort CastCheck;
            public ushort Target_Cnt;
            public ushort Target_MinCnt;
            public ushort Target_MaxCNt;
            public ushort UseTerms;
            public ushort Skill_Type;
            public int Eff_Val_Min;
            public int Eff_Val_Max;
            public ushort unk2;
            public ushort RangeIDX;
            public int SequenceID;
            public ushort Ani_Delay;
            public ushort Valocity;
            public ushort Accel;
            public ushort Eff_Factor;
            public ushort Eff_Factor2;
            public ushort Eff_Factor3;
            public int Eff_Fact_Val;
            public int Eff_Fact_Val2;
            public int Eff_Fact_Val3;
            public uint TalkID;
            public uint Activetype;
            public float NoticeTime;
            public string NoticeEffname;
        }
        public class MonsterSkillTerms
        {
            public ushort IDX;
            public ushort Direction;
            public uint Range;
            public ushort TargetingType;
            public ushort RefCode;
        }

        public static (Monsters[], MonsterHit[], MonsterSkill[], MonsterSkillTerms[]) ReadMonstersFromBinary(string filePath)
        {
            using (BitReader reader = new BitReader(File.Open(filePath, FileMode.Open)))
            {
                Monsters[] monsters = ReadMonsters(reader);
                MonsterHit[] monsterHits = ReadMonsterHit(reader);
                MonsterSkill[] monsterSkills = ReadMonsterSkill(reader);
                MonsterSkillTerms[] monsterSkillTerms = ReadMonsterSkillTerms(reader);

                return (monsters, monsterHits, monsterSkills, monsterSkillTerms);
            }
        }
        private static Monsters[] ReadMonsters(BitReader read)
        {
            int count = read.ReadInt();
            Monsters[] monsters = new Monsters[count];

            for (int i = 0; i < count; i++)
            {
                Monsters monster = new Monsters();
                monster.MonsterID = read.ReadInt();
                monster.ModelDigimon = read.ReadInt();
                monster.Name = read.ReadZString(Encoding.Unicode, 128);
                monster.Comment = read.ReadZString(Encoding.Unicode, 68);
                byte[] nameBytees = read.ReadBytes(128);
                var Title = System.Text.Encoding.Unicode.GetString(nameBytees);
                monster.Title = CleanString(Title);
                monster.Comment = AdicionarParenteses(monster.Comment);
                monster.Level = read.ReadShort();
                monster.EXP = read.ReadUShort();
                monster.Battle = read.ReadUShort();
                monster.Unknown = read.ReadShort();
                monster.HP = read.ReadInt();
                monster.DS = read.ReadInt();
                monster.DE = read.ReadUShort();
                monster.EV = read.ReadUShort();
                monster.MS = read.ReadUShort();
                monster.WS = read.ReadUShort();
                monster.CT = read.ReadUShort();
                monster.AT = read.ReadUShort();
                monster.AS = read.ReadUShort();
                monster.AR = read.ReadUShort();
                monster.HT = read.ReadUShort();
                monster.Sight = read.ReadUShort();
                monster.HuntRange = read.ReadUShort();
                monster.Scale = read.ReadFloat();
                monster.Unknown2 = read.ReadUShort();
                monster.Class = read.ReadUShort();
                monster.Icon1 = read.ReadShort();
                monster.Icon2 = read.ReadShort();
                monster.Icon3 = read.ReadShort();
                monster.Icon4 = read.ReadShort();
                monster.Icon5 = read.ReadShort();
                monster.Icon6 = read.ReadShort();
                monster.ExpMin = read.ReadUShort();
                monster.ExpMax = read.ReadUShort();
                monster.Unknown3 = read.ReadUShort();

                monsters[i] = monster;
            }

            return monsters;
        }
        private static MonsterHit[] ReadMonsterHit(BitReader read)
        {
            int dcount = read.ReadInt();
            MonsterHit[] monsters = new MonsterHit[dcount];
            for (int g = 0; g < dcount; g++)
            {
                MonsterHit monsterhit = new MonsterHit();
                monsterhit.Lv = read.ReadInt();
                monsterhit.Ht = read.ReadInt();

                monsters[g] = monsterhit;
            }

            return monsters;
        }
        private static MonsterSkill[] ReadMonsterSkill(BitReader read)
        {
            int scount = read.ReadInt();
            MonsterSkill[] monsters = new MonsterSkill[scount];
            for (int h = 0; h < scount; h++)
            {
                MonsterSkill monsterskill = new MonsterSkill();
                monsterskill.Skill_IDX = read.ReadUShort();
                monsterskill.unk = read.ReadUShort();
                monsterskill.MonsterID = read.ReadInt();
                monsterskill.CoolTime = read.ReadInt();
                monsterskill.CastTime = read.ReadInt();
                monsterskill.CastCheck = read.ReadUShort();
                monsterskill.Target_Cnt = read.ReadUShort();
                monsterskill.Target_MinCnt = read.ReadUShort();
                monsterskill.Target_MaxCNt = read.ReadUShort();
                monsterskill.UseTerms = read.ReadUShort();
                monsterskill.Skill_Type = read.ReadUShort();
                monsterskill.Eff_Val_Min = read.ReadInt();
                monsterskill.Eff_Val_Max = read.ReadInt();
                monsterskill.unk2 = read.ReadUShort();
                monsterskill.RangeIDX = read.ReadUShort();
                monsterskill.SequenceID = read.ReadInt();
                monsterskill.Ani_Delay = read.ReadUShort();
                monsterskill.Valocity = read.ReadUShort();
                monsterskill.Accel = read.ReadUShort();
                monsterskill.Eff_Factor = read.ReadUShort();
                monsterskill.Eff_Factor2 = read.ReadUShort();
                monsterskill.Eff_Factor3 = read.ReadUShort();
                monsterskill.Eff_Fact_Val = read.ReadInt();
                monsterskill.Eff_Fact_Val2 = read.ReadInt();
                monsterskill.Eff_Fact_Val3 = read.ReadInt();
                monsterskill.TalkID = read.ReadUInt();
                monsterskill.Activetype = read.ReadUInt();
                monsterskill.NoticeTime = read.ReadFloat();
       
                byte[] nameBytees = read.ReadBytes(64);
                var Effname = System.Text.Encoding.ASCII.GetString(nameBytees);
                monsterskill.NoticeEffname = CleanString(Effname);

                monsters[h] = monsterskill;                                                              //monsterskill.NoticeEffname = read.ReadZString(Encoding.Unicode, 64);              
            }
            return monsters;
        }
        private static MonsterSkillTerms[] ReadMonsterSkillTerms(BitReader read)
        {
            int jcount = read.ReadInt();
            MonsterSkillTerms[] monsterSkills = new MonsterSkillTerms[jcount];
            for (int j = 0; j < jcount; j++)
            {
                MonsterSkillTerms monsterskillterms = new MonsterSkillTerms();
                monsterskillterms.IDX = read.ReadUShort();
                monsterskillterms.Direction = read.ReadUShort();
                monsterskillterms.Range = read.ReadUInt();
                monsterskillterms.TargetingType = read.ReadUShort();
                monsterskillterms.RefCode = read.ReadUShort();


                monsterSkills[j] = monsterskillterms;
            }

            return monsterSkills;
        }
        public static void ExportMonsterToXml(Monsters[] monsters, string filePath)
        {

            XElement monstersElement = new XElement("Monsters",
                from monster in monsters
                select new XElement("Monster",
                    new XElement("MonsterID", monster.MonsterID),
                    new XElement("ModelDigimon", monster.ModelDigimon),
                    new XElement("Name", monster.Name),
                    new XElement("Comment", monster.Comment),
                    new XElement("Title", monster.Title),
                    new XElement("HP", monster.HP),
                    new XElement("DS", monster.DS),
                    new XElement("DE", monster.DE),
                    new XElement("EV", monster.EV),
                    new XElement("MS", monster.MS),
                    new XElement("WS", monster.WS),
                    new XElement("CT", monster.CT),
                    new XElement("AT", monster.AT),
                    new XElement("AS", monster.AS),
                    new XElement("AR", monster.AR),
                    new XElement("HT", monster.HT),
                    new XElement("Sight", monster.Sight),
                    new XElement("HuntRange", monster.HuntRange),
                    new XElement("Scale", monster.Scale),
                    new XElement("Unknown2", monster.Unknown2),
                    new XElement("Class", monster.Class),
                    new XElement("Icon1", monster.Icon1),
                    new XElement("Icon2", monster.Icon2),
                    new XElement("Icon3", monster.Icon3),
                    new XElement("Icon4", monster.Icon4),
                    new XElement("Icon5", monster.Icon5),
                    new XElement("Icon6", monster.Icon6),
                    new XElement("ExpMin", monster.ExpMin),
                    new XElement("ExpMax", monster.ExpMax),
                    new XElement("Unknown3", monster.Unknown3),
                    new XElement("Title", monster.Title),
                    new XElement("Level", monster.Level),
                    new XElement("EXP", monster.EXP),
                    new XElement("Battle", monster.Battle),
                    new XElement("Unknown", monster.Unknown)
                )
            );

            XDocument xmlDocument = new XDocument(monstersElement);
            xmlDocument.Save(filePath);

        }
        public static void ExportMonsterHitToXml(MonsterHit[] monsters, string filePath)
        {
            XElement rootElement = new XElement("Monsters");

            foreach (MonsterHit monster in monsters)
            {
                XElement monsterElement = new XElement("Monster",
                    new XAttribute("Lv", monster.Lv),
                    new XAttribute("Ht", monster.Ht));

                rootElement.Add(monsterElement);
            }

            XDocument xmlDocument = new XDocument(rootElement);
            xmlDocument.Save(filePath);
        }
        public static void ExportMonsterSkillToXml(MonsterSkill[] skills, string filePath)
        {
            XElement monsterSkillsElement = new XElement("MonsterSkills",
       from monsterSkill in skills
       select new XElement("MonsterSkill",
           new XElement("Skill_IDX", monsterSkill.Skill_IDX),
           new XElement("unk", monsterSkill.unk),
           new XElement("MonsterID", monsterSkill.MonsterID),
           new XElement("CoolTime", monsterSkill.CoolTime),
           new XElement("CastTime", monsterSkill.CastTime),
           new XElement("CastCheck", monsterSkill.CastCheck),
           new XElement("Target_Cnt", monsterSkill.Target_Cnt),
           new XElement("Target_MinCnt", monsterSkill.Target_MinCnt),
           new XElement("Target_MaxCnt", monsterSkill.Target_MaxCNt),
           new XElement("UseTerms", monsterSkill.UseTerms),
           new XElement("Skill_Type", monsterSkill.Skill_Type),
           new XElement("Eff_Val_Min", monsterSkill.Eff_Val_Min),
           new XElement("Eff_Val_Max", monsterSkill.Eff_Val_Max),
           new XElement("unk2", monsterSkill.unk2),
           new XElement("RangeIDX", monsterSkill.RangeIDX),
           new XElement("SequenceID", monsterSkill.SequenceID),
           new XElement("Ani_Delay", monsterSkill.Ani_Delay),
           new XElement("Valocity", monsterSkill.Valocity),
           new XElement("Accel", monsterSkill.Accel),
           new XElement("Eff_Factor", monsterSkill.Eff_Factor),
           new XElement("Eff_Factor2", monsterSkill.Eff_Factor2),
           new XElement("Eff_Factor3", monsterSkill.Eff_Factor3),
           new XElement("Eff_Fact_Val", monsterSkill.Eff_Fact_Val),
           new XElement("Eff_Fact_Val2", monsterSkill.Eff_Fact_Val2),
           new XElement("Eff_Fact_Val3", monsterSkill.Eff_Fact_Val3),
           new XElement("TalkID", monsterSkill.TalkID),
           new XElement("Activetype", monsterSkill.Activetype),
           new XElement("NoticeTime", monsterSkill.NoticeTime),
           new XElement("NoticeEffname", monsterSkill.NoticeEffname)
       )
   );

            XDocument xmlDocument = new XDocument(monsterSkillsElement);
            xmlDocument.Save(filePath);
        }
        public static void ExportMonsterSkillTermsToXml(MonsterSkillTerms[] terms, string filePath)
        {
            XElement rootElement = new XElement("MonsterSkillTerms");

            foreach (MonsterSkillTerms term in terms)
            {
                XElement termElement = new XElement("MonsterSkillTerm",
                    new XElement("IDX", term.IDX),
                    new XElement("Direction", term.Direction),
                    new XElement("Range", term.Range),
                    new XElement("TargetingType", term.TargetingType),
                    new XElement("RefCode", term.RefCode)
                );

                rootElement.Add(termElement);
            }

            XDocument xmlDocument = new XDocument(rootElement);
            xmlDocument.Save(filePath);
        }
        public static Monsters[] ImportMonsterFromXml(string filePath)
        {
            XDocument xmlDocument = XDocument.Load(filePath);

            Monsters[] monsters = (
                from monsterElement in xmlDocument.Descendants("Monster")
                select new Monsters
                {
                    MonsterID = Convert.ToInt32(monsterElement.Element("MonsterID").Value),
                    ModelDigimon = Convert.ToInt32(monsterElement.Element("ModelDigimon").Value),
                    Name = monsterElement.Element("Name").Value,
                    Comment = monsterElement.Element("Comment").Value,
                    Title = monsterElement.Element("Title").Value,
                    HP = Convert.ToInt32(monsterElement.Element("HP").Value),
                    DS = Convert.ToInt32(monsterElement.Element("DS").Value),
                    DE = Convert.ToUInt16(monsterElement.Element("DE").Value),
                    EV = Convert.ToUInt16(monsterElement.Element("EV").Value),
                    MS = Convert.ToUInt16(monsterElement.Element("MS").Value),
                    WS = Convert.ToUInt16(monsterElement.Element("WS").Value),
                    CT = Convert.ToUInt16(monsterElement.Element("CT").Value),
                    AT = Convert.ToUInt16(monsterElement.Element("AT").Value),
                    AS = Convert.ToUInt16(monsterElement.Element("AS").Value),
                    AR = Convert.ToUInt16(monsterElement.Element("AR").Value),
                    HT = Convert.ToUInt16(monsterElement.Element("HT").Value),
                    Sight = Convert.ToUInt16(monsterElement.Element("Sight").Value),
                    HuntRange = Convert.ToUInt16(monsterElement.Element("HuntRange").Value),
                    Scale = Convert.ToSingle(monsterElement.Element("Scale").Value),
                    Unknown2 = Convert.ToUInt16(monsterElement.Element("Unknown2").Value),
                    Class = Convert.ToUInt16(monsterElement.Element("Class").Value),
                    Icon1 = Convert.ToInt16(monsterElement.Element("Icon1").Value),
                    Icon2 = Convert.ToInt16(monsterElement.Element("Icon2").Value),
                    Icon3 = Convert.ToInt16(monsterElement.Element("Icon3").Value),
                    Icon4 = Convert.ToInt16(monsterElement.Element("Icon4").Value),
                    Icon5 = Convert.ToInt16(monsterElement.Element("Icon5").Value),
                    Icon6 = Convert.ToInt16(monsterElement.Element("Icon6").Value),
                    ExpMin = Convert.ToUInt16(monsterElement.Element("ExpMin").Value),
                    ExpMax = Convert.ToUInt16(monsterElement.Element("ExpMax").Value),
                    Unknown3 = Convert.ToUInt16(monsterElement.Element("Unknown3").Value),
                    Level = Convert.ToInt16(monsterElement.Element("Level").Value),
                    EXP = Convert.ToUInt16(monsterElement.Element("EXP").Value),
                    Battle = Convert.ToUInt16(monsterElement.Element("Battle").Value),
                    Unknown = Convert.ToInt16(monsterElement.Element("Unknown").Value)
                }
            ).ToArray();

            return monsters;
        }
        public static MonsterHit[] ImportMonsterHitFromXml(string filePath)
        {
            List<MonsterHit> monstersList = new List<MonsterHit>();

            XDocument xmlDocument = XDocument.Load(filePath);
            foreach (XElement monsterElement in xmlDocument.Descendants("Monster"))
            {
                MonsterHit monster = new MonsterHit();
                monster.Lv = int.Parse(monsterElement.Attribute("Lv").Value);
                monster.Ht = int.Parse(monsterElement.Attribute("Ht").Value);

                monstersList.Add(monster);
            }

            return monstersList.ToArray();
        }
        public static MonsterSkill[] ImportMonsterSkillFromXml(string filePath)
        {
            List<MonsterSkill> skillsList = new List<MonsterSkill>();

            XDocument xmlDocument = XDocument.Load(filePath);
            foreach (XElement skillElement in xmlDocument.Descendants("MonsterSkill"))
            {
                MonsterSkill skill = new MonsterSkill();
                skill.Skill_IDX = ushort.Parse(skillElement.Element("Skill_IDX").Value);
                skill.unk = ushort.Parse(skillElement.Element("unk").Value);
                skill.MonsterID = int.Parse(skillElement.Element("MonsterID").Value);
                skill.CoolTime = int.Parse(skillElement.Element("CoolTime").Value);
                skill.CastTime = int.Parse(skillElement.Element("CastTime").Value);
                skill.CastCheck = ushort.Parse(skillElement.Element("CastCheck").Value);
                skill.Target_Cnt = ushort.Parse(skillElement.Element("Target_Cnt").Value);
                skill.Target_MinCnt = ushort.Parse(skillElement.Element("Target_MinCnt").Value);
                skill.Target_MaxCNt = ushort.Parse(skillElement.Element("Target_MaxCnt").Value);
                skill.UseTerms = ushort.Parse(skillElement.Element("UseTerms").Value);
                skill.Skill_Type = ushort.Parse(skillElement.Element("Skill_Type").Value);
                skill.Eff_Val_Min = int.Parse(skillElement.Element("Eff_Val_Min").Value);
                skill.Eff_Val_Max = int.Parse(skillElement.Element("Eff_Val_Max").Value);
                skill.unk2 = ushort.Parse(skillElement.Element("unk2").Value);
                skill.RangeIDX = ushort.Parse(skillElement.Element("RangeIDX").Value);
                skill.SequenceID = int.Parse(skillElement.Element("SequenceID").Value);
                skill.Ani_Delay = ushort.Parse(skillElement.Element("Ani_Delay").Value);
                skill.Valocity = ushort.Parse(skillElement.Element("Valocity").Value);
                skill.Accel = ushort.Parse(skillElement.Element("Accel").Value);
                skill.Eff_Factor = ushort.Parse(skillElement.Element("Eff_Factor").Value);
                skill.Eff_Factor2 = ushort.Parse(skillElement.Element("Eff_Factor2").Value);
                skill.Eff_Factor3 = ushort.Parse(skillElement.Element("Eff_Factor3").Value);
                skill.Eff_Fact_Val = int.Parse(skillElement.Element("Eff_Fact_Val").Value);
                skill.Eff_Fact_Val2 = int.Parse(skillElement.Element("Eff_Fact_Val2").Value);
                skill.Eff_Fact_Val3 = int.Parse(skillElement.Element("Eff_Fact_Val3").Value);
                skill.TalkID = uint.Parse(skillElement.Element("TalkID").Value);
                skill.Activetype = uint.Parse(skillElement.Element("Activetype").Value);
                skill.NoticeTime = float.Parse(skillElement.Element("NoticeTime").Value);
                skill.NoticeEffname = skillElement.Element("NoticeEffname").Value;

                skillsList.Add(skill);
            }

            return skillsList.ToArray();
        }
        public static MonsterSkillTerms[] ImportMonsterSkillTermsFromXml(string filePath)
        {
            List<MonsterSkillTerms> termsList = new List<MonsterSkillTerms>();

            XDocument xmlDocument = XDocument.Load(filePath);
            foreach (XElement termElement in xmlDocument.Descendants("MonsterSkillTerm"))
            {
                MonsterSkillTerms term = new MonsterSkillTerms();
                term.IDX = ushort.Parse(termElement.Element("IDX").Value);
                term.Direction = ushort.Parse(termElement.Element("Direction").Value);
                term.Range = uint.Parse(termElement.Element("Range").Value);
                term.TargetingType = ushort.Parse(termElement.Element("TargetingType").Value);
                term.RefCode = ushort.Parse(termElement.Element("RefCode").Value);

                termsList.Add(term);
            }

            return termsList.ToArray();
        }
        private static string CleanString(string input)
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
        private static string AdicionarParenteses(string texto)
        {
            if (texto.StartsWith("(") && texto.EndsWith(")"))
            {
                return texto; // A string já contém os parênteses
            }

            return string.Empty; // Retorna uma string vazia se não começar com parênteses
        }
        public static void WriteMonstersToBinary(string Output, Monsters[] monsters, MonsterHit[] monsterHits, MonsterSkill[] monsterSkills, MonsterSkillTerms[] monsterSkillTerms)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(Output, FileMode.Create)))
            {
                writer.Write(monsters.Length);
                foreach (var monster in monsters)
                {
                    writer.Write(monster.MonsterID);
                    writer.Write(monster.ModelDigimon);
                    byte[] NameBytes = Encoding.Unicode.GetBytes(monster.Name);
                    Array.Resize(ref NameBytes, 128 ); // Ajusta o tamanho do array para 512 caracteres wchar_t
                    writer.Write(NameBytes);

                    byte[] commentBytes = Encoding.Unicode.GetBytes(monster.Comment);
                    Array.Resize(ref commentBytes, 68); // Ajusta o tamanho do array para 512 caracteres wchar_t
                    writer.Write(commentBytes);
                    byte[] TitleBytes = Encoding.Unicode.GetBytes(monster.Title);
                    Array.Resize(ref TitleBytes, 128); // Ajusta o tamanho do array para 512 caracteres wchar_t
                    writer.Write(TitleBytes);
                    writer.Write(monster.Level);
                    writer.Write(monster.EXP);
                    writer.Write(monster.Battle);
                    writer.Write(monster.Unknown);
                    writer.Write(monster.HP);
                    writer.Write(monster.DS);
                    writer.Write(monster.DE);
                    writer.Write(monster.EV);
                    writer.Write(monster.MS);
                    writer.Write(monster.WS);
                    writer.Write(monster.CT);
                    writer.Write(monster.AT);
                    writer.Write(monster.AS);
                    writer.Write(monster.AR);
                    writer.Write(monster.HT);
                    writer.Write(monster.Sight);
                    writer.Write(monster.HuntRange);
                    writer.Write(monster.Scale);
                    writer.Write(monster.Unknown2);
                    writer.Write(monster.Class);
                    writer.Write(monster.Icon1);
                    writer.Write(monster.Icon2);
                    writer.Write(monster.Icon3);
                    writer.Write(monster.Icon4);
                    writer.Write(monster.Icon5);
                    writer.Write(monster.Icon6);
                    writer.Write(monster.ExpMin);
                    writer.Write(monster.ExpMax);
                    writer.Write(monster.Unknown3);
                }

                writer.Write(monsterHits.Length);
                foreach (var monsterhit in monsterHits)
                {
                    writer.Write(monsterhit.Lv);
                    writer.Write(monsterhit.Ht);
                }

                writer.Write(monsterSkills.Length);
                foreach (var monsterskill in monsterSkills)
                {
                    writer.Write(monsterskill.Skill_IDX);
                    writer.Write(monsterskill.unk);
                    writer.Write(monsterskill.MonsterID);
                    writer.Write(monsterskill.CoolTime);
                    writer.Write(monsterskill.CastTime);
                    writer.Write(monsterskill.CastCheck);
                    writer.Write(monsterskill.Target_Cnt);
                    writer.Write(monsterskill.Target_MinCnt);
                    writer.Write(monsterskill.Target_MaxCNt);
                    writer.Write(monsterskill.UseTerms);
                    writer.Write(monsterskill.Skill_Type);
                    writer.Write(monsterskill.Eff_Val_Min);
                    writer.Write(monsterskill.Eff_Val_Max);
                    writer.Write(monsterskill.unk2);
                    writer.Write(monsterskill.RangeIDX);
                    writer.Write(monsterskill.SequenceID);
                    writer.Write(monsterskill.Ani_Delay);
                    writer.Write(monsterskill.Valocity);
                    writer.Write(monsterskill.Accel);
                    writer.Write(monsterskill.Eff_Factor);
                    writer.Write(monsterskill.Eff_Factor2);
                    writer.Write(monsterskill.Eff_Factor3);
                    writer.Write(monsterskill.Eff_Fact_Val);
                    writer.Write(monsterskill.Eff_Fact_Val2);
                    writer.Write(monsterskill.Eff_Fact_Val3);
                    writer.Write(monsterskill.TalkID);
                    writer.Write(monsterskill.Activetype);
                    writer.Write(monsterskill.NoticeTime);
                    byte[] NameBytes = Encoding.Unicode.GetBytes(monsterskill.NoticeEffname);
                    Array.Resize(ref NameBytes, 64); // Ajusta o tamanho do array para 512 caracteres wchar_t
                    writer.Write(NameBytes);
                }

                writer.Write(monsterSkillTerms.Length);
                foreach (var monsterskillterms in monsterSkillTerms)
                {
                   writer.Write(monsterskillterms.IDX);
                   writer.Write(monsterskillterms.Direction);
                   writer.Write(monsterskillterms.Range );
                   writer.Write(monsterskillterms.TargetingType);
                   writer.Write(monsterskillterms.RefCode);
                }
                    
            }
        }

    }
}

