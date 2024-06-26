using Microsoft.Data.SqlClient;
using System.Xml.Serialization;
using static DataImporterTool.Form1;

namespace DSO.DataImport.Processors
{
    internal static class SealInfoImportProcessor
    {
        internal static void Import(string cs, string[] files)
        {
            var newDataList = new SealList();

            foreach (var file in files)
            {
                var xmlDocument = File.ReadAllText(file);

                var serializer = new XmlSerializer(typeof(SealList));

                using (TextReader reader = new StringReader(xmlDocument))
                {
                    var newList = (SealList)serializer.Deserialize(reader);

                    newDataList.Seals.AddRange(newList.Seals);
                }
            }

            var mainQuery = $@"  IF NOT EXISTS(SELECT 1 FROM Asset.SealDetail WHERE SealId = @SealId and SequentialId = @SequentialId and RequiredAmount = @RequiredAmount)
                                BEGIN
                                    INSERT INTO Asset.SealDetail
                                        (SealId, RequiredAmount, SequentialId, ASValue, ARValue, ATValue, 
                                        BLValue, CTValue, DEValue, DSValue, EVValue, HPValue, HTValue, MSValue, WSValue)
                                    VALUES
                                        (@SealId, @RequiredAmount, @SequentialId, @ASValue, @ARValue, @ATValue, 
                                        @BLValue, @CTValue, @DEValue, @DSValue, @EVValue, @HPValue, @HTValue, @MSValue, @WSValue);
                                END;";

            var conn = new SqlConnection(cs);

            conn.Open();
            foreach (var seal in newDataList.Seals)
            {
                int i = 0;
                foreach (var sealStatus in seal.SealStatusObject.SealStatusList)
                {
                    var command = new SqlCommand(mainQuery, conn);
                    command.Parameters.AddWithValue($"SealId", seal.Id);
                    command.Parameters.AddWithValue($"SequentialId", seal.Sequential);
                    command.Parameters.AddWithValue("ARValue", 0);
                    command.Parameters.AddWithValue("WSValue", 0);

                    switch (i)
                    {
                        case 0:
                            command.Parameters.AddWithValue($"RequiredAmount", 1);
                            break;

                        case 1:
                            command.Parameters.AddWithValue($"RequiredAmount", 50);
                            break;

                        case 2:
                            command.Parameters.AddWithValue($"RequiredAmount", 200);
                            break;

                        case 3:
                            command.Parameters.AddWithValue($"RequiredAmount", 500);
                            break;

                        case 4:
                            command.Parameters.AddWithValue($"RequiredAmount", 1000);
                            break;

                        case 5:
                            command.Parameters.AddWithValue($"RequiredAmount", 3000);
                            break;
                    }

                    switch (sealStatus.Type)
                    {
                        case 1:
                            {
                                command.Parameters.AddWithValue("ASValue", 0);
                                command.Parameters.AddWithValue("ATValue", 0);
                                command.Parameters.AddWithValue("BLValue", 0);
                                command.Parameters.AddWithValue("CTValue", 0);
                                command.Parameters.AddWithValue("DEValue", 0);
                                command.Parameters.AddWithValue("DSValue", 0);
                                command.Parameters.AddWithValue("EVValue", 0);
                                command.Parameters.AddWithValue("HPValue", sealStatus.Amount);
                                command.Parameters.AddWithValue("HTValue", 0);
                                command.Parameters.AddWithValue("MSValue", 0);
                            }
                            break;

                        case 3:
                            {
                                command.Parameters.AddWithValue("ASValue", 0);
                                command.Parameters.AddWithValue("ATValue", 0);
                                command.Parameters.AddWithValue("BLValue", 0);
                                command.Parameters.AddWithValue("CTValue", 0);
                                command.Parameters.AddWithValue("DEValue", 0);
                                command.Parameters.AddWithValue("DSValue", sealStatus.Amount);
                                command.Parameters.AddWithValue("EVValue", 0);
                                command.Parameters.AddWithValue("HPValue", 0);
                                command.Parameters.AddWithValue("HTValue", 0);
                                command.Parameters.AddWithValue("MSValue", 0);
                            }
                            break;

                        case 5:
                            {
                                command.Parameters.AddWithValue("ASValue", 0);
                                command.Parameters.AddWithValue("ATValue", sealStatus.Amount);
                                command.Parameters.AddWithValue("BLValue", 0);
                                command.Parameters.AddWithValue("CTValue", 0);
                                command.Parameters.AddWithValue("DEValue", 0);
                                command.Parameters.AddWithValue("DSValue", 0);
                                command.Parameters.AddWithValue("EVValue", 0);
                                command.Parameters.AddWithValue("HPValue", 0);
                                command.Parameters.AddWithValue("HTValue", 0);
                                command.Parameters.AddWithValue("MSValue", 0);
                            }
                            break;

                        case 9:
                            {
                                command.Parameters.AddWithValue("ASValue", 0);
                                command.Parameters.AddWithValue("ATValue", 0);
                                command.Parameters.AddWithValue("BLValue", 0);
                                command.Parameters.AddWithValue("CTValue", sealStatus.Amount * 100);
                                command.Parameters.AddWithValue("DEValue", 0);
                                command.Parameters.AddWithValue("DSValue", 0);
                                command.Parameters.AddWithValue("EVValue", 0);
                                command.Parameters.AddWithValue("HPValue", 0);
                                command.Parameters.AddWithValue("HTValue", 0);
                                command.Parameters.AddWithValue("MSValue", 0);
                            }
                            break;

                        case 11:
                            {
                                command.Parameters.AddWithValue("ASValue", 0);
                                command.Parameters.AddWithValue("ATValue", 0);
                                command.Parameters.AddWithValue("BLValue", 0);
                                command.Parameters.AddWithValue("CTValue", 0);
                                command.Parameters.AddWithValue("DEValue", 0);
                                command.Parameters.AddWithValue("DSValue", 0);
                                command.Parameters.AddWithValue("EVValue", 0);
                                command.Parameters.AddWithValue("HPValue", 0);
                                command.Parameters.AddWithValue("HTValue", sealStatus.Amount);
                                command.Parameters.AddWithValue("MSValue", 0);
                            }
                            break;

                        case 13:
                            {
                                command.Parameters.AddWithValue("ASValue", 0);
                                command.Parameters.AddWithValue("ATValue", 0);
                                command.Parameters.AddWithValue("BLValue", 0);
                                command.Parameters.AddWithValue("CTValue", 0);
                                command.Parameters.AddWithValue("DEValue", sealStatus.Amount);
                                command.Parameters.AddWithValue("DSValue", 0);
                                command.Parameters.AddWithValue("EVValue", 0);
                                command.Parameters.AddWithValue("HPValue", 0);
                                command.Parameters.AddWithValue("HTValue", 0);
                                command.Parameters.AddWithValue("MSValue", 0);
                            }
                            break;

                        case 15:
                            {
                                command.Parameters.AddWithValue("ASValue", 0);
                                command.Parameters.AddWithValue("ATValue", 0);
                                command.Parameters.AddWithValue("BLValue", sealStatus.Amount * 100);
                                command.Parameters.AddWithValue("CTValue", 0);
                                command.Parameters.AddWithValue("DEValue", 0);
                                command.Parameters.AddWithValue("DSValue", 0);
                                command.Parameters.AddWithValue("EVValue", 0);
                                command.Parameters.AddWithValue("HPValue", 0);
                                command.Parameters.AddWithValue("HTValue", 0);
                                command.Parameters.AddWithValue("MSValue", 0);
                            }
                            break;

                        case 17:
                            {
                                command.Parameters.AddWithValue("ASValue", 0);
                                command.Parameters.AddWithValue("ATValue", 0);
                                command.Parameters.AddWithValue("BLValue", 0);
                                command.Parameters.AddWithValue("CTValue", 0);
                                command.Parameters.AddWithValue("DEValue", 0);
                                command.Parameters.AddWithValue("DSValue", 0);
                                command.Parameters.AddWithValue("EVValue", sealStatus.Amount * 100);
                                command.Parameters.AddWithValue("HPValue", 0);
                                command.Parameters.AddWithValue("HTValue", 0);
                                command.Parameters.AddWithValue("MSValue", 0);
                            }
                            break;
                    }
                    i++;
                    command.ExecuteNonQuery();
                }
            }

            conn.Close();
        }
    }
}