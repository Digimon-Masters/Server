using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static BinXmlConverter.Classes.TalkGlobal;

namespace BinXmlConverter.Classes
{
    public class TalkGlobal
    {
        public class TalkDigimon
        {
            public int Id;
            public int s_dwParam;
            public ushort s_nType;
            public string s_szText;
            public string s_szList;
            public byte unknow;
        }
        public class TalkEvent
        {
            public int Id;
            public int s_dwTalkNum;
            public string s_szText;
            public byte unknow;
        }
        public class TalkMessage
        {
            public int s_dwID;
            public int s_MsgType;
            public int s_Type;

            public string s_TitleName;
            public string s_Message;
            public int s_dwLinkID;
        }
        public class TalkTip
        {
            public int Id;
            public string s_szTip;
        }
        public class TalkLoadingTip
        {
            public int Id;
            public string s_szLoadingTip;
            public int s_nLevel;
        }

        public static (TalkDigimon[], TalkEvent[], TalkMessage[], TalkTip[], TalkLoadingTip[]) ReadTalkFromBinary(string filePath)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                var count = reader.ReadInt32();

                TalkDigimon[] talkDigimons = new TalkDigimon[count];

                for (int i = 0; i < count; i++)
                {
                    TalkDigimon talkDigimon = new();
                    talkDigimon.Id = reader.ReadInt32();
                    talkDigimon.s_dwParam = reader.ReadInt32();
                    talkDigimon.s_nType = reader.ReadUInt16();
                    byte[] s_szTextBytes = reader.ReadBytes(200);
                    talkDigimon.s_szText = CleanString(System.Text.Encoding.Unicode.GetString(s_szTextBytes));
                    byte[] s_szListBytes = reader.ReadBytes(201);
                    talkDigimon.s_szList = CleanString(System.Text.Encoding.Unicode.GetString(s_szListBytes));
                    talkDigimon.unknow = reader.ReadByte();
                    talkDigimons[i] = talkDigimon;
                }

                count = reader.ReadInt32();

                TalkEvent[] talkEvent = new TalkEvent[count];

                for (int i = 0; i < count; i++)
                {
                    TalkEvent Event = new();
                    Event.Id = reader.ReadInt32();
                    Event.s_dwTalkNum = reader.ReadInt32();
                    byte[] s_szTextBytes = reader.ReadBytes(400);
                    Event.s_szText = CleanString(System.Text.Encoding.Unicode.GetString(s_szTextBytes));

                    talkEvent[i] = Event;
                }

                count = reader.ReadInt32();

                TalkMessage[] talkMessages = new TalkMessage[count];

                for (int i = 0; i < count; i++)
                {
                    TalkMessage talkMessage = new();
                    talkMessage.s_dwID = reader.ReadInt32();
                    talkMessage.s_MsgType = reader.ReadInt32();
                    talkMessage.s_Type = reader.ReadInt32();

                    byte[] s_szTextBytes = reader.ReadBytes(32);
                    talkMessage.s_TitleName = CleanString(System.Text.Encoding.Unicode.GetString(s_szTextBytes));

                    byte[] s_MessageBytes = reader.ReadBytes(512);
                    talkMessage.s_Message = CleanString(System.Text.Encoding.Unicode.GetString(s_MessageBytes));
                    talkMessage.s_dwLinkID = reader.ReadInt32();

                    talkMessages[i] = talkMessage;
                }

                count = reader.ReadInt32();

                TalkTip[] talkTips = new TalkTip[count];

                for (int i = 0; i < count; i++)
                {
                    TalkTip talkTip = new();
                    talkTip.Id = reader.ReadInt32();

                    byte[] s_szTextBytes = reader.ReadBytes(400);
                    talkTip.s_szTip = CleanString(System.Text.Encoding.Unicode.GetString(s_szTextBytes));

                    talkTips[i] = talkTip;
                }
                count = reader.ReadInt32();

                TalkLoadingTip[] loadingTips = new TalkLoadingTip[count];

                for (int i = 0; i < count; i++)
                {
                    TalkLoadingTip loadingTip = new();
                    loadingTip.Id = reader.ReadInt32();

                    byte[] s_szTextBytes = reader.ReadBytes(400);
                    loadingTip.s_szLoadingTip = CleanString(System.Text.Encoding.Unicode.GetString(s_szTextBytes));
                    loadingTip.s_nLevel = reader.ReadInt32();

                    loadingTips[i] = loadingTip;
                }

