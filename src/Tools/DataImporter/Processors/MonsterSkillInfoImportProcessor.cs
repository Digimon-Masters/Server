using Microsoft.Data.SqlClient;
using System.Globalization;
using System.Xml.Serialization;
using static DataImporterTool.Form1;

namespace DSO.DataImport.Processors
{
    internal static class MonsterSkillInfoImportProcessor
    {
        internal static void Import(string cs, string[] files)
        {


            var maindata = new MonsterSkillsList();

            foreach (var file in files)
            {


                var xmlDocument = File.ReadAllText(file);

                var serializer = new XmlSerializer(typeof(MonsterSkillsList));

                using (TextReader reader = new StringReader(xmlDocument))
                {
                    var newList = (MonsterSkillsList)serializer.Deserialize(reader);

                    maindata.SkillData.AddRange(newList.SkillData);
                }
            }

            var insertSkillCode = @"    IF NOT EXISTS (SELECT 1 FROM Asset.MonsterSkill WHERE SkillId = @SkillId)
                              BEGIN
                                  INSERT INTO [Asset].[MonsterSkill]
                                      ([SkillId]    
                                      ,[Type])
                                  VALUES

                                     (@SkillId
                                      ,@Type);

                                  SELECT SCOPE_IDENTITY();
                              END ELSE BEGIN SELECT 0; END;";

            var insertSkillCodeApply = @"  
                                          INSERT INTO [Asset].[MonsterSkillInfo]
                                          (
                                             [SkillId]
                                            ,[MinValue]
                                            ,[MaxValue]
                                            ,[CastingTime]
                                            ,[Cooldown]
                                            ,[TargetCount]
                                            ,[TargetMin]
                                            ,[TargetMax]
                                            ,[UseTerms]
                                            ,[RangeId]
                                            ,[AnimationDelay]
                                            ,[ActiveType]
                                            ,[SkillType]
                                            ,[NoticeTime]   
                                            ,[Type]
                                          )
                                          VALUES
                                          (
                                             @SkillId
                                            ,@MinValue
                                            ,@MaxValue
                                            ,@CastingTime
                                            ,@Cooldown
                                            ,@TargetCount
                                            ,@TargetMin
                                            ,@TargetMax
                                            ,@UseTerms
                                            ,@RangeId
                                            ,@AnimationDelay
                                            ,@ActiveType
                                            ,@SkillType
                                            ,@NoticeTime
                                            ,@Type
                                          );";

            using (var conn = new SqlConnection(cs))
            {

                foreach (var skillCode in maindata.SkillData)
                {
                    conn.Open();

                    var command = new SqlCommand(insertSkillCode, conn);
                    command.Parameters.AddWithValue("SkillId", skillCode.SkillId);
                    command.Parameters.AddWithValue("Type", skillCode.Type);

                    command.ExecuteNonQuery();



                    using (var subConn = new SqlConnection(cs))
                    {

                        subConn.Open();

                        var subCommand = new SqlCommand(insertSkillCodeApply, subConn);
                        subCommand.Parameters.AddWithValue("SkillId", skillCode.SkillId);
                        subCommand.Parameters.AddWithValue("MinValue", skillCode.MinValue);
                        subCommand.Parameters.AddWithValue("MaxValue", skillCode.MaxValue);
                        subCommand.Parameters.AddWithValue("CastingTime", skillCode.CastingTime);
                        subCommand.Parameters.AddWithValue("UseTerms", skillCode.UseTerms);
                        subCommand.Parameters.AddWithValue("Cooldown", skillCode.Cooldown);
                        subCommand.Parameters.AddWithValue("TargetCount", skillCode.TargetCount);
                        subCommand.Parameters.AddWithValue("TargetMin", skillCode.TargetMin);
                        subCommand.Parameters.AddWithValue("TargetMax", skillCode.TargetMax);
                        subCommand.Parameters.AddWithValue("RangeId", skillCode.RangeId);
                        subCommand.Parameters.AddWithValue("AnimationDelay", skillCode.AnimationDelay);
                        subCommand.Parameters.AddWithValue("ActiveType", skillCode.ActiveType);
                        subCommand.Parameters.AddWithValue("SkillType", skillCode.SkillType);
                        subCommand.Parameters.AddWithValue("NoticeTime", skillCode.NoticeTime);
                        subCommand.Parameters.AddWithValue("Type", skillCode.Type);     
                        subCommand.ExecuteNonQuery();

                        subConn.Close();
                    }

                    conn.Close();
                }
              
            }
        }
    }
}