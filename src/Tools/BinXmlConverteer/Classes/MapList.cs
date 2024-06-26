using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static BinXmlConverter.Classes.Mastercard;
using static BinXmlConverter.Classes.MonstersInfo;

namespace BinXmlConverter.Classes
{
    public class MapData
    {
        public int s_dwMapID;
        public string s_cMapName;
        public string s_cMapPath;
        public string s_cBGSound;
        public int s_nWidth;
        public int s_nHeight;
        public string s_szMapDiscript_Eng;
        public string s_szMapDiscript;
        public int s_dwResurrectionMapID;
        public ushort s_nMapRegionID;
        public ushort s_nFatigue_Type;
        public ushort s_nFatigue_DeBuff;
        public ushort s_nFatigue_StartTime;
        public ushort s_nFatigue_AddTime;
        public short s_nFatigue_AddPoint;
        public short s_nCamera_Max_Level;
        public byte s_bXgConsumeType;
        public byte s_bBattleTagUse;





        public static MapData[] ReadMoApDataFromBinary(string filePath)
        {
            using (BitReader read = new BitReader(File.Open(filePath, FileMode.Open)))
            {


                int count = read.ReadInt();
                MapData[] maap = new MapData[count];
                for (int i = 0; i < count; i++)
                {
                    MapData map = new MapData();
                    map.s_dwMapID = read.ReadInt();
                    var Msize = read.ReadInt();
                    byte[] mname_get = new byte[Msize];
                    for (int g = 0; g < Msize; g++)
                    {
                        mname_get[g] = read.ReadByte();
                    }
                    map.s_cMapName = Encoding.ASCII.GetString(mname_get).Trim();
                    int MapLocSize = read.ReadInt();
                    byte[] mloc_get = new byte[MapLocSize];
                    for (int f = 0; f < MapLocSize; f++)
                    {
                        mloc_get[f] = read.ReadByte();
                    }
                    map.s_cMapPath = Encoding.ASCII.GetString(mloc_get).Trim();
                    int MapSoundSize = read.ReadInt();

                    byte[] msound_get = new byte[MapSoundSize];
                    for (int z = 0; z < MapSoundSize; z++)
                    {
                        msound_get[z] = read.ReadByte();
                    }
                    map.s_cBGSound = Encoding.ASCII.GetString(msound_get).Trim();

                    map.s_nWidth = read.ReadInt();
                    map.s_nHeight = read.ReadInt();
                    int dnamesize = read.ReadInt();
                    byte[] nameBytes = read.ReadBytes(dnamesize * 2);

                    var Name = System.Text.Encoding.Unicode.GetString(nameBytes);

                    map.s_szMapDiscript_Eng = Name;

                    int korgnamesize = read.ReadInt();
                    byte[] nameeBytes = read.ReadBytes(korgnamesize * 2);

                    var Namee = System.Text.Encoding.Unicode.GetString(nameeBytes);
                    map.s_szMapDiscript = Namee;


                    map.s_dwResurrectionMapID = read.ReadInt();
                    map.s_nMapRegionID = read.ReadUShort();
                    map.s_nFatigue_Type = read.ReadUShort();
                    map.s_nFatigue_DeBuff = read.ReadUShort();
                    map.s_nFatigue_StartTime = read.ReadUShort();
                    map.s_nFatigue_AddTime = read.ReadUShort();
                    map.s_nFatigue_AddPoint = read.ReadShort();
                    map.s_nCamera_Max_Level = read.ReadShort();
                    map.s_bXgConsumeType = read.ReadByte();
                    map.s_bBattleTagUse = read.ReadByte();
                    maap[i] = map;
                }

                return maap;
            }
        }
        public static void ExportMapListToXml(MapData[] mapDataArray, string filePath)
        {
            // Cria um elemento raiz XML
            XElement rootElement = new XElement("MapData");

            // Adiciona cada instância de MapData como um elemento filho
            foreach (MapData mapData in mapDataArray)
            {
                XElement mapDataElement = new XElement("Map",
                    new XElement("MapID", mapData.s_dwMapID),
                    new XElement("MapName", mapData.s_cMapName),
                    new XElement("MapPath", mapData.s_cMapPath),
                    new XElement("BGSound", mapData.s_cBGSound),
                    new XElement("Width", mapData.s_nWidth),
                    new XElement("Height", mapData.s_nHeight),
                    new XElement("MapDescription_Eng", mapData.s_szMapDiscript_Eng),
                    new XElement("MapDescription", mapData.s_szMapDiscript),
                    new XElement("ResurrectionMapID", mapData.s_dwResurrectionMapID),
                    new XElement("MapRegionID", mapData.s_nMapRegionID),
                    new XElement("FatigueType", mapData.s_nFatigue_Type),
                    new XElement("FatigueDeBuff", mapData.s_nFatigue_DeBuff),
                    new XElement("FatigueStartTime", mapData.s_nFatigue_StartTime),
                    new XElement("FatigueAddTime", mapData.s_nFatigue_AddTime),
                    new XElement("FatigueAddPoint", mapData.s_nFatigue_AddPoint),
                    new XElement("CameraMaxLevel", mapData.s_nCamera_Max_Level),
                    new XElement("XgConsumeType", mapData.s_bXgConsumeType),
                    new XElement("BattleTagUse", mapData.s_bBattleTagUse)
                );

                rootElement.Add(mapDataElement);
            }

            // Cria o documento XML com o elemento raiz
            XDocument xmlDoc = new XDocument(rootElement);

            // Salva o documento XML em um arquivo
            xmlDoc.Save(filePath);
        }
        public static MapData[] ImportMapListFromXml(string filePath)
        {
            XDocument xDocument = XDocument.Load(filePath);
            XElement rootElement = xDocument.Root;

            List<MapData> mapDataList = new List<MapData>();

            foreach (XElement mapElement in rootElement.Elements("Map"))
            {
                MapData mapData = new MapData();

                mapData.s_dwMapID = int.Parse(mapElement.Element("MapID").Value);
                mapData.s_cMapName = mapElement.Element("MapName").Value;
                mapData.s_cMapPath = mapElement.Element("MapPath").Value;
                mapData.s_cBGSound = mapElement.Element("BGSound").Value;
                mapData.s_nWidth = int.Parse(mapElement.Element("Width").Value);
                mapData.s_nHeight = int.Parse(mapElement.Element("Height").Value);
                mapData.s_szMapDiscript_Eng = mapElement.Element("MapDescription_Eng").Value;
                mapData.s_szMapDiscript = mapElement.Element("MapDescription").Value;
                mapData.s_dwResurrectionMapID = int.Parse(mapElement.Element("ResurrectionMapID").Value);
                mapData.s_nMapRegionID = ushort.Parse(mapElement.Element("MapRegionID").Value);
                mapData.s_nFatigue_Type = ushort.Parse(mapElement.Element("FatigueType").Value);
                mapData.s_nFatigue_DeBuff = ushort.Parse(mapElement.Element("FatigueDeBuff").Value);
                mapData.s_nFatigue_StartTime = ushort.Parse(mapElement.Element("FatigueStartTime").Value);
                mapData.s_nFatigue_AddTime = ushort.Parse(mapElement.Element("FatigueAddTime").Value);
                mapData.s_nFatigue_AddPoint = short.Parse(mapElement.Element("FatigueAddPoint").Value);
                mapData.s_nCamera_Max_Level = short.Parse(mapElement.Element("CameraMaxLevel").Value);
                mapData.s_bXgConsumeType = byte.Parse(mapElement.Element("XgConsumeType").Value);
                mapData.s_bBattleTagUse = byte.Parse(mapElement.Element("BattleTagUse").Value);

                mapDataList.Add(mapData);
            }

            return mapDataList.ToArray();
        }

