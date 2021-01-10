using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationServer.CommonEntity
{
    /// <summary>
    /// it法电沉积采集数据
    /// </summary>
    public class ITValue:IComparable
    {
        public int Index { get; set; }

        public List<float> Time { get; set; }

        public List<double> CurrentValue { get; set; }

        public List<double> AvgCurrentValue { get; set; }

        public ITValue(int index,List<double> currentvalue,List<double> avgcurrentvalue)
        {
            this.Index = index;
            this.CurrentValue = currentvalue;
            this.AvgCurrentValue = avgcurrentvalue;
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
