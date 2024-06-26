using DSO.BinXmlConverter;
using System.Text;
using System.Xml.Linq;

namespace DSO.BinXmlConverter.Process
{
    public class ModelConverter : IProcessBase
    {
        private string _dataBinFile = "DATA\\Bin\\Model.dat";
        private string _dataXmlFile = "DATA\\Xml\\Model.xml";
        private string _outputBinFile = "Output\\Bin\\Model.dat";

        public void XmlToBin()
        {
            if (!File.Exists(_dataXmlFile))
                return;

            var xDoc = XDocument.Load(_dataXmlFile);
            var models = xDoc.Descendants().Where(x => x.Name.LocalName == "Model").ToList();

            if (File.Exists(_outputBinFile))
            {
                File.Delete(_outputBinFile);

                Thread.Sleep(1000);
            }

            using (var stream = File.Open(_outputBinFile, FileMode.OpenOrCreate))
            {
                using (var writer = new BinaryWriter(stream, Encoding.ASCII, false))
                {
                    writer.Write(models.Count);

                    foreach (var model in models)
                    {
                        writer.Write(Convert.ToInt32(model.Attribute("Id")?.Value));

                        var pathValue = Util.RemoveCdataFromString(model.Element("Path")?.Value);
                        pathValue = pathValue?.Substring(0, pathValue.Length > 78 ? 78 : pathValue.Length);

                        //var pathBytes = Encoding.Unicode.GetBytes(pathValue + char.MinValue);
                        var pathBytes = Encoding.ASCII.GetBytes(pathValue);
                        var pathArray = new byte[160];
                        pathBytes.CopyTo(pathArray, 0);
                        writer.Write(pathArray, 0, pathArray.Length);

                        writer.Write(Util.ToFloat(model.Element("Scale")?.Value));
                        writer.Write(Util.ToFloat(model.Element("Height")?.Value));
                        writer.Write(Util.ToFloat(model.Element("Width")?.Value));

                        var sequenceList = model.Element("SequenceList");
                        var sequenceObjects = sequenceList?.Descendants().Where(x => x.Name.LocalName == "Sequence").ToList();
                        writer.Write(sequenceObjects?.Count ?? 0);

                        var dummyList = model.Element("DummyList");
                        foreach (var dummy in dummyList.Descendants().Where(x => x.Name.LocalName == "Dummy").ToList())
                        {
                            writer.Write(Convert.ToByte(dummy.Value));
                        }

                        if (sequenceObjects != null)
                        {
                            foreach (var sequenceObject in sequenceObjects)
                            {
                                writer.Write(Convert.ToInt32(sequenceObject.Attribute("Id")?.Value));
                                var sequenceEventList = sequenceObject.Element("SequenceEventList");
                                var sequenceEventObjects = sequenceEventList?.Descendants().Where(x => x.Name.LocalName == "SequenceEvent").ToList();
                                writer.Write(sequenceEventObjects?.Count ?? 0);

                                var sequenceLoopList = sequenceObject.Element("SequenceLoopList");
                                var sequenceLoopObjects = sequenceLoopList?.Descendants().Where(x => x.Name.LocalName == "SequenceLoop").ToList();
                                writer.Write(sequenceLoopObjects?.Count ?? 0);

                                var sequenceShaderList = sequenceObject.Element("SequenceShaderList");
                                var sequenceShaderObjects = sequenceShaderList?.Descendants().Where(x => x.Name.LocalName == "SequenceShader").ToList();
                                writer.Write(sequenceShaderObjects?.Count ?? 0);

                                if (sequenceEventObjects != null)
                                {
                                    foreach (var sequenceEventObject in sequenceEventObjects)
                                    {
                                        writer.Write(Util.ToFloat(sequenceEventObject.Element("Time")?.Value));
                                        writer.Write(Convert.ToInt32(sequenceEventObject.Element("Type")?.Value));
                                        writer.Write(Convert.ToInt32(sequenceEventObject.Element("Index")?.Value));

                                        var textValue = Util.RemoveCdataFromString(sequenceEventObject.Element("Text")?.Value);
                                        textValue = textValue?.Substring(0, textValue.Length > 62 ? 62 : textValue.Length);
                                        var textBytes = Encoding.ASCII.GetBytes(textValue);
                                        var textArray = new byte[128];
                                        textBytes.CopyTo(textArray, 0);
                                        writer.Write(textArray, 0, textArray.Length);

                                        writer.Write(Convert.ToInt32(sequenceEventObject.Element("Plag")?.Value));
                                        writer.Write(Util.ToFloat(sequenceEventObject.Element("OffsetX")?.Value));
                                        writer.Write(Util.ToFloat(sequenceEventObject.Element("OffsetY")?.Value));
                                        writer.Write(Util.ToFloat(sequenceEventObject.Element("OffsetZ")?.Value));
                                        writer.Write(Util.ToFloat(sequenceEventObject.Element("EffectScale")?.Value));
                                        writer.Write(Convert.ToByte(sequenceEventObject.Element("ParentScale")?.Value));

                                        var subDummyList = sequenceEventObject.Element("SubDummy");
                                        foreach (var subDummy in subDummyList.Descendants().Where(x => x.Name.LocalName == "SubDummyValue").ToList())
                                        {
                                            writer.Write(Convert.ToByte(subDummy.Value));
                                        }

                                        writer.Write(Util.ToFloat(sequenceEventObject.Element("FadeoutTime")?.Value));
                                        writer.Write(Util.ToFloat(sequenceEventObject.Element("ValueX")?.Value));
                                        writer.Write(Util.ToFloat(sequenceEventObject.Element("ValueY")?.Value));
                                        writer.Write(Util.ToFloat(sequenceEventObject.Element("ValueZ")?.Value));
                                        writer.Write(Util.ToFloat(sequenceEventObject.Element("OtherValueX")?.Value));
                                        writer.Write(Util.ToFloat(sequenceEventObject.Element("OtherValueY")?.Value));
                                        writer.Write(Util.ToFloat(sequenceEventObject.Element("OtherValueZ")?.Value));
                                    }
                                }

                                if (sequenceLoopObjects != null)
                                {
                                    //Verificar
                                }

                                if (sequenceShaderObjects != null)
                                {
                                    foreach (var sequenceShaderObject in sequenceShaderObjects)
                                    {
                                        var objectNameValue = Util.RemoveCdataFromString(sequenceShaderObject.Element("ObjectName")?.Value);
                                        objectNameValue = objectNameValue?.Substring(0, objectNameValue.Length > 14 ? 14 : objectNameValue.Length);
                                        var objectNameBytes = Encoding.ASCII.GetBytes(objectNameValue);
                                        var objectNameArray = new byte[32];
                                        objectNameBytes.CopyTo(objectNameArray, 0);
                                        writer.Write(objectNameArray, 0, objectNameArray.Length);

                                        writer.Write(Convert.ToInt32(sequenceShaderObject.Element("Type")?.Value));
                                        writer.Write(Convert.ToInt32(sequenceShaderObject.Element("Index")?.Value));
                                        writer.Write(Util.ToFloat(sequenceShaderObject.Element("ValueX")?.Value));
                                        writer.Write(Util.ToFloat(sequenceShaderObject.Element("ValueY")?.Value));
                                        writer.Write(Util.ToFloat(sequenceShaderObject.Element("ValueZ")?.Value));

                                        var subDummyList = sequenceShaderObject.Element("SubDummy");
                                        foreach (var subDummy in subDummyList.Descendants().Where(x => x.Name.LocalName == "SubDummyValue").ToList())
                                        {
                                            writer.Write(Convert.ToInt32(subDummy.Value));
                                        }
                                    }
                                }
                            }
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
                    sb.AppendLine("<ModelList>");
                    int count = read.ReadInt();
                    for (int i = 0; i < count; i++)
                    {
                        sb.AppendLine($"<Model Id=\"{read.ReadInt()}\">");
                        sb.AppendLine($"<Path>{Util.AddCdataForUnicodeChars(read.ReadZString(Encoding.ASCII, 160))}</Path>");
                        sb.AppendLine($"<Scale>{read.ReadFloat()}</Scale>");
                        sb.AppendLine($"<Height>{read.ReadFloat()}</Height>");
                        sb.AppendLine($"<Width>{read.ReadFloat()}</Width>");
                        var sequence = read.ReadInt();

                        sb.AppendLine($"<DummyList>");
                        for (int j = 0; j < 16; j++)
                        {
                            sb.AppendLine($"<Dummy>{read.ReadByte()}</Dummy>");
                        }
                        sb.AppendLine($"</DummyList>");

                        if (sequence > 0)
                        {
                            sb.AppendLine($"<SequenceList>");
                            for (int j = 0; j < sequence; j++)
                            {
                                sb.AppendLine($"<Sequence Id=\"{read.ReadInt()}\">");
                                var sequenceEvent = read.ReadInt();
                                var sequenceLoop = read.ReadInt();
                                var sequenceShader = read.ReadInt();

                                if (sequenceEvent > 0)
                                {
                                    sb.AppendLine($"<SequenceEventList>");
                                    for (int k = 0; k < sequenceEvent; k++)
                                    {
                                        sb.AppendLine($"<SequenceEvent>");

                                        sb.AppendLine($"<Time>{read.ReadFloat()}</Time>");
                                        sb.AppendLine($"<Type>{read.ReadInt()}</Type>");
                                        sb.AppendLine($"<Index>{read.ReadInt()}</Index>");
                                        sb.AppendLine($"<Text>{Util.AddCdataForUnicodeChars(read.ReadZString(Encoding.ASCII, 128))}</Text>");
                                        sb.AppendLine($"<Plag>{read.ReadInt()}</Plag>");
                                        sb.AppendLine($"<OffsetX>{read.ReadFloat()}</OffsetX>");
                                        sb.AppendLine($"<OffsetY>{read.ReadFloat()}</OffsetY>");
                                        sb.AppendLine($"<OffsetZ>{read.ReadFloat()}</OffsetZ>");
                                        sb.AppendLine($"<EffectScale>{read.ReadFloat()}</EffectScale>");
                                        sb.AppendLine($"<ParentScale>{read.ReadByte()}</ParentScale>");
                                        sb.AppendLine($"<SubDummy>");
                                        for (int l = 0; l < 3; l++)
                                        {
                                            sb.AppendLine($"<SubDummyValue>{read.ReadByte()}</SubDummyValue>");
                                        }
                                        sb.AppendLine($"</SubDummy>");
                                        sb.AppendLine($"<FadeoutTime>{read.ReadFloat()}</FadeoutTime>");
                                        sb.AppendLine($"<ValueX>{read.ReadFloat()}</ValueX>");
                                        sb.AppendLine($"<ValueY>{read.ReadFloat()}</ValueY>");
                                        sb.AppendLine($"<ValueZ>{read.ReadFloat()}</ValueZ>");
                                        sb.AppendLine($"<OtherValueX>{read.ReadFloat()}</OtherValueX>");
                                        sb.AppendLine($"<OtherValueY>{read.ReadFloat()}</OtherValueY>");
                                        sb.AppendLine($"<OtherValueZ>{read.ReadFloat()}</OtherValueZ>");

                                        sb.AppendLine($"</SequenceEvent>");
                                    }

                                    sb.AppendLine($"</SequenceEventList>");
                                }

                                if (sequenceLoop > 0)
                                {
                                    sb.AppendLine($"<SequenceLoopList>");
                                    for (int k = 0; k < sequenceLoop; k++)
                                    {
                                        sb.AppendLine($"<SequenceLoop>");
                                        sb.AppendLine($"</SequenceLoop>");
                                    }
                                    sb.AppendLine($"</SequenceLoopList>");
                                }

                                if (sequenceShader > 0)
                                {
                                    sb.AppendLine($"<SequenceShaderList>");
                                    for (int k = 0; k < sequenceShader; k++)
                                    {
                                        sb.AppendLine($"<SequenceShader>");
                                        sb.AppendLine($"<ObjectName>{Util.AddCdataForUnicodeChars(read.ReadZString(Encoding.ASCII, 32))}</ObjectName>");
                                        sb.AppendLine($"<Type>{read.ReadInt()}</Type>");
                                        sb.AppendLine($"<Index>{read.ReadInt()}</Index>");
                                        sb.AppendLine($"<ValueX>{read.ReadFloat()}</ValueX>");
                                        sb.AppendLine($"<ValueY>{read.ReadFloat()}</ValueY>");
                                        sb.AppendLine($"<ValueZ>{read.ReadFloat()}</ValueZ>");

                                        sb.AppendLine($"<SubDummy>");
                                        for (int l = 0; l < 4; l++)
                                        {
                                            sb.AppendLine($"<SubDummyValue>{read.ReadInt()}</SubDummyValue>");
                                        }
                                        sb.AppendLine($"</SubDummy>");

                                        sb.AppendLine($"</SequenceShader>");
                                    }
                                    sb.AppendLine($"</SequenceShaderList>");
                                }

                                sb.AppendLine($"</Sequence>");
                            }
                            sb.AppendLine($"</SequenceList>");
                        }
                        sb.AppendLine($"</Model>");
                    }
                    sb.AppendLine("</ModelList>");
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
