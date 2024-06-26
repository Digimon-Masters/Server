using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BinXmlConverter.Classes
{
    public class ExtraExchanges
    {

        public static ExtraExchangeNPC[] ExtraExchangesFromBinary(string filePath)
        {
            using (BitReader read = new BitReader(File.Open(filePath, FileMode.Open)))
            {
                int count = read.ReadInt();

                ExtraExchangeNPC[] ExtraNpc = new ExtraExchangeNPC[count];

                for (int i = 0; i < count; i++)
                {
                    ExtraExchangeNPC npc = new ExtraExchangeNPC();
                    npc.NpcId = read.ReadInt();

                    int cCount = read.ReadInt();
                    for (int g = 0; g < cCount; g++)
                    {
                        ExtraExchangesId id = new();
                        id.Id = read.ReadShort();

                        int cCount2 = read.ReadInt();
                        for (int u = 0; u < cCount2; u++)
                        {
                            ExtraExchange exchange = new();
                            exchange.DigimonID = read.ReadInt();
                            exchange.Unknow = read.ReadShort();
                            exchange.RequiredLevel = read.ReadInt();
                            exchange.Price = read.ReadInt();
                            exchange.Unknow1 = read.ReadShort();
                            exchange.ItemCount = read.ReadInt();

                            for (int a = 0; a < exchange.ItemCount; a++)
                            {
                                MaterialData materialData = new();
                                materialData.ItemId = read.ReadInt();
                                materialData.ItemCount = read.ReadInt();
                                exchange.MaterialDatas.Add(materialData);
                            }
                            exchange.MaterialCount = read.ReadInt();

                            for (int q = 0; q < exchange.MaterialCount; q++)
                            {
                                MaterialData materialData = new();
                                materialData.ItemId = read.ReadInt();
                                materialData.ItemCount = read.ReadInt();
                                exchange.SubMaterials.Add(materialData);
                            }

                            id.extraExchanges.Add(exchange);
                        }
                        npc.extraExchanges.Add(id);
                    }
                    ExtraNpc[i] = npc;
                }

                return ExtraNpc;
            }
        }
        public static void ExportExtraExchangeNPCToXml(ExtraExchangeNPC[] extraExchangeNPCs, string filePath)
        {
            // Cria um elemento raiz XML
            XElement rootElement = new XElement("ExtraExchangeNPCs");

            // Adiciona cada instância de ExtraExchangeNPC como um elemento filho
            foreach (ExtraExchangeNPC extraExchangeNPC in extraExchangeNPCs)
            {
                XElement extraExchangeNPCElement = new XElement("ExtraExchangeNPC",
                    new XElement("NpcId", extraExchangeNPC.NpcId)
                );

                // Adiciona cada instância de ExtraExchangesId como um elemento filho
                foreach (ExtraExchangesId extraExchangesId in extraExchangeNPC.extraExchanges)
                {
                    XElement extraExchangesIdElement = new XElement("ExtraExchangesId",
                        new XElement("Id", extraExchangesId.Id)
                    );

                    // Adiciona cada instância de ExtraExchange como um elemento filho
                    foreach (ExtraExchange extraExchange in extraExchangesId.extraExchanges)
                    {
                        XElement extraExchangeElement = new XElement("ExtraExchange",
                            new XElement("DigimonID", extraExchange.DigimonID),
                            new XElement("Unknow", extraExchange.Unknow),
                            new XElement("RequiredLevel", extraExchange.RequiredLevel),
                            new XElement("Unknow1", extraExchange.Unknow1),
                            new XElement("Price", extraExchange.Price),
                            new XElement("ItemCount", extraExchange.ItemCount),
                            new XElement("MaterialCount", extraExchange.MaterialCount)
                        );

                        // Adiciona cada instância de MaterialData como um elemento filho de MaterialDatas
                        foreach (MaterialData materialData in extraExchange.MaterialDatas)
                        {
                            XElement materialDataElement = new XElement("MaterialData",
                                new XElement("ItemId", materialData.ItemId),
                                new XElement("ItemCount", materialData.ItemCount)
                            );

                            extraExchangeElement.Add(materialDataElement);
                        }

                        // Adiciona cada instância de MaterialData como um elemento filho de SubMaterials
                        foreach (MaterialData subMaterialData in extraExchange.SubMaterials)
                        {
                            XElement subMaterialDataElement = new XElement("SubMaterialData",
                                new XElement("ItemId", subMaterialData.ItemId),
                                new XElement("ItemCount", subMaterialData.ItemCount)
                            );

                            extraExchangeElement.Add(subMaterialDataElement);
                        }

                        extraExchangesIdElement.Add(extraExchangeElement);
                    }

                    extraExchangeNPCElement.Add(extraExchangesIdElement);
                }

                rootElement.Add(extraExchangeNPCElement);
            }

            // Cria o documento XML com o elemento raiz
            XDocument xmlDoc = new XDocument(rootElement);

            // Salva o documento XML em um arquivo
            xmlDoc.Save(filePath);
        }
        public static ExtraExchangeNPC[] ImportExtraExchangeNPCsFromXml(string filePath)
        {
            XDocument xDocument = XDocument.Load(filePath);
            XElement rootElement = xDocument.Root;

            List<ExtraExchangeNPC> extraExchangeNPCList = new List<ExtraExchangeNPC>();

            foreach (XElement extraExchangeNPCElement in rootElement.Elements("ExtraExchangeNPC"))
            {
                ExtraExchangeNPC extraExchangeNPC = new ExtraExchangeNPC();

                extraExchangeNPC.NpcId = int.Parse(extraExchangeNPCElement.Element("NpcId").Value);

                foreach (XElement extraExchangesIdElement in extraExchangeNPCElement.Elements("ExtraExchangesId"))
                {
                    ExtraExchangesId extraExchangesId = new ExtraExchangesId();
                    extraExchangesId.Id = short.Parse(extraExchangesIdElement.Element("Id").Value);

                    foreach (XElement extraExchangeElement in extraExchangesIdElement.Elements("ExtraExchange"))
                    {
                        ExtraExchange extraExchange = new ExtraExchange();
                        extraExchange.DigimonID = int.Parse(extraExchangeElement.Element("DigimonID").Value);
                        extraExchange.Unknow = short.Parse(extraExchangeElement.Element("Unknow").Value);
                        extraExchange.RequiredLevel = int.Parse(extraExchangeElement.Element("RequiredLevel").Value);
                        extraExchange.Unknow1 = short.Parse(extraExchangeElement.Element("Unknow1").Value);
                        extraExchange.Price = int.Parse(extraExchangeElement.Element("Price").Value);
                        extraExchange.ItemCount = int.Parse(extraExchangeElement.Element("ItemCount").Value);
                        extraExchange.MaterialCount = int.Parse(extraExchangeElement.Element("MaterialCount").Value);

                        foreach (XElement materialDataElement in extraExchangeElement.Elements("MaterialData"))
                        {
                            MaterialData materialData = new MaterialData();
                            materialData.ItemId = int.Parse(materialDataElement.Element("ItemId").Value);
                            materialData.ItemCount = int.Parse(materialDataElement.Element("ItemCount").Value);
                            extraExchange.MaterialDatas.Add(materialData);
                        }

                        foreach (XElement subMaterialDataElement in extraExchangeElement.Elements("SubMaterialData"))
                        {
                            MaterialData subMaterialData = new MaterialData();
                            subMaterialData.ItemId = int.Parse(subMaterialDataElement.Element("ItemId").Value);
                            subMaterialData.ItemCount = int.Parse(subMaterialDataElement.Element("ItemCount").Value);
                            extraExchange.SubMaterials.Add(subMaterialData);
                        }

                        extraExchangesId.extraExchanges.Add(extraExchange);
                    }

                    extraExchangeNPC.extraExchanges.Add(extraExchangesId);
                }

                extraExchangeNPCList.Add(extraExchangeNPC);
            }

            return extraExchangeNPCList.ToArray();
        }
        public static void WriteExtraExchangeToBinary(ExtraExchangeNPC[] extra, string filePath)
        {
            using (var writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {
                writer.Write(extra.Length);
                foreach (var npc in extra)
                {
                    writer.Write(npc.NpcId);
                    writer.Write(npc.extraExchanges.Count);
                    foreach (var ExtraId in npc.extraExchanges)
                    {
                        writer.Write(ExtraId.Id);
                        writer.Write(ExtraId.extraExchanges.Count);
                        foreach (var ExtraInfo in ExtraId.extraExchanges)
                        {
                            writer.Write(ExtraInfo.DigimonID);
                            writer.Write(ExtraInfo.Unknow);
                            writer.Write(ExtraInfo.RequiredLevel);
                            writer.Write(ExtraInfo.Price);
                            writer.Write(ExtraInfo.Unknow1);
                            writer.Write(ExtraInfo.ItemCount);

                            foreach (var item in ExtraInfo.MaterialDatas)
                            {
                                writer.Write(item.ItemId);
                                writer.Write(item.ItemCount);
                            }

                            foreach (var item in ExtraInfo.SubMaterials)
                            {
                                writer.Write(item.ItemId);
                                writer.Write(item.ItemCount);
                            }
                        }
                    }
                }
            }
        }
        public class ExtraExchangeNPC
        {
            public int NpcId;
            public List<ExtraExchangesId> extraExchanges = new();
        }
        public class ExtraExchangesId
        {
            public short Id;
            public List<ExtraExchange> extraExchanges = new();
        }
        public class ExtraExchange
        {
            public int DigimonID;
            public short Unknow;
            public int RequiredLevel;
            public short Unknow1;
            public int Price;
            public int ItemCount;
            public int MaterialCount;
            public List<MaterialData> MaterialDatas = new();
            public List<MaterialData> SubMaterials = new();
        }
        public class MaterialData
        {
            public int ItemId;
            public int ItemCount;
        }
    }
}
