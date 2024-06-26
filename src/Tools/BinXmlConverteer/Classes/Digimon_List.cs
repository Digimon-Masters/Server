using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace BinXmlConverter.Classes
{
    public class DigimonData
    {
        public int Species, Model, DigimonRank;
        public int EvolutionType, AttributeType, NatureType;
        public int Family1, Family2, Family3;
        public int Nature1, Nature2, Nature3, BaseNatureType;
        public int BaseLevel, NatureBase1, NatureBase2, NatureBase3;
        public string Name, s_szEvoEffectDir, SoundDirName;
        public string DisplayName, Form, DigimonFamily;
        public short s_nBaseStat_HP, s_nBaseStat_DS, s_nBaseStat_DE, s_nBaseStat_AS, s_nBaseStat_MS, s_nBaseStat_CR, s_nBaseStat_AT, s_nBaseStat_EV, s_nBaseStat_AR, s_nBaseStat_HT, s_nBaseStat_BL, CharSize;
        public int Skill1_ID, Skill2_ID, Skill3_ID, Skill4_ID, Skill5_ID, SkillLevel, SkillLevel1, SkillLevel2, SkillLevel3;
        public float s_fSelectScale, WakkLen, RunLen, aRunLen;
        public byte DigimonType;
        public byte unknow;

        public static void ExportDigimon_ListToXml(string outputFile, DigimonData[] Digimon)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(outputFile, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("DigimonList");

                foreach (DigimonData digiData in Digimon)
                {

                    writer.WriteStartElement("Digimon");
                    WriteElement(writer, "Species", digiData.Species);
                    WriteElement(writer, "Model", digiData.Model);
                    WriteElement(writer, "DisplayName", digiData.DisplayName);
                    WriteElement(writer, "Name", digiData.Name);
                    WriteElement(writer, "s_fSelectScale", digiData.s_fSelectScale);
                    WriteElement(writer, "s_szEvoEffectDir", digiData.s_szEvoEffectDir);
                    WriteElement(writer, "EvolutionType", digiData.EvolutionType);
                    WriteElement(writer, "AttributeType", digiData.AttributeType);
                    WriteElement(writer, "Family1", digiData.Family1);
                    WriteElement(writer, "Family2", digiData.Family2);
                    WriteElement(writer, "Family3", digiData.Family3);
                    WriteElement(writer, "NatureType", digiData.NatureType);
                    WriteElement(writer, "Nature1", digiData.Nature1);
                    WriteElement(writer, "Nature2", digiData.Nature2);
                    WriteElement(writer, "Nature3", digiData.Nature3);
                    WriteElement(writer, "BaseLevel", digiData.BaseLevel);
                    WriteElement(writer, "s_nBaseStat_HP", digiData.s_nBaseStat_HP);
                    WriteElement(writer, "s_nBaseStat_DS", digiData.s_nBaseStat_DS);
                    WriteElement(writer, "s_nBaseStat_DE", digiData.s_nBaseStat_DE);
                    WriteElement(writer, "s_nBaseStat_EV", digiData.s_nBaseStat_EV);
                    WriteElement(writer, "s_nBaseStat_MS", digiData.s_nBaseStat_MS);
                    WriteElement(writer, "s_nBaseStat_CR", digiData.s_nBaseStat_CR);
                    WriteElement(writer, "s_nBaseStat_AT", digiData.s_nBaseStat_AT);
                    WriteElement(writer, "s_nBaseStat_AS", digiData.s_nBaseStat_AS);
                    WriteElement(writer, "s_nBaseStat_AR", digiData.s_nBaseStat_AR);
                    WriteElement(writer, "s_nBaseStat_HT", digiData.s_nBaseStat_HT);
                    WriteElement(writer, "DigimonType", digiData.DigimonType);
                    WriteElement(writer, "CharSize", digiData.CharSize);
                    WriteElement(writer, "Unknow", digiData.unknow);
                    WriteElement(writer, "Skill1_ID", digiData.Skill1_ID);
                    WriteElement(writer, "SkillLevel", digiData.SkillLevel);
                    WriteElement(writer, "Skill2_ID", digiData.Skill2_ID);
                    WriteElement(writer, "SkillLevel1", digiData.SkillLevel1);
                    WriteElement(writer, "Skill3_ID", digiData.Skill3_ID);
                    WriteElement(writer, "Skill4_ID", digiData.Skill4_ID);
                    WriteElement(writer, "SkillLevel3", digiData.SkillLevel3);
                    WriteElement(writer, "WakkLen", digiData.WakkLen);
                    WriteElement(writer, "RunLen", digiData.RunLen);
                    WriteElement(writer, "aRunLen", digiData.aRunLen);
                    WriteElement(writer, "Form", digiData.Form);
                    WriteElement(writer, "DigimonRank", digiData.DigimonRank);
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
        public  static DigimonData[] DigimonToXml(string DigimonInput)
        {
            DigimonData[] DigimonData = new DigimonData[1];

            using (BitReader read = new BitReader(File.OpenRead(DigimonInput)))
            {
                int count = read.ReadInt();

                DigimonData = new DigimonData[count];

                for (int i = 0; i < count; i++)
                {

                    DigimonData digiData = new DigimonData();
                    digiData.Species = read.ReadInt();
                    digiData.Model = read.ReadInt();
                    digiData.DisplayName = read.ReadZString(Encoding.Unicode, 128);
                    digiData.Name = read.ReadZString(Encoding.ASCII, 64);
                    digiData.s_fSelectScale = read.ReadFloat();
                    digiData.s_szEvoEffectDir = read.ReadZString(Encoding.Unicode, 128);
                    digiData.EvolutionType = read.ReadInt();
                    digiData.AttributeType = read.ReadInt();
                    digiData.Family1 = MapearTipo(read.ReadInt());
                    
                    digiData.Family2 = MapearTipo(read.ReadInt());
                    digiData.Family3 = MapearTipo(read.ReadInt());
                    digiData.NatureType = read.ReadInt();
                    digiData.Nature1 = read.ReadInt();
                    digiData.Nature2 = read.ReadInt();
                    digiData.Nature3 = read.ReadInt();
                    digiData.BaseLevel = read.ReadInt();
                    digiData.s_nBaseStat_HP = read.ReadShort();
                    digiData.s_nBaseStat_DS = read.ReadShort();
                    digiData.s_nBaseStat_DE = read.ReadShort();
                    digiData.s_nBaseStat_EV = read.ReadShort();
                    digiData.s_nBaseStat_MS = read.ReadShort();
                    digiData.s_nBaseStat_CR = read.ReadShort();
                    digiData.s_nBaseStat_AT = read.ReadShort();
                    digiData.s_nBaseStat_AS = read.ReadShort();
                    digiData.s_nBaseStat_AR = read.ReadShort();
                    digiData.s_nBaseStat_HT = read.ReadShort();
                    digiData.DigimonType = read.ReadByte();
                    digiData.CharSize = read.ReadShort();
                    digiData.unknow = read.ReadByte();
                    digiData.Skill1_ID = read.ReadInt();
                    digiData.SkillLevel = read.ReadInt();
                    digiData.Skill2_ID = read.ReadInt();
                    digiData.SkillLevel1 = read.ReadInt();
                    digiData.Skill3_ID = read.ReadInt();
                    digiData.SkillLevel2 = read.ReadInt();
                    digiData.Skill4_ID = read.ReadInt();
                    digiData.SkillLevel3 = read.ReadInt();
                    digiData.WakkLen = read.ReadFloat();
                    digiData.RunLen = read.ReadFloat();
                    digiData.aRunLen = read.ReadFloat();
                    digiData.Form = read.ReadZString(Encoding.Unicode, 128);
                    digiData.DigimonRank = read.ReadInt();

                    DigimonData[i] = digiData;
                }

            }

            return DigimonData;
        }
        private static void SetProperty(DigimonData digimonData, string propertyName, string value)
        {
            // Set the value of the property based on the property name
            switch (propertyName)
            {
                case "Species":
                    digimonData.Species = int.Parse(value);
                    break;
                case "Model":
                    digimonData.Model = int.Parse(value);
                    break;
                case "DigimonRank":
                    digimonData.DigimonRank = int.Parse(value);
                    break;
                case "EvolutionType":
                    digimonData.EvolutionType = int.Parse(value);
                    break;
                case "AttributeType":
                    digimonData.AttributeType = int.Parse(value);
                    break;
                case "Unknow":
                    digimonData.unknow = byte.Parse(value);
                    break;
                case "NatureType":
                    digimonData.NatureType = int.Parse(value);
                    break;
                case "Family1":
                    digimonData.Family1 = int.Parse(value);
                    break;
                case "Family2":
                    digimonData.Family2 = int.Parse(value);
                    break;
                case "Family3":
                    digimonData.Family3 = int.Parse(value);
                    break;
                case "Nature1":
                    digimonData.Nature1 = int.Parse(value);
                    break;
                case "Nature2":
                    digimonData.Nature2 = int.Parse(value);
                    break;
                case "Nature3":
                    digimonData.Nature3 = int.Parse(value);
                    break;
                case "BaseNatureType":
                    digimonData.BaseNatureType = int.Parse(value);
                    break;
                case "BaseLevel":
                    digimonData.BaseLevel = int.Parse(value);
                    break;
                case "NatureBase1":
                    digimonData.NatureBase1 = int.Parse(value);
                    break;
                case "NatureBase2":
                    digimonData.NatureBase2 = int.Parse(value);
                    break;
                case "NatureBase3":
                    digimonData.NatureBase3 = int.Parse(value);
                    break;
                case "Name":
                    digimonData.Name = value;
                    break;
                case "s_szEvoEffectDir":
                    digimonData.s_szEvoEffectDir = value;
                    break;
                case "SoundDirName":
                    digimonData.SoundDirName = value;
                    break;
                case "DisplayName":
                    digimonData.DisplayName = value;
                    break;
                case "Form":
                    digimonData.Form = value;
                    break;
                case "DigimonFamily":
                    digimonData.DigimonFamily = value;
                    break;
                case "s_nBaseStat_HP":
                    digimonData.s_nBaseStat_HP = short.Parse(value);
                    break;
                case "s_nBaseStat_DS":
                    digimonData.s_nBaseStat_DS = short.Parse(value);
                    break;
                case "s_nBaseStat_DE":
                    digimonData.s_nBaseStat_DE = short.Parse(value);
                    break;
                case "s_nBaseStat_AS":
                    digimonData.s_nBaseStat_AS = short.Parse(value);
                    break;
                case "s_nBaseStat_MS":
                    digimonData.s_nBaseStat_MS = short.Parse(value);
                    break;
                case "s_nBaseStat_CR":
                    digimonData.s_nBaseStat_CR = short.Parse(value);
                    break;
                case "s_nBaseStat_AT":
                    digimonData.s_nBaseStat_AT = short.Parse(value);
                    break;
                case "s_nBaseStat_EV":
                    digimonData.s_nBaseStat_EV = short.Parse(value);
                    break;
                case "s_nBaseStat_AR":
                    digimonData.s_nBaseStat_AR = short.Parse(value);
                    break;
                case "s_nBaseStat_HT":
                    digimonData.s_nBaseStat_HT = short.Parse(value);
                    break;
                case "s_nBaseStat_BL":
                    digimonData.s_nBaseStat_BL = short.Parse(value);
                    break;
                case "CharSize":
                    digimonData.CharSize = short.Parse(value);
                    break;
                case "Skill1_ID":
                    digimonData.Skill1_ID = int.Parse(value);
                    break;
                case "Skill2_ID":
                    digimonData.Skill2_ID = int.Parse(value);
                    break;
                case "Skill3_ID":
                    digimonData.Skill3_ID = int.Parse(value);
                    break;
                case "Skill4_ID":
                    digimonData.Skill4_ID = int.Parse(value);
                    break;
                case "Skill5_ID":
                    digimonData.Skill5_ID = int.Parse(value);
                    break;
                case "SkillLevel":
                    digimonData.SkillLevel = int.Parse(value);
                    break;
                case "SkillLevel1":
                    digimonData.SkillLevel1 = int.Parse(value);
                    break;
                case "SkillLevel2":
                    digimonData.SkillLevel2 = int.Parse(value);
                    break;
                case "SkillLevel3":
                    digimonData.SkillLevel3 = int.Parse(value);
                    break;
                case "s_fSelectScale":
                    digimonData.s_fSelectScale = float.Parse(value);
                    break;
                case "WakkLen":
                    digimonData.WakkLen = float.Parse(value);
                    break;
                case "RunLen":
                    digimonData.RunLen = float.Parse(value);
                    break;
                case "aRunLen":
                    digimonData.aRunLen = float.Parse(value);
                    break;
                case "DigimonType":
                    digimonData.DigimonType = byte.Parse(value);
                    break;
                default:
                    // Unknown property, you can handle it according to your logic
                    break;
            }
        }
        public static DigimonData[] ImportDigimonDataFromXml(string inputFile)
        {
            List<DigimonData> digimonDataList = new List<DigimonData>();

            using (XmlReader reader = XmlReader.Create(inputFile))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Digimon")
                    {
                        DigimonData digimonData = new DigimonData();

                        // Read the properties of the Digimon from the XML
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                string propertyName = reader.Name;
                                reader.Read(); // Read the value of the property

                                // Set the value of the property in the DigimonData object
                                SetProperty(digimonData, propertyName, reader.Value);
                            }
                            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Digimon")
                            {
                                // Add the Digimon data to the list when we reach the end of the "Digimon" element
                                digimonDataList.Add(digimonData);
                                break;
                            }
                        }
                    }
                }
            }

            return digimonDataList.ToArray();
        }
        public static void ExportToBinary(string outputFile, DigimonData[] items)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(outputFile, FileMode.Create)))
            {
                writer.Write(items.Length);
                foreach (DigimonData digimon in items)
                {
                    writer.Write(digimon.Species);
                   
                    writer.Write(digimon.Model);


                    for (int i = 0; i < 64; i++)
                    {
                        char c = i < digimon.DisplayName.Length ? digimon.DisplayName[i] : '\0';
                        writer.Write((ushort)c);
                    }

                    string soundDirName = digimon.Name;
                    byte[] soundDirNameBytes = Encoding.ASCII.GetBytes(soundDirName);


                    Array.Resize(ref soundDirNameBytes, 64);


                    // Escreva os primeiros 64 bytes da string no arquivo
                    writer.Write(soundDirNameBytes, 0, 64);

                    writer.Write(digimon.s_fSelectScale);

                    for (int i = 0; i < 64; i++)
                    {
                        char c = i < digimon.s_szEvoEffectDir.Length ? digimon.s_szEvoEffectDir[i] : '\0';
                        writer.Write((ushort)c);
                    }

                    writer.Write(digimon.EvolutionType);
                    writer.Write(digimon.AttributeType);
                    writer.Write(digimon.Family1);
                    writer.Write(digimon.Family2);
                    writer.Write(digimon.Family3);
                    writer.Write(digimon.NatureType);
                    writer.Write(digimon.Nature1);
                    writer.Write(digimon.Nature2);
                    writer.Write(digimon.Nature3);
                    writer.Write(digimon.BaseLevel);
                    writer.Write(digimon.s_nBaseStat_HP);
                    writer.Write(digimon.s_nBaseStat_DS);
                    writer.Write(digimon.s_nBaseStat_DE);
                    writer.Write(digimon.s_nBaseStat_EV);
                    writer.Write(digimon.s_nBaseStat_MS);
                    writer.Write(digimon.s_nBaseStat_CR);
                    writer.Write(digimon.s_nBaseStat_AT);
                    writer.Write(digimon.s_nBaseStat_AS);
                    writer.Write(digimon.s_nBaseStat_AR);
                    writer.Write(digimon.s_nBaseStat_HT);
                    writer.Write(digimon.DigimonType);
                    writer.Write(digimon.CharSize);
                    writer.Write(digimon.unknow);
                    writer.Write(digimon.Skill1_ID);
                    writer.Write(digimon.SkillLevel);
                    writer.Write(digimon.Skill2_ID);
                    writer.Write(digimon.SkillLevel1);
                    writer.Write(digimon.Skill3_ID);
                    writer.Write(digimon.SkillLevel2);
                    writer.Write(digimon.Skill4_ID);
                    writer.Write(digimon.SkillLevel3);
                    writer.Write(digimon.WakkLen);
                    writer.Write(digimon.RunLen);
                    writer.Write(digimon.aRunLen);

                    string form = digimon.Form;
                    byte[] formBytes = Encoding.Unicode.GetBytes(form.PadRight(64, '\0'));

                    writer.Write(formBytes, 0, formBytes.Length);

                    writer.Write(digimon.DigimonRank);
                }

            }
        }
        private static int MapearTipo(int tipo)
        {
            switch (tipo)
            {
                case 111:
                    return 1;
                case 101:
                    return 6;
                case 102:
                    return 7;
                case 103:
                    return 8;
                case 104:
                    return 9;
                case 105:
                    return 10;
                case 106:
                    return 11;
                case 107:
                    return 12;
                case 108:
                    return 13;
                case 109:
                    return 14;
                case 110:
                    return 15;
                default:
                    return tipo; // Se o tipo não estiver mapeado, retorna o valor original
            }
        }
    }

}
