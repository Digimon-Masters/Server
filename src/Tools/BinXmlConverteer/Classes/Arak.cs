using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static BinXmlConverter.Classes.Arak;

namespace BinXmlConverter.Classes
{
    public class Arak
    {


        public class Patch
        {
            public float[] Loc;
            public uint TIMESTAMP;
        }

        public class PatchMove
        {
            public uint ID;
            public uint DISTANCE;
            public byte MovementsCount;
            public List<Patch> Data;
        }

        public class PatchMoveList
        {
            public uint CounterTables;
            public List<PatchMove> PatchMove;
        }


        public static void Main(string filePath)
        {

            using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                PatchMoveList container = new PatchMoveList();

                container.CounterTables = reader.ReadUInt32();
                container.PatchMove = ReadPatchMoveList(reader, (int)container.CounterTables);

                XElement xmlData = GenerateXml(container);
                xmlData.Save("XML\\Arak.xml");
            }
        }

        static List<PatchMove> ReadPatchMoveList(BinaryReader reader,int count)
        {
            int patchMoveCount = count;
            List<PatchMove> patchMoves = new List<PatchMove>(patchMoveCount);

            for (int i = 0; i < patchMoveCount; i++)
            {
                PatchMove patchMove = new PatchMove();
                {
                    patchMove.ID = reader.ReadUInt32();
                    patchMove.DISTANCE = reader.ReadUInt32();
                    patchMove.MovementsCount = reader.ReadByte();
                    patchMove.Data = ReadPatchList(reader, patchMove.MovementsCount);
                }
                patchMoves.Add(patchMove);
            }

            return patchMoves;
        }

        static List<Patch> ReadPatchList(BinaryReader reader,int count)
        {
            int patchCount = count;
            List<Patch> patches = new List<Patch>(patchCount);

            for (int i = 0; i < patchCount; i++)
            {
                Patch patch = new Patch
                {
                    Loc = new float[3]
                };
                for (int j = 0; j < 3; j++)
                {
                    patch.Loc[j] = reader.ReadSingle();
                }
                patch.TIMESTAMP = reader.ReadUInt32();
                patches.Add(patch);
            }

            return patches;
        }

        static XElement GenerateXml(PatchMoveList data)
        {
            XElement xmlRoot = new XElement("Data",
                new XElement("CounterTables", data.CounterTables),
                new XElement("PatchMoves",
                    from patchMove in data.PatchMove
                    select new XElement("PatchMove",
                        new XElement("ID", patchMove.ID),
                        new XElement("DISTANCE", patchMove.DISTANCE),
                        new XElement("MovementsCount", patchMove.MovementsCount),
                        new XElement("Data",
                            from patch in patchMove.Data
                            select new XElement("Patch",
                                new XElement("Loc", string.Join(", ", patch.Loc)),
                                new XElement("TIMESTAMP", patch.TIMESTAMP)
                            )
                        )
                    )
                )
            );

            return xmlRoot;
        }
        static PatchMoveList ImportFromXMl(string filePath)
        {
            XDocument xmlDoc = XDocument.Load(filePath);
            PatchMoveList container = new PatchMoveList();

            XElement rootElement = xmlDoc.Element("Data");
            if (rootElement != null)
            {
                container.CounterTables = uint.Parse(rootElement.Element("CounterTables")?.Value);

                container.PatchMove = (
                    from patchMoveElement in rootElement.Element("PatchMoves").Elements("PatchMove")
                    select new PatchMove
                    {
                        ID = uint.Parse(patchMoveElement.Element("ID")?.Value),
                        DISTANCE = uint.Parse(patchMoveElement.Element("DISTANCE")?.Value),
                        MovementsCount = byte.Parse(patchMoveElement.Element("MovementsCount")?.Value),
                        Data = (
                            from patchElement in patchMoveElement.Element("Data").Elements("Patch")
                            select new Patch
                            {
                                Loc = patchElement.Element("Loc").Value.Split(',').Select(float.Parse).ToArray(),
                                TIMESTAMP = uint.Parse(patchElement.Element("TIMESTAMP")?.Value)
                            }
                        ).ToList()
                    }
                ).ToList();
            }

            return container;
        }
    }

}

