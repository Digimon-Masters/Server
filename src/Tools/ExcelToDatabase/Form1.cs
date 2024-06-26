using System.Data.SqlClient;

namespace ExcelToDatabase
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        void button1_Click(object sender, EventArgs e)
        {
            ExtractSkillCodeInfo();

            MessageBox.Show("Fim");
        }

        private static void ExtractSkillCodeInfo()
        {
            var csvFile = "D:\\Projetos\\DM_Item_List_E_Skill_Code.tsv"; // textBox1.Text;
            var cs = "Server=DESKTOP-FAC04DI;Database=DSO;User Id=sa;Password=P@ssw0rd!@#$%"; // textBox2.Text;

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
                                               ,[AdditionalValue]
                                               ,[SkillCodeAssetId])
                                            VALUES
                                                (@Type
                                                ,@Attribute
                                                ,@Value
                                                ,@AdditionalValue
                                               ,@SkillCodeAssetId)";

            using (var conn = new SqlConnection(cs))
            {
                var list = new List<SkillCodeDTO>();

                var skip = 2;

                using (var reader = new StreamReader(csvFile))
                {
                    conn.Open();

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line?.Split('\t');

                        if (values == null)
                            continue;

                        if (skip > 0)
                        {
                            skip--;
                            continue;
                        }

                        var skillCode = new SkillCodeDTO();
                        skillCode.SkillCode = Convert.ToInt64(values[1]);
                        skillCode.Comment = Convert.ToString(values[5]);
                        skillCode.SkillApply1 = Convert.ToInt32(values[6]);
                        skillCode.SkillApply1Attribute = Convert.ToInt32(values[8]);
                        skillCode.SkillApply1Value = Convert.ToInt32(values[10]);
                        skillCode.SkillApply1ExtraValue = Convert.ToInt32(String.IsNullOrEmpty(values[11]) ? 0 : values[11]);
                        skillCode.SkillApply2 = Convert.ToInt32(values[14]);
                        skillCode.SkillApply2Attribute = Convert.ToInt32(values[16]);
                        skillCode.SkillApply2Value = Convert.ToInt32(values[18]);
                        skillCode.SkillApply2ExtraValue = Convert.ToInt32(values[19]);
                        skillCode.SkillApply3 = Convert.ToInt32(values[22]);
                        skillCode.SkillApply3Attribute = Convert.ToInt32(values[24]);
                        skillCode.SkillApply3Value = Convert.ToInt32(values[26]);
                        skillCode.SkillApply3ExtraValue = Convert.ToInt32(values[27]);

                        var command = new SqlCommand(insertSkillCode, conn);
                        command.Parameters.AddWithValue("SkillCode", skillCode.SkillCode);
                        command.Parameters.AddWithValue("Comment", skillCode.Comment);
                        var skillCodeId = Convert.ToInt64(command.ExecuteScalar());

                        if(skillCodeId > 0)
                        {
                            using (var subConn = new SqlConnection(cs))
                            {
                                subConn.Open();
                                var subCommand = new SqlCommand(insertSkillCodeApply, subConn);
                                subCommand.Parameters.AddWithValue("Type", skillCode.SkillApply1);
                                subCommand.Parameters.AddWithValue("Attribute", skillCode.SkillApply1Attribute);
                                subCommand.Parameters.AddWithValue("Value", skillCode.SkillApply1Value);
                                subCommand.Parameters.AddWithValue("AdditionalValue", skillCode.SkillApply1ExtraValue);
                                subCommand.Parameters.AddWithValue("SkillCodeAssetId", skillCodeId);
                                subCommand.ExecuteNonQuery();
                                
                                subCommand = new SqlCommand(insertSkillCodeApply, subConn);
                                subCommand.Parameters.AddWithValue("Type", skillCode.SkillApply2);
                                subCommand.Parameters.AddWithValue("Attribute", skillCode.SkillApply2Attribute);
                                subCommand.Parameters.AddWithValue("Value", skillCode.SkillApply2Value);
                                subCommand.Parameters.AddWithValue("AdditionalValue", skillCode.SkillApply2ExtraValue);
                                subCommand.Parameters.AddWithValue("SkillCodeAssetId", skillCodeId);
                                subCommand.ExecuteNonQuery();
                                
                                subCommand = new SqlCommand(insertSkillCodeApply, subConn);
                                subCommand.Parameters.AddWithValue("Type", skillCode.SkillApply3);
                                subCommand.Parameters.AddWithValue("Attribute", skillCode.SkillApply3Attribute);
                                subCommand.Parameters.AddWithValue("Value", skillCode.SkillApply3Value);
                                subCommand.Parameters.AddWithValue("AdditionalValue", skillCode.SkillApply3ExtraValue);
                                subCommand.Parameters.AddWithValue("SkillCodeAssetId", skillCodeId);
                                subCommand.ExecuteNonQuery();

                                subConn.Close();
                            }
                        }
                    }
                    
                    conn.Close();
                }
            }
        }
    }
}