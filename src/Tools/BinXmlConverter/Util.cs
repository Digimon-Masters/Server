using System.Text;
using System.Text.RegularExpressions;

namespace DSO.BinXmlConverter
{
    public class Util
    {
        public static byte[] GetStringBytes(string targetString, int arraySize)
        {
            var stringCut = arraySize / 2;

            var stringValue = RemoveCdataFromString(targetString);
            stringValue = stringValue?.Substring(0, stringValue.Length > stringCut - 1 ? stringCut - 1 : stringValue.Length);

            var stringBytes = Encoding.Unicode.GetBytes(stringValue + char.MinValue);
            var nameArray = new byte[arraySize];
            stringBytes.CopyTo(nameArray, 0);

            return nameArray;
        }

        public static void GenerateXml(string text, string dest)
        {
            if (File.Exists(dest))
                File.Delete(dest);

            Thread.Sleep(1000);
            if (!File.Exists(dest))
            {
                using (FileStream fileStr = File.Create(dest))
                { }
                Thread.Sleep(1000);
            }

            File.WriteAllText(dest, text);
            Thread.Sleep(1000);
        }

        public static string AddCdataForUnicodeChars(string targetString)
        {
            if (string.IsNullOrEmpty(targetString))
                return targetString;

            targetString = Regex.Replace(targetString, @"[\p{C}-[\t\r\n]]+", string.Empty);

            //targetString = targetString
            //    .Replace((char)0x02, 'a')
            //    .Replace((char)0x0B, 'b')
            //    .Replace((char)0x1F, 'c');

            var needCdata = false;

            foreach (var character in targetString)
            {
                if (character >= 'a' && character <= 'z' ||
                    character >= 'A' && character <= 'Z' ||
                    character >= '0' && character <= '9' ||
                    character == '-' ||
                    character == '(' ||
                    character == ')' ||
                    character == '.' ||
                    character == ',' ||
                    character == '?' ||
                    character == '!' ||
                    character == (char)32
                ) { }
                else
                    needCdata = true;
            }

            if (needCdata)
                return "<![CDATA[" + targetString + "]]>";
            else
                return targetString;
        }

        public static float ToFloat(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return 0.0f;
            }

            float result;
            if (float.TryParse(input, out result))
            {
                return result;
            }
            else
            {
                return 0.0f;
            }
        }

        public static string RemoveCdataFromString(string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            return s
                .Replace("<![CDATA[", string.Empty)
                .Replace("]]>", string.Empty);
        }
    }
}
