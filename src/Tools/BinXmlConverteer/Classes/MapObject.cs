using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BinXmlConverter.Classes
{
    public class MapObject
    {
        public int MapId;
        public int Size;
        public List<MapSourceObject> mapSourceObjects = new();


        public static MapObject[] ExportMapObjectFromBinary(string MapObjectInput)
        {
            using (BitReader reader = new BitReader(File.Open(MapObjectInput, FileMode.Open)))
            {
                var count = reader.ReadInt();
                MapObject[] objects = new MapObject[count];
                for (int i = 0; i < count; i++)
                {

                    MapObject mapObject = new MapObject();  
                    mapObject.MapId = reader.ReadInt();
                    mapObject.Size = reader.ReadInt();

                    for (int x = 0; x < mapObject.Size; x++)
                    {
                        MapSourceObject mapobject = new();

                        mapobject.ObjectId = reader.ReadInt();
                        int ObjectCount = reader.ReadInt();
                        for (int z= 0; z < ObjectCount; z++)
                        {
                            OrderObject orderObject = new();
                            orderObject.OrderId = reader.ReadInt();
                            orderObject.FactorSize = reader.ReadInt();
                            for (int y = 0; y < orderObject.FactorSize; y++)
                            {
                                Object @object = new();
                                @object.s_nOpenType = reader.ReadInt();
                                @object.s_nFactorCnt = reader.ReadInt();
                                @object.s_nFactor = reader.ReadInt();
                                orderObject.objects.Add(@object);
                            }
                            mapobject.Objects.Add(orderObject);
                        }

                        mapObject.mapSourceObjects.Add(mapobject);
                    }
                    objects[i] = mapObject;
                }

                return objects;
            }
         }
        public static void ExportMapObjectToXml(MapObject[] mapObjects, string filePath)
        {
            XElement rootElement = new XElement("MapObjects");

            foreach (MapObject mapObject in mapObjects)
            {
                XElement mapObjectElement = new XElement("MapObject");
                XElement mapIdElement = new XElement("MapId", mapObject.MapId);
                XElement sizeElement = new XElement("Size", mapObject.Size);
                mapObjectElement.Add(mapIdElement, sizeElement);

                foreach (MapSourceObject mapSourceObject in mapObject.mapSourceObjects)
                {
                    XElement mapSourceObjectElement = new XElement("MapSourceObject");
                    XElement objectIdElement = new XElement("ObjectId", mapSourceObject.ObjectId);
                    mapSourceObjectElement.Add(objectIdElement);

                    foreach (OrderObject orderObject in mapSourceObject.Objects)
                    {
                        XElement orderObjectElement = new XElement("OrderObject");
                        XElement orderIdElement = new XElement("OrderId", orderObject.OrderId);
                        XElement factorSizeElement = new XElement("FactorSize", orderObject.FactorSize);
                        orderObjectElement.Add(orderIdElement, factorSizeElement);

                        foreach (Object obj in orderObject.objects)
                        {
                            XElement objectElement = new XElement("Object");
                            XElement openTypeElement = new XElement("s_nOpenType", obj.s_nOpenType);
                            XElement factorElement = new XElement("s_nFactor", obj.s_nFactor);
                            XElement factorCntElement = new XElement("s_nFactorCnt", obj.s_nFactorCnt);
                            objectElement.Add(openTypeElement, factorElement, factorCntElement);

                            orderObjectElement.Add(objectElement);
                        }

                        mapSourceObjectElement.Add(orderObjectElement);
                    }

                    mapObjectElement.Add(mapSourceObjectElement);
                }

                rootElement.Add(mapObjectElement);
            }

            XDocument xmlDoc = new XDocument(rootElement);
            xmlDoc.Save(filePath);
        }

        public static MapObject[] ImportMapObjectFromXml(string filePath)
        {
            XDocument xmlDoc = XDocument.Load(filePath);
            XElement rootElement = xmlDoc.Element("MapObjects");

            List<MapObject> mapObjectsList = new List<MapObject>();

            foreach (XElement mapObjectElement in rootElement.Elements("MapObject"))
            {
                MapObject mapObject = new MapObject();
                mapObject.MapId = int.Parse(mapObjectElement.Element("MapId").Value);
                mapObject.Size = int.Parse(mapObjectElement.Element("Size").Value);
                mapObject.mapSourceObjects = new List<MapSourceObject>();

                foreach (XElement mapSourceObjectElement in mapObjectElement.Elements("MapSourceObject"))
                {
                    MapSourceObject mapSourceObject = new MapSourceObject();
                    mapSourceObject.ObjectId = int.Parse(mapSourceObjectElement.Element("ObjectId").Value);
                    mapSourceObject.Objects = new List<OrderObject>();

                    foreach (XElement orderObjectElement in mapSourceObjectElement.Elements("OrderObject"))
                    {
                        OrderObject orderObject = new OrderObject();
                        orderObject.OrderId = int.Parse(orderObjectElement.Element("OrderId").Value);
                        orderObject.FactorSize = int.Parse(orderObjectElement.Element("FactorSize").Value);
                        orderObject.objects = new List<Object>();

                        foreach (XElement objectElement in orderObjectElement.Elements("Object"))
                        {
                            Object obj = new Object();
                            obj.s_nOpenType = int.Parse(objectElement.Element("s_nOpenType").Value);
                            obj.s_nFactor = int.Parse(objectElement.Element("s_nFactor").Value);
                            obj.s_nFactorCnt = int.Parse(objectElement.Element("s_nFactorCnt").Value);

                            orderObject.objects.Add(obj);
                        }

                        mapSourceObject.Objects.Add(orderObject);
                    }

                    mapObject.mapSourceObjects.Add(mapSourceObject);
                }

                mapObjectsList.Add(mapObject);
            }

            return mapObjectsList.ToArray();
        }
        public static void WriteMapObjectToBinary(MapObject[] mapObjects, string filePath)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {
                writer.Write(mapObjects.Length);
                foreach (var mapObject in mapObjects)
                {
                    writer.Write(mapObject.MapId);
                    writer.Write(mapObject.Size);

                    foreach (MapSourceObject mapSourceObject in mapObject.mapSourceObjects)
                    {
                        writer.Write(mapSourceObject.ObjectId);

                        foreach (OrderObject orderObject in mapSourceObject.Objects)
                        {
                            writer.Write(orderObject.OrderId);
                            writer.Write(orderObject.FactorSize);

                            foreach (Object obj in orderObject.objects)
                            {
                                writer.Write(obj.s_nOpenType);
                                writer.Write(obj.s_nFactor);
                                writer.Write(obj.s_nFactorCnt);
                            }
                        }
                    }
                }
            }
        }
    }

    public class MapSourceObject
    {
        public int ObjectId;
        public List<OrderObject> Objects = new();

    }
    public class OrderObject
    {
        public int OrderId;
        public int FactorSize;
        public List<Object> objects = new();
    }
    public class Object
    {

        public int s_nOpenType;        // 0 : 사용안함, 1 : 몬스터 Kill, 2 : 퀘스트 완료
        public int s_nFactor;         // 타입에 따른 Factor( 타입1 : 몬스터코드, 타입2 : 퀘스트 코드 )
        public int s_nFactorCnt;		// ( 타입1 : 몬스터 수 )
    }
}
