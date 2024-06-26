using System.Text;
using System.Xml.Linq;

namespace BinXmlConverter.Classes
{
    public class Digimon_Book
    {
        public class BookInfo
        {
            public int s_dwOptID;                            // número da opção
            public string s_szOptName;        // nome da opção
            public ushort s_nIcon;                          // ícone de opção
            public string s_szOptExplain;   // descrição das opções
        }
        public class EncyclopediaException
        {
            public int s_dwDigimonID;                // número único
            public string s_szName;	// nome do Digimon
        }
        public class DeckOption
        {
            public ushort s_nGroupIdx;                 // Índice de convés (grupo) (1001 ~)
            public string s_szGroupName;  // Nome do convés (grupo)
            public string s_szExplain;// Descrição do deck

            public ushort[] s_nCondition;// Condições de ativação da opção (1: Passivo, 2: Probabilidade, 3: Probabilidade + Duração)
            public ushort[] s_nAT_Type;   // Tipo de verificação do efeito de ativação (0: Desativado-Passivo 1: Ataque normal, 2: Ataque de habilidade)
            public ushort[] s_nOption;    // opção
                                          // Opção: Aumenta o número (1: Poder de ataque 2: HP 3: Dano da habilidade 4: Crítico)
                                          // Opção: Ativação de probabilidade (5: Reinicializar o resfriamento da habilidade)
                                          // Opção: Ativação de probabilidade (6: Aumentar a velocidade de ataque)
            public ushort[] s_nVal;       // valor da opção
            public uint[] s_nProb;        // porcentagem
            public uint[] s_nTime;
        }
        public class DeckComposition
        {
            public ushort s_nGroupIdx;                 // Índice de convés (grupo) (1001 ~)
            public string s_szGroupName;  // Nome do convés (grupo)
            public ushort s_nVal;
            public List<DeckDigimon> deckDigimons = new();
        }
        public class DeckDigimon
        {
            public int s_dwBaseDigimonID;                        //계열체 번호
            public string s_szBaseDigimonName;   //계열체 이름
            public ushort s_nEvolslot;                         //진화슬롯 번호
            public int s_dwDestDigimonID;                        //디지몬 번호
            public string s_szDestDigimonName;	//디지몬 이름
        }

