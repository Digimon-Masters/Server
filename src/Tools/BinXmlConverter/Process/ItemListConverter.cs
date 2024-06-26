using System.Text;
using System.Xml.Linq;

namespace DSO.BinXmlConverter.Process
{
    public class ItemListConverter : IProcessBase
    {
        //BIN
        private string _dataBinFile = "DATA\\Bin\\ItemList.bin";
        private string _outputBinFile = "Output\\Bin\\ItemList.bin";

        //XML
        private string _dataXmlFile_BaseItem = "DATA\\Xml\\1BaseItemList.xml";
        private string _dataXmlFile_ItemTap = "DATA\\Xml\\2ItemTapList.xml";
        private string _dataXmlFile_ItemCooldown = "DATA\\Xml\\3ItemCooldownList.xml";
        private string _dataXmlFile_ItemDisplay = "DATA\\Xml\\4ItemDisplayList.xml";
        private string _dataXmlFile_ItemTypeName = "DATA\\Xml\\5ItemTypeNameList.xml";
        private string _dataXmlFile_ItemRank = "DATA\\Xml\\6ItemRankList.xml";
        private string _dataXmlFile_ItemElementBase = "DATA\\Xml\\7ItemElementBaseList.xml";
        private string _dataXmlFile_ItemElement = "DATA\\Xml\\8ItemElementList.xml";
        private string _dataXmlFile_ItemExchange = "DATA\\Xml\\9ItemExchangeList.xml";
        private string _dataXmlFile_ItemAcessory = "DATA\\Xml\\10ItemAcessoryList.xml";
        private string _dataXmlFile_ItemAcessoryEnchant = "DATA\\Xml\\11ItemAcessoryEnchantList.xml";
        private string _dataXmlFile_ItemCraft = "DATA\\Xml\\12ItemCraftList.xml";
        private string _dataXmlFile_ItemCraftGroup = "DATA\\Xml\\13ItemCraftGroupList.xml";
        private string _dataXmlFile_ItemXai = "DATA\\Xml\\14ItemXaiList.xml";
        private string _dataXmlFile_ItemRankEffect = "DATA\\Xml\\15ItemRankEffectList.xml";
        private string _dataXmlFile_ItemLook = "DATA\\Xml\\16ItemLookList.xml";