        public static void WriteMapListToBinary(MapData[] mapDatas, string filePath)
        {
            using (var writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {
                writer.Write(mapDatas.Length);
                foreach (var map in mapDatas)
                {
                    writer.Write(map.s_dwMapID);
                    writer.Write(map.s_cMapName.Length);
                    byte[] s_cMapNameBytes = Encoding.ASCII.GetBytes(map.s_cMapName);
                    writer.Write(s_cMapNameBytes);
                    writer.Write(map.s_cMapPath.Length);
                    byte[] s_cMapPathBytes = Encoding.ASCII.GetBytes(map.s_cMapPath);
                    writer.Write(s_cMapPathBytes);
                    writer.Write(map.s_cBGSound.Length);
                    byte[] s_cBGSoundBytes = Encoding.ASCII.GetBytes(map.s_cBGSound);
                    writer.Write(s_cBGSoundBytes);       
                    writer.Write(map.s_nWidth);
                    writer.Write(map.s_nHeight);         
                    writer.Write(map.s_szMapDiscript_Eng.Length); 
                    byte[] s_szMapDiscript_EngBytes = Encoding.Unicode.GetBytes(map.s_szMapDiscript_Eng);
                    writer.Write(s_szMapDiscript_EngBytes);
                    writer.Write(map.s_szMapDiscript.Length);
                    byte[] s_szMapDiscriptBytes = Encoding.Unicode.GetBytes(map.s_szMapDiscript);   
                    writer.Write(s_szMapDiscriptBytes);
                    writer.Write(map.s_dwResurrectionMapID);
                    writer.Write(map.s_nMapRegionID);
                    writer.Write(map.s_nFatigue_Type);
                    writer.Write(map.s_nFatigue_DeBuff);
                    writer.Write(map.s_nFatigue_StartTime);
                    writer.Write(map.s_nFatigue_AddTime);
                    writer.Write(map.s_nFatigue_AddPoint);
                    writer.Write(map.s_nCamera_Max_Level);
                    writer.Write(map.s_bXgConsumeType);
                    writer.Write(map.s_bBattleTagUse);
                }
            }
        }
    }
}
