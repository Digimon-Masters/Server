using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using static BinXmlConverter.Classes.MapNpc;

namespace BinXmlConverter.Classes
{
    public class MapNpc
    {

        public class MapNPCs
        {
            public int MapID;
            public int NpcID;
            public int InitPosX;
            public int InitPosY;
            public float s_fRotation;
        }

        public static MapNPCs[] ReadMapNPCFromBinary(string FilePath)
        {
            using (BinaryReader read = new BinaryReader(File.Open(FilePath, FileMode.Open)))
            {
                var count = read.ReadInt32();
                MapNPCs[] map = new MapNPCs[count];
                for (int i = 0; i < count; i++)
                {
                    MapNPCs npc = new MapNPCs();
                    npc.NpcID = read.ReadInt32();
                    npc.MapID = read.ReadInt32();
                    npc.InitPosX = read.ReadInt32();
                    npc.InitPosY = read.ReadInt32();
                    npc.s_fRotation = read.ReadSingle();

                    map[i] = npc;
                }

                return map;
            }
        }
        public static void ExportMapNPCsToXml(MapNPCs[] mapNPCs, string filePath)
        {
            var mapNPCElements = new List<XElement>();
            foreach (var npc in mapNPCs)
            {
                var element = new XElement("MapNPC",
                    new XElement("MapID", npc.MapID),
                    new XElement("NpcID", npc.NpcID),
                    new XElement("InitPosX", npc.InitPosX),
                    new XElement("InitPosY", npc.InitPosY),
                    new XElement("s_fRotation", npc.s_fRotation)
                );

                mapNPCElements.Add(element);
            }

            var xml = new XDocument(new XElement("MapNPCs", mapNPCElements));
            xml.Save(filePath);
        }
        public static MapNPCs[] ImportMapNPCsFromXml(string filePath)
        {
            var mapNPCs = new List<MapNPCs>();

            var xml = XDocument.Load(filePath);
            var elements = xml.Root.Elements("MapNPC");

            foreach (var element in elements)
            {
                var npc = new MapNPCs
                {
                    MapID = Convert.ToInt32(element.Element("MapID").Value),
                    NpcID = Convert.ToInt32(element.Element("NpcID").Value),
                    InitPosX = Convert.ToInt32(element.Element("InitPosX").Value),
                    InitPosY = Convert.ToInt32(element.Element("InitPosY").Value),
                    s_fRotation = XmlConvert.ToSingle(element.Element("s_fRotation").Value)
                };

                mapNPCs.Add(npc);
            }

            return mapNPCs.ToArray();
        }
        public static void WriteMapNPCToBinary(string FilePath, MapNPCs[] mapNPCs)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(FilePath, FileMode.Create)))
            {
                writer.Write(mapNPCs.Length);
                foreach (var npc in mapNPCs)
                {
                    writer.Write(npc.NpcID);
                    writer.Write(npc.MapID);
                    writer.Write(npc.InitPosX);
                    writer.Write(npc.InitPosY);
                    writer.Write(npc.s_fRotation);

                }
            }
        }
    }
}
