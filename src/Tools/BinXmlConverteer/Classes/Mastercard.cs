
using System.Text;
using System.Xml.Linq;
using static BinXmlConverter.Classes.Mastercard;

namespace BinXmlConverter.Classes
{
    public class Mastercard
    {
        public class MasterCards
        {
            public int s_nID;
            public string s_szName;
            public int s_nDigimonID;
            public int s_nIcon;
            public int unknow;
            public int s_nLeader;
            public int s_nScale;
            public List<GradeInfo> GradeInfo = new(6);

        }

        public class GradeInfo
        {
            public ushort s_nIcon;
            public ushort s_nMax;
            public ushort s_nIdentiQuantity;
            public ushort s_nEff1;
            public ushort s_nEff1val;
            public ushort s_nEff2;
            public ushort s_nEff2val;
            public ushort unk;
            public int ItemId;
        }

        public class Leader
        {
            public int s_nID;
            public int s_nDigimonID;
            public int s_nPetID;
            public int s_nAni1;
            public int s_nAni2;
            public int s_nSpecial1;
            public int s_nSpecial2;
            public int s_nAbil1;
            public int s_nAbil2;
            public int s_nAbil3;
            public int s_nAbil4;
        }

        public class LeaderAbility
        {
            public ushort s_nID;
            public short unknow;
            public List<Sterms> STerms = new(3);

        }

        public class Sterms
        {
            public int s_nterm;
            public int s_ntermval;
            public int s_nEff;

        }

        public class DigimonImgPath
        {
            public int ID;
            public string s_szDigimonSealImgPath;
            public int nullp;
            public int unknow;
            public byte unknow1;
        }
        public class PlatePath
        {
            public int ID;
            public string s_szName;
            public string s_szNifFilePath;
            public string s_szGradeBackImagePath;
        }

        public class Elemental
        {
            public int s_nType;
            public string s_nFilePath;
        }
        public class SealAttribute
        {
            public int s_nType;
            public string s_nFilePath;
        }

        public class UnknowInformation
        {
         public int unknow;
         public int unknow1;
         public int unknow2;
        }


        public static (MasterCards[], Leader[], LeaderAbility[], DigimonImgPath[], PlatePath[], Elemental[], SealAttribute[], UnknowInformation[]) ExportMastercardFromBinary(string filePath)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                // Lê os MasterCards
                MasterCards[] masterCards = ReadMasterCards(reader);

                // Lê os Leaders
                Leader[] leaders = ReadLeaders(reader);

                // Lê os LeaderAbilities
                LeaderAbility[] leaderAbilities = ReadLeaderAbilities(reader);

                // Lê os DigimonImgPaths
                DigimonImgPath[] digimonImgPaths = ReadDigimonImgPaths(reader);

                // Lê os PlatePaths
                PlatePath[] platePaths = ReadPlatePaths(reader);

                // Lê os Elementals
                Elemental[] elementals = ReadElementals(reader);

                // Lê os Attributes
                SealAttribute[] attributes = ReadAttributes(reader);

