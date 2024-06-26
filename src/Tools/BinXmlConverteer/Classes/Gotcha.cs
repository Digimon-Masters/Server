using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace BinXmlConverter.Classes
{
    public class GotchaMachine
    {
        public class Gotcha
        {
            public int s_dwNpc_Id;
            public int s_dwUseItem_Code;
            public ushort s_nUseItem_Cnt;
            public ushort s_bLimit;
            public uint s_nStart_Date;
            public uint s_nEnd_Date;
            public uint s_nStart_Time;
            public uint s_nEnd_Time;
            public ushort s_nMin_Lv;
            public ushort s_nMax_Lv;
            public int nRareItemCnt;
        }
        public class GotchaItems
        {
            public ushort s_nGroup_Id;
            public ushort s_Level;
            public int[] s_dwItem_Code;
            public ushort[] s_nItem_Cnt;
        }
        public class GotchaRareItems
        {
            public uint s_nNpcID;
            public string SzNameRareItem;
            public uint s_nRareItem;
            public uint s_nRareItemCnt;
            public uint s_nRareItemGive;
        }
        public class GotchaMysteryItem
        {
            public ushort s_nGroup;        //Número do grupo
            public int s_nItem;           //Número de item
            public ushort s_nEffect;

        }
        public class GotchaMysteryCoin
        {
            public ushort s_nCoinOrder;        //Prioridade de moedas
            public uint s_nCoinIdx;            //Número do Item da Moeda
            public ushort s_nCoinCnt;		//Número de moedas consumidas
        }

        public static (Gotcha[], GotchaItems[], GotchaRareItems[], GotchaMysteryItem[], GotchaMysteryCoin[]) ReadGotchaFromBinary(string filePath)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                // Lê os Gotchas
                Gotcha[] gotchas = ReadGotchas(reader);

                // Lê os GotchaItems
                GotchaItems[] gotchaItems = ReadGotchaItems(reader);

                // Lê os GotchaRareItems
                GotchaRareItems[] gotchaRareItems = ReadGotchaRareItems(reader);

                // Lê os GotchaMysteryItems
                GotchaMysteryItem[] gotchaMysteryItems = ReadGotchaMysteryItems(reader);

                // Lê os GotchaMysteryCoins
                GotchaMysteryCoin[] gotchaMysteryCoins = ReadGotchaMysteryCoins(reader);

                return (gotchas, gotchaItems, gotchaRareItems, gotchaMysteryItems, gotchaMysteryCoins);
            }
        }
        public static Gotcha[] ReadGotchas(BinaryReader reader)
        {
            // Lê a quantidade de Gotchas
            int gotchaCount = reader.ReadInt32();

            // Cria um array para armazenar os Gotchas
            Gotcha[] gotchas = new Gotcha[gotchaCount];

            for (int i = 0; i < gotchaCount; i++)
            {
                // Cria uma nova instância de Gotcha
                Gotcha gotcha = new Gotcha();

                // Lê os campos inteiros diretamente
                gotcha.s_dwNpc_Id = reader.ReadInt32();
                gotcha.s_dwUseItem_Code = reader.ReadInt32();
                gotcha.s_nUseItem_Cnt = reader.ReadUInt16();
                gotcha.s_bLimit = reader.ReadUInt16();
                gotcha.s_nStart_Date = reader.ReadUInt32();
                gotcha.s_nEnd_Date = reader.ReadUInt32();
                gotcha.s_nStart_Time = reader.ReadUInt32();
                gotcha.s_nEnd_Time = reader.ReadUInt32();
                gotcha.s_nMin_Lv = reader.ReadUInt16();
                gotcha.s_nMax_Lv = reader.ReadUInt16();
                gotcha.nRareItemCnt = reader.ReadInt32();

                // Adiciona o Gotcha ao array de Gotchas
                gotchas[i] = gotcha;
            }

            return gotchas;
        }
        public static GotchaItems[] ReadGotchaItems(BinaryReader reader)
        {
            // Lê a quantidade de GotchaItems
            int gotchaItemCount = reader.ReadInt32();

            // Cria um array para armazenar os GotchaItems
            GotchaItems[] gotchaItems = new GotchaItems[gotchaItemCount];

            for (int i = 0; i < gotchaItemCount; i++)
            {
                // Cria uma nova instância de GotchaItems
                GotchaItems gotchaItem = new GotchaItems();

                // Lê os campos inteiros diretamente
                gotchaItem.s_nGroup_Id = reader.ReadUInt16();
                gotchaItem.s_Level = reader.ReadUInt16();


                // Cria um array para armazenar os s_dwItem_Code
                gotchaItem.s_dwItem_Code = new int[10];
                gotchaItem.s_nItem_Cnt = new ushort[10];

                for (int j = 0; j < 10; j++)
                {
                    gotchaItem.s_dwItem_Code[j] = reader.ReadInt32();
                }
                for (int x = 0; x < 10; x++)
                {
 
                    gotchaItem.s_nItem_Cnt[x] = reader.ReadUInt16();
                }
                gotchaItems[i] = gotchaItem;
            }

            return gotchaItems;
        }
        public static GotchaRareItems[] ReadGotchaRareItems(BinaryReader reader)
        {
            // Lê a quantidade de GotchaRareItems
            int gotchaRareItemCount = reader.ReadInt32();

            // Cria um array para armazenar os GotchaRareItems
            GotchaRareItems[] gotchaRareItems = new GotchaRareItems[gotchaRareItemCount];

            for (int i = 0; i < gotchaRareItemCount; i++)
            {
                GotchaRareItems gotchaRareItem = new GotchaRareItems();

                // Lê os campos s_nNpcID e s_szRareItemName diretamente
                gotchaRareItem.s_nNpcID = reader.ReadUInt32();
                byte[] nameBytes = reader.ReadBytes(128);
                string nameString = System.Text.Encoding.Unicode.GetString(nameBytes, 0, 128);
                gotchaRareItem.SzNameRareItem = CleanString(nameString);
                gotchaRareItem.s_nRareItem = reader.ReadUInt32();
                gotchaRareItem.s_nRareItemCnt = reader.ReadUInt32();
                gotchaRareItem.s_nRareItemGive = reader.ReadUInt32();

                // Adiciona o GotchaRareItems ao array de GotchaRareItems
                gotchaRareItems[i] = gotchaRareItem;
            }

            return gotchaRareItems;
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

        public static GotchaMysteryItem[] ReadGotchaMysteryItems(BinaryReader reader)
        {
            // Lê a quantidade de GotchaMysteryItems
            int gotchaMysteryItemCount = reader.ReadInt32();

            // Cria um array para armazenar os GotchaMysteryItems
            GotchaMysteryItem[] gotchaMysteryItems = new GotchaMysteryItem[gotchaMysteryItemCount];

            for (int i = 0; i < gotchaMysteryItemCount; i++)
            {
                GotchaMysteryItem gotchaMysteryItem = new GotchaMysteryItem();

                // Lê os campos inteiros diretamente
                gotchaMysteryItem.s_nGroup = reader.ReadUInt16();
                gotchaMysteryItem.s_nItem = reader.ReadInt32();
                gotchaMysteryItem.s_nEffect = reader.ReadUInt16();

                // Adiciona o GotchaMysteryItem ao array de GotchaMysteryItems
                gotchaMysteryItems[i] = gotchaMysteryItem;
            }

            return gotchaMysteryItems;
        }
        public static GotchaMysteryCoin[] ReadGotchaMysteryCoins(BinaryReader reader)
        {
            // Lê a quantidade de GotchaMysteryItems
            int GotchaMysteryCoinItemCount = reader.ReadInt32();

            // Cria um array para armazenar os GotchaMysteryItems
            GotchaMysteryCoin[] GotchaMysteryCoins = new GotchaMysteryCoin[GotchaMysteryCoinItemCount];

            for (int i = 0; i < GotchaMysteryCoinItemCount; i++)
            {
                GotchaMysteryCoin gotchaMysteryItem = new GotchaMysteryCoin();

                // Lê os campos inteiros diretamente
                gotchaMysteryItem.s_nCoinOrder = reader.ReadUInt16();
                gotchaMysteryItem.s_nCoinIdx = reader.ReadUInt32();
                gotchaMysteryItem.s_nCoinCnt = reader.ReadUInt16();

                // Adiciona o GotchaMysteryItem ao array de GotchaMysteryItems
                GotchaMysteryCoins[i] = gotchaMysteryItem;
            }

            return GotchaMysteryCoins;
        }
        public static void ExportGotchasToXml(string filePath, Gotcha[] gotchas)
        {
            XElement rootElement = new XElement("Gotchas");

            foreach (Gotcha gotcha in gotchas)
            {
                XElement gotchaElement = new XElement("Gotcha",
                    new XElement("s_dwNpc_Id", gotcha.s_dwNpc_Id),
                    new XElement("s_dwUseItem_Code", gotcha.s_dwUseItem_Code),
                    new XElement("s_nUseItem_Cnt", gotcha.s_nUseItem_Cnt),
                    new XElement("s_bLimit", gotcha.s_bLimit),
                    new XElement("s_nStart_Date", gotcha.s_nStart_Date),
                    new XElement("s_nEnd_Date", gotcha.s_nEnd_Date),
                    new XElement("s_nStart_Time", gotcha.s_nStart_Time),
                    new XElement("s_nEnd_Time", gotcha.s_nEnd_Time),
                    new XElement("s_nMin_Lv", gotcha.s_nMin_Lv),
                    new XElement("s_nMax_Lv", gotcha.s_nMax_Lv),
                    new XElement("nRareItemCnt", gotcha.nRareItemCnt)
                );

                rootElement.Add(gotchaElement);
            }

            XDocument xmlDoc = new XDocument(rootElement);
            xmlDoc.Save(filePath);
        }
        public static void ExportGotchaItemsToXml(string filePath, GotchaItems[] gotchaItems)
        {
            // Cria um documento XML
            XmlDocument xmlDoc = new XmlDocument();

            // Cria o elemento raiz do documento
            XmlElement rootElement = xmlDoc.CreateElement("GotchaItems");
            xmlDoc.AppendChild(rootElement);

            // Itera sobre os gotchaItems e cria elementos para cada um
            foreach (GotchaItems gotchaItem in gotchaItems)
            {
                // Cria um elemento para o gotchaItem
                XmlElement gotchaItemElement = xmlDoc.CreateElement("GotchaItem");
                rootElement.AppendChild(gotchaItemElement);

                // Adiciona os atributos ao elemento do gotchaItem
                gotchaItemElement.SetAttribute("Group_Id", gotchaItem.s_nGroup_Id.ToString());
                gotchaItemElement.SetAttribute("Level", gotchaItem.s_Level.ToString());

                // Cria um elemento para armazenar os s_dwItem_Code
                XmlElement itemCodeElement = xmlDoc.CreateElement("ItemCode");
                gotchaItemElement.AppendChild(itemCodeElement);

                // Itera sobre os s_dwItem_Code e cria elementos para cada um
                for (int i = 0; i < gotchaItem.s_dwItem_Code.Length; i++)
                {
                    // Cria um elemento para o s_dwItem_Code
                    XmlElement itemCodeValueElement = xmlDoc.CreateElement("ItemCodeValue");
                    itemCodeValueElement.InnerText = gotchaItem.s_dwItem_Code[i].ToString();
                    itemCodeElement.AppendChild(itemCodeValueElement);

                    // Cria um elemento para o itemCodeCount
                    XmlElement itemCodeCountValueElement = xmlDoc.CreateElement("itemCodeCount");
                    itemCodeCountValueElement.InnerText = gotchaItem.s_nItem_Cnt[i].ToString();
                    itemCodeElement.AppendChild(itemCodeCountValueElement);
                }

            }

            // Salva o documento XML no arquivo
            xmlDoc.Save(filePath);
        }
        public static void ExportGotchaRareItemsToXml(GotchaRareItems[] gotchaRareItems, string filePath)
        {
            XmlDocument xmlDoc = new XmlDocument();

            XmlElement rootElement = xmlDoc.CreateElement("GotchaRareItems");
            xmlDoc.AppendChild(rootElement);

            foreach (GotchaRareItems gotchaRareItem in gotchaRareItems)
            {
                XmlElement gotchaRareItemElement = xmlDoc.CreateElement("GotchaRareItem");
                gotchaRareItemElement.SetAttribute("NpcID", gotchaRareItem.s_nNpcID.ToString());
                gotchaRareItemElement.SetAttribute("SzNameRareItem", gotchaRareItem.SzNameRareItem.ToString());
                gotchaRareItemElement.SetAttribute("RareItem", gotchaRareItem.s_nRareItem.ToString());
                gotchaRareItemElement.SetAttribute("RareItemCnt", gotchaRareItem.s_nRareItemCnt.ToString());
                gotchaRareItemElement.SetAttribute("RareItemGive", gotchaRareItem.s_nRareItemGive.ToString());

                rootElement.AppendChild(gotchaRareItemElement);
            }

            xmlDoc.Save(filePath);
        }
        public static void ExportGotchaMysteryItemsToXml(GotchaMysteryItem[] gotchaMysteryItems, string filePath)
        {
            XmlDocument xmlDoc = new XmlDocument();

            XmlElement rootElement = xmlDoc.CreateElement("GotchaMysteryItems");
            xmlDoc.AppendChild(rootElement);

            foreach (GotchaMysteryItem gotchaMysteryItem in gotchaMysteryItems)
            {
                XmlElement gotchaMysteryItemElement = xmlDoc.CreateElement("GotchaMysteryItem");
                gotchaMysteryItemElement.SetAttribute("Group", gotchaMysteryItem.s_nGroup.ToString());
                gotchaMysteryItemElement.SetAttribute("Item", gotchaMysteryItem.s_nItem.ToString());
                gotchaMysteryItemElement.SetAttribute("Effect", gotchaMysteryItem.s_nEffect.ToString());

                rootElement.AppendChild(gotchaMysteryItemElement);
            }

            xmlDoc.Save(filePath);
        }
        public static void ExportGotchaMysteryCoinsToXml(GotchaMysteryCoin[] gotchaMysteryCoins, string filePath)
        {
            XmlDocument xmlDoc = new XmlDocument();

            XmlElement rootElement = xmlDoc.CreateElement("GotchaMysteryCoins");
            xmlDoc.AppendChild(rootElement);

            foreach (GotchaMysteryCoin gotchaMysteryCoin in gotchaMysteryCoins)
            {
                XmlElement gotchaMysteryCoinElement = xmlDoc.CreateElement("GotchaMysteryCoin");
                gotchaMysteryCoinElement.SetAttribute("CoinOrder", gotchaMysteryCoin.s_nCoinOrder.ToString());
                gotchaMysteryCoinElement.SetAttribute("CoinIdx", gotchaMysteryCoin.s_nCoinIdx.ToString());
                gotchaMysteryCoinElement.SetAttribute("CoinCnt", gotchaMysteryCoin.s_nCoinCnt.ToString());

                rootElement.AppendChild(gotchaMysteryCoinElement);
            }

            xmlDoc.Save(filePath);
        }
        public static Gotcha[] ImportGotchasFromXml(string filePath)
        {
            // Carrega o documento XML do arquivo
            XElement rootElement = XElement.Load(filePath);

            // Obtém todos os elementos "Gotcha"
            IEnumerable<XElement> gotchaElements = rootElement.Elements("Gotcha");

            // Cria um array para armazenar os Gotchas
            Gotcha[] gotchas = new Gotcha[gotchaElements.Count()];

            // Itera sobre os elementos "Gotcha" e cria instâncias de Gotcha
            int i = 0;
            foreach (XElement gotchaElement in gotchaElements)
            {
                // Cria uma nova instância de Gotcha
                Gotcha gotcha = new Gotcha();

                // Lê os elementos e atribui aos campos do Gotcha
                gotcha.s_dwNpc_Id = int.Parse(gotchaElement.Element("s_dwNpc_Id").Value);
                gotcha.s_dwUseItem_Code = int.Parse(gotchaElement.Element("s_dwUseItem_Code").Value);
                gotcha.s_nUseItem_Cnt = ushort.Parse(gotchaElement.Element("s_nUseItem_Cnt").Value);
                gotcha.s_bLimit = ushort.Parse(gotchaElement.Element("s_bLimit").Value);
                gotcha.s_nStart_Date = uint.Parse(gotchaElement.Element("s_nStart_Date").Value);
                gotcha.s_nEnd_Date = uint.Parse(gotchaElement.Element("s_nEnd_Date").Value);
                gotcha.s_nStart_Time = uint.Parse(gotchaElement.Element("s_nStart_Time").Value);
                gotcha.s_nEnd_Time = uint.Parse(gotchaElement.Element("s_nEnd_Time").Value);
                gotcha.s_nMin_Lv = ushort.Parse(gotchaElement.Element("s_nMin_Lv").Value);
                gotcha.s_nMax_Lv = ushort.Parse(gotchaElement.Element("s_nMax_Lv").Value);
                gotcha.nRareItemCnt = int.Parse(gotchaElement.Element("nRareItemCnt").Value);

                // Adiciona o Gotcha ao array de Gotchas
                gotchas[i] = gotcha;
                i++;
            }

            return gotchas;

        }
        public static GotchaItems[] ImportGotchaItemsFromXml(string filePath)
        {
            // Carrega o documento XML do arquivo
            XElement rootElement = XElement.Load(filePath);

            // Obtém todos os elementos "GotchaItem"
            IEnumerable<XElement> gotchaItemElements = rootElement.Elements("GotchaItem");

            // Cria um array para armazenar os GotchaItems
            GotchaItems[] gotchaItems = new GotchaItems[gotchaItemElements.Count()];

            // Itera sobre os elementos "GotchaItem" e cria instâncias de GotchaItems
            int i = 0;
            foreach (XElement gotchaItemElement in gotchaItemElements)
            {
                // Cria uma nova instância de GotchaItems
                GotchaItems gotchaItem = new GotchaItems();

                // Lê os atributos do elemento e atribui aos campos do GotchaItems
                gotchaItem.s_nGroup_Id = ushort.Parse(gotchaItemElement.Attribute("Group_Id").Value);
                gotchaItem.s_Level = ushort.Parse(gotchaItemElement.Attribute("Level").Value);

                // Obtém o elemento "ItemCode" dentro do elemento "GotchaItem"
                XElement itemCodeElement = gotchaItemElement.Element("ItemCode");

                // Obtém todos os elementos "ItemCodeValue" dentro do elemento "ItemCode"
                IEnumerable<XElement> itemCodeValueElements = itemCodeElement.Elements("ItemCodeValue");

                // Cria um array para armazenar os s_dwItem_Code
                gotchaItem.s_dwItem_Code = new int[itemCodeValueElements.Count()];

                // Obtém todos os elementos "itemCodeCount" dentro do elemento "ItemCode"
                IEnumerable<XElement> itemCodeCountElements = itemCodeElement.Elements("itemCodeCount");

                // Cria um array para armazenar os s_nItem_Cnt
                gotchaItem.s_nItem_Cnt = new ushort[itemCodeCountElements.Count()];

                // Itera sobre os elementos "ItemCodeValue" e atribui aos s_dwItem_Code
                int j = 0;
                foreach (XElement itemCodeValueElement in itemCodeValueElements)
                {
                    gotchaItem.s_dwItem_Code[j] = int.Parse(itemCodeValueElement.Value);
                    j++;
                }

                // Itera sobre os elementos "itemCodeCount" e atribui aos s_nItem_Cnt
                j = 0;
                foreach (XElement itemCodeCountElement in itemCodeCountElements)
                {
                    gotchaItem.s_nItem_Cnt[j] = ushort.Parse(itemCodeCountElement.Value);
                    j++;
                }

                // Adiciona o GotchaItems ao array de GotchaItems
                gotchaItems[i] = gotchaItem;
                i++;
            }

            return gotchaItems;
        }
        public static GotchaRareItems[] ImportGotchaRareItemsFromXml(string filePath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            XmlNodeList itemNodes = xmlDoc.SelectNodes("//GotchaRareItems/GotchaRareItem");

            GotchaRareItems[] gotchaRareItems = new GotchaRareItems[itemNodes.Count];

            for (int i = 0; i < itemNodes.Count; i++)
            {
                XmlNode itemNode = itemNodes[i];

                GotchaRareItems gotchaRareItem = new GotchaRareItems();
                gotchaRareItem.s_nNpcID = uint.Parse(itemNode.Attributes["NpcID"].Value);
                gotchaRareItem.SzNameRareItem =  itemNode.Attributes["SzNameRareItem"].Value;
                gotchaRareItem.s_nRareItem = uint.Parse(itemNode.Attributes["RareItem"].Value);
                gotchaRareItem.s_nRareItemCnt = uint.Parse(itemNode.Attributes["RareItemCnt"].Value);
                gotchaRareItem.s_nRareItemGive = uint.Parse(itemNode.Attributes["RareItemGive"].Value);

                gotchaRareItems[i] = gotchaRareItem;
            }

            return gotchaRareItems;
        }
        public static GotchaMysteryItem[] ImportGotchaMysteryItemsFromXml(string filePath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            XmlNodeList itemNodes = xmlDoc.SelectNodes("//GotchaMysteryItems/GotchaMysteryItem");

            GotchaMysteryItem[] gotchaMysteryItems = new GotchaMysteryItem[itemNodes.Count];

            for (int i = 0; i < itemNodes.Count; i++)
            {
                XmlNode itemNode = itemNodes[i];

                GotchaMysteryItem gotchaMysteryItem = new GotchaMysteryItem();
                gotchaMysteryItem.s_nGroup = ushort.Parse(itemNode.Attributes["Group"].Value);
                gotchaMysteryItem.s_nItem = int.Parse(itemNode.Attributes["Item"].Value);
                gotchaMysteryItem.s_nEffect = ushort.Parse(itemNode.Attributes["Effect"].Value);

                gotchaMysteryItems[i] = gotchaMysteryItem;
            }

            return gotchaMysteryItems;
        }
        public static GotchaMysteryCoin[] ImportGotchaMysteryCoinsFromXml(string filePath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            XmlNodeList itemNodes = xmlDoc.SelectNodes("//GotchaMysteryCoins/GotchaMysteryCoin");

            GotchaMysteryCoin[] gotchaMysteryCoins = new GotchaMysteryCoin[itemNodes.Count];

            for (int i = 0; i < itemNodes.Count; i++)
            {
                XmlNode itemNode = itemNodes[i];

                GotchaMysteryCoin gotchaMysteryCoin = new GotchaMysteryCoin();
                gotchaMysteryCoin.s_nCoinOrder = ushort.Parse(itemNode.Attributes["CoinOrder"].Value);
                gotchaMysteryCoin.s_nCoinIdx = uint.Parse(itemNode.Attributes["CoinIdx"].Value);
                gotchaMysteryCoin.s_nCoinCnt = ushort.Parse(itemNode.Attributes["CoinCnt"].Value);

                gotchaMysteryCoins[i] = gotchaMysteryCoin;
            }

            return gotchaMysteryCoins;
        }
        public static void WriteGotchaToBinary(string filePath, Gotcha[] gotchas, GotchaItems[] gotchaItems, GotchaRareItems[] gotchaRareItems, GotchaMysteryItem[] gotchaMysteryItems, GotchaMysteryCoin[] gotchaMysteryCoins)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {
                // Escreve a quantidade de Gotchas
                writer.Write(gotchas.Length);

                // Escreve cada Gotcha
                foreach (Gotcha gotcha in gotchas)
                {
                    writer.Write(gotcha.s_dwNpc_Id);
                    writer.Write(gotcha.s_dwUseItem_Code);
                    writer.Write(gotcha.s_nUseItem_Cnt);
                    writer.Write(gotcha.s_bLimit);
                    writer.Write(gotcha.s_nStart_Date);
                    writer.Write(gotcha.s_nEnd_Date);
                    writer.Write(gotcha.s_nStart_Time);
                    writer.Write(gotcha.s_nEnd_Time);
                    writer.Write(gotcha.s_nMin_Lv);
                    writer.Write(gotcha.s_nMax_Lv);
                    writer.Write(gotcha.nRareItemCnt);
                }

                // Escreve a quantidade de GotchaItems
                writer.Write(gotchaItems.Length);

                // Escreve cada GotchaItems
                foreach (GotchaItems gotchaItem in gotchaItems)
                {
                    writer.Write(gotchaItem.s_nGroup_Id);
                    writer.Write(gotchaItem.s_Level);

                    for (int i = 0; i < 10; i++)
                    {
                        writer.Write(gotchaItem.s_dwItem_Code[i]);
                    }
                    for (int x = 0; x < 10; x++)
                    {
                        writer.Write(gotchaItem.s_nItem_Cnt[x]);
                    }
                }

                // Escreve a quantidade de GotchaRareItems
                writer.Write(gotchaRareItems.Length);

                // Escreve cada GotchaRareItems
                foreach (GotchaRareItems gotchaRareItem in gotchaRareItems)
                {
                    writer.Write(gotchaRareItem.s_nNpcID);
                    byte[] nameBytes = Encoding.Unicode.GetBytes(gotchaRareItem.SzNameRareItem);

                    // Verificar se a string tem mais de 64 caracteres
                    if (nameBytes.Length > 128)
                    {
                        // Se tiver mais de 64 caracteres, truncar a string para 64 caracteres
                        Array.Resize(ref nameBytes,128);
                    }
                    else if (nameBytes.Length < 128)
                    {
                        // Se tiver menos de 64 caracteres, preencher com bytes nulos até alcançar 64 caracteres
                        Array.Resize(ref nameBytes, 128);
                        Array.Clear(nameBytes, nameBytes.Length, 128 - nameBytes.Length);
                    }

                    // Gravar os bytes no arquivo binário
                    writer.Write(nameBytes);
                    writer.Write(gotchaRareItem.s_nRareItem);
                    writer.Write(gotchaRareItem.s_nRareItemCnt);
                    writer.Write(gotchaRareItem.s_nRareItemGive);
                }

                // Escreve a quantidade de GotchaMysteryItems
                writer.Write(gotchaMysteryItems.Length);

                // Escreve cada GotchaMysteryItems
                foreach (GotchaMysteryItem gotchaMysteryItem in gotchaMysteryItems)
                {
                    writer.Write(gotchaMysteryItem.s_nGroup);
                    writer.Write(gotchaMysteryItem.s_nItem);
                    writer.Write(gotchaMysteryItem.s_nEffect);
                }

                // Escreve a quantidade de GotchaMysteryCoins
                writer.Write(gotchaMysteryCoins.Length);

                // Escreve cada GotchaMysteryCoins
                foreach (GotchaMysteryCoin gotchaMysteryCoin in gotchaMysteryCoins)
                {
                    writer.Write(gotchaMysteryCoin.s_nCoinOrder);
                    writer.Write(gotchaMysteryCoin.s_nCoinIdx);
                    writer.Write(gotchaMysteryCoin.s_nCoinCnt);
                }
            }
        }
    }
}
