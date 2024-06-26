using Microsoft.Data.SqlClient;
using System.Xml.Serialization;
using static DataImporterTool.Form1;

namespace DSO.DataImport.Processors
{
    internal static class EvoInfoImportProcessor
    {
        internal static void Import(string cs, string[] files)
        {
            var newEvoInfoList = new DigimonEvoList();

            foreach (var file in files)
            {
                var xmlDocument = File.ReadAllText(file);

                var serializer = new XmlSerializer(typeof(DigimonEvoList));

                using (TextReader reader = new StringReader(xmlDocument))
                {
                    var newList = (DigimonEvoList)serializer.Deserialize(reader);

                    newEvoInfoList.DigimonEvos.AddRange(newList.DigimonEvos);
                }
            }

            var evoQuery = $@"  IF NOT EXISTS(SELECT 1 FROM [Asset].[Evolution] WHERE Type = @Type)
                                BEGIN
                                    INSERT INTO [Asset].[Evolution]
                                        ([Type], [EvolutionRank])
                                    VALUES
                                        (@Type, @EvolutionRank);

                                    SELECT SCOPE_IDENTITY();
                                END ELSE BEGIN SELECT 0; END;";

            var lineQuery = $@" INSERT INTO Asset.EvolutionLine
                                    (EvolutionId, [Type], UnlockItemSection, UnlockItemSectionAmount, SlotLevel,UnlockLevel, UnlockQuestId)
                                VALUES
                                    (@EvolutionId, @Type, @UnlockItemSection, @UnlockItemSectionAmount,@SlotLevel, @UnlockLevel, @UnlockQuestId);

                                SELECT SCOPE_IDENTITY();";

            var stageQuery = $@"INSERT INTO [Asset].[EvolutionStage]
                                    ([Type]
                                    ,[Value]
                                    ,[EvolutionLineId])
                                VALUES
                                    (@Type
                                    ,@Value
                                    ,@EvolutionLineId)";

            var conn = new SqlConnection(cs);

            conn.Open();

            foreach (var evo in newEvoInfoList.DigimonEvos)
            {
                var command = new SqlCommand(evoQuery, conn);
                command.Parameters.AddWithValue("Type", evo.Type);
                command.Parameters.AddWithValue("EvolutionRank", evo.EvolutionRank);

                var evoId = Convert.ToInt64(command.ExecuteScalar());

                if (evoId == 0)
                    continue;

                foreach (var evoLine in evo.EvolutionLines)
                {
                    var lineCommand = new SqlCommand(lineQuery, conn);
                    lineCommand.Parameters.AddWithValue("EvolutionId", evoId);
                    lineCommand.Parameters.AddWithValue("Type", evoLine.Type);
                    lineCommand.Parameters.AddWithValue("UnlockItemSection", evoLine.UnlockItemSection);
                    lineCommand.Parameters.AddWithValue("UnlockItemSectionAmount", evoLine.UnlockItemSectionAmount);
                    lineCommand.Parameters.AddWithValue("UnlockLevel", evoLine.UnlockLevel);
                    lineCommand.Parameters.AddWithValue("SlotLevel", evoLine.SlotLevel);
                    lineCommand.Parameters.AddWithValue("UnlockQuestId", evoLine.UnlockQuestId);

                    var lineId = Convert.ToInt64(lineCommand.ExecuteScalar());

                    foreach (var stage in evoLine.EvolutionStages)
                    {
                        var stageCommand = new SqlCommand(stageQuery, conn);
                        stageCommand.Parameters.AddWithValue("Type", stage.Type);
                        stageCommand.Parameters.AddWithValue("Value", stage.Value);
                        stageCommand.Parameters.AddWithValue("EvolutionLineId", lineId);

                        stageCommand.ExecuteNonQuery();
                    }
                }
            }

            conn.Close();
        }
    }
}