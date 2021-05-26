using CommunicationServer.CommonEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECH192.Entity
{
    [Serializable]
    public class TestResultSerialize
    {
        /// <summary>
        /// 存储测试过程中所有采集的IT结果值
        /// </summary>
        public  List<ITValue> ITValues = new List<ITValue>();

        /// <summary>
        /// 存储测试过程中所有采集的CV结果值
        /// </summary>
        public  List<CVValue> CVValues = new List<CVValue>();

        /// <summary>
        /// 测点的状态
        /// </summary>
        public  List<int> NodeStatus = new List<int>();

        public TestResultSerialize() { }

        public TestResultSerialize(List<ITValue> ITValues, List<CVValue> CVValues, List<int> NodeStatus)
        {
            this.ITValues = ITValues;
            this.CVValues = CVValues;
            this.NodeStatus = NodeStatus;
        }
    }
}
