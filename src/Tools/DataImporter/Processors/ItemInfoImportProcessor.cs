using Microsoft.Data.SqlClient;
using System.Xml.Serialization;
using static DataImporterTool.Form1;

namespace DSO.DataImport.Processors
{
    internal static class ItemInfoImportProcessor
    {
        internal static void Import(string cs, string[] files)
        {
            var newDataList = new ItemInfoList();

            foreach (var file in files)
            {
                var xmlDocument = File.ReadAllText(file);

                var serializer = new XmlSerializer(typeof(ItemInfoList));

                using (TextReader reader = new StringReader(xmlDocument))
                {
                    var newList = (ItemInfoList)serializer.Deserialize(reader);

                    newDataList.ItemInfo.ItemList.AddRange(newList.ItemInfo.ItemList);
                }
            }

            var mainQuery = $@" IF NOT EXISTS(SELECT 1 FROM Asset.ItemInfo WHERE ItemId = @ItemId)
                                BEGIN
                                    INSERT INTO Asset.ItemInfo
                                        (ItemId, Name, Class, [Type], [Section], SellType, BoundType, UseTimeType, TypeN, ApplyValueMin, ApplyValueMax,
                                        ApplyElement,SkillCode, TamerMinLevel, TamerMaxLevel, DigimonMinLevel, DigimonMaxLevel, 
                                        SellPrice, ScanPrice, DigicorePrice, EventPriceId, EventPriceAmount, UsageTimeMinutes, Overlap, Target)
                                    VALUES
                                        (@ItemId, @Name, @Class, @Type, @Section, @SellType, @BoundType, @UseTimeType, @TypeN, @ApplyValueMin, @ApplyValueMax,
                                        @ApplyElement,@SkillCode, @TamerMinLevel, @TamerMaxLevel, @DigimonMinLevel, @DigimonMaxLevel, 
                                        @SellPrice, @ScanPrice, @DigicorePrice, @EventPriceId, @EventPriceAmount, @UsageTimeMinutes, @Overlap, @Target);
                                END;";

            var conn = new SqlConnection(cs);

            conn.Open();
            foreach (var item in newDataList.ItemInfo.ItemList)
            {
                var command = new SqlCommand(mainQuery, conn);
                command.Parameters.AddWithValue("ItemId", item.ItemId);
                command.Parameters.AddWithValue("Name", item.Name);
                command.Parameters.AddWithValue("Class", item.Class);
                command.Parameters.AddWithValue("Type", item.Type);
                command.Parameters.AddWithValue("TypeN", item.TypeN);
                command.Parameters.AddWithValue("ApplyValueMin", item.ApplyValueMin);
                command.Parameters.AddWithValue("ApplyValueMax", item.ApplyValueMax);
                command.Parameters.AddWithValue("ApplyElement", item.ApplyElement);
                command.Parameters.AddWithValue("Section", item.Section);
                command.Parameters.AddWithValue("SellType", item.SellType);
                command.Parameters.AddWithValue("BoundType", item.BoundType);
                command.Parameters.AddWithValue("UseTimeType", item.UseTimeType);
                command.Parameters.AddWithValue("SkillCode", item.SkillCode);
                command.Parameters.AddWithValue("TamerMinLevel", item.TamerMinLevel);
                command.Parameters.AddWithValue("TamerMaxLevel", item.TamerMaxLevel);
                command.Parameters.AddWithValue("DigimonMinLevel", item.DigimonMinLevel);
                command.Parameters.AddWithValue("DigimonMaxLevel", item.DigimonMaxLevel);
                command.Parameters.AddWithValue("SellPrice", item.SellPrice);
                command.Parameters.AddWithValue("ScanPrice", item.ScanPrice);
                command.Parameters.AddWithValue("DigicorePrice", item.DigicorePrice);
                command.Parameters.AddWithValue("EventPriceId", item.EventItemId);
                command.Parameters.AddWithValue("EventPriceAmount", item.EventItemAmount);
                command.Parameters.AddWithValue("UsageTimeMinutes", item.UsageTimeMinutes);
                command.Parameters.AddWithValue("Overlap", item.Overlap);
                command.Parameters.AddWithValue("Target", item.Target);
                command.ExecuteNonQuery();
            }
            conn.Close();
        }
    }
}