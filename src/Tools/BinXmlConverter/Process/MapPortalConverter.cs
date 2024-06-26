using System.Text;
using System.Xml.Linq;

namespace DSO.BinXmlConverter.Process
{
    public class MapPortalConverter : IProcessBase
    {
        private string _dataBinFile = "DATA\\Bin\\MapPortal.bin";
        private string _dataXmlFile = "DATA\\Xml\\MapPortal.xml";
        private string _outputBinFile = "Output\\Bin\\MapPortal.bin";

        public void XmlToBin()
        {
            if (!File.Exists(_dataXmlFile))
                return;

            var xDoc = XDocument.Load(_dataXmlFile);
            var mapPortals = xDoc.Descendants().Where(x => x.Name.LocalName == "MapPortal").ToList();

            if (File.Exists(_outputBinFile))
            {
                File.Delete(_outputBinFile);

                Thread.Sleep(1000);
            }

            using (var stream = File.Open(_outputBinFile, FileMode.OpenOrCreate))
            {
                using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
                {
                    writer.Write(mapPortals.Count);

                    foreach (var mapPortal in mapPortals)
                    {
                        var portals = mapPortal.Descendants().Where(x => x.Name.LocalName == "Portal").ToList();
                        writer.Write(portals.Count);

                        foreach (var portal in portals)
                        {
                            writer.Write(Convert.ToInt32(portal.Attribute("Id")?.Value));
                            writer.Write(Convert.ToInt32(portal.Element("Type")?.Value));
                            writer.Write(Convert.ToInt32(portal.Element("SourceMapId")?.Value));
                            writer.Write(Convert.ToInt32(portal.Element("SourcePosX")?.Value));
                            writer.Write(Convert.ToInt32(portal.Element("SourcePosY")?.Value));
                            writer.Write(Convert.ToInt32(portal.Element("SourceRadius")?.Value));
                            writer.Write(Convert.ToInt32(portal.Element("DestMapId")?.Value));
                            writer.Write(Convert.ToInt32(portal.Element("DestPosX")?.Value));
                            writer.Write(Convert.ToInt32(portal.Element("DestPosY")?.Value));
                            writer.Write(Convert.ToInt32(portal.Element("DestRadius")?.Value));
                            writer.Write(Convert.ToInt32(portal.Element("eType")?.Value));
                            writer.Write(Convert.ToInt32(portal.Element("UniqueId")?.Value));
                            writer.Write(Convert.ToInt32(portal.Element("Kind")?.Value));
                            writer.Write(Convert.ToInt32(portal.Element("ViewTargetX")?.Value));
                            writer.Write(Convert.ToInt32(portal.Element("ViewTargetY")?.Value));
                        }
                    }
                }
            }

            Thread.Sleep(1000);
            System.Diagnostics.Process.Start("explorer.exe", Directory.GetParent(_outputBinFile).FullName);
        }

        public void BinToXml()
        {
            var sb = new StringBuilder();

            if (!File.Exists(_dataBinFile))
                return;

            using (Stream s = File.OpenRead(_dataBinFile))
            {
                using (BitReader read = new BitReader(s))
                {
                    sb.AppendLine("<MapPortalList>");
                    int count = read.ReadInt();
                    for (int i = 0; i < count; i++)
                    {
                        sb.AppendLine("<MapPortal>");
                        var count2 = read.ReadInt();
                        for (int h = 0; h < count2; h++)
                        {
                            sb.AppendLine($"<Portal Id=\"{read.ReadInt()}\">");
                            sb.AppendLine($"<Type>{read.ReadInt()}</Type>");
                            sb.AppendLine($"<SourceMapId>{read.ReadInt()}</SourceMapId>");
                            sb.AppendLine($"<SourcePosX>{read.ReadInt()}</SourcePosX>");
                            sb.AppendLine($"<SourcePosY>{read.ReadInt()}</SourcePosY>");
                            sb.AppendLine($"<SourceRadius>{read.ReadInt()}</SourceRadius>");
                            sb.AppendLine($"<DestMapId>{read.ReadInt()}</DestMapId>");
                            sb.AppendLine($"<DestPosX>{read.ReadInt()}</DestPosX>");
                            sb.AppendLine($"<DestPosY>{read.ReadInt()}</DestPosY>");
                            sb.AppendLine($"<DestRadius>{read.ReadInt()}</DestRadius>");
                            sb.AppendLine($"<eType>{read.ReadInt()}</eType>");
                            sb.AppendLine($"<UniqueId>{read.ReadInt()}</UniqueId>");
                            sb.AppendLine($"<Kind>{read.ReadInt()}</Kind>");
                            sb.AppendLine($"<ViewTargetX>{read.ReadInt()}</ViewTargetX>");
                            sb.AppendLine($"<ViewTargetY>{read.ReadInt()}</ViewTargetY>");
                            sb.AppendLine("</Portal>");
                        }
                        sb.AppendLine("</MapPortal>");
                    }
                    sb.AppendLine("</MapPortalList>");
                }
            }

            var teste = sb.ToString();

            if (File.Exists(_dataXmlFile))
                File.Delete(_dataXmlFile);

            Thread.Sleep(1000);
            if (!File.Exists(_dataXmlFile))
            {
                using (FileStream fileStr = File.Create(_dataXmlFile))
                { }
            }

            Thread.Sleep(1000);
            File.WriteAllText(_dataXmlFile, teste);

            Thread.Sleep(1000);
            System.Diagnostics.Process.Start("explorer.exe", Directory.GetParent(_dataXmlFile).FullName);
        }
    }
}
