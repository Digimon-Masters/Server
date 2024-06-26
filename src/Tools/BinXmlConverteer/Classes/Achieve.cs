using System.Text;
using System.Xml.Linq;


namespace BinXmlConverter.Classes
{
    public class Achieve
    {
        public class AchieveSINFO
        {
            public int s_nQuestID;
            public int s_nIcon;
            public ushort s_nPoint;
            public byte s_bDisplay;
            public byte s_bDisplay2;
            public string s_szName;
            public string s_szComment;
            public string s_szTitle;
            public int s_nGroup;
            public int s_nSubGroup;
            public int s_nType;
            public int s_nBuffCode;
        }
        public class sINFO
        {
            public string s_szName;
            public int s_listChild;

        }

        public static (sINFO[], AchieveSINFO[]) ReadAchieveFromBinary(string FileInput)
        {

            using (BitReader reader = new BitReader(File.Open(FileInput, FileMode.Open)))
            {
                // Lê os Gotchas
                sINFO[] sInfo = ReadsINFOFromBinary(reader);
                AchieveSINFO[] sINFOs = ReadsAchieveFromBinary(reader);

                return (sInfo, sINFOs);
            }


        }

        public static sINFO[] ReadsINFOFromBinary(BitReader read)
        {
            int dcount = 18;   //Parte sem funçao alguma ainda nao descobri o sentido dela
            sINFO[] sINFOs = new sINFO[dcount];
            for (int i = 0; i < dcount; i++)
            {
                sINFO achieve = new sINFO();
                achieve.s_szName = read.ReadZString(Encoding.Unicode, 32 * 2);   // wchar_t colocar valor multiplicar por 2
                achieve.s_listChild = read.ReadInt();
                sINFOs[i] = achieve;
            }

            return sINFOs;
        }

