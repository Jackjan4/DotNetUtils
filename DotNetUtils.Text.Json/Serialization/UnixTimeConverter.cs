using System;
using System.Text.Json;
using System.Text.Json.Serialization;



namespace Roslan.DotNetUtils.Text.Json.Serialization {



    public class UnixTimeConverter : JsonConverter<DateTime> {



        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            long unixTime = reader.GetInt64();

            // Convert to DateTime
            return DateTimeOffset.FromUnixTimeSeconds(unixTime).UtcDateTime;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options) {

            // Convert DateTime to Unix time
            long unixTime = ((DateTimeOffset)value).ToUnixTimeSeconds();
            writer.WriteNumberValue(unixTime);
        }
    }
}
