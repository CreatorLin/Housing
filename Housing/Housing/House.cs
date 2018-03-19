using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Housing
{
    public class House
    {
        public string 鄉鎮市區 { get; set; }
        public string 交易標的 { get; set; }
        public string 土地區段位置或建物區門牌 { get; set; }
        public float? 土地移轉總面積平方公尺 { get; set; }
        public string 都市土地使用分區 { get; set; }
        public string 非都市土地使用分區 { get; set; }
        public string 非都市土地使用編定 { get; set; }
        [JsonConverter(typeof(UnixTimestampConverter))]
        public DateTime 交易年月日 { get; set; }
        public string 交易筆棟數 { get; set; }
        public string 移轉層次 { get; set; }
        public string 總樓層數 { get; set; }
        public string 建物型態 { get; set; }
        public string 主要用途 { get; set; }
        public string 主要建材 { get; set; }
        [JsonConverter(typeof(UnixTimestampConverter))]
        public DateTime 建築完成年月 { get; set; }
        public float? 建物移轉總面積平方公尺 { get; set; }
        public int? 建物現況格局_房 { get; set; }
        public int? 建物現況格局_廳 { get; set; }
        public int? 建物現況格局_衛 { get; set; }
        public string 有無管理組織 { get; set; }
        public int 總價元 { get; set; }
        public int? 單價每平方公尺 { get; set; }
        public string 車位類別 { get; set; }
        public float? 車位移轉總面積平方公尺 { get; set; }
        public int? 車位總價元 { get; set; }
        public string 備註 { get; set; }
        public string 編號 { get; set; }
    }

    public class UnixTimestampConverter : DateTimeConverterBase
    {
        private readonly DateTime offset = new DateTime(1970, 1, 1);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(((DateTime)value - offset).TotalSeconds.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) return null;

            return offset.AddSeconds((long)reader.Value);
        }
    }
}




