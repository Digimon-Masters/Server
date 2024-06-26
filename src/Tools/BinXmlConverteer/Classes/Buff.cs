using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace BinXmlConverter.Classes
{
    public class BuffData
    {
        public ushort s_dwID;
        public string s_szName;
        public string s_szComment;
        public ushort s_nBuffIcon;
        public ushort s_nBuffType;
        public ushort s_nBuffLifeType;
        public ushort s_nBuffTimeType;
        public ushort s_nMinLv;
        public int s_nBuffClass;
        public short unknow;
        public uint s_dwSkillCode;
        public int s_dwDigimonSkillCode;
        public byte s_bDelete;
        public string s_szEffectFile;
        public ushort s_nConditionLv;
        public byte u;

        public static BuffData[] BuffToXml(string BuffInput)
        {
            using (BitReader read = new BitReader(File.Open(BuffInput, FileMode.Open)))
            {

                int count = read.ReadInt();
                BuffData[] buffDatas = new BuffData[count];
                for (int i = 0; i < count; i++)
                {
                    BuffData buff = new BuffData();
                    buff.s_dwID = read.ReadUShort();
                    byte[] nameBytes = read.ReadBytes(64 * 2); // Lê os bytes da string s_szName

                    StringBuilder nameBuilder = new StringBuilder();

                    for (int x = 0; x < 64; x++)
                    {
                        char character = BitConverter.ToChar(nameBytes, x * 2);

                        if (character != '\0')
                            nameBuilder.Append(character);
                        else
                            break;
                    }

                    buff.s_szName = nameBuilder.ToString();

                    byte[] commentBytes = read.ReadBytes(128 * 2); // Lê os bytes da string s_szComment

                    StringBuilder commentBuilder = new StringBuilder();

                    for (int s = 0; s < 128; s++)
                    {
                        char character = BitConverter.ToChar(commentBytes, s * 2);

                        if (character != '\0')
                            commentBuilder.Append(character);
                        else
                            break;
                    }

                    buff.s_szComment = commentBuilder.ToString();
                    buff.s_nBuffIcon = read.ReadUShort();
                    buff.s_nBuffType = read.ReadUShort();
                    buff.s_nBuffLifeType = read.ReadUShort();
                    buff.s_nBuffTimeType = read.ReadUShort();
                    buff.s_nMinLv = read.ReadUShort();
                    buff.s_nBuffClass = read.ReadShort();
                    buff.unknow = read.ReadShort();
                    buff.s_dwSkillCode = read.ReadUInt();
                    buff.s_dwDigimonSkillCode = read.ReadInt();
                    buff.s_bDelete = read.ReadByte();

                    buff.s_szEffectFile = read.ReadZString(Encoding.ASCII, 64);
                    buff.s_nConditionLv = read.ReadUShort();
                    buff.u = read.ReadByte();

                    buffDatas[i] = buff;
                }

                return buffDatas;
            }
        }
        public static void ExportBuffToXml(BuffData[] buffArray, string filePath)
        {
            XElement rootElement = new XElement("BuffDataArray");

            foreach (BuffData buffData in buffArray)
            {
                XElement buffElement = new XElement("BuffData");
                buffElement.Add(new XElement("s_dwID", buffData.s_dwID));
                buffElement.Add(new XElement("s_szName", buffData.s_szName));
                buffElement.Add(new XElement("s_szComment", buffData.s_szComment));
                buffElement.Add(new XElement("s_nBuffIcon", buffData.s_nBuffIcon));
                buffElement.Add(new XElement("s_nBuffType", buffData.s_nBuffType));
                buffElement.Add(new XElement("s_nBuffLifeType", buffData.s_nBuffLifeType));
                buffElement.Add(new XElement("s_nBuffTimeType", buffData.s_nBuffTimeType));
                buffElement.Add(new XElement("s_nMinLv", buffData.s_nMinLv));
                buffElement.Add(new XElement("s_nBuffClass", buffData.s_nBuffClass));
                buffElement.Add(new XElement("unknow", buffData.unknow));
                buffElement.Add(new XElement("s_dwSkillCode", buffData.s_dwSkillCode));
                buffElement.Add(new XElement("s_dwDigimonSkillCode", buffData.s_dwDigimonSkillCode));
                buffElement.Add(new XElement("s_bDelete", buffData.s_bDelete));
                buffElement.Add(new XElement("s_szEffectFile", buffData.s_szEffectFile));
                buffElement.Add(new XElement("s_nConditionLv", buffData.s_nConditionLv));
                buffElement.Add(new XElement("u", buffData.u));

                rootElement.Add(buffElement);
            }

            rootElement = new XElement("BuffDataArray");

            foreach (BuffData buffData in buffArray)
            {
                XElement buffElement = new XElement("BuffData");

                // Percorra os campos do objeto BuffData e substitua os caracteres inválidos e remova espaços em branco extras
                foreach (var field in buffData.GetType().GetFields())
                {
                    string fieldValue = field.GetValue(buffData)?.ToString();
                    if (fieldValue != null)
                    {
                        fieldValue = RemoveInvalidXmlCharacters(fieldValue);
                        fieldValue = fieldValue.Trim(); // Remover espaços em branco extras
                    }
                    buffElement.Add(new XElement(field.Name, fieldValue));
                }

                rootElement.Add(buffElement);
            }

            XDocument xmlDoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), rootElement);
            xmlDoc.Save(filePath);

            Console.WriteLine("Dados exportados com sucesso para o arquivo XML!");
        }
        public static string RemoveInvalidXmlCharacters(string input)
        {
            // Remove caracteres inválidos do input substituindo-os por uma string vazia
            return new string(input.Where(c => XmlConvert.IsXmlChar(c)).ToArray());
        }
        public static BuffData[] ImportBuffFromXml(string filePath)
        {
            List<BuffData> buffList = new List<BuffData>();

            XElement rootElement = XElement.Load(filePath);
            foreach (XElement buffElement in rootElement.Elements("BuffData"))
            {
                BuffData buffData = new BuffData();
                buffData.s_dwID = ushort.Parse(buffElement.Element("s_dwID").Value);
                buffData.s_szName = buffElement.Element("s_szName").Value;
                buffData.s_szComment = buffElement.Element("s_szComment").Value;
                buffData.s_nBuffIcon = ushort.Parse(buffElement.Element("s_nBuffIcon").Value);
                buffData.s_nBuffType = ushort.Parse(buffElement.Element("s_nBuffType").Value);
                buffData.s_nBuffLifeType = ushort.Parse(buffElement.Element("s_nBuffLifeType").Value);
                buffData.s_nBuffTimeType = ushort.Parse(buffElement.Element("s_nBuffTimeType").Value);
                buffData.s_nMinLv = ushort.Parse(buffElement.Element("s_nMinLv").Value);
                buffData.s_nBuffClass = int.Parse(buffElement.Element("s_nBuffClass").Value);
                buffData.unknow = short.Parse(buffElement.Element("unknow").Value);
                buffData.s_dwSkillCode = uint.Parse(buffElement.Element("s_dwSkillCode").Value);
                buffData.s_dwDigimonSkillCode = int.Parse(buffElement.Element("s_dwDigimonSkillCode").Value);
                buffData.s_bDelete = byte.Parse(buffElement.Element("s_bDelete").Value);
                buffData.s_szEffectFile = buffElement.Element("s_szEffectFile").Value;
                buffData.s_nConditionLv = ushort.Parse(buffElement.Element("s_nConditionLv").Value);
                buffData.u = byte.Parse(buffElement.Element("u").Value);

                buffList.Add(buffData);
            }

            return buffList.ToArray();
        }
        public static void ExportBuffToBinary(string binPathRide, BuffData[] buffs)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(binPathRide, FileMode.Create)))
            {
                writer.Write(buffs.Length);

                foreach (BuffData buff in buffs)
                {
                    writer.Write(buff.s_dwID);
                    byte[] stringBytes = Encoding.Unicode.GetBytes(buff.s_szName);
                    // Criar um array de bytes com o tamanho fixo desejado (128 bytes)
                    byte[] fixedSizeBytes = new byte[128];
                    // Copiar os bytes da string para o array de tamanho fixo
                    Array.Copy(stringBytes, fixedSizeBytes, Math.Min(stringBytes.Length, fixedSizeBytes.Length));
                    // Gravar o array de bytes no arquivo binário
                    writer.Write(fixedSizeBytes);

                    //var CommentSize = 256;
                    //
                    //string paddedString = buff.s_szComment.PadRight(CommentSize);
                    //
                    //// Escrever a string no arquivo binário
                    //byte[] stringBytees = Encoding.ASCII.GetBytes(paddedString);
                    WriteFixedString(writer, buff.s_szComment, 128);
                    //writer.Write(stringBytees);
                    writer.Write(buff.s_nBuffIcon);
                    writer.Write(buff.s_nBuffType);
                    writer.Write(buff.s_nBuffLifeType);
                    writer.Write(buff.s_nBuffTimeType);
                    writer.Write(buff.s_nMinLv);
                    writer.Write(buff.s_nBuffClass);
                    writer.Write(buff.s_dwSkillCode);
                    writer.Write(buff.s_dwDigimonSkillCode);
                    writer.Write(buff.s_bDelete);
                    byte[] s_szEffectFile = Encoding.ASCII.GetBytes(buff.s_szEffectFile);
                    Array.Resize(ref s_szEffectFile, 64); // Ajusta o tamanho do array para 512 caracteres wchar_t
                    writer.Write(s_szEffectFile);

                    writer.Write(buff.s_nConditionLv);
                    writer.Write(buff.u);
                }
            }
        }
        private static void WriteFixedString(BinaryWriter writer, string value, int length)
        {
            // Preenche a string com espaços em branco ou trunca se necessário
            string paddedValue = value.PadRight(length, '\0');

            // Converte a string para um array de bytes usando a codificação UTF-16
            byte[] stringBytes = Encoding.Unicode.GetBytes(paddedValue);

            // Verifica se o tamanho do array é maior do que o tamanho desejado
            if (stringBytes.Length > length * 2)
            {
                // Trunca o array para o tamanho desejado
                Array.Resize(ref stringBytes, length * 2);
            }
            else if (stringBytes.Length < length * 2)
            {
                // Preenche o array com bytes nulos se for menor do que o tamanho desejado
                Array.Resize(ref stringBytes, length * 2);
                Array.Clear(stringBytes, value.Length * 2, (length - value.Length) * 2);
            }

            // Grava o array de bytes no arquivo
            writer.Write(stringBytes);
        }
    }
}
