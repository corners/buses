using Newtonsoft.Json;
using ReadingBusesCore;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReadingBusesCore
{
    public static class Utility
    {
        /// <summary>
        /// Convert to GMT Standard Time
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ToUkTime(DateTime value)
        {
            TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            var ukTime = TimeZoneInfo.ConvertTime(value, TimeZoneInfo.Utc, timeInfo);
            return ukTime;
        }

        public static void SaveToFile<T>(T instance, string path)
        {
            using (var fs = new FileStream(path, FileMode.Create))
            {
                using (var textWriter = new StreamWriter(fs))
                {
                    using (var writer = new JsonTextWriter(textWriter))
                    {
                        var s = new JsonSerializer();
                        s.Serialize(writer, instance);
                    }
                }
            }
        }

        public static T LoadFromFile<T>(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open))
            {
                using (var textReader = new StreamReader(fs))
                {
                    using (var reader = new JsonTextReader(textReader))
                    {
                        var s = new JsonSerializer();
                        T result = s.Deserialize<T>(reader);
                        return result;
                    }
                }
            }
        }

        public static string SaveToString<T>(T instance)
        {
            using (var textWriter = new StringWriter())
            {
                using (var writer = new JsonTextWriter(textWriter))
                {
                    var s = new JsonSerializer();
                    s.Serialize(writer, instance);
                }
                return textWriter.ToString();
            }
        }

        public static T LoadFromString<T>(string text)
        {
            using (var textReader = new StringReader(text))
            {
                using (var reader = new JsonTextReader(textReader))
                {
                    var s = new JsonSerializer();
                    T result = s.Deserialize<T>(reader);
                    return result;
                }
            }
        }
        
        public static string FriendlyTime(TimeSpan span)
        {
            string result;
            var mins = Math.Floor(span.TotalMinutes);
            if (mins <= 60)
                result = string.Format("{0}m", mins);
            else
                result = ">1hr";
            return result;
        }

        public static LikelyhoodOfSuccess IsReachable(DateTime departureUtc, DateTime nowUtc, int travelTimeInMinutes, double departureMarginInSecods)
        {
            if (departureUtc.Kind != DateTimeKind.Utc)
                throw new ArgumentException("Departure is not in UTC");
            if (nowUtc.Kind != DateTimeKind.Utc)
                throw new ArgumentException("Now is not in UTC");

            var span = (departureUtc - nowUtc).TotalMinutes - travelTimeInMinutes;

            var margin = Math.Floor(departureMarginInSecods / 60);
            LikelyhoodOfSuccess result;
            if (Math.Abs(span) <= margin)
            {
                result = LikelyhoodOfSuccess.Marginal;
            }
            else if (span > margin)
            {
                result = LikelyhoodOfSuccess.Likely;
            }
            else
            {
                result = LikelyhoodOfSuccess.NotPossible;
            }
            return result;
        }

        public readonly static char[] Base62 = new char[] { '0','1','2','3','4','5','6','7','8','9',
            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
            'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z'};

        public static int StringToInt(string encodedString, char[] baseChars)
        {
            int result = 0;
            int sourceBase = baseChars.Length;
            int nextCharIndex = 0;

            for (int currentChar = encodedString.Length - 1; currentChar >= 0; currentChar--)
            {
                char next = encodedString[currentChar];

                // For loop gets us: baseChar.IndexOf(char) => int
                for (nextCharIndex = 0; nextCharIndex < baseChars.Length; nextCharIndex++)
                {
                    if (baseChars[nextCharIndex] == next)
                    {
                        break;
                    }
                }

                // For character N (from the end of the string), we multiply our value
                // by 64^N. eg. if we have "CE" in hex, F = 16 * 13.
                result += (int)Math.Pow(baseChars.Length, encodedString.Length - 1 - currentChar) * nextCharIndex;
            }

            return result;
        }

        public static string IntToString(int value, char[] baseChars)
        {
            string result = string.Empty;
            int targetBase = baseChars.Length;

            do
            {
                result = baseChars[value % targetBase] + result;
                value = value / targetBase;
            }
            while (value > 0);

            return result;
        }
    }
}