        public void XmlToBin()
        {
            if (!File.Exists(_dataXmlFile_BaseItem)) return;
            if (!File.Exists(_dataXmlFile_ItemTap)) return;
            if (!File.Exists(_dataXmlFile_ItemCooldown)) return;
            if (!File.Exists(_dataXmlFile_ItemDisplay)) return;
            if (!File.Exists(_dataXmlFile_ItemTypeName)) return;
            if (!File.Exists(_dataXmlFile_ItemRank)) return;
            if (!File.Exists(_dataXmlFile_ItemElementBase)) return;
            if (!File.Exists(_dataXmlFile_ItemElement)) return;
            if (!File.Exists(_dataXmlFile_ItemExchange)) return;
            if (!File.Exists(_dataXmlFile_ItemAcessory)) return;
            if (!File.Exists(_dataXmlFile_ItemAcessoryEnchant)) return;
            if (!File.Exists(_dataXmlFile_ItemCraft)) return;
            if (!File.Exists(_dataXmlFile_ItemCraftGroup)) return;
            if (!File.Exists(_dataXmlFile_ItemXai)) return;
            if (!File.Exists(_dataXmlFile_ItemRankEffect)) return;
            if (!File.Exists(_dataXmlFile_ItemLook)) return;

            if (File.Exists(_outputBinFile))
            {
                File.Delete(_outputBinFile);

                Thread.Sleep(1000);
            }

            using (var stream = File.Open(_outputBinFile, FileMode.OpenOrCreate))
            {
                using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
                {
                    var xDoc = XDocument.Load(_dataXmlFile_BaseItem);
                    var items = xDoc.Descendants().Where(x => x.Name.LocalName == "Item").ToList();
                    writer.Write(items.Count);

                    foreach (var item in items)
                    {
                        writer.Write(Convert.ToInt32(item.Attribute("Id")?.Value));

                        var stringBytes = Util.GetStringBytes(Util.RemoveCdataFromString(item.Element("Name")?.Value), 128);
                        writer.Write(stringBytes, 0, stringBytes.Length);

                        writer.Write(Convert.ToInt32(item.Element("Icon")?.Value));

                        stringBytes = Util.GetStringBytes(Util.RemoveCdataFromString(item.Element("Comment")?.Value), 1024);
                        writer.Write(stringBytes, 0, stringBytes.Length);

                        stringBytes = Util.GetStringBytes(Util.RemoveCdataFromString(item.Element("Nif")?.Value), 64);
                        writer.Write(stringBytes, 0, stringBytes.Length);

                        writer.Write(Convert.ToUInt16(item.Element("Class")?.Value));

                        stringBytes = Util.GetStringBytes(Util.RemoveCdataFromString(item.Element("TypeComment")?.Value), 128);
                        writer.Write(stringBytes, 0, stringBytes.Length);

                        writer.Write(Convert.ToUInt16(item.Element("TypeL")?.Value));
                        writer.Write(Convert.ToUInt16(item.Element("TypeL1")?.Value));
                        writer.Write(Convert.ToUInt16(item.Element("TypeS")?.Value));
                        writer.Write(Convert.ToUInt16(item.Element("UseType")?.Value));
                        writer.Write(Convert.ToUInt16(item.Element("BoundType")?.Value));
                        writer.Write(Convert.ToInt32(item.Element("TypeValue")?.Value));
                        writer.Write(Convert.ToUInt16(item.Element("SellType")?.Value));
                        writer.Write(Convert.ToUInt16(item.Element("Unknown1")?.Value));
                        writer.Write(Convert.ToUInt16(item.Element("UseTimeGroup")?.Value));
                        writer.Write(Convert.ToUInt16(item.Element("Overlap")?.Value));
                        writer.Write(Convert.ToUInt16(item.Element("TamerMinLevel")?.Value));
                        writer.Write(Convert.ToUInt16(item.Element("TamerMaxLevel")?.Value));
                        writer.Write(Convert.ToUInt16(item.Element("DigimonMinLevel")?.Value));
                        writer.Write(Convert.ToUInt16(item.Element("DigimonMaxLevel")?.Value));
                        writer.Write(Convert.ToUInt16(item.Element("Posses")?.Value));
                        writer.Write(Convert.ToUInt16(item.Element("EquipSeries")?.Value));
                        writer.Write(Convert.ToUInt16(item.Element("Target")?.Value));
                        writer.Write(Convert.ToUInt16(item.Element("Unknown2")?.Value));
                        writer.Write(Convert.ToUInt16(item.Element("Drop")?.Value));
                        writer.Write(Convert.ToUInt16(item.Element("Unknown3")?.Value));
                        writer.Write(Convert.ToUInt16(item.Element("EventItemPrice")?.Value));
                        writer.Write(Convert.ToUInt16(item.Element("Unknown4")?.Value));
                        writer.Write(Convert.ToUInt16(item.Element("DigicorePrice")?.Value));
                        writer.Write(Convert.ToUInt16(item.Element("Digicore")?.Value));
                        writer.Write(Convert.ToInt32(item.Element("ScanPrice")?.Value));
                        writer.Write(Convert.ToInt32(item.Element("Sale")?.Value));

                        stringBytes = Util.GetStringBytes(Util.RemoveCdataFromString(item.Element("ModelNif")?.Value), 64);
                        writer.Write(stringBytes, 0, stringBytes.Length);

                        stringBytes = Util.GetStringBytes(Util.RemoveCdataFromString(item.Element("ModelEffect")?.Value), 64);
                        writer.Write(stringBytes, 0, stringBytes.Length);

                        writer.Write(Convert.ToByte(item.Element("ModelLoop")?.Value));
                        writer.Write(Convert.ToByte(item.Element("ModelShader")?.Value));
                        writer.Write(Convert.ToUInt16(item.Element("SkillCodeType")?.Value));
                        writer.Write(Convert.ToInt32(item.Element("Skill")?.Value));
                        writer.Write(Convert.ToByte(item.Element("ApplyRateMax")?.Value));
                        writer.Write(Convert.ToByte(item.Element("ApplyRateMin")?.Value));
                        writer.Write(Convert.ToByte(item.Element("ApplyElement")?.Value));
                        writer.Write(Convert.ToUInt16(item.Element("SocketCount")?.Value));
                        writer.Write(Convert.ToByte(item.Element("Unknown5")?.Value));
                        writer.Write(Convert.ToUInt16(item.Element("SoundId")?.Value));

                        stringBytes = Util.GetStringBytes(Util.RemoveCdataFromString(item.Element("AsString")?.Value), 40);
                        writer.Write(stringBytes, 0, stringBytes.Length);
                    }

                    xDoc = XDocument.Load(_dataXmlFile_ItemTap);
                    items = xDoc.Descendants().Where(x => x.Name.LocalName == "ItemTap").ToList();
                    writer.Write(items.Count);

                    foreach (var item in items)
                    {
                        writer.Write(Convert.ToUInt16(item.Element("SellClass")?.Value));

                        var stringBytes = Util.GetStringBytes(Util.RemoveCdataFromString(item.Element("Name")?.Value), 64);
                        writer.Write(stringBytes, 0, stringBytes.Length);
                    }

                    xDoc = XDocument.Load(_dataXmlFile_ItemCooldown);
                    items = xDoc.Descendants().Where(x => x.Name.LocalName == "ItemCooldownTime").ToList();
                    writer.Write(items.Count);

                    foreach (var item in items)
                    {
                        writer.Write(Convert.ToInt32(item.Element("GroupId")?.Value));
                        writer.Write(Convert.ToByte(item.Element("Network")?.Value));
                        writer.Write(Convert.ToDouble(item.Element("TimeSec")?.Value));
                        writer.Write(Convert.ToInt16(item.Element("Unknown")?.Value));
                        writer.Write(Convert.ToByte(item.Element("Unknown2")?.Value));
                    }

                    xDoc = XDocument.Load(_dataXmlFile_ItemDisplay);
                    items = xDoc.Descendants().Where(x => x.Name.LocalName == "ItemDisplay").ToList();
                    writer.Write(items.Count);

                    foreach (var item in items)
                    {
                        writer.Write(Convert.ToInt32(item.Element("ItemS")?.Value));
                        writer.Write(Convert.ToInt32(item.Element("DispId")?.Value));
                    }

                    xDoc = XDocument.Load(_dataXmlFile_ItemTypeName);
                    items = xDoc.Descendants().Where(x => x.Name.LocalName == "ItemTypeName").ToList();
                    writer.Write(items.Count);

                    foreach (var item in items)
                    {
                        var stringBytes = Util.GetStringBytes(Util.RemoveCdataFromString(item.Element("Name")?.Value), 132);
                        writer.Write(stringBytes, 0, stringBytes.Length);
                    }

                    xDoc = XDocument.Load(_dataXmlFile_ItemRank);
                    items = xDoc.Descendants().Where(x => x.Name.LocalName == "ItemRank").ToList();
                    writer.Write(items.Count);

                    foreach (var item in items)
                    {
                        writer.Write(Convert.ToInt32(item.Element("Id")?.Value));
                        writer.Write(Convert.ToInt16(item.Element("Class")?.Value));
                        writer.Write(Convert.ToInt16(item.Element("Count")?.Value));
                    }

                    xDoc = XDocument.Load(_dataXmlFile_ItemElementBase);
                    items = xDoc.Descendants().Where(x => x.Name.LocalName == "ElementItemBase").ToList();
                    writer.Write(items.Count);

                    foreach (var item in items)
                    {
                        writer.Write(Convert.ToInt32(item.Attribute("Id")?.Value));
                    }

                    xDoc = XDocument.Load(_dataXmlFile_ItemElement);
                    items = xDoc.Descendants().Where(x => x.Name.LocalName == "ElementItem").ToList();
                    writer.Write(items.Count);

                    foreach (var item in items)
                    {
                        writer.Write(Convert.ToInt32(item.Attribute("Id")?.Value));
                    }

                    xDoc = XDocument.Load(_dataXmlFile_ItemExchange);
                    items = xDoc.Descendants().Where(x => x.Name.LocalName == "ItemExchange").ToList();
                    writer.Write(items.Count);

                    foreach (var item in items)
                    {
                        writer.Write(Convert.ToInt32(item.Attribute("NpcId")?.Value));
                        writer.Write(Convert.ToInt16(item.Attribute("ItemIndex")?.Value));
                        writer.Write(Convert.ToInt16(item.Element("Unknown")?.Value));
                        writer.Write(Convert.ToInt32(item.Element("ItemId")?.Value));
                        writer.Write(Convert.ToInt32(item.Element("CodeA")?.Value));
                        writer.Write(Convert.ToInt32(item.Element("CodeB")?.Value));
                        writer.Write(Convert.ToInt32(item.Element("CodeC")?.Value));
                        writer.Write(Convert.ToInt32(item.Element("CodeD")?.Value));
                        writer.Write(Convert.ToInt16(item.Element("PriceA")?.Value));
                        writer.Write(Convert.ToInt16(item.Element("PriceB")?.Value));
                        writer.Write(Convert.ToInt16(item.Element("PriceC")?.Value));
                        writer.Write(Convert.ToInt16(item.Element("PriceD")?.Value));
                        writer.Write(Convert.ToUInt16(item.Element("Count")?.Value));
                        writer.Write(Convert.ToInt16(item.Element("Unknown2")?.Value));
                        writer.Write(Convert.ToInt32(item.Element("Unknown3")?.Value));
                    }

                    xDoc = XDocument.Load(_dataXmlFile_ItemAcessory);
                    items = xDoc.Descendants().Where(x => x.Name.LocalName == "Acessory").ToList();
                    writer.Write(items.Count);

                    foreach (var item in items)
                    {
                        writer.Write(Convert.ToInt32(item.Element("Index")?.Value));
                        writer.Write(Convert.ToInt32(item.Element("Index2")?.Value));
                        writer.Write(Convert.ToInt16(item.Element("GainOption")?.Value));
                        writer.Write(Convert.ToInt16(item.Element("RenewalNumber")?.Value));

                        var subItems = item.Descendants().Where(x => x.Name.LocalName == "Option").ToList();

                        foreach (var subItem in subItems)
                        {
                            writer.Write(Convert.ToInt16(item.Attribute("Id")?.Value));
                            writer.Write(Convert.ToInt16(item.Element("Unknown")?.Value));
                            writer.Write(Convert.ToUInt32(item.Element("MinValue")?.Value));
                            writer.Write(Convert.ToUInt32(item.Element("MaxValue")?.Value));
                        }
                    }

                    xDoc = XDocument.Load(_dataXmlFile_ItemAcessoryEnchant);
                    items = xDoc.Descendants().Where(x => x.Name.LocalName == "AcessoryEnchant").ToList();
                    writer.Write(items.Count);

                    foreach (var item in items)
                    {
                        writer.Write(Convert.ToInt32(item.Attribute("Id")?.Value));
                        writer.Write(Convert.ToInt32(item.Element("Index")?.Value));
                        writer.Write(Convert.ToInt16(item.Element("Option")?.Value));
                        writer.Write(Convert.ToInt16(item.Element("Factor")?.Value));
                    }

                    xDoc = XDocument.Load(_dataXmlFile_ItemCraft);
                    items = xDoc.Descendants().Where(x => x.Name.LocalName == "ItemCraft").ToList();
                    writer.Write(items.Count);

                    foreach (var item in items)
                    {
                        writer.Write(Convert.ToInt32(item.Attribute("NpcId")?.Value));

                        var subItems = item.Descendants().Where(x => x.Name.LocalName == "CraftList").ToList();

                        writer.Write(subItems.Count);

                        foreach (var subItem in subItems)
                        {
                            writer.Write(Convert.ToInt32(subItem.Attribute("Unknown")?.Value));
                            writer.Write(Convert.ToInt32(subItem.Element("UnknownVar")?.Value));

                            var thirdLevelItems = subItem.Descendants().Where(x => x.Name.LocalName == "ValueVar").ToList();
                            writer.Write(thirdLevelItems.Count);

                            foreach (var thirdLevelItem in thirdLevelItems)
                            {
                                writer.Write(Convert.ToInt16(thirdLevelItem.Value));
                            }

                            thirdLevelItems = subItem.Descendants().Where(x => x.Name.LocalName == "Categorie").ToList();
                            writer.Write(thirdLevelItems.Count);

                            foreach (var thirdLevelItem in thirdLevelItems)
                            {
                                writer.Write(Convert.ToInt32(thirdLevelItem.Attribute("Id")?.Value));
                                writer.Write(Convert.ToInt32(thirdLevelItem.Attribute("Unknown")?.Value));

                                var fourthLevelItems = thirdLevelItem.Descendants().Where(x => x.Name.LocalName == "Value").ToList();
                                writer.Write(fourthLevelItems.Count);

                                foreach (var fourthLevelItem in fourthLevelItems)
                                {
                                    writer.Write(Convert.ToInt16(fourthLevelItem.Value));
                                }

                                fourthLevelItems = thirdLevelItem.Descendants().Where(x => x.Name.LocalName == "Item").ToList();
                                writer.Write(fourthLevelItems.Count);

                                foreach (var fourthLevelItem in fourthLevelItems)
                                {
                                    writer.Write(Convert.ToInt32(fourthLevelItem.Attribute("Index")?.Value));
                                    writer.Write(Convert.ToInt32(fourthLevelItem.Attribute("Id")?.Value));
                                    writer.Write(Convert.ToInt32(fourthLevelItem.Element("Enabled")?.Value));
                                    writer.Write(Convert.ToInt32(fourthLevelItem.Element("SuccessRate")?.Value));
                                    writer.Write(Convert.ToInt32(fourthLevelItem.Element("Unknown")?.Value));
                                    writer.Write(Convert.ToInt32(fourthLevelItem.Element("Unknown2")?.Value));
                                    writer.Write(Convert.ToInt32(fourthLevelItem.Element("Price")?.Value));

                                    var fifthLevelItems = fourthLevelItem.Descendants().Where(x => x.Name.LocalName == "Material").ToList();
                                    writer.Write(fifthLevelItems.Count);

                                    foreach (var fifthLevelItem in fifthLevelItems)
                                    {
                                        writer.Write(Convert.ToInt32(fourthLevelItem.Attribute("Id")?.Value));
                                        writer.Write(Convert.ToInt32(fourthLevelItem.Attribute("Amount")?.Value));
                                    }
                                }
                            }
                        }
                    }

                    xDoc = XDocument.Load(_dataXmlFile_ItemCraftGroup);
                    items = xDoc.Descendants().Where(x => x.Name.LocalName == "ItemCraftGroup").ToList();
                    writer.Write(items.Count);

                    foreach (var item in items)
                    {
                        writer.Write(Convert.ToInt32(item.Attribute("Index")?.Value));
                        writer.Write(Convert.ToInt32(item.Element("Type")?.Value));
                        writer.Write(Convert.ToInt32(item.Element("Number")?.Value));
                        writer.Write(Convert.ToInt32(item.Element("Code")?.Value));
                        writer.Write(Convert.ToInt32(item.Element("Unknown")?.Value));
                    }

                    xDoc = XDocument.Load(_dataXmlFile_ItemXai);
                    items = xDoc.Descendants().Where(x => x.Name.LocalName == "Xai").ToList();
                    writer.Write(items.Count);

                    foreach (var item in items)
                    {
                        writer.Write(Convert.ToInt32(item.Attribute("Id")?.Value));
                        writer.Write(Convert.ToInt32(item.Element("XGauge")?.Value));
                        writer.Write(Convert.ToByte(item.Element("XCrystals")?.Value));
                    }

                    xDoc = XDocument.Load(_dataXmlFile_ItemRankEffect);
                    items = xDoc.Descendants().Where(x => x.Name.LocalName == "RankEffect").ToList();
                    writer.Write(items.Count);

                    foreach (var item in items)
                    {
                        writer.Write(Convert.ToInt32(item.Element("ItemCode")?.Value));
                        var subItems = item.Descendants().Where(x => x.Name.LocalName == "SubRankEffect").ToList();
                        writer.Write(subItems.Count);

                        foreach (var subItem in subItems)
                        {
                            writer.Write(Convert.ToInt32(subItem.Element("Code")?.Value));
                            writer.Write(Convert.ToInt32(subItem.Element("IconNumber")?.Value));
                            writer.Write(Convert.ToInt32(subItem.Element("Rank")?.Value));
                        }
                    }

                    xDoc = XDocument.Load(_dataXmlFile_ItemLook);
                    items = xDoc.Descendants().Where(x => x.Name.LocalName == "LookItem").ToList();
                    writer.Write(items.Count);

                    foreach (var item in items)
                    {
                        writer.Write(Convert.ToInt32(item.Element("ItemCode")?.Value));
                        writer.Write(Convert.ToInt32(item.Element("Di")?.Value));
                        writer.Write(Convert.ToInt32(item.Element("ChangeType")?.Value));

                        var filenameAsciiLength = Convert.ToInt32(item.Element("FileName")?.Attribute("Length")?.Value);
                        var fileName = item.Element("FileName")?.Value;

                        writer.Write(filenameAsciiLength);

                        for (int i = 0; i < filenameAsciiLength; i++)
                        {
                            writer.Write(Convert.ToByte(fileName?[i]));
                        }
                    }
                }
            }

            Thread.Sleep(1000);
            System.Diagnostics.Process.Start("explorer.exe", Directory.GetParent(_outputBinFile).FullName);
        }

