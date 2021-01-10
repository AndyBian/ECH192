using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationServer.CommonEntity
{
    /// <summary>
    /// 采集数据值的通用类
    /// </summary>
    public class FetchValue
    {
        // 通道的索引号
        public int Index { get; set; }

        // 采集时间
        public float Time { get; set; }

        // 通道采集的电流值
        public double CurrentValue { get; set; }

        // 通道采集的平均电流值（例如索引0和1的平均值是一样的）
        public double AvgCurrentValue { get; set; }

        // 通道采集的压值
        public double VoltageValue { get; set; }

        // 通道采集的平均压值（例如索引0和1的平均值是一样的）
        public double AvgVoltageValue { get; set; }

        public FetchValue(int index,double current,double avgcurrent)
        {
            this.Index = index;
            this.CurrentValue = current;
            this.AvgCurrentValue = avgcurrent;
        }

        public FetchValue(int index, double current, double avgcurrent, double voltage, double avgvoltage)
        {
            this.Index = index;
            this.CurrentValue = current;
            this.AvgCurrentValue = avgcurrent;
            this.VoltageValue = voltage;
            this.AvgVoltageValue = avgvoltage;
        }
    }
}
