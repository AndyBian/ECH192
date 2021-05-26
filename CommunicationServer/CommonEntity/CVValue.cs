using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationServer.CommonEntity
{
    /// <summary>
    /// CV法扫描采集数据
    /// </summary>
    [Serializable]
    public class CVValue
    {
        // 通道的索引号
        public int Index { get; set; }

        // 采集的时间间隔
        public float Time { get; set; }

        // 采集的电流值
        public List<double> CurrentValue { get; set; }

        // 采集的电流平均值（例如索引0和1的平均值是一样的）
        public List<double> AvgCurrentValue { get; set; }

        // 采集的电压值
        public List<double> VoltageValue { get; set; }

        // 采集的电压平均值（例如索引0和1的平均值是一样的）
        public List<double> AvgVoltageValue { get; set; }

        public CVValue()
        {
            CurrentValue = new List<double>();
            AvgCurrentValue = new List<double>();
            VoltageValue = new List<double>();
            AvgVoltageValue = new List<double>();
        }

        public CVValue(int index, List<double> currentvalue, List<double> avgcurrentvalue, List<double> voltvalue, List<double> avgvoltvalue)
        {
            this.Index = index;
            this.CurrentValue = currentvalue;
            this.AvgCurrentValue = avgcurrentvalue;
            this.VoltageValue = voltvalue;
            this.AvgVoltageValue = avgvoltvalue;
        }
    }
}