        public void BinToXml()
        {
            if (!File.Exists(_dataBinFile))
                return;

            using (Stream s = File.OpenRead(_dataBinFile))
            {
                using (BitReader reader = new BitReader(s))
                {
                    Util.GenerateXml(LoadItemList(reader), _dataXmlFile_BaseItem);
                    Util.GenerateXml(LoadItemTap(reader), _dataXmlFile_ItemTap);
                    Util.GenerateXml(LoadItemCoolTime(reader), _dataXmlFile_ItemCooldown);
                    Util.GenerateXml(LoadItemDisplay(reader), _dataXmlFile_ItemDisplay);
                    Util.GenerateXml(LoadItemTypeName(reader), _dataXmlFile_ItemTypeName);
                    Util.GenerateXml(LoadItemRank(reader), _dataXmlFile_ItemRank);
                    Util.GenerateXml(LoadElementItemBase(reader), _dataXmlFile_ItemElementBase);
                    Util.GenerateXml(LoadElementItem(reader), _dataXmlFile_ItemElement);
                    Util.GenerateXml(LoadItemExchangeList(reader), _dataXmlFile_ItemExchange);
                    Util.GenerateXml(LoadAcessoryList(reader), _dataXmlFile_ItemAcessory);
                    Util.GenerateXml(LoadAcessoryEnchantList(reader), _dataXmlFile_ItemAcessoryEnchant);
                    Util.GenerateXml(LoadItemMaking(reader), _dataXmlFile_ItemCraft);
                    Util.GenerateXml(LoadItemMakingGroupList(reader), _dataXmlFile_ItemCraftGroup);
                    Util.GenerateXml(LoadXaiItemList(reader), _dataXmlFile_ItemXai);
                    Util.GenerateXml(LoadItemRankEffectList(reader), _dataXmlFile_ItemRankEffect);
                    Util.GenerateXml(LoadLookItemList(reader), _dataXmlFile_ItemLook);
                }
            }

            Thread.Sleep(1000);
            System.Diagnostics.Process.Start("explorer.exe", Directory.GetParent(_dataXmlFile_BaseItem).FullName);
        }

