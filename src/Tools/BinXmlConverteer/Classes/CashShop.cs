
using System;
using System.CodeDom.Compiler;
using System.Diagnostics.Metrics;
using System.Dynamic;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using static BinXmlConverter.Classes.CashShop;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace BinXmlConverter.Classes
{
    public class CashShop
    {
        public static void CashShopToXml(string fileName)
        {
            using (Stream s = File.OpenRead(fileName))
            {
                using (var reader = new BitReader(s))
                {
                    int CashShopCount = reader.ReadInt();

                    for (int i = 0; i < CashShopCount; i++)
                    {
                        CashShopMainInformation cashShopMain = LoadCashMainInfo(reader);
                        ExportCashShopMainInfoToXml(cashShopMain, i);

                        CashShopTamerInfo CashTamerInfo = LoadCashShopTamerItems(reader);
                        ExportCashTamerInfoToXml(CashTamerInfo, i);

                        CashShopDigimonInfo CashDigimonInfo = LoadCashShopDigimonItems(reader);
                        ExportCashDigimonInfoToXml(CashDigimonInfo, i);

                        CashShopAvatarInfo CashAvatarInfo = LoadCashShopAvatarItems(reader);
                        ExportCashAvatarInfoToXml(CashAvatarInfo, i);

                        CashShopPackageInfo CashPackageInfo = LoadCashShopPackageItems(reader);
                        ExportCashPackageInfoToXml(CashPackageInfo, i);
                    }

                    CashWebData[] CashWeb = LoadCashWebData(reader);
                    ExportCashWebDataToXml(CashWeb);

                }
            }

        }


        private static CashShopTamerInfo LoadCashShopTamerItems(BitReader read)
        {


            CashShopTamerInfo cashShopTamerInfo1 = new CashShopTamerInfo();

            int CategoryNameSize = read.ReadInt();
            var CategoryNameBytes = read.ReadBytes(CategoryNameSize * 2);
            cashShopTamerInfo1.CategoryName = CleanString(System.Text.Encoding.Unicode.GetString(CategoryNameBytes));

            cashShopTamerInfo1.unknow = read.ReadInt();
            cashShopTamerInfo1.unknow1 = read.ReadInt();
            int AllSize = read.ReadInt();
            var AllSizeBytes = read.ReadBytes(AllSize * 2);
            cashShopTamerInfo1.AllName = CleanString(System.Text.Encoding.Unicode.GetString(AllSizeBytes));

            cashShopTamerInfo1.unknow2 = read.ReadInt();
            cashShopTamerInfo1.unknow3 = read.ReadInt();
            int ExpasionSize = read.ReadInt();
            var ExpasionSizeBytes = read.ReadBytes(ExpasionSize * 2);
            cashShopTamerInfo1.ExpasionName = CleanString(System.Text.Encoding.Unicode.GetString(ExpasionSizeBytes));

            #region [ExpansionItems]
            int ExpansionCount = read.ReadInt();

            for (int i = 0; i < ExpansionCount; i++)
            {
                CashShopInformationCount CashInfo = new CashShopInformationCount();

                CashInfo.CashShopId = read.ReadInt();
                int count = read.ReadInt();
                for (int j = 0; j < count; j++)
                {
                    CASHINFO s = new CASHINFO();
                    int NameSize = read.ReadInt();
                    var NameBytes = read.ReadBytes(NameSize * 2);
                    s.Name = CleanString(System.Text.Encoding.Unicode.GetString(NameBytes));
                    int DesSize = read.ReadInt();
                    var DesBytes = read.ReadBytes(DesSize * 2);
                    s.Description = CleanString(System.Text.Encoding.Unicode.GetString(DesBytes));

                    s.cashshop_id = CashInfo.CashShopId;
                    s.Enabled = read.ReadByte();
                    s.unique_id = read.ReadInt();
                    s.Date1 = read.ReadZString(Encoding.ASCII, 64);
                    s.Date2 = read.ReadZString(Encoding.ASCII, 64);
                    s.nPurchaseCashType = read.ReadInt();
                    s.nStandardSellingPrice = read.ReadInt();
                    s.nRealSellingPrice = read.ReadInt();
                    s.nSalePersent = read.ReadInt();
                    s.nIconID = read.ReadInt();
                    s.nMaskType = read.ReadInt();
                    s.nDispType = read.ReadInt();
                    s.nDispCount = read.ReadInt();
                    int ItemCount = read.ReadInt();

                    for (int k = 0; k < ItemCount; k++)
                    {
                        var CashItem = new Item();
                        CashItem.ItemId = read.ReadInt();
                        CashItem.Amount = read.ReadInt();

                        s.CashItems.Add(CashItem);
                    }

                    CashInfo.CashInfo.Add(s);
                }
                cashShopTamerInfo1.ExpansionItems.Add(CashInfo);
            }
            #endregion

            cashShopTamerInfo1.unknow4 = read.ReadInt();
            int ExpSize = read.ReadInt();
            var ExpSizeBytes = read.ReadBytes(ExpSize * 2);
            cashShopTamerInfo1.ExpName = CleanString(System.Text.Encoding.Unicode.GetString(ExpSizeBytes));


            #region [ExpItems]
            int ExpCount = read.ReadInt();

            for (int i = 0; i < ExpCount; i++)
            {
                CashShopInformationCount CashInfo = new CashShopInformationCount();

                CashInfo.CashShopId = read.ReadInt();
                int count = read.ReadInt();
                for (int j = 0; j < count; j++)
                {
                    CASHINFO s = new CASHINFO();
                    int NameSize = read.ReadInt();
                    var NameBytes = read.ReadBytes(NameSize * 2);
                    s.Name = CleanString(System.Text.Encoding.Unicode.GetString(NameBytes));
                    int DesSize = read.ReadInt();
                    var DesBytes = read.ReadBytes(DesSize * 2);
                    s.Description = CleanString(System.Text.Encoding.Unicode.GetString(DesBytes));

                    s.cashshop_id = CashInfo.CashShopId;
                    s.Enabled = read.ReadByte();
                    s.unique_id = read.ReadInt();
                    s.Date1 = read.ReadZString(Encoding.ASCII, 64);
                    s.Date2 = read.ReadZString(Encoding.ASCII, 64);
                    s.nPurchaseCashType = read.ReadInt();
                    s.nStandardSellingPrice = read.ReadInt();
                    s.nRealSellingPrice = read.ReadInt();
                    s.nSalePersent = read.ReadInt();
                    s.nIconID = read.ReadInt();
                    s.nMaskType = read.ReadInt();
                    s.nDispType = read.ReadInt();
                    s.nDispCount = read.ReadInt();
                    int ItemCount = read.ReadInt();

                    for (int k = 0; k < ItemCount; k++)
                    {
                        var CashItem = new Item();
                        CashItem.ItemId = read.ReadInt();
                        CashItem.Amount = read.ReadInt();

                        s.CashItems.Add(CashItem);
                    }

                    CashInfo.CashInfo.Add(s);
                }
                cashShopTamerInfo1.ExpItems.Add(CashInfo);
            }
            #endregion

            cashShopTamerInfo1.unknow5 = read.ReadInt();
            int MovimentSize = read.ReadInt();
            var MovimentSizeBytes = read.ReadBytes(MovimentSize * 2);
            cashShopTamerInfo1.MovimentName = CleanString(System.Text.Encoding.Unicode.GetString(MovimentSizeBytes));


            #region [MovimentItems]
            int MovimentCount = read.ReadInt();

            for (int i = 0; i < MovimentCount; i++)
            {
                CashShopInformationCount CashInfo = new CashShopInformationCount();

                CashInfo.CashShopId = read.ReadInt();
                int count = read.ReadInt();
                for (int j = 0; j < count; j++)
                {
                    CASHINFO s = new CASHINFO();
                    int NameSize = read.ReadInt();
                    var NameBytes = read.ReadBytes(NameSize * 2);
                    s.Name = CleanString(System.Text.Encoding.Unicode.GetString(NameBytes));
                    int DesSize = read.ReadInt();
                    var DesBytes = read.ReadBytes(DesSize * 2);
                    s.Description = CleanString(System.Text.Encoding.Unicode.GetString(DesBytes));

                    s.cashshop_id = CashInfo.CashShopId;
                    s.Enabled = read.ReadByte();
                    s.unique_id = read.ReadInt();
                    s.Date1 = read.ReadZString(Encoding.ASCII, 64);
                    s.Date2 = read.ReadZString(Encoding.ASCII, 64);
                    s.nPurchaseCashType = read.ReadInt();
                    s.nStandardSellingPrice = read.ReadInt();
                    s.nRealSellingPrice = read.ReadInt();
                    s.nSalePersent = read.ReadInt();
                    s.nIconID = read.ReadInt();
                    s.nMaskType = read.ReadInt();
                    s.nDispType = read.ReadInt();
                    s.nDispCount = read.ReadInt();
                    int ItemCount = read.ReadInt();

                    for (int k = 0; k < ItemCount; k++)
                    {
                        var CashItem = new Item();
                        CashItem.ItemId = read.ReadInt();
                        CashItem.Amount = read.ReadInt();

                        s.CashItems.Add(CashItem);
                    }

                    CashInfo.CashInfo.Add(s);
                }
                cashShopTamerInfo1.MovimentItems.Add(CashInfo);
            }
            #endregion

            cashShopTamerInfo1.unknow6 = read.ReadInt();
            int ChatShopSize = read.ReadInt();
            var ChatShopSizeBytes = read.ReadBytes(ChatShopSize * 2);
            cashShopTamerInfo1.ChatName = CleanString(System.Text.Encoding.Unicode.GetString(ChatShopSizeBytes));

            #region [ChatShopSizeItems]
            int ChatShopSizeCount = read.ReadInt();

            for (int i = 0; i < ChatShopSizeCount; i++)
            {
                CashShopInformationCount CashInfo = new CashShopInformationCount();

                CashInfo.CashShopId = read.ReadInt();
                int count = read.ReadInt();
                for (int j = 0; j < count; j++)
                {
                    CASHINFO s = new CASHINFO();
                    int NameSize = read.ReadInt();
                    var NameBytes = read.ReadBytes(NameSize * 2);
                    s.Name = CleanString(System.Text.Encoding.Unicode.GetString(NameBytes));
                    int DesSize = read.ReadInt();
                    var DesBytes = read.ReadBytes(DesSize * 2);
                    s.Description = CleanString(System.Text.Encoding.Unicode.GetString(DesBytes));

                    s.cashshop_id = CashInfo.CashShopId;
                    s.Enabled = read.ReadByte();
                    s.unique_id = read.ReadInt();
                    s.Date1 = read.ReadZString(Encoding.ASCII, 64);
                    s.Date2 = read.ReadZString(Encoding.ASCII, 64);
                    s.nPurchaseCashType = read.ReadInt();
                    s.nStandardSellingPrice = read.ReadInt();
                    s.nRealSellingPrice = read.ReadInt();
                    s.nSalePersent = read.ReadInt();
                    s.nIconID = read.ReadInt();
                    s.nMaskType = read.ReadInt();
                    s.nDispType = read.ReadInt();
                    s.nDispCount = read.ReadInt();
                    int ItemCount = read.ReadInt();

                    for (int k = 0; k < ItemCount; k++)
                    {
                        var CashItem = new Item();
                        CashItem.ItemId = read.ReadInt();
                        CashItem.Amount = read.ReadInt();

                        s.CashItems.Add(CashItem);
                    }

                    CashInfo.CashInfo.Add(s);
                }
                cashShopTamerInfo1.ChatItems.Add(CashInfo);
            }
            #endregion

            cashShopTamerInfo1.unknow7 = read.ReadInt();
            int EtcSize = read.ReadInt();
            var EtcSizeBytes = read.ReadBytes(EtcSize * 2);
            cashShopTamerInfo1.EtcName = CleanString(System.Text.Encoding.Unicode.GetString(EtcSizeBytes));

            #region [EtcItems]
            int EtcCount = read.ReadInt();

            for (int i = 0; i < EtcCount; i++)
            {
                CashShopInformationCount CashInfo = new CashShopInformationCount();

                CashInfo.CashShopId = read.ReadInt();
                int count = read.ReadInt();
                for (int j = 0; j < count; j++)
                {
                    CASHINFO s = new CASHINFO();
                    int NameSize = read.ReadInt();
                    var NameBytes = read.ReadBytes(NameSize * 2);
                    s.Name = CleanString(System.Text.Encoding.Unicode.GetString(NameBytes));
                    int DesSize = read.ReadInt();
                    var DesBytes = read.ReadBytes(DesSize * 2);
                    s.Description = CleanString(System.Text.Encoding.Unicode.GetString(DesBytes));

                    s.cashshop_id = CashInfo.CashShopId;
                    s.Enabled = read.ReadByte();
                    s.unique_id = read.ReadInt();
                    s.Date1 = read.ReadZString(Encoding.ASCII, 64);
                    s.Date2 = read.ReadZString(Encoding.ASCII, 64);
                    s.nPurchaseCashType = read.ReadInt();
                    s.nStandardSellingPrice = read.ReadInt();
                    s.nRealSellingPrice = read.ReadInt();
                    s.nSalePersent = read.ReadInt();
                    s.nIconID = read.ReadInt();
                    s.nMaskType = read.ReadInt();
                    s.nDispType = read.ReadInt();
                    s.nDispCount = read.ReadInt();
                    int ItemCount = read.ReadInt();

                    for (int k = 0; k < ItemCount; k++)
                    {
                        var CashItem = new Item();
                        CashItem.ItemId = read.ReadInt();
                        CashItem.Amount = read.ReadInt();

                        s.CashItems.Add(CashItem);
                    }

                    CashInfo.CashInfo.Add(s);
                }
                cashShopTamerInfo1.EtcItems.Add(CashInfo);
            }
            #endregion

            return cashShopTamerInfo1;
        }

        private static CashShopDigimonInfo LoadCashShopDigimonItems(BitReader read)
        {

            CashShopDigimonInfo cashShopDigimonInfo1 = new CashShopDigimonInfo();

            cashShopDigimonInfo1.unknow = read.ReadInt();
            int CategoryNameSize = read.ReadInt();
            var CategoryNameSizeBytes = read.ReadBytes(CategoryNameSize * 2);
            cashShopDigimonInfo1.CategoryName = CleanString(System.Text.Encoding.Unicode.GetString(CategoryNameSizeBytes));

            cashShopDigimonInfo1.unknow1 = read.ReadInt();
            cashShopDigimonInfo1.unknow2 = read.ReadInt();

            int AllSize = read.ReadInt();
            var AllSizeBytes = read.ReadBytes(AllSize * 2);
            cashShopDigimonInfo1.AllName = CleanString(System.Text.Encoding.Unicode.GetString(AllSizeBytes));

            cashShopDigimonInfo1.unknow3 = read.ReadInt();
            cashShopDigimonInfo1.unknow4 = read.ReadInt();

            int DigiEggSize = read.ReadInt();
            var DigiEggBytes = read.ReadBytes(DigiEggSize * 2);
            cashShopDigimonInfo1.DigiEggName = CleanString(System.Text.Encoding.Unicode.GetString(DigiEggBytes));

            #region [DigiEggItems]
            int DigiEggCount = read.ReadInt();

            for (int i = 0; i < DigiEggCount; i++)
            {

                CashShopInformationCount CashInfo = new CashShopInformationCount();

                CashInfo.CashShopId = read.ReadInt();
                int count = read.ReadInt();
                for (int j = 0; j < count; j++)
                {
                    CASHINFO s = new CASHINFO();
                    int NameSize = read.ReadInt();
                    var NameBytes = read.ReadBytes(NameSize * 2);
                    s.Name = CleanString(System.Text.Encoding.Unicode.GetString(NameBytes));
                    int DesSize = read.ReadInt();
                    var DesBytes = read.ReadBytes(DesSize * 2);
                    s.Description = CleanString(System.Text.Encoding.Unicode.GetString(DesBytes));

                    s.cashshop_id = CashInfo.CashShopId;
                    s.Enabled = read.ReadByte();
                    s.unique_id = read.ReadInt();
                    s.Date1 = read.ReadZString(Encoding.ASCII, 64);
                    s.Date2 = read.ReadZString(Encoding.ASCII, 64);
                    s.nPurchaseCashType = read.ReadInt();
                    s.nStandardSellingPrice = read.ReadInt();
                    s.nRealSellingPrice = read.ReadInt();
                    s.nSalePersent = read.ReadInt();
                    s.nIconID = read.ReadInt();
                    s.nMaskType = read.ReadInt();
                    s.nDispType = read.ReadInt();
                    s.nDispCount = read.ReadInt();
                    int ItemCount = read.ReadInt();

                    for (int k = 0; k < ItemCount; k++)
                    {
                        var CashItem = new Item();
                        CashItem.ItemId = read.ReadInt();
                        CashItem.Amount = read.ReadInt();

                        s.CashItems.Add(CashItem);
                    }

                    CashInfo.CashInfo.Add(s);
                }
                cashShopDigimonInfo1.DigiEgg.Add(CashInfo);

            }
            #endregion

            cashShopDigimonInfo1.unknow5 = read.ReadInt();

            int EvolutionSize = read.ReadInt();
            var EvolutionBytes = read.ReadBytes(EvolutionSize * 2);
            cashShopDigimonInfo1.EvolutionName = CleanString(System.Text.Encoding.Unicode.GetString(EvolutionBytes));

            #region [EvolutionItems]
            int EvolutionCount = read.ReadInt();

            for (int i = 0; i < EvolutionCount; i++)
            {
                CashShopInformationCount CashInfo = new CashShopInformationCount();

                CashInfo.CashShopId = read.ReadInt();
                int count = read.ReadInt();
                for (int j = 0; j < count; j++)
                {
                    CASHINFO s = new CASHINFO();
                    int NameSize = read.ReadInt();
                    var NameBytes = read.ReadBytes(NameSize * 2);
                    s.Name = CleanString(System.Text.Encoding.Unicode.GetString(NameBytes));
                    int DesSize = read.ReadInt();
                    var DesBytes = read.ReadBytes(DesSize * 2);
                    s.Description = CleanString(System.Text.Encoding.Unicode.GetString(DesBytes));

                    s.cashshop_id = CashInfo.CashShopId;
                    s.Enabled = read.ReadByte();
                    s.unique_id = read.ReadInt();
                    s.Date1 = read.ReadZString(Encoding.ASCII, 64);
                    s.Date2 = read.ReadZString(Encoding.ASCII, 64);
                    s.nPurchaseCashType = read.ReadInt();
                    s.nStandardSellingPrice = read.ReadInt();
                    s.nRealSellingPrice = read.ReadInt();
                    s.nSalePersent = read.ReadInt();
                    s.nIconID = read.ReadInt();
                    s.nMaskType = read.ReadInt();
                    s.nDispType = read.ReadInt();
                    s.nDispCount = read.ReadInt();
                    int ItemCount = read.ReadInt();

                    for (int k = 0; k < ItemCount; k++)
                    {
                        var CashItem = new Item();
                        CashItem.ItemId = read.ReadInt();
                        CashItem.Amount = read.ReadInt();

                        s.CashItems.Add(CashItem);
                    }

                    CashInfo.CashInfo.Add(s);
                }
                cashShopDigimonInfo1.EvolutionItems.Add(CashInfo);

            }
            #endregion

            cashShopDigimonInfo1.unknow6 = read.ReadInt();
            int HatchSize = read.ReadInt();
            var HatchBytes = read.ReadBytes(HatchSize * 2);
            cashShopDigimonInfo1.HatchName = CleanString(System.Text.Encoding.Unicode.GetString(HatchBytes));

            #region [Hatch/Trans Items]
            int HatchCount = read.ReadInt();

            for (int i = 0; i < HatchCount; i++)
            {
                CashShopInformationCount CashInfo = new CashShopInformationCount();

                CashInfo.CashShopId = read.ReadInt();
                int count = read.ReadInt();
                for (int j = 0; j < count; j++)
                {
                    CASHINFO s = new CASHINFO();
                    int NameSize = read.ReadInt();
                    var NameBytes = read.ReadBytes(NameSize * 2);
                    s.Name = CleanString(System.Text.Encoding.Unicode.GetString(NameBytes));
                    int DesSize = read.ReadInt();
                    var DesBytes = read.ReadBytes(DesSize * 2);
                    s.Description = CleanString(System.Text.Encoding.Unicode.GetString(DesBytes));

                    s.cashshop_id = CashInfo.CashShopId;
                    s.Enabled = read.ReadByte();
                    s.unique_id = read.ReadInt();
                    s.Date1 = read.ReadZString(Encoding.ASCII, 64);
                    s.Date2 = read.ReadZString(Encoding.ASCII, 64);
                    s.nPurchaseCashType = read.ReadInt();
                    s.nStandardSellingPrice = read.ReadInt();
                    s.nRealSellingPrice = read.ReadInt();
                    s.nSalePersent = read.ReadInt();
                    s.nIconID = read.ReadInt();
                    s.nMaskType = read.ReadInt();
                    s.nDispType = read.ReadInt();
                    s.nDispCount = read.ReadInt();
                    int ItemCount = read.ReadInt();

                    for (int k = 0; k < ItemCount; k++)
                    {
                        var CashItem = new Item();
                        CashItem.ItemId = read.ReadInt();
                        CashItem.Amount = read.ReadInt();

                        s.CashItems.Add(CashItem);
                    }

                    CashInfo.CashInfo.Add(s);
                }
                cashShopDigimonInfo1.HatchItems.Add(CashInfo);

            }
            #endregion

            cashShopDigimonInfo1.unknow7 = read.ReadInt();
            int ReinforcedSize = read.ReadInt();
            var ReinforcedBytes = read.ReadBytes(ReinforcedSize * 2);
            cashShopDigimonInfo1.ReinforcedName = CleanString(System.Text.Encoding.Unicode.GetString(ReinforcedBytes));

            #region [ReinforceItems]
            int ReinforceCount = read.ReadInt();

            for (int i = 0; i < ReinforceCount; i++)
            {
                CashShopInformationCount CashInfo = new CashShopInformationCount();

                CashInfo.CashShopId = read.ReadInt();
                int count = read.ReadInt();
                for (int j = 0; j < count; j++)
                {
                    CASHINFO s = new CASHINFO();
                    int NameSize = read.ReadInt();
                    var NameBytes = read.ReadBytes(NameSize * 2);
                    s.Name = CleanString(System.Text.Encoding.Unicode.GetString(NameBytes));
                    int DesSize = read.ReadInt();
                    var DesBytes = read.ReadBytes(DesSize * 2);
                    s.Description = CleanString(System.Text.Encoding.Unicode.GetString(DesBytes));

                    s.cashshop_id = CashInfo.CashShopId;
                    s.Enabled = read.ReadByte();
                    s.unique_id = read.ReadInt();
                    s.Date1 = read.ReadZString(Encoding.ASCII, 64);
                    s.Date2 = read.ReadZString(Encoding.ASCII, 64);
                    s.nPurchaseCashType = read.ReadInt();
                    s.nStandardSellingPrice = read.ReadInt();
                    s.nRealSellingPrice = read.ReadInt();
                    s.nSalePersent = read.ReadInt();
                    s.nIconID = read.ReadInt();
                    s.nMaskType = read.ReadInt();
                    s.nDispType = read.ReadInt();
                    s.nDispCount = read.ReadInt();
                    int ItemCount = read.ReadInt();

                    for (int k = 0; k < ItemCount; k++)
                    {
                        var CashItem = new Item();
                        CashItem.ItemId = read.ReadInt();
                        CashItem.Amount = read.ReadInt();

                        s.CashItems.Add(CashItem);
                    }

                    CashInfo.CashInfo.Add(s);
                }
                cashShopDigimonInfo1.ReinforcedItems.Add(CashInfo);


            }
            #endregion

            cashShopDigimonInfo1.unknow8 = read.ReadInt();
            int RidingSize = read.ReadInt();
            var RidingBytes = read.ReadBytes(RidingSize * 2);
            cashShopDigimonInfo1.RidingName = CleanString(System.Text.Encoding.Unicode.GetString(RidingBytes));

            #region [Riding Items]
            int RidingCount = read.ReadInt();

            for (int i = 0; i < RidingCount; i++)
            {

                CashShopInformationCount CashInfo = new CashShopInformationCount();

                CashInfo.CashShopId = read.ReadInt();
                int count = read.ReadInt();
                for (int j = 0; j < count; j++)
                {
                    CASHINFO s = new CASHINFO();
                    int NameSize = read.ReadInt();
                    var NameBytes = read.ReadBytes(NameSize * 2);
                    s.Name = CleanString(System.Text.Encoding.Unicode.GetString(NameBytes));
                    int DesSize = read.ReadInt();
                    var DesBytes = read.ReadBytes(DesSize * 2);
                    s.Description = CleanString(System.Text.Encoding.Unicode.GetString(DesBytes));

                    s.cashshop_id = CashInfo.CashShopId;
                    s.Enabled = read.ReadByte();
                    s.unique_id = read.ReadInt();
                    s.Date1 = read.ReadZString(Encoding.ASCII, 64);
                    s.Date2 = read.ReadZString(Encoding.ASCII, 64);
                    s.nPurchaseCashType = read.ReadInt();
                    s.nStandardSellingPrice = read.ReadInt();
                    s.nRealSellingPrice = read.ReadInt();
                    s.nSalePersent = read.ReadInt();
                    s.nIconID = read.ReadInt();
                    s.nMaskType = read.ReadInt();
                    s.nDispType = read.ReadInt();
                    s.nDispCount = read.ReadInt();
                    int ItemCount = read.ReadInt();

                    for (int k = 0; k < ItemCount; k++)
                    {
                        var CashItem = new Item();
                        CashItem.ItemId = read.ReadInt();
                        CashItem.Amount = read.ReadInt();

                        s.CashItems.Add(CashItem);
                    }

                    CashInfo.CashInfo.Add(s);
                }
                cashShopDigimonInfo1.RidingItems.Add(CashInfo);
            }
            #endregion

            cashShopDigimonInfo1.unknow9 = read.ReadInt();
            int EtcSize = read.ReadInt();
            var EtcBytes = read.ReadBytes(EtcSize * 2);
            cashShopDigimonInfo1.EtcName = CleanString(System.Text.Encoding.Unicode.GetString(EtcBytes));

            #region [EtcItems]
            int EtcCount = read.ReadInt();

            for (int i = 0; i < EtcCount; i++)
            {


                CashShopInformationCount CashInfo = new CashShopInformationCount();

                CashInfo.CashShopId = read.ReadInt();
                int count = read.ReadInt();
                for (int j = 0; j < count; j++)
                {
                    CASHINFO s = new CASHINFO();
                    int NameSize = read.ReadInt();
                    var NameBytes = read.ReadBytes(NameSize * 2);
                    s.Name = CleanString(System.Text.Encoding.Unicode.GetString(NameBytes));
                    int DesSize = read.ReadInt();
                    var DesBytes = read.ReadBytes(DesSize * 2);
                    s.Description = CleanString(System.Text.Encoding.Unicode.GetString(DesBytes));

                    s.cashshop_id = CashInfo.CashShopId;
                    s.Enabled = read.ReadByte();
                    s.unique_id = read.ReadInt();
                    s.Date1 = read.ReadZString(Encoding.ASCII, 64);
                    s.Date2 = read.ReadZString(Encoding.ASCII, 64);
                    s.nPurchaseCashType = read.ReadInt();
                    s.nStandardSellingPrice = read.ReadInt();
                    s.nRealSellingPrice = read.ReadInt();
                    s.nSalePersent = read.ReadInt();
                    s.nIconID = read.ReadInt();
                    s.nMaskType = read.ReadInt();
                    s.nDispType = read.ReadInt();
                    s.nDispCount = read.ReadInt();
                    int ItemCount = read.ReadInt();

                    for (int k = 0; k < ItemCount; k++)
                    {
                        var CashItem = new Item();
                        CashItem.ItemId = read.ReadInt();
                        CashItem.Amount = read.ReadInt();

                        s.CashItems.Add(CashItem);
                    }

                    CashInfo.CashInfo.Add(s);
                }
                cashShopDigimonInfo1.EtcItems.Add(CashInfo);


            }
            #endregion

            return cashShopDigimonInfo1;
        }

        private static CashShopAvatarInfo LoadCashShopAvatarItems(BitReader read)
        {

            CashShopAvatarInfo cashShopAvatarInfo1 = new CashShopAvatarInfo();

            cashShopAvatarInfo1.unknow = read.ReadInt();
            int CategoryNameSize = read.ReadInt();
            var CategoryNameSizeBytes = read.ReadBytes(CategoryNameSize * 2);
            cashShopAvatarInfo1.CategoryName = CleanString(System.Text.Encoding.Unicode.GetString(CategoryNameSizeBytes));

            cashShopAvatarInfo1.unknow1 = read.ReadInt();
            cashShopAvatarInfo1.unknow2 = read.ReadInt();

            int AllSize = read.ReadInt();
            var AllSizeBytes = read.ReadBytes(AllSize * 2);
            cashShopAvatarInfo1.AllName = CleanString(System.Text.Encoding.Unicode.GetString(AllSizeBytes));

            cashShopAvatarInfo1.unknow3 = read.ReadInt();
            cashShopAvatarInfo1.unknow4 = read.ReadInt();

            int DigiEggSize = read.ReadInt();
            var DigiEggBytes = read.ReadBytes(DigiEggSize * 2);
            cashShopAvatarInfo1.ReinforcedName = CleanString(System.Text.Encoding.Unicode.GetString(DigiEggBytes));

            #region [Reinforce Items]
            int ReinforceCount = read.ReadInt();

            for (int i = 0; i < ReinforceCount; i++)
            {

                CashShopInformationCount CashInfo = new CashShopInformationCount();

                CashInfo.CashShopId = read.ReadInt();
                int count = read.ReadInt();
                for (int j = 0; j < count; j++)
                {
                    CASHINFO s = new CASHINFO();
                    int NameSize = read.ReadInt();
                    var NameBytes = read.ReadBytes(NameSize * 2);
                    s.Name = CleanString(System.Text.Encoding.Unicode.GetString(NameBytes));
                    int DesSize = read.ReadInt();
                    var DesBytes = read.ReadBytes(DesSize * 2);
                    s.Description = CleanString(System.Text.Encoding.Unicode.GetString(DesBytes));

                    s.cashshop_id = CashInfo.CashShopId;
                    s.Enabled = read.ReadByte();
                    s.unique_id = read.ReadInt();
                    s.Date1 = read.ReadZString(Encoding.ASCII, 64);
                    s.Date2 = read.ReadZString(Encoding.ASCII, 64);
                    s.nPurchaseCashType = read.ReadInt();
                    s.nStandardSellingPrice = read.ReadInt();
                    s.nRealSellingPrice = read.ReadInt();
                    s.nSalePersent = read.ReadInt();
                    s.nIconID = read.ReadInt();
                    s.nMaskType = read.ReadInt();
                    s.nDispType = read.ReadInt();
                    s.nDispCount = read.ReadInt();
                    int ItemCount = read.ReadInt();

                    for (int k = 0; k < ItemCount; k++)
                    {
                        var CashItem = new Item();
                        CashItem.ItemId = read.ReadInt();
                        CashItem.Amount = read.ReadInt();

                        s.CashItems.Add(CashItem);
                    }

                    CashInfo.CashInfo.Add(s);
                }
                cashShopAvatarInfo1.ReinforcedItems.Add(CashInfo);

            }
            #endregion

            cashShopAvatarInfo1.unknow5 = read.ReadInt();
            int HeadSize = read.ReadInt();
            var HeadSizeBytes = read.ReadBytes(HeadSize * 2);
            cashShopAvatarInfo1.HeadName = CleanString(System.Text.Encoding.Unicode.GetString(HeadSizeBytes));

            #region [Head Items]
            int HeadCount = read.ReadInt();

            for (int i = 0; i < HeadCount; i++)
            {

                CashShopInformationCount CashInfo = new CashShopInformationCount();

                CashInfo.CashShopId = read.ReadInt();
                int count = read.ReadInt();
                for (int j = 0; j < count; j++)
                {
                    CASHINFO s = new CASHINFO();
                    int NameSize = read.ReadInt();
                    var NameBytes = read.ReadBytes(NameSize * 2);
                    s.Name = CleanString(System.Text.Encoding.Unicode.GetString(NameBytes));
                    int DesSize = read.ReadInt();
                    var DesBytes = read.ReadBytes(DesSize * 2);
                    s.Description = CleanString(System.Text.Encoding.Unicode.GetString(DesBytes));

                    s.cashshop_id = CashInfo.CashShopId;
                    s.Enabled = read.ReadByte();
                    s.unique_id = read.ReadInt();
                    s.Date1 = read.ReadZString(Encoding.ASCII, 64);
                    s.Date2 = read.ReadZString(Encoding.ASCII, 64);
                    s.nPurchaseCashType = read.ReadInt();
                    s.nStandardSellingPrice = read.ReadInt();
                    s.nRealSellingPrice = read.ReadInt();
                    s.nSalePersent = read.ReadInt();
                    s.nIconID = read.ReadInt();
                    s.nMaskType = read.ReadInt();
                    s.nDispType = read.ReadInt();
                    s.nDispCount = read.ReadInt();
                    int ItemCount = read.ReadInt();

                    for (int k = 0; k < ItemCount; k++)
                    {
                        var CashItem = new Item();
                        CashItem.ItemId = read.ReadInt();
                        CashItem.Amount = read.ReadInt();

                        s.CashItems.Add(CashItem);
                    }

                    CashInfo.CashInfo.Add(s);
                }
                cashShopAvatarInfo1.HeadItems.Add(CashInfo);

            }
            #endregion

            cashShopAvatarInfo1.unknow6 = read.ReadInt();
            int TopSize = read.ReadInt();
            var TopSizeBytes = read.ReadBytes(TopSize * 2);
            cashShopAvatarInfo1.TopName = CleanString(System.Text.Encoding.Unicode.GetString(TopSizeBytes));
            #region [Top Items]
            int TopCount = read.ReadInt();

            for (int i = 0; i < TopCount; i++)
            {


                CashShopInformationCount CashInfo = new CashShopInformationCount();

                CashInfo.CashShopId = read.ReadInt();
                int count = read.ReadInt();
                for (int j = 0; j < count; j++)
                {
                    CASHINFO s = new CASHINFO();
                    int NameSize = read.ReadInt();
                    var NameBytes = read.ReadBytes(NameSize * 2);
                    s.Name = CleanString(System.Text.Encoding.Unicode.GetString(NameBytes));
                    int DesSize = read.ReadInt();
                    var DesBytes = read.ReadBytes(DesSize * 2);
                    s.Description = CleanString(System.Text.Encoding.Unicode.GetString(DesBytes));

                    s.cashshop_id = CashInfo.CashShopId;
                    s.Enabled = read.ReadByte();
                    s.unique_id = read.ReadInt();
                    s.Date1 = read.ReadZString(Encoding.ASCII, 64);
                    s.Date2 = read.ReadZString(Encoding.ASCII, 64);
                    s.nPurchaseCashType = read.ReadInt();
                    s.nStandardSellingPrice = read.ReadInt();
                    s.nRealSellingPrice = read.ReadInt();
                    s.nSalePersent = read.ReadInt();
                    s.nIconID = read.ReadInt();
                    s.nMaskType = read.ReadInt();
                    s.nDispType = read.ReadInt();
                    s.nDispCount = read.ReadInt();
                    int ItemCount = read.ReadInt();

                    for (int k = 0; k < ItemCount; k++)
                    {
                        var CashItem = new Item();
                        CashItem.ItemId = read.ReadInt();
                        CashItem.Amount = read.ReadInt();

                        s.CashItems.Add(CashItem);
                    }

                    CashInfo.CashInfo.Add(s);
                }
                cashShopAvatarInfo1.TopItems.Add(CashInfo);
            }
            #endregion

            cashShopAvatarInfo1.unknow7 = read.ReadInt();
            int BottomSize = read.ReadInt();
            var BottomSizeBytes = read.ReadBytes(BottomSize * 2);
            cashShopAvatarInfo1.BottomName = CleanString(System.Text.Encoding.Unicode.GetString(BottomSizeBytes));

            #region [Bottom Items]
            int BottomCount = read.ReadInt();

            for (int i = 0; i < BottomCount; i++)
            {
                CashShopInformationCount CashInfo = new CashShopInformationCount();

                CashInfo.CashShopId = read.ReadInt();
                int count = read.ReadInt();
                for (int j = 0; j < count; j++)
                {
                    CASHINFO s = new CASHINFO();
                    int NameSize = read.ReadInt();
                    var NameBytes = read.ReadBytes(NameSize * 2);
                    s.Name = CleanString(System.Text.Encoding.Unicode.GetString(NameBytes));
                    int DesSize = read.ReadInt();
                    var DesBytes = read.ReadBytes(DesSize * 2);
                    s.Description = CleanString(System.Text.Encoding.Unicode.GetString(DesBytes));

                    s.cashshop_id = CashInfo.CashShopId;
                    s.Enabled = read.ReadByte();
                    s.unique_id = read.ReadInt();
                    s.Date1 = read.ReadZString(Encoding.ASCII, 64);
                    s.Date2 = read.ReadZString(Encoding.ASCII, 64);
                    s.nPurchaseCashType = read.ReadInt();
                    s.nStandardSellingPrice = read.ReadInt();
                    s.nRealSellingPrice = read.ReadInt();
                    s.nSalePersent = read.ReadInt();
                    s.nIconID = read.ReadInt();
                    s.nMaskType = read.ReadInt();
                    s.nDispType = read.ReadInt();
                    s.nDispCount = read.ReadInt();
                    int ItemCount = read.ReadInt();

                    for (int k = 0; k < ItemCount; k++)
                    {
                        var CashItem = new Item();
                        CashItem.ItemId = read.ReadInt();
                        CashItem.Amount = read.ReadInt();

                        s.CashItems.Add(CashItem);
                    }

                    CashInfo.CashInfo.Add(s);
                }
                cashShopAvatarInfo1.BottomItems.Add(CashInfo);

            }
            #endregion

            cashShopAvatarInfo1.unknow8 = read.ReadInt();
            int GlovesSize = read.ReadInt();
            var GlovesSizeBytes = read.ReadBytes(GlovesSize * 2);
            cashShopAvatarInfo1.GlovesName = CleanString(System.Text.Encoding.Unicode.GetString(GlovesSizeBytes));

            #region [Gloves Items]
            int GlovesCount = read.ReadInt();

            for (int i = 0; i < GlovesCount; i++)
            {
                CashShopInformationCount CashInfo = new CashShopInformationCount();

                CashInfo.CashShopId = read.ReadInt();
                int count = read.ReadInt();
                for (int j = 0; j < count; j++)
                {
                    CASHINFO s = new CASHINFO();
                    int NameSize = read.ReadInt();
                    var NameBytes = read.ReadBytes(NameSize * 2);
                    s.Name = CleanString(System.Text.Encoding.Unicode.GetString(NameBytes));
                    int DesSize = read.ReadInt();
                    var DesBytes = read.ReadBytes(DesSize * 2);
                    s.Description = CleanString(System.Text.Encoding.Unicode.GetString(DesBytes));

                    s.cashshop_id = CashInfo.CashShopId;
                    s.Enabled = read.ReadByte();
                    s.unique_id = read.ReadInt();
                    s.Date1 = read.ReadZString(Encoding.ASCII, 64);
                    s.Date2 = read.ReadZString(Encoding.ASCII, 64);
                    s.nPurchaseCashType = read.ReadInt();
                    s.nStandardSellingPrice = read.ReadInt();
                    s.nRealSellingPrice = read.ReadInt();
                    s.nSalePersent = read.ReadInt();
                    s.nIconID = read.ReadInt();
                    s.nMaskType = read.ReadInt();
                    s.nDispType = read.ReadInt();
                    s.nDispCount = read.ReadInt();
                    int ItemCount = read.ReadInt();

                    for (int k = 0; k < ItemCount; k++)
                    {
                        var CashItem = new Item();
                        CashItem.ItemId = read.ReadInt();
                        CashItem.Amount = read.ReadInt();

                        s.CashItems.Add(CashItem);
                    }

                    CashInfo.CashInfo.Add(s);
                }
                cashShopAvatarInfo1.GlovesItems.Add(CashInfo);

            }
            #endregion

            cashShopAvatarInfo1.unknow9 = read.ReadInt();
            int ShoesSize = read.ReadInt();
            var ShoesSizeBytes = read.ReadBytes(ShoesSize * 2);
            cashShopAvatarInfo1.ShoesName = CleanString(System.Text.Encoding.Unicode.GetString(ShoesSizeBytes));

            #region [Shoes Items]
            int ShoesCount = read.ReadInt();

            for (int i = 0; i < ShoesCount; i++)
            {

                CashShopInformationCount CashInfo = new CashShopInformationCount();

                CashInfo.CashShopId = read.ReadInt();
                int count = read.ReadInt();
                for (int j = 0; j < count; j++)
                {
                    CASHINFO s = new CASHINFO();
                    int NameSize = read.ReadInt();
                    var NameBytes = read.ReadBytes(NameSize * 2);
                    s.Name = CleanString(System.Text.Encoding.Unicode.GetString(NameBytes));
                    int DesSize = read.ReadInt();
                    var DesBytes = read.ReadBytes(DesSize * 2);
                    s.Description = CleanString(System.Text.Encoding.Unicode.GetString(DesBytes));

                    s.cashshop_id = CashInfo.CashShopId;
                    s.Enabled = read.ReadByte();
                    s.unique_id = read.ReadInt();
                    s.Date1 = read.ReadZString(Encoding.ASCII, 64);
                    s.Date2 = read.ReadZString(Encoding.ASCII, 64);
                    s.nPurchaseCashType = read.ReadInt();
                    s.nStandardSellingPrice = read.ReadInt();
                    s.nRealSellingPrice = read.ReadInt();
                    s.nSalePersent = read.ReadInt();
                    s.nIconID = read.ReadInt();
                    s.nMaskType = read.ReadInt();
                    s.nDispType = read.ReadInt();
                    s.nDispCount = read.ReadInt();
                    int ItemCount = read.ReadInt();

                    for (int k = 0; k < ItemCount; k++)
                    {
                        var CashItem = new Item();
                        CashItem.ItemId = read.ReadInt();
                        CashItem.Amount = read.ReadInt();

                        s.CashItems.Add(CashItem);
                    }

                    CashInfo.CashInfo.Add(s);
                }
                cashShopAvatarInfo1.ShoesItems.Add(CashInfo);

            }
            #endregion

            cashShopAvatarInfo1.unknow10 = read.ReadInt();
            int FashionSize = read.ReadInt();
            var FashionSizeBytes = read.ReadBytes(FashionSize * 2);
            cashShopAvatarInfo1.FashionName = CleanString(System.Text.Encoding.Unicode.GetString(FashionSizeBytes));

            #region [Fashion Items]
            int FashionCount = read.ReadInt();

            for (int i = 0; i < FashionCount; i++)
            {

                CashShopInformationCount CashInfo = new CashShopInformationCount();

                CashInfo.CashShopId = read.ReadInt();
                int count = read.ReadInt();
                for (int j = 0; j < count; j++)
                {
                    CASHINFO s = new CASHINFO();
                    int NameSize = read.ReadInt();
                    var NameBytes = read.ReadBytes(NameSize * 2);
                    s.Name = CleanString(System.Text.Encoding.Unicode.GetString(NameBytes));
                    int DesSize = read.ReadInt();
                    var DesBytes = read.ReadBytes(DesSize * 2);
                    s.Description = CleanString(System.Text.Encoding.Unicode.GetString(DesBytes));

                    s.cashshop_id = CashInfo.CashShopId;
                    s.Enabled = read.ReadByte();
                    s.unique_id = read.ReadInt();
                    s.Date1 = read.ReadZString(Encoding.ASCII, 64);
                    s.Date2 = read.ReadZString(Encoding.ASCII, 64);
                    s.nPurchaseCashType = read.ReadInt();
                    s.nStandardSellingPrice = read.ReadInt();
                    s.nRealSellingPrice = read.ReadInt();
                    s.nSalePersent = read.ReadInt();
                    s.nIconID = read.ReadInt();
                    s.nMaskType = read.ReadInt();
                    s.nDispType = read.ReadInt();
                    s.nDispCount = read.ReadInt();
                    int ItemCount = read.ReadInt();

                    for (int k = 0; k < ItemCount; k++)
                    {
                        var CashItem = new Item();
                        CashItem.ItemId = read.ReadInt();
                        CashItem.Amount = read.ReadInt();

                        s.CashItems.Add(CashItem);
                    }

                    CashInfo.CashInfo.Add(s);
                }
                cashShopAvatarInfo1.FashionItems.Add(CashInfo);


            }
            #endregion

            cashShopAvatarInfo1.unknow11 = read.ReadInt();
            int CostumeSize = read.ReadInt();
            var CostumeSizeBytes = read.ReadBytes(CostumeSize * 2);
            cashShopAvatarInfo1.CostumeName = CleanString(System.Text.Encoding.Unicode.GetString(CostumeSizeBytes));

            #region [Costume Items]
            int CostumeCount = read.ReadInt();

            for (int i = 0; i < CostumeCount; i++)
            {

                CashShopInformationCount CashInfo = new CashShopInformationCount();

                CashInfo.CashShopId = read.ReadInt();
                int count = read.ReadInt();
                for (int j = 0; j < count; j++)
                {
                    CASHINFO s = new CASHINFO();
                    int NameSize = read.ReadInt();
                    var NameBytes = read.ReadBytes(NameSize * 2);
                    s.Name = CleanString(System.Text.Encoding.Unicode.GetString(NameBytes));
                    int DesSize = read.ReadInt();
                    var DesBytes = read.ReadBytes(DesSize * 2);
                    s.Description = CleanString(System.Text.Encoding.Unicode.GetString(DesBytes));

                    s.cashshop_id = CashInfo.CashShopId;
                    s.Enabled = read.ReadByte();
                    s.unique_id = read.ReadInt();
                    s.Date1 = read.ReadZString(Encoding.ASCII, 64);
                    s.Date2 = read.ReadZString(Encoding.ASCII, 64);
                    s.nPurchaseCashType = read.ReadInt();
                    s.nStandardSellingPrice = read.ReadInt();
                    s.nRealSellingPrice = read.ReadInt();
                    s.nSalePersent = read.ReadInt();
                    s.nIconID = read.ReadInt();
                    s.nMaskType = read.ReadInt();
                    s.nDispType = read.ReadInt();
                    s.nDispCount = read.ReadInt();
                    int ItemCount = read.ReadInt();

                    for (int k = 0; k < ItemCount; k++)
                    {
                        var CashItem = new Item();
                        CashItem.ItemId = read.ReadInt();
                        CashItem.Amount = read.ReadInt();

                        s.CashItems.Add(CashItem);
                    }

                    CashInfo.CashInfo.Add(s);
                }
                cashShopAvatarInfo1.CostumeItems.Add(CashInfo);

            }
            #endregion

            return cashShopAvatarInfo1;
        }

        private static CashShopPackageInfo LoadCashShopPackageItems(BitReader read)
        {

            CashShopPackageInfo cashShopPackageInfo1 = new CashShopPackageInfo();

            cashShopPackageInfo1.unknow = read.ReadInt();
            int CategoryNameSize = read.ReadInt();
            var CategoryNameSizeBytes = read.ReadBytes(CategoryNameSize * 2);
            cashShopPackageInfo1.CategoryName = CleanString(System.Text.Encoding.Unicode.GetString(CategoryNameSizeBytes));

            cashShopPackageInfo1.unknow1 = read.ReadInt();
            cashShopPackageInfo1.unknow2 = read.ReadInt();

            int AllSize = read.ReadInt();
            var AllSizeBytes = read.ReadBytes(AllSize * 2);
            cashShopPackageInfo1.AllName = CleanString(System.Text.Encoding.Unicode.GetString(AllSizeBytes));


            #region [Package Items]
            int PackageCount = read.ReadInt();

            for (int i = 0; i < PackageCount; i++)
            {
                CashShopInformationCount CashInfo = new CashShopInformationCount();

                CashInfo.CashShopId = read.ReadInt();
                int count = read.ReadInt();
                for (int j = 0; j < count; j++)
                {
                    CASHINFO s = new CASHINFO();
                    int NameSize = read.ReadInt();
                    var NameBytes = read.ReadBytes(NameSize * 2);
                    s.Name = CleanString(System.Text.Encoding.Unicode.GetString(NameBytes));
                    int DesSize = read.ReadInt();
                    var DesBytes = read.ReadBytes(DesSize * 2);
                    s.Description = CleanString(System.Text.Encoding.Unicode.GetString(DesBytes));

                    s.cashshop_id = CashInfo.CashShopId;
                    s.Enabled = read.ReadByte();
                    s.unique_id = read.ReadInt();
                    s.Date1 = read.ReadZString(Encoding.ASCII, 64);
                    s.Date2 = read.ReadZString(Encoding.ASCII, 64);
                    s.nPurchaseCashType = read.ReadInt();
                    s.nStandardSellingPrice = read.ReadInt();
                    s.nRealSellingPrice = read.ReadInt();
                    s.nSalePersent = read.ReadInt();
                    s.nIconID = read.ReadInt();
                    s.nMaskType = read.ReadInt();
                    s.nDispType = read.ReadInt();
                    s.nDispCount = read.ReadInt();
                    int ItemCount = read.ReadInt();

                    for (int k = 0; k < ItemCount; k++)
                    {
                        var CashItem = new Item();
                        CashItem.ItemId = read.ReadInt();
                        CashItem.Amount = read.ReadInt();

                        s.CashItems.Add(CashItem);
                    }

                    CashInfo.CashInfo.Add(s);
                }

                cashShopPackageInfo1.PackageItems.Add(CashInfo);

            }

            #endregion

            return cashShopPackageInfo1;
        }

        private static CashShopMainInformation LoadCashMainInfo(BitReader read)
        {


            CashShopMainInformation Main = new CashShopMainInformation();

            Main.unknow = read.ReadInt();
            Main.unknow1 = read.ReadInt();
            Main.unknow2 = read.ReadInt();
            int TitleSize = read.ReadInt();
            var TitleBytes = read.ReadBytes(TitleSize * 2);
            Main.MainTitle = CleanString(System.Text.Encoding.Unicode.GetString(TitleBytes));
            Main.unknow3 = read.ReadInt();
            Main.unknow4 = read.ReadInt();
            int TitleSize2 = read.ReadInt();
            var TitleBytes2 = read.ReadBytes(TitleSize2 * 2);
            Main.MainNewTitle = CleanString(System.Text.Encoding.Unicode.GetString(TitleBytes2));

            int NewItemsCount = read.ReadInt();
            for (int j = 0; j < NewItemsCount; j++)
            {
                MainCashShopInfo MainItem = new MainCashShopInfo();
                MainItem.ProductID = read.ReadLong();
                Main.MainNewItems.Add(MainItem);
            }
            Main.unknow5 = read.ReadInt();
            int TitleSize3 = read.ReadInt();
            var TitleBytes3 = read.ReadBytes(TitleSize3 * 2);
            Main.MainHotTitle = CleanString(System.Text.Encoding.Unicode.GetString(TitleBytes3));
            int HotItemsCount = read.ReadInt();
            for (int j = 0; j < HotItemsCount; j++)
            {
                MainCashShopInfo MainItem = new MainCashShopInfo();
                MainItem.ProductID = read.ReadLong();
                Main.MainHotItems.Add(MainItem);
            }
            Main.unknow6 = read.ReadInt();
            int TitleSize4 = read.ReadInt();
            var TitleBytes4 = read.ReadBytes(TitleSize4 * 2);
            Main.MainEventTitle = CleanString(System.Text.Encoding.Unicode.GetString(TitleBytes4));
            int EventItemsCount = read.ReadInt();
            for (int j = 0; j < EventItemsCount; j++)
            {
                MainCashShopInfo MainItem = new MainCashShopInfo();
                MainItem.ProductID = read.ReadLong();
                Main.MainEventItems.Add(MainItem);
            }

            Main.unknow7 = read.ReadLong();
            int UnknowItemsCount = read.ReadInt();
            for (int j = 0; j < UnknowItemsCount; j++)
            {
                MainCashShopInfo MainItem = new MainCashShopInfo();
                MainItem.ProductID = read.ReadLong();
                Main.MainUnknowItems.Add(MainItem);
            }

            Main.unknow8 = read.ReadInt();

            return Main;
        }


        private static CashWebData[] LoadCashWebData(BitReader read)
        {

            var CashWebDataCount = read.ReadInt();
            CashWebData[] cashWebDataInfo = new CashWebData[CashWebDataCount];

            for (int i = 0; i < CashWebDataCount; i++)
            {
                CashWebData CashWebData = new();
                CashWebData.nTableType = read.ReadInt();
                CashWebData.m_mapWebData = read.ReadInt();
                for (int j = 0; j < CashWebData.m_mapWebData; j++)
                {
                    CashWebDataInfo CashWeb = new();
                    CashWeb.Size = read.ReadInt();
                    byte[] mname_get = new byte[CashWeb.Size];
                    for (int g = 0; g < CashWeb.Size; g++)
                    {
                        mname_get[g] = read.ReadByte();
                    }
                    CashWeb.sWebImageFile = Encoding.ASCII.GetString(mname_get).Trim();
                    CashWeb.Size2 = read.ReadInt();
                    mname_get = new byte[CashWeb.Size2];
                    for (int g = 0; g < CashWeb.Size2; g++)
                    {
                        mname_get[g] = read.ReadByte();
                    }
                    CashWeb.sWebLinkUrl = CleanString(Encoding.ASCII.GetString(mname_get).Trim());
                    CashWebData.CashWebInfo.Add(CashWeb);
                }

                cashWebDataInfo[i] = CashWebData;
            }
            return cashWebDataInfo;
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
        public static void ExportCashTamerInfoToXml(CashShopTamerInfo cashShopTamerInfo, int index)
        {

            XElement cashShopTamerElement = new XElement("CashShopTamerInfo",
                    new XElement("CategoryName", cashShopTamerInfo.CategoryName),
                    new XElement("unknow", cashShopTamerInfo.unknow),
                    new XElement("unknow1", cashShopTamerInfo.unknow1),
                    new XElement("AllName", cashShopTamerInfo.AllName),
                    new XElement("unknow2", cashShopTamerInfo.unknow2),
                    new XElement("unknow3", cashShopTamerInfo.unknow3),
                    new XElement("ExpasionName", cashShopTamerInfo.ExpasionName),
                    new XElement("unknow4", cashShopTamerInfo.unknow4),
                    new XElement("ExpName", cashShopTamerInfo.ExpName),
                    new XElement("unknow5", cashShopTamerInfo.unknow5),
                    new XElement("MovimentName", cashShopTamerInfo.MovimentName),
                    new XElement("unknow6", cashShopTamerInfo.unknow6),
                    new XElement("ChatName", cashShopTamerInfo.ChatName),
                    new XElement("unknow7", cashShopTamerInfo.unknow7),
                    new XElement("EtcName", cashShopTamerInfo.EtcName)
                );

            if (index > 0)
            {
                string cashShopTamerFilename = $"CashShopTamerInfo.xml";
                var TamerCashOutput = $"XML\\CashShop\\TamerInfo{index}";
                string cashShopTamerFilePath = Path.Combine(TamerCashOutput, cashShopTamerFilename);
                cashShopTamerElement.Save(cashShopTamerFilePath);

                string ExpansionPath = $"XML\\CashShop\\TamerInfo{index}\\Expansion\\Expansion.xml";
                string ExpPath = $"XML\\CashShop\\TamerInfo{index}\\Exp\\Exp.xml";
                string MovimentPath = $"XML\\CashShop\\TamerInfo{index}\\Moviment\\Moviment.xml";
                string ChatPath = $"XML\\CashShop\\TamerInfo{index}\\Chat\\Chat.xml";
                string EtcPath = $"XML\\CashShop\\TamerInfo{index}\\Etc\\Etc.xml";

                ExportItemsToXml(cashShopTamerInfo.ExpansionItems, ExpansionPath);
                ExportItemsToXml(cashShopTamerInfo.ExpItems, ExpPath);
                ExportItemsToXml(cashShopTamerInfo.MovimentItems, MovimentPath);
                ExportItemsToXml(cashShopTamerInfo.ChatItems, ChatPath);
                ExportItemsToXml(cashShopTamerInfo.EtcItems, EtcPath);
            }
            else
            {

                string cashShopTamerFilename = $"CashShopTamerInfo.xml";
                var TamerCashOutput = $"XML\\CashShop\\TamerInfo";
                string cashShopTamerFilePath = Path.Combine(TamerCashOutput, cashShopTamerFilename);
                cashShopTamerElement.Save(cashShopTamerFilePath);

                string ExpansionPath = $"XML\\CashShop\\TamerInfo\\Expansion\\Expansion.xml";
                string ExpPath = $"XML\\CashShop\\TamerInfo\\Exp\\Exp.xml";
                string MovimentPath = $"XML\\CashShop\\TamerInfo\\Moviment\\Moviment.xml";
                string ChatPath = $"XML\\CashShop\\TamerInfo\\Chat\\Chat.xml";
                string EtcPath = $"XML\\CashShop\\TamerInfo\\Etc\\Etc.xml";

                ExportItemsToXml(cashShopTamerInfo.ExpansionItems, ExpansionPath);
                ExportItemsToXml(cashShopTamerInfo.ExpItems, ExpPath);
                ExportItemsToXml(cashShopTamerInfo.MovimentItems, MovimentPath);
                ExportItemsToXml(cashShopTamerInfo.ChatItems, ChatPath);
                ExportItemsToXml(cashShopTamerInfo.EtcItems, EtcPath);
            }
        }
        public static void ExportCashDigimonInfoToXml(CashShopDigimonInfo cashShopDigimonInfo, int index)
        {
            XElement cashShopDigimonElement = new XElement("CashShopDigimonInfo",
                new XElement("CategoryNameSize", cashShopDigimonInfo.CategoryNameSize),
                new XElement("CategoryName", cashShopDigimonInfo.CategoryName),
                new XElement("unknow", cashShopDigimonInfo.unknow),
                new XElement("unknow1", cashShopDigimonInfo.unknow1),
                new XElement("AllName", cashShopDigimonInfo.AllName),
                new XElement("unknow2", cashShopDigimonInfo.unknow2),
                new XElement("unknow3", cashShopDigimonInfo.unknow3),
                new XElement("unknow4", cashShopDigimonInfo.unknow4),
                new XElement("DigiEggName", cashShopDigimonInfo.DigiEggName),
                new XElement("unknow5", cashShopDigimonInfo.unknow5),
                new XElement("EvolutionName", cashShopDigimonInfo.EvolutionName),
                new XElement("unknow6", cashShopDigimonInfo.unknow6),
                new XElement("HatchName", cashShopDigimonInfo.HatchName),
                new XElement("unknow7", cashShopDigimonInfo.unknow7),
                new XElement("ReinforcedName", cashShopDigimonInfo.ReinforcedName),
                new XElement("unknow8", cashShopDigimonInfo.unknow8),
                new XElement("RidingName", cashShopDigimonInfo.RidingName),
                new XElement("unknow9", cashShopDigimonInfo.unknow9),
                new XElement("EtcName", cashShopDigimonInfo.EtcName)
            );

            if (index > 0)
            {
                string cashShopDigimonFilename = $"CashShopDigimonInfo.xml";
                var digimonCashOutput = $"XML\\CashShop\\DigimonInfo{index}";
                string cashShopDigimonFilePath = Path.Combine(digimonCashOutput, cashShopDigimonFilename);
                cashShopDigimonElement.Save(cashShopDigimonFilePath);

                string expansionPath = $"XML\\CashShop\\DigimonInfo{index}\\Expansion\\Expansion.xml";
                string digiEggPath = $"XML\\CashShop\\DigimonInfo{index}\\DigiEgg\\DigiEgg.xml";
                string evolutionPath = $"XML\\CashShop\\DigimonInfo{index}\\Evolution\\Evolution.xml";
                string hatchPath = $"XML\\CashShop\\DigimonInfo{index}\\Hatch\\Hatch.xml";
                string reinforcedPath = $"XML\\CashShop\\DigimonInfo{index}\\Reinforced\\Reinforced.xml";
                string ridingPath = $"XML\\CashShop\\DigimonInfo{index}\\Riding\\Riding.xml";
                string etcPath = $"XML\\CashShop\\DigimonInfo{index}\\Etc\\Etc.xml";

                ExportItemsToXml(cashShopDigimonInfo.DigiEgg, digiEggPath);
                ExportItemsToXml(cashShopDigimonInfo.EvolutionItems, evolutionPath);
                ExportItemsToXml(cashShopDigimonInfo.HatchItems, hatchPath);
                ExportItemsToXml(cashShopDigimonInfo.ReinforcedItems, reinforcedPath);
                ExportItemsToXml(cashShopDigimonInfo.RidingItems, ridingPath);
                ExportItemsToXml(cashShopDigimonInfo.EtcItems, etcPath);
            }
            else
            {
                string cashShopDigimonFilename = "CashShopDigimonInfo.xml";
                var digimonCashOutput = $"XML\\CashShop\\DigimonInfo";
                string cashShopDigimonFilePath = Path.Combine(digimonCashOutput, cashShopDigimonFilename);
                cashShopDigimonElement.Save(cashShopDigimonFilePath);

                string expansionPath = $"XML\\CashShop\\DigimonInfo\\Expansion\\Expansion.xml";
                string digiEggPath = $"XML\\CashShop\\DigimonInfo\\DigiEgg\\DigiEgg.xml";
                string evolutionPath = $"XML\\CashShop\\DigimonInfo\\Evolution\\Evolution.xml";
                string hatchPath = $"XML\\CashShop\\DigimonInfo\\Hatch\\Hatch.xml";
                string reinforcedPath = $"XML\\CashShop\\DigimonInfo\\Reinforced\\Reinforced.xml";
                string ridingPath = $"XML\\CashShop\\DigimonInfo\\Riding\\Riding.xml";
                string etcPath = $"XML\\CashShop\\DigimonInfo\\Etc\\Etc.xml";

                ExportItemsToXml(cashShopDigimonInfo.DigiEgg, digiEggPath);
                ExportItemsToXml(cashShopDigimonInfo.EvolutionItems, evolutionPath);
                ExportItemsToXml(cashShopDigimonInfo.HatchItems, hatchPath);
                ExportItemsToXml(cashShopDigimonInfo.ReinforcedItems, reinforcedPath);
                ExportItemsToXml(cashShopDigimonInfo.RidingItems, ridingPath);
                ExportItemsToXml(cashShopDigimonInfo.EtcItems, etcPath);

            }
        }
        public static void ExportCashAvatarInfoToXml(CashShopAvatarInfo cashShopAvatarInfo, int index)
        {
            XElement cashShopAvatarElement = new XElement("CashShopAvatarInfo",
                new XElement("CategoryNameSize", cashShopAvatarInfo.CategoryNameSize),
                new XElement("CategoryName", cashShopAvatarInfo.CategoryName),
                new XElement("unknow", cashShopAvatarInfo.unknow),
                new XElement("unknow1", cashShopAvatarInfo.unknow1),
                new XElement("AllName", cashShopAvatarInfo.AllName),
                new XElement("unknow2", cashShopAvatarInfo.unknow2),
                new XElement("unknow3", cashShopAvatarInfo.unknow3),
                new XElement("ReinforcedName", cashShopAvatarInfo.ReinforcedName),
                new XElement("unknow4", cashShopAvatarInfo.unknow4),
                new XElement("unknow5", cashShopAvatarInfo.unknow5),
                new XElement("HeadName", cashShopAvatarInfo.HeadName),
                new XElement("unknow6", cashShopAvatarInfo.unknow6),
                new XElement("TopName", cashShopAvatarInfo.TopName),
                new XElement("unknow7", cashShopAvatarInfo.unknow7),
                new XElement("BottomName", cashShopAvatarInfo.BottomName),
                new XElement("unknow8", cashShopAvatarInfo.unknow8),
                new XElement("GlovesName", cashShopAvatarInfo.GlovesName),
                new XElement("unknow9", cashShopAvatarInfo.unknow9),
                new XElement("ShoesName", cashShopAvatarInfo.ShoesName),
                new XElement("unknow10", cashShopAvatarInfo.unknow10),
                new XElement("FashionName", cashShopAvatarInfo.FashionName),
                new XElement("unknow11", cashShopAvatarInfo.unknow11),
                new XElement("CostumeName", cashShopAvatarInfo.CostumeName)
            );

            if (index > 0)
            {
                string cashShopAvatarFilename = $"CashShopAvatarInfo.xml";
                var avatarCashOutput = $"XML\\CashShop\\AvatarInfo{index}";
                string cashShopAvatarFilePath = Path.Combine(avatarCashOutput, cashShopAvatarFilename);
                cashShopAvatarElement.Save(cashShopAvatarFilePath);

                string reinforcedPath = $"XML\\CashShop\\AvatarInfo{index}\\Reinforced\\Reinforced.xml";
                string digiEggPath = $"XML\\CashShop\\AvatarInfo{index}\\DigiEgg\\DigiEgg.xml";
                string headPath = $"XML\\CashShop\\AvatarInfo{index}\\Head\\Head.xml";
                string topPath = $"XML\\CashShop\\AvatarInfo{index}\\Top\\Top.xml";
                string bottomPath = $"XML\\CashShop\\AvatarInfo{index}\\Bottom\\Bottom.xml";
                string glovesPath = $"XML\\CashShop\\AvatarInfo{index}\\Gloves\\Gloves.xml";
                string shoesPath = $"XML\\CashShop\\AvatarInfo{index}\\Shoes\\Shoes.xml";
                string fashionPath = $"XML\\CashShop\\AvatarInfo{index}\\Fashion\\Fashion.xml";
                string costumePath = $"XML\\CashShop\\AvatarInfo{index}\\Costume\\Costume.xml";

                ExportItemsToXml(cashShopAvatarInfo.ReinforcedItems, reinforcedPath);
                ExportItemsToXml(cashShopAvatarInfo.HeadItems, headPath);
                ExportItemsToXml(cashShopAvatarInfo.TopItems, topPath);
                ExportItemsToXml(cashShopAvatarInfo.BottomItems, bottomPath);
                ExportItemsToXml(cashShopAvatarInfo.GlovesItems, glovesPath);
                ExportItemsToXml(cashShopAvatarInfo.ShoesItems, shoesPath);
                ExportItemsToXml(cashShopAvatarInfo.FashionItems, fashionPath);
                ExportItemsToXml(cashShopAvatarInfo.CostumeItems, costumePath);
            }
            else
            {
                string cashShopAvatarFilename = $"CashShopAvatarInfo.xml";
                var avatarCashOutput = $"XML\\CashShop\\AvatarInfo";
                string cashShopAvatarFilePath = Path.Combine(avatarCashOutput, cashShopAvatarFilename);
                cashShopAvatarElement.Save(cashShopAvatarFilePath);

                string reinforcedPath = $"XML\\CashShop\\AvatarInfo\\Reinforced\\Reinforced.xml";
                string digiEggPath = $"XML\\CashShop\\AvatarInfo\\DigiEgg\\DigiEgg.xml";
                string headPath = $"XML\\CashShop\\AvatarInfo\\Head\\Head.xml";
                string topPath = $"XML\\CashShop\\AvatarInfo\\Top\\Top.xml";
                string bottomPath = $"XML\\CashShop\\AvatarInfo\\Bottom\\Bottom.xml";
                string glovesPath = $"XML\\CashShop\\AvatarInfo\\Gloves\\Gloves.xml";
                string shoesPath = $"XML\\CashShop\\AvatarInfo\\Shoes\\Shoes.xml";
                string fashionPath = $"XML\\CashShop\\AvatarInfo\\Fashion\\Fashion.xml";
                string costumePath = $"XML\\CashShop\\AvatarInfo\\Costume\\Costume.xml";

                ExportItemsToXml(cashShopAvatarInfo.ReinforcedItems, reinforcedPath);
                ExportItemsToXml(cashShopAvatarInfo.HeadItems, headPath);
                ExportItemsToXml(cashShopAvatarInfo.TopItems, topPath);
                ExportItemsToXml(cashShopAvatarInfo.BottomItems, bottomPath);
                ExportItemsToXml(cashShopAvatarInfo.GlovesItems, glovesPath);
                ExportItemsToXml(cashShopAvatarInfo.ShoesItems, shoesPath);
                ExportItemsToXml(cashShopAvatarInfo.FashionItems, fashionPath);
                ExportItemsToXml(cashShopAvatarInfo.CostumeItems, costumePath);

            }
        }
        public static void ExportCashPackageInfoToXml(CashShopPackageInfo cashShopPackageInfo, int index)
        {
            XElement cashShopPackageElement = new XElement("CashShopPackageInfo",
                new XElement("CategoryNameSize", cashShopPackageInfo.CategoryNameSize),
                new XElement("CategoryName", cashShopPackageInfo.CategoryName),
                new XElement("unknow", cashShopPackageInfo.unknow),
                new XElement("unknow1", cashShopPackageInfo.unknow1),
                new XElement("AllName", cashShopPackageInfo.AllName),
                new XElement("unknow2", cashShopPackageInfo.unknow2),
                new XElement("PackageName", cashShopPackageInfo.PackageName)
            );

            if (index > 0)
            {
                string cashShopPackageFilename = $"CashShopPackageInfo.xml";
                var packageCashOutput = $"XML\\CashShop\\PackageInfo{index}";
                string cashShopPackageFilePath = Path.Combine(packageCashOutput, cashShopPackageFilename);
                cashShopPackageElement.Save(cashShopPackageFilePath);

                string packageItemsPath = $"XML\\CashShop\\PackageInfo{index}\\PackageItems\\PackageItems.xml";
                ExportItemsToXml(cashShopPackageInfo.PackageItems, packageItemsPath);
            }
            else
            {
                string cashShopPackageFilename = $"CashShopPackageInfo.xml";
                var packageCashOutput = $"XML\\CashShop\\PackageInfo";
                string cashShopPackageFilePath = Path.Combine(packageCashOutput, cashShopPackageFilename);
                cashShopPackageElement.Save(cashShopPackageFilePath);

                string packageItemsPath = $"XML\\CashShop\\PackageInfo\\PackageItems\\PackageItems.xml";
                ExportItemsToXml(cashShopPackageInfo.PackageItems, packageItemsPath);

            }
        }
        private static void ExportItemsToXml(List<CashShopInformationCount> items, string outputPath)
        {
            XElement rootElement = new XElement("CashShopInformationCounts"); // Criação do elemento raiz

            foreach (var item in items)
            {
                XElement cashShopElement = new XElement("CashShopInformationCount",
                    new XElement("CashShopId", item.CashShopId),
                    new XElement("CashInfo",
                        from cashInfo in item.CashInfo
                        select new XElement("CASHINFO",
                            new XElement("CashName", cashInfo.CashName),
                            new XElement("Description", cashInfo.Description),
                            new XElement("bActive", cashInfo.bActive),
                            new XElement("dwProductID", cashInfo.dwProductID),
                            new XElement("szStartTime", cashInfo.szStartTime),
                            new XElement("szEndTime", cashInfo.szEndTime),
                            new XElement("nPurchaseCashType", cashInfo.nPurchaseCashType),
                            new XElement("nStandardSellingPrice", cashInfo.nStandardSellingPrice),
                            new XElement("nRealSellingPrice", cashInfo.nRealSellingPrice),
                            new XElement("nSalePersent", cashInfo.nSalePersent),
                            new XElement("nIconID", cashInfo.nIconID),
                            new XElement("nMaskType", cashInfo.nMaskType),
                            new XElement("nDispType", cashInfo.nDispType),
                            new XElement("nDispCount", cashInfo.nDispCount),
                            new XElement("packageItems", cashInfo.packageItems),
                            new XElement("cashshop_id", cashInfo.cashshop_id),
                            new XElement("Name", cashInfo.Name),
                            new XElement("Desc", cashInfo.Desc),
                            new XElement("Enabled", cashInfo.Enabled),
                            new XElement("Date1", cashInfo.Date1),
                            new XElement("Date2", cashInfo.Date2),
                            new XElement("unique_id", cashInfo.unique_id),
                            new XElement("CashItems",
                                from cashItem in cashInfo.CashItems
                                select new XElement("Item",
                                    new XElement("ItemId", cashItem.ItemId),
                                    new XElement("Amount", cashItem.Amount)
                                )
                            )
                        )
                    )
                );

                rootElement.Add(cashShopElement); // Adiciona o elemento do cashShop atual ao elemento raiz
            }

            // Salva todos os itens em um único arquivo XML
            rootElement.Save(outputPath);
        }
        public static void ExportCashShopMainInfoToXml(CashShopMainInformation cashShopMainInfo, int index)
        {
            XElement cashShopMainElement = new XElement("CashShopMainInformation",
                new XElement("unknow", cashShopMainInfo.unknow),
                new XElement("unknow1", cashShopMainInfo.unknow1),
                new XElement("unknow2", cashShopMainInfo.unknow2),
                new XElement("MainTitle", cashShopMainInfo.MainTitle),
                new XElement("unknow3", cashShopMainInfo.unknow3),
                new XElement("unknow4", cashShopMainInfo.unknow4),
                new XElement("MainNewTitle", cashShopMainInfo.MainNewTitle),
                new XElement("unknow5", cashShopMainInfo.unknow5),
                new XElement("MainHotTitle", cashShopMainInfo.MainHotTitle),
                new XElement("unknow6", cashShopMainInfo.unknow6),
                new XElement("MainEventTitle", cashShopMainInfo.MainEventTitle),
                new XElement("unknow7", cashShopMainInfo.unknow7),
                new XElement("UnknowItemsTitle", cashShopMainInfo.UnknowItemsTitle),
                new XElement("unknow8", cashShopMainInfo.unknow8)
            );

            foreach (var item in cashShopMainInfo.MainNewItems)
            {
                cashShopMainElement.Add(new XElement("MainNewItems", new XElement("ProductID", item.ProductID)));
            }

            foreach (var item in cashShopMainInfo.MainHotItems)
            {
                cashShopMainElement.Add(new XElement("MainHotItems", new XElement("ProductID", item.ProductID)));
            }

            foreach (var item in cashShopMainInfo.MainEventItems)
            {
                cashShopMainElement.Add(new XElement("MainEventItems", new XElement("ProductID", item.ProductID)));
            }

            foreach (var item in cashShopMainInfo.MainUnknowItems)
            {
                cashShopMainElement.Add(new XElement("MainUnknowItems", new XElement("ProductID", item.ProductID)));
            }

            if (index > 0)
            {
                var outputPath = $"XML\\CashShop\\Main{index}\\";
                string cashShopMainFilename = $"CashShopMainInformation.xml";
                string cashShopMainFilePath = Path.Combine(outputPath, cashShopMainFilename);
                cashShopMainElement.Save(cashShopMainFilePath);
            }
            else
            {
                var outputPath = $"XML\\CashShop\\Main\\";
                string cashShopMainFilename = $"CashShopMainInformation.xml";
                string cashShopMainFilePath = Path.Combine(outputPath, cashShopMainFilename);
                cashShopMainElement.Save(cashShopMainFilePath);
            }
        }
        public static void ExportCashWebDataToXml(CashWebData[] cashWebDataArray)
        {
            XElement rootElement = new XElement("CashWebDataList");

            foreach (var cashWebData in cashWebDataArray)
            {
                XElement cashWebDataElement = new XElement("CashWebData",
                    new XElement("nTableType", cashWebData.nTableType),
                    new XElement("m_mapWebData", cashWebData.m_mapWebData)
                );

                foreach (var cashWebInfo in cashWebData.CashWebInfo)
                {
                    XElement cashWebInfoElement = new XElement("CashWebDataInfo",
                        new XElement("Size", cashWebInfo.Size),
                        new XElement("sWebImageFile", cashWebInfo.sWebImageFile),
                        new XElement("Size2", cashWebInfo.Size2),
                        new XElement("sWebLinkUrl", cashWebInfo.sWebLinkUrl)
                    );

                    cashWebDataElement.Add(cashWebInfoElement);
                }

                rootElement.Add(cashWebDataElement);
            }

            string cashWebDataFilename = "XML\\CashShop\\WebData\\WebData.xml";

            rootElement.Save(cashWebDataFilename);
        }
        public static CashShopTamerInfo ImportCashTamerInfoFromXml(string filePath, int index)
        {
            XElement doc = XElement.Load(filePath);
     
            CashShopTamerInfo cashShopTamerInfo = new CashShopTamerInfo
            {
                CategoryName = doc.Element("CategoryName")?.Value,
                unknow = int.Parse(doc.Element("unknow")?.Value ?? "0"),
                unknow1 = int.Parse(doc.Element("unknow1")?.Value ?? "0"),
                AllName = doc.Element("AllName")?.Value,
                unknow2 = int.Parse(doc.Element("unknow2")?.Value ?? "0"),
                unknow3 = int.Parse(doc.Element("unknow3")?.Value ?? "0"),
                ExpasionName = doc.Element("ExpasionName")?.Value,
                unknow4 = int.Parse(doc.Element("unknow4")?.Value ?? "0"),
                ExpName = doc.Element("ExpName")?.Value,
                unknow5 = int.Parse(doc.Element("unknow5")?.Value ?? "0"),
                MovimentName = doc.Element("MovimentName")?.Value,
                unknow6 = int.Parse(doc.Element("unknow6")?.Value ?? "0"),
                ChatName = doc.Element("ChatName")?.Value,
                unknow7 = int.Parse(doc.Element("unknow7")?.Value ?? "0"),
                EtcName = doc.Element("EtcName")?.Value
            };

            if (index > 0)
            {
                string ExpansionPath = $"XML\\CashShop\\TamerInfo{index}\\Expansion\\Expansion.xml";
                string ExpPath = $"XML\\CashShop\\TamerInfo{index}\\Exp\\Exp.xml";
                string MovimentPath = $"XML\\CashShop\\TamerInfo{index}\\Moviment\\Moviment.xml";
                string ChatPath = $"XML\\CashShop\\TamerInfo{index}\\Chat\\Chat.xml";
                string EtcPath = $"XML\\CashShop\\TamerInfo{index}\\Etc\\Etc.xml";
                cashShopTamerInfo.ExpansionItems = ImportItemsFromXml(ExpansionPath);
                cashShopTamerInfo.ExpansionItems = ImportItemsFromXml(ExpPath);
                cashShopTamerInfo.MovimentItems = ImportItemsFromXml(MovimentPath);
                cashShopTamerInfo.ChatItems = ImportItemsFromXml(ChatPath);
                cashShopTamerInfo.EtcItems = ImportItemsFromXml(EtcPath);

            }
            else
            {
                string ExpansionPath = $"XML\\CashShop\\TamerInfo\\Expansion\\Expansion.xml";
                string ExpPath = $"XML\\CashShop\\TamerInfo\\Exp\\Exp.xml";
                string MovimentPath = $"XML\\CashShop\\TamerInfo\\Moviment\\Moviment.xml";
                string ChatPath = $"XML\\CashShop\\TamerInfo\\Chat\\Chat.xml";
                string EtcPath = $"XML\\CashShop\\TamerInfo\\Etc\\Etc.xml";
                cashShopTamerInfo.ExpansionItems = ImportItemsFromXml(ExpansionPath);
                cashShopTamerInfo.ExpItems = ImportItemsFromXml(ExpPath);
                cashShopTamerInfo.MovimentItems = ImportItemsFromXml(MovimentPath);
                cashShopTamerInfo.ChatItems = ImportItemsFromXml(ChatPath);
                cashShopTamerInfo.EtcItems = ImportItemsFromXml(EtcPath);
            }

            return cashShopTamerInfo;
        }

        public static CashShopDigimonInfo ImportCashDigimonInfoFromXml(string filePath, int index)
        {
            XElement cashShopDigimonElement = XElement.Load(filePath);

            CashShopDigimonInfo cashShopDigimonInfo = new CashShopDigimonInfo
            {
                CategoryNameSize = int.Parse(cashShopDigimonElement.Element("CategoryNameSize")?.Value ?? "0"),
                CategoryName = cashShopDigimonElement.Element("CategoryName")?.Value,
                unknow = int.Parse(cashShopDigimonElement.Element("unknow")?.Value ?? "0"),
                unknow1 = int.Parse(cashShopDigimonElement.Element("unknow1")?.Value ?? "0"),
                AllName = cashShopDigimonElement.Element("AllName")?.Value,
                unknow2 = int.Parse(cashShopDigimonElement.Element("unknow2")?.Value ?? "0"),
                unknow3 = int.Parse(cashShopDigimonElement.Element("unknow3")?.Value ?? "0"),
                unknow4 = int.Parse(cashShopDigimonElement.Element("unknow4")?.Value ?? "0"),
                DigiEggName = cashShopDigimonElement.Element("DigiEggName")?.Value,
                unknow5 = int.Parse(cashShopDigimonElement.Element("unknow5")?.Value ?? "0"),
                EvolutionName = cashShopDigimonElement.Element("EvolutionName")?.Value,
                unknow6 = int.Parse(cashShopDigimonElement.Element("unknow6")?.Value ?? "0"),
                HatchName = cashShopDigimonElement.Element("HatchName")?.Value,
                unknow7 = int.Parse(cashShopDigimonElement.Element("unknow7")?.Value ?? "0"),
                ReinforcedName = cashShopDigimonElement.Element("ReinforcedName")?.Value,
                unknow8 = int.Parse(cashShopDigimonElement.Element("unknow8")?.Value ?? "0"),
                RidingName = cashShopDigimonElement.Element("RidingName")?.Value,
                unknow9 = int.Parse(cashShopDigimonElement.Element("unknow9")?.Value ?? "0"),
                EtcName = cashShopDigimonElement.Element("EtcName")?.Value
            };


            if (index > 0)
            {
                string cashShopDigimonFilename = $"CashShopDigimonInfo{index}.xml";
                var digimonCashOutput = $"XML\\CashShop\\DigimonInfo{index}";
                string cashShopDigimonFilePath = Path.Combine(digimonCashOutput, cashShopDigimonFilename);
                cashShopDigimonElement.Save(cashShopDigimonFilePath);

                string expansionPath = $"XML\\CashShop\\DigimonInfo{index}\\Expansion\\Expansion.xml";
                string digiEggPath = $"XML\\CashShop\\DigimonInfo{index}\\DigiEgg\\DigiEgg.xml";
                string evolutionPath = $"XML\\CashShop\\DigimonInfo{index}\\Evolution\\Evolution.xml";
                string hatchPath = $"XML\\CashShop\\DigimonInfo{index}\\Hatch\\Hatch.xml";
                string reinforcedPath = $"XML\\CashShop\\DigimonInfo{index}\\Reinforced\\Reinforced.xml";
                string ridingPath = $"XML\\CashShop\\DigimonInfo{index}\\Riding\\Riding.xml";
                string etcPath = $"XML\\CashShop\\DigimonInfo{index}\\Etc\\Etc.xml";

                cashShopDigimonInfo.DigiEgg = ImportItemsFromXml(digiEggPath);
                cashShopDigimonInfo.EvolutionItems = ImportItemsFromXml(evolutionPath);
                cashShopDigimonInfo.HatchItems = ImportItemsFromXml(hatchPath);
                cashShopDigimonInfo.ReinforcedItems = ImportItemsFromXml(reinforcedPath);
                cashShopDigimonInfo.RidingItems = ImportItemsFromXml(ridingPath);
                cashShopDigimonInfo.EtcItems = ImportItemsFromXml(etcPath);
            }
            else
            {
                string cashShopDigimonFilename = "CashShopDigimonInfo.xml";
                var digimonCashOutput = $"XML\\CashShop\\DigimonInfo";
                string cashShopDigimonFilePath = Path.Combine(digimonCashOutput, cashShopDigimonFilename);
                cashShopDigimonElement.Save(cashShopDigimonFilePath);

                string expansionPath = $"XML\\CashShop\\DigimonInfo\\Expansion\\Expansion.xml";
                string digiEggPath = $"XML\\CashShop\\DigimonInfo\\DigiEgg\\DigiEgg.xml";
                string evolutionPath = $"XML\\CashShop\\DigimonInfo\\Evolution\\Evolution.xml";
                string hatchPath = $"XML\\CashShop\\DigimonInfo\\Hatch\\Hatch.xml";
                string reinforcedPath = $"XML\\CashShop\\DigimonInfo\\Reinforced\\Reinforced.xml";
                string ridingPath = $"XML\\CashShop\\DigimonInfo\\Riding\\Riding.xml";
                string etcPath = $"XML\\CashShop\\DigimonInfo\\Etc\\Etc.xml";


                cashShopDigimonInfo.DigiEgg = ImportItemsFromXml(digiEggPath);
                cashShopDigimonInfo.EvolutionItems = ImportItemsFromXml(evolutionPath);
                cashShopDigimonInfo.HatchItems = ImportItemsFromXml(hatchPath);
                cashShopDigimonInfo.ReinforcedItems = ImportItemsFromXml(reinforcedPath);
                cashShopDigimonInfo.RidingItems = ImportItemsFromXml(ridingPath);
                cashShopDigimonInfo.EtcItems = ImportItemsFromXml(etcPath);

            }

            return cashShopDigimonInfo;
        }

        public static CashShopAvatarInfo ImportCashAvatarInfoFromXml(string filePath, int index)
        {
            XElement cashShopAvatarElement = XElement.Load(filePath);

            CashShopAvatarInfo cashShopAvatarInfo = new CashShopAvatarInfo
            {
                CategoryNameSize = int.Parse(cashShopAvatarElement.Element("CategoryNameSize")?.Value ?? "0"),
                CategoryName = cashShopAvatarElement.Element("CategoryName")?.Value,
                unknow = int.Parse(cashShopAvatarElement.Element("unknow")?.Value ?? "0"),
                unknow1 = int.Parse(cashShopAvatarElement.Element("unknow1")?.Value ?? "0"),
                AllName = cashShopAvatarElement.Element("AllName")?.Value,
                unknow2 = int.Parse(cashShopAvatarElement.Element("unknow2")?.Value ?? "0"),
                unknow3 = int.Parse(cashShopAvatarElement.Element("unknow3")?.Value ?? "0"),
                ReinforcedName = cashShopAvatarElement.Element("ReinforcedName")?.Value,
                unknow4 = int.Parse(cashShopAvatarElement.Element("unknow4")?.Value ?? "0"),
                unknow5 = int.Parse(cashShopAvatarElement.Element("unknow5")?.Value ?? "0"),
                HeadName = cashShopAvatarElement.Element("HeadName")?.Value,
                unknow6 = int.Parse(cashShopAvatarElement.Element("unknow6")?.Value ?? "0"),
                TopName = cashShopAvatarElement.Element("TopName")?.Value,
                unknow7 = int.Parse(cashShopAvatarElement.Element("unknow7")?.Value ?? "0"),
                BottomName = cashShopAvatarElement.Element("BottomName")?.Value,
                unknow8 = int.Parse(cashShopAvatarElement.Element("unknow8")?.Value ?? "0"),
                GlovesName = cashShopAvatarElement.Element("GlovesName")?.Value,
                unknow9 = int.Parse(cashShopAvatarElement.Element("unknow9")?.Value ?? "0"),
                ShoesName = cashShopAvatarElement.Element("ShoesName")?.Value,
                unknow10 = int.Parse(cashShopAvatarElement.Element("unknow10")?.Value ?? "0"),
                FashionName = cashShopAvatarElement.Element("FashionName")?.Value,
                unknow11 = int.Parse(cashShopAvatarElement.Element("unknow11")?.Value ?? "0"),
                CostumeName = cashShopAvatarElement.Element("CostumeName")?.Value
            };

            if (index > 0)
            {
                string cashShopAvatarFilename = $"CashShopAvatarInfo{index}.xml";
                var avatarCashOutput = $"XML\\CashShop\\AvatarInfo{index}";
                string cashShopAvatarFilePath = Path.Combine(avatarCashOutput, cashShopAvatarFilename);
                cashShopAvatarElement.Save(cashShopAvatarFilePath);

                string reinforcedPath = $"XML\\CashShop\\AvatarInfo{index}\\Reinforced\\Reinforced.xml";
                string digiEggPath = $"XML\\CashShop\\AvatarInfo{index}\\DigiEgg\\DigiEgg.xml";
                string headPath = $"XML\\CashShop\\AvatarInfo{index}\\Head\\Head.xml";
                string topPath = $"XML\\CashShop\\AvatarInfo{index}\\Top\\Top.xml";
                string bottomPath = $"XML\\CashShop\\AvatarInfo{index}\\Bottom\\Bottom.xml";
                string glovesPath = $"XML\\CashShop\\AvatarInfo{index}\\Gloves\\Gloves.xml";
                string shoesPath = $"XML\\CashShop\\AvatarInfo{index}\\Shoes\\Shoes.xml";
                string fashionPath = $"XML\\CashShop\\AvatarInfo{index}\\Fashion\\Fashion.xml";
                string costumePath = $"XML\\CashShop\\AvatarInfo{index}\\Costume\\Costume.xml";

                cashShopAvatarInfo.ReinforcedItems = ImportItemsFromXml(reinforcedPath);
                cashShopAvatarInfo.HeadItems = ImportItemsFromXml(headPath);
                cashShopAvatarInfo.TopItems = ImportItemsFromXml(topPath);
                cashShopAvatarInfo.BottomItems = ImportItemsFromXml(bottomPath);
                cashShopAvatarInfo.GlovesItems = ImportItemsFromXml(glovesPath);
                cashShopAvatarInfo.ShoesItems = ImportItemsFromXml(shoesPath);
                cashShopAvatarInfo.FashionItems = ImportItemsFromXml(fashionPath);
                cashShopAvatarInfo.CostumeItems = ImportItemsFromXml(costumePath);
            }
            else
            {
                string cashShopAvatarFilename = $"CashShopAvatarInfo.xml";
                var avatarCashOutput = $"XML\\CashShop\\AvatarInfo";
                string cashShopAvatarFilePath = Path.Combine(avatarCashOutput, cashShopAvatarFilename);
                cashShopAvatarElement.Save(cashShopAvatarFilePath);

                string reinforcedPath = $"XML\\CashShop\\AvatarInfo\\Reinforced\\Reinforced.xml";
                string digiEggPath = $"XML\\CashShop\\AvatarInfo\\DigiEgg\\DigiEgg.xml";
                string headPath = $"XML\\CashShop\\AvatarInfo\\Head\\Head.xml";
                string topPath = $"XML\\CashShop\\AvatarInfo\\Top\\Top.xml";
                string bottomPath = $"XML\\CashShop\\AvatarInfo\\Bottom\\Bottom.xml";
                string glovesPath = $"XML\\CashShop\\AvatarInfo\\Gloves\\Gloves.xml";
                string shoesPath = $"XML\\CashShop\\AvatarInfo\\Shoes\\Shoes.xml";
                string fashionPath = $"XML\\CashShop\\AvatarInfo\\Fashion\\Fashion.xml";
                string costumePath = $"XML\\CashShop\\AvatarInfo\\Costume\\Costume.xml";

                cashShopAvatarInfo.ReinforcedItems = ImportItemsFromXml(reinforcedPath);
                cashShopAvatarInfo.HeadItems = ImportItemsFromXml(headPath);
                cashShopAvatarInfo.TopItems = ImportItemsFromXml(topPath);
                cashShopAvatarInfo.BottomItems = ImportItemsFromXml(bottomPath);
                cashShopAvatarInfo.GlovesItems = ImportItemsFromXml(glovesPath);
                cashShopAvatarInfo.ShoesItems = ImportItemsFromXml(shoesPath);
                cashShopAvatarInfo.FashionItems = ImportItemsFromXml(fashionPath);
                cashShopAvatarInfo.CostumeItems = ImportItemsFromXml(costumePath);

            }

            return cashShopAvatarInfo;
        }

        public static CashShopPackageInfo ImportCashPackageInfoFromXml(string filePath, int index)
        {
            XElement cashShopPackageElement = XElement.Load(filePath);

            CashShopPackageInfo cashShopPackageInfo = new CashShopPackageInfo
            {
                CategoryNameSize = int.Parse(cashShopPackageElement.Element("CategoryNameSize")?.Value ?? "0"),
                CategoryName = cashShopPackageElement.Element("CategoryName")?.Value,
                unknow = int.Parse(cashShopPackageElement.Element("unknow")?.Value ?? "0"),
                unknow1 = int.Parse(cashShopPackageElement.Element("unknow1")?.Value ?? "0"),
                AllName = cashShopPackageElement.Element("AllName")?.Value,
                unknow2 = int.Parse(cashShopPackageElement.Element("unknow2")?.Value ?? "0"),
                PackageName = cashShopPackageElement.Element("PackageName")?.Value
            };

            if (index > 0)
            {
                string packageItemsPath = $"XML\\CashShop\\PackageInfo{index}\\PackageItems\\PackageItems.xml";
                cashShopPackageInfo.PackageItems = ImportItemsFromXml(packageItemsPath);
            }
            else
            {
                string packageItemsPath = $"XML\\CashShop\\PackageInfo\\PackageItems\\PackageItems.xml";
                cashShopPackageInfo.PackageItems = ImportItemsFromXml(packageItemsPath);

            }
            return cashShopPackageInfo;
        }

        private static List<CashShopInformationCount> ImportItemsFromXml(string filePath)
        {
            XDocument doc = XDocument.Load(filePath);
            var rootElement = doc.Root;

            List<CashShopInformationCount> items = new List<CashShopInformationCount>();

            foreach (var cashShopElement in rootElement.Elements("CashShopInformationCount"))
            {
                CashShopInformationCount item = new CashShopInformationCount
                {
                    CashShopId = int.Parse(cashShopElement.Element("CashShopId")?.Value ?? "0"),
                    CashInfo = new List<CASHINFO>()
                };

                foreach (var cashInfoElement in cashShopElement.Elements("CashInfo").Elements("CASHINFO"))
                {
                    CASHINFO cashInfo = new CASHINFO
                    {
                        CashName = cashInfoElement.Element("CashName")?.Value,
                        Description = cashInfoElement.Element("Description")?.Value,
                        bActive = byte.TryParse(cashInfoElement.Element("bActive")?.Value, out byte bActiveValue) ? bActiveValue : (byte)0,
                        dwProductID = int.Parse(cashInfoElement.Element("dwProductID")?.Value ?? "0"),
                        szStartTime = cashInfoElement.Element("szStartTime")?.Value,
                        szEndTime = cashInfoElement.Element("szEndTime")?.Value,
                        nPurchaseCashType = int.Parse(cashInfoElement.Element("nPurchaseCashType")?.Value ?? "0"),
                        nStandardSellingPrice = int.Parse(cashInfoElement.Element("nStandardSellingPrice")?.Value ?? "0"),
                        nRealSellingPrice = int.Parse(cashInfoElement.Element("nRealSellingPrice")?.Value ?? "0"),
                        nSalePersent = int.TryParse(cashInfoElement.Element("nSalePersent")?.Value, out int nSalePersentValue) ? nSalePersentValue : 0,
                        nIconID = int.Parse(cashInfoElement.Element("nIconID")?.Value ?? "0"),
                        nMaskType = int.TryParse(cashInfoElement.Element("nMaskType")?.Value, out int nMaskTypeValue) ? nMaskTypeValue : 0,
                        nDispType = int.Parse(cashInfoElement.Element("nDispType")?.Value ?? "0"),
                        nDispCount = int.Parse(cashInfoElement.Element("nDispCount")?.Value ?? "0"),
                        packageItems = int.Parse(cashInfoElement.Element("packageItems")?.Value ?? "0"),
                        cashshop_id = int.Parse(cashInfoElement.Element("cashshop_id")?.Value ?? "0"),
                        Name = cashInfoElement.Element("Name")?.Value,
                        Desc = cashInfoElement.Element("Desc")?.Value,
                        Enabled = byte.TryParse(cashInfoElement.Element("Enabled")?.Value, out byte enabledValue) ? enabledValue : (byte)0,
                        Date1 = cashInfoElement.Element("Date1")?.Value,
                        Date2 = cashInfoElement.Element("Date2")?.Value,
                        unique_id = int.Parse(cashInfoElement.Element("unique_id")?.Value ?? "0"),
                        CashItems = new List<Item>()
                    };

                    XElement cashItemsElement = cashInfoElement.Element("CashItems");
                    if (cashItemsElement != null)
                    {
                        foreach (var cashItemElement in cashItemsElement.Elements("Item"))
                        {
                            Item cashItem = new Item
                            {
                                ItemId = int.Parse(cashItemElement.Element("ItemId")?.Value ?? "0"),
                                Amount = int.Parse(cashItemElement.Element("Amount")?.Value ?? "0")
                            };

                            cashInfo.CashItems.Add(cashItem);
                        }
                    }
                    else
                    {
                        Console.WriteLine("CashItems element not found or is null.");
                    }

                    item.CashInfo.Add(cashInfo);
                }

                items.Add(item);
            }

            return items;
        }

        public static CashShopMainInformation ImportCashShopMainInfoFromXml(string filePath)
        {
            XDocument doc = XDocument.Load(filePath);
            var rootElement = doc.Root;

            CashShopMainInformation cashShopMainInfo = new CashShopMainInformation
            {
                unknow = int.Parse(rootElement.Element("unknow")?.Value ?? "0"),
                unknow1 = int.Parse(rootElement.Element("unknow1")?.Value ?? "0"),
                unknow2 = int.Parse(rootElement.Element("unknow2")?.Value ?? "0"),
                MainTitle = rootElement.Element("MainTitle")?.Value,
                unknow3 = int.Parse(rootElement.Element("unknow3")?.Value ?? "0"),
                unknow4 = int.Parse(rootElement.Element("unknow4")?.Value ?? "0"),
                MainNewTitle = rootElement.Element("MainNewTitle")?.Value,
                unknow5 = int.Parse(rootElement.Element("unknow5")?.Value ?? "0"),
                MainHotTitle = rootElement.Element("MainHotTitle")?.Value,
                unknow6 = int.Parse(rootElement.Element("unknow6")?.Value ?? "0"),
                MainEventTitle = rootElement.Element("MainEventTitle")?.Value,
                unknow7 = int.Parse(rootElement.Element("unknow7")?.Value ?? "0"),
                UnknowItemsTitle = rootElement.Element("UnknowItemsTitle")?.Value,
                unknow8 = int.Parse(rootElement.Element("unknow8")?.Value ?? "0"),
                MainNewItems = new List<MainCashShopInfo>(),
                MainHotItems = new List<MainCashShopInfo>(),
                MainEventItems = new List<MainCashShopInfo>(),
                MainUnknowItems = new List<MainCashShopInfo>()
            };

            foreach (var itemElement in rootElement.Elements("MainNewItems").Elements("ProductID"))
            {
                cashShopMainInfo.MainNewItems.Add(new MainCashShopInfo { ProductID = int.Parse(itemElement?.Value ?? "0") });
            }

            foreach (var itemElement in rootElement.Elements("MainHotItems").Elements("ProductID"))
            {
                cashShopMainInfo.MainHotItems.Add(new MainCashShopInfo { ProductID = int.Parse(itemElement?.Value ?? "0") });
            }

            foreach (var itemElement in rootElement.Elements("MainEventItems").Elements("ProductID"))
            {
                cashShopMainInfo.MainEventItems.Add(new MainCashShopInfo { ProductID = int.Parse(itemElement?.Value ?? "0") });
            }

            foreach (var itemElement in rootElement.Elements("MainUnknowItems").Elements("ProductID"))
            {
                cashShopMainInfo.MainUnknowItems.Add(new MainCashShopInfo { ProductID = int.Parse(itemElement?.Value ?? "0") });
            }

            return cashShopMainInfo;
        }
        public static CashWebData[] ImportCashWebDataFromXml(string filePath)
        {
            List<CashWebData> cashWebDataList = new List<CashWebData>();

            XDocument doc = XDocument.Load(filePath);
            var rootElement = doc.Root;

            foreach (var cashWebDataElement in rootElement.Elements("CashWebData"))
            {
                CashWebData cashWebData = new CashWebData
                {
                    nTableType = int.Parse(cashWebDataElement.Element("nTableType")?.Value ?? "0"),
                    m_mapWebData = int.Parse(cashWebDataElement.Element("m_mapWebData")?.Value ?? "0"),
                    CashWebInfo = new List<CashWebDataInfo>()
                };

                foreach (var cashWebInfoElement in cashWebDataElement.Elements("CashWebDataInfo"))
                {
                    CashWebDataInfo cashWebDataInfo = new CashWebDataInfo
                    {
                        Size = int.Parse(cashWebInfoElement.Element("Size")?.Value ?? "0"),
                        sWebImageFile = cashWebInfoElement.Element("sWebImageFile")?.Value,
                        Size2 = int.Parse(cashWebInfoElement.Element("Size2")?.Value ?? "0"),
                        sWebLinkUrl = cashWebInfoElement.Element("sWebLinkUrl")?.Value
                    };

                    cashWebData.CashWebInfo.Add(cashWebDataInfo);
                }

                cashWebDataList.Add(cashWebData);
            }

            return cashWebDataList.ToArray();
        }

        public static void WriteCashShopToBinary(string filePath, CashShopMainInformation[] cashShopMainInformation, CashShopTamerInfo[] cashShopTamerInfo, CashShopDigimonInfo[] cashShopDigimonInfo, CashShopAvatarInfo[] cashShopAvatarInfo, CashShopPackageInfo[] cashShopPackageInfo, CashWebData[] cashWebData)
        {

            using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {
                for (int i = 0; i < 2; i++)
                {
                    WriteMainCashShopToBinary(writer, cashShopMainInformation[i], i);
                    WriteCashShopTamerInfo(writer, cashShopTamerInfo[i]);
                    WriteCashShopDigimonInfo(writer, cashShopDigimonInfo[i]);
                    WriteCashShopAvatarInfo(writer, cashShopAvatarInfo[i]);
                    WriterCashShopPackageInfo(writer, cashShopPackageInfo[i]);
                }

                WriteCashWebData(writer, cashWebData);
            }

        }

        private static void WriteMainCashShopToBinary(BinaryWriter writer, CashShopMainInformation Main, int index)
        {
            if (index == 0)
            {
                writer.Write(2);
            }
            writer.Write(Main.unknow);
            writer.Write(Main.unknow1);
            writer.Write(Main.unknow2);
            writer.Write(Main.MainTitle.Length);

            for (int i = 0; i < Main.MainTitle.Length; i++)
            {
                char c = i < Main.MainTitle.Length ? Main.MainTitle[i] : '\0';
                writer.Write((ushort)c);
            }

            writer.Write(Main.unknow3);
            writer.Write(Main.unknow4);
            writer.Write(Main.MainNewTitle.Length);

            for (int i = 0; i < Main.MainNewTitle.Length; i++)
            {
                char c = i < Main.MainNewTitle.Length ? Main.MainNewTitle[i] : '\0';
                writer.Write((ushort)c);
            }

            writer.Write(Main.MainNewItems.Count);
            foreach (var MainItem in Main.MainNewItems)
            {
                writer.Write(MainItem.ProductID);
            }

            writer.Write(Main.unknow5);

            writer.Write(Main.MainHotTitle.Length);

            for (int i = 0; i < Main.MainHotTitle.Length; i++)
            {
                char c = i < Main.MainHotTitle.Length ? Main.MainHotTitle[i] : '\0';
                writer.Write((ushort)c);
            }

            writer.Write(Main.MainHotItems.Count);
            foreach (var MainItem in Main.MainHotItems)
            {
                writer.Write(MainItem.ProductID);
            }

            writer.Write(Main.unknow6);

            writer.Write(Main.MainEventTitle.Length);
            for (int i = 0; i < Main.MainEventTitle.Length; i++)
            {
                char c = i < Main.MainEventTitle.Length ? Main.MainEventTitle[i] : '\0';
                writer.Write((ushort)c);
            }

            writer.Write(Main.MainEventItems.Count);
            foreach (var MainItem in Main.MainEventItems)
            {
                writer.Write(MainItem.ProductID);
            }


            writer.Write(Main.unknow7);

            writer.Write(Main.MainUnknowItems.Count);
            foreach (var MainItem in Main.MainUnknowItems)
            {
                writer.Write(MainItem.ProductID);
            }


            writer.Write(Main.unknow8);
        }
        private static void WriteCashShopTamerInfo(BinaryWriter writer, CashShopTamerInfo CashInfo)
        {

           
            writer.Write(CashInfo.CategoryName.Length);
            for (int i = 0; i < CashInfo.CategoryName.Length; i++)
            {
                char c = i < CashInfo.CategoryName.Length ? CashInfo.CategoryName[i] : '\0';
                writer.Write((ushort)c);
            }


            writer.Write(CashInfo.unknow);
            writer.Write(CashInfo.unknow1);


            writer.Write(CashInfo.AllName.Length);

            for (int i = 0; i < CashInfo.AllName.Length; i++)
            {
                char c = i < CashInfo.AllName.Length ? CashInfo.AllName[i] : '\0';
                writer.Write((ushort)c);
            }


            writer.Write(CashInfo.unknow2);
            writer.Write(CashInfo.unknow3);

            writer.Write(CashInfo.ExpasionName.Length);

            for (int i = 0; i < CashInfo.ExpasionName.Length; i++)
            {
                char c = i < CashInfo.ExpasionName.Length ? CashInfo.ExpasionName[i] : '\0';
                writer.Write((ushort)c);
            }


            #region [ExpansionItems]          
            writer.Write(CashInfo.ExpansionItems.Count);
            WriteMainInformation(writer, CashInfo.ExpansionItems);
            #endregion

            writer.Write(CashInfo.unknow4);

            writer.Write(CashInfo.ExpName.Length);
            for (int i = 0; i < CashInfo.ExpName.Length; i++)
            {
                char c = i < CashInfo.ExpName.Length ? CashInfo.ExpName[i] : '\0';
                writer.Write((ushort)c);
            }

            #region [ExpItems]
            writer.Write(CashInfo.ExpItems.Count);
            WriteMainInformation(writer, CashInfo.ExpItems);
            #endregion

            writer.Write(CashInfo.unknow5);

            writer.Write(CashInfo.MovimentName.Length);
            for (int i = 0; i < CashInfo.MovimentName.Length; i++)
            {
                char c = i < CashInfo.MovimentName.Length ? CashInfo.MovimentName[i] : '\0';
                writer.Write((ushort)c);
            }

            #region [MovimentItems]
            writer.Write(CashInfo.MovimentItems.Count);
            WriteMainInformation(writer, CashInfo.MovimentItems);
            #endregion

            writer.Write(CashInfo.unknow6);

            writer.Write(CashInfo.ChatName.Length);
            for (int i = 0; i < CashInfo.ChatName.Length; i++)
            {
                char c = i < CashInfo.ChatName.Length ? CashInfo.ChatName[i] : '\0';
                writer.Write((ushort)c);
            }

            #region [ChatShopSizeItems]
            writer.Write(CashInfo.ChatItems.Count);
            WriteMainInformation(writer, CashInfo.ChatItems);
            #endregion

            writer.Write(CashInfo.unknow7);
            writer.Write(CashInfo.EtcName.Length);
            for (int i = 0; i < CashInfo.EtcName.Length; i++)
            {
                char c = i < CashInfo.EtcName.Length ? CashInfo.EtcName[i] : '\0';
                writer.Write((ushort)c);
            }

            #region [EtcItems]
            writer.Write(CashInfo.EtcItems.Count);
            WriteMainInformation(writer, CashInfo.EtcItems);
            #endregion

        }
        private static void WriteCashShopDigimonInfo(BinaryWriter writer, CashShopDigimonInfo DigiInfo)
        {


            writer.Write(DigiInfo.unknow);
            writer.Write(DigiInfo.CategoryName.Length);
            for (int i = 0; i < DigiInfo.CategoryName.Length; i++)
            {
                char c = i < DigiInfo.CategoryName.Length ? DigiInfo.CategoryName[i] : '\0';
                writer.Write((ushort)c);
            }

            writer.Write(DigiInfo.unknow1);
            writer.Write(DigiInfo.unknow2);


            writer.Write(DigiInfo.AllName.Length);

            for (int i = 0; i < DigiInfo.AllName.Length; i++)
            {
                char c = i < DigiInfo.AllName.Length ? DigiInfo.AllName[i] : '\0';
                writer.Write((ushort)c);
            }

            writer.Write(DigiInfo.unknow3);
            writer.Write(DigiInfo.unknow4);

            writer.Write(DigiInfo.DigiEggName.Length);

            for (int i = 0; i < DigiInfo.DigiEggName.Length; i++)
            {
                char c = i < DigiInfo.DigiEggName.Length ? DigiInfo.DigiEggName[i] : '\0';
                writer.Write((ushort)c);
            }

            #region [DigiEggItems]
            writer.Write(DigiInfo.DigiEgg.Count);
            WriteMainInformation(writer, DigiInfo.DigiEgg);
            #endregion

            writer.Write(DigiInfo.unknow5);

            writer.Write(DigiInfo.EvolutionName.Length);

            for (int i = 0; i < DigiInfo.EvolutionName.Length; i++)
            {
                char c = i < DigiInfo.EvolutionName.Length ? DigiInfo.EvolutionName[i] : '\0';
                writer.Write((ushort)c);
            }

            #region [EvolutionItems]
            writer.Write(DigiInfo.EvolutionItems.Count);
            WriteMainInformation(writer, DigiInfo.EvolutionItems);
            #endregion

            writer.Write(DigiInfo.unknow6);

            writer.Write(DigiInfo.HatchName.Length);

            for (int i = 0; i < DigiInfo.HatchName.Length; i++)
            {
                char c = i < DigiInfo.HatchName.Length ? DigiInfo.HatchName[i] : '\0';
                writer.Write((ushort)c);
            }


            #region [Hatch/Trans Items]
            writer.Write(DigiInfo.HatchItems.Count);
            WriteMainInformation(writer, DigiInfo.HatchItems);
            #endregion

            writer.Write(DigiInfo.unknow7);

            writer.Write(DigiInfo.ReinforcedName.Length);

            for (int i = 0; i < DigiInfo.ReinforcedName.Length; i++)
            {
                char c = i < DigiInfo.ReinforcedName.Length ? DigiInfo.ReinforcedName[i] : '\0';
                writer.Write((ushort)c);
            }

            #region [ReinforceItems]
            writer.Write(DigiInfo.ReinforcedItems.Count);
            WriteMainInformation(writer, DigiInfo.ReinforcedItems);
            #endregion

            writer.Write(DigiInfo.unknow8);

            writer.Write(DigiInfo.RidingName.Length);

            for (int i = 0; i < DigiInfo.RidingName.Length; i++)
            {
                char c = i < DigiInfo.RidingName.Length ? DigiInfo.RidingName[i] : '\0';
                writer.Write((ushort)c);
            }


            #region [Riding Items]
            writer.Write(DigiInfo.RidingItems.Count);
            WriteMainInformation(writer, DigiInfo.RidingItems);
            #endregion

            writer.Write(DigiInfo.unknow9);

            writer.Write(DigiInfo.EtcName.Length);

            for (int i = 0; i < DigiInfo.EtcName.Length; i++)
            {
                char c = i < DigiInfo.EtcName.Length ? DigiInfo.EtcName[i] : '\0';
                writer.Write((ushort)c);
            }

            #region [EtcItems]
            writer.Write(DigiInfo.EtcItems.Count);
            WriteMainInformation(writer, DigiInfo.EtcItems);
            #endregion

        }
        private static void WriteCashShopAvatarInfo(BinaryWriter writer, CashShopAvatarInfo AvatarInfo)
        {

            writer.Write(AvatarInfo.unknow);
            writer.Write(AvatarInfo.CategoryName.Length);
            for (int i = 0; i < AvatarInfo.CategoryName.Length; i++)
            {
                char c = i < AvatarInfo.CategoryName.Length ? AvatarInfo.CategoryName[i] : '\0';
                writer.Write((ushort)c);
            }

            writer.Write(AvatarInfo.unknow1);
            writer.Write(AvatarInfo.unknow2);


            writer.Write(AvatarInfo.AllName.Length);

            for (int i = 0; i < AvatarInfo.AllName.Length; i++)
            {
                char c = i < AvatarInfo.AllName.Length ? AvatarInfo.AllName[i] : '\0';
                writer.Write((ushort)c);
            }

            writer.Write(AvatarInfo.unknow3);
            writer.Write(AvatarInfo.unknow4);

            writer.Write(AvatarInfo.ReinforcedName.Length);

            for (int i = 0; i < AvatarInfo.ReinforcedName.Length; i++)
            {
                char c = i < AvatarInfo.ReinforcedName.Length ? AvatarInfo.ReinforcedName[i] : '\0';
                writer.Write((ushort)c);
            }

            #region [Reinforce Items]
            writer.Write(AvatarInfo.ReinforcedItems.Count);
            WriteMainInformation(writer, AvatarInfo.ReinforcedItems);
            #endregion

            writer.Write(AvatarInfo.unknow5);

            writer.Write(AvatarInfo.HeadName.Length);

            for (int i = 0; i < AvatarInfo.HeadName.Length; i++)
            {
                char c = i < AvatarInfo.HeadName.Length ? AvatarInfo.HeadName[i] : '\0';
                writer.Write((ushort)c);
            }

            #region [Head Items]
            writer.Write(AvatarInfo.HeadItems.Count);
            WriteMainInformation(writer, AvatarInfo.HeadItems);
            #endregion

            writer.Write(AvatarInfo.unknow6);

            writer.Write(AvatarInfo.TopName.Length);

            for (int i = 0; i < AvatarInfo.TopName.Length; i++)
            {
                char c = i < AvatarInfo.TopName.Length ? AvatarInfo.TopName[i] : '\0';
                writer.Write((ushort)c);
            }

            #region [Top Items]
            writer.Write(AvatarInfo.TopItems.Count);
            WriteMainInformation(writer, AvatarInfo.TopItems);
            #endregion

            writer.Write(AvatarInfo.unknow7);

            writer.Write(AvatarInfo.BottomName.Length);

            for (int i = 0; i < AvatarInfo.BottomName.Length; i++)
            {
                char c = i < AvatarInfo.BottomName.Length ? AvatarInfo.BottomName[i] : '\0';
                writer.Write((ushort)c);
            }


            #region [Bottom Items]
            writer.Write(AvatarInfo.BottomItems.Count);
            WriteMainInformation(writer, AvatarInfo.BottomItems);
            #endregion

            writer.Write(AvatarInfo.unknow8);
            writer.Write(AvatarInfo.GlovesName.Length);

            for (int i = 0; i < AvatarInfo.GlovesName.Length; i++)
            {
                char c = i < AvatarInfo.GlovesName.Length ? AvatarInfo.GlovesName[i] : '\0';
                writer.Write((ushort)c);
            }

            #region [Gloves Items]
            writer.Write(AvatarInfo.GlovesItems.Count);
            WriteMainInformation(writer, AvatarInfo.GlovesItems);
            #endregion

            writer.Write(AvatarInfo.unknow9);
            writer.Write(AvatarInfo.ShoesName.Length);

            for (int i = 0; i < AvatarInfo.ShoesName.Length; i++)
            {
                char c = i < AvatarInfo.ShoesName.Length ? AvatarInfo.ShoesName[i] : '\0';
                writer.Write((ushort)c);
            }

            #region [Shoes Items]
            writer.Write(AvatarInfo.ShoesItems.Count);
            WriteMainInformation(writer, AvatarInfo.ShoesItems);
            #endregion

            writer.Write(AvatarInfo.unknow10);
            writer.Write(AvatarInfo.FashionName.Length);

            for (int i = 0; i < AvatarInfo.FashionName.Length; i++)
            {
                char c = i < AvatarInfo.FashionName.Length ? AvatarInfo.FashionName[i] : '\0';
                writer.Write((ushort)c);
            }

            #region [Fashion Items]    
            writer.Write(AvatarInfo.FashionItems.Count);
            WriteMainInformation(writer, AvatarInfo.FashionItems);
            #endregion

            writer.Write(AvatarInfo.unknow11);
            writer.Write(AvatarInfo.CostumeName.Length);

            for (int i = 0; i < AvatarInfo.CostumeName.Length; i++)
            {
                char c = i < AvatarInfo.CostumeName.Length ? AvatarInfo.CostumeName[i] : '\0';
                writer.Write((ushort)c);
            }


            #region [Costume Items]  
            writer.Write(AvatarInfo.CostumeItems.Count);
            WriteMainInformation(writer, AvatarInfo.CostumeItems);
            #endregion

        }
        private static void WriterCashShopPackageInfo(BinaryWriter writer, CashShopPackageInfo PackageInfo)
        {


            writer.Write(PackageInfo.unknow);
            writer.Write(PackageInfo.CategoryName.Length);
            for (int i = 0; i < PackageInfo.CategoryName.Length; i++)
            {
                char c = i < PackageInfo.CategoryName.Length ? PackageInfo.CategoryName[i] : '\0';
                writer.Write((ushort)c);
            }

            writer.Write(PackageInfo.unknow1);
            writer.Write(PackageInfo.unknow2);

            writer.Write(PackageInfo.AllName.Length);
            for (int i = 0; i < PackageInfo.AllName.Length; i++)
            {
                char c = i < PackageInfo.AllName.Length ? PackageInfo.AllName[i] : '\0';
                writer.Write((ushort)c);

            }

            #region [Package Items]
            writer.Write(PackageInfo.PackageItems.Count);
            WriteMainInformation(writer, PackageInfo.PackageItems);
            #endregion


        }
        private static void WriteMainInformation(BinaryWriter writer, List<CashShopInformationCount> cashShopInformationCount)
        {
            foreach (var CashInfo in cashShopInformationCount)
            {
                writer.Write(CashInfo.CashShopId);
                writer.Write(CashInfo.CashInfo.Count);
                foreach (var s in CashInfo.CashInfo)
                {
                    writer.Write(s.Name.Length);

                    for (int i = 0; i < s.Name.Length; i++)
                    {
                        char c = i < s.Name.Length ? s.Name[i] : '\0';
                        writer.Write((ushort)c);
                    }

                    writer.Write(s.Description.Length);

                    for (int i = 0; i < s.Description.Length; i++)
                    {
                        char c = i < s.Description.Length ? s.Description[i] : '\0';
                        writer.Write((ushort)c);
                    }

                  

                    writer.Write((byte)s.Enabled);
                    writer.Write(s.unique_id);

                    char[] Date1 = s.Date1.PadRight(64, '\0').ToCharArray();

                    // Converte o array de caracteres s_cModel_Effect para bytes no formato UTF-8
                    byte[] Date1Bytes = Encoding.ASCII.GetBytes(Date1);
                    writer.Write(Date1Bytes, 0, 64);

                    char[] Date2 = s.Date2.PadRight(64, '\0').ToCharArray();

                    // Converte o array de caracteres s_cModel_Effect para bytes no formato UTF-8
                    byte[] Date2Bytes = Encoding.ASCII.GetBytes(Date2);
                    writer.Write(Date2Bytes, 0, 64);


                    writer.Write(s.nPurchaseCashType);
                    writer.Write(s.nStandardSellingPrice);
                    writer.Write(s.nRealSellingPrice);
                    writer.Write(s.nSalePersent);
                    writer.Write(s.nIconID);
                    writer.Write(s.nMaskType);
                    writer.Write(s.nDispType);
                    writer.Write(s.nDispCount);
                    writer.Write(s.CashItems.Count);

                    foreach (var CashItem in s.CashItems)
                    {
                        writer.Write(CashItem.ItemId);
                        writer.Write(CashItem.Amount);

                    }

                }
            }
        }

        private static void WriteCashWebData(BinaryWriter writer, CashWebData[] cashWebDatas)
        {

            writer.Write(cashWebDatas.Length);
            foreach (var CashWebData in cashWebDatas)
            {
                writer.Write(CashWebData.nTableType);
                writer.Write(CashWebData.m_mapWebData);

                foreach (var CashWeb in CashWebData.CashWebInfo)
                {
                    writer.Write(CashWeb.sWebImageFile.Length);

                    char[] sWebImageFile = CashWeb.sWebImageFile.PadRight(CashWeb.sWebImageFile.Length, '\0').ToCharArray();
                    byte[] sWebImageFileBytes = Encoding.ASCII.GetBytes(sWebImageFile);
                    writer.Write(sWebImageFileBytes, 0, CashWeb.sWebImageFile.Length);

                    writer.Write(CashWeb.sWebLinkUrl.Length);

                    char[] sWebLinkUrl = CashWeb.sWebLinkUrl.PadRight(CashWeb.sWebLinkUrl.Length, '\0').ToCharArray();
                    byte[] sWebLinkUrlBytes = Encoding.ASCII.GetBytes(sWebLinkUrl);
                    writer.Write(sWebLinkUrlBytes, 0, CashWeb.sWebLinkUrl.Length);
                }
            }

        }
    }
    public class CashShopTamerInfo
    {
        public string CategoryName;
        public int unknow;
        public int unknow1;
        public string AllName;

        public int unknow2;
        public int unknow3;
        public string ExpasionName;
        public List<CashShopInformationCount> ExpansionItems = new();

        public int unknow4;
        public string ExpName;
        public List<CashShopInformationCount> ExpItems = new();

        public int unknow5;
        public string MovimentName;
        public List<CashShopInformationCount> MovimentItems = new();

        public int unknow6;
        public string ChatName;
        public List<CashShopInformationCount> ChatItems = new();

        public int unknow7;
        public string EtcName;
        public List<CashShopInformationCount> EtcItems = new();
    }
    public class CashShopDigimonInfo
    {

        public int CategoryNameSize;
        public string CategoryName;

        public int unknow;
        public int unknow1;
        public string AllName;

        public int unknow2;
        public int unknow3;


        public int unknow4;
        public string DigiEggName;
        public List<CashShopInformationCount> DigiEgg = new();

        public int unknow5;
        public string EvolutionName;
        public List<CashShopInformationCount> EvolutionItems = new();

        public int unknow6;
        public string HatchName;
        public List<CashShopInformationCount> HatchItems = new();

        public int unknow7;
        public string ReinforcedName;
        public List<CashShopInformationCount> ReinforcedItems = new();

        public int unknow8;
        public string RidingName;
        public List<CashShopInformationCount> RidingItems = new();

        public int unknow9;
        public string EtcName;
        public List<CashShopInformationCount> EtcItems = new();
    }
    public class CashShopAvatarInfo
    {

        public int CategoryNameSize;
        public string CategoryName;

        public int unknow;
        public int unknow1;
        public string AllName;

        public int unknow2;
        public int unknow3;
        public string ReinforcedName;
        public List<CashShopInformationCount> ReinforcedItems = new();

        public int unknow4;

        public int unknow5;
        public string HeadName;
        public List<CashShopInformationCount> HeadItems = new();

        public int unknow6;
        public string TopName;
        public List<CashShopInformationCount> TopItems = new();

        public int unknow7;
        public string BottomName;
        public List<CashShopInformationCount> BottomItems = new();

        public int unknow8;
        public string GlovesName;
        public List<CashShopInformationCount> GlovesItems = new();

        public int unknow9;
        public string ShoesName;
        public List<CashShopInformationCount> ShoesItems = new();

        public int unknow10;
        public string FashionName;
        public List<CashShopInformationCount> FashionItems = new();


        public int unknow11;
        public string CostumeName;
        public List<CashShopInformationCount> CostumeItems = new();
    }
    public class CashShopPackageInfo
    {

        public int CategoryNameSize;
        public string CategoryName;

        public int unknow;
        public int unknow1;
        public string AllName;

        public int unknow2;
        public string PackageName;
        public List<CashShopInformationCount> PackageItems = new();


    }
    public class CashShopInformationCount
    {
        public int CashShopId;
        public List<CASHINFO> CashInfo = new();

    }

    public class CashShopMainInformation
    {
        public int unknow;
        public int unknow1;
        public int unknow2;
        public string MainTitle;
        public int unknow3;
        public int unknow4;
        public string MainNewTitle;
        public List<MainCashShopInfo> MainNewItems = new();
        public int unknow5;
        public string MainHotTitle;
        public List<MainCashShopInfo> MainHotItems = new();
        public int unknow6;
        public string MainEventTitle;
        public List<MainCashShopInfo> MainEventItems = new();

        public long unknow7;
        public string UnknowItemsTitle;
        public List<MainCashShopInfo> MainUnknowItems = new();
        public int unknow8;
    }
    public class CASHINFO
    {
        public string CashName;
        public string Description;
        public byte bActive;           // Se habilitado
        public int dwProductID;      // Numero de vendas - Valor-chave
        public string szStartTime;   // hora de inicio da venda
        public string szEndTime; // Fim do periodo de venda
        public int nPurchaseCashType;  // Tipo de compra de item em dinheiro. 1: Dinheiro, 2: Bonus em dinheiro
        public int nStandardSellingPrice;  // preco de venda base
        public int nRealSellingPrice;      // desconto no preco de venda
        public int nSalePersent;       // porcentagem de desconto
        public int nIconID;            // Icon ID
        public int nMaskType;          // 1 : New, 2 : Hot, 3 : Event
        public int nDispType;          // Tipo, prazo e numero de vendas
        public int nDispCount;          // periodo e numero de vendas
        public int packageItems;
        public int cashshop_id;
        public string Name;
        public string Desc;
        public int Enabled;
        public string Date1;
        public string Date2;
        public int unique_id;
        public List<Item> CashItems = new();

        public CASHINFO() { }

    }

    public class Item
    {
        public int ItemId;
        public int Amount;
    }

    public class CashWebData
    {
        public int nTableType;
        public int m_mapWebData;

        public List<CashWebDataInfo> CashWebInfo = new();
    }

    public class CashWebDataInfo
    {
        public int Size;
        public string sWebImageFile;
        public int Size2;
        public string sWebLinkUrl;
    }
    public class MainCashShopInfo
    {
        public long ProductID;
    }


}



