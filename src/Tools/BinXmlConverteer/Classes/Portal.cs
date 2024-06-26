using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BinXmlConverter.Classes
{
    public class Portal
    {
        public int pMapGroup;
        public List<PortalInfo> portalInfos = new();


        public static Portal[] ReadPortalFromBinary(string filePath)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                int length = reader.ReadInt32();
                Portal[] portals = new Portal[length];
                for (int i = 0; i < length; i++)
                {
                    Portal portal = new Portal();
                    portal.pMapGroup = reader.ReadInt32();

                    for (int x = 0; x < portal.pMapGroup; x++)
                    {
                        PortalInfo portalInfo = new PortalInfo();
                        portalInfo.s_dwPortalID = reader.ReadInt32();
                        portalInfo.s_dwPortalType = reader.ReadInt32();
                        portalInfo.s_dwSrcMapID = reader.ReadInt32();
                        portalInfo.s_nSrcTargetX = reader.ReadInt32();
                        portalInfo.s_nSrcTargetY = reader.ReadInt32();
                        portalInfo.s_nSrcRadius = reader.ReadInt32();
                        portalInfo.s_dwDestMapID = reader.ReadInt32();
                        portalInfo.s_nDestTargetX = reader.ReadInt32();
                        portalInfo.s_nDestTargetY = reader.ReadInt32();
                        portalInfo.s_nDestRadius = reader.ReadInt32();
                        portalInfo.s_ePortalType = reader.ReadInt32();
                        portalInfo.s_dwUniqObjectID = reader.ReadInt32();
                        portalInfo.s_nPortalKindIndex = reader.ReadInt32();
                        portalInfo.s_nViewTargetX = reader.ReadInt32();
                        portalInfo.s_nViewTargetY = reader.ReadInt32();
                        portal.portalInfos.Add(portalInfo);
                    }



                    portals[i] = portal;
                }

                return portals;
            }
        }

        public static void ExportPortalToXml(Portal[] portalArray, string filePath)
        {
            XElement rootElement = new XElement("Portal");


            foreach (Portal portal in portalArray)
            {
                XElement portalElement = new XElement("Portal",
                    new XElement("pMapGroup", portal.pMapGroup),
                    new XElement("PortalInfos")
                );

                foreach (PortalInfo portalInfo in portal.portalInfos)
                {
                    XElement portalInfoElement = new XElement("PortalInfo",
                        new XElement("s_dwPortalID", portalInfo.s_dwPortalID),
                        new XElement("s_dwPortalType", portalInfo.s_dwPortalType),
                        new XElement("s_dwSrcMapID", portalInfo.s_dwSrcMapID),
                        new XElement("s_nSrcTargetX", portalInfo.s_nSrcTargetX),
                        new XElement("s_nSrcTargetY", portalInfo.s_nSrcTargetY),
                        new XElement("s_nSrcRadius", portalInfo.s_nSrcRadius),
                        new XElement("s_dwDestMapID", portalInfo.s_dwDestMapID),
                        new XElement("s_nDestTargetX", portalInfo.s_nDestTargetX),
                        new XElement("s_nDestTargetY", portalInfo.s_nDestTargetY),
                        new XElement("s_nDestRadius", portalInfo.s_nDestRadius),
                        new XElement("s_ePortalType", portalInfo.s_ePortalType),
                        new XElement("s_dwUniqObjectID", portalInfo.s_dwUniqObjectID),
                        new XElement("s_nPortalKindIndex", portalInfo.s_nPortalKindIndex),
                        new XElement("s_nViewTargetX", portalInfo.s_nViewTargetX),
                        new XElement("s_nViewTargetY", portalInfo.s_nViewTargetY)
                    );

                    portalElement.Element("PortalInfos").Add(portalInfoElement);
                }

                rootElement.Add(portalElement);
            }
            XDocument xmlDoc = new XDocument(rootElement);
            xmlDoc.Save(filePath);
        }
        public static Portal[] ImportPortalFromXml(string filePath)
        {
            XElement importedRootElement = XElement.Load(filePath);

            List<Portal> importedPortalList = new List<Portal>();
            foreach (XElement importedPortalElement in importedRootElement.Elements("Portal"))
            {
                Portal importedPortal = new Portal();
                importedPortal.pMapGroup = int.Parse(importedPortalElement.Element("pMapGroup").Value);
                importedPortal.portalInfos = new List<PortalInfo>();

                XElement importedPortalInfosElement = importedPortalElement.Element("PortalInfos");
                if (importedPortalInfosElement != null)
                {
                    foreach (XElement importedPortalInfoElement in importedPortalInfosElement.Elements("PortalInfo"))
                    {
                        PortalInfo importedPortalInfo = new PortalInfo();
                        importedPortalInfo.s_dwPortalID = int.Parse(importedPortalInfoElement.Element("s_dwPortalID").Value);
                        importedPortalInfo.s_dwPortalType = int.Parse(importedPortalInfoElement.Element("s_dwPortalType").Value);
                        importedPortalInfo.s_dwSrcMapID = int.Parse(importedPortalInfoElement.Element("s_dwSrcMapID").Value);
                        importedPortalInfo.s_nSrcTargetX = int.Parse(importedPortalInfoElement.Element("s_nSrcTargetX").Value);
                        importedPortalInfo.s_nSrcTargetY = int.Parse(importedPortalInfoElement.Element("s_nSrcTargetY").Value);
                        importedPortalInfo.s_nSrcRadius = int.Parse(importedPortalInfoElement.Element("s_nSrcRadius").Value);
                        importedPortalInfo.s_dwDestMapID = int.Parse(importedPortalInfoElement.Element("s_dwDestMapID").Value);
                        importedPortalInfo.s_nDestTargetX = int.Parse(importedPortalInfoElement.Element("s_nDestTargetX").Value);
                        importedPortalInfo.s_nDestTargetY = int.Parse(importedPortalInfoElement.Element("s_nDestTargetY").Value);
                        importedPortalInfo.s_nDestRadius = int.Parse(importedPortalInfoElement.Element("s_nDestRadius").Value);
                        importedPortalInfo.s_ePortalType = int.Parse(importedPortalInfoElement.Element("s_ePortalType").Value);
                        importedPortalInfo.s_dwUniqObjectID = int.Parse(importedPortalInfoElement.Element("s_dwUniqObjectID").Value);
                        importedPortalInfo.s_nPortalKindIndex = int.Parse(importedPortalInfoElement.Element("s_nPortalKindIndex").Value);
                        importedPortalInfo.s_nViewTargetX = int.Parse(importedPortalInfoElement.Element("s_nViewTargetX").Value);
                        importedPortalInfo.s_nViewTargetY = int.Parse(importedPortalInfoElement.Element("s_nViewTargetY").Value);

                        importedPortal.portalInfos.Add(importedPortalInfo);
                    }
                }

                importedPortalList.Add(importedPortal);
            }

            return importedPortalList.ToArray();
        }


        public static void WritePortalToBinary(Portal[] portal, string filePath)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {

                writer.Write(portal.Length);

                foreach (Portal portals in portal)
                {
                    writer.Write(portals.pMapGroup);
                    foreach (PortalInfo portalInfo in portals.portalInfos)
                    {

                        writer.Write(portalInfo.s_dwPortalID);
                        writer.Write(portalInfo.s_dwPortalType);
                        writer.Write(portalInfo.s_dwSrcMapID);
                        writer.Write(portalInfo.s_nSrcTargetX);
                        writer.Write(portalInfo.s_nSrcTargetY);
                        writer.Write(portalInfo.s_nSrcRadius);
                        writer.Write(portalInfo.s_dwDestMapID);
                        writer.Write(portalInfo.s_nDestTargetX);
                        writer.Write(portalInfo.s_nDestTargetY);
                        writer.Write(portalInfo.s_nDestRadius);
                        writer.Write(portalInfo.s_ePortalType);
                        writer.Write(portalInfo.s_dwUniqObjectID);
                        writer.Write(portalInfo.s_nPortalKindIndex);
                        writer.Write(portalInfo.s_nViewTargetX);
                        writer.Write(portalInfo.s_nViewTargetY);
                    }
                }
            }
        }
    }
    public class PortalInfo
    {
        public int s_dwPortalID;
        public int s_dwPortalType;
        public int s_dwSrcMapID;
        public int s_nSrcTargetX;
        public int s_nSrcTargetY;
        public int s_nSrcRadius;
        public int s_dwDestMapID;
        public int s_nDestTargetX;
        public int s_nDestTargetY;
        public int s_nDestRadius;
        public int s_ePortalType;
        public int s_dwUniqObjectID;
        public int s_nPortalKindIndex;
        public int s_nViewTargetX;
        public int s_nViewTargetY;
    }
}
