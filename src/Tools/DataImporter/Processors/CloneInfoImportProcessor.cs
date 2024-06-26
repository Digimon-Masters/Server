using Microsoft.Data.SqlClient;
using System.Xml.Serialization;
using static DataImporterTool.Form1;

namespace DSO.DataImport.Processors
{
    internal static class CloneInfoImportProcessor
    {
        internal static void Import(string cs, string[] files)
        {
            var newDataList = new CloneInfoList();

            foreach (var file in files)
            {
                var xmlDocument = File.ReadAllText(file);

                var serializer = new XmlSerializer(typeof(CloneInfoList));

                using (TextReader reader = new StringReader(xmlDocument))
                {
                    var newList = (CloneInfoList)serializer.Deserialize(reader);

                    newDataList.CloneList.AddRange(newList.CloneList);
                }
            }

            var mainQuery = $@" IF NOT EXISTS(SELECT 1 FROM Asset.Clone WHERE ItemSection = @ItemSection and MinLevel = @MinLevel)
                                BEGIN
                                    INSERT INTO Asset.Clone
                                        (ItemSection, MinLevel, MaxLevel, Bits)
                                    VALUES
                                        (@ItemSection, @MinLevel, @MaxLevel, @Bits);
                                END;";

            var conn = new SqlConnection(cs);
            conn.Open();
            foreach (var clone in newDataList.CloneList)
            {
                
                var command = new SqlCommand(mainQuery, conn);
                command.Parameters.AddWithValue("ItemSection", clone.ItemSection);
                command.Parameters.AddWithValue("MinLevel", clone.MinLevel);
                command.Parameters.AddWithValue("MaxLevel", clone.MaxLevel);
                command.Parameters.AddWithValue("Bits", clone.Bits);

                command.ExecuteNonQuery();
            }
            conn.Close();
        }
    }
}