        public static (BookInfo[], EncyclopediaException[], DeckOption[], DeckComposition[]) ReadDigimonBookFromBinary(string InputFile)
        {
            using (BitReader read = new BitReader(File.Open(InputFile, FileMode.Open)))
            {
                var count = read.ReadInt();
                BookInfo[] books = new BookInfo[count];
                for (int i = 0; i < count; i++)
                {
                    BookInfo book = new BookInfo();
                    book.s_dwOptID = read.ReadInt();
                    book.s_szOptName = read.ReadZString(Encoding.Unicode, 128);
                    book.s_nIcon = read.ReadUShort();
                    book.s_szOptExplain = read.ReadZString(Encoding.Unicode, 513 * 2);

                    books[i] = book;
                }

                var dcount = read.ReadInt();
                EncyclopediaException[] encyclopediaExceptions = new EncyclopediaException[dcount];
                for (int x = 0; x < dcount; x++)
                {
                    EncyclopediaException encyclopediaException = new();
                    encyclopediaException.s_dwDigimonID = read.ReadInt();
                    byte[] s_szNameBytes = read.ReadBytes(128);
                    string s_szName = System.Text.Encoding.Unicode.GetString(s_szNameBytes, 0, 128);
                    encyclopediaException.s_szName = CleanString(s_szName);

                    encyclopediaExceptions[x] = encyclopediaException;
                }

                var scount = read.ReadInt();
                DeckOption[] decks = new DeckOption[scount];
                for (int i = 0; i < scount; i++)
                {
                    DeckOption deck = new();
                    deck.s_nGroupIdx = read.ReadUShort();
                    deck.s_szGroupName = read.ReadZString(Encoding.Unicode, 128);
                    deck.s_szExplain = read.ReadZString(Encoding.Unicode, 513 * 2);
                    deck.s_nCondition = new ushort[3];
                    deck.s_nAT_Type = new ushort[3];
                    deck.s_nOption = new ushort[3];
                    deck.s_nVal = new ushort[3];
                    deck.s_nProb = new uint[3];
                    deck.s_nTime = new uint[3];

                    deck.s_nCondition[0] = read.ReadUShort();
                    deck.s_nCondition[1] = read.ReadUShort();
                    deck.s_nCondition[2] = read.ReadUShort();

                    deck.s_nAT_Type[0] = read.ReadUShort();
                    deck.s_nAT_Type[1] = read.ReadUShort();
                    deck.s_nAT_Type[2] = read.ReadUShort();

                    deck.s_nOption[0] = read.ReadUShort();
                    deck.s_nOption[1] = read.ReadUShort();
                    deck.s_nOption[2] = read.ReadUShort();

                    deck.s_nVal[0] = read.ReadUShort();
                    deck.s_nVal[1] = read.ReadUShort();
                    deck.s_nVal[2] = read.ReadUShort();

                    deck.s_nProb[0] = read.ReadUInt();
                    deck.s_nProb[1] = read.ReadUInt();
                    deck.s_nProb[2] = read.ReadUInt();

                    deck.s_nTime[0] = read.ReadUInt();
                    deck.s_nTime[1] = read.ReadUInt();
                    deck.s_nTime[2] = read.ReadUInt();

                    decks[i] = deck;
                }

                var tcount = read.ReadInt();
                DeckComposition[] deckCompositions = new DeckComposition[tcount];
                for (int i = 0; i < tcount; i++)
                {
                    DeckComposition deck = new();
                    deck.s_nGroupIdx = read.ReadUShort();
                    deck.s_nVal = read.ReadUShort();
                    for (int y = 0; y < deck.s_nVal; y++)
                    {
                        DeckDigimon deckDigimon = new();
                        deckDigimon.s_dwBaseDigimonID = read.ReadInt();
                        deckDigimon.s_szBaseDigimonName = read.ReadZString(Encoding.Unicode, 130);
                        deckDigimon.s_nEvolslot = read.ReadUShort();
                        deckDigimon.s_dwDestDigimonID = read.ReadInt();
                        deckDigimon.s_szDestDigimonName = read.ReadZString(Encoding.Unicode, 128);
                        deck.deckDigimons.Add(deckDigimon);
                    }

                    deckCompositions[i] = deck;
                }

                return (books, encyclopediaExceptions, decks, deckCompositions);
            }
        }
        public static void ExportBookInfoToXml(BookInfo[] bookInfos, string filePath)
        {
            var bookInfoElements = new List<XElement>();
            foreach (var bookInfo in bookInfos)
            {
                var element = new XElement("BookInfo",
                    new XElement("s_dwOptID", bookInfo.s_dwOptID),
                    new XElement("s_szOptName", bookInfo.s_szOptName),
                    new XElement("s_nIcon", bookInfo.s_nIcon),
                    new XElement("s_szOptExplain", bookInfo.s_szOptExplain)
                );
                bookInfoElements.Add(element);
            }

            var xml = new XDocument(new XElement("BookInfos", bookInfoElements));
            xml.Save(filePath);
        }
        public static void ExportEncyclopediaExceptionToXml(EncyclopediaException[] encyclopediaExceptions, string filePath)
        {
            var encyclopediaExceptionElements = new List<XElement>();
            foreach (var encyclopediaException in encyclopediaExceptions)
            {
                var element = new XElement("EncyclopediaException",
                    new XElement("s_dwDigimonID", encyclopediaException.s_dwDigimonID),
                    new XElement("s_szName", encyclopediaException.s_szName)
                );
                encyclopediaExceptionElements.Add(element);
            }

            var xml = new XDocument(new XElement("EncyclopediaExceptions", encyclopediaExceptionElements));
            xml.Save(filePath);
        }
        public static void ExportDeckOptionToXml(DeckOption[] deckOptions, string filePath)
        {
            var deckOptionElements = new List<XElement>();
            foreach (var deckOption in deckOptions)
            {
                var element = new XElement("DeckOption",
                    new XElement("s_nGroupIdx", deckOption.s_nGroupIdx),
                    new XElement("s_szGroupName", deckOption.s_szGroupName),
                    new XElement("s_szExplain", deckOption.s_szExplain)
                );

                var conditions = new XElement("s_nCondition");
                foreach (var condition in deckOption.s_nCondition)
                {
                    conditions.Add(new XElement("condition", condition));
                }
                element.Add(conditions);

                var atTypes = new XElement("s_nAT_Type");
                foreach (var atType in deckOption.s_nAT_Type)
                {
                    atTypes.Add(new XElement("atType", atType));
                }
                element.Add(atTypes);

                var options = new XElement("s_nOption");
                foreach (var option in deckOption.s_nOption)
                {
                    options.Add(new XElement("option", option));
                }
                element.Add(options);

                var values = new XElement("s_nVal");
                foreach (var value in deckOption.s_nVal)
                {
                    values.Add(new XElement("value", value));
                }
                element.Add(values);

                var probs = new XElement("s_nProb");
                foreach (var prob in deckOption.s_nProb)
                {
                    probs.Add(new XElement("prob", prob));
                }
                element.Add(probs);

                var times = new XElement("s_nTime");
                foreach (var time in deckOption.s_nTime)
                {
                    times.Add(new XElement("time", time));
                }
                element.Add(times);

                deckOptionElements.Add(element);
            }

            var xml = new XDocument(new XElement("DeckOptions", deckOptionElements));
            xml.Save(filePath);
        }
        public static void ExportDeckCompositionToXml(DeckComposition[] deckCompositions, string filePath)
        {
            var deckCompositionElements = new List<XElement>();
            foreach (var deckComposition in deckCompositions)
            {
                var element = new XElement("DeckComposition",
                    new XElement("s_nGroupIdx", deckComposition.s_nGroupIdx),
                    new XElement("s_szGroupName", deckComposition.s_szGroupName),
                    new XElement("s_nVal", deckComposition.s_nVal)
                );

                var deckDigimonElements = new List<XElement>();
                foreach (var deckDigimon in deckComposition.deckDigimons)
                {
                    var digimonElement = new XElement("DeckDigimon",
                        new XElement("s_dwBaseDigimonID", deckDigimon.s_dwBaseDigimonID),
                        new XElement("s_szBaseDigimonName", deckDigimon.s_szBaseDigimonName),
                        new XElement("s_nEvolslot", deckDigimon.s_nEvolslot),
                        new XElement("s_dwDestDigimonID", deckDigimon.s_dwDestDigimonID),
                        new XElement("s_szDestDigimonName", deckDigimon.s_szDestDigimonName)
                    );
                    deckDigimonElements.Add(digimonElement);
                }
                element.Add(deckDigimonElements);

                deckCompositionElements.Add(element);
            }

            var xml = new XDocument(new XElement("DeckCompositions", deckCompositionElements));
            xml.Save(filePath);
        }
        public static BookInfo[] ImportBookInfosFromXml(string filePath)
        {
            var bookInfos = new List<BookInfo>();

            var xml = XDocument.Load(filePath);
            var elements = xml.Root.Elements("BookInfo");

            foreach (var element in elements)
            {
                var bookInfo = new BookInfo
                {
                    s_dwOptID = (int)element.Element("s_dwOptID"),
                    s_szOptName = (string)element.Element("s_szOptName"),
                    s_nIcon = (ushort)(int)element.Element("s_nIcon"),
                    s_szOptExplain = (string)element.Element("s_szOptExplain")
                };

                bookInfos.Add(bookInfo);
            }

            return bookInfos.ToArray();
        }
        public static EncyclopediaException[] ImportEncyclopediaExceptionsFromXml(string filePath)
        {
            var encyclopediaExceptions = new List<EncyclopediaException>();

            var xml = XDocument.Load(filePath);
            var elements = xml.Root.Elements("EncyclopediaException");

            foreach (var element in elements)
            {
                var encyclopediaException = new EncyclopediaException
                {
                    s_dwDigimonID = (int)element.Element("s_dwDigimonID"),
                    s_szName = (string)element.Element("s_szName")
                };

                encyclopediaExceptions.Add(encyclopediaException);
            }

            return encyclopediaExceptions.ToArray();
        }
        public static DeckOption[] ImportDeckOptionsFromXml(string filePath)
        {
            var deckOptions = new List<DeckOption>();

            var xml = XDocument.Load(filePath);
            var elements = xml.Root.Elements("DeckOption");

            foreach (var element in elements)
            {
                var deckOption = new DeckOption
                {
                    s_nGroupIdx = Convert.ToUInt16(element.Element("s_nGroupIdx").Value),
                    s_szGroupName = (string)element.Element("s_szGroupName"),
                    s_szExplain = (string)element.Element("s_szExplain"),
                    s_nCondition = element.Element("s_nCondition").Elements("condition").Select(e => Convert.ToUInt16(e.Value)).ToArray(),
                    s_nAT_Type = element.Element("s_nAT_Type").Elements("atType").Select(e => Convert.ToUInt16(e.Value)).ToArray(),
                    s_nOption = element.Element("s_nOption").Elements("option").Select(e => Convert.ToUInt16(e.Value)).ToArray(),
                    s_nVal = element.Element("s_nVal").Elements("value").Select(e => Convert.ToUInt16(e.Value)).ToArray(),
                    s_nProb = element.Element("s_nProb").Elements("prob").Select(e => Convert.ToUInt32(e.Value)).ToArray(),
                    s_nTime = element.Element("s_nTime").Elements("time").Select(e => Convert.ToUInt32(e.Value)).ToArray()
                };

                deckOptions.Add(deckOption);
            }

            return deckOptions.ToArray();
        }
        public static DeckComposition[] ImportDeckCompositionsFromXml(string filePath)
        {
            var deckCompositions = new List<DeckComposition>();

            var xml = XDocument.Load(filePath);
            var elements = xml.Root.Elements("DeckComposition");

            foreach (var element in elements)
            {
                var deckComposition = new DeckComposition
                {
                    s_nGroupIdx = (ushort)(int)element.Element("s_nGroupIdx"),
                    s_szGroupName = (string)element.Element("s_szGroupName"),
                    s_nVal = (ushort)(int)element.Element("s_nVal")
                };

                var deckDigimonElements = element.Elements("DeckDigimon");
                foreach (var digimonElement in deckDigimonElements)
                {
                    var deckDigimon = new DeckDigimon
                    {
                        s_dwBaseDigimonID = (int)digimonElement.Element("s_dwBaseDigimonID"),
                        s_szBaseDigimonName = (string)digimonElement.Element("s_szBaseDigimonName"),
                        s_nEvolslot = (ushort)(int)digimonElement.Element("s_nEvolslot"),
                        s_dwDestDigimonID = (int)digimonElement.Element("s_dwDestDigimonID"),
                        s_szDestDigimonName = (string)digimonElement.Element("s_szDestDigimonName")
                    };

                    deckComposition.deckDigimons.Add(deckDigimon);
                }

                deckCompositions.Add(deckComposition);
            }

            return deckCompositions.ToArray();
        }

