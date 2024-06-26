using System.Text;
using System.Xml.Linq;


namespace BinXmlConverter.Classes
{
    public class UIText
    {

        public long ID_Maybe;
        public string Text;

        public static UIText[] ReadUITextFromBinary(string FilePath)
        {
            using (BitReader reader = new(File.Open(FilePath, FileMode.Open)))
            {
                int count = reader.ReadInt();
                UIText[] result = new UIText[count];
                for (int i = 0; i < count; i++)
                {

                    UIText text = new UIText();
                    text.ID_Maybe = reader.ReadLong();
                    var TextSize = reader.ReadInt();
                    byte[] nameBytes = reader.ReadBytes(TextSize * 2);
                    string nameString = System.Text.Encoding.Unicode.GetString(nameBytes, 0, TextSize * 2);
                    text.Text = nameString;

                    result[i] = text;
                }

                return result;
            }
        }
        public static void ExportUitextToXml(string filePath, UIText[] ui)
        {
            XElement rootElement = new XElement("UITexts");

            foreach (UIText text in ui)
            {
                XElement textElement = new XElement("UIText",
                     new XElement("ID_Maybe", text.ID_Maybe),
                     new XElement("Text", text.Text)
                );

                rootElement.Add(textElement);
            }

            XDocument xmlDoc = new XDocument(rootElement);
            xmlDoc.Save(filePath);
        }
        public static UIText[] ImportUITextsFromXml(string fileName)
        {
            // Carrega o arquivo XML
            XDocument xmlDocument = XDocument.Load(fileName);

            // Obtém os elementos "UIText" do XML
            IEnumerable<XElement> uiTextElements = xmlDocument.Descendants("UITexts");

            // Cria uma lista para armazenar os objetos UIText importados
            List<UIText> importedUITexts = new List<UIText>();

            // Percorre os elementos e cria objetos UIText
            foreach (XElement uiTextElement in uiTextElements.Descendants("UIText"))
            {
                UIText uiText = new UIText();
                uiText.ID_Maybe = (long)uiTextElement.Element("ID_Maybe");
                uiText.Text = (string)uiTextElement.Element("Text");
                importedUITexts.Add(uiText);
            }

            // Retorna os objetos UIText importados como um array
            return importedUITexts.ToArray();
        }
        public static void WriteUItextToBinary(string filePath, UIText[] ui)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {
                writer.Write(ui.Length);
                foreach (var text in ui)
                {
                    writer.Write(text.ID_Maybe);
                    writer.Write(text.Text.Length);
                    // Converte a string StartDate para formato Unicode
                    byte[] startTextBytes = Encoding.Unicode.GetBytes(text.Text);
                    // Escreve os bytes da string StartDate
                    writer.Write(startTextBytes);
                }
            }
        }

    }
}
