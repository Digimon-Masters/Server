using System.Text;
using System.Xml.Linq;


namespace BinXmlConverter.Classes
{
    public class EventTotal
    {

        public class Atendence
        {
            public List<TimeSpan> Time = new();
        }

        public class TimeSpan
        {
            public int tm_sec;   // seconds after the minute - [0, 60] including leap second
            public int tm_min;   // minutes after the hour - [0, 59]
            public int tm_hour;  // hours since midnight - [0, 23]
            public int tm_mday;  // day of the month - [1, 31]
            public int tm_mon;   // months since January - [0, 11]
            public int tm_year;  // years since 1900
            public int tm_wday;  // days since Sunday - [0, 6]
            public int tm_yday;  // days since January 1 - [0, 365]
            public int tm_isdst; // daylight savings time flag
        }


        public class Event
        {
            public int s_TableNo;
            public int TimeInMinutes;
            public int NameSize;
            public string Name;
            public List<EventItems> EventItems = new();

        }
        public class EventItems
        {
            public int ItemId;
            public short ItemCount;

        }
        public class AttendenceEvent
        {
            public int Id;
            public int s_nUse;
            public int s_nIndex;
            public int s_nType;
            public int s_nSuccessType;
            public int s_nSuccessValue;
            public int s_nItemKind;
            public int NameSize;
            public string Name;
            public string EndTime;
            public List<EventItems> MensalItems = new();
        }
        public class MonthlyEvent
        {
            public int s_nTableNo;
            public string s_szMessage;
            public List<EventItems> MonthlyItems = new();
        }
        public class TimeEvent
        {
            public int Id;
            public int Unknow;
            public int ItemId;
            public short ItemCount;
            public short unknow;
            public int StartDateSize;
            public string StartDate;
            public int EndDateSize;
            public string EndDate;
            public int Day;
            public int StartTimeSize;
            public string StartTime;
            public int EndTimeSize;
            public string EndTime;

        }
        public class Event100Days
        {
            public int Id;
            public int EventSize;
            public string Event;
            public int EventTitleSize;
            public string EventTitle;
            public int EventDescriptSize;
            public string EventDescript;
            public int StartTimeSize;
            public string StartTime;
            public int EndTimeSize;
            public string EndTime;
            public int ResetSize;
            public string Reset;
            public List<CemItems> Events = new();

        }
        public class CemItems
        {
            public int ItemId;
            public short ItemCount;
            public short Unknow;
            public int NameSize;
            public string Name;

        }
        public class Jumping
        {
            public int Id;
            public int Id1;
            public List<JumpingStruct> JumpingStructs = new();

        }
        public class JumpingStruct
        {
            public int Id;
            public int NameSize;
            public string Name;
            public List<EventItems> MensalItems = new();
        }

        public static (Atendence[], Event[], AttendenceEvent[], MonthlyEvent[], TimeEvent[], Event100Days[]) ReadEventFromBinary(string filePath)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {

                Atendence[] atendences = ReadAttendence(reader);
                // Lê os eventos
                Event[] events = ReadEvents(reader);

                // Lê os eventos mensais
                AttendenceEvent[] mensalEvents = ReadMensalEvents(reader);

                MonthlyEvent[] monthlyEvents = ReadMonthlyEvent(reader);
                // Lê os eventos de tempo
                TimeEvent[] timeEvents = ReadTimeEvents(reader);

                // Lê os eventos de 100 dias
                Event100Days[] event100Days = ReadEvent100Days(reader);

                return (atendences, events, mensalEvents, monthlyEvents, timeEvents, event100Days);
            }
        }

