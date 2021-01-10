using CommunicationServer.Protocol.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationServer.Util
{
    public class JsonParse
    {
        /// <summary>
        /// 从字符串转为FrameBuffer类
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static FrameBuffer JsonParseFromString(string result)
        {
            FrameBuffer rb = JsonConvert.DeserializeObject<FrameBuffer>(result);
            return rb;
        }

        /// <summary>
        /// 将FrameBuffer转换为字符串
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string JsonToString(FrameBuffer buffer)
        {
            //格式去除json串中未定义的字段
            var jsonSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

            return JsonConvert.SerializeObject(buffer, Formatting.Indented, jsonSetting);
        }
    }
}
