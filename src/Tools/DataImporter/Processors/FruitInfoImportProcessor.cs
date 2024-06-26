using Microsoft.Data.SqlClient;
using System.Xml.Serialization;
using static DataImporterTool.Form1;

namespace DSO.DataImport.Processors
{
    internal static class FruitInfoImportProcessor
    {
        internal static void Import(string cs, string[] files)
        {
            var newDataList = new FruitInfoList();

            foreach (var file in files)
            {
                var xmlDocument = File.ReadAllText(file);

                var serializer = new XmlSerializer(typeof(FruitInfoList));

                using (TextReader reader = new StringReader(xmlDocument))
                {
                    var newList = (FruitInfoList)serializer.Deserialize(reader);

                    newDataList.FruitList.AddRange(newList.FruitList);
                }
            }

            var mainQuery = $@" IF NOT EXISTS(SELECT 1 FROM Config.Fruit WHERE ItemId = @ItemId)
                                BEGIN
                                    INSERT INTO Config.Fruit
                                        (ItemId, ItemSection)
                                    VALUES
                                        (@ItemId, @ItemSection);
                                    SELECT SCOPE_IDENTITY();
                                END ELSE BEGIN SELECT 0; END;";
            
            var subQuery = $@"  INSERT INTO DSO.Config.FruitSize
                                    (HatchGrade, [Size], Chance, FruitConfigId)
                                VALUES
                                    (@HatchGrade, @Size, @Chance, @FruitConfigId);";

            var conn = new SqlConnection(cs);
            conn.Open();

            foreach (var fruit in newDataList.FruitList)
            {
                var command = new SqlCommand(mainQuery, conn);
                command.Parameters.AddWithValue("ItemId", fruit.ItemId);
                command.Parameters.AddWithValue("ItemSection", fruit.ItemSection);

                var fruitId = Convert.ToInt64(command.ExecuteScalar());

                if(fruitId > 0)
                {
                    foreach (var item in fruit.IncreaseInfo.IncreaseList)
                    {
                        foreach (var subItem in item.SizeInfo.SizeList)
                        {
                            var subCommand = new SqlCommand(subQuery, conn);
                            subCommand.Parameters.AddWithValue("HatchGrade", item.Grade);
                            subCommand.Parameters.AddWithValue("Size", subItem.Size);
                            subCommand.Parameters.AddWithValue("Chance", subItem.Chance);
                            subCommand.Parameters.AddWithValue("FruitConfigId", fruitId);

                            subCommand.ExecuteNonQuery();
                        }
                    }
                }
            }

            conn.Close();
        }
    }
}