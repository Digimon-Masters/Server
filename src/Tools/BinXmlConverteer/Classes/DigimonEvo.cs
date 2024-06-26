using System.Text;
using System.Xml;
using System.Xml.Linq;
using static BinXmlConverter.Classes.DigimonEvo;
using static BinXmlConverter.Classes.MonstersInfo;

namespace BinXmlConverter.Classes
{
    public static class BinaryReaderExtensions
    {

        public static string ReadNullTerminatedAsciiString(this BinaryReader reader, int maxLength)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < maxLength; i++)
            {
                char c = reader.ReadChar();
                if (c == '\0')
                    break;
                sb.Append(c);
            }
            return sb.ToString();
        }

    }

    public class DigimonEvo
    {
        public class Evolution
        {
            public int digiId = 0;
            public int Digivolutions = 0;
            public int CountEvo;
            public int BattleType;
            public List<EvolutionLine> Evolutions = new List<EvolutionLine>();

            public Evolution() { }
        }

        public class EvolutionLine
        {

            public int digiId = 0;
            public ushort iLevel = 0;
            public ushort nType;
            public short uShort1 = 0;
            public int m_IconPos;
            public int m_IconPos2;
            public ushort m_nEnableSlot;               // Se usar ou não
            public ushort m_nOpenQualification;        // qualificação aberta
            public ushort m_nOpenLevel;                // nível aberto
            public ushort m_nOpenQuest;                // abrir a missão
            public ushort m_nOpenItemTypeS;            // item aberto
            public ushort m_nOpenItemNum;              // número de itens abertos
            public ushort m_nUseItem;                  // Itens consumíveis
            public ushort m_nUseItemNum;               // número de itens consumíveis
            public ushort m_nIntimacy;             // Intimidade necessária para a evolução
            public ushort m_nOpenCrest;                // declaração aberta
            public ushort m_EvoCard1;                  // Carta de evolução 1
            public ushort m_EvoCard2;                  // Carta de evolução 2
            public ushort m_EvoCard3;                  // Carta de evolução 3
            public ushort m_nEvoDigimental;            // cápsula de evolução
            public ushort m_nEvoTamerDS;               // domador de consumo ds
            public ushort m_nDummy;                    // use mais tarde se necessário                                           // use mais tarde se necessário
            public int Render;
            public int PosX;
            public int PosY;
            public int m_nStartHegiht;
            public int m_nStartRot;
            public int StartPosX;
            public int StartPosY;
            public int OtherPosX;
            public int OtherPosY;
            public int m_nEndHegiht;
            public int m_nEndRot;
            public int m_nSpeed;
            public int m_dwAni;
            public double m_fStartTime;                // Tempo de reprodução da tela de incubação
            public double m_fEndTime;                  // Tempo de reprodução da tela de incubação
            public int m_nR;
            public int m_nG;
            public int m_nB;
            public string m_szLeve;
            public string m_szEnchant;
            public string m_szSize;
            public int m_nEvolutionTree;
            public int m_nJoGressQuestCheck;                 // Jogress quest check
            public ushort m_nChipsetType;                  // chipset Jogress
            public ushort m_nChipsetTypeC;         // Consumir itens necessários para o chipset Jogress
            public ushort m_nChipsetNum;               // Quantidade do chipset Jograss
            public ushort m_nChipsetTypeP;         // Período de item obrigatório do chipset Jogress
            public ushort m_nJoGressesNum;                                 // Jogress obrigatório mercenários
            public int m_nJoGress_Tacticses1;
            public int m_nJoGress_Tacticses2;
            public int m_nJoGress_Tacticses3;
            public int m_nJoGress_Tacticses4;
            public int unknow;
            public ushort unknow1;
            public List<EvolutionType> EvolutionTypes = new();
            public int slot = 0;
        }

        public class EvolutionType
        {
            public int nSlot;
            public int dwDigimonID;
        }

        public static Evolution[] DigimonEvoToXml(string DigimonInput)
        {
            Evolution[] DigimonData;

            using (BinaryReader read = new BinaryReader(File.OpenRead(DigimonInput)))
            {
                int count = read.ReadInt32();
                DigimonData = new Evolution[count];
                for (int i = 0; i < count; i++)
                {
                    Evolution evo = new Evolution();
                    evo.digiId = read.ReadInt32(); // ->GET ID
                    evo.BattleType = read.ReadInt32();
                    evo.CountEvo = read.ReadInt32();

                    for (int j = 0; j < evo.CountEvo; j++)
                    {
                        EvolutionLine line = new EvolutionLine();
                        line.digiId = read.ReadInt32(); //GET THE DIGI-ID
                        line.iLevel = read.ReadUInt16(); //GET THE DIGI-LEVEL - ROOKIE, ETC
                        line.nType = read.ReadUInt16(); 
                        if (line.digiId == 51058)
                        {

                        }
                        for (int k = 0; k < 9; k++) // -> Change to 9 for ver 128
                        {
                            EvolutionType evolution = new EvolutionType();
                            evolution.nSlot = read.ReadInt32();
                            evolution.dwDigimonID = read.ReadInt32();
                            line.EvolutionTypes.Add(evolution);
                        }

                        line.m_IconPos = read.ReadInt32();
                        line.m_IconPos2 = read.ReadInt32();
                        line.m_nEnableSlot = read.ReadUInt16();
                        line.m_nOpenQualification = read.ReadUInt16();
                        line.m_nOpenLevel = read.ReadUInt16();
                        line.m_nOpenQuest = read.ReadUInt16();
                        line.m_nOpenItemTypeS = read.ReadUInt16();
                        line.m_nOpenItemNum = read.ReadUInt16();
                        line.m_nUseItem = read.ReadUInt16();
                        line.m_nUseItemNum = read.ReadUInt16();
                        line.m_nIntimacy = read.ReadUInt16();
                        line.m_nOpenCrest = read.ReadUInt16();
                        line.m_EvoCard1 = read.ReadUInt16();
                        line.m_EvoCard2 = read.ReadUInt16();
                        line.m_EvoCard3 = read.ReadUInt16();
                        line.m_nEvoDigimental = read.ReadUInt16();
                        line.m_nEvoTamerDS = read.ReadUInt16();

                        line.m_nDummy = read.ReadUInt16();
                        line.Render = read.ReadInt32();
                        line.StartPosX = read.ReadInt32();
                        line.StartPosY = read.ReadInt32();
                        line.m_nStartHegiht = read.ReadInt32();
                        line.m_nStartRot = read.ReadInt32();
                        line.OtherPosX = read.ReadInt32();
                        line.OtherPosY = read.ReadInt32();
                        line.m_nEndHegiht = read.ReadInt32();
                        line.m_nEndRot = read.ReadInt32();
                        line.m_nSpeed = read.ReadInt32();
                        line.m_dwAni = read.ReadInt32();
                        line.unknow = read.ReadInt32();
                        line.m_fStartTime = read.ReadDouble();
                        line.m_fEndTime = read.ReadDouble();
                        line.m_nR = read.ReadInt32();
                        line.m_nG = read.ReadInt32();
                        line.m_nB = read.ReadInt32();
                        byte[] m_szLeveBytes = read.ReadBytes(32);
                        var Levename = System.Text.Encoding.ASCII.GetString(m_szLeveBytes);
                        line.m_szLeve = CleanString(Levename);
                        byte[] m_szEnchantBytes = read.ReadBytes(32);
                        var m_szEnchantname = System.Text.Encoding.ASCII.GetString(m_szEnchantBytes);
                        line.m_szEnchant = CleanString(m_szEnchantname);
                        byte[] m_szSizeBytes = read.ReadBytes(32);
                        var m_szSizename = System.Text.Encoding.ASCII.GetString(m_szSizeBytes);
                        line.m_szSize = CleanString(m_szSizename);
                        line.m_nEvolutionTree = read.ReadInt32();
                        line.m_nJoGressQuestCheck = read.ReadInt32();
                        line.m_nChipsetType = read.ReadUInt16();
                        line.m_nChipsetTypeC = read.ReadUInt16();
                        line.m_nChipsetNum = read.ReadUInt16();
                        line.m_nChipsetTypeP = read.ReadUInt16();
                        line.m_nJoGressesNum = read.ReadUInt16();
                        line.unknow1 = read.ReadUInt16();
                        line.m_nJoGress_Tacticses1 = read.ReadInt32();
                        line.m_nJoGress_Tacticses2 = read.ReadInt32();
                        line.m_nJoGress_Tacticses3 = read.ReadInt32();
                        line.m_nJoGress_Tacticses4 = read.ReadInt32();

                        evo.Evolutions.Add(line);
                    }

                    DigimonData[i] = evo;
                }
            }
            return DigimonData;
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
        public static void ExportDigimonEvolToXml(string outputFile, Evolution[] Digimon)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(outputFile, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("DigimonList");

                foreach (Evolution digiData in Digimon)
                {

                    writer.WriteStartElement("Digimon");

                    WriteElement(writer, "digiId", digiData.digiId);
                    WriteElement(writer, "BattleType", digiData.BattleType);
                    WriteElement(writer, "CountEvo", digiData.CountEvo);
                    foreach (var item in digiData.Evolutions)
                    {
                        writer.WriteStartElement("Evolution");
                        WriteElement(writer, "digiId", item.digiId);
                        WriteElement(writer, "Level", item.iLevel);
                        WriteElement(writer, "nType", item.nType);
                        WriteElement(writer, "uShort1", item.uShort1);

                        for (int k = 0; k < 9; k++) // -> Change to 9 for ver 128
                        {
                            writer.WriteStartElement("EvolutionType");
                            WriteElement(writer, "nSlot", item.EvolutionTypes[k].nSlot);
                            WriteElement(writer, "dwDigimonID", item.EvolutionTypes[k].dwDigimonID);
                            writer.WriteEndElement();
                        }


                        WriteElement(writer, "m_IconPos", item.m_IconPos);
                        WriteElement(writer, "m_IconPos2", item.m_IconPos2);
                        WriteElement(writer, "m_nEnableSlot", item.m_nEnableSlot);
                        WriteElement(writer, "m_nOpenQualification", item.m_nOpenQualification);
                        WriteElement(writer, "m_nOpenLevel", item.m_nOpenLevel);
                        WriteElement(writer, "m_nOpenQuest", item.m_nOpenQuest);
                        WriteElement(writer, "m_nOpenItemTypeS", item.m_nOpenItemTypeS);
                        WriteElement(writer, "m_nOpenItemNum", item.m_nOpenItemNum);
                        WriteElement(writer, "m_nUseItem", item.m_nUseItem);
                        WriteElement(writer, "m_nUseItemNum", item.m_nUseItemNum);
                        WriteElement(writer, "m_nIntimacy", item.m_nIntimacy);
                        WriteElement(writer, "m_nOpenCrest", item.m_nOpenCrest);
                        WriteElement(writer, "m_EvoCard1", item.m_EvoCard1);
                        WriteElement(writer, "m_EvoCard2", item.m_EvoCard2);
                        WriteElement(writer, "m_EvoCard3", item.m_EvoCard3);
                        WriteElement(writer, "m_nEvoDigimental", item.m_nEvoDigimental);
                        WriteElement(writer, "m_nEvoTamerDS", item.m_nEvoTamerDS);
                        WriteElement(writer, "m_nDummy", item.m_nDummy);
                        WriteElement(writer, "Render", item.Render);
                        WriteElement(writer, "StartPosX", item.StartPosX);
                        WriteElement(writer, "StartPosY", item.StartPosY);
                        WriteElement(writer, "m_nStartHegiht", item.m_nStartHegiht);
                        WriteElement(writer, "m_nStartRot", item.m_nStartRot);
                        WriteElement(writer, "OtherPosX", item.OtherPosX);
                        WriteElement(writer, "OtherPosY", item.OtherPosY);
                        WriteElement(writer, "m_nEndHegiht", item.m_nEndHegiht);
                        WriteElement(writer, "m_nEndRot", item.m_nEndRot);
                        WriteElement(writer, "m_nSpeed", item.m_nSpeed);
                        WriteElement(writer, "m_dwAni", item.m_dwAni);
                        WriteElement(writer, "unknow", item.unknow);
                        WriteElement(writer, "m_fStartTime", item.m_fStartTime);
                        WriteElement(writer, "m_fEndTime", item.m_fEndTime);
                        WriteElement(writer, "m_nR", item.m_nR);
                        WriteElement(writer, "m_nG", item.m_nG);
                        WriteElement(writer, "m_nB", item.m_nB);
                        WriteElement(writer, "m_szLeve", item.m_szLeve);
                        WriteElement(writer, "m_szEnchant", item.m_szEnchant);
                        WriteElement(writer, "m_szSize", item.m_szSize);
                        WriteElement(writer, "m_nEvolutionTree", item.m_nEvolutionTree);
                        WriteElement(writer, "m_nJoGressQuestCheck", item.m_nJoGressQuestCheck);
                        WriteElement(writer, "m_nChipsetType", item.m_nChipsetType);
                        WriteElement(writer, "m_nChipsetTypeC", item.m_nChipsetTypeC);
                        WriteElement(writer, "m_nChipsetNum", item.m_nChipsetNum);
                        WriteElement(writer, "m_nChipsetTypeP", item.m_nChipsetTypeP);
                        WriteElement(writer, "m_nJoGressesNum", item.m_nJoGressesNum);
                        WriteElement(writer, "unknow1", item.unknow1);
                        WriteElement(writer, "m_nJoGress_Tacticses1", item.m_nJoGress_Tacticses1);
                        WriteElement(writer, "m_nJoGress_Tacticses2", item.m_nJoGress_Tacticses2);
                        WriteElement(writer, "m_nJoGress_Tacticses3", item.m_nJoGress_Tacticses3);
                        WriteElement(writer, "m_nJoGress_Tacticses4", item.m_nJoGress_Tacticses4);
                        writer.WriteEndElement();
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

            return;
        }
        public static Evolution[] ImportDigimonEvolutionFromXml(string InputFile)
        {
            List<Evolution> digimonDataList = new List<Evolution>();

            XDocument doc = XDocument.Load(InputFile);
            var digimonElements = doc.Descendants("Digimon");

            foreach (var digimonElement in digimonElements)
            {
                Evolution digimonData = new Evolution();

                digimonData.digiId = int.Parse(digimonElement.Element("digiId").Value);
                digimonData.BattleType = int.Parse(digimonElement.Element("BattleType").Value);
                digimonData.CountEvo = int.Parse(digimonElement.Element("CountEvo").Value);
                var evolutionElements = digimonElement.Descendants("Evolution");
                int evolutionIndex = 0;

                foreach (var evolutionElement in evolutionElements)
                {
                    EvolutionLine evolution = new EvolutionLine();
                    evolution.digiId = int.Parse(evolutionElement.Element("digiId").Value);
                    evolution.iLevel = ushort.Parse(evolutionElement.Element("Level").Value);
                    evolution.nType = ushort.Parse(evolutionElement.Element("nType").Value);


                    foreach (var lineElement in evolutionElement.Elements("EvolutionType"))
                    {
                        EvolutionType evolutionType = new EvolutionType();
                        evolutionType.nSlot = int.Parse(lineElement.Element("nSlot").Value);                 
                        evolutionType.dwDigimonID = int.Parse(lineElement.Element("dwDigimonID").Value);
                        evolution.EvolutionTypes.Add(evolutionType);
                    }


                    evolution.m_IconPos = int.Parse(evolutionElement.Element("m_IconPos").Value);
                    evolution.m_IconPos2 = int.Parse(evolutionElement.Element("m_IconPos2").Value);
                    evolution.m_nEnableSlot = ushort.Parse(evolutionElement.Element("m_nEnableSlot").Value);
                    evolution.m_nOpenQualification = ushort.Parse(evolutionElement.Element("m_nOpenQualification").Value);
                    evolution.m_nOpenLevel = ushort.Parse(evolutionElement.Element("m_nOpenLevel").Value);
                    evolution.m_nOpenQuest = ushort.Parse(evolutionElement.Element("m_nOpenQuest").Value);
                    evolution.m_nOpenItemTypeS = ushort.Parse(evolutionElement.Element("m_nOpenItemTypeS").Value);
                    evolution.m_nOpenItemNum = ushort.Parse(evolutionElement.Element("m_nOpenItemNum").Value);
                    evolution.m_nUseItem = ushort.Parse(evolutionElement.Element("m_nUseItem").Value);
                    evolution.m_nUseItemNum = ushort.Parse(evolutionElement.Element("m_nUseItemNum").Value);
                    evolution.m_nIntimacy = ushort.Parse(evolutionElement.Element("m_nIntimacy").Value);
                    evolution.m_nOpenCrest = ushort.Parse(evolutionElement.Element("m_nOpenCrest").Value);
                    evolution.m_EvoCard1 = ushort.Parse(evolutionElement.Element("m_EvoCard1").Value);
                    evolution.m_EvoCard2 = ushort.Parse(evolutionElement.Element("m_EvoCard2").Value);
                    evolution.m_EvoCard3 = ushort.Parse(evolutionElement.Element("m_EvoCard3").Value);
                    evolution.m_nEvoDigimental = ushort.Parse(evolutionElement.Element("m_nEvoDigimental").Value);
                    evolution.m_nEvoTamerDS = ushort.Parse(evolutionElement.Element("m_nEvoTamerDS").Value);
                    evolution.m_nDummy = ushort.Parse(evolutionElement.Element("m_nDummy").Value);
                    evolution.Render = int.Parse(evolutionElement.Element("Render").Value);
                    evolution.StartPosX = int.Parse(evolutionElement.Element("StartPosX").Value);
                    evolution.StartPosY = int.Parse(evolutionElement.Element("StartPosY").Value);
                    evolution.m_nStartHegiht = int.Parse(evolutionElement.Element("m_nStartHegiht").Value);
                    evolution.m_nStartRot = int.Parse(evolutionElement.Element("m_nStartRot").Value);
                    evolution.OtherPosX = int.Parse(evolutionElement.Element("OtherPosX").Value);
                    evolution.OtherPosY = int.Parse(evolutionElement.Element("OtherPosY").Value);
                    evolution.m_nEndHegiht = int.Parse(evolutionElement.Element("m_nEndHegiht").Value);
                    evolution.m_nEndRot = int.Parse(evolutionElement.Element("m_nEndRot").Value);
                    evolution.m_nSpeed = int.Parse(evolutionElement.Element("m_nSpeed").Value);
                    evolution.m_dwAni = int.Parse(evolutionElement.Element("m_dwAni").Value);
                    evolution.unknow = int.Parse(evolutionElement.Element("unknow").Value);
                    evolution.m_fStartTime = double.Parse(evolutionElement.Element("m_fStartTime").Value);
                    evolution.m_fEndTime = double.Parse(evolutionElement.Element("m_fEndTime").Value);
                    evolution.m_nR = int.Parse(evolutionElement.Element("m_nR").Value);
                    evolution.m_nG = int.Parse(evolutionElement.Element("m_nG").Value);
                    evolution.m_nB = int.Parse(evolutionElement.Element("m_nB").Value);
                    evolution.m_szLeve = evolutionElement.Element("m_szLeve").Value;
                    evolution.m_szEnchant = evolutionElement.Element("m_szEnchant").Value;
                    evolution.m_szSize = evolutionElement.Element("m_szSize").Value;
                    evolution.m_nEvolutionTree = int.Parse(evolutionElement.Element("m_nEvolutionTree").Value);
                    evolution.m_nChipsetType = ushort.Parse(evolutionElement.Element("m_nChipsetType").Value);
                    evolution.m_nChipsetTypeC = ushort.Parse(evolutionElement.Element("m_nChipsetTypeC").Value);
                    evolution.m_nChipsetNum = ushort.Parse(evolutionElement.Element("m_nChipsetNum").Value);
                    evolution.m_nChipsetTypeP = ushort.Parse(evolutionElement.Element("m_nChipsetTypeP").Value);
                    evolution.m_nJoGressesNum = ushort.Parse(evolutionElement.Element("m_nJoGressesNum").Value);
                    evolution.unknow1 = ushort.Parse(evolutionElement.Element("unknow1").Value);
                    evolution.m_nJoGress_Tacticses1 = int.Parse(evolutionElement.Element("m_nJoGress_Tacticses1").Value);
                    evolution.m_nJoGress_Tacticses2 = int.Parse(evolutionElement.Element("m_nJoGress_Tacticses2").Value);
                    evolution.m_nJoGress_Tacticses3 = int.Parse(evolutionElement.Element("m_nJoGress_Tacticses3").Value);
                    evolution.m_nJoGress_Tacticses4 = int.Parse(evolutionElement.Element("m_nJoGress_Tacticses4").Value);
                    digimonData.Evolutions.Add(evolution);
                    evolutionIndex++;
                }

                digimonDataList.Add(digimonData);
            }

            return digimonDataList.ToArray();
        }
        public static void ExportToBinary(string outputFile, Evolution[] items)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(outputFile, FileMode.Create)))
            {
                writer.Write(items.Length);
                foreach (Evolution digimon in items)
                {
                    writer.Write(digimon.digiId);
                    writer.Write(digimon.BattleType);
                    writer.Write(digimon.CountEvo);

                    foreach (var item in digimon.Evolutions)
                    {
                        writer.Write(item.digiId);
                        writer.Write(item.iLevel);
                        writer.Write(item.nType);
                        foreach (var evolution in item.EvolutionTypes)
                        {
                            writer.Write(evolution.nSlot);                        
                            writer.Write(evolution.dwDigimonID);
                        }

                        writer.Write(item.m_IconPos);
                        writer.Write(item.m_IconPos2);
                        writer.Write(item.m_nEnableSlot);
                        writer.Write(item.m_nOpenQualification);
                        writer.Write(item.m_nOpenLevel);
                        writer.Write(item.m_nOpenQuest);
                        writer.Write(item.m_nOpenItemTypeS);
                        writer.Write(item.m_nOpenItemNum);
                        writer.Write(item.m_nUseItem);
                        writer.Write(item.m_nUseItemNum);
                        writer.Write(item.m_nIntimacy);
                        writer.Write(item.m_nOpenCrest);
                        writer.Write(item.m_EvoCard1);
                        writer.Write(item.m_EvoCard2);
                        writer.Write(item.m_EvoCard3);
                        writer.Write(item.m_nEvoDigimental);
                        writer.Write(item.m_nEvoTamerDS);
                        writer.Write(item.m_nDummy);
                        writer.Write(item.Render);
                        writer.Write(item.StartPosX);
                        writer.Write(item.StartPosY);
                        writer.Write(item.m_nStartHegiht);
                        writer.Write(item.m_nStartRot);
                        writer.Write(item.OtherPosX);
                        writer.Write(item.OtherPosY);
                        writer.Write(item.m_nEndHegiht);
                        writer.Write(item.m_nEndRot);
                        writer.Write(item.m_nSpeed);
                        writer.Write(item.m_dwAni);
                        writer.Write(item.unknow);
                        writer.Write(item.m_fStartTime);
                        writer.Write(item.m_fEndTime);
                        writer.Write(item.m_nR);
                        writer.Write(item.m_nG);
                        writer.Write(item.m_nB);

                        char[] m_szLeve = item.m_szLeve.PadRight(32, '\0').ToCharArray();

                        // Converte o array de caracteres s_cModel_Effect para bytes no formato UTF-8
                        byte[] m_szLevee = Encoding.ASCII.GetBytes(m_szLeve);
                        writer.Write(m_szLevee, 0, 32);

                        char[] m_szEnchant = item.m_szEnchant.PadRight(32, '\0').ToCharArray();

                        // Converte o array de caracteres s_cModel_Effect para bytes no formato UTF-8
                        byte[] m_szEnchant1 = Encoding.ASCII.GetBytes(m_szEnchant);
                        writer.Write(m_szEnchant1, 0, 32);

                        char[] m_szSize = item.m_szSize.PadRight(32, '\0').ToCharArray();

                        // Converte o array de caracteres s_cModel_Effect para bytes no formato UTF-8
                        byte[] m_szSizee = Encoding.ASCII.GetBytes(m_szSize);
                        writer.Write(m_szSizee, 0, 32);

                        writer.Write(item.m_nEvolutionTree);
                        writer.Write(item.m_nJoGressQuestCheck);
                        writer.Write(item.m_nChipsetType);
                        writer.Write(item.m_nChipsetTypeC);
                        writer.Write(item.m_nChipsetNum);
                        writer.Write(item.m_nChipsetTypeP);
                        writer.Write(item.m_nJoGressesNum);
                        writer.Write(item.unknow1);
                        writer.Write(item.m_nJoGress_Tacticses1);
                        writer.Write(item.m_nJoGress_Tacticses2);
                        writer.Write(item.m_nJoGress_Tacticses3);
                        writer.Write(item.m_nJoGress_Tacticses4);
                    }


                }

            }
        }

    }
}