        public static AchieveSINFO[] ReadsAchieveFromBinary(BitReader read)
        {
            int count = read.ReadInt();
            AchieveSINFO[] sINFOs = new AchieveSINFO[count];
            for (int i = 0; i < count; i++)
            {

                AchieveSINFO achieve = new AchieveSINFO();
                achieve.s_nQuestID = read.ReadInt();
                achieve.s_nIcon = read.ReadInt();
                achieve.s_nPoint = read.ReadUShort();
                achieve.s_bDisplay = read.ReadByte();
                achieve.s_bDisplay2 = read.ReadByte();
                achieve.s_szName = read.ReadZString(Encoding.Unicode, 64 * 2);   // wchar_t colocar valor multiplicar por 2
                achieve.s_szComment = read.ReadZString(Encoding.Unicode, 256 * 2);   // wchar_t colocar valor multiplicar por 2
                achieve.s_szTitle = read.ReadZString(Encoding.Unicode, 64 * 2);   // wchar_t colocar valor multiplicar por 2
                achieve.s_nGroup = read.ReadInt();
                achieve.s_nSubGroup = read.ReadInt();
                achieve.s_nType = read.ReadInt();
                achieve.s_nBuffCode = read.ReadInt();

                sINFOs[i] = achieve;
            }

            return sINFOs;
        }
        public static void ExportSinfoToXml(string filePath, sINFO[] sinfos)
        {
            XElement rootElement = new XElement("sINFOs");

            foreach (sINFO sinfo in sinfos)
            {
                XElement sinfoElement = new XElement("sINFO",
                    new XElement("s_szName", sinfo.s_szName),
                    new XElement("s_listChild", sinfo.s_listChild)
                );

                rootElement.Add(sinfoElement);
            }

            XDocument xmlDoc = new XDocument(rootElement);
            xmlDoc.Save(filePath);
        }
        public static void ExportAchieveSinfoToXml(string filePath, AchieveSINFO[] achieveInfos)
        {
            XElement rootElement = new XElement("AchieveSINFOs");

            foreach (AchieveSINFO achieveInfo in achieveInfos)
            {
                XElement achieveInfoElement = new XElement("AchieveSINFO",
                    new XElement("s_nQuestID", achieveInfo.s_nQuestID),
                    new XElement("s_nIcon", achieveInfo.s_nIcon),
                    new XElement("s_nPoint", achieveInfo.s_nPoint),
                    new XElement("s_bDisplay", achieveInfo.s_bDisplay),
                    new XElement("s_bDisplay2", achieveInfo.s_bDisplay2),
                    new XElement("s_szName", achieveInfo.s_szName),
                    new XElement("s_szComment", achieveInfo.s_szComment),
                    new XElement("s_szTitle", achieveInfo.s_szTitle),
                    new XElement("s_nGroup", achieveInfo.s_nGroup),
                    new XElement("s_nSubGroup", achieveInfo.s_nSubGroup),
                    new XElement("s_nType", achieveInfo.s_nType),
                    new XElement("s_nBuffCode", achieveInfo.s_nBuffCode)
                );

                rootElement.Add(achieveInfoElement);
            }

            XDocument xmlDoc = new XDocument(rootElement);
            xmlDoc.Save(filePath);
        }
        public static sINFO[] ImportsInfoFromXml(string filePath)
        {
            XDocument xmlDoc = XDocument.Load(filePath);
            var sInfoElements = xmlDoc.Descendants("sINFO");

            List<sINFO> sInfoList = new List<sINFO>();

            foreach (var element in sInfoElements)
            {
                sINFO sInfo = new sINFO
                {
                    s_szName = (string)element.Element("s_szName"),
                    s_listChild = (int)element.Element("s_listChild")
                };

                sInfoList.Add(sInfo);
            }

            return sInfoList.ToArray();
        }
        public static AchieveSINFO[] ImportAchieveSinfoFromXml(string filePath)
        {
            XDocument xmlDoc = XDocument.Load(filePath);
            var achieveInfoElements = xmlDoc.Descendants("AchieveSINFO");

            AchieveSINFO[] achieveInfos = new AchieveSINFO[achieveInfoElements.Count()];
            int index = 0;

            foreach (var element in achieveInfoElements)
            {
                AchieveSINFO achieveInfo = new AchieveSINFO
                {
                    s_nQuestID = (int)element.Element("s_nQuestID"),
                    s_nIcon = (int)element.Element("s_nIcon"),
                    s_nPoint = (ushort)(int)element.Element("s_nPoint"),
                    s_bDisplay = (byte)(int)element.Element("s_bDisplay"),
                    s_bDisplay2 = (byte)(int)element.Element("s_bDisplay2"),
                    s_szName = (string)element.Element("s_szName"),
                    s_szComment = (string)element.Element("s_szComment"),
                    s_szTitle = (string)element.Element("s_szTitle"),
                    s_nGroup = (int)element.Element("s_nGroup"),
                    s_nSubGroup = (int)element.Element("s_nSubGroup"),
                    s_nType = (int)element.Element("s_nType"),
                    s_nBuffCode = (int)element.Element("s_nBuffCode")
                };

                achieveInfos[index] = achieveInfo;
                index++;
            }

            return achieveInfos;
        }
        public static void WriteAchieveToBinary(string FileOutput, sINFO[] sINFOs, AchieveSINFO[] achieveSINFOs)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(FileOutput, FileMode.Create)))
            {

                foreach (var item in sINFOs)
                {
                    for (int i = 0; i < 32; i++)
                    {
                        char c = i < item.s_szName.Length ? item.s_szName[i] : '\0';
                        writer.Write((ushort)c);
                    }

                    writer.Write(item.s_listChild);
                }

                writer.Write(achieveSINFOs.Length);
                foreach (var achieve in achieveSINFOs)
                {
                    writer.Write(achieve.s_nQuestID);
                    writer.Write(achieve.s_nIcon);
                    writer.Write(achieve.s_nPoint);
                    writer.Write(achieve.s_bDisplay);
                    writer.Write(achieve.s_bDisplay2);

                    for (int i = 0; i < 64; i++)
                    {
                        char c = i < achieve.s_szName.Length ? achieve.s_szName[i] : '\0';
                        writer.Write((ushort)c);
                    }

                    for (int i = 0; i < 256; i++)
                    {
                        char c = i < achieve.s_szComment.Length ? achieve.s_szComment[i] : '\0';
                        writer.Write((ushort)c);
                    }


                    for (int i = 0; i < 64; i++)
                    {
                        char c = i < achieve.s_szTitle.Length ? achieve.s_szTitle[i] : '\0';
                        writer.Write((ushort)c);
                    }

                    writer.Write(achieve.s_nGroup);
                    writer.Write(achieve.s_nSubGroup);
                    writer.Write(achieve.s_nType);
                    writer.Write(achieve.s_nBuffCode);
                }
            }

        }
    }
}

