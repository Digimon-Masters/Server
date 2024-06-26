using System.Text;
using System.Xml.Linq;

namespace DSO.BinXmlConverter.Process
{
    public class QuestConverter : IProcessBase
    {
        private string _dataBinFile = "DATA\\Bin\\Quest.bin";
        private string _dataXmlFile = "DATA\\Xml\\Quest.xml";
        private string _outputBinFile = "Output\\Bin\\Quest.bin";

        public void XmlToBin()
        {
            if (!File.Exists(_dataXmlFile))
                return;

            var xDoc = XDocument.Load(_dataXmlFile);
            var quests = xDoc.Descendants().Where(x => x.Name.LocalName == "Quest").ToList();

            if (File.Exists(_outputBinFile))
            {
                File.Delete(_outputBinFile);

                Thread.Sleep(1000);
            }

            using (var stream = File.Open(_outputBinFile, FileMode.OpenOrCreate))
            {
                using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
                {
                    writer.Write(quests.Count);

                    foreach (var quest in quests)
                    {
                        writer.Write(Convert.ToInt32(quest.Attribute("Id")?.Value));
                        writer.Write(Convert.ToInt32(quest.Element("Model")?.Value));
                        writer.Write(Convert.ToInt32(quest.Element("Model2")?.Value));
                        writer.Write(Convert.ToInt16(quest.Element("Level")?.Value));
                        writer.Write(Convert.ToInt32(quest.Element("Pos")?.Value));
                        writer.Write(Convert.ToInt32(quest.Element("Pos2")?.Value));
                        writer.Write(Convert.ToInt32(quest.Element("ManagedID")?.Value));
                        writer.Write(Convert.ToInt16(quest.Element("Active")?.Value));
                        writer.Write(Convert.ToByte(quest.Element("Unknown")?.Value));
                        writer.Write(Convert.ToInt32(quest.Element("Type")?.Value));
                        writer.Write(Convert.ToInt32(quest.Element("StartTargetType")?.Value));
                        writer.Write(Convert.ToInt32(quest.Element("StartTargetID")?.Value));
                        writer.Write(Convert.ToInt32(quest.Element("Target")?.Value));
                        writer.Write(Convert.ToInt32(quest.Element("TargetValue")?.Value));

                        var titleTabValue = Util.RemoveCdataFromString(quest.Element("TitleTab")?.Value);
                        titleTabValue = titleTabValue?.Substring(0, titleTabValue.Length > 78 ? 78 : titleTabValue.Length);
                        var titleTabBytes = Encoding.Unicode.GetBytes(titleTabValue + char.MinValue);
                        var titleTabArray = new byte[160];
                        titleTabBytes.CopyTo(titleTabArray, 0);
                        writer.Write(titleTabArray, 0, titleTabArray.Length);

                        var titleTextValue = Util.RemoveCdataFromString(quest.Element("TitleText")?.Value);
                        titleTextValue = titleTextValue?.Substring(0, titleTextValue.Length > 78 ? 78 : titleTextValue.Length);
                        var titleTextBytes = Encoding.Unicode.GetBytes(titleTextValue + char.MinValue);
                        var titleTextArray = new byte[160];
                        titleTextBytes.CopyTo(titleTextArray, 0);
                        writer.Write(titleTextArray, 0, titleTextArray.Length);

                        var bodyTextValue = Util.RemoveCdataFromString(quest.Element("Body")?.Value);
                        bodyTextValue = bodyTextValue?.Substring(0, bodyTextValue.Length > 2046 ? 2046 : bodyTextValue.Length);
                        var bodyTextBytes = Encoding.Unicode.GetBytes(bodyTextValue + char.MinValue);
                        var bodyTextArray = new byte[4096];
                        bodyTextBytes.CopyTo(bodyTextArray, 0);
                        writer.Write(bodyTextArray, 0, bodyTextArray.Length);

                        var simpleTextValue = Util.RemoveCdataFromString(quest.Element("Simple")?.Value);
                        simpleTextValue = simpleTextValue?.Substring(0, simpleTextValue.Length > 126 ? 126 : simpleTextValue.Length);
                        var simpleTextBytes = Encoding.Unicode.GetBytes(simpleTextValue + char.MinValue);
                        var simpleTextArray = new byte[256];
                        simpleTextBytes.CopyTo(simpleTextArray, 0);
                        writer.Write(simpleTextArray, 0, simpleTextArray.Length);

                        var helperTextValue = Util.RemoveCdataFromString(quest.Element("Helper")?.Value);
                        helperTextValue = helperTextValue?.Substring(0, helperTextValue.Length > 510 ? 510 : helperTextValue.Length);
                        var helperTextBytes = Encoding.Unicode.GetBytes(helperTextValue + char.MinValue);
                        var helperTextArray = new byte[1024];
                        helperTextBytes.CopyTo(helperTextArray, 0);
                        writer.Write(helperTextArray, 0, helperTextArray.Length);

                        var processTextValue = Util.RemoveCdataFromString(quest.Element("Process")?.Value);
                        processTextValue = processTextValue?.Substring(0, processTextValue.Length > 318 ? 318 : processTextValue.Length);
                        var processTextBytes = Encoding.Unicode.GetBytes(processTextValue + char.MinValue);
                        var processTextArray = new byte[640];
                        processTextBytes.CopyTo(processTextArray, 0);
                        writer.Write(processTextArray, 0, processTextArray.Length);

                        var completeTextValue = Util.RemoveCdataFromString(quest.Element("Complete")?.Value);
                        completeTextValue = completeTextValue?.Substring(0, completeTextValue.Length > 698 ? 698 : completeTextValue.Length);
                        var completeTextBytes = Encoding.Unicode.GetBytes(completeTextValue + char.MinValue);
                        var completeTextArray = new byte[1400];
                        completeTextBytes.CopyTo(completeTextArray, 0);
                        writer.Write(completeTextArray, 0, completeTextArray.Length);

                        var expertTextValue = Util.RemoveCdataFromString(quest.Element("Expert")?.Value);
                        expertTextValue = expertTextValue?.Substring(0, expertTextValue.Length > 318 ? 318 : expertTextValue.Length);
                        var expertTextBytes = Encoding.Unicode.GetBytes(expertTextValue + char.MinValue);
                        var expertTextArray = new byte[640];
                        expertTextBytes.CopyTo(expertTextArray, 0);
                        writer.Write(expertTextArray, 0, expertTextArray.Length);

                        var givenList = quest.Element("ItemGivenList");
                        writer.Write(Convert.ToInt32(givenList?.Attribute("Count")?.Value));
                        foreach (var given in givenList.Descendants().Where(x => x.Name.LocalName == "ItemGiven").ToList())
                        {
                            writer.Write(Convert.ToInt32(given.Element("Given2")?.Value));
                            writer.Write(Convert.ToInt32(given.Element("Type")?.Value));
                            writer.Write(Convert.ToInt32(given.Element("Amount")?.Value));
                        }

                        var conditionList = quest.Element("ConditionList");
                        writer.Write(Convert.ToInt32(conditionList?.Attribute("Count")?.Value));
                        foreach (var condition in conditionList.Descendants().Where(x => x.Name.LocalName == "Condition").ToList())
                        {
                            writer.Write(Convert.ToInt32(condition.Element("Type")?.Value));
                            writer.Write(Convert.ToInt32(condition.Element("Id")?.Value));
                            writer.Write(Convert.ToInt32(condition.Element("Amount")?.Value));
                        }

                        var goalList = quest.Element("GoalList");
                        writer.Write(Convert.ToInt32(goalList?.Attribute("Count")?.Value));
                        foreach (var goal in goalList.Descendants().Where(x => x.Name.LocalName == "Goal").ToList())
                        {
                            writer.Write(Convert.ToInt32(goal.Element("Type")?.Value));
                            writer.Write(Convert.ToInt32(goal.Element("Id")?.Value));
                            writer.Write(Convert.ToInt32(goal.Element("Amount")?.Value));
                            writer.Write(Convert.ToInt32(goal.Element("CurType")?.Value));
                            writer.Write(Convert.ToInt32(goal.Element("Sub")?.Value));
                            writer.Write(Convert.ToInt32(goal.Element("Sub2")?.Value));
                        }

                        var rewardList = quest.Element("RewardList");
                        writer.Write(Convert.ToInt32(rewardList?.Attribute("Count")?.Value));
                        foreach (var reward in rewardList.Descendants().Where(x => x.Name.LocalName == "Reward").ToList())
                        {
                            writer.Write(Convert.ToInt32(reward.Attribute("Id")?.Value));
                            var type = Convert.ToInt32(reward.Element("Type")?.Value);
                            writer.Write(type);

                            if (type <= 0)
                            {
                                writer.Write(Convert.ToInt32(reward.Element("Money")?.Value));
                                writer.Write(Convert.ToInt32(reward.Element("Unkn")?.Value));
                            }
                            else
                            {
                                writer.Write(Convert.ToInt32(reward.Element("ItemId")?.Value));
                                writer.Write(Convert.ToInt32(reward.Element("ItemAmount")?.Value));
                            }
                        }

                        writer.Write(Convert.ToInt32(quest.Element("UnknInt1")?.Value));
                        writer.Write(Convert.ToInt32(quest.Element("UnknInt2")?.Value));
                        writer.Write(Convert.ToInt32(quest.Element("UnknInt3")?.Value));
                        writer.Write(Convert.ToInt32(quest.Element("UnknInt4")?.Value));
                        writer.Write(Convert.ToInt32(quest.Element("UnknInt5")?.Value));
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
                    sb.AppendLine("<QuestList>");
                    int questcount = read.ReadInt();
                    for (int i = 0; i < questcount; i++)
                    {
                        sb.AppendLine($"<Quest Id=\"{read.ReadInt()}\">");
                        sb.AppendLine($"<Model>{read.ReadInt()}</Model>");
                        sb.AppendLine($"<Model2>{read.ReadInt()}</Model2>");
                        sb.AppendLine($"<Level>{read.ReadShort()}</Level>");
                        sb.AppendLine($"<Pos>{read.ReadInt()}</Pos>");
                        sb.AppendLine($"<Pos2>{read.ReadInt()}</Pos2>");
                        sb.AppendLine($"<ManagedID>{read.ReadInt()}</ManagedID>");
                        sb.AppendLine($"<Active>{read.ReadShort()}</Active>");
                        sb.AppendLine($"<Unknown>{read.ReadByte()}</Unknown>");
                        sb.AppendLine($"<Type>{read.ReadInt()}</Type>");
                        sb.AppendLine($"<StartTargetType>{read.ReadInt()}</StartTargetType>");
                        sb.AppendLine($"<StartTargetID>{read.ReadInt()}</StartTargetID>");
                        sb.AppendLine($"<Target>{read.ReadInt()}</Target>");
                        sb.AppendLine($"<TargetValue>{read.ReadInt()}</TargetValue>");
                        sb.AppendLine($"<TitleTab>{Util.AddCdataForUnicodeChars(read.ReadZString(Encoding.Unicode, 160))}</TitleTab>");
                        sb.AppendLine($"<TitleText>{Util.AddCdataForUnicodeChars(read.ReadZString(Encoding.Unicode, 160))}</TitleText>");
                        sb.AppendLine($"<Body>{Util.AddCdataForUnicodeChars(read.ReadZString(Encoding.Unicode, 4096))}</Body>");
                        sb.AppendLine($"<Simple>{Util.AddCdataForUnicodeChars(read.ReadZString(Encoding.Unicode, 256))}</Simple>");
                        sb.AppendLine($"<Helper>{Util.AddCdataForUnicodeChars(read.ReadZString(Encoding.Unicode, 1024))}</Helper>");
                        sb.AppendLine($"<Process>{Util.AddCdataForUnicodeChars(read.ReadZString(Encoding.Unicode, 640))}</Process>");
                        sb.AppendLine($"<Complete>{Util.AddCdataForUnicodeChars(read.ReadZString(Encoding.Unicode, 1400))}</Complete>");
                        sb.AppendLine($"<Expert>{Util.AddCdataForUnicodeChars(read.ReadZString(Encoding.Unicode, 640))}</Expert>");

                        var givenCount = read.ReadInt();
                        sb.AppendLine($"<ItemGivenList Count=\"{givenCount}\">");
                        for (int j = 0; j < givenCount; j++)
                        {
                            sb.AppendLine($"<ItemGiven>");
                            sb.AppendLine($"<Given2>{read.ReadInt()}</Given2>");
                            sb.AppendLine($"<Type>{read.ReadInt()}</Type>");
                            sb.AppendLine($"<Amount>{read.ReadInt()}</Amount>");
                            sb.AppendLine($"</ItemGiven>");
                        }
                        sb.AppendLine($"</ItemGivenList>");

                        var conditionCount = read.ReadInt();
                        sb.AppendLine($"<ConditionList Count=\"{conditionCount}\">");
                        for (int j = 0; j < conditionCount; j++)
                        {
                            sb.AppendLine($"<Condition>");
                            sb.AppendLine($"<Type>{read.ReadInt()}</Type>");
                            sb.AppendLine($"<Id>{read.ReadInt()}</Id>");
                            sb.AppendLine($"<Amount>{read.ReadInt()}</Amount>");
                            sb.AppendLine($"</Condition>");
                        }
                        sb.AppendLine($"</ConditionList>");

                        var goalCount = read.ReadInt();
                        sb.AppendLine($"<GoalList Count=\"{goalCount}\">");
                        for (int j = 0; j < goalCount; j++)
                        {
                            sb.AppendLine($"<Goal>");
                            sb.AppendLine($"<Type>{read.ReadInt()}</Type>");
                            sb.AppendLine($"<Id>{read.ReadInt()}</Id>");
                            sb.AppendLine($"<Amount>{read.ReadInt()}</Amount>");
                            sb.AppendLine($"<CurType>{read.ReadInt()}</CurType>");
                            sb.AppendLine($"<Sub>{read.ReadInt()}</Sub>");
                            sb.AppendLine($"<Sub2>{read.ReadInt()}</Sub2>");
                            sb.AppendLine($"</Goal>");
                        }
                        sb.AppendLine($"</GoalList>");

                        var rewardCount = read.ReadInt();
                        sb.AppendLine($"<RewardList Count=\"{rewardCount}\">");
                        for (int j = 0; j < rewardCount; j++)
                        {
                            sb.AppendLine($"<Reward Id=\"{read.ReadInt()}\">");
                            var rewardType = read.ReadInt();

                            sb.AppendLine($"<Type>{rewardType}</Type>");

                            if (rewardType <= 0)
                            {
                                sb.AppendLine($"<Money>{read.ReadInt()}</Money>");
                                sb.AppendLine($"<Unkn>{read.ReadInt()}</Unkn>");
                            }
                            else
                            {
                                sb.AppendLine($"<ItemId>{read.ReadInt()}</ItemId>");
                                sb.AppendLine($"<ItemAmount>{read.ReadInt()}</ItemAmount>");
                            }
                            sb.AppendLine($"</Reward>");
                        }
                        sb.AppendLine($"</RewardList>");

                        sb.AppendLine($"<UnknInt1>{read.ReadInt()}</UnknInt1>");
                        sb.AppendLine($"<UnknInt2>{read.ReadInt()}</UnknInt2>");
                        sb.AppendLine($"<UnknInt3>{read.ReadInt()}</UnknInt3>");
                        sb.AppendLine($"<UnknInt4>{read.ReadInt()}</UnknInt4>");
                        sb.AppendLine($"<UnknInt5>{read.ReadInt()}</UnknInt5>");
                        sb.AppendLine($"</Quest>");
                    }
                    sb.AppendLine("</QuestList>");
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
