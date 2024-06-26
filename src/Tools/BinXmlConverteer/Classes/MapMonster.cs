using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BinXmlConverter.Classes
{
    public class MapMonsters
    {
        public int FileID;
        public int nSize;
        public List<MapInformation> MapInformation = new();


        public static MapMonsters[] ReadMapMonstersFromBinary(string FilePath)
        {
            using (BitReader read = new(File.Open(FilePath, FileMode.Open)))
            {
                int count = read.ReadInt();
                MapMonsters[] mapMonsters = new MapMonsters[count];
                string filePath = "KillSpawninfo.txt";
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    for (int i = 0; i < count; i++)
                    {
                        MapMonsters map = new MapMonsters();
                        map.FileID = read.ReadInt();
                        map.nSize = read.ReadInt();
                        for (int j = 0; j < map.nSize; ++j)
                        {
                            MapInformation mapInformation = new MapInformation();

                            mapInformation.Map = read.ReadInt();
                            mapInformation.MapNum = read.ReadInt();

                            for (int k = 0; k < mapInformation.MapNum; k++)
                            {
                                Monsters monsters = new Monsters();
                                monsters.MapID = read.ReadInt();
                                monsters.MonsterID = read.ReadInt();
                                monsters.CenterX = read.ReadInt();
                                monsters.CenterY = read.ReadInt();
                                monsters.Radius = read.ReadInt();
                                monsters.Count = read.ReadInt();
                                monsters.RespawnTime = read.ReadInt();
                                monsters.KillGenMonFTID = read.ReadInt();
                                monsters.KillgenCount = read.ReadInt();
                                monsters.KillgenViewCnt = read.ReadInt();
                                monsters.MoveType = read.ReadInt();

                                if (monsters.KillGenMonFTID > 0)
                                {
                                    writer.WriteLine($" MapID:{mapInformation.Map} MonsterID: {monsters.MonsterID}, KillGenCount: {monsters.KillgenCount}, KillGenMonFTID: {monsters.KillGenMonFTID} KillgenViewCnt: {monsters.KillgenViewCnt}");
                                }
                                monsters.InstRespawn = read.ReadByte();
                                monsters.u10 = read.ReadShort();
                                monsters.u2 = read.ReadByte();
                                mapInformation.Monsters.Add(monsters);
                            }

                            map.MapInformation.Add(mapInformation);
                        }

                        mapMonsters[i] = map;
                    }
                }

                return mapMonsters;
            }

          
        }

        public static void ExportMapMonstersToXml(MapMonsters[] mapMonsters, string filePath)
        {
            XElement rootElement = new XElement("MapMonsters");

            foreach (MapMonsters map in mapMonsters)
            {
                XElement mapElement = new XElement("Map",
                    new XElement("FileID", map.FileID),
                    new XElement("nSize", map.nSize)
                );

                foreach (MapInformation mapInformation in map.MapInformation)
                {
                    XElement mapInformationElement = new XElement("MapInformation",
                        new XElement("Map", mapInformation.Map),
                        new XElement("MapNum", mapInformation.MapNum)
                    );

                    foreach (Monsters monsters in mapInformation.Monsters)
                    {
                        XElement monstersElement = new XElement("Monsters",
                            new XElement("MapID", monsters.MapID),
                            new XElement("MonsterID", monsters.MonsterID),
                            new XElement("CenterX", monsters.CenterX),
                            new XElement("CenterY", monsters.CenterY),
                            new XElement("Radius", monsters.Radius),
                            new XElement("Count", monsters.Count),
                            new XElement("RespawnTime", monsters.RespawnTime),
                            new XElement("KillGenMonFTID", monsters.KillGenMonFTID),
                            new XElement("KillgenCount", monsters.KillgenCount),
                            new XElement("KillgenViewCnt", monsters.KillgenViewCnt),
                            new XElement("MoveType", monsters.MoveType),
                            new XElement("InstRespawn", monsters.InstRespawn),
                            new XElement("u10", monsters.u10),
                            new XElement("u2", monsters.u2)
                        );

                        mapInformationElement.Add(monstersElement);
                    }

                    mapElement.Add(mapInformationElement);
                }

                rootElement.Add(mapElement);
            }

            XDocument xmlDocument = new XDocument(rootElement);
            xmlDocument.Save(filePath);
        }
        public static MapMonsters[] ImportMapMonstersFromXml(string filePath)
        {
            XDocument xmlDocument = XDocument.Load(filePath);
            IEnumerable<XElement> mapElements = xmlDocument.Descendants("Map");

            List<MapMonsters> mapMonstersList = new List<MapMonsters>();

            foreach (XElement mapElement in mapElements)
            {
                MapMonsters mapMonsters = new MapMonsters();

                XElement fileIdElement = mapElement.Element("FileID");
                if (fileIdElement == null || string.IsNullOrEmpty(fileIdElement.Value))
                {
                    continue;
                }

                mapMonsters.FileID = (int)fileIdElement;

                XElement nSizeElement = mapElement.Element("nSize");
                if (nSizeElement == null || string.IsNullOrEmpty(nSizeElement.Value))
                {
                    continue;
                }
                mapMonsters.nSize = (int)nSizeElement;

                IEnumerable<XElement> mapInformationElements = mapElement.Descendants("MapInformation");

                foreach (XElement mapInformationElement in mapInformationElements)
                {
                    MapInformation mapInformation = new MapInformation();

                    XElement maapElement = mapInformationElement.Element("Map");
                    if (maapElement == null || string.IsNullOrEmpty(maapElement.Value))
                    {
                        continue;
                    }
                    mapInformation.Map = (int)maapElement;

                    XElement mapNumElement = mapInformationElement.Element("MapNum");
                    if (mapNumElement == null || string.IsNullOrEmpty(mapNumElement.Value))
                    {
                        continue;
                    }
                    mapInformation.MapNum = (int)mapNumElement;

                    IEnumerable<XElement> monstersElements = mapInformationElement.Descendants("Monsters");

                    foreach (XElement monstersElement in monstersElements)
                    {
                        Monsters monsters = new Monsters();
                        monsters.MapID = (int)monstersElement.Element("MapID");
                        monsters.MonsterID = (int)monstersElement.Element("MonsterID");
                        monsters.CenterX = (int)monstersElement.Element("CenterX");
                        monsters.CenterY = (int)monstersElement.Element("CenterY");
                        monsters.Radius = (int)monstersElement.Element("Radius");
                        monsters.Count = (int)monstersElement.Element("Count");
                        monsters.RespawnTime = (int)monstersElement.Element("RespawnTime");
                        monsters.KillGenMonFTID = (int)monstersElement.Element("KillGenMonFTID");
                        monsters.KillgenCount = (int)monstersElement.Element("KillgenCount");
                        monsters.KillgenViewCnt = (int)monstersElement.Element("KillgenViewCnt");
                        monsters.MoveType = (int)monstersElement.Element("MoveType");
                        monsters.InstRespawn = (byte)(int)monstersElement.Element("InstRespawn");
                        monsters.u10 = (short)monstersElement.Element("u10");
                        monsters.u2 = (byte)(int)monstersElement.Element("u2");

                        mapInformation.Monsters.Add(monsters);
                    }

                    mapMonsters.MapInformation.Add(mapInformation);
                }

                mapMonstersList.Add(mapMonsters);
            }

            return mapMonstersList.ToArray();
        }
        public static void WriteMapMonstersToBinary(MapMonsters[] mapMonsters, string filePath)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {
                writer.Write(mapMonsters.Length); // Escreve o número de objetos MapMonsters

                foreach (MapMonsters map in mapMonsters)
                {
                    writer.Write(map.FileID);
                    writer.Write(map.nSize);

                    foreach (MapInformation mapInformation in map.MapInformation)
                    {
                        writer.Write(mapInformation.Map);
                        writer.Write(mapInformation.MapNum);

                        foreach (Monsters monsters in mapInformation.Monsters)
                        {
                            writer.Write(monsters.MapID);
                            writer.Write(monsters.MonsterID);
                            writer.Write(monsters.CenterX);
                            writer.Write(monsters.CenterY);
                            writer.Write(monsters.Radius);
                            writer.Write(monsters.Count);
                            writer.Write(monsters.RespawnTime);
                            writer.Write(monsters.KillGenMonFTID);
                            writer.Write(monsters.KillgenCount);
                            writer.Write(monsters.KillgenViewCnt);
                            writer.Write(monsters.MoveType);
                            writer.Write(monsters.InstRespawn);
                            writer.Write(monsters.u10);
                            writer.Write(monsters.u2);
                        }
                    }
                }
            }
        }
    }

    public class MapInformation
    {
        public int Map;
        public int MapNum;
        public List<Monsters> Monsters = new();
    }

    public class Monsters
    {
        public int MapID;
        public int MonsterID;
        public int CenterX;
        public int CenterY;
        public int Radius;
        public int Count;
        public int RespawnTime;
        public int KillGenMonFTID;
        public int KillgenCount;
        public int KillgenViewCnt;
        public int MoveType;
        public byte InstRespawn;
        public short u10;
        public byte u2;
    }
}
