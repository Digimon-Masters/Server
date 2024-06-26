using Microsoft.Data.SqlClient;
using System.Xml.Serialization;
using static DataImporterTool.Form1;

namespace DSO.DataImport.Processors
{
    internal static class DigimonBaseInfoImportProcessor
    {
        internal static void Import(string cs, string[] files)
        {
            var newDataList = new DigimonBaseInfoList();

            foreach (var file in files)
            {
                var xmlDocument = File.ReadAllText(file);

                var serializer = new XmlSerializer(typeof(DigimonBaseInfoList));

                using (TextReader reader = new StringReader(xmlDocument))
                {
                    var newList = (DigimonBaseInfoList)serializer.Deserialize(reader);

                    newDataList.DigimonBaseList.AddRange(newList.DigimonBaseList);
                }
            }

            var mainQuery = $@" IF NOT EXISTS(SELECT 1 FROM Asset.DigimonBaseInfo WHERE Type = @Type)
                                BEGIN
                                    INSERT INTO Asset.DigimonBaseInfo
                                        ([Type], Model, Name, [Level], ScaleType, [Attribute], 
                                        [Element], Family1, Family2, Family3, ASValue, ARValue, ATValue, BLValue, CTValue, DEValue, 
                                        DSValue, EVValue, HPValue, HTValue, MSValue, WSValue, EvolutionType)
                                    VALUES
                                        (@Type, @Model, @Name, @Level, @ScaleType, @Attribute, 
                                        @Element, @Family1, @Family2, @Family3, @ASValue, @ARValue, @ATValue, @BLValue, @CTValue, @DEValue, 
                                        @DSValue, @EVValue, @HPValue, @HTValue, @MSValue, @WSValue, @EvolutionType);
                                END;";

            var subQuery = $@"  INSERT INTO DSO.Asset.DigimonSkill
                                    ([Type], Slot, SkillId)
                                VALUES
                                    (@Type, @Slot, @SkillId);";

            var conn = new SqlConnection(cs);

            conn.Open();
            foreach (var digimonBase in newDataList.DigimonBaseList)
            {
                var command = new SqlCommand(mainQuery, conn);
                command.Parameters.AddWithValue("Type", digimonBase.Type);
                command.Parameters.AddWithValue("Model", digimonBase.Model);
                command.Parameters.AddWithValue("Name", digimonBase.Name);
                command.Parameters.AddWithValue("Level", digimonBase.Level);
                command.Parameters.AddWithValue("ScaleType", digimonBase.ScaleType);
                command.Parameters.AddWithValue("Attribute", digimonBase.Attribute);
                command.Parameters.AddWithValue("Element", digimonBase.Element);
                command.Parameters.AddWithValue("Family1", digimonBase.Family1);
                command.Parameters.AddWithValue("Family2", digimonBase.Family2);
                command.Parameters.AddWithValue("Family3", digimonBase.Family3);
                command.Parameters.AddWithValue("ASValue", digimonBase.ASValue);
                command.Parameters.AddWithValue("ARValue", digimonBase.ARValue);
                command.Parameters.AddWithValue("ATValue", digimonBase.ATValue);
                command.Parameters.AddWithValue("BLValue", digimonBase.BLValue);
                command.Parameters.AddWithValue("CTValue", digimonBase.CTValue);
                command.Parameters.AddWithValue("DEValue", digimonBase.DEValue);
                command.Parameters.AddWithValue("DSValue", digimonBase.DSValue);
                command.Parameters.AddWithValue("EVValue", digimonBase.EVValue);
                command.Parameters.AddWithValue("HPValue", digimonBase.HPValue);
                command.Parameters.AddWithValue("HTValue", digimonBase.HTValue);
                command.Parameters.AddWithValue("MSValue", digimonBase.MSValue);
                command.Parameters.AddWithValue("WSValue", digimonBase.WSValue);
                command.Parameters.AddWithValue("EvolutionType", digimonBase.EvolutionType);

                command.ExecuteNonQuery();

                if (digimonBase.Skill1 > 0)
                {
                    var subConn = new SqlConnection(cs);
                    subConn.Open();
                    var subCommand = new SqlCommand(subQuery, subConn);
                    subCommand.Parameters.AddWithValue("Type", digimonBase.Type);
                    subCommand.Parameters.AddWithValue("Slot", 1);
                    subCommand.Parameters.AddWithValue("SkillId", digimonBase.Skill1);
                    subCommand.ExecuteNonQuery();
                    subConn.Close();
                }
                
                if (digimonBase.Skill2 > 0)
                {
                    var subConn = new SqlConnection(cs);
                    subConn.Open();
                    var subCommand = new SqlCommand(subQuery, subConn);
                    subCommand.Parameters.AddWithValue("Type", digimonBase.Type);
                    subCommand.Parameters.AddWithValue("Slot", 2);
                    subCommand.Parameters.AddWithValue("SkillId", digimonBase.Skill2);
                    subCommand.ExecuteNonQuery();
                    subConn.Close();
                }
                
                if (digimonBase.Skill3 > 0)
                {
                    var subConn = new SqlConnection(cs);
                    subConn.Open();
                    var subCommand = new SqlCommand(subQuery, subConn);
                    subCommand.Parameters.AddWithValue("Type", digimonBase.Type);
                    subCommand.Parameters.AddWithValue("Slot", 3);
                    subCommand.Parameters.AddWithValue("SkillId", digimonBase.Skill3);
                    subCommand.ExecuteNonQuery();
                    subConn.Close();
                }
                
                if (digimonBase.Skill4 > 0)
                {
                    var subConn = new SqlConnection(cs);
                    subConn.Open();
                    var subCommand = new SqlCommand(subQuery, subConn);
                    subCommand.Parameters.AddWithValue("Type", digimonBase.Type);
                    subCommand.Parameters.AddWithValue("Slot", 4);
                    subCommand.Parameters.AddWithValue("SkillId", digimonBase.Skill4);
                    subCommand.ExecuteNonQuery();
                    subConn.Close();
                }
            }

            conn.Close();
        }
    }
}