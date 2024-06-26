using Microsoft.Data.SqlClient;
using System.Xml.Serialization;
using static DataImporterTool.Form1;

namespace DSO.DataImport.Processors
{
    internal static class CloneValueImportProcessor
    {
        internal static void Import(string cs, string[] files)
        {
            var newDataList = new CloneValueList();

            foreach (var file in files)
            {
                var xmlDocument = File.ReadAllText(file);

                var serializer = new XmlSerializer(typeof(CloneValueList));

                using (TextReader reader = new StringReader(xmlDocument))
                {
                    var newList = (CloneValueList)serializer.Deserialize(reader);

                    newDataList.CloneList.AddRange(newList.CloneList);
                }
            }

            var mainQuery = $@" IF NOT EXISTS(SELECT 1 FROM Asset.CloneValue WHERE Type = @Type and MinLevel = @MinLevel)
                                BEGIN
                                    INSERT INTO Asset.CloneValue
                                        ([Type], MinLevel, MaxLevel, [MinValue], [MaxValue])
                                    VALUES
                                        (@Type, @MinLevel, @MaxLevel, @MinValue, @MaxValue);
                                END;";

            var conn = new SqlConnection(cs);
            conn.Open();
            foreach (var clone in newDataList.CloneList)
            {
                foreach (var item in clone.SubValue.List)
                {
                    var command = new SqlCommand(mainQuery, conn);
                    command.Parameters.AddWithValue("Type", clone.Type);
                    command.Parameters.AddWithValue("MinLevel", item.MinLevel);
                    command.Parameters.AddWithValue("MaxLevel", item.MaxLevel);
                    command.Parameters.AddWithValue("MinValue", item.MinValue);
                    command.Parameters.AddWithValue("MaxValue", item.MaxValue);

                    command.ExecuteNonQuery();
                }
            }
            conn.Close();
        }
    }
}