        public static void WriteDigimon_BookToBinary(string OutputFile, BookInfo[] bookInfos, EncyclopediaException[] encyclopediaExceptions, DeckOption[] deckOptions, DeckComposition[] deckCompositions)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(OutputFile, FileMode.Create)))
            {

                writer.Write(bookInfos.Length);
                foreach (var book in bookInfos)
                {
                    writer.Write(book.s_dwOptID);
                    for (int i = 0; i < 64; i++)
                    {
                        char c = i < book.s_szOptName.Length ? book.s_szOptName[i] : '\0';
                        writer.Write((ushort)c);
                    }

                    writer.Write(book.s_nIcon);

                    for (int i = 0; i < 513; i++)
                    {
                        char c = i < book.s_szOptExplain.Length ? book.s_szOptExplain[i] : '\0';
                        writer.Write((ushort)c);
                    }
                }

                writer.Write(encyclopediaExceptions.Length);
                foreach (var book in encyclopediaExceptions)
                {
                    writer.Write(book.s_dwDigimonID);

                    for (int i = 0; i < 64; i++)
                    {
                        char c = i < book.s_szName.Length ? book.s_szName[i] : '\0';
                        writer.Write((ushort)c);
                    }
                }

                writer.Write(deckOptions.Length);
                foreach (var deck in deckOptions)
                {

                    writer.Write(deck.s_nGroupIdx);

                    for (int i = 0; i < 64; i++)
                    {
                        char c = i < deck.s_szGroupName.Length ? deck.s_szGroupName[i] : '\0';
                        writer.Write((ushort)c);
                    }
                    for (int i = 0; i < 513; i++)
                    {
                        char c = i < deck.s_szExplain.Length ? deck.s_szExplain[i] : '\0';
                        writer.Write((ushort)c);
                    }

                    writer.Write(deck.s_nCondition[0]);
                    writer.Write(deck.s_nCondition[1]);
                    writer.Write(deck.s_nCondition[2]);

                    writer.Write(deck.s_nAT_Type[0]);
                    writer.Write(deck.s_nAT_Type[1]);
                    writer.Write(deck.s_nAT_Type[2]);

                    writer.Write(deck.s_nOption[0]);
                    writer.Write(deck.s_nOption[1]);
                    writer.Write(deck.s_nOption[2]);

                    writer.Write(deck.s_nVal[0]);
                    writer.Write(deck.s_nVal[1]);
                    writer.Write(deck.s_nVal[2]);

                    writer.Write(deck.s_nProb[0]);
                    writer.Write(deck.s_nProb[1]);
                    writer.Write(deck.s_nProb[2]);

                    writer.Write(deck.s_nTime[0]);
                    writer.Write(deck.s_nTime[1]);
                    writer.Write(deck.s_nTime[2]);
                }

                writer.Write(deckCompositions.Length);
                foreach (var deck in deckCompositions)
                {
                    writer.Write(deck.s_nGroupIdx);
                    writer.Write(deck.s_nVal);
                    foreach (var deckDigimon  in deck.deckDigimons)
                    {
                        writer.Write(deckDigimon.s_dwBaseDigimonID);
                     
                        for (int i = 0; i < 65; i++)
                        {
                            char c = i < deckDigimon.s_szBaseDigimonName.Length ? deckDigimon.s_szBaseDigimonName[i] : '\0';
                            writer.Write((ushort)c);
                        }
                        writer.Write(deckDigimon.s_nEvolslot);
                        writer.Write(deckDigimon.s_dwDestDigimonID);

                        for (int s = 0; s < 64; s++)
                        {
                            char c = s < deckDigimon.s_szDestDigimonName.Length ? deckDigimon.s_szDestDigimonName[s] : '\0';
                            writer.Write((ushort)c);
                        }
                    }
                }

            }
        }
        public static string CleanString(string input)
        {
            int nullIndex = input.IndexOf('\0');
            if (nullIndex >= 0)
            {
                return input.Substring(0, nullIndex);
            }
            else
            {
                return input;
            }
        }
    }
}
