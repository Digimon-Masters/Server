
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BinXmlConverter.Classes
{
    public enum eQUEST_TYPE
    {
        QT_SUB = 0,
        QT_MAIN,
        QT_DAILY,
        QT_REPEAT,
        QT_EVENTREPEAT,
        QT_ACHIEVE,
        QT_JOINTPROGRESS,
        QT_TUTORIAL,
        QT_XANTI_JOINTPROGRESS,
        QT_TIME,
    };

    public class QuestInfos
    {
        public int UniqID;
        public int Model;
        public int Model2;
        public short Level;
        public int Pos;
        public int Pos2;
        public int ManagedID;
        public short Active;
        public byte Unknown;
        public int Immediate;
        public short ResetQuest;
        public eQUEST_TYPE Type;
        public int StartTargetType;
        public int StartTargetID;
        public int Target;
        public int TargetValue;
        public string TitleTab;
        public string TitleText;
        public string Body;
        public string Simple;
        public string Helper;
        public string Process;
        public string Complete;
        public string Expert;
        public int Itemgiven;
        public List<QuestItem> questItems = new();
        public int condition;
        public List<QuestCondition> conditions = new();
        public int Goals;
        public List<QuestGoals> QuestGoals = new();
        public int RewardNumber;
        public List<RewardQuantity> rewardQuantities = new();
        public int[] Event;

        public static QuestInfos[] ReadQuestFromBinary(string filePath)
        {
            using (BitReader read = new BitReader(File.Open(filePath, FileMode.Open)))
            {
                int questcount = read.ReadInt();
                QuestInfos[] questInfos = new QuestInfos[questcount];
                for (int i = 0; i < questcount; i++)
                {
                    QuestInfos quest = new QuestInfos();

                    quest.UniqID = read.ReadInt();
                    quest.Model = read.ReadInt();
                    quest.Model2 = read.ReadInt();
                    quest.Level = read.ReadShort();
                    quest.Pos = read.ReadInt();
                    quest.Pos2 = read.ReadInt();
                    quest.ManagedID = read.ReadInt();
                    quest.Active = read.ReadShort();

                    quest.Unknown = read.ReadByte();

                    quest.Type = (eQUEST_TYPE)read.ReadInt();
                    quest.StartTargetType = read.ReadInt();
                    quest.StartTargetID = read.ReadInt();
                    quest.Target = read.ReadInt();
                    quest.TargetValue = read.ReadInt();
                    quest.TitleTab = read.ReadZString(Encoding.Unicode, 160);
                    quest.TitleText = read.ReadZString(Encoding.Unicode, 160);
                    quest.Body = read.ReadZString(Encoding.Unicode, 4096);
                    quest.Simple = read.ReadZString(Encoding.Unicode, 256);
                    quest.Helper = read.ReadZString(Encoding.Unicode, 1024);
                    quest.Process = read.ReadZString(Encoding.Unicode, 640);
                    quest.Complete = read.ReadZString(Encoding.Unicode, 1400);
                    quest.Expert = read.ReadZString(Encoding.Unicode, 640);

                    quest.Itemgiven = read.ReadInt(); //items, given for quest
                    for (int a = 0; a < quest.Itemgiven; a++)
                    {
                        QuestItem questItem = new();
                        questItem.Itemgiven = read.ReadInt();
                        questItem.ItemgivenType = read.ReadInt();
                        questItem.ItemgivenAmount = read.ReadInt();
                        quest.questItems.Add(questItem);
                    }

                    quest.condition = read.ReadInt(); //conditions for starting the quest
                    for (int g = 0; g < quest.condition; g++)
                    {
                        QuestCondition questCondition = new();
                        questCondition.ConditionType = read.ReadInt();
                        questCondition.ConditionId = read.ReadInt();
                        questCondition.ConditionCount = read.ReadInt();
                        quest.conditions.Add(questCondition);
                    }

                    quest.Goals = read.ReadInt(); //quest goals

                    for (int f = 0; f < quest.Goals; f++)
                    {
                        QuestGoals questGoals = new();
                        questGoals.GoalType = read.ReadInt();
                        questGoals.GoalId = read.ReadInt();
                        questGoals.goalAmount = read.ReadInt(); //?
                        questGoals.CurTypeCount = read.ReadInt();
                        questGoals.SubValue = read.ReadInt();
                        questGoals.SubValue1 = read.ReadInt();
                        quest.QuestGoals.Add(questGoals);

                    }

                    quest.RewardNumber = read.ReadInt(); //reward
                    for (int f = 0; f < quest.RewardNumber; f++)
                    {
                        RewardQuantity quantity = new RewardQuantity();

                        quantity.Reward = read.ReadInt();
                        quantity.RewardType = read.ReadInt();

                        if (quantity.RewardType <= 0) //money
                        {
                            QuestRewardMoney questRewardMoney = new();
                            questRewardMoney.RewardMoney = read.ReadInt();
                            questRewardMoney.RewardUnk = read.ReadInt();

                            quantity.QuestRewardMoney.Add(questRewardMoney);

                        }
                        if (quantity.RewardType >= 1) //item
                        {
                            QuestRewardItems questRewardItems = new();
                            questRewardItems.RewardItem = read.ReadInt();
                            questRewardItems.RewardAmount = read.ReadInt();
                            quantity.QuestRewardItems.Add(questRewardItems);

                        }

                        quest.rewardQuantities.Add(quantity);
                    }

                    quest.Event = new int[read.ReadInt()];
                    for (int x = 0; x < quest.Event.Length; x++)
                    {
                        quest.Event[x] = read.ReadInt(); //?
                    }

                    questInfos[i] = quest;
                }

                return questInfos;
            }

        }
        public static void ExportQuestsToXml(QuestInfos[] questInfosArray, string filePath)
        {

            XElement rootElement = new XElement("QuestInfos");

            foreach (QuestInfos questInfo in questInfosArray)
            {
                XElement questInfoElement = new XElement("QuestInfo");
                questInfoElement.Add(new XElement("UniqID", questInfo.UniqID));
                questInfoElement.Add(new XElement("Model", questInfo.Model));
                questInfoElement.Add(new XElement("Model2", questInfo.Model2));
                questInfoElement.Add(new XElement("Level", questInfo.Level));
                questInfoElement.Add(new XElement("Pos", questInfo.Pos));
                questInfoElement.Add(new XElement("Pos2", questInfo.Pos2));
                questInfoElement.Add(new XElement("ManagedID", questInfo.ManagedID));
                questInfoElement.Add(new XElement("Active", questInfo.Active));
                questInfoElement.Add(new XElement("Unknown", questInfo.Unknown));
                questInfoElement.Add(new XElement("Immediate", questInfo.Immediate));
                questInfoElement.Add(new XElement("ResetQuest", questInfo.ResetQuest));
                questInfoElement.Add(new XElement("Type", (int)questInfo.Type));
                questInfoElement.Add(new XElement("StartTargetType", questInfo.StartTargetType));
                questInfoElement.Add(new XElement("StartTargetID", questInfo.StartTargetID));
                questInfoElement.Add(new XElement("Target", questInfo.Target));
                questInfoElement.Add(new XElement("TargetValue", questInfo.TargetValue));
                questInfoElement.Add(new XElement("TitleTab", questInfo.TitleTab));
                questInfoElement.Add(new XElement("TitleText", questInfo.TitleText));
                questInfoElement.Add(new XElement("Body", questInfo.Body));
                questInfoElement.Add(new XElement("Simple", questInfo.Simple));
                questInfoElement.Add(new XElement("Helper", questInfo.Helper));
                questInfoElement.Add(new XElement("Process", questInfo.Process));
                questInfoElement.Add(new XElement("Complete", questInfo.Complete));
                questInfoElement.Add(new XElement("Expert", questInfo.Expert));
                questInfoElement.Add(new XElement("Itemgiven", questInfo.Itemgiven));

                XElement questItemsElement = new XElement("QuestItems");
                foreach (QuestItem questItem in questInfo.questItems)
                {
                    XElement questItemElement = new XElement("QuestItem");
                    questItemElement.Add(new XElement("Itemgiven", questItem.Itemgiven));
                    questItemElement.Add(new XElement("ItemgivenType", questItem.ItemgivenType));
                    questItemElement.Add(new XElement("ItemgivenAmount", questItem.ItemgivenAmount));
                    questItemsElement.Add(questItemElement);
                }
                questInfoElement.Add(questItemsElement);

                questInfoElement.Add(new XElement("condition", questInfo.condition));

                XElement questConditionsElement = new XElement("QuestConditions");
                foreach (QuestCondition questCondition in questInfo.conditions)
                {
                    XElement questConditionElement = new XElement("QuestCondition");
                    questConditionElement.Add(new XElement("ConditionType", questCondition.ConditionType));
                    questConditionElement.Add(new XElement("ConditionId", questCondition.ConditionId));
                    questConditionElement.Add(new XElement("ConditionCount", questCondition.ConditionCount));
                    questConditionsElement.Add(questConditionElement);
                }
                questInfoElement.Add(questConditionsElement);

                questInfoElement.Add(new XElement("Goals", questInfo.Goals));

                XElement questGoalsElement = new XElement("QuestGoals");
                foreach (QuestGoals questGoal in questInfo.QuestGoals)
                {
                    XElement questGoalElement = new XElement("QuestGoal");
                    questGoalElement.Add(new XElement("GoalType", questGoal.GoalType));
                    questGoalElement.Add(new XElement("GoalId", questGoal.GoalId));
                    questGoalElement.Add(new XElement("GoalCount", questGoal.GoalCount));
                    questGoalElement.Add(new XElement("goalAmount", questGoal.goalAmount));
                    questGoalElement.Add(new XElement("CurTypeCount", questGoal.CurTypeCount));
                    questGoalElement.Add(new XElement("SubValue", questGoal.SubValue));
                    questGoalElement.Add(new XElement("SubValue1", questGoal.SubValue1));
                    questGoalsElement.Add(questGoalElement);
                }
                questInfoElement.Add(questGoalsElement);

                questInfoElement.Add(new XElement("RewardNumber", questInfo.RewardNumber));

                XElement rewardQuantitiesElement = new XElement("RewardQuantities");
                foreach (RewardQuantity rewardQuantity in questInfo.rewardQuantities)
                {
                    XElement rewardQuantityElement = new XElement("RewardQuantity");
                    rewardQuantityElement.Add(new XElement("Reward", rewardQuantity.Reward));
                    rewardQuantityElement.Add(new XElement("RewardType", rewardQuantity.RewardType));

                    XElement questRewardMoneyElement = new XElement("QuestRewardMoney");
                    foreach (QuestRewardMoney questRewardMoney in rewardQuantity.QuestRewardMoney)
                    {
                        XElement questRewardMoneyItemElement = new XElement("QuestRewardMoneyItem");
                        questRewardMoneyItemElement.Add(new XElement("RewardMoney", questRewardMoney.RewardMoney));
                        questRewardMoneyItemElement.Add(new XElement("RewardUnk", questRewardMoney.RewardUnk));
                        questRewardMoneyElement.Add(questRewardMoneyItemElement);
                    }
                    rewardQuantityElement.Add(questRewardMoneyElement);

                    XElement questRewardItemsElement = new XElement("QuestRewardItems");
                    foreach (QuestRewardItems questRewardItems in rewardQuantity.QuestRewardItems)
                    {
                        XElement questRewardItemsItemElement = new XElement("QuestRewardItemsItem");
                        questRewardItemsItemElement.Add(new XElement("RewardItem", questRewardItems.RewardItem));
                        questRewardItemsItemElement.Add(new XElement("RewardAmount", questRewardItems.RewardAmount));
                        questRewardItemsElement.Add(questRewardItemsItemElement);
                    }
                    rewardQuantityElement.Add(questRewardItemsElement);

                    rewardQuantitiesElement.Add(rewardQuantityElement);
                }
                questInfoElement.Add(rewardQuantitiesElement);

                XElement eventElement = new XElement("Event");
                foreach (int eventId in questInfo.Event)
                {
                    XElement eventIdElement = new XElement("EventId", eventId);
                    eventElement.Add(eventIdElement);
                }
                questInfoElement.Add(eventElement);

                rootElement.Add(questInfoElement);


                XDocument xmlDocument = new XDocument(rootElement);
                xmlDocument.Save(filePath);
            }
        }
        public static QuestInfos[] ImportQuestsFromXml(string filePath)
        {
            XDocument xDocument = XDocument.Load(filePath);
            XElement rootElement = xDocument.Root;

            List<QuestInfos> questInfosList = new List<QuestInfos>();

            foreach (XElement questInfoElement in rootElement.Elements("QuestInfo"))
            {
                QuestInfos questInfo = new QuestInfos();

                questInfo.UniqID = int.Parse(questInfoElement.Element("UniqID").Value);
                questInfo.Model = int.Parse(questInfoElement.Element("Model").Value);
                questInfo.Model2 = int.Parse(questInfoElement.Element("Model2").Value);
                questInfo.Level = short.Parse(questInfoElement.Element("Level").Value);
                questInfo.Pos = int.Parse(questInfoElement.Element("Pos").Value);
                questInfo.Pos2 = int.Parse(questInfoElement.Element("Pos2").Value);
                questInfo.ManagedID = int.Parse(questInfoElement.Element("ManagedID").Value);
                questInfo.Active = short.Parse(questInfoElement.Element("Active").Value);
                questInfo.Unknown = byte.Parse(questInfoElement.Element("Unknown").Value);
                questInfo.Immediate = int.Parse(questInfoElement.Element("Immediate").Value);
                questInfo.ResetQuest = short.Parse(questInfoElement.Element("ResetQuest").Value);
                questInfo.Type = (eQUEST_TYPE)Enum.Parse(typeof(eQUEST_TYPE), questInfoElement.Element("Type").Value);
                questInfo.StartTargetType = int.Parse(questInfoElement.Element("StartTargetType").Value);
                questInfo.StartTargetID = int.Parse(questInfoElement.Element("StartTargetID").Value);
                questInfo.Target = int.Parse(questInfoElement.Element("Target").Value);
                questInfo.TargetValue = int.Parse(questInfoElement.Element("TargetValue").Value);
                questInfo.TitleTab = questInfoElement.Element("TitleTab").Value;
                questInfo.TitleText = questInfoElement.Element("TitleText").Value;
                questInfo.Body = questInfoElement.Element("Body").Value;
                questInfo.Simple = questInfoElement.Element("Simple").Value;
                questInfo.Helper = questInfoElement.Element("Helper").Value;
                questInfo.Process = questInfoElement.Element("Process").Value;
                questInfo.Complete = questInfoElement.Element("Complete").Value;
                questInfo.Expert = questInfoElement.Element("Expert").Value;
                questInfo.Itemgiven = int.Parse(questInfoElement.Element("Itemgiven").Value);
                questInfo.condition = int.Parse(questInfoElement.Element("condition").Value);
                questInfo.Goals = int.Parse(questInfoElement.Element("Goals").Value);
                questInfo.RewardNumber = int.Parse(questInfoElement.Element("RewardNumber").Value);

                XElement questItemsElement = questInfoElement.Element("QuestItems");
                if (questItemsElement != null)
                {
                    foreach (XElement questItemElement in questItemsElement.Elements("QuestItem"))
                    {
                        QuestItem questItem = new QuestItem();
                        questItem.Itemgiven = int.Parse(questItemElement.Element("Itemgiven").Value);
                        questItem.ItemgivenType = int.Parse(questItemElement.Element("ItemgivenType").Value);
                        questItem.ItemgivenAmount = int.Parse(questItemElement.Element("ItemgivenAmount").Value);
                        questInfo.questItems.Add(questItem);
                    }
                }

                XElement questConditionsElement = questInfoElement.Element("QuestConditions");
                if (questConditionsElement != null)
                {
                    foreach (XElement questConditionElement in questConditionsElement.Elements("QuestCondition"))
                    {
                        QuestCondition questCondition = new QuestCondition();
                        questCondition.ConditionType = int.Parse(questConditionElement.Element("ConditionType").Value);
                        questCondition.ConditionId = int.Parse(questConditionElement.Element("ConditionId").Value);
                        questCondition.ConditionCount = int.Parse(questConditionElement.Element("ConditionCount").Value);
                        questInfo.conditions.Add(questCondition);
                    }
                }

                XElement questGoalsElement = questInfoElement.Element("QuestGoals");
                if (questGoalsElement != null)
                {
                    foreach (XElement questGoalElement in questGoalsElement.Elements("QuestGoal"))
                    {
                        QuestGoals questGoal = new QuestGoals();
                        questGoal.GoalType = int.Parse(questGoalElement.Element("GoalType").Value);
                        questGoal.GoalId = int.Parse(questGoalElement.Element("GoalId").Value);
                        questGoal.GoalCount = int.Parse(questGoalElement.Element("GoalCount").Value);
                        questGoal.goalAmount = int.Parse(questGoalElement.Element("goalAmount").Value);
                        questGoal.CurTypeCount = int.Parse(questGoalElement.Element("CurTypeCount").Value);
                        questGoal.SubValue = int.Parse(questGoalElement.Element("SubValue").Value);
                        questGoal.SubValue1 = int.Parse(questGoalElement.Element("SubValue1").Value);
                        questInfo.QuestGoals.Add(questGoal);
                    }
                }

                XElement rewardQuantitiesElement = questInfoElement.Element("RewardQuantities");
                if (rewardQuantitiesElement != null)
                {
                    foreach (XElement rewardQuantityElement in rewardQuantitiesElement.Elements("RewardQuantity"))
                    {
                        RewardQuantity rewardQuantity = new RewardQuantity();
                        rewardQuantity.Reward = int.Parse(rewardQuantityElement.Element("Reward").Value);
                        rewardQuantity.RewardType = int.Parse(rewardQuantityElement.Element("RewardType").Value);

                        XElement questRewardMoneyElement = rewardQuantityElement.Element("QuestRewardMoney");
                        if (questRewardMoneyElement != null)
                        {
                            foreach (XElement questRewardMoneySubElement in questRewardMoneyElement.Elements("QuestRewardMoneyItem"))
                            {
                                QuestRewardMoney rewardMoney = new QuestRewardMoney();
                                rewardMoney.RewardMoney = int.Parse(questRewardMoneySubElement.Element("RewardMoney").Value);
                                rewardMoney.RewardUnk = int.Parse(questRewardMoneySubElement.Element("RewardUnk").Value);
                                rewardQuantity.QuestRewardMoney.Add(rewardMoney);
                            }
                        }

                        XElement questRewardItemsElement = rewardQuantityElement.Element("QuestRewardItems");
                        if (questRewardItemsElement != null)
                        {
                            foreach (XElement questRewardItemsSubElement in questRewardItemsElement.Elements("QuestRewardItemsItem"))
                            {
                                QuestRewardItems rewardItem = new QuestRewardItems();
                                rewardItem.RewardItem = int.Parse(questRewardItemsSubElement.Element("RewardItem").Value);
                                rewardItem.RewardAmount = int.Parse(questRewardItemsSubElement.Element("RewardAmount").Value);
                                rewardQuantity.QuestRewardItems.Add(rewardItem);
                            }
                        }

                        questInfo.rewardQuantities.Add(rewardQuantity);
                    }
                }

                XElement eventElement = questInfoElement.Element("Event");
                if (eventElement != null)
                {
                    questInfo.Event = eventElement.Elements("EventId").Select(x => int.Parse(x.Value)).ToArray();
                }

                questInfosList.Add(questInfo);
            }

            return questInfosList.ToArray();
        }

        public static void WriteQuestToBinary(string outputFile, QuestInfos[] questInfos)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(outputFile, FileMode.Create)))
            {
                writer.Write(questInfos.Length);
                foreach (var quest in questInfos)
                {
                    writer.Write(quest.UniqID);
                    writer.Write(quest.Model);
                    writer.Write(quest.Model2);
                    writer.Write(quest.Level);
                    writer.Write(quest.Pos);
                    writer.Write(quest.Pos2);
                    writer.Write(quest.ManagedID);
                    writer.Write(quest.Active);
                    writer.Write(quest.Unknown);
                    writer.Write(quest.Type.GetHashCode());
                    writer.Write(quest.StartTargetType);
                    writer.Write(quest.StartTargetID);
                    writer.Write(quest.Target);
                    writer.Write(quest.TargetValue);

                    for (int i = 0; i < 160 / 2; i++)
                    {
                        char c = i < quest.TitleTab.Length ? quest.TitleTab[i] : '\0';
                        writer.Write((ushort)c);
                    }

                    for (int i = 0; i < 160 / 2; i++)
                    {
                        char c = i < quest.TitleText.Length ? quest.TitleText[i] : '\0';
                        writer.Write((ushort)c);
                    }

                    for (int i = 0; i < 4096 / 2; i++)
                    {
                        char c = i < quest.Body.Length ? quest.Body[i] : '\0';
                        writer.Write((ushort)c);
                    }

                    for (int i = 0; i < 256 / 2; i++)
                    {
                        char c = i < quest.Simple.Length ? quest.Simple[i] : '\0';
                        writer.Write((ushort)c);
                    }

                    for (int i = 0; i < 1024 / 2; i++)
                    {
                        char c = i < quest.Helper.Length ? quest.Helper[i] : '\0';
                        writer.Write((ushort)c);
                    }

                    for (int i = 0; i < 640 / 2; i++)
                    {
                        char c = i < quest.Process.Length ? quest.Process[i] : '\0';
                        writer.Write((ushort)c);
                    }

                    for (int i = 0; i < 1400 / 2; i++)
                    {
                        char c = i < quest.Complete.Length ? quest.Complete[i] : '\0';
                        writer.Write((ushort)c);
                    }

                    for (int i = 0; i < 640 / 2; i++)
                    {
                        char c = i < quest.Expert.Length ? quest.Expert[i] : '\0';
                        writer.Write((ushort)c);
                    }

                    writer.Write(quest.Itemgiven); //items, given for quest
                    foreach (var questItem in quest.questItems)
                    {
                        writer.Write(questItem.Itemgiven);
                        writer.Write(questItem.ItemgivenType);
                        writer.Write(questItem.ItemgivenAmount);
                    }



                    writer.Write(quest.condition); //conditions for starting the quest
                    foreach (var questCondition in quest.conditions)
                    {
                        writer.Write(questCondition.ConditionType);
                        writer.Write(questCondition.ConditionId);
                        writer.Write(questCondition.ConditionCount);
                    }


                    writer.Write(quest.Goals); //quest goals
                    foreach (var questGoals in quest.QuestGoals)
                    {
                        writer.Write(questGoals.GoalType);
                        writer.Write(questGoals.GoalId);
                        writer.Write(questGoals.goalAmount); //?
                        writer.Write(questGoals.CurTypeCount);
                        writer.Write(questGoals.SubValue);
                        writer.Write(questGoals.SubValue1);
                    }



                    writer.Write(quest.RewardNumber); //reward

                    foreach (var quantity in quest.rewardQuantities)
                    {
                        writer.Write(quantity.Reward);
                        writer.Write(quantity.RewardType);
                        foreach (var money in quantity.QuestRewardMoney)
                        {
                            writer.Write(money.RewardMoney);
                            writer.Write(money.RewardUnk);
                        }
                        foreach (var item in quantity.QuestRewardItems)
                        {
                            writer.Write(item.RewardItem);
                            writer.Write(item.RewardAmount);
                        }
                    }

                    writer.Write(quest.Event.Length);
                    for (int x = 0; x < quest.Event.Length; x++)
                    {
                        writer.Write(quest.Event[x]);
                    }

                }
            }
        }
    }

    public class QuestItem
    {
        public int Itemgiven;
        public int ItemgivenType;
        public int ItemgivenAmount;
    }

    public class QuestCondition
    {
        public int ConditionType;
        public int ConditionId;
        public int ConditionCount;
    }

    public class QuestGoals
    {
        public int GoalType;
        public int GoalId;
        public int GoalCount;
        public int goalAmount;
        public int CurTypeCount;
        public int SubValue;
        public int SubValue1;
    }

    public class RewardQuantity
    {
        public int Reward;
        public int RewardType;
        public List<QuestRewardMoney> QuestRewardMoney = new();
        public List<QuestRewardItems> QuestRewardItems = new();
    }

    public class QuestRewardMoney
    {
        public int RewardMoney;
        public int RewardUnk;

    }
    public class QuestRewardItems
    {
        public int RewardItem;
        public int RewardAmount;
    }
}