        private string LoadItemList(BitReader read)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"<BaseItemList>");
            int count = read.ReadInt();
            for (int i = 0; i < count; i++)
            {
                sb.AppendLine($"<Item Id=\"{read.ReadInt()}\">");
                sb.AppendLine($"<Name>{Util.AddCdataForUnicodeChars(read.ReadZString(Encoding.Unicode, 64 * 2))}</Name>");
                sb.AppendLine($"<Icon>{read.ReadInt()}</Icon>");
                sb.AppendLine($"<Comment>{Util.AddCdataForUnicodeChars(read.ReadZString(Encoding.Unicode, 512 * 2))}</Comment>");
                sb.AppendLine($"<Nif>{Util.AddCdataForUnicodeChars(read.ReadZString(Encoding.UTF8, 64))}</Nif>");
                sb.AppendLine($"<Class>{read.ReadUShort()}</Class>");
                sb.AppendLine($"<TypeComment>{Util.AddCdataForUnicodeChars(read.ReadZString(Encoding.Unicode, 64 * 2))}</TypeComment>");
                sb.AppendLine($"<TypeL>{read.ReadUShort()}</TypeL>");
                sb.AppendLine($"<TypeL1>{read.ReadUShort()}</TypeL1>");
                sb.AppendLine($"<TypeS>{read.ReadUShort()}</TypeS>");
                sb.AppendLine($"<UseType>{read.ReadUShort()}</UseType>");
                sb.AppendLine($"<BoundType>{read.ReadUShort()}</BoundType>");
                sb.AppendLine($"<TypeValue>{read.ReadInt()}</TypeValue>");
                sb.AppendLine($"<SellType>{read.ReadUShort()}</SellType>");
                sb.AppendLine($"<Unknown1>{read.ReadUShort()}</Unknown1>");
                sb.AppendLine($"<UseTimeGroup>{read.ReadUShort()}</UseTimeGroup>");
                sb.AppendLine($"<Overlap>{read.ReadUShort()}</Overlap>");
                sb.AppendLine($"<TamerMinLevel>{read.ReadUShort()}</TamerMinLevel>");
                sb.AppendLine($"<TamerMaxLevel>{read.ReadUShort()}</TamerMaxLevel>");
                sb.AppendLine($"<DigimonMinLevel>{read.ReadUShort()}</DigimonMinLevel>");
                sb.AppendLine($"<DigimonMaxLevel>{read.ReadUShort()}</DigimonMaxLevel>");
                sb.AppendLine($"<Posses>{read.ReadUShort()}</Posses>");
                sb.AppendLine($"<EquipSeries>{read.ReadUShort()}</EquipSeries>");
                sb.AppendLine($"<Target>{read.ReadUShort()}</Target>");
                sb.AppendLine($"<Unknown2>{read.ReadUShort()}</Unknown2>");
                sb.AppendLine($"<Drop>{read.ReadUShort()}</Drop>");
                sb.AppendLine($"<Unknown3>{read.ReadUShort()}</Unknown3>");
                sb.AppendLine($"<EventItemPrice>{read.ReadUShort()}</EventItemPrice>");
                sb.AppendLine($"<Unknown4>{read.ReadUShort()}</Unknown4>");
                sb.AppendLine($"<DigicorePrice>{read.ReadUShort()}</DigicorePrice>");
                sb.AppendLine($"<Digicore>{read.ReadUShort()}</Digicore>");
                sb.AppendLine($"<ScanPrice>{read.ReadInt()}</ScanPrice>");
                sb.AppendLine($"<Sale>{read.ReadInt()}</Sale>");
                sb.AppendLine($"<ModelNif>{Util.AddCdataForUnicodeChars(read.ReadZString(Encoding.UTF8, 64))}</ModelNif>");
                sb.AppendLine($"<ModelEffect>{Util.AddCdataForUnicodeChars(read.ReadZString(Encoding.UTF8, 64))}</ModelEffect>");
                sb.AppendLine($"<ModelLoop>{read.ReadByte()}</ModelLoop>");
                sb.AppendLine($"<ModelShader>{read.ReadByte()}</ModelShader>");
                sb.AppendLine($"<SkillCodeType>{read.ReadUShort()}</SkillCodeType>");
                sb.AppendLine($"<Skill>{read.ReadInt()}</Skill>");
                sb.AppendLine($"<ApplyRateMax>{read.ReadByte()}</ApplyRateMax>");
                sb.AppendLine($"<ApplyRateMin>{read.ReadByte()}</ApplyRateMin>");
                sb.AppendLine($"<ApplyElement>{read.ReadByte()}</ApplyElement>");
                sb.AppendLine($"<SocketCount>{read.ReadUShort()}</SocketCount>");
                sb.AppendLine($"<Unknown5>{read.ReadByte()}</Unknown5>");
                sb.AppendLine($"<SoundId>{read.ReadUShort()}</SoundId>");
                sb.AppendLine($"<AsString>{Util.AddCdataForUnicodeChars(read.ReadZString(Encoding.Unicode, 40))}</AsString>");
                sb.AppendLine("</Item>");
            }
            sb.AppendLine($"</BaseItemList>");

