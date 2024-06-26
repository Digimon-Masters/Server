using DigitalWorldOnline.Commons.Enums;
using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.Writers;
using System.Diagnostics;

namespace DigitalWorldOnline.Commons.Utils
{
    //TODO: Separar utils de extensions
    public static class UtilitiesFunctions
    {
        public static List<short> DungeonMapIds = new List<short>()
        {
            17, 13, 20, 50, 210, 215, 252, 1110, 1308, 1311,
            1600, 1601, 1602, 1603, 1604, 1605, 1606, 1607, 1608,
            1609, 1610, 1611, 1612, 1613, 1614,2001,2002
        };

        public static List<int> IncreasePerLevelStun = new List<int>()
        {
            7501411,7500811,7500511
        };

        private static readonly Random _random = new();

        public class fPos
        {
            public int x;
            public int y;

            public fPos()
            {
                x = 0;
                y = 0;
            }

            public fPos(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public void Set(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public void Set(fPos other)
            {
                x = other.x;
                y = other.y;
            }

            public static fPos operator -(fPos a, fPos b)
            {
                return new fPos(a.x - b.x, a.y - b.y);
            }

            public static fPos operator +(fPos a, fPos b)
            {
                return new fPos(a.x + b.x, a.y + b.y);
            }

            public static fPos operator *(fPos a, int factor)
            {
                return new fPos(a.x * factor, a.y * factor);
            }

            public static fPos operator /(fPos a, int divisor)
            {
                if (divisor == 0)
                    return new fPos(0, 0);

                return new fPos(a.x / divisor, a.y / divisor);
            }

            public int Length()
            {
                return (int)Math.Sqrt(x * x + y * y);
            }

            public int Unitize()
            {
                int length = Length();
                if (length > 1e-06)
                {
                    int recip = 1 / length;
                    x *= recip;
                    y *= recip;
                }
                else
                {
                    x = 0;
                    y = 0;
                    length = 0;
                }
                return length;
            }
        }
        public static byte[] GroupPackets(params byte[][] packets)
        {
            var resultArray = new byte[packets.Sum(a => a.Length)];

            var offset = 0;

            foreach (var packet in packets)
            {
                Buffer.BlockCopy(packet, 0, resultArray, offset, packet.Length);
                offset += packet.Length;
            }

            return resultArray;
        }
        
        public static ItemListMovimentationEnum SwitchItemList(int originSlot, int destinationSlot)
        {
            if (originSlot.IsBetween(GeneralSizeEnum.InventoryMinSlot, GeneralSizeEnum.InventoryMaxSlot)
                &&
                destinationSlot.IsBetween(GeneralSizeEnum.InventoryMinSlot, GeneralSizeEnum.InventoryMaxSlot))
            {
                return ItemListMovimentationEnum.InventoryToInventory;
            }
            else if (originSlot.IsBetween(GeneralSizeEnum.InventoryMinSlot, GeneralSizeEnum.InventoryMaxSlot)
                &&
                destinationSlot.IsBetween(GeneralSizeEnum.EquipmentMinSlot, GeneralSizeEnum.EquipmentMaxSlot))
            {
                return ItemListMovimentationEnum.InventoryToEquipment;
            }
            else if (originSlot.IsBetween(GeneralSizeEnum.InventoryMinSlot, GeneralSizeEnum.InventoryMaxSlot)
                &&
                destinationSlot.IsBetween(GeneralSizeEnum.WarehouseMinSlot, GeneralSizeEnum.WarehouseMaxSlot))
            {
                return ItemListMovimentationEnum.InventoryToWarehouse;
            }
            else if (originSlot.IsBetween(GeneralSizeEnum.InventoryMinSlot, GeneralSizeEnum.InventoryMaxSlot)
                &&
                destinationSlot.IsBetween(GeneralSizeEnum.AccountWarehouseMinSlot, GeneralSizeEnum.AccountWarehouseMaxSlot))
            {
                return ItemListMovimentationEnum.InventoryToAccountWarehouse;
            }
            else if (originSlot.IsBetween(GeneralSizeEnum.EquipmentMinSlot, GeneralSizeEnum.EquipmentMaxSlot)
                &&
                destinationSlot.IsBetween(GeneralSizeEnum.InventoryMinSlot, GeneralSizeEnum.InventoryMaxSlot))
            {
                return ItemListMovimentationEnum.EquipmentToInventory;
            }
            else if (originSlot.IsBetween(GeneralSizeEnum.WarehouseMinSlot, GeneralSizeEnum.WarehouseMaxSlot)
                &&
                destinationSlot.IsBetween(GeneralSizeEnum.WarehouseMinSlot, GeneralSizeEnum.WarehouseMaxSlot))
            {
                return ItemListMovimentationEnum.WarehouseToWarehouse;
            }
            else if (originSlot.IsBetween(GeneralSizeEnum.WarehouseMinSlot, GeneralSizeEnum.WarehouseMaxSlot)
                &&
                destinationSlot.IsBetween(GeneralSizeEnum.InventoryMinSlot, GeneralSizeEnum.InventoryMaxSlot))
            {
                return ItemListMovimentationEnum.WarehouseToInventory;
            }
            else if (originSlot.IsBetween(GeneralSizeEnum.WarehouseMinSlot, GeneralSizeEnum.WarehouseMaxSlot)
                &&
                destinationSlot.IsBetween(GeneralSizeEnum.AccountWarehouseMinSlot, GeneralSizeEnum.AccountWarehouseMaxSlot))
            {
                return ItemListMovimentationEnum.WarehouseToAccountWarehouse;
            }
            else if (originSlot.IsBetween(GeneralSizeEnum.AccountWarehouseMinSlot, GeneralSizeEnum.AccountWarehouseMaxSlot)
                &&
                destinationSlot.IsBetween(GeneralSizeEnum.AccountWarehouseMinSlot, GeneralSizeEnum.AccountWarehouseMaxSlot))
            {
                return ItemListMovimentationEnum.AccountWarehouseToAccountWarehouse;
            }
            else if (originSlot.IsBetween(GeneralSizeEnum.AccountWarehouseMinSlot, GeneralSizeEnum.AccountWarehouseMaxSlot)
                &&
                destinationSlot.IsBetween(GeneralSizeEnum.InventoryMinSlot, GeneralSizeEnum.InventoryMaxSlot))
            {
                return ItemListMovimentationEnum.AccountWarehouseToInventory;
            }
            else if (originSlot.IsBetween(GeneralSizeEnum.AccountWarehouseMinSlot, GeneralSizeEnum.AccountWarehouseMaxSlot)
                &&
                destinationSlot.IsBetween(GeneralSizeEnum.WarehouseMinSlot, GeneralSizeEnum.WarehouseMaxSlot))
            {
                return ItemListMovimentationEnum.AccountWarehouseToWarehouse;
            }
            else if (originSlot.IsBetween(GeneralSizeEnum.InventoryMinSlot, GeneralSizeEnum.InventoryMaxSlot)
                &&
                destinationSlot == GeneralSizeEnum.XaiSlot.GetHashCode())
            {
                return ItemListMovimentationEnum.InventoryToEquipment;
            }
            else if (originSlot == GeneralSizeEnum.XaiSlot.GetHashCode()
                &&
                destinationSlot.IsBetween(GeneralSizeEnum.InventoryMinSlot, GeneralSizeEnum.InventoryMaxSlot))
            {
                return ItemListMovimentationEnum.EquipmentToInventory;
            }
            else if (originSlot == GeneralSizeEnum.DigiviceSlot.GetHashCode()
                &&
                destinationSlot.IsBetween(GeneralSizeEnum.InventoryMinSlot, GeneralSizeEnum.InventoryMaxSlot))
            {
                return ItemListMovimentationEnum.DigiviceToInventory;
            }
            else if (originSlot.IsBetween(GeneralSizeEnum.InventoryMinSlot, GeneralSizeEnum.InventoryMaxSlot)
                &&
                destinationSlot == GeneralSizeEnum.DigiviceSlot.GetHashCode())
            {
                return ItemListMovimentationEnum.InventoryToDigivice;
            }
            else if (originSlot.IsBetween(GeneralSizeEnum.InventoryMinSlot, GeneralSizeEnum.InventoryMaxSlot)
                &&
                destinationSlot.IsBetween(GeneralSizeEnum.ChipsetMinSlot, GeneralSizeEnum.ChipsetMaxSlot))
            {
                return ItemListMovimentationEnum.InventoryToChipset;
            }
            else if (originSlot.IsBetween(GeneralSizeEnum.ChipsetMinSlot, GeneralSizeEnum.ChipsetMaxSlot)
                &&
                destinationSlot.IsBetween(GeneralSizeEnum.InventoryMinSlot, GeneralSizeEnum.InventoryMaxSlot))
            {
                return ItemListMovimentationEnum.ChipsetToInventory;
            }
            else
            {
                return ItemListMovimentationEnum.InvalidMovimentation;
            }
        }

        public static int RemainingTimeSeconds(int seconds)
        {
            return (int)DateTimeOffset.Now
                .AddSeconds(DateTime.Now
                .AddSeconds(seconds)
                .Subtract(DateTime.Now).TotalSeconds)
                .ToUnixTimeSeconds();
        }

        public static int RemainingTimeMinutes(int minutes)
        {
           

            if (minutes == 0)
                return 0;

            return (int)DateTimeOffset.UtcNow.AddMinutes(minutes).ToUnixTimeSeconds();
        }

      

        public static long CurrentRemainingTimeToResetDay()
        {
            // Obter o próximo reset time para o mesmo dia
            var nextResetTime = DateTime.Today.AddDays(1) - DateTime.Now;

            // Calcular e retornar o Unix timestamp do próximo reset
            return DateTimeOffset.UtcNow.Add(nextResetTime).ToUnixTimeSeconds();
        }

        public static long CurrentRemainingTimeToResetHour()
        {
            var hourlyResetTime = DateTimeOffset.UtcNow
          .AddSeconds(DateTime.Now
              .AddMinutes(60 - DateTime.Now.Minute)
              .Subtract(DateTime.Now)
              .TotalSeconds
          ).ToUnixTimeSeconds();

            return hourlyResetTime;
        }
        public static int GetUtcSeconds(this DateTime? dateTime)
        {
            if (dateTime == null)
                return 0;
            else
                return
                (int)DateTimeOffset.UtcNow.AddSeconds(
                        dateTime.Value.Subtract(DateTime.Now).TotalSeconds
                    ).ToUnixTimeSeconds();
        }

        public static byte GetNewChannel(this IEnumerable<byte> currentChannels)
        {
            for (byte i = 0; i <= 15; i++)
            {
                if (!currentChannels.Contains(i))
                    return i;
            }

            return 16;
        }

        public static byte GetChannelLoad(this byte playerCount)
        {
            if (playerCount >= 0 && playerCount < 28)
                return (byte)ChannelLoadEnum.Empty;
            else if (playerCount >= 28 && playerCount < 56)
                return (byte)ChannelLoadEnum.TwentyPercent;
            else if (playerCount >= 56 && playerCount < 84)
                return (byte)ChannelLoadEnum.ThirtyPercent;
            else if (playerCount >= 84 && playerCount < 112)
                return (byte)ChannelLoadEnum.FourtyPercent;
            else if (playerCount >= 112 && playerCount < 140)
                return (byte)ChannelLoadEnum.FiftyPercent;
            else if (playerCount >= 140 && playerCount < 168)
                return (byte)ChannelLoadEnum.SixtyPercent;
            else if (playerCount >= 168 && playerCount < 196)
                return (byte)ChannelLoadEnum.SeventyPercent;
            else if (playerCount >= 196 && playerCount < 224)
                return (byte)ChannelLoadEnum.EightyPercent;
            else if (playerCount >= 224 && playerCount < 252)
                return (byte)ChannelLoadEnum.NinetyPercent;
            else
                return (byte)ChannelLoadEnum.Full;
        }

        public static int GetUtcSeconds(this DateTime dateTime)
        {
            return (int)DateTimeOffset.UtcNow.AddSeconds(
                        dateTime.Subtract(DateTime.Now).TotalSeconds
                    ).ToUnixTimeSeconds();
        }

        public static bool HasAttributeAdvantage(this DigimonAttributeEnum hitter, DigimonAttributeEnum target)
        {
            return hitter switch
            {
                DigimonAttributeEnum.Data => target == DigimonAttributeEnum.None || target == DigimonAttributeEnum.Vaccine,
                DigimonAttributeEnum.Vaccine => target == DigimonAttributeEnum.None || target == DigimonAttributeEnum.Virus,
                DigimonAttributeEnum.Virus => target == DigimonAttributeEnum.None || target == DigimonAttributeEnum.Data,
                DigimonAttributeEnum.Unknown => true,
                _ => false,
            };
        }

        public static bool HasElementAdvantage(this DigimonElementEnum hitter, DigimonElementEnum target)
        {
            return hitter switch
            {
                DigimonElementEnum.Ice => target == DigimonElementEnum.Neutral || target == DigimonElementEnum.Water,
                DigimonElementEnum.Water => target == DigimonElementEnum.Neutral || target == DigimonElementEnum.Fire,
                DigimonElementEnum.Fire => target == DigimonElementEnum.Neutral || target == DigimonElementEnum.Ice,
                DigimonElementEnum.Land => target == DigimonElementEnum.Neutral || target == DigimonElementEnum.Wind,
                DigimonElementEnum.Wind => target == DigimonElementEnum.Neutral || target == DigimonElementEnum.Wood,
                DigimonElementEnum.Wood => target == DigimonElementEnum.Neutral || target == DigimonElementEnum.Land,
                DigimonElementEnum.Light => target == DigimonElementEnum.Neutral || target == DigimonElementEnum.Dark,
                DigimonElementEnum.Dark => target == DigimonElementEnum.Neutral || target == DigimonElementEnum.Thunder,
                DigimonElementEnum.Thunder => target == DigimonElementEnum.Neutral || target == DigimonElementEnum.Steel,
                DigimonElementEnum.Steel => target == DigimonElementEnum.Neutral || target == DigimonElementEnum.Light,
                _ => false,
            };
        }

        public static bool HasAcessoryAttribute(this DigimonAttributeEnum hitter, AccessoryStatusTypeEnum accessory)
        {
            return accessory == AccessoryStatusTypeEnum.Data && hitter == DigimonAttributeEnum.Data ||
                   accessory == AccessoryStatusTypeEnum.Vacina && hitter == DigimonAttributeEnum.Vaccine ||
                   accessory == AccessoryStatusTypeEnum.Virus && hitter == DigimonAttributeEnum.Virus ||
                   accessory == AccessoryStatusTypeEnum.Unknown && hitter == DigimonAttributeEnum.Unknown;
        }

        public static bool HasAcessoryElement(this DigimonElementEnum hitter, AccessoryStatusTypeEnum accessory)
        {
            return accessory == AccessoryStatusTypeEnum.Ice && hitter == DigimonElementEnum.Ice ||
                   accessory == AccessoryStatusTypeEnum.Water && hitter == DigimonElementEnum.Water ||
                   accessory == AccessoryStatusTypeEnum.Fire && hitter == DigimonElementEnum.Fire ||
                   accessory == AccessoryStatusTypeEnum.Earth && hitter == DigimonElementEnum.Land ||
                   accessory == AccessoryStatusTypeEnum.Wind && hitter == DigimonElementEnum.Wind ||
                   accessory == AccessoryStatusTypeEnum.Wood && hitter == DigimonElementEnum.Wood ||
                   accessory == AccessoryStatusTypeEnum.Light && hitter == DigimonElementEnum.Light ||
                   accessory == AccessoryStatusTypeEnum.Dark && hitter == DigimonElementEnum.Dark ||
                   accessory == AccessoryStatusTypeEnum.Thunder && hitter == DigimonElementEnum.Thunder ||
                   accessory == AccessoryStatusTypeEnum.Steel && hitter == DigimonElementEnum.Steel;
        }

        public static short GetLevelSize(int hatchLevel)
        {
            return hatchLevel switch
            {
                3 => UtilitiesFunctions.RandomShort(8200, 10000),
                4 => UtilitiesFunctions.RandomShort(11000, 12500),
                5 => UtilitiesFunctions.RandomShort(11800, 13000),
                _ => 0,
            };
        }
        public static int RandomInt(int minValue = 0, int maxValue = int.MaxValue)
        {
            return _random.Next(minValue, maxValue < int.MaxValue ? maxValue + 1 : int.MaxValue);
        }

        public static byte RandomByte(byte minValue = 0, byte maxValue = byte.MaxValue)
        {
            return (byte)_random.Next(minValue, maxValue < byte.MaxValue ? maxValue + 1 : byte.MaxValue);
        }

        public static short RandomShort(short minValue = 0, short maxValue = short.MaxValue)
        {
            return (short)_random.Next(minValue, maxValue < short.MaxValue ? maxValue + 1 : short.MaxValue);
        }

        /// <summary>
        /// Returns a random value between 0.0% and 100.0%
        /// </summary>
        public static double RandomDouble() => _random.NextDouble() * 100;

        public static bool IsBetween(this int baseValue, params int[] range)
        {
            return range.Contains(baseValue);
        }

        public static bool IsBetween(this int baseValue, int minimalRange, int maximumRange)
        {
            return baseValue >= minimalRange && baseValue <= maximumRange;
        }

        public static bool IsBetween(this int baseValue, Enum minimalRangeEnum, Enum maximumRangeEnum)
        {
            return baseValue.IsBetween(minimalRangeEnum.GetHashCode(), maximumRangeEnum.GetHashCode());
        }

        public static long CalculateDistance(int xa, int xb, int ya, int yb)
        {
            var distanceX = (long)Math.Pow(xb - xa, 2);
            var distanceY = (long)Math.Pow(yb - ya, 2);

            var result = (long)Math.Sqrt(distanceX + distanceY);

            return result;
        }
        public static double CalculateDistanceD(int x1, int y1, int x2, int y2)
        {
            var deltaX = x2 - x1;
            var deltaY = y2 - y1;
            return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }
        public static fPos Lerp(fPos start, fPos end, float t)
        {
            float x = start.x + (end.x - start.x) * t;
            float y = start.y + (end.y - start.y) * t;
            return new fPos((int)x, (int)y);
        }
        public static void Aguardar(int milissegundos)
        {
            if (milissegundos > 0)
            {
                Stopwatch tempStopwatch = new Stopwatch();
                tempStopwatch.Start();

                while (tempStopwatch.ElapsedMilliseconds < milissegundos)
                {
                    // Aguardar até que o tempo especificado seja atingido
                }

                tempStopwatch.Stop();
            }
        }
        public static int MapGroup(int mapId)
        {
            if (mapId >= 1600 && mapId <= 1650|| mapId >= 2001 && mapId <= 2100)
            {
                return 2; // Dterminal
            }
            else if (mapId == 17 || mapId == 13)
            {
                return 3; // Dats
            }
            else if (mapId >= 1308 && mapId <= 1320)
            {
                return 1306; // Infinite Mountain
            }
            else
            {
                return -1; // Valor para indicar que o mapa não pertence a nenhum grupo conhecido
            }
        }
    }
}