        private static Atendence[] ReadAttendence(BinaryReader reader)
        {
            Atendence[] atendences = new Atendence[1];
            Atendence atendence = new();

            for (int x = 0; x < 2; x++)
            {
                TimeSpan timeSpan = new();
                timeSpan.tm_sec = reader.ReadInt32();
                timeSpan.tm_min = reader.ReadInt32();
                timeSpan.tm_hour = reader.ReadInt32();
                timeSpan.tm_mday = reader.ReadInt32();
                timeSpan.tm_mon = reader.ReadInt32();
                timeSpan.tm_year = reader.ReadInt32();
                timeSpan.tm_wday = reader.ReadInt32();
                timeSpan.tm_yday = reader.ReadInt32();
                timeSpan.tm_isdst = reader.ReadInt32();
                atendence.Time.Add(timeSpan);
            }
            atendences[0] = atendence;

            return atendences;
        }
        private static Event[] ReadEvents(BinaryReader reader)
        {
            // Lê a quantidade de eventos
            int eventCount = reader.ReadInt32();

            // Cria um array para armazenar os eventos
            Event[] events = new Event[eventCount];

            for (int i = 0; i < eventCount; i++)
            {
                // Cria uma nova instância de Event
                Event ev = new Event();

                // Lê os campos inteiros diretamente
                ev.s_TableNo = reader.ReadInt32();
                ev.TimeInMinutes = reader.ReadInt32();

                // Cria uma lista para armazenar os EventItems
                ev.EventItems = new List<EventItems>();

                for (int j = 0; j < 6; j++)
                {
                    // Cria uma nova instância de EventItems
                    EventItems item = new EventItems();

                    // Lê os campos ItemId e ItemCount diretamente
                    item.ItemId = reader.ReadInt32();

                    // Adiciona o EventItems à lista
                    ev.EventItems.Add(item);
                }

                foreach (var item in ev.EventItems)
                {
                    item.ItemCount = reader.ReadInt16();
                }

                byte[] nameBytes = reader.ReadBytes(1024);
                var NameString = System.Text.Encoding.ASCII.GetString(nameBytes, 0, 1024);
                ev.Name = CleanString(NameString);
                // Adiciona o evento ao array de eventos
                events[i] = ev;
            }

            return events;


            static string CleanString(string input)
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
        private static AttendenceEvent[] ReadMensalEvents(BinaryReader reader)
        {
            // Lê a quantidade de eventos mensais
            int mensalEventCount = reader.ReadInt32();

            // Cria um array para armazenar os eventos mensais
            AttendenceEvent[] mensalEvents = new AttendenceEvent[mensalEventCount];

            for (int i = 0; i < mensalEventCount; i++)
            {
                // Cria uma nova instância de MensalEvent
                AttendenceEvent mensalEv = new AttendenceEvent();

                // Lê os campos inteiros diretamente
                mensalEv.Id = reader.ReadInt32();
                mensalEv.s_nUse = reader.ReadInt32();
                mensalEv.s_nIndex = reader.ReadInt32();
                mensalEv.s_nType = reader.ReadInt32();
                mensalEv.s_nSuccessType = reader.ReadInt32();
                mensalEv.s_nSuccessValue = reader.ReadInt32();
                mensalEv.s_nItemKind = reader.ReadInt32();

                // Cria uma lista para armazenar os EventItems mensais
                mensalEv.MensalItems = new List<EventItems>();
                for (int j = 0; j < 6; j++)
                {
                    // Cria uma nova instância de EventItems
                    EventItems item = new EventItems();

                    // Lê os campos ItemId e ItemCount diretamente
                    item.ItemId = reader.ReadInt32();
                    // Adiciona o EventItems à lista
                    mensalEv.MensalItems.Add(item);
                }

                foreach (var item in mensalEv.MensalItems)
                {
                    item.ItemCount = reader.ReadInt16();
                }

                byte[] nameBytes = reader.ReadBytes(64);
                var Name = System.Text.Encoding.Unicode.GetString(nameBytes);



                byte[] nameBytees = reader.ReadBytes(64);
                var EndTime = System.Text.Encoding.Unicode.GetString(nameBytees);

                mensalEv.EndTime = CleanString(EndTime);
                mensalEv.Name = CleanString(Name);



                // Adiciona o evento mensal ao array de eventos mensais
                mensalEvents[i] = mensalEv;
            }

            static string CleanString(string input)
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

            return mensalEvents;
        }
        private static MonthlyEvent[] ReadMonthlyEvent(BinaryReader reader)
        {
            var MonthlyCount = reader.ReadInt32();
            MonthlyEvent[] monthlyEvents = new MonthlyEvent[MonthlyCount];
            for (int i = 0; i < MonthlyCount; i++)
            {
                MonthlyEvent monthlyEvent = new();
                monthlyEvent.s_nTableNo = reader.ReadInt32();
                byte[] nameBytees = reader.ReadBytes(1024);
                var EndTime = System.Text.Encoding.Unicode.GetString(nameBytees);
                monthlyEvent.s_szMessage = CleanString(EndTime);

                for (int w = 0; w < 28; w++)
                {
                    EventItems items = new();
                    items.ItemId = reader.ReadInt32();
                    monthlyEvent.MonthlyItems.Add(items);
                }

                foreach (var item in monthlyEvent.MonthlyItems)
                {
                    item.ItemCount = reader.ReadInt16();
                }

                monthlyEvents[i] = monthlyEvent;
            }


            static string CleanString(string input)
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

            return monthlyEvents;
        }
        private static TimeEvent[] ReadTimeEvents(BinaryReader reader)
        {
            // Lê a quantidade de eventos de tempo
            int timeEventCount = reader.ReadInt32();

            // Cria um array para armazenar os eventos de tempo
            TimeEvent[] timeEvents = new TimeEvent[timeEventCount];

            for (int i = 0; i < timeEventCount; i++)
            {
                // Cria uma nova instância de TimeEvent
                TimeEvent timeEv = new TimeEvent();

                // Lê os campos inteiros diretamente
                timeEv.Id = reader.ReadInt32();

                if (timeEv.Id == 0)
                    continue;

                timeEv.StartDateSize = reader.ReadInt32();
                // Lê a string StartDate com base no tamanho informado
                byte[] startDateBytes = reader.ReadBytes(timeEv.StartDateSize * 2);
                timeEv.StartDate = System.Text.Encoding.Unicode.GetString(startDateBytes);

                timeEv.EndDateSize = reader.ReadInt32();

                // Lê a string EndDate com base no tamanho informado
                byte[] endDateBytes = reader.ReadBytes(timeEv.EndDateSize * 2);
                timeEv.EndDate = System.Text.Encoding.Unicode.GetString(endDateBytes);

                timeEv.Day = reader.ReadInt32();
                timeEv.StartTimeSize = reader.ReadInt32();

                // Lê a string StartTime com base no tamanho informado
                byte[] startTimeBytes = reader.ReadBytes(timeEv.StartTimeSize * 2);
                timeEv.StartTime = System.Text.Encoding.Unicode.GetString(startTimeBytes);

                timeEv.EndTimeSize = reader.ReadInt32();

                // Lê a string EndTime com base no tamanho informado
                byte[] endTimeBytes = reader.ReadBytes(timeEv.EndTimeSize * 2);
                timeEv.EndTime = System.Text.Encoding.Unicode.GetString(endTimeBytes);

                timeEv.ItemId = reader.ReadInt32();
                timeEv.ItemCount = reader.ReadInt16();
                var ItemId1 = reader.ReadInt16();


                // Adiciona o evento de tempo ao array de eventos de tempo
                timeEvents[i] = timeEv;
            }

            return timeEvents;
        }
        private static Event100Days[] ReadEvent100Days(BinaryReader reader)
        {
            // Lê a quantidade de eventos de 100 dias
            int event100DaysCount = reader.ReadInt32();

            // Cria um array para armazenar os eventos de 100 dias
            Event100Days[] event100Days = new Event100Days[event100DaysCount];

            for (int i = 0; i < event100DaysCount; i++)
            {
                // Cria uma nova instância de Event100Days
                Event100Days event100DaysEv = new Event100Days();

                // Lê os campos inteiros diretamente
                event100DaysEv.Id = reader.ReadInt32();
                event100DaysEv.EventSize = reader.ReadInt32();

                // Lê a string Event com base no tamanho informado
                byte[] eventBytes = reader.ReadBytes(event100DaysEv.EventSize * 2);
                event100DaysEv.Event = System.Text.Encoding.Unicode.GetString(eventBytes);

                event100DaysEv.EventTitleSize = reader.ReadInt32();

                // Lê a string EventTitle com base no tamanho informado
                byte[] eventTitleBytes = reader.ReadBytes(event100DaysEv.EventTitleSize * 2);
                event100DaysEv.EventTitle = System.Text.Encoding.Unicode.GetString(eventTitleBytes);

                event100DaysEv.EventDescriptSize = reader.ReadInt32();

                // Lê a string EventDescript com base no tamanho informado
                byte[] eventDescriptBytes = reader.ReadBytes(event100DaysEv.EventDescriptSize * 2);
                event100DaysEv.EventDescript = System.Text.Encoding.Unicode.GetString(eventDescriptBytes);

                event100DaysEv.StartTimeSize = reader.ReadInt32();

                // Lê a string StartTime com base no tamanho informado
                byte[] startTimeBytes = reader.ReadBytes(event100DaysEv.StartTimeSize * 2);
                event100DaysEv.StartTime = System.Text.Encoding.Unicode.GetString(startTimeBytes);

                event100DaysEv.EndTimeSize = reader.ReadInt32();

                // Lê a string EndTime com base no tamanho informado
                byte[] endTimeBytes = reader.ReadBytes(event100DaysEv.EndTimeSize * 2);
                event100DaysEv.EndTime = System.Text.Encoding.Unicode.GetString(endTimeBytes);

                event100DaysEv.ResetSize = reader.ReadInt32();

                // Lê a string Reset com base no tamanho informado
                byte[] resetBytes = reader.ReadBytes(event100DaysEv.ResetSize * 2);
                event100DaysEv.Reset = System.Text.Encoding.Unicode.GetString(resetBytes);

                // Lê a quantidade de EventItems
                int itemCount = reader.ReadInt32();

                // Cria uma lista para armazenar os EventItems
                event100DaysEv.Events = new List<CemItems>();

                for (int j = 0; j < itemCount; j++)
                {
                    // Cria uma nova instância de EventItems
                    CemItems item = new CemItems();
                    item.NameSize = reader.ReadInt32();
                    byte[] NametBytes = reader.ReadBytes(item.NameSize * 2);
                    item.Name = System.Text.Encoding.Unicode.GetString(NametBytes);
                    // Lê os campos ItemId e ItemCount diretamente
                    item.ItemId = reader.ReadInt32();
                    item.ItemCount = reader.ReadInt16();
                    item.Unknow = reader.ReadInt16();
                    // Adiciona o EventItems à lista
                    event100DaysEv.Events.Add(item);
                }

                // Adiciona o evento de 100 dias ao array de eventos de 100 dias
                event100Days[i] = event100DaysEv;
            }

            return event100Days;
        }
        public static Jumping[] ReadJumpingFromBinary(BinaryReader reader)
        {
            // Lê os campos Id e Id1 diretamente
            int Id = reader.ReadInt32();
            int Id1 = reader.ReadInt32();

            // Cria um array para armazenar os objetos Jumping
            Jumping[] jumpings = new Jumping[2];

            for (int i = 0; i < 2; i++)
            {
                Jumping jumping = new Jumping();

                // Lê a quantidade de objetos JumpingStruct
                int jumpingCount = reader.ReadInt32();

                // Cria uma lista para armazenar os objetos JumpingStruct
                jumping.JumpingStructs = new List<JumpingStruct>();

                for (int x = 0; x < jumpingCount; x++)
                {
                    JumpingStruct jumpingStruct = new JumpingStruct();

                    // Lê os campos inteiros diretamente
                    jumpingStruct.Id = reader.ReadInt32();
                    jumpingStruct.NameSize = reader.ReadInt32();

                    // Lê a string Name com base no tamanho informado
                    byte[] nameBytes = reader.ReadBytes(jumpingStruct.NameSize * 2);
                    jumpingStruct.Name = System.Text.Encoding.Unicode.GetString(nameBytes);

                    // Lê a quantidade de EventItems mensais
                    int itemCount = reader.ReadInt32();

                    // Cria uma lista para armazenar os EventItems mensais
                    jumpingStruct.MensalItems = new List<EventItems>();

                    for (int k = 0; k < itemCount; k++)
                    {
                        EventItems item = new EventItems();

                        // Lê os campos ItemId e ItemCount diretamente
                        item.ItemId = reader.ReadInt32();
                        item.ItemCount = reader.ReadInt16();

                        // Adiciona o EventItems à lista
                        jumpingStruct.MensalItems.Add(item);
                    }

                    // Adiciona o objeto JumpingStruct à lista
                    jumping.JumpingStructs.Add(jumpingStruct);
                }

                // Adiciona o objeto Jumping ao array de Jumpings
                jumpings[i] = jumping;
            }

            return jumpings;

        }

        public static void ExportAttendenceToXml(Atendence[] atendences, string exportPath)
        {
            // Cria o elemento raiz do documento XML
            XElement rootElement = new XElement("Atendences");

            foreach (var atendence in atendences)
            {
                // Cria o elemento para cada atendimento
                XElement atendenceElement = new XElement("Atendence");

                // Cria o elemento para a lista de TimeSpans
                XElement timeSpansElement = new XElement("TimeSpans");

                foreach (var timeSpan in atendence.Time)
                {
                    // Cria o elemento para cada TimeSpan
                    XElement timeSpanElement = new XElement("TimeSpan");
                    timeSpanElement.Add(new XElement("tm_sec", timeSpan.tm_sec));
                    timeSpanElement.Add(new XElement("tm_min", timeSpan.tm_min));
                    timeSpanElement.Add(new XElement("tm_hour", timeSpan.tm_hour));
                    timeSpanElement.Add(new XElement("tm_mday", timeSpan.tm_mday));
                    timeSpanElement.Add(new XElement("tm_mon", timeSpan.tm_mon));
                    timeSpanElement.Add(new XElement("tm_year", timeSpan.tm_year));
                    timeSpanElement.Add(new XElement("tm_wday", timeSpan.tm_wday));
                    timeSpanElement.Add(new XElement("tm_yday", timeSpan.tm_yday));
                    timeSpanElement.Add(new XElement("tm_isdst", timeSpan.tm_isdst));

                    // Adiciona o elemento TimeSpan à lista
                    timeSpansElement.Add(timeSpanElement);
                }

                // Adiciona o elemento TimeSpans ao elemento Atendence
                atendenceElement.Add(timeSpansElement);

                // Adiciona o elemento Atendence ao elemento raiz
                rootElement.Add(atendenceElement);
            }

            // Cria o documento XML com o elemento raiz
            XDocument xmlDocument = new XDocument(rootElement);

            // Salva o documento XML no caminho especificado
            xmlDocument.Save(exportPath);
        }
        public static void ExportEventsToXml(Event[] events, string exportPath)
        {
            // Cria o elemento raiz do documento XML
            XElement rootElement = new XElement("Events");

            foreach (var ev in events)
            {
                // Cria o elemento para cada evento
                XElement eventElement = new XElement("Event");

                eventElement.Add(new XElement("s_TableNo", ev.s_TableNo));
                eventElement.Add(new XElement("TimeInMinutes", ev.TimeInMinutes));

                // Cria o elemento para a lista de EventItems
                XElement eventItemsElement = new XElement("EventItems");

                foreach (var item in ev.EventItems)
                {
                    // Cria o elemento para cada EventItems
                    XElement eventItemElement = new XElement("EventItem");
                    eventItemElement.Add(new XElement("ItemId", item.ItemId));
                    eventItemElement.Add(new XElement("ItemCount", item.ItemCount));

                    // Adiciona o elemento EventItem à lista
                    eventItemsElement.Add(eventItemElement);
                }

                // Adiciona o elemento EventItems ao elemento Event
                eventElement.Add(eventItemsElement);

                // Adiciona o elemento Name ao elemento Event
                eventElement.Add(new XElement("Name", ev.Name));

                // Adiciona o elemento Event ao elemento raiz
                rootElement.Add(eventElement);
            }

            // Cria o documento XML com o elemento raiz
            XDocument xmlDocument = new XDocument(rootElement);

            // Salva o documento XML no caminho especificado
            xmlDocument.Save(exportPath);
        }
        public static void ExportMensalEventToXml(AttendenceEvent[] mensalEvents, string exportPath)
        {
            // Cria o elemento raiz do documento XML
            XElement rootElement = new XElement("MensalEvents");

            foreach (var mensalEv in mensalEvents)
            {
                // Cria o elemento para cada evento mensal
                XElement mensalEventElement = new XElement("MensalEvent");

                // Adiciona os campos inteiros como elementos individuais
                mensalEventElement.Add(new XElement("Id", mensalEv.Id));
                mensalEventElement.Add(new XElement("s_nUse", mensalEv.s_nUse));
                mensalEventElement.Add(new XElement("s_nIndex", mensalEv.s_nIndex));
                mensalEventElement.Add(new XElement("s_nType", mensalEv.s_nType));
                mensalEventElement.Add(new XElement("s_nSuccessType", mensalEv.s_nSuccessType));
                mensalEventElement.Add(new XElement("s_nSuccessValue", mensalEv.s_nSuccessValue));
                mensalEventElement.Add(new XElement("s_nItemKind", mensalEv.s_nItemKind));

                // Cria o elemento para a lista de EventItems mensais
                XElement mensalItemsElement = new XElement("MensalItems");

                foreach (var item in mensalEv.MensalItems)
                {
                    // Cria o elemento para cada EventItems
                    XElement itemElement = new XElement("EventItems");
                    itemElement.Add(new XElement("ItemId", item.ItemId));
                    itemElement.Add(new XElement("ItemCount", item.ItemCount));

                    // Adiciona o elemento EventItems à lista
                    mensalItemsElement.Add(itemElement);
                }

                // Adiciona o elemento MensalItems ao elemento MensalEvent
                mensalEventElement.Add(mensalItemsElement);

                // Adiciona os campos Name e EndTime como elementos individuais
                mensalEventElement.Add(new XElement("Name", mensalEv.Name));
                mensalEventElement.Add(new XElement("EndTime", mensalEv.EndTime));

                // Adiciona o elemento MensalEvent ao elemento raiz
                rootElement.Add(mensalEventElement);
            }

            // Cria o documento XML com o elemento raiz
            XDocument xmlDocument = new XDocument(rootElement);

            // Salva o documento XML no caminho especificado
            xmlDocument.Save(exportPath);
        }
        public static void ExportMonthlyEventsToXml(MonthlyEvent[] monthlyEvents, string exportPath)
        {
            // Cria o elemento raiz do documento XML
            XElement rootElement = new XElement("MonthlyEvents");

            foreach (var monthlyEvent in monthlyEvents)
            {
                // Cria o elemento para cada evento mensal
                XElement monthlyEventElement = new XElement("MonthlyEvent");

                monthlyEventElement.Add(new XElement("s_nTableNo", monthlyEvent.s_nTableNo));
                monthlyEventElement.Add(new XElement("s_szMessage", monthlyEvent.s_szMessage));

                // Cria o elemento para a lista de MonthlyItems
                XElement monthlyItemsElement = new XElement("MonthlyItems");

                foreach (var item in monthlyEvent.MonthlyItems)
                {
                    // Cria o elemento para cada MonthlyItem
                    XElement monthlyItemElement = new XElement("MonthlyItem");
                    monthlyItemElement.Add(new XElement("ItemId", item.ItemId));
                    monthlyItemElement.Add(new XElement("ItemCount", item.ItemCount));

                    // Adiciona o elemento MonthlyItem à lista
                    monthlyItemsElement.Add(monthlyItemElement);
                }

                // Adiciona o elemento MonthlyItems ao elemento MonthlyEvent
                monthlyEventElement.Add(monthlyItemsElement);

                // Adiciona o elemento MonthlyEvent ao elemento raiz
                rootElement.Add(monthlyEventElement);
            }

            // Cria o documento XML com o elemento raiz
            XDocument xmlDocument = new XDocument(rootElement);

            // Salva o documento XML no caminho especificado
            xmlDocument.Save(exportPath);
        }
        public static void ExportTimeEventToXml(TimeEvent[] timeEvents, string exportPath)
        {
            // Cria o elemento raiz do documento XML
            XElement rootElement = new XElement("TimeEvents");

            foreach (var timeEvent in timeEvents)
            {
                // Cria o elemento para cada evento de tempo
                XElement timeEventElement = new XElement("TimeEvent");

                // Verifica se o Id é igual a 0 e pula para a próxima iteração caso seja
                if (timeEvent.Id == 0)
                    continue;

                timeEventElement.Add(new XElement("Id", timeEvent.Id));
                timeEventElement.Add(new XElement("StartDate", timeEvent.StartDate));
                timeEventElement.Add(new XElement("EndDate", timeEvent.EndDate));
                timeEventElement.Add(new XElement("Day", timeEvent.Day));
                timeEventElement.Add(new XElement("StartTime", timeEvent.StartTime));
                timeEventElement.Add(new XElement("EndTime", timeEvent.EndTime));
                timeEventElement.Add(new XElement("ItemId", timeEvent.ItemId));
                timeEventElement.Add(new XElement("ItemCount", timeEvent.ItemCount));

                // Adiciona o elemento do evento de tempo ao elemento raiz
                rootElement.Add(timeEventElement);
            }

            // Cria o documento XML com o elemento raiz
            XDocument xmlDocument = new XDocument(rootElement);

            // Salva o documento XML no caminho especificado
            xmlDocument.Save(exportPath);
        }
        public static void ExportEvent100DaysToXml(Event100Days[] event100Days, string exportPath)
        {
            // Cria o elemento raiz do documento XML
            XElement rootElement = new XElement("Event100Days");

            foreach (var event100DaysEv in event100Days)
            {
                // Cria o elemento para cada evento de 100 dias
                XElement event100DaysElement = new XElement("Event100Days");

                event100DaysElement.Add(new XElement("Id", event100DaysEv.Id));
                event100DaysElement.Add(new XElement("Event", event100DaysEv.Event));
                event100DaysElement.Add(new XElement("EventTitle", event100DaysEv.EventTitle));
                event100DaysElement.Add(new XElement("EventDescript", event100DaysEv.EventDescript));
                event100DaysElement.Add(new XElement("StartTime", event100DaysEv.StartTime));
                event100DaysElement.Add(new XElement("EndTime", event100DaysEv.EndTime));
                event100DaysElement.Add(new XElement("Reset", event100DaysEv.Reset));

                // Cria o elemento para a lista de EventItems
                XElement eventItemsElement = new XElement("EventItems");

                foreach (var item in event100DaysEv.Events)
                {
                    // Cria o elemento para cada EventItem
                    XElement eventItemElement = new XElement("EventItem");

                    eventItemElement.Add(new XElement("Name", item.Name));
                    eventItemElement.Add(new XElement("ItemId", item.ItemId));
                    eventItemElement.Add(new XElement("ItemCount", item.ItemCount));
                    eventItemElement.Add(new XElement("Unknow", item.Unknow));

                    // Adiciona o elemento do EventItem ao elemento da lista de EventItems
                    eventItemsElement.Add(eventItemElement);
                }

                // Adiciona o elemento da lista de EventItems ao elemento do evento de 100 dias
                event100DaysElement.Add(eventItemsElement);

                // Adiciona o elemento do evento de 100 dias ao elemento raiz
                rootElement.Add(event100DaysElement);
            }

            // Cria o documento XML com o elemento raiz
            XDocument xmlDocument = new XDocument(rootElement);

            // Salva o documento XML no caminho especificado
            xmlDocument.Save(exportPath);
        }
        public static Atendence[] ImportAttendenceFromXml(string importPath)
        {
            XDocument xmlDocument = XDocument.Load(importPath);
            XElement rootElement = xmlDocument.Root;
            List<Atendence> atendences = new List<Atendence>();

            foreach (XElement atendenceElement in rootElement.Elements("Atendence"))
            {
                Atendence atendence = new Atendence();

                XElement timeSpansElement = atendenceElement.Element("TimeSpans");

                foreach (XElement timeElement in timeSpansElement.Elements("TimeSpan"))
                {
                    TimeSpan timeSpan = new TimeSpan();
                    timeSpan.tm_sec = Convert.ToInt32(timeElement.Element("tm_sec").Value);
                    timeSpan.tm_min = Convert.ToInt32(timeElement.Element("tm_min").Value);
                    timeSpan.tm_hour = Convert.ToInt32(timeElement.Element("tm_hour").Value);
                    timeSpan.tm_mday = Convert.ToInt32(timeElement.Element("tm_mday").Value);
                    timeSpan.tm_mon = Convert.ToInt32(timeElement.Element("tm_mon").Value);
                    timeSpan.tm_year = Convert.ToInt32(timeElement.Element("tm_year").Value);
                    timeSpan.tm_wday = Convert.ToInt32(timeElement.Element("tm_wday").Value);
                    timeSpan.tm_yday = Convert.ToInt32(timeElement.Element("tm_yday").Value);
                    timeSpan.tm_isdst = Convert.ToInt32(timeElement.Element("tm_isdst").Value);

                    atendence.Time.Add(timeSpan);
                }

                atendences.Add(atendence);
            }

            return atendences.ToArray();
        }
        public static Event[] ImportEventsFromXml(string importPath)
        {
            XDocument xmlDocument = XDocument.Load(importPath);
            XElement rootElement = xmlDocument.Root;
            List<Event> events = new List<Event>();

            foreach (XElement eventElement in rootElement.Elements("Event"))
            {
                Event ev = new Event();
                ev.s_TableNo = Convert.ToInt32(eventElement.Element("s_TableNo").Value);
                ev.TimeInMinutes = Convert.ToInt32(eventElement.Element("TimeInMinutes").Value);

                List<EventItems> eventItems = new List<EventItems>();
                foreach (XElement eventItemElement in eventElement.Element("EventItems").Elements("EventItem"))
                {
                    EventItems item = new EventItems();
                    item.ItemId = Convert.ToInt32(eventItemElement.Element("ItemId").Value);
                    item.ItemCount = Convert.ToInt16(eventItemElement.Element("ItemCount").Value);

                    eventItems.Add(item);
                }

                ev.EventItems = eventItems;
                ev.Name = eventElement.Element("Name").Value;

                events.Add(ev);
            }

            return events.ToArray();
        }
        public static AttendenceEvent[] ImportMensalEventsFromXml(string importPath)
        {
            XDocument xmlDocument = XDocument.Load(importPath);
            XElement rootElement = xmlDocument.Root;
            List<AttendenceEvent> mensalEvents = new List<AttendenceEvent>();

            foreach (XElement mensalEventElement in rootElement.Elements("MensalEvent"))
            {
                AttendenceEvent mensalEv = new AttendenceEvent();
                mensalEv.Id = Convert.ToInt32(mensalEventElement.Element("Id").Value);
                mensalEv.s_nUse = Convert.ToInt32(mensalEventElement.Element("s_nUse").Value);
                mensalEv.s_nIndex = Convert.ToInt32(mensalEventElement.Element("s_nIndex").Value);
                mensalEv.s_nType = Convert.ToInt32(mensalEventElement.Element("s_nType").Value);
                mensalEv.s_nSuccessType = Convert.ToInt32(mensalEventElement.Element("s_nSuccessType").Value);
                mensalEv.s_nSuccessValue = Convert.ToInt32(mensalEventElement.Element("s_nSuccessValue").Value);
                mensalEv.s_nItemKind = Convert.ToInt32(mensalEventElement.Element("s_nItemKind").Value);

                List<EventItems> mensalItems = new List<EventItems>();
                foreach (XElement itemElement in mensalEventElement.Element("MensalItems").Elements("EventItems"))
                {
                    EventItems item = new EventItems();
                    item.ItemId = Convert.ToInt32(itemElement.Element("ItemId").Value);
                    item.ItemCount = Convert.ToInt16(itemElement.Element("ItemCount").Value);

                    mensalItems.Add(item);
                }

                mensalEv.MensalItems = mensalItems;
                mensalEv.Name = mensalEventElement.Element("Name").Value;
                mensalEv.EndTime = mensalEventElement.Element("EndTime").Value;

                mensalEvents.Add(mensalEv);
            }

            return mensalEvents.ToArray();
        }
        public static MonthlyEvent[] ImportMonthlyEventsFromXml(string importPath)
        {
            XDocument xmlDocument = XDocument.Load(importPath);
            XElement rootElement = xmlDocument.Root;
            List<MonthlyEvent> monthlyEvents = new List<MonthlyEvent>();

            foreach (XElement monthlyEventElement in rootElement.Elements("MonthlyEvent"))
            {
                MonthlyEvent monthlyEvent = new MonthlyEvent();
                monthlyEvent.s_nTableNo = Convert.ToInt32(monthlyEventElement.Element("s_nTableNo").Value);
                monthlyEvent.s_szMessage = monthlyEventElement.Element("s_szMessage").Value;

                List<EventItems> monthlyItems = new List<EventItems>();
                foreach (XElement monthlyItemElement in monthlyEventElement.Element("MonthlyItems").Elements("MonthlyItem"))
                {
                    EventItems item = new EventItems();
                    item.ItemId = Convert.ToInt32(monthlyItemElement.Element("ItemId").Value);
                    item.ItemCount = Convert.ToInt16(monthlyItemElement.Element("ItemCount").Value);

                    monthlyItems.Add(item);
                }

                monthlyEvent.MonthlyItems = monthlyItems;
                monthlyEvents.Add(monthlyEvent);
            }

            return monthlyEvents.ToArray();
        }
        public static TimeEvent[] ImportTimeEventsFromXml(string importPath)
        {
            XDocument xmlDocument = XDocument.Load(importPath);
            XElement rootElement = xmlDocument.Root;
            List<TimeEvent> timeEvents = new List<TimeEvent>();

            foreach (XElement timeEventElement in rootElement.Elements("TimeEvent"))
            {
                TimeEvent timeEv = new TimeEvent();
                timeEv.Id = Convert.ToInt32(timeEventElement.Element("Id").Value);
                timeEv.StartDate = timeEventElement.Element("StartDate").Value;
                timeEv.EndDate = timeEventElement.Element("EndDate").Value;
                timeEv.Day = Convert.ToInt32(timeEventElement.Element("Day").Value);
                timeEv.StartTime = timeEventElement.Element("StartTime").Value;
                timeEv.EndTime = timeEventElement.Element("EndTime").Value;
                timeEv.ItemId = Convert.ToInt32(timeEventElement.Element("ItemId").Value);
                timeEv.ItemCount = Convert.ToInt16(timeEventElement.Element("ItemCount").Value);

                timeEvents.Add(timeEv);
            }

            return timeEvents.ToArray();
        }
        public static Event100Days[] ImportEvent100DaysFromXml(string importPath)
        {
            XDocument xmlDocument = XDocument.Load(importPath);
            XElement rootElement = xmlDocument.Root;
            List<Event100Days> event100Days = new List<Event100Days>();

            foreach (XElement event100DaysElement in rootElement.Elements("Event100Days"))
            {
                Event100Days event100DaysEv = new Event100Days();
                event100DaysEv.Id = Convert.ToInt32(event100DaysElement.Element("Id").Value);
                event100DaysEv.Event = event100DaysElement.Element("Event").Value;
                event100DaysEv.EventTitle = event100DaysElement.Element("EventTitle").Value;
                event100DaysEv.EventDescript = event100DaysElement.Element("EventDescript").Value;
                event100DaysEv.StartTime = event100DaysElement.Element("StartTime").Value;
                event100DaysEv.EndTime = event100DaysElement.Element("EndTime").Value;
                event100DaysEv.Reset = event100DaysElement.Element("Reset").Value;

                List<CemItems> eventItems = new List<CemItems>();
                foreach (XElement eventItemElement in event100DaysElement.Element("EventItems").Elements("EventItem"))
                {
                    CemItems item = new CemItems();
                    item.Name = eventItemElement.Element("Name").Value;
                    item.ItemId = Convert.ToInt32(eventItemElement.Element("ItemId").Value);
                    item.ItemCount = Convert.ToInt16(eventItemElement.Element("ItemCount").Value);
                    item.Unknow = Convert.ToInt16(eventItemElement.Element("Unknow").Value);

                    eventItems.Add(item);
                }

                event100DaysEv.Events = eventItems;
                event100Days.Add(event100DaysEv);
            }

            return event100Days.ToArray();
        }
        public static void ExportJumpingToXml(Jumping[] jumpings, string filePath)
        {
            XElement rootElement = new XElement("Jumpings");

            foreach (Jumping jumping in jumpings)
            {
                XElement jumpingElement = new XElement("Jumping",
                    new XElement("Id", jumping.Id),
                    new XElement("Id1", jumping.Id1)
                );

                foreach (JumpingStruct jumpingStruct in jumping.JumpingStructs)
                {
                    XElement jumpingStructElement = new XElement("JumpingStruct",
                        new XElement("Id", jumpingStruct.Id),
                        new XElement("NameSize", jumpingStruct.NameSize),
                        new XElement("Name", jumpingStruct.Name)
                    );

                    XElement mensalItemsElement = new XElement("MensalItems");

                    foreach (EventItems item in jumpingStruct.MensalItems)
                    {
                        XElement itemElement = new XElement("EventItems",
                            new XElement("ItemId", item.ItemId),
                            new XElement("ItemCount", item.ItemCount)
                        );

                        mensalItemsElement.Add(itemElement);
                    }

                    jumpingStructElement.Add(mensalItemsElement);
                    jumpingElement.Add(jumpingStructElement);
                }

                rootElement.Add(jumpingElement);
            }

            XDocument xmlDocument = new XDocument(rootElement);
            xmlDocument.Save(filePath);
        }
        public static Jumping[] ImportJumpingFromXml(string filePath)
        {
            XDocument xmlDocument = XDocument.Load(filePath);
            XElement rootElement = xmlDocument.Root;

            List<Jumping> jumpings = new List<Jumping>();

            foreach (XElement jumpingElement in rootElement.Elements("Jumping"))
            {
                Jumping jumping = new Jumping
                {
                    Id = int.Parse(jumpingElement.Element("Id").Value),
                    Id1 = int.Parse(jumpingElement.Element("Id1").Value)
                };

                List<JumpingStruct> jumpingStructs = new List<JumpingStruct>();

                foreach (XElement jumpingStructElement in jumpingElement.Elements("JumpingStruct"))
                {
                    JumpingStruct jumpingStruct = new JumpingStruct
                    {
                        Id = int.Parse(jumpingStructElement.Element("Id").Value),
                        NameSize = int.Parse(jumpingStructElement.Element("NameSize").Value),
                        Name = jumpingStructElement.Element("Name").Value
                    };

                    List<EventItems> mensalItems = new List<EventItems>();

                    foreach (XElement itemElement in jumpingStructElement.Element("MensalItems").Elements("EventItems"))
                    {
                        EventItems item = new EventItems
                        {
                            ItemId = int.Parse(itemElement.Element("ItemId").Value),
                            ItemCount = short.Parse(itemElement.Element("ItemCount").Value)
                        };

                        mensalItems.Add(item);
                    }

                    jumpingStruct.MensalItems = mensalItems;
                    jumpingStructs.Add(jumpingStruct);
                }

                jumping.JumpingStructs = jumpingStructs;
                jumpings.Add(jumping);
            }

            return jumpings.ToArray();
        }

        public static void WriteEventToBinary(string filePath, Atendence[] attendences, MonthlyEvent[] monthlyEvents, Event[] events, AttendenceEvent[] mensalEvents, TimeEvent[] timeEvents, Event100Days[] event100DaysList)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {


                foreach (var attendence in attendences)
                {
                    foreach (var timeSpan in attendence.Time)
                    {
                        writer.Write(timeSpan.tm_sec);
                        writer.Write(timeSpan.tm_min);
                        writer.Write(timeSpan.tm_hour);
                        writer.Write(timeSpan.tm_mday);
                        writer.Write(timeSpan.tm_mon);
                        writer.Write(timeSpan.tm_year);
                        writer.Write(timeSpan.tm_wday);
                        writer.Write(timeSpan.tm_yday);
                        writer.Write(timeSpan.tm_isdst);
                    }
                }

                // Escreve a contagem de objetos para cada classe
                writer.Write(events.Length);
                // Escreve os objetos de Event
                foreach (Event evt in events)
                {
                    writer.Write(evt.s_TableNo);
                    writer.Write(evt.TimeInMinutes);


                    foreach (EventItems item in evt.EventItems)
                    {
                        writer.Write(item.ItemId);
                    }

                    foreach (EventItems eventItems in evt.EventItems)
                    {
                        writer.Write(eventItems.ItemCount);
                    }

                    for (int i = 0; i < 512; i++)
                    {
                        char c = i < evt.Name.Length ? evt.Name[i] : '\0';
                        writer.Write((ushort)c);
                    }

                }

                // Escreve os objetos de MensalEvent
                writer.Write(mensalEvents.Length);
                foreach (AttendenceEvent mensalEvent in mensalEvents)
                {
                    // Lê os campos inteiros diretamente
                    writer.Write(mensalEvent.Id);
                    writer.Write(mensalEvent.s_nUse);
                    writer.Write(mensalEvent.s_nIndex);
                    writer.Write(mensalEvent.s_nType);
                    writer.Write(mensalEvent.s_nSuccessType);
                    writer.Write(mensalEvent.s_nSuccessValue);
                    writer.Write(mensalEvent.s_nItemKind);

                    foreach (EventItems item in mensalEvent.MensalItems)
                    {
                        writer.Write(item.ItemId);
                    }

                    foreach (EventItems item in mensalEvent.MensalItems)
                    {
                        writer.Write(item.ItemCount);
                    }


                    for (int i = 0; i < 32; i++)
                    {
                        char c = i < mensalEvent.Name.Length ? mensalEvent.Name[i] : '\0';
                        writer.Write((ushort)c);
                    }


                    for (int i = 0; i < 32; i++)
                    {
                        char c = i < mensalEvent.EndTime.Length ? mensalEvent.EndTime[i] : '\0';
                        writer.Write((ushort)c);
                    }
                }

                writer.Write(monthlyEvents.Length);
                foreach (var monthlyEvent in monthlyEvents)
                {

                    writer.Write(monthlyEvent.s_nTableNo);

                    for (int i = 0; i < 512; i++)
                    {
                        char c = i < monthlyEvent.s_szMessage.Length ? monthlyEvent.s_szMessage[i] : '\0';
                        writer.Write((ushort)c);
                    }

                    foreach (var item in monthlyEvent.MonthlyItems)
                    {
                        writer.Write(item.ItemId);
                    }
                    foreach (var item in monthlyEvent.MonthlyItems)
                    {
                        writer.Write(item.ItemCount);
                    }

                }

                // Escreve os objetos de TimeEvent
                writer.Write(timeEvents.Length);
                foreach (TimeEvent timeEvent in timeEvents)
                {
                    writer.Write(timeEvent.Id);


                    if (timeEvent.Id == 0)
                        continue;

                    // Converte a string StartDate para formato Unicode
                    byte[] startDateBytes = Encoding.Unicode.GetBytes(timeEvent.StartDate);

                    // Escreve a quantidade de caracteres da string StartDate (wchar_t)
                    writer.Write(startDateBytes.Length / 2);

                    // Escreve os bytes da string StartDate
                    writer.Write(startDateBytes);

                    // Converte a string EndDate para formato Unicode
                    byte[] endDateBytes = Encoding.Unicode.GetBytes(timeEvent.EndDate);

                    // Escreve a quantidade de caracteres da string EndDate (wchar_t)
                    writer.Write(endDateBytes.Length / 2);

                    // Escreve os bytes da string EndDate
                    writer.Write(endDateBytes);

                    writer.Write(timeEvent.Day);

                    // Converte a string StartTime para formato Unicode
                    byte[] startTimeBytes = Encoding.Unicode.GetBytes(timeEvent.StartTime);

                    // Escreve a quantidade de caracteres da string StartTime (wchar_t)
                    writer.Write(startTimeBytes.Length / 2);

                    // Escreve os bytes da string StartTime
                    writer.Write(startTimeBytes);

                    // Converte a string EndTime para formato Unicode
                    byte[] endTimeBytes = Encoding.Unicode.GetBytes(timeEvent.EndTime);

                    // Escreve a quantidade de caracteres da string EndTime (wchar_t)
                    writer.Write(endTimeBytes.Length / 2);

                    // Escreve os bytes da string EndTime
                    writer.Write(endTimeBytes);

                    writer.Write(timeEvent.ItemId);
                    writer.Write(timeEvent.ItemCount);
                    writer.Write(timeEvent.unknow);

                }

                // Escreve os objetos de Event100Days
                writer.Write(event100DaysList.Length);
                foreach (Event100Days event100Days in event100DaysList)
                {
                    writer.Write(event100Days.Id);

                    // Converte a string Event para formato Unicode
                    byte[] eventBytes = Encoding.Unicode.GetBytes(event100Days.Event);

                    // Escreve a quantidade de caracteres da string Event (wchar_t)
                    writer.Write(eventBytes.Length / 2);

                    // Escreve os bytes da string Event
                    writer.Write(eventBytes);

                    // Repita o processo para as strings EventTitle, EventDescript, StartTime, EndTime e Reset
                    // Converte a string EventTitle para formato Unicode
                    byte[] eventTitleBytes = Encoding.Unicode.GetBytes(event100Days.EventTitle);

                    // Escreve a quantidade de caracteres da string EventTitle (wchar_t)
                    writer.Write(eventTitleBytes.Length / 2);

                    // Escreve os bytes da string EventTitle
                    writer.Write(eventTitleBytes);

                    // Converte a string EventDescript para formato Unicode
                    byte[] eventDescriptBytes = Encoding.Unicode.GetBytes(event100Days.EventDescript);

                    // Escreve a quantidade de caracteres da string EventDescript (wchar_t)
                    writer.Write(eventDescriptBytes.Length / 2);

                    // Escreve os bytes da string EventDescript
                    writer.Write(eventDescriptBytes);

                    // Converte a string StartTime para formato Unicode
                    byte[] startTimeBytes = Encoding.Unicode.GetBytes(event100Days.StartTime);

                    // Escreve a quantidade de caracteres da string StartTime (wchar_t)
                    writer.Write(startTimeBytes.Length / 2);

                    // Escreve os bytes da string StartTime
                    writer.Write(startTimeBytes);

                    // Converte a string EndTime para formato Unicode
                    byte[] endTimeBytes = Encoding.Unicode.GetBytes(event100Days.EndTime);

                    // Escreve a quantidade de caracteres da string EndTime (wchar_t)
                    writer.Write(endTimeBytes.Length / 2);

                    // Escreve os bytes da string EndTime
                    writer.Write(endTimeBytes);

                    // Converte a string Reset para formato Unicode
                    byte[] resetBytes = Encoding.Unicode.GetBytes(event100Days.Reset);

                    // Escreve a quantidade de caracteres da string Reset (wchar_t)
                    writer.Write(resetBytes.Length / 2);

                    // Escreve os bytes da string Reset
                    writer.Write(resetBytes);

                    writer.Write(event100Days.Events.Count);
                    foreach (CemItems item in event100Days.Events)
                    {
                        // Converte a string Name para formato Unicode
                        byte[] nameBytes = Encoding.Unicode.GetBytes(item.Name);

                        // Escreve a quantidade de caracteres da string Name (wchar_t)
                        writer.Write(nameBytes.Length / 2);

                        // Escreve os bytes da string Name
                        writer.Write(nameBytes);
                        writer.Write(item.ItemId);
                        writer.Write(item.ItemCount);
                        writer.Write(item.Unknow);


                    }
                }

                //writer.Write(0);
                //writer.Write(0);
                //foreach (Jumping jumping in jumpings)
                //{
                //    // Escreve a quantidade de objetos JumpingStruct
                //    writer.Write(jumping.JumpingStructs.Count);

                //    foreach (JumpingStruct jumpingStruct in jumping.JumpingStructs)
                //    {
                //        writer.Write(jumpingStruct.Id);

                //        // Converte a string Name para formato Unicode
                //        byte[] nameBytes = Encoding.Unicode.GetBytes(jumpingStruct.Name);

                //        // Escreve a quantidade de caracteres da string Name (wchar_t)
                //        writer.Write(nameBytes.Length / 2);

                //        // Escreve os bytes da string Name
                //        writer.Write(nameBytes);

                //        // Escreve a quantidade de objetos EventItems
                //        writer.Write(jumpingStruct.MensalItems.Count);

                //        foreach (EventItems item in jumpingStruct.MensalItems)
                //        {
                //            writer.Write(item.ItemId);
                //            writer.Write(item.ItemCount);

                //            // Repita o processo para as strings Name
                //        }
                //    }

            }
        }
    }
}