                return (talkDigimons, talkEvent, talkMessages, talkTips, loadingTips);
            }
        }
        public static void ExportTalkDigimonToXml(TalkDigimon[] talkDigimonArray, string filePath)
        {
            XElement rootElement = new XElement("TalkDigimon");

            foreach (TalkDigimon talkDigimon in talkDigimonArray)
            {
                XElement talkDigimonElement = new XElement("TalkDigimon",
                    new XElement("Id", talkDigimon.Id),
                    new XElement("s_dwParam", talkDigimon.s_dwParam),
                    new XElement("s_nType", talkDigimon.s_nType),
                    new XElement("s_szText", talkDigimon.s_szText),
                    new XElement("s_szList", talkDigimon.s_szList),
                    new XElement("unknow", talkDigimon.unknow)
                );

                rootElement.Add(talkDigimonElement);
            }

            XDocument xmlDoc = new XDocument(rootElement);
            xmlDoc.Save(filePath);
        }
        public static void ExportTalkEventToXml(TalkEvent[] talkEventArray, string filePath)
        {
            XElement rootElement = new XElement("TalkEvent");

            foreach (TalkEvent talkEvent in talkEventArray)
            {
                XElement talkEventElement = new XElement("TalkEvent",
                    new XElement("Id", talkEvent.Id),
                    new XElement("s_dwTalkNum", talkEvent.s_dwTalkNum),
                    new XElement("s_szText", talkEvent.s_szText),
                    new XElement("unknow", talkEvent.unknow)
                );

                rootElement.Add(talkEventElement);
            }

            XDocument xmlDoc = new XDocument(rootElement);
            xmlDoc.Save(filePath);
        }
        public static void ExportTalkMessageToXml(TalkMessage[] talkMessageArray, string filePath)
        {
            XElement rootElement = new XElement("TalkMessage");

            foreach (TalkMessage talkMessage in talkMessageArray)
            {
                XElement talkMessageElement = new XElement("TalkMessage",
                    new XElement("s_dwID", talkMessage.s_dwID),
                    new XElement("s_MsgType", talkMessage.s_MsgType),
                    new XElement("s_Type", talkMessage.s_Type),
                    new XElement("s_TitleName", talkMessage.s_TitleName),
                    new XElement("s_Message", talkMessage.s_Message),
                    new XElement("s_dwLinkID", talkMessage.s_dwLinkID)
                );

                rootElement.Add(talkMessageElement);
            }

            XDocument xmlDoc = new XDocument(rootElement);
            xmlDoc.Save(filePath);
        }
        public static void ExportTalkTipToXml(TalkTip[] talkTipArray, string filePath)
        {
            XElement rootElement = new XElement("TalkTip");

            foreach (TalkTip talkTip in talkTipArray)
            {
                XElement talkTipElement = new XElement("TalkTip",
                    new XElement("Id", talkTip.Id),
                    new XElement("s_szTip", talkTip.s_szTip)
                );

                rootElement.Add(talkTipElement);
            }

            XDocument xmlDoc = new XDocument(rootElement);
            xmlDoc.Save(filePath);
        }
        public static void ExportTalkLoadingTipToXml(TalkLoadingTip[] talkLoadingTipArray, string filePath)
        {
            XElement rootElement = new XElement("TalkLoadingTip");

            foreach (TalkLoadingTip talkLoadingTip in talkLoadingTipArray)
            {
                XElement talkLoadingTipElement = new XElement("TalkLoadingTip",
                    new XElement("Id", talkLoadingTip.Id),
                    new XElement("s_szLoadingTip", talkLoadingTip.s_szLoadingTip),
                    new XElement("s_nLevel", talkLoadingTip.s_nLevel)
                );

                rootElement.Add(talkLoadingTipElement);
            }

            XDocument xmlDoc = new XDocument(rootElement);
            xmlDoc.Save(filePath);
        }
        public static TalkDigimon[] ImportTalkDigimonFromXml(string filePath)
        {
            XDocument xmlDoc = XDocument.Load(filePath);
            XElement rootElement = xmlDoc.Element("TalkDigimon");

            List<TalkDigimon> talkDigimonList = new List<TalkDigimon>();

            if (rootElement != null)
            {
                foreach (XElement talkDigimonElement in rootElement.Elements("TalkDigimon"))
                {
                    TalkDigimon talkDigimon = new TalkDigimon();
                    talkDigimon.Id = int.Parse(talkDigimonElement.Element("Id").Value);
                    talkDigimon.s_dwParam = int.Parse(talkDigimonElement.Element("s_dwParam").Value);
                    talkDigimon.s_nType = ushort.Parse(talkDigimonElement.Element("s_nType").Value);
                    talkDigimon.s_szText = talkDigimonElement.Element("s_szText").Value;
                    talkDigimon.s_szList = talkDigimonElement.Element("s_szList").Value;
                    talkDigimon.unknow = byte.Parse(talkDigimonElement.Element("unknow").Value);

                    talkDigimonList.Add(talkDigimon);
                }
            }

            return talkDigimonList.ToArray();
        }
        public static TalkEvent[] ImportTalkEventFromXml(string filePath)
        {
            XDocument xmlDoc = XDocument.Load(filePath);
            XElement rootElement = xmlDoc.Element("TalkEvent");

            List<TalkEvent> talkEventList = new List<TalkEvent>();

            if (rootElement != null)
            {
                foreach (XElement talkEventElement in rootElement.Elements("TalkEvent"))
                {
                    TalkEvent talkEvent = new TalkEvent();
                    talkEvent.Id = int.Parse(talkEventElement.Element("Id").Value);
                    talkEvent.s_dwTalkNum = int.Parse(talkEventElement.Element("s_dwTalkNum").Value);
                    talkEvent.s_szText = talkEventElement.Element("s_szText").Value;
                    talkEvent.unknow = byte.Parse(talkEventElement.Element("unknow").Value);

                    talkEventList.Add(talkEvent);
                }
            }

            return talkEventList.ToArray();
        }
        public static TalkMessage[] ImportTalkMessageFromXml(string filePath)
        {
            XDocument xmlDoc = XDocument.Load(filePath);
            XElement rootElement = xmlDoc.Element("TalkMessage");

            List<TalkMessage> talkMessageList = new List<TalkMessage>();

            if (rootElement != null)
            {
                foreach (XElement talkMessageElement in rootElement.Elements("TalkMessage"))
                {
                    TalkMessage talkMessage = new TalkMessage();
                    talkMessage.s_dwID = int.Parse(talkMessageElement.Element("s_dwID").Value);
                    talkMessage.s_MsgType = int.Parse(talkMessageElement.Element("s_MsgType").Value);
                    talkMessage.s_Type = int.Parse(talkMessageElement.Element("s_Type").Value);
                    talkMessage.s_TitleName = talkMessageElement.Element("s_TitleName").Value;
                    talkMessage.s_Message = talkMessageElement.Element("s_Message").Value;
                    talkMessage.s_dwLinkID = int.Parse(talkMessageElement.Element("s_dwLinkID").Value);

                    talkMessageList.Add(talkMessage);
                }
            }

            return talkMessageList.ToArray();
        }
        public static TalkTip[] ImportTalkTipFromXml(string filePath)
        {
            XDocument xmlDoc = XDocument.Load(filePath);
            XElement rootElement = xmlDoc.Element("TalkTip");

            List<TalkTip> talkTipList = new List<TalkTip>();

            if (rootElement != null)
            {
                foreach (XElement talkTipElement in rootElement.Elements("TalkTip"))
                {
                    TalkTip talkTip = new TalkTip();
                    talkTip.Id = int.Parse(talkTipElement.Element("Id").Value);
                    talkTip.s_szTip = talkTipElement.Element("s_szTip").Value;

                    talkTipList.Add(talkTip);
                }
            }

            return talkTipList.ToArray();
        }
        public static TalkLoadingTip[] ImportTalkLoadingTipFromXml(string filePath)
        {
            XDocument xmlDoc = XDocument.Load(filePath);
            XElement rootElement = xmlDoc.Element("TalkLoadingTip");

            List<TalkLoadingTip> talkLoadingTipList = new List<TalkLoadingTip>();

            if (rootElement != null)
            {
                foreach (XElement talkLoadingTipElement in rootElement.Elements("TalkLoadingTip"))
                {
                    TalkLoadingTip talkLoadingTip = new TalkLoadingTip();
                    talkLoadingTip.Id = int.Parse(talkLoadingTipElement.Element("Id").Value);
                    talkLoadingTip.s_szLoadingTip = talkLoadingTipElement.Element("s_szLoadingTip").Value;
                    talkLoadingTip.s_nLevel = int.Parse(talkLoadingTipElement.Element("s_nLevel").Value);

                    talkLoadingTipList.Add(talkLoadingTip);
                }
            }

            return talkLoadingTipList.ToArray();
        }
        public static void ExportTalkToBinary(TalkDigimon[] talkDigimonArray, TalkEvent[] talkEventArray, TalkMessage[] talkMessageArray, TalkTip[] talkTipArray, TalkLoadingTip[] talkLoadingTipArray, string filePath)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {
                // Export TalkDigimon
                writer.Write(talkDigimonArray.Length);
                foreach (TalkDigimon talkDigimon in talkDigimonArray)
                {
                    writer.Write(talkDigimon.Id);
                    writer.Write(talkDigimon.s_dwParam);
                    writer.Write(talkDigimon.s_nType);

                    for (int i = 0; i < 200 / 2; i++)
                    {
                        char c = i < talkDigimon.s_szText.Length ? talkDigimon.s_szText[i] : '\0';
                        writer.Write((ushort)c);
                    }

                    for (int i = 0; i < 101; i++)
                    {
                        char c = i < talkDigimon.s_szList.Length ? talkDigimon.s_szList[i] : '\0';
                        writer.Write((ushort)c);
                    }     
                }

                // Export TalkEvent
                writer.Write(talkEventArray.Length);
                foreach (TalkEvent talkEvent in talkEventArray)
                {
                    writer.Write(talkEvent.Id);
                    writer.Write(talkEvent.s_dwTalkNum);

                    for (int i = 0; i < 400 / 2; i++)
                    {
                        char c = i < talkEvent.s_szText.Length ? talkEvent.s_szText[i] : '\0';
                        writer.Write((ushort)c);
                    }
    
                }

                // Export TalkMessage
                writer.Write(talkMessageArray.Length);
                foreach (TalkMessage talkMessage in talkMessageArray)
                {
                    writer.Write(talkMessage.s_dwID);
                    writer.Write(talkMessage.s_MsgType);
                    writer.Write(talkMessage.s_Type);

                    for (int i = 0; i < 32 / 2; i++)
                    {
                        char c = i < talkMessage.s_TitleName.Length ? talkMessage.s_TitleName[i] : '\0';
                        writer.Write((ushort)c);
                    }

                    for (int i = 0; i < 512 / 2; i++)
                    {
                        char c = i < talkMessage.s_Message.Length ? talkMessage.s_Message[i] : '\0';
                        writer.Write((ushort)c);
                    }
                    writer.Write(talkMessage.s_dwLinkID);
                }

                // Export TalkTip
                writer.Write(talkTipArray.Length);
                foreach (TalkTip talkTip in talkTipArray)
                {
                    writer.Write(talkTip.Id);

                    for (int i = 0; i < 400 / 2; i++)
                    {
                        char c = i < talkTip.s_szTip.Length ? talkTip.s_szTip[i] : '\0';
                        writer.Write((ushort)c);
                    }
                }

                // Export TalkLoadingTip
                writer.Write(talkLoadingTipArray.Length);
                foreach (TalkLoadingTip talkLoadingTip in talkLoadingTipArray)
                {
                    writer.Write(talkLoadingTip.Id);

                    for (int i = 0; i < 400 / 2; i++)
                    {
                        char c = i < talkLoadingTip.s_szLoadingTip.Length ? talkLoadingTip.s_szLoadingTip[i] : '\0';
                        writer.Write((ushort)c);
                    }

                    writer.Write(talkLoadingTip.s_nLevel);
                }
            }
        }
        static string CleanString(string input)
        {
            int nullIndex = input.IndexOf('\0');
            if (nullIndex >= 0)
            {
                return input.Substring(0, nullIndex);
            }
            else
            {
                return input;
            }
        }

    }

}