            return sb.ToString();
        }

        private string LoadItemTap(BitReader read)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"<ItemTapList>");
            int count = read.ReadInt();
            for (int y = 0; y < count; y++)
            {
                sb.AppendLine($"<ItemTap>");
                sb.AppendLine($"<SellClass>{read.ReadUShort()}</SellClass>");
                sb.AppendLine($"<Name>{Util.AddCdataForUnicodeChars(read.ReadZString(Encoding.Unicode, 64))}</Name>");
                sb.AppendLine($"</ItemTap>");
            }
            sb.AppendLine($"</ItemTapList>");

            return sb.ToString();
        }

        private string LoadItemCoolTime(BitReader read)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"<ItemCooldownTimeList>");
            int count = read.ReadInt();
            for (int c = 0; c < count; c++)
            {
                sb.AppendLine($"<ItemCooldownTime>");
                sb.AppendLine($"<GroupId>{read.ReadInt()}</GroupId>");
                sb.AppendLine($"<Network>{read.ReadByte()}</Network>");
                sb.AppendLine($"<TimeSec>{read.ReadDouble()}</TimeSec>");
                sb.AppendLine($"<Unknown>{read.ReadShort()}</Unknown>");
                sb.AppendLine($"<Unknown2>{read.ReadByte()}</Unknown2>");
                sb.AppendLine($"</ItemCooldownTime>");
            }
            sb.AppendLine($"</ItemCooldownTimeList>");

            return sb.ToString();
        }

        private string LoadItemDisplay(BitReader read)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"<ItemDisplayList>");
            int count = read.ReadInt();
            for (int r = 0; r < count; r++)
            {
                sb.AppendLine($"<ItemDisplay>");
                sb.AppendLine($"<ItemS>{read.ReadInt()}</ItemS>");
                sb.AppendLine($"<DispId>{read.ReadInt()}</DispId>");
                sb.AppendLine($"</ItemDisplay>");
            }
            sb.AppendLine($"</ItemDisplayList>");

            return sb.ToString();
        }

        private string LoadItemTypeName(BitReader read)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"<ItemTypeNameList>");
            int count = read.ReadInt();
            for (int r = 0; r < count; r++)
            {
                sb.AppendLine($"<ItemTypeName>");
                sb.AppendLine($"<Name>{Util.AddCdataForUnicodeChars(read.ReadZString(Encoding.Unicode, 132))}</Name>");
                sb.AppendLine($"</ItemTypeName>");
            }
            sb.AppendLine($"</ItemTypeNameList>");

            return sb.ToString();
        }

        private string LoadItemRank(BitReader read)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"<ItemRankList>");
            int count = read.ReadInt();
            for (int g = 0; g < count; g++)
            {
                sb.AppendLine($"<ItemRank>");
                sb.AppendLine($"<Id>{read.ReadInt()}</Id>");
                sb.AppendLine($"<Class>{read.ReadShort()}</Class>");
                sb.AppendLine($"<Count>{read.ReadShort()}</Count>");
                sb.AppendLine($"</ItemRank>");
            }
            sb.AppendLine($"</ItemRankList>");

            return sb.ToString();
        }

        private string LoadElementItemBase(BitReader read)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"<ItemElementBaseList>");
            int count = read.ReadInt();
            for (int g = 0; g < count; g++)
            {
                sb.AppendLine($"<ElementItemBase Id=\"{read.ReadInt()}\" />");
            }
            sb.AppendLine($"</ItemElementBaseList>");

            return sb.ToString();
        }

        private string LoadElementItem(BitReader read)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"<ItemElementList>");
            int count = read.ReadInt();
            for (int g = 0; g < count; g++)
            {
                sb.AppendLine($"<ElementItem Id=\"{read.ReadInt()}\" />");
            }
            sb.AppendLine($"</ItemElementList>");

            return sb.ToString();
        }

        private string LoadItemExchangeList(BitReader read)
        {
            var sb = new StringBuilder();
            int count = read.ReadInt();
            sb.AppendLine($"<ItemExchangeList Skip=\"{read.ReadInt()}\">");
            for (int h = 0; h < count; h++)
            {
                sb.AppendLine($"<ItemExchange NpcId=\"{read.ReadInt()}\" ItemIndex=\"{read.ReadShort()}\">");
                sb.AppendLine($"<Unknown>{read.ReadShort()}</Unknown>");
                sb.AppendLine($"<ItemId>{read.ReadInt()}</ItemId>");
                sb.AppendLine($"<CodeA>{read.ReadInt()}</CodeA>");
                sb.AppendLine($"<CodeB>{read.ReadInt()}</CodeB>");
                sb.AppendLine($"<CodeC>{read.ReadInt()}</CodeC>");
                sb.AppendLine($"<CodeD>{read.ReadInt()}</CodeD>");
                sb.AppendLine($"<PriceA>{read.ReadShort()}</PriceA>");
                sb.AppendLine($"<PriceB>{read.ReadShort()}</PriceB>");
                sb.AppendLine($"<PriceC>{read.ReadShort()}</PriceC>");
                sb.AppendLine($"<PriceD>{read.ReadShort()}</PriceD>");
                sb.AppendLine($"<Count>{read.ReadUShort()}</Count>");
                sb.AppendLine($"<Unknown2>{read.ReadShort()}</Unknown2>");
                if (h < count - 1) read.ReadInt();
                sb.AppendLine($"<Unknown3>{h + 1}</Unknown3>");
                sb.AppendLine($"</ItemExchange>");
            }
            sb.AppendLine($"</ItemExchangeList>");

            return sb.ToString();
        }

        private string LoadAcessoryList(BitReader read)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"<AcessoryList>");
            var count = read.ReadInt();
            for (int t = 0; t < count; t++)
            {
                sb.AppendLine($"<Acessory>");
                sb.AppendLine($"<Index>{read.ReadInt()}</Index>");
                sb.AppendLine($"<Index2>{read.ReadInt()}</Index2>");
                sb.AppendLine($"<GainOption>{read.ReadShort()}</GainOption>");
                sb.AppendLine($"<RenewalNumber>{read.ReadShort()}</RenewalNumber>");

                for (int i = 0; i < 16; i++)
                {
                    sb.AppendLine($"<Option Id=\"{read.ReadShort()}\">");
                    sb.AppendLine($"<Unknown>{read.ReadShort()}</Unknown>");
                    sb.AppendLine($"<MinValue>{read.ReadUInt()}</MinValue>");
                    sb.AppendLine($"<MaxValue>{read.ReadUInt()}</MaxValue>");
                    sb.AppendLine($"</Option>");
                }
                sb.AppendLine($"</Acessory>");
            }
            sb.AppendLine($"</AcessoryList>");

            return sb.ToString();
        }

        private string LoadAcessoryEnchantList(BitReader read)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"<AcessoryEnchantList>");
            int kcount = read.ReadInt();
            for (int t = 0; t < kcount; t++)
            {
                sb.AppendLine($"<AcessoryEnchant Id=\"{read.ReadInt()}\">");
                sb.AppendLine($"<Index>{read.ReadInt()}</Index>");
                sb.AppendLine($"<Option>{read.ReadShort()}</Option>");
                sb.AppendLine($"<Factor>{read.ReadShort()}</Factor>");
                sb.AppendLine($"</AcessoryEnchant>");
            }
            sb.AppendLine($"</AcessoryEnchantList>");

            return sb.ToString();
        }

        private string LoadItemMaking(BitReader read)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"<ItemCraftList>");
            int count3 = read.ReadInt();
            for (int i = 0; i < count3; i++)
            {
                sb.AppendLine($"<ItemCraft NpcId=\"{read.ReadInt()}\">");
                int List = read.ReadInt();
                for (int g = 0; g < List; g++)
                {
                    sb.AppendLine($"<CraftList Unknown=\"{read.ReadInt()}\">");

                    sb.AppendLine($"<UnknownVar>{read.ReadInt()}</UnknownVar>");
                    int ID3 = read.ReadInt();
                    for (int c = 0; c < ID3; c++)
                    {
                        sb.AppendLine($"<ValueVar>{read.ReadShort()}</ValueVar>");
                    }

                    int Categories = read.ReadInt();
                    for (int a = 0; a < Categories; a++)
                    {
                        sb.AppendLine($"<Categorie Id=\"{read.ReadInt()}\" Unknown=\"{read.ReadInt()}\">");
                        int u2 = read.ReadInt();
                        for (int d = 0; d < u2; d++)
                        {
                            sb.AppendLine($"<Value>{read.ReadShort()}</Value>");
                        }

                        int Items = read.ReadInt();
                        for (int r = 0; r < Items; r++)
                        {
                            sb.AppendLine($"<Item Index=\"{read.ReadInt()}\" Id=\"{read.ReadInt()}\">");
                            sb.AppendLine($"<Enabled>{read.ReadInt()}</Enabled>");
                            sb.AppendLine($"<SuccessRate>{read.ReadInt()}</SuccessRate>");
                            sb.AppendLine($"<Unknown>{read.ReadInt()}</Unknown>");
                            sb.AppendLine($"<Unknown2>{read.ReadInt()}</Unknown2>");
                            sb.AppendLine($"<Price>{read.ReadInt()}</Price>");
                            sb.AppendLine($"<Materials>");
                            var materialCount = read.ReadInt();
                            for (int y = 0; y < materialCount; y++)
                                sb.AppendLine($"<Material Id=\"{read.ReadInt()}\" Amount=\"{read.ReadInt()}\" />");

                            sb.AppendLine($"</Materials>");
                            sb.AppendLine($"</Item>");
                        }
                        sb.AppendLine($"</Categorie>");
                    }

                    sb.AppendLine($"</CraftList>");
                }
                sb.AppendLine($"</ItemCraft>");
            }
            sb.AppendLine($"</ItemCraftList>");

            return sb.ToString();
        }

        private string LoadItemMakingGroupList(BitReader read)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"<ItemCraftGroupList>");
            int count = read.ReadInt();
            for (int d = 0; d < count; d++)
            {
                sb.AppendLine($"<ItemCraftGroup Index=\"{read.ReadInt()}\">");
                sb.AppendLine($"<Type>{read.ReadInt()}</Type>");
                sb.AppendLine($"<Number>{read.ReadInt()}</Number>");
                sb.AppendLine($"<Code>{read.ReadInt()}</Code>");
                sb.AppendLine($"<Unknown>{read.ReadInt()}</Unknown>");
                sb.AppendLine($"</ItemCraftGroup>");
            }
            sb.AppendLine($"</ItemCraftGroupList>");

            return sb.ToString();
        }

        private string LoadXaiItemList(BitReader read)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"<XaiList>");
            int XAICount = read.ReadInt();
            for (int i = 0; i < XAICount; i++)
            {
                sb.AppendLine($"<Xai Id=\"{read.ReadInt()}\">");
                sb.AppendLine($"<XGauge>{read.ReadInt()}</XGauge>");
                sb.AppendLine($"<XCrystals>{read.ReadByte()}</XCrystals>");
                sb.AppendLine($"</Xai>");
            }
            sb.AppendLine($"</XaiList>");

            return sb.ToString();
        }

        private string LoadItemRankEffectList(BitReader read)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"<RankEffectList>");
            int count = read.ReadInt();
            for (int q = 0; q < count; q++)
            {
                sb.AppendLine($"<RankEffect>");
                sb.AppendLine($"<ItemCode>{read.ReadInt()}</ItemCode>");

                var subCount = read.ReadInt();
                for (int a = 0; a < subCount; a++)
                {
                    sb.AppendLine($"<SubRankEffect>");
                    sb.AppendLine($"<Code>{read.ReadInt()}</Code>");
                    sb.AppendLine($"<IconNumber>{read.ReadInt()}</IconNumber>");
                    sb.AppendLine($"<Rank>{read.ReadInt()}</Rank>");
                    sb.AppendLine($"</SubRankEffect>");
                }

                sb.AppendLine($"</RankEffect>");
            }
            sb.AppendLine($"</RankEffectList>");

            return sb.ToString();
        }

        private string LoadLookItemList(BitReader read)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"<LookItemList>");
            int count = read.ReadInt();
            for (int u = 0; u < count; u++)
            {
                sb.AppendLine($"<LookItem>");
                sb.AppendLine($"<ItemCode>{read.ReadInt()}</ItemCode>");
                sb.AppendLine($"<Di>{read.ReadInt()}</Di>");
                sb.AppendLine($"<ChangeType>{read.ReadInt()}</ChangeType>");

                int nameSize = read.ReadInt();
                byte[] mname_get = new byte[nameSize];

                for (int g = 0; g < nameSize; g++)
                    mname_get[g] = read.ReadByte();

                sb.AppendLine($"<FileName Length=\"{nameSize}\">{Util.AddCdataForUnicodeChars(Encoding.ASCII.GetString(mname_get).Trim())}</FileName>");
                sb.AppendLine($"</LookItem>");
            }
            sb.AppendLine($"</LookItemList>");

            return sb.ToString();
        }
    }
}