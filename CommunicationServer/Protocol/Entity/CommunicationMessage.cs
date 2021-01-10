using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationServer.Protocol.Entity
{
    public class CommunicationMessage
    {
        //通信服务名称
        public string ServiceName { get; set; }
        /// <summary>
        /// 通信客户端的SessionId
        /// </summary>
        public string SessionId { get; set; }
        /// <summary>
        /// 通信的相关参数
        /// </summary>
        public Dictionary<string, dynamic> Message { get; set; }
    }
}
