using System.Text;
using System.Xml.Linq;

namespace BinXmlConverter.Classes
{
    public class ModelInfo
    {
        public int s_dwID;
        public string s_cKfmPath;
        public float s_fScale;
        public float s_fHeight;
        public float s_fWidth;
        public int s_nSequenceCount;
        public byte[] s_Dummy;
        public List<SequenceInfo> SequenceList;
        public static ModelInfo[] ModelToXml(string DigimonInput)
        {
            using (BitReader reader = new BitReader(File.Open(DigimonInput, FileMode.Open)))
            {
                int count = reader.ReadInt();
                ModelInfo[] modelInfos = new ModelInfo[count];

                for (int i = 0; i < count; i++)
                {
                    ModelInfo modelInfo = new ModelInfo();

                    modelInfo.s_dwID = reader.ReadInt();

                    modelInfo.s_cKfmPath = reader.ReadZString(Encoding.ASCII, 160);

                    modelInfo.s_fScale = reader.ReadFloat();
                    modelInfo.s_fHeight = reader.ReadFloat();
                    modelInfo.s_fWidth = reader.ReadFloat();
                    modelInfo.s_nSequenceCount = reader.ReadInt();

                    modelInfo.s_Dummy = new byte[16];
                    for (int x = 0; x < 16; x++)
                    {
                        modelInfo.s_Dummy[x] = reader.ReadByte();
                    }

                    modelInfo.SequenceList = new List<SequenceInfo>();

                    for (int j = 0; j < modelInfo.s_nSequenceCount; j++)
                    {
                        SequenceInfo sequenceInfo = new SequenceInfo();
                        sequenceInfo.s_dwSequenceID = reader.ReadInt();
                        sequenceInfo.s_nEventCount = reader.ReadInt();
                        sequenceInfo.s_nLoopCnt = reader.ReadInt();
                        sequenceInfo.s_nShaderCnt = reader.ReadInt();

                        sequenceInfo.EventList = new List<EventInfo>();

                        for (int k = 0; k < sequenceInfo.s_nEventCount; k++)
                        {
                            EventInfo eventInfo = new EventInfo();
                            eventInfo.s_fEventTime = reader.ReadFloat();
                            eventInfo.s_nType = reader.ReadInt();
                            eventInfo.s_nStaticIndex = reader.ReadInt();


                            eventInfo.s_cText = reader.ReadZString(Encoding.ASCII, 128);

                            eventInfo.s_dwPlag = reader.ReadInt();
                            eventInfo.s_vOffsetx = reader.ReadFloat();
                            eventInfo.s_vOffsety = reader.ReadFloat();
                            eventInfo.s_vOffsetz = reader.ReadFloat();
                            eventInfo.s_fEffectScale = reader.ReadFloat();
                            eventInfo.s_bParentScale = reader.ReadByte();

                            eventInfo.unk = new byte[3];

                            for (int x = 0; x < 3; x++)
                            {
                                eventInfo.unk[x] = reader.ReadByte();
                            }

                            eventInfo.s_fFadeoutTime = reader.ReadFloat();
                            eventInfo.s_vValuex = reader.ReadFloat();
                            eventInfo.s_vValuey = reader.ReadFloat();
                            eventInfo.s_vValuez = reader.ReadFloat();
                            eventInfo.s_vValue2x = reader.ReadFloat();
                            eventInfo.s_vValue2y = reader.ReadFloat();
                            eventInfo.s_vValue2z = reader.ReadFloat();

                            sequenceInfo.EventList.Add(eventInfo);
                        }

                        sequenceInfo.ShaderList = new List<ShaderInfo>();

                        for (int k = 0; k < sequenceInfo.s_nShaderCnt; k++)
                        {
                            ShaderInfo shaderInfo = new();

                            shaderInfo.s_cApplyObjectName = reader.ReadZString(Encoding.ASCII, 32);

                            shaderInfo.s_eShaderType = reader.ReadInt();
                            shaderInfo.s_nValue1 = reader.ReadInt();
                            shaderInfo.s_fValue1 = reader.ReadFloat();
                            shaderInfo.s_fValue2 = reader.ReadFloat();
                            shaderInfo.s_fValue3 = reader.ReadFloat();

                            shaderInfo.s_nDummy = new int[4];
                            for (int l = 0; l < 4; l++)
                            {
                                shaderInfo.s_nDummy[l] = reader.ReadInt();
                            }

                            sequenceInfo.ShaderList.Add(shaderInfo);
                        }

                        modelInfo.SequenceList.Add(sequenceInfo);
                    }

                    modelInfos[i] = modelInfo;
                }

                return modelInfos;
            }

        }
        public static void ExportModelToXml(string xmlPath, ModelInfo[] DigimonData)
        {
            XDocument doc = new XDocument();
            XElement rootElement = new XElement("DigimonData");
            doc.Add(rootElement);

            foreach (var model in DigimonData)
            {
                if (model == null)
                    continue;

                XElement modelElement = new XElement("Model");
                modelElement.Add(new XElement("s_dwID", model.s_dwID));
                modelElement.Add(new XElement("s_cKfmPath", model.s_cKfmPath));
                modelElement.Add(new XElement("s_fScale", model.s_fScale));
                modelElement.Add(new XElement("s_fHeight", model.s_fHeight));
                modelElement.Add(new XElement("s_fWidth", model.s_fWidth));
                modelElement.Add(new XElement("s_nSequenceCount", model.s_nSequenceCount));
                modelElement.Add(new XElement("s_Dummy", Convert.ToBase64String(model.s_Dummy)));

                foreach (var sequence in model.SequenceList)
                {
                    XElement sequenceElement = new XElement("Sequence");
                    sequenceElement.Add(new XElement("s_dwSequenceID", sequence.s_dwSequenceID));
                    sequenceElement.Add(new XElement("s_nEventCount", sequence.s_nEventCount));
                    sequenceElement.Add(new XElement("s_nLoopCnt", sequence.s_nLoopCnt));
                    sequenceElement.Add(new XElement("s_nShaderCnt", sequence.s_nShaderCnt));

                    foreach (var evnt in sequence.EventList)
                    {
                        XElement eventElement = new XElement("Event");
                        eventElement.Add(new XElement("s_fEventTime", evnt.s_fEventTime));
                        eventElement.Add(new XElement("s_nType", evnt.s_nType));
                        eventElement.Add(new XElement("s_nStaticIndex", evnt.s_nStaticIndex));
                        eventElement.Add(new XElement("s_cText", evnt.s_cText));
                        eventElement.Add(new XElement("s_dwPlag", evnt.s_dwPlag));
                        eventElement.Add(new XElement("s_vOffsetx", evnt.s_vOffsetx));
                        eventElement.Add(new XElement("s_vOffsety", evnt.s_vOffsety));
                        eventElement.Add(new XElement("s_vOffsetz", evnt.s_vOffsetz));
                        eventElement.Add(new XElement("s_fEffectScale", evnt.s_fEffectScale));
                        eventElement.Add(new XElement("s_bParentScale", evnt.s_bParentScale));
                        eventElement.Add(new XElement("unk", Convert.ToBase64String(evnt.unk)));
                        eventElement.Add(new XElement("s_fFadeoutTime", evnt.s_fFadeoutTime));
                        eventElement.Add(new XElement("s_vValuex", evnt.s_vValuex));
                        eventElement.Add(new XElement("s_vValuey", evnt.s_vValuey));
                        eventElement.Add(new XElement("s_vValuez", evnt.s_vValuez));
                        eventElement.Add(new XElement("s_vValue2x", evnt.s_vValue2x));
                        eventElement.Add(new XElement("s_vValue2y", evnt.s_vValue2y));
                        eventElement.Add(new XElement("s_vValue2z", evnt.s_vValue2z));

                        sequenceElement.Add(eventElement);
                    }

                    foreach (var shader in sequence.ShaderList)
                    {
                        XElement shaderElement = new XElement("Shader");
                        shaderElement.Add(new XElement("s_cApplyObjectName", shader.s_cApplyObjectName));
                        shaderElement.Add(new XElement("s_eShaderType", shader.s_eShaderType));
                        shaderElement.Add(new XElement("s_nValue1", shader.s_nValue1));
                        shaderElement.Add(new XElement("s_fValue1", shader.s_fValue1));
                        shaderElement.Add(new XElement("s_fValue2", shader.s_fValue2));
                        shaderElement.Add(new XElement("s_fValue3", shader.s_fValue3));
                        shaderElement.Add(new XElement("s_nDummy", string.Join(",", shader.s_nDummy)));

                        sequenceElement.Add(shaderElement);
                    }

                    modelElement.Add(sequenceElement);
                }

                rootElement.Add(modelElement);
            }

            doc.Save(xmlPath);

        }
        public static ModelInfo[] ImportModelFromXml(string filePath)
        {
            XDocument xmlDoc = XDocument.Load(filePath);
            List<ModelInfo> modelList = new List<ModelInfo>();

            foreach (XElement modelElement in xmlDoc.Root.Elements("Model"))
            {
                ModelInfo model = new ModelInfo();
                model.s_dwID = int.Parse(modelElement.Element("s_dwID").Value);
                model.s_cKfmPath = modelElement.Element("s_cKfmPath").Value;
                model.s_fScale = float.Parse(modelElement.Element("s_fScale").Value.Replace(".", ","));
                model.s_fHeight = float.Parse(modelElement.Element("s_fHeight").Value.Replace(".", ","));
                model.s_fWidth = float.Parse(modelElement.Element("s_fWidth").Value.Replace(".", ","));
                model.s_nSequenceCount = int.Parse(modelElement.Element("s_nSequenceCount").Value);
                model.s_Dummy = Convert.FromBase64String(modelElement.Element("s_Dummy").Value);

                if (model.s_nSequenceCount > 0)
                {
                    List<SequenceInfo> sequenceList = new List<SequenceInfo>();

                    foreach (XElement sequenceElement in modelElement.Elements("Sequence"))
                    {
                        SequenceInfo sequence = new SequenceInfo();
                        sequence.s_dwSequenceID = int.Parse(sequenceElement.Element("s_dwSequenceID").Value);
                        sequence.s_nEventCount = int.Parse(sequenceElement.Element("s_nEventCount").Value);
                        sequence.s_nLoopCnt = int.Parse(sequenceElement.Element("s_nLoopCnt").Value);
                        sequence.s_nShaderCnt = int.Parse(sequenceElement.Element("s_nShaderCnt").Value);

                        if (sequence.s_nEventCount > 0)
                        {
                            List<EventInfo> eventList = new List<EventInfo>();

                            foreach (XElement eventElement in sequenceElement.Elements("Event"))
                            {
                                EventInfo evnt = new EventInfo();
                                evnt.s_fEventTime = float.Parse(eventElement.Element("s_fEventTime").Value.Replace(".", ","));
                                evnt.s_nType = int.Parse(eventElement.Element("s_nType").Value);
                                evnt.s_nStaticIndex = int.Parse(eventElement.Element("s_nStaticIndex").Value);
                                evnt.s_cText = eventElement.Element("s_cText").Value.TrimEnd('\0');
                                evnt.s_dwPlag = int.Parse(eventElement.Element("s_dwPlag").Value);
                                evnt.s_vOffsetx = float.Parse(eventElement.Element("s_vOffsetx").Value.Replace(".", ","));
                                evnt.s_vOffsety = float.Parse(eventElement.Element("s_vOffsety").Value.Replace(".", ","));
                                evnt.s_vOffsetz = float.Parse(eventElement.Element("s_vOffsetz").Value.Replace(".", ","));
                                evnt.s_fEffectScale = float.Parse(eventElement.Element("s_fEffectScale").Value.Replace(".", ","));
                                evnt.s_bParentScale = byte.Parse(eventElement.Element("s_bParentScale").Value);
                                evnt.unk = Convert.FromBase64String(eventElement.Element("unk").Value);
                                evnt.s_fFadeoutTime = float.Parse(eventElement.Element("s_fFadeoutTime").Value.Replace(".", ","));
                                evnt.s_vValuex = float.Parse(eventElement.Element("s_vValuex").Value.Replace(".", ","));
                                evnt.s_vValuey = float.Parse(eventElement.Element("s_vValuey").Value.Replace(".", ","));
                                evnt.s_vValuez = float.Parse(eventElement.Element("s_vValuez").Value.Replace(".", ","));
                                evnt.s_vValue2x = float.Parse(eventElement.Element("s_vValue2x").Value.Replace(".", ","));
                                evnt.s_vValue2y = float.Parse(eventElement.Element("s_vValue2y").Value.Replace(".", ","));
                                evnt.s_vValue2z = float.Parse(eventElement.Element("s_vValue2z").Value.Replace(".", ","));

                                eventList.Add(evnt);
                            }

                            sequence.EventList = eventList;
                        }

                        if (sequence.s_nShaderCnt > 0)
                        {
                            List<ShaderInfo> shaderList = new List<ShaderInfo>();

                            foreach (XElement shaderElement in sequenceElement.Elements("Shader"))
                            {
                                ShaderInfo shader = new ShaderInfo();
                                shader.s_cApplyObjectName = shaderElement.Element("s_cApplyObjectName").Value.TrimEnd('\0');
                                shader.s_eShaderType = int.Parse(shaderElement.Element("s_eShaderType").Value);
                                shader.s_nValue1 = int.Parse(shaderElement.Element("s_nValue1").Value);
                                shader.s_fValue1 = float.Parse(shaderElement.Element("s_fValue1").Value.Replace(".", ","));
                                shader.s_fValue2 = float.Parse(shaderElement.Element("s_fValue2").Value.Replace(".", ","));
                                shader.s_fValue3 = float.Parse(shaderElement.Element("s_fValue3").Value.Replace(".", ","));
                                shader.s_nDummy = shaderElement.Element("s_nDummy").Value.Split(',').Select(int.Parse).ToArray();

                                shaderList.Add(shader);
                            }

                            sequence.ShaderList = shaderList;
                        }

                        sequenceList.Add(sequence);
                    }

                    model.SequenceList = sequenceList;
                }

                modelList.Add(model);
            }

            return modelList.ToArray();
        }
        public static void ExportModelToBinary(string outputFile, ModelInfo[] modelInfos)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(outputFile, FileMode.Create)))
            {
                writer.Write(modelInfos.Length);

                foreach (ModelInfo model in modelInfos)
                {
                    writer.Write(model.s_dwID);

                    byte[] kfmPathBytes = Encoding.ASCII.GetBytes(model.s_cKfmPath);
                    Array.Resize(ref kfmPathBytes, 160); // Adiciona padding para alcançar o tamanho desejado
                    writer.Write(kfmPathBytes);

                    writer.Write(model.s_fScale);
                    writer.Write(model.s_fHeight);
                    writer.Write(model.s_fWidth);
                    writer.Write(model.s_nSequenceCount);
                    writer.Write(model.s_Dummy);

                    if (model.s_nSequenceCount == 0)
                        continue;

                    foreach (SequenceInfo sequence in model.SequenceList)
                    {
                        writer.Write(sequence.s_dwSequenceID);
                        writer.Write(sequence.s_nEventCount);
                        writer.Write(sequence.s_nLoopCnt);
                        writer.Write(sequence.s_nShaderCnt);

                        if (sequence.s_nEventCount > 0)
                        {
                            foreach (EventInfo evnt in sequence.EventList)
                            {
                                writer.Write(evnt.s_fEventTime);
                                writer.Write(evnt.s_nType);
                                writer.Write(evnt.s_nStaticIndex);

                                byte[] textBytes = Encoding.ASCII.GetBytes(evnt.s_cText);
                                Array.Resize(ref textBytes, 128); // Adiciona padding para alcançar o tamanho desejado
                                writer.Write(textBytes);

                                writer.Write(evnt.s_dwPlag);
                                writer.Write(evnt.s_vOffsetx);
                                writer.Write(evnt.s_vOffsety);
                                writer.Write(evnt.s_vOffsetz);
                                writer.Write(evnt.s_fEffectScale);
                                writer.Write(evnt.s_bParentScale);
                                writer.Write(evnt.unk);
                                writer.Write(evnt.s_fFadeoutTime);
                                writer.Write(evnt.s_vValuex);
                                writer.Write(evnt.s_vValuey);
                                writer.Write(evnt.s_vValuez);
                                writer.Write(evnt.s_vValue2x);
                                writer.Write(evnt.s_vValue2y);
                                writer.Write(evnt.s_vValue2z);
                            }
                        }
                        if (sequence.s_nShaderCnt > 0)
                        {
                            foreach (ShaderInfo shader in sequence.ShaderList)
                            {
                                byte[] applyObjectNameBytes = Encoding.ASCII.GetBytes(shader.s_cApplyObjectName);
                                Array.Resize(ref applyObjectNameBytes, 32); // Adiciona padding para alcançar o tamanho desejado
                                writer.Write(applyObjectNameBytes);
                                writer.Write(shader.s_eShaderType);
                                writer.Write(shader.s_nValue1);
                                writer.Write(shader.s_fValue1);
                                writer.Write(shader.s_fValue2);
                                writer.Write(shader.s_fValue3);

                                for (int i = 0; i < 4; i++)
                                {
                                    writer.Write(shader.s_nDummy[i]);
                                }
                            }
                        }
                    }
                }
            }

        }
    }

    public class SequenceInfo
    {
        public int s_dwSequenceID;
        public int s_nEventCount;
        public int s_nLoopCnt;
        public int s_nShaderCnt;
        public List<EventInfo> EventList;
        public List<ShaderInfo> ShaderList;
    }

    public class EventInfo
    {
        public float s_fEventTime;
        public int s_nType;
        public int s_nStaticIndex;
        public string s_cText;
        public int s_dwPlag;
        public float s_vOffsetx;
        public float s_vOffsety;
        public float s_vOffsetz;
        public float s_fEffectScale;
        public byte s_bParentScale;
        public byte[] unk;
        public float s_fFadeoutTime;
        public float s_vValuex;
        public float s_vValuey;
        public float s_vValuez;
        public float s_vValue2x;
        public float s_vValue2y;
        public float s_vValue2z;
    }

    public class ShaderInfo
    {
        public string s_cApplyObjectName;
        public int s_eShaderType;
        public int s_nValue1;
        public float s_fValue1;
        public float s_fValue2;
        public float s_fValue3;
        public int[] s_nDummy;
    }

}
