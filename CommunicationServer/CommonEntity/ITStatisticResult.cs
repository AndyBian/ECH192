using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationServer.CommonEntity
{
    /// <summary>
    /// IT法测试结果统计信息
    /// </summary>
    public class ITStatisticResult
    {
        // IT法采集结果电流最大值
        public int MaxValue;

        // IT法采集结果统计后个数最大值
        public int MaxCount;

        // 将IT法采集结果按照最大值和最小值分为10等分
        public List<int> ITValues = new List<int>();

        // IT法采集结果划分后各个区间的个数
        public List<int> Counts = new List<int>();

        public ITStatisticResult() { }

        public ITStatisticResult(int maxvalue, int maxcount)
        {
            this.MaxValue = maxvalue;
            this.MaxCount = maxcount;
        }
    }
}