                UnknowInformation[] unknowInformation = ReadUnknowInformation(reader);
                return (masterCards, leaders, leaderAbilities, digimonImgPaths, platePaths, elementals, attributes, unknowInformation);
            }
        }
        public static MasterCards[] ReadMasterCards(BinaryReader reader)
        {
            // Lê a quantidade de MasterCards
            int masterCardsCount = reader.ReadInt32();

            // Cria um array para armazenar os MasterCards
            MasterCards[] masterCards = new MasterCards[masterCardsCount];

            for (int i = 0; i < masterCardsCount; i++)
            {
                // Cria uma nova instância de MasterCards
                MasterCards card = new MasterCards();

                // Lê os campos inteiros diretamente
                card.s_nID = reader.ReadInt32();
                byte[] nameBytes = reader.ReadBytes(64 * 2);
                card.s_szName = System.Text.Encoding.Unicode.GetString(nameBytes);
                card.s_szName = CleanString(card.s_szName);
                card.s_nDigimonID = reader.ReadInt32();
                card.s_nIcon = reader.ReadInt32();
                card.unknow = reader.ReadInt32();
                card.s_nLeader = reader.ReadInt32();
                card.s_nScale = reader.ReadInt32();


                // Cria uma lista para armazenar os GradeInfo
                card.GradeInfo = new List<GradeInfo>();

                for (int j = 0; j < 6; j++)
                {
                    // Cria uma nova instância de GradeInfo
                    GradeInfo gradeInfo = new GradeInfo();

                    // Lê os campos inteiros diretamente
                    gradeInfo.s_nIcon = reader.ReadUInt16();
                    gradeInfo.s_nMax = reader.ReadUInt16();
                    gradeInfo.s_nIdentiQuantity = reader.ReadUInt16();
                    gradeInfo.s_nEff1 = reader.ReadUInt16();
                    gradeInfo.s_nEff1val = reader.ReadUInt16();
                    gradeInfo.s_nEff2 = reader.ReadUInt16();
                    gradeInfo.s_nEff2val = reader.ReadUInt16();
                    gradeInfo.unk = reader.ReadUInt16();

                    if (gradeInfo.s_nMax != 3000)
                    {
                        gradeInfo.ItemId = reader.ReadInt32();
                    }
                    // Adiciona o GradeInfo à lista de GradeInfo do MasterCards
                    card.GradeInfo.Add(gradeInfo);
                }

                // Adiciona o MasterCards ao array de MasterCards
                masterCards[i] = card;
            }

            return masterCards;
        }
        public static Leader[] ReadLeaders(BinaryReader reader)
        {
            // Lê a quantidade de Leaders
            int leadersCount = reader.ReadInt32();

            // Cria um array para armazenar os Leaders
            Leader[] leaders = new Leader[leadersCount];

            for (int i = 0; i < leadersCount; i++)
            {
                // Cria uma nova instância de Leader
                Leader leader = new Leader();

                // Lê os campos inteiros diretamente
                leader.s_nID = reader.ReadInt32();
                leader.s_nDigimonID = reader.ReadInt32();
                leader.s_nPetID = reader.ReadInt32();
                leader.s_nAni1 = reader.ReadInt32();
                leader.s_nAni2 = reader.ReadInt32();
                leader.s_nSpecial1 = reader.ReadInt32();
                leader.s_nSpecial2 = reader.ReadInt32();
                leader.s_nAbil1 = reader.ReadInt32();
                leader.s_nAbil2 = reader.ReadInt32();
                leader.s_nAbil3 = reader.ReadInt32();
                leader.s_nAbil4 = reader.ReadInt32();

                // Adiciona o Leader ao array de Leaders
                leaders[i] = leader;
            }

            return leaders;
        }
        public static LeaderAbility[] ReadLeaderAbilities(BinaryReader reader)
        {
            // Lê a quantidade de LeaderAbilities
            int leaderAbilitiesCount = reader.ReadInt32();

            // Cria um array para armazenar os LeaderAbilities
            LeaderAbility[] leaderAbilities = new LeaderAbility[leaderAbilitiesCount];

            for (int i = 0; i < leaderAbilitiesCount; i++)
            {
                // Cria uma nova instância de LeaderAbility
                LeaderAbility leaderAbility = new LeaderAbility();

                // Lê os campos inteiros diretamente
                leaderAbility.s_nID = reader.ReadUInt16();

                // Cria uma lista para armazenar os Sterms
                leaderAbility.STerms = new List<Sterms>();

                for (int j = 0; j < 3; j++)
                {
                    // Cria uma nova instância de Sterms
                    Sterms sterm = new Sterms();

                    // Lê os campos inteiros diretamente
                    sterm.s_nterm = reader.ReadInt32();
                    sterm.s_ntermval = reader.ReadInt32();
                    sterm.s_nEff = reader.ReadInt32();

                    // Adiciona o Sterms à lista de Sterms do LeaderAbility
                    leaderAbility.STerms.Add(sterm);
                }
                leaderAbility.unknow = reader.ReadInt16();
                // Adiciona o LeaderAbility ao array de LeaderAbilities
                leaderAbilities[i] = leaderAbility;
            }

            return leaderAbilities;
        }
        public static DigimonImgPath[] ReadDigimonImgPaths(BinaryReader reader)
        {
            // Lê a quantidade de DigimonImgPaths
            int digimonImgPathsCount = reader.ReadInt32();
            int Nullo = reader.ReadInt32();
            int Unknow = reader.ReadInt32();
            byte unknow = reader.ReadByte();
            // Cria um array para armazenar os DigimonImgPaths
            DigimonImgPath[] digimonImgPaths = new DigimonImgPath[digimonImgPathsCount -1];

            for (int i = 0; i < digimonImgPathsCount -1; i++)
            {
                // Cria uma nova instância de DigimonImgPath
                DigimonImgPath digimonImgPath = new DigimonImgPath();

                // Lê os campos inteiros diretamente
                digimonImgPath.ID = reader.ReadInt32();
                int NameSize = reader.ReadInt32();

                byte[] mname_get = new byte[NameSize];

                for (int g = 0; g < NameSize; g++)
                    mname_get[g] = reader.ReadByte();

                digimonImgPath.s_szDigimonSealImgPath = Encoding.ASCII.GetString(mname_get).Trim();


                // Adiciona o DigimonImgPath ao array de DigimonImgPaths
                digimonImgPaths[i] = digimonImgPath;
            }
            digimonImgPaths[0].nullp = Nullo;
            digimonImgPaths[0].unknow = Unknow;
            digimonImgPaths[0].unknow1 = unknow;
            return digimonImgPaths;
        }
        public static PlatePath[] ReadPlatePaths(BinaryReader reader)
        {
            // Lê a quantidade de PlatePaths
            int platePathsCount = reader.ReadInt32();

            // Cria um array para armazenar os PlatePaths
            PlatePath[] platePaths = new PlatePath[platePathsCount];

            for (int i = 0; i < platePathsCount; i++)
            {
                // Cria uma nova instância de PlatePath
                PlatePath platePath = new PlatePath();

                // Lê os campos inteiros diretamente
                platePath.ID = reader.ReadInt32();
                int nameSize = reader.ReadInt32() * 2;
                byte[] mname_get = new byte[nameSize];

                for (int g = 0; g < nameSize; g++)
                    mname_get[g] = reader.ReadByte();

                platePath.s_szName = Encoding.Unicode.GetString(mname_get).Trim();

                int FileSize = reader.ReadInt32();
                byte[] FileSize_Get = new byte[FileSize];

                for (int g = 0; g < FileSize; g++)
                    FileSize_Get[g] = reader.ReadByte();

                platePath.s_szNifFilePath = Encoding.ASCII.GetString(FileSize_Get).Trim();

                int PathSize = reader.ReadInt32();
                byte[] PathSize_Get = new byte[PathSize];

                for (int g = 0; g < PathSize; g++)
                    PathSize_Get[g] = reader.ReadByte();

                platePath.s_szGradeBackImagePath = Encoding.ASCII.GetString(PathSize_Get).Trim();
                // Adiciona o PlatePath ao array de PlatePaths
                platePaths[i] = platePath;
            }

            return platePaths;
        }
        public static Elemental[] ReadElementals(BinaryReader reader)
        {
            // Lê a quantidade de Elementals
            int elementalsCount = reader.ReadInt32();

            // Cria um array para armazenar os Elementals
            Elemental[] elementals = new Elemental[elementalsCount];

            for (int i = 0; i < elementalsCount; i++)
            {
                // Cria uma nova instância de Elemental
                Elemental elemental = new Elemental();

                // Lê os campos inteiros diretamente
                elemental.s_nType = reader.ReadInt32();
                int nameSize = reader.ReadInt32();
                byte[] mname_get = new byte[nameSize];

                for (int g = 0; g < nameSize; g++)
                    mname_get[g] = reader.ReadByte();

                elemental.s_nFilePath = Encoding.ASCII.GetString(mname_get).Trim();


                // Adiciona o Elemental ao array de Elementals
                elementals[i] = elemental;
            }

            return elementals;
        }

        public static SealAttribute[] ReadAttributes(BinaryReader reader)
        {
            // Lê a quantidade de Attributes
            int attributesCount = reader.ReadInt32();

            // Cria um array para armazenar os Attributes
            SealAttribute[] attributes = new SealAttribute[attributesCount];

            for (int i = 0; i < attributesCount; i++)
            {
                // Cria uma nova instância de Attribute
                SealAttribute attribute = new SealAttribute();

                // Lê os campos inteiros diretamente
                attribute.s_nType = reader.ReadInt32();
                int nameSize = reader.ReadInt32();
                byte[] mname_get = new byte[nameSize];

                for (int g = 0; g < nameSize; g++)
                    mname_get[g] = reader.ReadByte();

                attribute.s_nFilePath = Encoding.ASCII.GetString(mname_get).Trim();


                // Adiciona o Attribute ao array de Attributes
                attributes[i] = attribute;
            }

            return attributes;
        }
        public static UnknowInformation[] ReadUnknowInformation(BinaryReader reader)
        {
            // Lê a quantidade de LeaderAbilities
            int Count = reader.ReadInt32();

            // Cria um array para armazenar os LeaderAbilities
            UnknowInformation[] unknowInformation = new UnknowInformation[Count];

            for (int i = 0; i < Count; i++)
            {
                // Cria uma nova instância de LeaderAbility
                UnknowInformation unknow= new UnknowInformation();

                // Lê os campos inteiros diretamente
                unknow.unknow = reader.ReadInt32();
                unknow.unknow1 = reader.ReadInt32();
                unknow.unknow2 = reader.ReadInt32();

               unknowInformation[i] = unknow;
            }

            return unknowInformation;
        }

        public static void ExportMastercardToXml(string exportPath, MasterCards[] masterCards, Leader[] leaders, LeaderAbility[] leaderAbilities, DigimonImgPath[] digimonImgPaths, PlatePath[] platePaths, Elemental[] elementals, SealAttribute[] attributes, UnknowInformation[] unknows)
        {
            // Exportar MasterCards
            XElement masterCardsElement = new XElement("MasterCards");
            foreach (MasterCards card in masterCards)
            {
                XElement cardElement = new XElement("Card");

                cardElement.Add(new XElement("s_nID", card.s_nID));
                cardElement.Add(new XElement("s_szName", card.s_szName));
                cardElement.Add(new XElement("s_nDigimonID", card.s_nDigimonID));
                cardElement.Add(new XElement("s_nIcon", card.s_nIcon));
                cardElement.Add(new XElement("unknow", card.unknow));
                cardElement.Add(new XElement("s_nLeader", card.s_nLeader));
                cardElement.Add(new XElement("s_nScale", card.s_nScale));

                XElement gradeInfoElement = new XElement("GradeInfo");
                foreach (GradeInfo gradeInfo in card.GradeInfo)
                {
                    XElement gradeInfoItem = new XElement("GradeInfoItem");
                    gradeInfoItem.Add(new XElement("s_nIcon", gradeInfo.s_nIcon));
                    gradeInfoItem.Add(new XElement("s_nMax", gradeInfo.s_nMax));
                    gradeInfoItem.Add(new XElement("s_nIdentiQuantity", gradeInfo.s_nIdentiQuantity));
                    gradeInfoItem.Add(new XElement("s_nEff1", gradeInfo.s_nEff1));
                    gradeInfoItem.Add(new XElement("s_nEff1val", gradeInfo.s_nEff1val));
                    gradeInfoItem.Add(new XElement("s_nEff2", gradeInfo.s_nEff2));
                    gradeInfoItem.Add(new XElement("s_nEff2val", gradeInfo.s_nEff2val));
                    gradeInfoItem.Add(new XElement("unk", gradeInfo.unk));
                    gradeInfoItem.Add(new XElement("ItemId", gradeInfo.ItemId));

                    gradeInfoElement.Add(gradeInfoItem);
                }

                cardElement.Add(gradeInfoElement);

                masterCardsElement.Add(cardElement);
            }

            // Salvar o XML de MasterCards
            masterCardsElement.Save(Path.Combine(exportPath, "MasterCards.xml"));

            // Exportar Leaders
            XElement leadersElement = new XElement("Leaders");
            foreach (Leader leader in leaders)
            {
                XElement leaderElement = new XElement("Leader");

                leaderElement.Add(new XElement("s_nID", leader.s_nID));
                leaderElement.Add(new XElement("s_nDigimonID", leader.s_nDigimonID));
                leaderElement.Add(new XElement("s_nPetID", leader.s_nPetID));
                leaderElement.Add(new XElement("s_nAni1", leader.s_nAni1));
                leaderElement.Add(new XElement("s_nAni2", leader.s_nAni2));
                leaderElement.Add(new XElement("s_nSpecial1", leader.s_nSpecial1));
                leaderElement.Add(new XElement("s_nSpecial2", leader.s_nSpecial2));
                leaderElement.Add(new XElement("s_nAbil1", leader.s_nAbil1));
                leaderElement.Add(new XElement("s_nAbil2", leader.s_nAbil2));
                leaderElement.Add(new XElement("s_nAbil3", leader.s_nAbil3));
                leaderElement.Add(new XElement("s_nAbil4", leader.s_nAbil4));

                leadersElement.Add(leaderElement);
            }

            // Salvar o XML de Leaders
            leadersElement.Save(Path.Combine(exportPath, "Leaders.xml"));

            // Exportar LeaderAbilities
            XElement leaderAbilitiesElement = new XElement("LeaderAbilities");
            foreach (LeaderAbility leaderAbility in leaderAbilities)
            {
                XElement leaderAbilityElement = new XElement("LeaderAbility");

                leaderAbilityElement.Add(new XElement("s_nID", leaderAbility.s_nID));

                XElement sTermsElement = new XElement("STerms");
                foreach (Sterms sTerm in leaderAbility.STerms)
                {
                    XElement sTermElement = new XElement("STerm");
                    sTermElement.Add(new XElement("s_nterm", sTerm.s_nterm));
                    sTermElement.Add(new XElement("s_ntermval", sTerm.s_ntermval));
                    sTermElement.Add(new XElement("s_nEff", sTerm.s_nEff));

                    sTermsElement.Add(sTermElement);
                }
                leaderAbilityElement.Add(new XElement("unknow", leaderAbility.unknow));
                leaderAbilityElement.Add(sTermsElement);

                leaderAbilitiesElement.Add(leaderAbilityElement);
            }

            // Salvar o XML de LeaderAbilities
            leaderAbilitiesElement.Save(Path.Combine(exportPath, "LeaderAbilities.xml"));

            // Exportar DigimonImgPaths
            XElement digimonImgPathsElement = new XElement("DigimonImgPaths");

            foreach (DigimonImgPath imgPath in digimonImgPaths)
            {
                XElement imgPathElement = new XElement("DigimonImgPath");
                imgPathElement.Add(new XElement("nullp", imgPath.nullp));
                imgPathElement.Add(new XElement("unknow", imgPath.unknow));
                imgPathElement.Add(new XElement("unknow1", imgPath.unknow1));
                imgPathElement.Add(new XElement("ID", imgPath.ID));
                imgPathElement.Add(new XElement("s_szDigimonSealImgPath", imgPath.s_szDigimonSealImgPath));

                digimonImgPathsElement.Add(imgPathElement);
            }

            // Salvar o XML de DigimonImgPaths
            digimonImgPathsElement.Save(Path.Combine(exportPath, "DigimonImgPaths.xml"));

            // Exportar PlatePaths
            XElement platePathsElement = new XElement("PlatePaths");
            foreach (PlatePath platePath in platePaths)
            {
                XElement platePathElement = new XElement("PlatePath");
                platePathElement.Add(new XElement("ID", platePath.ID));
                platePathElement.Add(new XElement("s_szName", platePath.s_szName));
                platePathElement.Add(new XElement("s_szNifFilePath", platePath.s_szNifFilePath));
                platePathElement.Add(new XElement("s_szGradeBackImagePath", platePath.s_szGradeBackImagePath));

                platePathsElement.Add(platePathElement);
            }

            // Salvar o XML de PlatePaths
            platePathsElement.Save(Path.Combine(exportPath, "PlatePaths.xml"));

            // Exportar Elementals
            XElement elementalsElement = new XElement("Elementals");
            foreach (Elemental elemental in elementals)
            {
                XElement elementalElement = new XElement("Elemental");
                elementalElement.Add(new XElement("s_nType", elemental.s_nType));
                elementalElement.Add(new XElement("s_nFilePath", elemental.s_nFilePath));

                elementalsElement.Add(elementalElement);
            }

            // Salvar o XML de Elementals
            elementalsElement.Save(Path.Combine(exportPath, "Elementals.xml"));

            // Exportar Attributes
            XElement attributesElement = new XElement("Attributes");
            foreach (SealAttribute attribute in attributes)
            {
                XElement attributeElement = new XElement("Attribute");
                attributeElement.Add(new XElement("s_nType", attribute.s_nType));
                attributeElement.Add(new XElement("s_nFilePath", attribute.s_nFilePath));

                attributesElement.Add(attributeElement);
            }

            // Salvar o XML de Attributes
            attributesElement.Save(Path.Combine(exportPath, "Attributes.xml"));
            leaderAbilitiesElement.Save(Path.Combine(exportPath, "LeaderAbilities.xml"));


         
            // Exportar DigimonImgPaths
            XElement UnknowElement = new XElement("UnknowInformations");

            foreach (UnknowInformation un in unknows)
            {
                XElement imgPathElement = new XElement("UnknowInformation");
                imgPathElement.Add(new XElement("unknow", un.unknow));
                imgPathElement.Add(new XElement("unknow1", un.unknow1));
                imgPathElement.Add(new XElement("unknow2", un.unknow2));

                UnknowElement.Add(imgPathElement);
            }

            UnknowElement.Save(Path.Combine(exportPath, "UnknowInformations.xml"));
        }
        public static (MasterCards[], Leader[], LeaderAbility[], DigimonImgPath[], PlatePath[], Elemental[], SealAttribute[], UnknowInformation[]) ImportMastercardsFromXml(string importPath)
        {
            // Importar MasterCards
            XElement masterCardsElement = XElement.Load(Path.Combine(importPath, "MasterCards.xml"));
            MasterCards[] cards = masterCardsElement.Elements("Card")
                .Select(cardElement => new MasterCards
                {
                    s_nID = int.Parse(cardElement.Element("s_nID").Value),
                    s_szName = cardElement.Element("s_szName").Value,
                    s_nDigimonID = int.Parse(cardElement.Element("s_nDigimonID").Value),
                    s_nIcon = int.Parse(cardElement.Element("s_nIcon").Value),
                    unknow = int.Parse(cardElement.Element("unknow").Value),
                    s_nLeader = int.Parse(cardElement.Element("s_nLeader").Value),
                    s_nScale = int.Parse(cardElement.Element("s_nScale").Value),
                    GradeInfo = cardElement.Element("GradeInfo")
                  .Elements("GradeInfoItem")
                  .Select(giElement => new GradeInfo
                  {
                      s_nIcon = ushort.Parse(giElement.Element("s_nIcon").Value),
                      s_nMax = ushort.Parse(giElement.Element("s_nMax").Value),
                      s_nIdentiQuantity = ushort.Parse(giElement.Element("s_nIdentiQuantity").Value),
                      s_nEff1 = ushort.Parse(giElement.Element("s_nEff1").Value),
                      s_nEff1val = ushort.Parse(giElement.Element("s_nEff1val").Value),
                      s_nEff2 = ushort.Parse(giElement.Element("s_nEff2").Value),
                      s_nEff2val = ushort.Parse(giElement.Element("s_nEff2val").Value),
                      unk = ushort.Parse(giElement.Element("unk").Value),
                      ItemId = int.Parse(giElement.Element("ItemId").Value ?? "0")
                  })
                              .ToList()
                })
                .ToArray();

            // Importar Leaders
            XElement leadersElement = XElement.Load(Path.Combine(importPath, "Leaders.xml"));
            Leader[] leaders = leadersElement.Elements("Leader")
                .Select(lElement => new Leader
                {
                    s_nID = int.Parse(lElement.Element("s_nID").Value),
                    s_nDigimonID = int.Parse(lElement.Element("s_nDigimonID").Value),
                    s_nPetID = int.Parse(lElement.Element("s_nPetID").Value),
                    s_nAni1 = int.Parse(lElement.Element("s_nAni1").Value),
                    s_nAni2 = int.Parse(lElement.Element("s_nAni2").Value),
                    s_nSpecial1 = int.Parse(lElement.Element("s_nSpecial1").Value),
                    s_nSpecial2 = int.Parse(lElement.Element("s_nSpecial2").Value),
                    s_nAbil1 = int.Parse(lElement.Element("s_nAbil1").Value),
                    s_nAbil2 = int.Parse(lElement.Element("s_nAbil2").Value),
                    s_nAbil3 = int.Parse(lElement.Element("s_nAbil3").Value),
                    s_nAbil4 = int.Parse(lElement.Element("s_nAbil4").Value)
                })
                .ToArray();

            // Importar LeaderAbilities
            XElement leaderAbilitiesElement = XElement.Load(Path.Combine(importPath, "LeaderAbilities.xml"));
            LeaderAbility[] leaderAbilities = leaderAbilitiesElement.Elements("LeaderAbility")
                .Select(laElement => new LeaderAbility
                {
                    s_nID = ushort.Parse(laElement.Element("s_nID").Value),
                    unknow = short.Parse(laElement.Element("unknow").Value),
                    STerms = laElement.Element("STerms").Elements("STerm")
                        .Select(stElement => new Sterms
                        {
                            s_nterm = int.Parse(stElement.Element("s_nterm").Value),
                            s_ntermval = int.Parse(stElement.Element("s_ntermval").Value),
                            s_nEff = int.Parse(stElement.Element("s_nEff").Value)
                        })
                        .ToList()

                })
                .ToArray();

            // Importar DigimonImgPaths
            XElement digimonImgPathsElement = XElement.Load(Path.Combine(importPath, "DigimonImgPaths.xml"));

            DigimonImgPath[] digimonImgPaths = digimonImgPathsElement.Elements("DigimonImgPath")
                .Select(dipElement =>
                {
                    DigimonImgPath digimonImgPath = new DigimonImgPath();

                    digimonImgPath.nullp = int.Parse(dipElement.Element("nullp").Value);
                    digimonImgPath.unknow = int.Parse(dipElement.Element("unknow").Value);
                    digimonImgPath.unknow1 = byte.Parse(dipElement.Element("unknow1").Value);
                    digimonImgPath.ID = int.Parse(dipElement.Element("ID").Value);
                    digimonImgPath.s_szDigimonSealImgPath = dipElement.Element("s_szDigimonSealImgPath").Value;

                    return digimonImgPath;
                })
                .ToArray();

            // Importar PlatePaths
            XElement platePathsElement = XElement.Load(Path.Combine(importPath, "PlatePaths.xml"));
            PlatePath[] platePaths = platePathsElement.Elements("PlatePath")
                .Select(ppElement => new PlatePath
                {
                    ID = int.Parse(ppElement.Element("ID").Value),
                    s_szName = ppElement.Element("s_szName").Value,
                    s_szNifFilePath = ppElement.Element("s_szNifFilePath").Value,
                    s_szGradeBackImagePath = ppElement.Element("s_szGradeBackImagePath").Value
                })
                .ToArray();

            // Importar Elementals
            XElement elementalsElement = XElement.Load(Path.Combine(importPath, "Elementals.xml"));
            Elemental[] elementals = elementalsElement.Elements("Elemental")
                .Select(eElement => new Elemental
                {
                    s_nType = int.Parse(eElement.Element("s_nType").Value),
                    s_nFilePath = eElement.Element("s_nFilePath").Value
                })
                .ToArray();

            // Importar Attributes
            XElement attributesElement = XElement.Load(Path.Combine(importPath, "Attributes.xml"));
            SealAttribute[] attributes = attributesElement.Elements("Attribute")
                .Select(aElement => new SealAttribute
                {
                    s_nType = int.Parse(aElement.Element("s_nType").Value),
                    s_nFilePath = aElement.Element("s_nFilePath").Value
                })
                .ToArray();

            XElement UnknowElement = XElement.Load(Path.Combine(importPath, "UnknowInformations.xml"));
            UnknowInformation[] unknows = UnknowElement.Elements("UnknowInformation")
                .Select(unElement => new UnknowInformation
                {
                    unknow = int.Parse(unElement.Element("unknow").Value),
                    unknow1 = int.Parse(unElement.Element("unknow1").Value),
                    unknow2 = int.Parse(unElement.Element("unknow2").Value)
                })
                .ToArray();

            return (cards, leaders, leaderAbilities, digimonImgPaths, platePaths, elementals, attributes,unknows);
        }


        public static void WriteBinary(string filePath, MasterCards[] masterCards, Leader[] leaders, LeaderAbility[] leaderAbilities, DigimonImgPath[] digimonImgPaths, PlatePath[] platePaths, Elemental[] elementals, SealAttribute[] sealAttributes, UnknowInformation[] unknows)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {


                BinaryExtension.Write(writer, masterCards);
                BinaryExtension.Write(writer, leaders);
                BinaryExtension.Write(writer, leaderAbilities);
                BinaryExtension.Write(writer, digimonImgPaths);
                BinaryExtension.Write(writer, platePaths);
                BinaryExtension.Write(writer, elementals);
                BinaryExtension.Write(writer, sealAttributes);
                BinaryExtension.Write(writer, unknows);
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
    public static class BinaryExtension
    {

        public static void Write(this BinaryWriter writer, MasterCards[] masterCardss)
        {
            writer.Write(masterCardss.Length);
            foreach (MasterCards masterCards in masterCardss)
            {
                writer.Write(masterCards.s_nID);

                for (int i = 0; i < 64; i++)
                {
                    char c = i < masterCards.s_szName.Length ? masterCards.s_szName[i] : '\0';
                    writer.Write((ushort)c);
                }

                writer.Write(masterCards.s_nDigimonID);
                writer.Write(masterCards.s_nIcon);
                writer.Write(masterCards.unknow);
                writer.Write(masterCards.s_nLeader);
                writer.Write(masterCards.s_nScale);
                foreach (var gradeInfo in masterCards.GradeInfo)
                {
                    writer.Write(gradeInfo.s_nIcon);
                    writer.Write(gradeInfo.s_nMax);
                    writer.Write(gradeInfo.s_nIdentiQuantity);
                    writer.Write(gradeInfo.s_nEff1);
                    writer.Write(gradeInfo.s_nEff1val);
                    writer.Write(gradeInfo.s_nEff2);
                    writer.Write(gradeInfo.s_nEff2val);
                    writer.Write(gradeInfo.unk);
                    if (gradeInfo.ItemId != 0)
                    {
                        writer.Write(gradeInfo.ItemId);
                    }
                    
                }
            }
        }

        public static void Write(this BinaryWriter writer, Leader[] leaders)
        {
            writer.Write(leaders.Length);
            foreach (Leader leader in leaders)
            {
                writer.Write(leader.s_nID);
                writer.Write(leader.s_nDigimonID);
                writer.Write(leader.s_nPetID);
                writer.Write(leader.s_nAni1);
                writer.Write(leader.s_nAni2);
                writer.Write(leader.s_nSpecial1);
                writer.Write(leader.s_nSpecial2);
                writer.Write(leader.s_nAbil1);
                writer.Write(leader.s_nAbil2);
                writer.Write(leader.s_nAbil3);
                writer.Write(leader.s_nAbil4);
            }
        }

        public static void Write(this BinaryWriter writer, LeaderAbility[] leaderAbilitys)
        {
            writer.Write(leaderAbilitys.Length);
            foreach (LeaderAbility leaderAbility in leaderAbilitys)
            {
                writer.Write(leaderAbility.s_nID);
                foreach (var term in leaderAbility.STerms)
                {
                    writer.Write(term.s_nterm);
                    writer.Write(term.s_ntermval);
                    writer.Write(term.s_nEff);
                }

                writer.Write(leaderAbility.unknow);
            }
        }

        public static void Write(this BinaryWriter writer, DigimonImgPath[] digimonImgPaths)
        {
            writer.Write(digimonImgPaths.Length +1);
            writer.Write(digimonImgPaths[0].nullp);
            writer.Write(digimonImgPaths[0].unknow);
            writer.Write(digimonImgPaths[0].unknow1);
            foreach (var digimonImgPath in digimonImgPaths)
            {
                writer.Write(digimonImgPath.ID);
                byte[] s_szDigimonSealImgPathBytes = Encoding.ASCII.GetBytes(digimonImgPath.s_szDigimonSealImgPath);

                // Escreve a quantidade de bytes da string s_szDigimonSealImgPath
                writer.Write(s_szDigimonSealImgPathBytes.Length);

                // Escreve os bytes da string s_szDigimonSealImgPath
                writer.Write(s_szDigimonSealImgPathBytes);
            }

        }

        public static void Write(this BinaryWriter writer, PlatePath[] platePaths)
        {
            writer.Write(platePaths.Length);
            foreach (var platePath in platePaths)
            {
                writer.Write(platePath.ID);
                byte[] s_szNameBytes = Encoding.Unicode.GetBytes(platePath.s_szName);

                // Escreve a quantidade de caracteres da string Name (wchar_t)
                writer.Write(s_szNameBytes.Length /2);
                // Escreve os bytes da string Name
                writer.Write(s_szNameBytes);

                byte[] s_szNifFilePathBytes = Encoding.ASCII.GetBytes(platePath.s_szNifFilePath);

                // Escreve a quantidade de caracteres da string Name (wchar_t)
                writer.Write(platePath.s_szNifFilePath.Length);
                // Escreve os bytes da string Name
                writer.Write(s_szNifFilePathBytes);

                byte[] s_szGradeBackImagePathBytes = Encoding.ASCII.GetBytes(platePath.s_szGradeBackImagePath);

                // Escreve a quantidade de caracteres da string Name (wchar_t)
                writer.Write(platePath.s_szGradeBackImagePath.Length);
                // Escreve os bytes da string Name
                writer.Write(s_szGradeBackImagePathBytes);
            }

        }

        public static void Write(this BinaryWriter writer, Elemental[] elementals)
        {
            writer.Write(elementals.Length);
            foreach (var elemental in elementals)
            {
                writer.Write(elemental.s_nType);
                byte[] s_nFilePathBytes = Encoding.ASCII.GetBytes(elemental.s_nFilePath);

                // Escreve a quantidade de caracteres da string Name (wchar_t)
                writer.Write(elemental.s_nFilePath.Length);
                writer.Write(s_nFilePathBytes);
            }

        }

        public static void Write(this BinaryWriter writer, SealAttribute[] attributes)
        {
            writer.Write(attributes.Length);
            foreach (var attribute in attributes)
            {
                writer.Write(attribute.s_nType);

                byte[] s_nFilePathBytes = Encoding.ASCII.GetBytes(attribute.s_nFilePath);

                // Escreve a quantidade de caracteres da string Name (wchar_t)
                writer.Write(attribute.s_nFilePath.Length);
                writer.Write(s_nFilePathBytes);
            }

        }
        public static void Write(this BinaryWriter writer, UnknowInformation[] unknows)
        {
            writer.Write(unknows.Length);
            foreach (UnknowInformation un in unknows)
            {
                writer.Write(un.unknow);
                writer.Write(un.unknow1);
                writer.Write(un.unknow2);

            }
        }


    }
}
