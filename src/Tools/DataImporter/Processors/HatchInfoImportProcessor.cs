using Microsoft.Data.SqlClient;
using System.Xml.Serialization;
using static DataImporterTool.Form1;

namespace DSO.DataImport.Processors
{
    internal static class HatchInfoImportProcessor
    {
        internal static void Import(string cs, string[] files)
        {
            var newDataList = new TacticsInfoList();

            foreach (var file in files)
            {
                var xmlDocument = File.ReadAllText(file);

                var serializer = new XmlSerializer(typeof(TacticsInfoList));

                using (TextReader reader = new StringReader(xmlDocument))
                {
                    var newList = (TacticsInfoList)serializer.Deserialize(reader);

                    newDataList.HatchList.AddRange(newList.HatchList);
                }
            }

            var mainQuery = $@" IF NOT EXISTS(SELECT 1 FROM Asset.Hatch WHERE ItemId = @ItemId and HatchType = @HatchType)
                                BEGIN
                                    INSERT INTO Asset.Hatch
                                        (ItemId, HatchType, LowClassDataSection, MidClassDataSection, LowClassDataAmount, MidClassDataAmount, LowClassBreakPoint, MidClassBreakPoint)
                                    VALUES
                                        (@ItemId, @HatchType, @LowClassDataSection, @MidClassDataSection, @LowClassDataAmount, @MidClassDataAmount, @LowClassBreakPoint, @MidClassBreakPoint);
                                END;";

            var conn = new SqlConnection(cs);
            conn.Open();
            foreach (var hatch in newDataList.HatchList)
            {
                
                var command = new SqlCommand(mainQuery, conn);
                command.Parameters.AddWithValue("ItemId", hatch.ItemId);
                command.Parameters.AddWithValue("HatchType", hatch.HatchType);
                command.Parameters.AddWithValue("LowClassDataSection", hatch.LowClassDataSection);
                command.Parameters.AddWithValue("MidClassDataSection", hatch.MidClassDataSection);
                command.Parameters.AddWithValue("LowClassDataAmount", hatch.LowClassDataAmount);
                command.Parameters.AddWithValue("MidClassDataAmount", hatch.MidClassDataAmount);
                command.Parameters.AddWithValue("LowClassBreakPoint", hatch.LowClassBreakPoint);
                command.Parameters.AddWithValue("MidClassBreakPoint", hatch.MidClassBreakPoint);

                command.ExecuteNonQuery();
            }
            conn.Close();
        }
    }
}