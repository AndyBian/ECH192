using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationServer.CommonEntity
{
    public class CVStatisticResult
    {
        // CV法采集结果电流最大值
        public int MaxValue;

        // CV法采集结果统计后个数最大值
        public int MaxCount;

        // 将CV法采集结果按照最大值和最小值分为10等分
        public List<int> CVValues = new List<int>();

        // CV法采集结果划分后各个区间的个数
        public List<int> Counts = new List<int>();

        public CVStatisticResult() { }

        public CVStatisticResult(int maxvalue, int maxcount)
        {
            this.MaxValue = maxvalue;
            this.MaxCount = maxcount;
        }

        public CVStatisticResult(List<int> cvvalues, List<int> counts)
        {
            this.CVValues = cvvalues;
            this.Counts = counts;
        }
    }
}
