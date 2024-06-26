using System.Xml.Linq;

namespace Helper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var currentDir = Directory.GetCurrentDirectory();
            var path = currentDir + "\\File";
            var filePath = Directory.GetFiles(path, "*.xml").First();
            //var file = File.ReadAllText(filePath);

            var xDoc = XDocument.Load(filePath);
            var sealInfoList = xDoc.Root?.Descendants().Where(x => x.Name.LocalName == "SealInfo").ToList();

            if (sealInfoList == null) return;

            foreach (var sealInfo in sealInfoList)
            {
                var sealInfoAttributes = sealInfo.Elements().ToList();

                AT(sealInfo, sealInfoAttributes);
                BL(sealInfo, sealInfoAttributes);
                CT(sealInfo, sealInfoAttributes);
                DE(sealInfo, sealInfoAttributes);
                DS(sealInfo, sealInfoAttributes);
                EV(sealInfo, sealInfoAttributes);
                HP(sealInfo, sealInfoAttributes);
                HT(sealInfo, sealInfoAttributes);
            }

            var teste = xDoc.ToString();
        }

        private static void AT(XElement sealInfo, List<XElement> sealInfoAttributes)
        {
            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "AT"))
                sealInfo.Add(new XElement("AT", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "AT1"))
                sealInfo.Add(new XElement("AT1", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "AT2"))
                sealInfo.Add(new XElement("AT2", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "AT3"))
                sealInfo.Add(new XElement("AT3", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "AT4"))
                sealInfo.Add(new XElement("AT4", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "AT5"))
                sealInfo.Add(new XElement("AT5", "0"));
        }
        
        private static void BL(XElement sealInfo, List<XElement> sealInfoAttributes)
        {
            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "BL"))
                sealInfo.Add(new XElement("BL", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "BL1"))
                sealInfo.Add(new XElement("BL1", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "BL2"))
                sealInfo.Add(new XElement("BL2", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "BL3"))
                sealInfo.Add(new XElement("BL3", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "BL4"))
                sealInfo.Add(new XElement("BL4", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "BL5"))
                sealInfo.Add(new XElement("BL5", "0"));
        }
        
        private static void CT(XElement sealInfo, List<XElement> sealInfoAttributes)
        {
            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "CT"))
                sealInfo.Add(new XElement("CT", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "CT1"))
                sealInfo.Add(new XElement("CT1", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "CT2"))
                sealInfo.Add(new XElement("CT2", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "CT3"))
                sealInfo.Add(new XElement("CT3", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "CT4"))
                sealInfo.Add(new XElement("CT4", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "CT5"))
                sealInfo.Add(new XElement("CT5", "0"));
        }
        
        private static void DE(XElement sealInfo, List<XElement> sealInfoAttributes)
        {
            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "DE"))
                sealInfo.Add(new XElement("DE", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "DE1"))
                sealInfo.Add(new XElement("DE1", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "DE2"))
                sealInfo.Add(new XElement("DE2", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "DE3"))
                sealInfo.Add(new XElement("DE3", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "DE4"))
                sealInfo.Add(new XElement("DE4", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "DE5"))
                sealInfo.Add(new XElement("DE5", "0"));
        }
        
        private static void DS(XElement sealInfo, List<XElement> sealInfoAttributes)
        {
            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "DS"))
                sealInfo.Add(new XElement("DS", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "DS1"))
                sealInfo.Add(new XElement("DS1", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "DS2"))
                sealInfo.Add(new XElement("DS2", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "DS3"))
                sealInfo.Add(new XElement("DS3", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "DS4"))
                sealInfo.Add(new XElement("DS4", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "DS5"))
                sealInfo.Add(new XElement("DS5", "0"));
        }
        
        private static void EV(XElement sealInfo, List<XElement> sealInfoAttributes)
        {
            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "EV"))
                sealInfo.Add(new XElement("EV", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "EV1"))
                sealInfo.Add(new XElement("EV1", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "EV2"))
                sealInfo.Add(new XElement("EV2", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "EV3"))
                sealInfo.Add(new XElement("EV3", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "EV4"))
                sealInfo.Add(new XElement("EV4", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "EV5"))
                sealInfo.Add(new XElement("EV5", "0"));
        }

        private static void HP(XElement sealInfo, List<XElement> sealInfoAttributes)
        {
            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "HP"))
                sealInfo.Add(new XElement("HP", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "HP1"))
                sealInfo.Add(new XElement("HP1", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "HP2"))
                sealInfo.Add(new XElement("HP2", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "HP3"))
                sealInfo.Add(new XElement("HP3", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "HP4"))
                sealInfo.Add(new XElement("HP4", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "HP5"))
                sealInfo.Add(new XElement("HP5", "0"));
        }
        
        private static void HT(XElement sealInfo, List<XElement> sealInfoAttributes)
        {
            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "HT"))
                sealInfo.Add(new XElement("HT", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "HT1"))
                sealInfo.Add(new XElement("HT1", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "HT2"))
                sealInfo.Add(new XElement("HT2", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "HT3"))
                sealInfo.Add(new XElement("HT3", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "HT4"))
                sealInfo.Add(new XElement("HT4", "0"));

            if (!sealInfoAttributes.Any(x => x.Name.LocalName == "HT5"))
                sealInfo.Add(new XElement("HT5", "0"));
        }
    }
}