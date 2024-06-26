using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using static BinXmlConverter.Classes.EventTotal;

namespace BinXmlConverter.Classes
{

    public class CharCreateTable
    {
        public int tcount;
        public int dwTamerID;
        public byte m_bShow;   //bool
        public byte m_bEnable; //bool
        public int m_nSeasonType;
        public int m_sVoiceSize;
        public string m_sVoiceFile;   //std::string	
        public int m_nIconIdx;
        public int CountDC;
        public List<int> ItemID = new List<int>();
        public static (CharCreateTable[], DigimonCreateTable[]) ReadCharCreateTableFromBinary(string inputFile)
        {
            CharCreateTable[] CharTable = new CharCreateTable[1];
            DigimonCreateTable[] digimonTable = new DigimonCreateTable[1];

            using (BitReader read = new BitReader(File.OpenRead(inputFile)))
            {
                int count = read.ReadInt();
                CharTable = new CharCreateTable[count];
                for (int i = 0; i < count; i++)
                {
                    CharCreateTable create = new CharCreateTable();

                    create.dwTamerID = read.ReadInt();
                    create.m_bShow = read.ReadByte();
                    create.m_bEnable = read.ReadByte();
                    create.m_nSeasonType = read.ReadInt();
                    create.m_sVoiceSize = read.ReadInt();
                    byte[] s_szName = read.ReadBytes(create.m_sVoiceSize);
                    string s_szNameString = System.Text.Encoding.ASCII.GetString(s_szName, 0, create.m_sVoiceSize);
                    create.m_sVoiceFile = CleanString(s_szNameString);
                    create.m_nIconIdx = read.ReadInt();
                    create.CountDC = read.ReadInt();

                    for (int h = 0; h < create.CountDC; h++)
                    {

                        create.ItemID.Add(read.ReadInt());
                    }


                    CharTable[i] = create;
                }

                int digcount = read.ReadInt();
                digimonTable = new DigimonCreateTable[digcount];

                for (int a = 0; a < digcount; a++)
                {
                    DigimonCreateTable create = new DigimonCreateTable();

                    create.m_digimonID = read.ReadInt();
                    create.d_bShow = read.ReadByte();
                    create.d_bEnable = read.ReadByte();
                    create.m_sVoiceSize = read.ReadInt();
                    byte[] s_szName = read.ReadBytes(create.m_sVoiceSize);
                    string s_szNameString = System.Text.Encoding.ASCII.GetString(s_szName, 0, create.m_sVoiceSize);
                    create.m_sVoiceFile = CleanString(s_szNameString);

                    digimonTable[a] = create;
                }

            }

            return (CharTable, digimonTable);
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

        public static void ExportCharacterCreateTableToXml(string outputFile, CharCreateTable[] charCreates)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(outputFile, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("CharacterList");

                foreach (CharCreateTable digiData in charCreates)
                {
                    writer.WriteStartElement("Tamer");
                    WriteElement(writer, "dwTamerID", digiData.dwTamerID);
                    WriteElement(writer, "m_bShow", digiData.m_bShow);
                    WriteElement(writer, "m_bEnable", digiData.m_bEnable);
                    WriteElement(writer, "m_nSeasonType", digiData.m_nSeasonType);
                    WriteElement(writer, "m_sVoiceSize", digiData.m_sVoiceSize);
                    WriteElement(writer, "m_sVoiceFile", digiData.m_sVoiceFile);
                    WriteElement(writer, "m_nIconIdx", digiData.m_nIconIdx);
                    WriteElement(writer, "CountDC", digiData.CountDC);
                    writer.WriteStartElement("ItemIDs");

                    for (int i = 0; i < digiData.CountDC; i++)
                    {
                        WriteElement(writer, "ItemID", digiData.ItemID[i]);
                    }

                    writer.WriteEndElement();
                    writer.WriteEndElement();

                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            void WriteElement(XmlWriter writer, string elementName, object elementValue)
            {
                writer.WriteElementString(elementName, elementValue.ToString());
            }

            return;
        }
        public static CharCreateTable[] ImportCharCreateTableFromXml(string filePath)
        {
            List<CharCreateTable> charCreateList = new List<CharCreateTable>();

            XDocument doc = XDocument.Load(filePath);
            var charCreateElements = doc.Descendants("Tamer");

            foreach (var charCreateElement in charCreateElements)
            {
                CharCreateTable charCreate = new CharCreateTable();

                charCreate.dwTamerID = int.Parse(charCreateElement.Element("dwTamerID").Value);
                charCreate.m_bShow = byte.Parse(charCreateElement.Element("m_bShow").Value);
                charCreate.m_bEnable = byte.Parse(charCreateElement.Element("m_bEnable").Value);
                charCreate.m_nSeasonType = int.Parse(charCreateElement.Element("m_nSeasonType").Value);
                charCreate.m_sVoiceSize = int.Parse(charCreateElement.Element("m_sVoiceSize").Value);
                charCreate.m_sVoiceFile = charCreateElement.Element("m_sVoiceFile").Value;
                charCreate.m_nIconIdx = int.Parse(charCreateElement.Element("m_nIconIdx").Value);
                charCreate.CountDC = int.Parse(charCreateElement.Element("CountDC").Value);

                var itemElements = charCreateElement.Element("ItemIDs").Elements("ItemID");
                foreach (var itemElement in itemElements)
                {
                    int itemID = int.Parse(itemElement.Value);
                    charCreate.ItemID.Add(itemID);
                }

                charCreateList.Add(charCreate);
            }

            return charCreateList.ToArray();
        }
        public static void ExportCharCreateTableToBinary(string outputFile, CharCreateTable[] charCreateTables, DigimonCreateTable[] digimonCreateTables)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(outputFile, FileMode.Create)))
            {
                writer.Write(charCreateTables.Length);
                foreach (CharCreateTable create in charCreateTables)
                {

                    writer.Write(create.dwTamerID);
                    writer.Write(create.m_bShow);
                    writer.Write(create.m_bEnable);
                    writer.Write(create.m_nSeasonType);
                    writer.Write(create.m_sVoiceSize);
                    if (create.m_sVoiceSize > 0)
                    {
                        byte[] VoiceFile = Encoding.ASCII.GetBytes(create.m_sVoiceFile);
                        writer.Write(VoiceFile);
                    }
                    writer.Write(create.m_nIconIdx);
                    writer.Write(create.CountDC);

                    for (int h = 0; h < create.CountDC; h++)
                    {
                        writer.Write(create.ItemID[h]);
                    }
                }

                writer.Write(digimonCreateTables.Length);
                foreach (DigimonCreateTable create in digimonCreateTables)
                {
                    writer.Write(create.m_digimonID);
                    writer.Write(create.d_bShow);
                    writer.Write(create.d_bEnable);
                    writer.Write(create.m_sVoiceSize);
                    if (create.m_sVoiceSize > 0)
                    {
                        byte[] VoiceFile = Encoding.ASCII.GetBytes(create.m_sVoiceFile);
                        writer.Write(VoiceFile);
                    }

                }

            }
        }

    }

    public class DigimonCreateTable
    {
        public int m_digimonID;
        public byte d_bShow;
        public byte d_bEnable;
        public int m_sVoiceSize;
        public string m_sVoiceFile;
        public static DigimonCreateTable[] ImportDigimonCreateTableFromXml(string filePath)
        {
            List<DigimonCreateTable> digiCreates = new List<DigimonCreateTable>();

            XElement rootElement = XElement.Load(filePath);
            var digimonElements = rootElement.Elements("Digimon");

            foreach (var digimonElement in digimonElements)
            {
                DigimonCreateTable digiData = new DigimonCreateTable();

                digiData.m_digimonID = int.Parse(digimonElement.Element("m_digimonID").Value);
                digiData.d_bShow = byte.Parse(digimonElement.Element("d_bShow").Value);
                digiData.d_bEnable = byte.Parse(digimonElement.Element("d_bEnable").Value);
                digiData.m_sVoiceSize = int.Parse(digimonElement.Element("m_sVoiceSize").Value);
                digiData.m_sVoiceFile = digimonElement.Element("m_sVoiceFile").Value;

                // Add more properties from DigimonCreateTable here

                digiCreates.Add(digiData);
            }

            return digiCreates.ToArray();
        }
        public static void ExportDigimonCreateTableToXml(string outputFile, DigimonCreateTable[] digiCreates)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(outputFile, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("DigimonList");

                foreach (DigimonCreateTable digiData in digiCreates)
                {

                    writer.WriteStartElement("Digimon");
                    WriteElement(writer, "m_digimonID", digiData.m_digimonID);
                    WriteElement(writer, "d_bShow", digiData.d_bShow);
                    WriteElement(writer, "d_bEnable", digiData.d_bEnable);
                    WriteElement(writer, "m_sVoiceSize", digiData.m_sVoiceSize);
                    WriteElement(writer, "m_sVoiceFile", digiData.m_sVoiceFile);
                    writer.WriteEndElement();

                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            void WriteElement(XmlWriter writer, string elementName, object elementValue)
            {
                writer.WriteElementString(elementName, elementValue.ToString());
            }

            return;
        }

    }
}
