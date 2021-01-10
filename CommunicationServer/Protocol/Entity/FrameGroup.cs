using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationServer.Protocol.Entity
{
    /// <summary>
    /// 通道电流灵敏度相关设置
    /// </summary>
    public class FrameGroup
    {
        /// <summary>
        /// 组号
        /// </summary>
        public int index { get; set; }

        /// <summary>
        /// 改组内通道
        /// </summary>
        public List<int> channels { get; set; }

        /// <summary>
        /// 有效的灵敏度值
        /// </summary>
        public List<int> values { get; set; }

        /// <summary>
        /// 当前的灵敏度值
        /// </summary>
        public int current { get; set; }
    }
}
