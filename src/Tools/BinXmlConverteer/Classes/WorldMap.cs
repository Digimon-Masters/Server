using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static BinXmlConverter.Classes.TalkGlobal;
using static BinXmlConverter.Classes.WorldMap;

namespace BinXmlConverter.Classes
{
    public class WorldMap
    {
        public static (WorldMapInfo[], AreaMapInfo[]) ReadWorldMapFromBinary(string filePath)
        {
            using (BitReader read = new BitReader(File.Open(filePath, FileMode.Open)))
            {
                int count = read.ReadInt();
                WorldMapInfo[] worldMap = new WorldMapInfo[count];

                for (int i = 0; i < count; i++)
                {
                    WorldMapInfo mapInfo = new WorldMapInfo();
                    mapInfo.s_nID = read.ReadUShort();
                    mapInfo.s_szName = read.ReadZString(Encoding.Unicode, 48 * 2);
                    mapInfo.s_szComment = read.ReadZString(Encoding.Unicode, 256 * 2);
                    mapInfo.s_nWorldType = read.ReadUShort();
                    mapInfo.s_nUI_X = read.ReadUShort();
                    mapInfo.s_nUI_Y = read.ReadUShort();

                    worldMap[i] = mapInfo;
                }

                int count2 = read.ReadInt();
                AreaMapInfo[] areaMapInfos = new AreaMapInfo[count2];
                for (int j = 0; j < count2; j++)
                {
                    AreaMapInfo areamapInfo = new AreaMapInfo();
                    areamapInfo.d_nMapID = read.ReadUShort();
                    areamapInfo.d_szName = read.ReadZString(Encoding.Unicode, 48 * 2);
                    areamapInfo.d_szComment = read.ReadZString(Encoding.Unicode, 256 * 2);
                    areamapInfo.d_nAreaType = read.ReadByte();
                    areamapInfo.d_nFieldType = read.ReadByte();
                    areamapInfo.d_nFTDetail = read.ReadInt();
                    areamapInfo.d_nUI_X = read.ReadUShort();
                    areamapInfo.d_nUI_Y = read.ReadUShort();
                    areamapInfo.d_fGaussianBlur = new float[3];

                    areamapInfo.d_fGaussianBlur[0] = read.ReadSingle();
                    areamapInfo.d_fGaussianBlur[1] = read.ReadSingle();
                    areamapInfo.d_fGaussianBlur[2] = read.ReadSingle();

                    areaMapInfos[j] = areamapInfo;
                }

                return (worldMap, areaMapInfos);
            }
        }
        public static void ExportWorldMapInfoToXml(WorldMapInfo[] worldMapInfoArray, string filePath)
        {
            XElement rootElement = new XElement("WorldMapInfo");

            foreach (WorldMapInfo worldMapInfo in worldMapInfoArray)
            {
                XElement worldMapInfoElement = new XElement("WorldMapInfo",
                    new XElement("s_nID", worldMapInfo.s_nID),
                    new XElement("s_szName", worldMapInfo.s_szName),
                    new XElement("s_szComment", worldMapInfo.s_szComment),
                    new XElement("s_nWorldType", worldMapInfo.s_nWorldType),
                    new XElement("s_nUI_X", worldMapInfo.s_nUI_X),
                    new XElement("s_nUI_Y", worldMapInfo.s_nUI_Y)
                );

                rootElement.Add(worldMapInfoElement);
            }

            XDocument xmlDoc = new XDocument(rootElement);
            xmlDoc.Save(filePath);
        }
        public static void ExportAreaMapInfoToXml(AreaMapInfo[] areaMapInfoArray, string filePath)
        {
            XElement rootElement = new XElement("AreaMapInfo");

            foreach (AreaMapInfo areaMapInfo in areaMapInfoArray)
            {
                XElement areaMapInfoElement = new XElement("AreaMapInfo",
                    new XElement("d_nMapID", areaMapInfo.d_nMapID),
                    new XElement("d_szName", areaMapInfo.d_szName),
                    new XElement("d_szComment", areaMapInfo.d_szComment),
                    new XElement("d_nAreaType", areaMapInfo.d_nAreaType),
                    new XElement("d_nFieldType", areaMapInfo.d_nFieldType),
                    new XElement("d_nFTDetail", areaMapInfo.d_nFTDetail),
                    new XElement("d_nUI_X", areaMapInfo.d_nUI_X),
                    new XElement("d_nUI_Y", areaMapInfo.d_nUI_Y),
                    new XElement("d_fGaussianBlur", areaMapInfo.d_fGaussianBlur.Select(b => new XElement("item", b)))
                );

                rootElement.Add(areaMapInfoElement);
            }

            XDocument xmlDoc = new XDocument(rootElement);
            xmlDoc.Save(filePath);
        }
        public static WorldMapInfo[] ImportWorldMapInfoFromXml(string filePath)
        {
            XDocument xmlDoc = XDocument.Load(filePath);
            XElement rootElement = xmlDoc.Element("WorldMapInfo");

            List<WorldMapInfo> worldMapInfoList = new List<WorldMapInfo>();

            if (rootElement != null)
            {
                foreach (XElement worldMapInfoElement in rootElement.Elements("WorldMapInfo"))
                {
                    WorldMapInfo worldMapInfo = new WorldMapInfo();
                    worldMapInfo.s_nID = ushort.Parse(worldMapInfoElement.Element("s_nID").Value);
                    worldMapInfo.s_szName = worldMapInfoElement.Element("s_szName").Value;
                    worldMapInfo.s_szComment = worldMapInfoElement.Element("s_szComment").Value;
                    worldMapInfo.s_nWorldType = ushort.Parse(worldMapInfoElement.Element("s_nWorldType").Value);
                    worldMapInfo.s_nUI_X = ushort.Parse(worldMapInfoElement.Element("s_nUI_X").Value);
                    worldMapInfo.s_nUI_Y = ushort.Parse(worldMapInfoElement.Element("s_nUI_Y").Value);

                    worldMapInfoList.Add(worldMapInfo);
                }
            }

            return worldMapInfoList.ToArray();
        }
        public static AreaMapInfo[] ImportAreaMapInfoFromXml(string filePath)
        {
            XDocument xmlDoc = XDocument.Load(filePath);
            XElement rootElement = xmlDoc.Element("AreaMapInfo");

            List<AreaMapInfo> areaMapInfoList = new List<AreaMapInfo>();

            if (rootElement != null)
            {
                foreach (XElement areaMapInfoElement in rootElement.Elements("AreaMapInfo"))
                {
                    AreaMapInfo areaMapInfo = new AreaMapInfo();
                    areaMapInfo.d_nMapID = ushort.Parse(areaMapInfoElement.Element("d_nMapID").Value);
                    areaMapInfo.d_szName = areaMapInfoElement.Element("d_szName").Value;
                    areaMapInfo.d_szComment = areaMapInfoElement.Element("d_szComment").Value;
                    areaMapInfo.d_nAreaType = byte.Parse(areaMapInfoElement.Element("d_nAreaType").Value);
                    areaMapInfo.d_nFieldType = byte.Parse(areaMapInfoElement.Element("d_nFieldType").Value);
                    areaMapInfo.d_nFTDetail = int.Parse(areaMapInfoElement.Element("d_nFTDetail").Value);
                    areaMapInfo.d_nUI_X = ushort.Parse(areaMapInfoElement.Element("d_nUI_X").Value);
                    areaMapInfo.d_nUI_Y = ushort.Parse(areaMapInfoElement.Element("d_nUI_Y").Value);
                    areaMapInfo.d_fGaussianBlur = areaMapInfoElement.Element("d_fGaussianBlur").Elements("item")
                        .Select(item => Single.Parse(item.Value)).ToArray();

                    areaMapInfoList.Add(areaMapInfo);
                }
            }

            return areaMapInfoList.ToArray();
        }
        public static void WriteAreaMapInfoToBinary(WorldMapInfo[] worldMapInfoArray, AreaMapInfo[] areaMapInfoArray, string filePath)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {
                writer.Write(worldMapInfoArray.Length);
                foreach (WorldMapInfo worldMapInfo in worldMapInfoArray)
                {
                    writer.Write(worldMapInfo.s_nID);

                    for (int i = 0; i < 48; i++)
                    {
                        char c = i < worldMapInfo.s_szName.Length ? worldMapInfo.s_szName[i] : '\0';
                        writer.Write((ushort)c);
                    }

                    for (int i = 0; i < 256; i++)
                    {
                        char c = i < worldMapInfo.s_szComment.Length ? worldMapInfo.s_szComment[i] : '\0';
                        writer.Write((ushort)c);
                    }

                    writer.Write(worldMapInfo.s_nWorldType);
                    writer.Write(worldMapInfo.s_nUI_X);
                    writer.Write(worldMapInfo.s_nUI_Y);
                }

                writer.Write(areaMapInfoArray.Length);
                foreach (AreaMapInfo areaMapInfo in areaMapInfoArray)
                {
                    writer.Write(areaMapInfo.d_nMapID);
                    for (int i = 0; i < 256; i++)
                    {
                        char c = i < areaMapInfo.d_szName.Length ? areaMapInfo.d_szName[i] : '\0';
                        writer.Write((ushort)c);
                    }


                    for (int i = 0; i < 48; i++)
                    {
                        char c = i < areaMapInfo.d_szComment.Length ? areaMapInfo.d_szComment[i] : '\0';
                        writer.Write((ushort)c);
                    }

                    writer.Write(areaMapInfo.d_nAreaType);
                    writer.Write(areaMapInfo.d_nFieldType);
                    writer.Write(areaMapInfo.d_nFTDetail);
                    writer.Write(areaMapInfo.d_nUI_X);
                    writer.Write(areaMapInfo.d_nUI_Y);
                    writer.Write(areaMapInfo.d_fGaussianBlur[0]);
                    writer.Write(areaMapInfo.d_fGaussianBlur[1]);
                    writer.Write(areaMapInfo.d_fGaussianBlur[2]);
                }
            }
        }

        public class WorldMapInfo
        {

            public ushort s_nID;
            public string s_szName;
            public string s_szComment;
            public ushort s_nWorldType;
            public ushort s_nUI_X;
            public ushort s_nUI_Y;
        }

        public class AreaMapInfo
        {

            public ushort d_nMapID;
            public string d_szName;
            public string d_szComment;
            public byte d_nAreaType;
            public byte d_nFieldType;
            public int d_nFTDetail;
            public ushort d_nUI_X;
            public ushort d_nUI_Y;
            public float[]  d_fGaussianBlur;
        }

    }
}
