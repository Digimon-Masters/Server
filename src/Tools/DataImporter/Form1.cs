using DSO.DataImport.Processors;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using static DataImporterTool.Form1;

namespace DataImporterTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            var currentDir = Directory.GetCurrentDirectory();
            string directory;
            string[]? files;

#if DEBUG
            var cs = "Server=192.35.206.50,8589;Database=DSO;User Id=sa;Password=OkGXTc2Lm7H4Ap37KkzE;TrustServerCertificate=True";
#else
            var cs = txtConnectionString.Text;
#endif

            if (string.IsNullOrEmpty(cs))
            {
                MessageBox.Show($"Set the connection string.");
                return;
            }

            progressBar1.Value = 0;
            progressBar1.Maximum = 20;
            progressBar1.Step = 1;
            progressBar1.Visible = true;
            progressBar1.PerformStep();

            try
            {
                directory = currentDir + "\\XMLs\\EvoInfo";
                files = Directory.GetFiles(directory, "*.xml");
                EvoInfoImportProcessor.Import(cs, files);
                progressBar1.PerformStep();
                //
                //directory = currentDir + "\\XMLs\\SealInfo";
                //files = Directory.GetFiles(directory, "*.xml");
                //SealInfoImportProcessor.Import(cs, files);
                //progressBar1.PerformStep();
                //
                directory = currentDir + "\\XMLs\\DigimonBaseInfo";
                files = Directory.GetFiles(directory, "*.xml");
                DigimonBaseInfoImportProcessor.Import(cs, files);
                progressBar1.PerformStep();
                //
                //directory = currentDir + "\\XMLs\\MonsterBaseInfo";
                //files = Directory.GetFiles(directory, "*.xml");
                //MonsterBaseInfoImportProcessor.Import(cs, files);
                //progressBar1.PerformStep();
                //
                //directory = currentDir + "\\XMLs\\HatchInfo";
                //files = Directory.GetFiles(directory, "*.xml");
                //HatchInfoImportProcessor.Import(cs, files);
                //progressBar1.PerformStep();
                //
                //directory = currentDir + "\\XMLs\\CloneInfo";
                //files = Directory.GetFiles(directory, "*.xml");
                //CloneInfoImportProcessor.Import(cs, files);
                //progressBar1.PerformStep();
                //
                //directory = currentDir + "\\XMLs\\CloneValues";
                //files = Directory.GetFiles(directory, "*.xml");
                //CloneValueImportProcessor.Import(cs, files);
                //progressBar1.PerformStep();
                //
                //directory = currentDir + "\\XMLs\\Maps";
                //files = Directory.GetFiles(directory, "*.xml");
                //MapInfoImportProcessor.Import(cs, files);
                //progressBar1.PerformStep();
                //
                //directory = currentDir + "\\XMLs\\NPCs";
                //files = Directory.GetFiles(directory, "*.xml");
                //NpcInfoImportProcessor.Import(cs, files);
                //progressBar1.PerformStep();
                //
                //directory = currentDir + "\\XMLs\\Fruits";
                //files = Directory.GetFiles(directory, "*.xml");
                //FruitInfoImportProcessor.Import(cs, files);
                //progressBar1.PerformStep();

                directory = currentDir + "\\XMLs\\ItemInfo";
                files = Directory.GetFiles(directory, "*.xml");
                ItemInfoImportProcessor.Import(cs, files);
                progressBar1.PerformStep();
                //
                ////directory = currentDir + "\\XMLs\\QuestList";
                ////files = Directory.GetFiles(directory, "*.xml");
                ////QuestImport(cs, files);
                ////progressBar1.PerformStep();
                //

                //directory = currentDir + "\\XMLs\\SkillInfo";
                //files = Directory.GetFiles(directory, "*.xml");
                //SkillCodeImport(cs, files);
                //progressBar1.PerformStep();

                //directory = currentDir + "\\XMLs\\PortalInfo";
                //files = Directory.GetFiles(directory, "*.xml");
                //PortalImport(cs, files);
                //progressBar1.PerformStep();

                //directory = currentDir + "\\XMLs\\Summon";
                //files = Directory.GetFiles(directory, "*.xml");
                //SummonImport(cs, files);
                //progressBar1.PerformStep();

                //directory = currentDir + "\\XMLs\\SkillInfo";
                //files = Directory.GetFiles(directory, "*.xml");
                //var directory2 = currentDir + "\\XMLs\\DigimonBaseInfo";
                //var files2 = Directory.GetFiles(directory2, "*.xml");
                //DigimonSkillImport(cs, files, files2);
                //progressBar1.PerformStep();

                //directory = currentDir + "\\XMLs\\SkillInfo";
                //files = Directory.GetFiles(directory, "*.xml");
                //var directory1 = currentDir + "\\XMLs\\DigimonBaseInfo";
                //var files1 = Directory.GetFiles(directory1, "*.xml");
                //SkillImport(cs, files, files1);
                //progressBar1.PerformStep();

                //directory = currentDir + "\\XMLs\\MonsterSkill";
                //files = Directory.GetFiles(directory, "*.xml");
                //MonsterSkillInfoImportProcessor.Import(cs, files);
                //progressBar1.PerformStep();

                //directory = currentDir + "\\XMLs\\TamerSkill";
                //files = Directory.GetFiles(directory, "*.xml");
                //TamerSkillImport(cs, files);

                //directory = currentDir + "\\XMLs\\Buff";
                //files = Directory.GetFiles(directory, "*.xml");
                //BuffImport(cs, files);
                //progressBar1.PerformStep();

                //directory = currentDir + "\\XMLs\\Event";
                //files = Directory.GetFiles(directory, "*.xml");
                //MonthlyEventImport(cs, files);
                //progressBar1.PerformStep();

                //directory = currentDir + "\\XMLs\\Achievement";
                //files = Directory.GetFiles(directory, "*.xml");
                //AchievementImport(cs, files);
                //progressBar1.PerformStep();

                //directory = currentDir + "\\XMLs\\Coliseum";
                //files = Directory.GetFiles(directory, "*.xml");
                //NpcColiseumImport(cs, files);
                //progressBar1.PerformStep();

                //directory = currentDir + "\\XMLs\\ArenaReward";
                //files = Directory.GetFiles(directory, "*.xml");
                //ArenaDailyItemRewardsImport(cs, files);
                //progressBar1.PerformStep();


                //directory = currentDir + "\\XMLs\\AccessoryStatusInfo";
                //files = Directory.GetFiles(directory, "*.xml");
                //AccessoryStatusImport(cs, files);

                //directory = currentDir + "\\XMLs\\EvolutionArmor";
                //files = Directory.GetFiles(directory, "*.xml");
                //EvolutionArmorImport(cs, files);

                progressBar1.PerformStep();

                MessageBox.Show($"Data imported successfully!");

                progressBar1.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unhandled exception: {ex.Message} {ex.StackTrace}.");

                progressBar1.Visible = false;
            }
        }

        private static void QuestImport(string cs, string[] files)
        {
            var maindata = new QuestInfosList();

            foreach (var file in files)
            {
                var xmlDocument = File.ReadAllText(file);

                var serializer = new XmlSerializer(typeof(QuestInfosList));

                using (TextReader reader = new StringReader(xmlDocument))
                {
                    var newList = (QuestInfosList)serializer.Deserialize(reader);

                    maindata.Quests.AddRange(newList.Quests);
                }
            }

            var query = $@" IF NOT EXISTS (SELECT 1 FROM Asset.Quest WHERE QuestId = @QuestId)
                            BEGIN
                                INSERT INTO Asset.Quest
                                    (QuestId, QuestType, TargetType, TargetValue)
                                VALUES
                                    (@QuestId, @QuestType, @TargetType, @TargetValue);

                                SELECT SCOPE_IDENTITY();

                            END ELSE BEGIN SELECT 0 END;";

            var subquerySupply = $@"  INSERT INTO Asset.QuestSupply
                                    (ItemId, Amount, QuestId)
                                VALUES
                                    (@ItemId, @Amount, @QuestId);";

            var subqueryEvent = $@" INSERT INTO Asset.QuestEvent
                                        (EventId, QuestId)
                                    VALUES
                                        (@EventId, @QuestId);";

            var subqueryGoal = $@"  INSERT INTO Asset.QuestGoal
                                        (GoalType, GoalId, GoalAmount, CurTypeCount, SubValue, SubValueTwo, QuestId)
                                    VALUES
                                        (@GoalType, @GoalId, @GoalAmount, @CurTypeCount, @SubValue, @SubValueTwo, @QuestId);";

            var subqueryCondition = $@" INSERT INTO Asset.QuestCondition
                                            (ConditionType, ConditionId, ConditionCount, QuestId)
                                        VALUES
                                            (@ConditionType, @ConditionId, @ConditionCount, @QuestId);";

            var subqueryReward = $@"INSERT INTO Asset.QuestReward
                                        (Reward, RewardType, QuestId)
                                    VALUES
                                        (@Reward, @RewardType, @QuestId);
                                    SELECT SCOPE_IDENTITY();";

            var subqueryRewardObject = $@"  INSERT INTO Asset.QuestRewardObject
                                                (Reward, Amount, QuestRewardId)
                                            VALUES
                                                (@Reward, @Amount, @QuestRewardId);";

            var conn = new SqlConnection(cs);
            conn.Open();

            foreach (var item in maindata.Quests)
            {
                var command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("QuestId", item.QuestId);
                command.Parameters.AddWithValue("QuestType", item.QuestType);
                command.Parameters.AddWithValue("TargetType", item.Target);
                command.Parameters.AddWithValue("TargetValue", item.TargetValue);

                var questId = Convert.ToInt64(command.ExecuteScalar());

                if (questId > 0)
                {
                    var subConn = new SqlConnection(cs);
                    subConn.Open();
                    foreach (var subItem in item.QuestItems.QuestItem)
                    {
                        var subCommand = new SqlCommand(subquerySupply, subConn);
                        subCommand.Parameters.AddWithValue("ItemId", subItem.ItemType);
                        subCommand.Parameters.AddWithValue("Amount", subItem.Amount);
                        subCommand.Parameters.AddWithValue("QuestId", questId);
                        subCommand.ExecuteNonQuery();
                    }
                    subConn.Close();

                    subConn = new SqlConnection(cs);
                    subConn.Open();
                    foreach (var subItem in item.QuestEvents.EventIds)
                    {
                        var subCommand = new SqlCommand(subqueryEvent, subConn);
                        subCommand.Parameters.AddWithValue("EventId", subItem);
                        subCommand.Parameters.AddWithValue("QuestId", questId);
                        subCommand.ExecuteNonQuery();
                    }
                    subConn.Close();

                    subConn = new SqlConnection(cs);
                    subConn.Open();
                    foreach (var subItem in item.QuestGoals.QuestGoal)
                    {
                        var subCommand = new SqlCommand(subqueryGoal, subConn);
                        subCommand.Parameters.AddWithValue("GoalType", subItem.GoalType);
                        subCommand.Parameters.AddWithValue("GoalId", subItem.GoalId);
                        subCommand.Parameters.AddWithValue("GoalAmount", subItem.GoalAmount);
                        subCommand.Parameters.AddWithValue("CurTypeCount", subItem.CurTypeCount);
                        subCommand.Parameters.AddWithValue("SubValue", subItem.SubValue);
                        subCommand.Parameters.AddWithValue("SubValueTwo", subItem.SubValueTwo);
                        subCommand.Parameters.AddWithValue("QuestId", questId);
                        subCommand.ExecuteNonQuery();
                    }
                    subConn.Close();

                    subConn = new SqlConnection(cs);
                    subConn.Open();
                    foreach (var subItem in item.QuestConditions.QuestCondition)
                    {
                        var subCommand = new SqlCommand(subqueryCondition, subConn);
                        subCommand.Parameters.AddWithValue("ConditionType", subItem.ConditionType);
                        subCommand.Parameters.AddWithValue("ConditionId", subItem.ConditionId);
                        subCommand.Parameters.AddWithValue("ConditionCount", subItem.ConditionCount);
                        subCommand.Parameters.AddWithValue("QuestId", questId);
                        subCommand.ExecuteNonQuery();
                    }
                    subConn.Close();

                    subConn = new SqlConnection(cs);
                    subConn.Open();
                    foreach (var subItem in item.QuestRewards.QuestReward)
                    {
                        var subCommand = new SqlCommand(subqueryReward, subConn);
                        subCommand.Parameters.AddWithValue("Reward", subItem.Reward);
                        subCommand.Parameters.AddWithValue("RewardType", subItem.RewardType);
                        subCommand.Parameters.AddWithValue("QuestId", questId);
                        var questRewardId = Convert.ToInt64(subCommand.ExecuteScalar());

                        var thirdConn = new SqlConnection(cs);
                        thirdConn.Open();
                        if (subItem.RewardType == 0)
                        {
                            foreach (var thirdItem in subItem.QuestRewardMoney.QuestRewardMoneyItems)
                            {
                                var thirdCommand = new SqlCommand(subqueryRewardObject, thirdConn);
                                thirdCommand.Parameters.AddWithValue("Reward", 0);
                                thirdCommand.Parameters.AddWithValue("Amount", thirdItem.Amount);
                                thirdCommand.Parameters.AddWithValue("QuestRewardId", questRewardId);
                                thirdCommand.ExecuteNonQuery();
                            }
                        }
                        else if (subItem.RewardType == 1)
                        {
                            foreach (var thirdItem in subItem.QuestRewardItems.QuestRewardItemsItems)
                            {
                                var thirdCommand = new SqlCommand(subqueryRewardObject, thirdConn);
                                thirdCommand.Parameters.AddWithValue("Reward", 1);
                                thirdCommand.Parameters.AddWithValue("Amount", thirdItem.ItemIdOrExp);
                                thirdCommand.Parameters.AddWithValue("QuestRewardId", questRewardId);
                                thirdCommand.ExecuteNonQuery();
                            }
                        }
                        else if (subItem.RewardType == 2)
                        {
                            foreach (var thirdItem in subItem.QuestRewardItems.QuestRewardItemsItems)
                            {
                                var thirdCommand = new SqlCommand(subqueryRewardObject, thirdConn);
                                thirdCommand.Parameters.AddWithValue("Reward", thirdItem.ItemIdOrExp);
                                thirdCommand.Parameters.AddWithValue("Amount", thirdItem.Amount);
                                thirdCommand.Parameters.AddWithValue("QuestRewardId", questRewardId);
                                thirdCommand.ExecuteNonQuery();
                            }
                        }
                        thirdConn.Close();
                    }
                    subConn.Close();
                }
            }
            conn.Close();
        }

        private static void SkillCodeImport(string cs, string[] files)
        {
            var maindata = new SkillCodeInfoList();

            foreach (var file in files)
            {
                var xmlDocument = File.ReadAllText(file);

                var serializer = new XmlSerializer(typeof(SkillCodeInfoList));

                using (TextReader reader = new StringReader(xmlDocument))
                {
                    var newList = (SkillCodeInfoList)serializer.Deserialize(reader);

                    maindata.SkillData.AddRange(newList.SkillData);
                }
            }

            var insertSkillCode = @"    IF NOT EXISTS (SELECT 1 FROM Asset.SkillCode WHERE SkillCode = @SkillCode)
                              BEGIN
                                  INSERT INTO [Asset].[SkillCode]
                                      ([SkillCode]
                                      ,[Comment])
                                  VALUES
                                     (@SkillCode
                                     ,@Comment);

                                  SELECT SCOPE_IDENTITY();
                              END ELSE BEGIN SELECT 0; END;";

            var insertSkillCodeApply = @"   INSERT INTO [Asset].[SkillCodeApply]
                                      ([Type]
                                     ,[Attribute]
                                     ,[Value]
                                     ,[Chance]
                                     ,[AdditionalValue]
                                     ,[IncreaseValue]
                                     ,[SkillCodeAssetId])
                                  VALUES
                                      (@Type
                                      ,@Attribute
                                      ,@Value
                                      ,@Chance
                                      ,@AdditionalValue
                                      ,@IncreaseValue
                                     ,@SkillCodeAssetId)";

            using (var conn = new SqlConnection(cs))
            {
                conn.Open();
                foreach (var skillCode in maindata.SkillData)
                {
                    var command = new SqlCommand(insertSkillCode, conn);
                    command.Parameters.AddWithValue("SkillCode", skillCode.SkillCode);

                    command.Parameters.AddWithValue("Comment", string.Empty);


                    var skillCodeId = Convert.ToInt64(command.ExecuteScalar());

                    if (skillCodeId > 0)
                    {
                        using (var subConn = new SqlConnection(cs))
                        {

                            subConn.Open();
                            for (int i = 0; i < skillCode.skillApply.IncreaseApplies.Count; i++)
                            {

                                var subCommand = new SqlCommand(insertSkillCodeApply, subConn);
                                subCommand.Parameters.AddWithValue("Type", skillCode.skillApply.IncreaseApplies[i].S_nID);
                                subCommand.Parameters.AddWithValue("Attribute", skillCode.skillApply.IncreaseApplies[i].S_nA);
                                subCommand.Parameters.AddWithValue("Value", skillCode.skillApply.IncreaseApplies[i].S_nB);
                                subCommand.Parameters.AddWithValue("Chance", skillCode.skillApply.IncreaseApplies[i].S_nInvoke_Rate / 100);
                                subCommand.Parameters.AddWithValue("AdditionalValue", skillCode.skillApply.IncreaseApplies[i].S_nC);
                                subCommand.Parameters.AddWithValue("IncreaseValue", skillCode.skillApply.IncreaseApplies[i].S_nIncrease_B_Point);
                                subCommand.Parameters.AddWithValue("SkillCodeAssetId", skillCodeId);
                                subCommand.ExecuteNonQuery();

                            }

                            subConn.Close();
                        }

                    }
                }
                conn.Close();
            }

        }
        private static void SkillImport(string cs, string[] files, string[] files1)
        {
            var newDataList = new DigimonBaseInfoList();

            foreach (var file in files1)
            {
                var xmlDocument = File.ReadAllText(file);

                var serializer = new XmlSerializer(typeof(DigimonBaseInfoList));

                using (TextReader reader = new StringReader(xmlDocument))
                {
                    var newList = (DigimonBaseInfoList)serializer.Deserialize(reader);

                    newDataList.DigimonBaseList.AddRange(newList.DigimonBaseList);
                }
            }

            var maindata = new SkillInfoList();

            foreach (var file in files)
            {

                var xmlDocument = File.ReadAllText(file);

                var serializer = new XmlSerializer(typeof(SkillInfoList));

                using (TextReader reader = new StringReader(xmlDocument))
                {
                    var newList = (SkillInfoList)serializer.Deserialize(reader);

                    maindata.SkillData.AddRange(newList.SkillData);
                }
            }

            var insertSkillCode = @"    IF NOT EXISTS (SELECT 1 FROM Asset.DigimonSkill WHERE SkillId = @SkillId)
                              BEGIN
                                  INSERT INTO [Asset].[DigimonSkill]
                                      ([SkillId]
                                      ,[Slot]
                                      ,[Type])
                                  VALUES

                                     (@SkillId
                                      ,@Slot
                                      ,@Type);

                                  SELECT SCOPE_IDENTITY();
                              END ELSE BEGIN SELECT 0; END;";

            var insertSkillCodeApply = @"  
                                          INSERT INTO [Asset].[SkillInfo]
                                          (
                                             [SkillId]
                                            ,[Name]
                                            ,[DSUSage]
                                            ,[HPUSage]
                                            ,[Value]
                                            ,[CastingTime]
                                            ,[Cooldown]
                                            ,[MaxLevel]
                                            ,[RequiredPoints]
                                            ,[Target]
                                            ,[FamilyType]
                                            ,[AreaOfEffect]
                                            ,[AoEMinDamage]
                                            ,[AoEMaxDamage]
                                            ,[Range]
                                            ,[UnlockLevel]
                                            ,[MemoryChips]
                                            ,[FirstConditionCode]
                                            ,[SecondConditionCode]
                                            ,[ThirdConditionCode]
                                            ,[Type]
                                            ,[Description]
                                          )
                                          VALUES
                                          (
                                             @SkillId
                                            ,@Name
                                            ,@DSUSage
                                            ,@HPUSage
                                            ,@Value
                                            ,@CastingTime
                                            ,@Cooldown
                                            ,@MaxLevel
                                            ,@RequiredPoints
                                            ,@Target
                                            ,@FamilyType
                                            ,@AreaOfEffect
                                            ,@AoEMinDamage
                                            ,@AoEMaxDamage
                                            ,@Range
                                            ,@UnlockLevel
                                            ,@MemoryChips
                                            ,@FirstConditionCode
                                            ,@SecondConditionCode
                                            ,@ThirdConditionCode
                                            ,@Type
                                            ,@Description
                                          );";

            using (var conn = new SqlConnection(cs))
            {

                foreach (var skillCode in maindata.SkillData)
                {
                    conn.Open();

                    if (newDataList.DigimonBaseList.FirstOrDefault(x => x.Skill1 == skillCode.SkillId || x.Skill2 == skillCode.SkillId || x.Skill3 == skillCode.SkillId || x.Skill4 == skillCode.SkillId) == null)
                    {
                        var command = new SqlCommand(insertSkillCode, conn);
                        command.Parameters.AddWithValue("SkillId", skillCode.SkillId);
                        command.Parameters.AddWithValue("Slot", 0);
                        command.Parameters.AddWithValue("Type", 0);

                        command.ExecuteNonQuery();



                        using (var subConn = new SqlConnection(cs))
                        {

                            subConn.Open();

                            var subCommand = new SqlCommand(insertSkillCodeApply, subConn);
                            subCommand.Parameters.AddWithValue("SkillId", skillCode.SkillId);
                            subCommand.Parameters.AddWithValue("Name", skillCode.Name);
                            subCommand.Parameters.AddWithValue("DSUsage", skillCode.DSUsage);
                            subCommand.Parameters.AddWithValue("HPUsage", skillCode.HPUsage);
                            subCommand.Parameters.AddWithValue("Value", 0);


                            subCommand.Parameters.AddWithValue("CastingTime", 0);
                            subCommand.Parameters.AddWithValue("Cooldown", skillCode.Cooldown);
                            subCommand.Parameters.AddWithValue("MaxLevel", skillCode.MaxLevel);
                            subCommand.Parameters.AddWithValue("RequiredPoints", skillCode.RequiredPoints);
                            subCommand.Parameters.AddWithValue("Target", skillCode.Target);
                            subCommand.Parameters.AddWithValue("AreaOfEffect", (int)skillCode.AreaOfEffect);
                            subCommand.Parameters.AddWithValue("AoEMinDamage", (int)skillCode.AoEMinDamage);
                            subCommand.Parameters.AddWithValue("AoEMaxDamage", (int)skillCode.AoEMaxnDamage);
                            subCommand.Parameters.AddWithValue("Range", skillCode.Range);
                            subCommand.Parameters.AddWithValue("UnlockLevel", skillCode.UnlockLevel);
                            subCommand.Parameters.AddWithValue("MemoryChips", 0);
                            subCommand.Parameters.AddWithValue("FirstConditionCode", skillCode.skillApply.IncreaseApplies[0].S_nB);
                            subCommand.Parameters.AddWithValue("SecondConditionCode", skillCode.skillApply.IncreaseApplies[1].S_nB);
                            subCommand.Parameters.AddWithValue("ThirdConditionCode", skillCode.skillApply.IncreaseApplies[2].S_nB);

                            subCommand.Parameters.AddWithValue("Type", 0);
                            subCommand.Parameters.AddWithValue("Slot", 0);

                            subCommand.Parameters.AddWithValue("Description", string.Empty);
                            subCommand.Parameters.AddWithValue("FamilyType", skillCode.FamilyType);
                            subCommand.ExecuteNonQuery();

                            subConn.Close();
                        }

                    }

                    conn.Close();
                }
            }
        }
        private static void DigimonSkillImport(string cs, string[] files, string[] files1)
        {
            var newDataList = new DigimonBaseInfoList();

            foreach (var file in files1)
            {
                var xmlDocument = File.ReadAllText(file);

                var serializer = new XmlSerializer(typeof(DigimonBaseInfoList));

                using (TextReader reader = new StringReader(xmlDocument))
                {
                    var newList = (DigimonBaseInfoList)serializer.Deserialize(reader);

                    newDataList.DigimonBaseList.AddRange(newList.DigimonBaseList);
                }
            }

            var maindata = new SkillInfoList();

            foreach (var file in files)
            {

                var xmlDocument = File.ReadAllText(file);

                var serializer = new XmlSerializer(typeof(SkillInfoList));

                using (TextReader reader = new StringReader(xmlDocument))
                {
                    var newList = (SkillInfoList)serializer.Deserialize(reader);

                    maindata.SkillData.AddRange(newList.SkillData);
                }
            }

            var insertSkillCode = @"    
                                  INSERT INTO [Asset].[DigimonSkill]
                                      ([SkillId]
                                      ,[Slot]
                                      ,[Type])
                                  VALUES

                                     (@SkillId
                                      ,@Slot
                                      ,@Type);";

            var insertSkillCodeApply = @"  
                                          INSERT INTO [Asset].[SkillInfo]
                                          (
                                             [SkillId]
                                            ,[Name]
                                            ,[DSUSage]
                                            ,[HPUSage]
                                            ,[Value]
                                            ,[CastingTime]
                                            ,[Cooldown]
                                            ,[MaxLevel]
                                            ,[RequiredPoints]
                                            ,[Target]
                                            ,[FamilyType]
                                            ,[AreaOfEffect]
                                            ,[AoEMinDamage]
                                            ,[AoEMaxDamage]
                                            ,[Range]
                                            ,[UnlockLevel]
                                            ,[MemoryChips]
                                            ,[FirstConditionCode]
                                            ,[SecondConditionCode]
                                            ,[ThirdConditionCode]
                                            ,[Type]
                                            ,[Description]
                                          )
                                          VALUES
                                          (
                                             @SkillId
                                            ,@Name
                                            ,@DSUSage
                                            ,@HPUSage
                                            ,@Value
                                            ,@CastingTime
                                            ,@Cooldown
                                            ,@MaxLevel
                                            ,@RequiredPoints
                                            ,@Target
                                            ,@FamilyType
                                            ,@AreaOfEffect
                                            ,@AoEMinDamage
                                            ,@AoEMaxDamage
                                            ,@Range
                                            ,@UnlockLevel
                                            ,@MemoryChips
                                            ,@FirstConditionCode
                                            ,@SecondConditionCode
                                            ,@ThirdConditionCode
                                            ,@Type
                                            ,@Description
                                          );";

            using (var conn = new SqlConnection(cs))
            {

                foreach (var target in newDataList.DigimonBaseList)
                {
                    if (target.Type == 41019)
                    {

                    }
                    conn.Open();

                    var slot = -1;

                    for (int i = 1; i <= 4; i++) // Loop para as quatro habilidades
                    {
                        slot++;

                        int skillId = 0; // Variável para armazenar o SkillId atual
                        switch (i)
                        {
                            case 1:
                                skillId = target.Skill1;
                                break;
                            case 2:
                                skillId = target.Skill2;
                                break;
                            case 3:
                                skillId = target.Skill3;
                                break;
                            case 4:
                                skillId = target.Skill4;
                                break;
                        }

                        if (skillId > 0)
                        {
                            foreach (var skillCode in maindata.SkillData.Where(x => x.SkillId == skillId))
                            {


                                var command = new SqlCommand(insertSkillCode, conn);
                                command.Parameters.AddWithValue("SkillId", skillCode.SkillId);


                                command.Parameters.AddWithValue("Type", target.Type);


                                command.Parameters.AddWithValue("Slot", slot);


                                command.ExecuteNonQuery();



                                using (var subConn = new SqlConnection(cs))
                                {

                                    subConn.Open();

                                    var subCommand = new SqlCommand(insertSkillCodeApply, subConn);
                                    subCommand.Parameters.AddWithValue("SkillId", skillCode.SkillId);
                                    subCommand.Parameters.AddWithValue("Name", skillCode.Name);
                                    subCommand.Parameters.AddWithValue("DSUsage", skillCode.DSUsage);
                                    subCommand.Parameters.AddWithValue("HPUsage", skillCode.HPUsage);
                                    subCommand.Parameters.AddWithValue("Value", 0);

                                    subCommand.Parameters.AddWithValue("CastingTime", 0);
                                    subCommand.Parameters.AddWithValue("Cooldown", skillCode.Cooldown);
                                    subCommand.Parameters.AddWithValue("MaxLevel", skillCode.MaxLevel);
                                    subCommand.Parameters.AddWithValue("RequiredPoints", skillCode.RequiredPoints);
                                    subCommand.Parameters.AddWithValue("Target", skillCode.Target);
                                    subCommand.Parameters.AddWithValue("AreaOfEffect", skillCode.AreaOfEffect);
                                    subCommand.Parameters.AddWithValue("AoEMinDamage", skillCode.AoEMinDamage);
                                    subCommand.Parameters.AddWithValue("AoEMaxDamage", skillCode.AoEMaxnDamage);
                                    subCommand.Parameters.AddWithValue("Range", skillCode.Range);
                                    subCommand.Parameters.AddWithValue("UnlockLevel", skillCode.UnlockLevel);
                                    subCommand.Parameters.AddWithValue("MemoryChips", 0);
                                    subCommand.Parameters.AddWithValue("FirstConditionCode", skillCode.skillApply.IncreaseApplies[0].S_nB);
                                    subCommand.Parameters.AddWithValue("SecondConditionCode", skillCode.skillApply.IncreaseApplies[1].S_nB);
                                    subCommand.Parameters.AddWithValue("ThirdConditionCode", skillCode.skillApply.IncreaseApplies[2].S_nB);

                                    subCommand.Parameters.AddWithValue("Type", target.Type);
                                    subCommand.Parameters.AddWithValue("Slot", slot);

                                    subCommand.Parameters.AddWithValue("Description", string.Empty);
                                    subCommand.Parameters.AddWithValue("FamilyType", skillCode.FamilyType);
                                    subCommand.ExecuteNonQuery();

                                    subConn.Close();
                                }

                            }
                        }
                    }

                    conn.Close(); // Lembre-se de fechar a conexão fora do loop
                }
            }
        }
        private static void TamerSkillImport(string cs, string[] files)
        {


            var maindata = new TamerSkillList();

            foreach (var file in files)
            {


                var xmlDocument = File.ReadAllText(file);

                var serializer = new XmlSerializer(typeof(TamerSkillList));

                using (TextReader reader = new StringReader(xmlDocument))
                {
                    var newList = (TamerSkillList)serializer.Deserialize(reader);

                    maindata.Skills.AddRange(newList.Skills);
                }
            }

            var insertTamerSkill = @"
                  IF NOT EXISTS (SELECT 1 FROM Asset.TamerSkill WHERE SkillId = @SkillId)
                  BEGIN
                      INSERT INTO Asset.TamerSkill
                          ([SkillId]
                          ,[SkillCode]
                          ,[Duration])
                      VALUES
                          (@SkillId
                          ,@SkillCode
                          ,@Duration);
                
                      SELECT SCOPE_IDENTITY();
                  END
                  ELSE
                  BEGIN
                      SELECT 0;
                  END;
                ";


            using (var conn = new SqlConnection(cs))
            {

                foreach (var skillCode in maindata.Skills)
                {
                    conn.Open();

                    var command = new SqlCommand(insertTamerSkill, conn);
                    command.Parameters.AddWithValue("SkillId", skillCode.SkillId);
                    command.Parameters.AddWithValue("SkillCode", skillCode.SkillCode);
                    command.Parameters.AddWithValue("Duration", skillCode.Duration);
                    command.ExecuteNonQuery();


                    conn.Close();
                }

            }
        }

        private static void BuffImport(string cs, string[] files)
        {
            var maindata = new BuffDataList();

            foreach (var file in files)
            {
                var xmlDocument = File.ReadAllText(file);
                var serializer = new XmlSerializer(typeof(BuffDataList));

                using (TextReader reader = new StringReader(xmlDocument))
                {
                    var newList = (BuffDataList)serializer.Deserialize(reader);
                    maindata.Buffs.AddRange(newList.Buffs);
                }
            }

            var insertBuff = @"
                                IF NOT EXISTS (SELECT 1 FROM Asset.Buff WHERE BuffId = @BuffId)
                                BEGIN
                                    INSERT INTO Asset.Buff
                                        ([BuffId]
                                        ,[Name]
                                        ,[SkillCode]
                                        ,[MinLevel]
                                        ,[ConditionLevel]
                                        ,[Class]
                                        ,[Type]
                                        ,[LifeType]
                                        ,[TimeType]
                                        ,[DigimonSkillCode])
                                    VALUES
                                        (@BuffId
                                        ,@Name
                                        ,@SkillCode
                                        ,@MinLevel
                                        ,@ConditionLevel
                                        ,@Class
                                        ,@Type
                                        ,@LifeType
                                        ,@TimeType
                                        ,@DigimonSkillCode);
                          
                                    SELECT SCOPE_IDENTITY();
                                END
                                ELSE
                                BEGIN
                                    SELECT 0;
                                END;
                            ";

            using (var conn = new SqlConnection(cs))
            {
                conn.Open();

                foreach (var buffData in maindata.Buffs)
                {
                    var command = new SqlCommand(insertBuff, conn);
                    command.Parameters.AddWithValue("BuffId", buffData.BuffId);
                    command.Parameters.AddWithValue("Name", buffData.Name);
                    command.Parameters.AddWithValue("SkillCode", buffData.SkillCode);
                    command.Parameters.AddWithValue("MinLevel", buffData.MinLevel);
                    command.Parameters.AddWithValue("ConditionLevel", buffData.ConditionLevel);
                    command.Parameters.AddWithValue("Class", buffData.Class);
                    command.Parameters.AddWithValue("Type", buffData.Type);
                    command.Parameters.AddWithValue("LifeType", buffData.LifeType);
                    command.Parameters.AddWithValue("TimeType", buffData.TimeType);
                    command.Parameters.AddWithValue("DigimonSkillCode", buffData.DigimonSkillCode);
                    command.ExecuteNonQuery();
                }

                conn.Close();
            }

        }

        private static void MonthlyEventImport(string cs, string[] files)
        {


            var maindata = new MonthlyEvents();

            foreach (var file in files)
            {


                var xmlDocument = File.ReadAllText(file);

                var serializer = new XmlSerializer(typeof(MonthlyEvents));

                using (TextReader reader = new StringReader(xmlDocument))
                {
                    var newList = (MonthlyEvents)serializer.Deserialize(reader);

                    maindata.Events.AddRange(newList.Events);
                }
            }

            var insertMonthlyEvent = @"
                  IF NOT EXISTS (SELECT 1 FROM Asset.MonthlyEvent WHERE CurrentDay = @CurrentDay)
                  BEGIN
                      INSERT INTO Asset.MonthlyEvent
                          ([CurrentDay]
                          ,[ItemId]
                          ,[ItemCount])
                      VALUES
                          (@CurrentDay
                          ,@ItemId
                          ,@ItemCount);
                
                      SELECT SCOPE_IDENTITY();
                  END
                  ELSE
                  BEGIN
                      SELECT 0;
                  END;
                ";


            using (var conn = new SqlConnection(cs))
            {
                var day = 0;

                foreach (var events in maindata.Events)
                {
                    foreach (var item in events.MonthlyItems.Items)
                    {

                        day++;

                        conn.Open();

                        var command = new SqlCommand(insertMonthlyEvent, conn);
                        command.Parameters.AddWithValue("CurrentDay", day);
                        command.Parameters.AddWithValue("ItemId", item.ItemId);
                        command.Parameters.AddWithValue("ItemCount", item.ItemCount);
                        command.ExecuteNonQuery();


                        conn.Close();
                    }
                }

            }
        }
        private static void AchievementImport(string cs, string[] files)
        {


            var maindata = new AchieveSINFOs();

            foreach (var file in files)
            {


                var xmlDocument = File.ReadAllText(file);

                var serializer = new XmlSerializer(typeof(AchieveSINFOs));

                using (TextReader reader = new StringReader(xmlDocument))
                {
                    var newList = (AchieveSINFOs)serializer.Deserialize(reader);

                    maindata.AchieveSINFO.AddRange(newList.AchieveSINFO);
                }
            }

            var insertMonthlyEvent = @"
                  IF NOT EXISTS (SELECT 1 FROM Asset.Achievement WHERE BuffId = @BuffId)
                  BEGIN
                      INSERT INTO Asset.Achievement
                          ([QuestId]
                          ,[Type]
                          ,[BuffId])
                      VALUES
                          (@QuestId
                          ,@Type
                          ,@BuffId);
                
                      SELECT SCOPE_IDENTITY();
                  END
                  ELSE
                  BEGIN
                      SELECT 0;
                  END;
                ";


            using (var conn = new SqlConnection(cs))
            {
                foreach (var achieve in maindata.AchieveSINFO)
                {


                    conn.Open();

                    var command = new SqlCommand(insertMonthlyEvent, conn);
                    command.Parameters.AddWithValue("QuestId", achieve.QuestID);
                    command.Parameters.AddWithValue("Type", 0);
                    command.Parameters.AddWithValue("BuffId", achieve.BuffCode);
                    command.ExecuteNonQuery();


                    conn.Close();

                }

            }
        }
        private static void SummonImport(string cs, string[] files)
        {
            var maindata = new SummonList();

            foreach (var file in files)
            {
                var xmlDocument = File.ReadAllText(file);

                var serializer = new XmlSerializer(typeof(SummonList));

                using (TextReader reader = new StringReader(xmlDocument))
                {
                    var newList = (SummonList)serializer.Deserialize(reader);

                    maindata.SummonDTOs.AddRange(newList.SummonDTOs);
                }
            }

            using (SqlConnection connection = new SqlConnection(cs))
            {
                foreach (var summond in maindata.SummonDTOs)
                {
                    connection.Open();

                    long summonId = 0;

                    string insertSummonQuery = @"INSERT INTO Config.Summon (ItemId, Maps) VALUES (@ItemId, @Maps); SELECT SCOPE_IDENTITY();";
                    SqlCommand insertSummonCommand = new SqlCommand(insertSummonQuery, connection);

                    // Supondo que summon.ItemId é do tipo int
                    insertSummonCommand.Parameters.AddWithValue("@ItemId", summond.ItemId);

                    List<string> mapValues = summond.Maps.Map.Select(map => map.ToString()).ToList();

                    string resultado = string.Join(", ", mapValues);

                    // Adicionando o parâmetro @Maps como NVARCHAR
                    insertSummonCommand.Parameters.Add("@Maps", SqlDbType.NVarChar).Value = resultado;

                    // Executando o comando e obtendo o ID inserido
                    object result = insertSummonCommand.ExecuteScalar();

                    // Verifique se a consulta retornou um valor antes de tentar convertê-lo para long
                    if (result != null && result != DBNull.Value)
                    {
                        summonId = Convert.ToInt64(result);
                        // Faça o que precisa ser feito com o summonId aqui
                    }
                    else
                    {
                        // Lidar com um cenário onde não foi possível obter o ID inserido
                    }

                    foreach (var summont in summond.SummonedMobs)
                    {




                        foreach (var summonedMob in summont.SummonMobs)
                        {
                            string insertSummonedMobQuery = @"
                                                         INSERT INTO config.SummonMob (Type, Duration, Amount, Model, Name, Level, ViewRange, HuntRange, Class,ReactionType, [Attribute], [Element], 
                                                             Family1, Family2, Family3, ASValue, ARValue, ATValue, BLValue, CTValue, DEValue, DSValue, EVValue, HPValue, HTValue, MSValue, WSValue, SummonDTOId)
                                                         VALUES (
                                                             @Type, @Duration, @Amount, @Model, @Name, @Level, @ViewRange, @HuntRange,@Class, @ReactionType, @Attribute, @Element, 
                                                             @Family1, @Family2, @Family3, @ASValue, @ARValue, @ATValue, @BLValue, @CTValue, @DEValue, @DSValue, @EVValue, @HPValue, 
                                                             @HTValue, @MSValue, @WSValue, @SummonDTOId
                                                         ); SELECT SCOPE_IDENTITY();";

                            SqlCommand insertSummonedMobCommand = new SqlCommand(insertSummonedMobQuery, connection);
                            insertSummonedMobCommand.Parameters.AddWithValue("@Type", summonedMob.Type);
                            insertSummonedMobCommand.Parameters.AddWithValue("@Duration", summonedMob.Duration);
                            insertSummonedMobCommand.Parameters.AddWithValue("@Amount", summonedMob.Amount);
                            insertSummonedMobCommand.Parameters.AddWithValue("@Model", summonedMob.Model);
                            insertSummonedMobCommand.Parameters.AddWithValue("@Name", "Test");
                            insertSummonedMobCommand.Parameters.AddWithValue("@Level", summonedMob.Level);
                            insertSummonedMobCommand.Parameters.AddWithValue("@ViewRange", summonedMob.ViewRange);
                            insertSummonedMobCommand.Parameters.AddWithValue("@HuntRange", summonedMob.HuntRange);
                            insertSummonedMobCommand.Parameters.AddWithValue("@ReactionType", summonedMob.ReactionType);
                            insertSummonedMobCommand.Parameters.AddWithValue("@Attribute", summonedMob.Attribute);
                            insertSummonedMobCommand.Parameters.AddWithValue("@Element", summonedMob.Element);
                            insertSummonedMobCommand.Parameters.AddWithValue("@Family1", summonedMob.Family1);
                            insertSummonedMobCommand.Parameters.AddWithValue("@Family2", summonedMob.Family2);
                            insertSummonedMobCommand.Parameters.AddWithValue("@Family3", summonedMob.Family3);
                            insertSummonedMobCommand.Parameters.AddWithValue("@ASValue", summonedMob.ASValue);
                            insertSummonedMobCommand.Parameters.AddWithValue("@ARValue", summonedMob.ARValue);
                            insertSummonedMobCommand.Parameters.AddWithValue("@ATValue", summonedMob.ATValue);
                            insertSummonedMobCommand.Parameters.AddWithValue("@BLValue", summonedMob.BLValue);
                            insertSummonedMobCommand.Parameters.AddWithValue("@CTValue", summonedMob.CTValue);
                            insertSummonedMobCommand.Parameters.AddWithValue("@DEValue", summonedMob.DEValue);
                            insertSummonedMobCommand.Parameters.AddWithValue("@DSValue", summonedMob.DSValue);
                            insertSummonedMobCommand.Parameters.AddWithValue("@EVValue", summonedMob.EVValue);
                            insertSummonedMobCommand.Parameters.AddWithValue("@HPValue", summonedMob.HPValue);
                            insertSummonedMobCommand.Parameters.AddWithValue("@HTValue", summonedMob.HTValue);
                            insertSummonedMobCommand.Parameters.AddWithValue("@MSValue", summonedMob.MSValue);
                            insertSummonedMobCommand.Parameters.AddWithValue("@WSValue", summonedMob.WSValue);
                            insertSummonedMobCommand.Parameters.AddWithValue("@Class", summonedMob.Class);
                            insertSummonedMobCommand.Parameters.AddWithValue("@SummonDTOId", summonId);

                            object result1 = insertSummonedMobCommand.ExecuteScalar();

                            var MobId = Convert.ToInt64(result1);


                            string insertDropRewardQuery = @"INSERT INTO config.SummonMobDropReward (MinAmount, MaxAmount,MobId) VALUES (@MinAmount, @MaxAmount, @MobId);";
                            SqlCommand insertDropRewardCommand = new SqlCommand(insertDropRewardQuery, connection);
                            insertDropRewardCommand.Parameters.AddWithValue("@MinAmount", summonedMob.DropReward.MinAmount);
                            insertDropRewardCommand.Parameters.AddWithValue("@MaxAmount", summonedMob.DropReward.MaxAmount);
                            insertDropRewardCommand.Parameters.AddWithValue("@MobId", MobId);
                            insertDropRewardCommand.ExecuteScalar();
                            SqlCommand getIdCommand = new SqlCommand("SELECT IDENT_CURRENT('config.SummonMobDropReward')", connection);
                            long DropRewardId = Convert.ToInt64(getIdCommand.ExecuteScalar());

                            foreach (SummonMobItemDropDTO itemDrop in summonedMob.DropReward.Drops.SummonMobItemDrop)
                            {
                                string insertItemDropQuery = @"INSERT INTO config.SummonMobItemDropReward (ItemId, MinAmount, MaxAmount, Chance, DropRewardId) VALUES (@ItemId, @MinAmount, @MaxAmount, @Chance, @DropRewardId);";
                                SqlCommand insertItemDropCommand = new SqlCommand(insertItemDropQuery, connection);
                                insertItemDropCommand.Parameters.AddWithValue("@ItemId", itemDrop.ItemId);
                                insertItemDropCommand.Parameters.AddWithValue("@MinAmount", itemDrop.MinAmount);
                                insertItemDropCommand.Parameters.AddWithValue("@MaxAmount", itemDrop.MaxAmount);
                                insertItemDropCommand.Parameters.AddWithValue("@Chance", itemDrop.Chance);
                                insertItemDropCommand.Parameters.AddWithValue("@DropRewardId", DropRewardId);

                                insertItemDropCommand.ExecuteNonQuery();
                            }


                            // Inserir SummonMobBitDrop para SummonMobDropReward
                            foreach (var itemDrop in summonedMob.DropReward.BitsDrop)
                            {
                                string insertBitDropQuery = @"INSERT INTO config.SummonBitsDropReward (MinAmount, MaxAmount, Chance, DropRewardId) VALUES (@MinAmount, @MaxAmount, @Chance, @DropRewardId);";
                                SqlCommand insertBitDropCommand = new SqlCommand(insertBitDropQuery, connection);

                                insertBitDropCommand.Parameters.AddWithValue("@MinAmount", itemDrop.MinAmount);
                                insertBitDropCommand.Parameters.AddWithValue("@MaxAmount", itemDrop.MaxAmount);
                                insertBitDropCommand.Parameters.AddWithValue("@Chance", itemDrop.Chance);
                                insertBitDropCommand.Parameters.AddWithValue("@DropRewardId", DropRewardId);

                                insertBitDropCommand.ExecuteNonQuery();
                            }

                            // Inserir ExpReward para SummonMobDTO
                            if (summonedMob.ExpReward != null)
                            {
                                string insertExpRewardQuery = @"INSERT INTO config.SummonMobExpReward (TamerExperience, DigimonExperience, NatureExperience, ElementExperience, SkillExperience, MobId) VALUES (@TamerExperience, @DigimonExperience, @NatureExperience, @ElementExperience, @SkillExperience, @MobId);";
                                SqlCommand insertExpRewardCommand = new SqlCommand(insertExpRewardQuery, connection);
                                insertExpRewardCommand.Parameters.AddWithValue("@TamerExperience", summonedMob.ExpReward.TamerExperience);
                                insertExpRewardCommand.Parameters.AddWithValue("@DigimonExperience", summonedMob.ExpReward.DigimonExperience);
                                insertExpRewardCommand.Parameters.AddWithValue("@NatureExperience", summonedMob.ExpReward.NatureExperience);
                                insertExpRewardCommand.Parameters.AddWithValue("@ElementExperience", summonedMob.ExpReward.ElementExperience);
                                insertExpRewardCommand.Parameters.AddWithValue("@SkillExperience", summonedMob.ExpReward.SkillExperience);
                                insertExpRewardCommand.Parameters.AddWithValue("@MobId", MobId);

                                insertExpRewardCommand.ExecuteNonQuery();
                            }

                            // Inserir Location para SummonMobDTO
                            if (summonedMob.Location != null)
                            {
                                string insertLocationQuery = @"INSERT INTO config.SummonMobLocation (MapId, X, Y, Z, MobConfigId) VALUES (@MapId, @X, @Y, @Z, @MobConfigId);";
                                SqlCommand insertLocationCommand = new SqlCommand(insertLocationQuery, connection);
                                insertLocationCommand.Parameters.AddWithValue("@MapId", 0);
                                insertLocationCommand.Parameters.AddWithValue("@X", summonedMob.Location.X);
                                insertLocationCommand.Parameters.AddWithValue("@Y", summonedMob.Location.Y);
                                insertLocationCommand.Parameters.AddWithValue("@Z", 0);
                                insertLocationCommand.Parameters.AddWithValue("@MobConfigId", MobId);

                                insertLocationCommand.ExecuteNonQuery();
                            }
                        }
                    }

                    connection.Close();
                }

                // Operações de inserção concluídas, pode fazer commit da transação se necessário
            }
        }

        private static void PortalImport(string cs, string[] files)
        {
            var newDataList = ImportPortalFromXml(files[0]);

            var mainQuery = @"
        SET IDENTITY_INSERT Asset.Portal ON;
        INSERT INTO Asset.Portal
        (Id, NpcId, DestinationMapId, DestinationX, DestinationY, PortalIndex)
        VALUES
        (@Id, @NpcId, @DestinationMapId, @DestinationX, @DestinationY, @PortalIndex);
        SET IDENTITY_INSERT Asset.Portal OFF;";

            using (var conn = new SqlConnection(cs))
            {
                conn.Open();

                foreach (var portal in newDataList)
                {
                    foreach (var portals in portal.portalInfos)
                    {
                        using (var command = new SqlCommand(mainQuery, conn))
                        {
                            command.Parameters.AddWithValue("@Id", portals.s_dwPortalID);
                            command.Parameters.AddWithValue("@NpcId", portals.s_dwUniqObjectID);
                            command.Parameters.AddWithValue("@DestinationMapId", portals.s_dwDestMapID);
                            command.Parameters.AddWithValue("@DestinationX", portals.s_nDestTargetX);
                            command.Parameters.AddWithValue("@DestinationY", portals.s_nDestTargetY);
                            command.Parameters.AddWithValue("@PortalIndex", portals.s_nPortalKindIndex);

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        private static void NpcColiseumImport(string cs, string[] files)
        {
            var newDataList = ImportFromXml(files[0]);


            var mainQuery = $@" INSERT INTO Asset.NpcColiseum
                                        (NpcId)
                                    VALUES
                                        (@NpcId);
                                    SELECT SCOPE_IDENTITY();";

            var subQuery = $@"  INSERT INTO Asset.NpcMobInfo
                                    (Round,SummonType,WinPoints,LosePoints, NpcAssetId)
                                VALUES
                                    (@Round,@SummonType,@WinPoints,@LosePoints, @NpcAssetId);";



            var conn = new SqlConnection(cs);
            conn.Open();

            foreach (var npc in newDataList)
            {
                var command = new SqlCommand(mainQuery, conn);
                command.Parameters.AddWithValue("NpcId", npc.dwNPCIdx);

                var npcId = Convert.ToInt64(command.ExecuteScalar());

                if (npcId > 0)
                {
                    foreach (var item in npc.StageInfo)
                    {

                        var subCommand = new SqlCommand(subQuery, conn);
                        subCommand.Parameters.AddWithValue("Round", item.nStage);
                        subCommand.Parameters.AddWithValue("SummonType", item.s_nSummon_Type);
                        subCommand.Parameters.AddWithValue("WinPoints", item.s_nWinPoint);
                        subCommand.Parameters.AddWithValue("LosePoints", item.s_nLosePoint);
                        subCommand.Parameters.AddWithValue("NpcAssetId", npcId);

                        subCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        private static void EvolutionArmorImport(string cs, string[] files)
        {
            var newDataList = new ArmorList();

            foreach (var file in files)
            {
                var xmlDocument = File.ReadAllText(file);

                var serializer = new XmlSerializer(typeof(ArmorList));

                using (TextReader reader = new StringReader(xmlDocument))
                {
                    var newList = (ArmorList)serializer.Deserialize(reader);

                    newDataList.Items.AddRange(newList.Items);
                }
            }

            var mainQuery = @"
   
        INSERT INTO Asset.EvolutionArmor
        (ItemId, Amount, Chance)
        VALUES
        (@ItemId, @Amount, @Chance);";

            using (var conn = new SqlConnection(cs))
            {
                conn.Open();

                foreach (var item in newDataList.Items)
                {

                    using (var command = new SqlCommand(mainQuery, conn))
                    {
                        command.Parameters.AddWithValue("@ItemId", item.ItemId);
                        command.Parameters.AddWithValue("@Amount", item.Amount);
                        command.Parameters.AddWithValue("@Chance", item.Chance);
                        command.ExecuteScalar();
                    }
                }

                conn.Close();
            }
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

        public static Stages[] ImportFromXml(string filePath)
        {
            XDocument doc = XDocument.Load(filePath);

            var stages = doc.Root.Elements("Stage").Select(stageElement => new Stages
            {
                dwNPCIdx = (int)stageElement.Element("dwNPCIdx"),
                StageInfo = stageElement.Elements("StageInfo").Select(stageInfoElement => new StageInfos
                {
                    nStage = (int)stageInfoElement.Element("nStage"),
                    s_nSummon_Type = (int)stageInfoElement.Element("s_nSummon_Type"),
                    s_dwRewardItemIdx = (int)stageInfoElement.Element("s_dwRewardItemIdx"),
                    s_nWinPoint = (int)stageInfoElement.Element("s_nWinPoint"),
                    s_nLosePoint = (int)stageInfoElement.Element("s_nLosePoint"),
                    s_nMonsterInfoIndex = (int)stageInfoElement.Element("s_nMonsterInfoIndex")
                }).ToList()
            }).ToArray();

            return stages;
        }

        private static void ArenaDailyItemRewardsImport(string cs, string[] files)
        {
            var maindata = new RewardList();

            foreach (var file in files)
            {
                var xmlDocument = File.ReadAllText(file);

                var serializer = new XmlSerializer(typeof(RewardList));

                using (TextReader reader = new StringReader(xmlDocument))
                {
                    var newList = (RewardList)serializer.Deserialize(reader);

                    maindata.Rewards.AddRange(newList.Rewards);
                }
            }

            using (SqlConnection connection = new SqlConnection(cs))
            {
                var day = 0;
                connection.Open();

                foreach (var rewards in maindata.Rewards)
                {
                    day++;



                    long summonId = 0;

                    string insertSummonQuery = @"INSERT INTO Asset.ArenaDailyItemRewards (WeekDay) VALUES (@WeekDay); SELECT SCOPE_IDENTITY();";
                    SqlCommand insertSummonCommand = new SqlCommand(insertSummonQuery, connection);

                    // Supondo que summon.ItemId é do tipo int
                    if (day == 1)
                    {
                        insertSummonCommand.Parameters.AddWithValue("@WeekDay", (int)DayOfWeek.Monday);
                    }
                    if (day == 2)
                    {
                        insertSummonCommand.Parameters.AddWithValue("@WeekDay", (int)DayOfWeek.Tuesday);
                    }
                    if (day == 3)
                    {
                        insertSummonCommand.Parameters.AddWithValue("@WeekDay", (int)DayOfWeek.Wednesday);
                    }
                    if (day == 4)
                    {
                        insertSummonCommand.Parameters.AddWithValue("@WeekDay", (int)DayOfWeek.Thursday);
                    }
                    if (day == 5)
                    {
                        insertSummonCommand.Parameters.AddWithValue("@WeekDay", (int)DayOfWeek.Friday);
                    }
                    if (day == 6)
                    {
                        insertSummonCommand.Parameters.AddWithValue("@WeekDay", (int)DayOfWeek.Saturday);
                    }
                    if (day == 7)
                    {
                        insertSummonCommand.Parameters.AddWithValue("@WeekDay", (int)DayOfWeek.Sunday);
                    }

                    // Executando o comando e obtendo o ID inserido
                    object result = insertSummonCommand.ExecuteScalar();

                    // Verifique se a consulta retornou um valor antes de tentar convertê-lo para long
                    if (result != null && result != DBNull.Value)
                    {
                        summonId = Convert.ToInt64(result);
                        // Faça o que precisa ser feito com o summonId aqui
                    }
                    else
                    {
                        // Lidar com um cenário onde não foi possível obter o ID inserido
                    }

                    foreach (var rewardneed in rewards.RewardInfos)
                    {
                        foreach (var reward in rewardneed.RewardsList)
                        {

                            string insertArenaDailyItemRewardQuery = @"
                                                         INSERT INTO Asset.ArenaDailyItemReward (ItemId,Amount,RequiredCoins,RewardId)
                                                         VALUES (@ItemId,@Amount,@RequiredCoins,@RewardId                                                                                                                                                                                   
                                                         ); SELECT SCOPE_IDENTITY();";

                            SqlCommand insertArenaDailyItemRewardCommand = new SqlCommand(insertArenaDailyItemRewardQuery, connection);
                            insertArenaDailyItemRewardCommand.Parameters.AddWithValue("@ItemId", reward.ItemId);
                            insertArenaDailyItemRewardCommand.Parameters.AddWithValue("@Amount", reward.Amount);
                            insertArenaDailyItemRewardCommand.Parameters.AddWithValue("@RequiredCoins", rewardneed.Amount);
                            insertArenaDailyItemRewardCommand.Parameters.AddWithValue("@RewardId", summonId);

                            insertArenaDailyItemRewardCommand.ExecuteNonQuery();
                        }



                    }
                }
                connection.Close();

                // Operações de inserção concluídas, pode fazer commit da transação se necessário
            }
        }


        private static void AccessoryStatusImport(string cs, string[] files)
        {
            var maindata = new AccessoryList();

            foreach (var file in files)
            {
                var xmlDocument = File.ReadAllText(file);

                var serializer = new XmlSerializer(typeof(AccessoryList));

                using (TextReader reader = new StringReader(xmlDocument))
                {
                    var newList = (AccessoryList)serializer.Deserialize(reader);

                    maindata.Accessories.AddRange(newList.Accessories);
                }
            }

            using (SqlConnection connection = new SqlConnection(cs))
            {
                var day = 0;
                connection.Open();

                foreach (var rewards in maindata.Accessories)
                {
                    day++;



                    long summonId = 0;

                    string insertSummonQuery = @"INSERT INTO Asset.AccessoryRoll (ItemId, StatusAmount, RerollAmount) VALUES (@ItemId, @StatusAmount, @RerollAmount); SELECT SCOPE_IDENTITY();";
                    SqlCommand insertSummonCommand = new SqlCommand(insertSummonQuery, connection);

                    // Supondo que summon.ItemId é do tipo int


                    insertSummonCommand.Parameters.AddWithValue("@ItemId", rewards.ItemId);
                    insertSummonCommand.Parameters.AddWithValue("@StatusAmount", rewards.StatusList.Amount);
                    insertSummonCommand.Parameters.AddWithValue("@RerollAmount", rewards.StatusList.MaxRoll);

                    // Executando o comando e obtendo o ID inserido
                    object result = insertSummonCommand.ExecuteScalar();

                    // Verifique se a consulta retornou um valor antes de tentar convertê-lo para long
                    if (result != null && result != DBNull.Value)
                    {
                        summonId = Convert.ToInt64(result);
                        // Faça o que precisa ser feito com o summonId aqui
                    }
                    else
                    {
                        // Lidar com um cenário onde não foi possível obter o ID inserido
                    }


                    foreach (var reward in rewards.StatusList.Statuses)
                    {

                        string insertArenaDailyItemRewardQuery = @"
                                                         INSERT INTO Asset.AccessoryRollStatus (Type,MinValue,MaxValue, AccessoryRollAssetId)
                                                         VALUES (@Type,@MinValue,@MaxValue, @AccessoryRollAssetId                                                                                                                                                                                   
                                                         ); SELECT SCOPE_IDENTITY();";

                        SqlCommand insertArenaDailyItemRewardCommand = new SqlCommand(insertArenaDailyItemRewardQuery, connection);
                        insertArenaDailyItemRewardCommand.Parameters.AddWithValue("@Type", reward.Type);
                        insertArenaDailyItemRewardCommand.Parameters.AddWithValue("@MinValue", reward.MinValue);
                        insertArenaDailyItemRewardCommand.Parameters.AddWithValue("@MaxValue", reward.MaxValue);
                        insertArenaDailyItemRewardCommand.Parameters.AddWithValue("@AccessoryRollAssetId", summonId);

                        insertArenaDailyItemRewardCommand.ExecuteNonQuery();
                    }

                }
                connection.Close();

                // Operações de inserção concluídas, pode fazer commit da transação se necessário
            }
        }
        public static string CleanString(string input)
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
