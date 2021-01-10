using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationServer.CommonEntity
{
    /// <summary>
    /// 数据源类，获取后在数据列表中展示
    /// </summary>
    public class DataSource
    {
        //序号
        public int SerialNum { get; set; }

        //通道名称
        public string ChannelName { get; set; }

        //IT值
        public string ITValue { get; set; } = "N/A";

        //CV法X1值
        public string CVX1Value { get; set; } = "N/A";

        //CV法Y1值
        public string CVY1Value { get; set; } = "N/A";

        //CV法X2值
        public string CVX2Value { get; set; } = "N/A";

        //CV法Y2值
        public string CVY2Value { get; set; } = "N/A";

        public DataSource()
        {

        }
    }
}
