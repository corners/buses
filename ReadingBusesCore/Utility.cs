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

        public static LikelyhoodOfSuccess IsReachable(DateTime departure, DateTime now, int travelTimeInMinutes, double departureMarginInSecods)
        {
            var span = departure - now.AddMinutes(travelTimeInMinutes);

            LikelyhoodOfSuccess result;
            if (Math.Abs(span.TotalSeconds) <= departureMarginInSecods)
            {
                result = LikelyhoodOfSuccess.Marginal;
            }
            else if (span.TotalSeconds > departureMarginInSecods)
            {
                result = LikelyhoodOfSuccess.Likely;
            }
            else
            {
                result = LikelyhoodOfSuccess.NotPossible;
            }
            return result;
        }

    }
}
