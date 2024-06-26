using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;

namespace BinXmlConverter.Classes
{
    public class Ride
    {
        public int dcount;
        public int s_dwDigimonID;
        public int s_dwChangeRide;
        public float s_fMoveSpeed;
        public string s_szComment;
        public int s_nRideType;
        public float s_fAniRate_Run;
        public int s_nSection;
        public int s_nNeedCount;
        public int s_nSection_2;
        public int s_nNeedCount_2;

        public static Ride[] RideToXml(string DigimonInput)
        {
            using (BitReader reader = new BitReader(File.Open(DigimonInput, FileMode.Open)))
            {
                int dcount = reader.ReadInt();
                Ride[] rides = new Ride[dcount];

                for (int i = 0; i < dcount; i++)
                {
                    Ride ride = new Ride();
                    ride.s_dwDigimonID = reader.ReadInt();
                    ride.s_dwChangeRide = reader.ReadInt();
                    ride.s_fMoveSpeed = reader.ReadFloat();
                    ride.s_szComment = reader.ReadZString(Encoding.Unicode, 512 * 2);
                    ride.s_nRideType = reader.ReadInt();
                    ride.s_fAniRate_Run = reader.ReadFloat();
                    ride.s_nSection = reader.ReadInt(); ;
                    ride.s_nNeedCount = reader.ReadInt();
                    ride.s_nSection_2 = reader.ReadInt();
                    ride.s_nNeedCount_2 = reader.ReadInt();

                    rides[i] = ride;
                }
                return rides;
            }

        }
        public static void ExportRidesToXml(string xmlPath, Ride[] rides)
        {
            XDocument doc = new XDocument();
            XElement rootElement = new XElement("Rides");
            doc.Add(rootElement);

            foreach (var ride in rides)
            {
                XElement rideElement = new XElement("Ride");
                rideElement.Add(new XElement("s_dwDigimonID", ride.s_dwDigimonID));
                rideElement.Add(new XElement("s_dwChangeRide", ride.s_dwChangeRide));
                rideElement.Add(new XElement("s_fMoveSpeed", ride.s_fMoveSpeed));
                rideElement.Add(new XElement("s_szComment", ride.s_szComment));
                rideElement.Add(new XElement("s_nRideType", ride.s_nRideType));
                rideElement.Add(new XElement("s_fAniRate_Run", ride.s_fAniRate_Run));
                rideElement.Add(new XElement("s_nSection", ride.s_nSection));
                rideElement.Add(new XElement("s_nNeedCount", ride.s_nNeedCount));
                rideElement.Add(new XElement("s_nSection_2", ride.s_nSection_2));
                rideElement.Add(new XElement("s_nNeedCount_2", ride.s_nNeedCount_2));

                rootElement.Add(rideElement);
            }

            doc.Save(xmlPath);
        }
        public  static Ride[] ImportRidesFromXml(string xmlPath)
        {
            XDocument doc = XDocument.Load(xmlPath);
            IEnumerable<XElement> rideElements = doc.XPathSelectElements("//Ride");

            List<Ride> rides = new List<Ride>();

            foreach (var rideElement in rideElements)
            {
                Ride ride = new Ride();

                ride.s_dwDigimonID = int.Parse(rideElement.Element("s_dwDigimonID").Value);
                ride.s_dwChangeRide = int.Parse(rideElement.Element("s_dwChangeRide").Value);
                ride.s_fMoveSpeed = float.Parse(rideElement.Element("s_fMoveSpeed").Value);
                ride.s_szComment = rideElement.Element("s_szComment").Value;
                ride.s_nRideType = int.Parse(rideElement.Element("s_nRideType").Value);
                ride.s_fAniRate_Run = float.Parse(rideElement.Element("s_fAniRate_Run").Value);
                ride.s_nSection = int.Parse(rideElement.Element("s_nSection").Value);
                ride.s_nNeedCount = int.Parse(rideElement.Element("s_nNeedCount").Value);
                ride.s_nSection_2 = int.Parse(rideElement.Element("s_nSection_2").Value);
                ride.s_nNeedCount_2 = int.Parse(rideElement.Element("s_nNeedCount_2").Value);

                rides.Add(ride);
            }

            return rides.ToArray();
        }
        public static void ExportRidesToBinary(string binPathRide, Ride[] rides)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(binPathRide, FileMode.Create)))
            {
                writer.Write(rides.Length);

                foreach (Ride ride in rides)
                {
                    writer.Write(ride.s_dwDigimonID);
                    writer.Write(ride.s_dwChangeRide);
                    writer.Write(ride.s_fMoveSpeed);
                    byte[] commentBytes = Encoding.Unicode.GetBytes(ride.s_szComment);
                    Array.Resize(ref commentBytes, 512 * 2); // Ajusta o tamanho do array para 512 caracteres wchar_t
                    writer.Write(commentBytes);
                    writer.Write(ride.s_nRideType);
                    writer.Write(ride.s_fAniRate_Run);
                    writer.Write(ride.s_nSection);
                    writer.Write(ride.s_nNeedCount);
                    writer.Write(ride.s_nSection_2);
                    writer.Write(ride.s_nNeedCount_2);
                }
            }
        }
    }

}
