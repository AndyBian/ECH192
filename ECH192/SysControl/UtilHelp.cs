using CommunicationServer.CommonEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ECH192.SysControl
{
    public class UtilHelp
    {
        /// <summary>
        /// 自动创建DataGridView中列表
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="propertyname">数据源中属性名</param>
        /// <param name="propertyname">列头名称</param>
        /// <param name="isshow">是否显示</param>
        /// <param name="isreadonly">是否只读</param>
        /// <param name="weight">宽度</param>
        /// <returns></returns>
        public static DataGridViewColumn CreateColumn(string name, string propertyname, string headtitle, bool isshow, bool isreadonly, int weight, int displayindex)
        {
            DataGridViewColumn dgc = new DataGridViewTextBoxColumn();
            dgc.Name = name;
            dgc.HeaderText = headtitle;
            dgc.ReadOnly = isreadonly;
            dgc.DataPropertyName = propertyname;
            dgc.Width = weight;
            dgc.FillWeight = weight;
            dgc.Visible = isshow;
            dgc.DisplayIndex = displayindex;
            dgc.SortMode = DataGridViewColumnSortMode.Automatic;
            return dgc;
        }

        /// <summary>
        /// 监测输入的数值是否为int数值
        /// </summary>
        /// <param name="inputvalue"></param>
        /// <returns></returns>
        public static bool CheckInput(string inputvalue)
        {
            int result = 0;
            float resultf = 0f;
            if (int.TryParse(inputvalue, out result) || float.TryParse(inputvalue, out resultf))
            {
                //if (result < 0 || resultf < 0)
                //    return false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 从大量IT数据中抽取数据进行展示
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<ITValue> FilterITData(List<ITValue> data,int Count,int interval)
        {
            if (data.Count == 0)
                return data;

            int total = data[0].CurrentValue.Count;
            int step = 1;
            if (Count != 0 && total > Count)
                step = total / Count;

            List<ITValue> results = new List<ITValue>();
            foreach (ITValue itValue in data)
            {
                ITValue result = new ITValue();
                result.Index = itValue.Index;
                for (int i = 0; i < itValue.Time.Count; i = i + step)
                {
                    result.Time.Add(float.Parse(((itValue.Time[i] * interval)/1000).ToString("f2")));
                    result.AvgCurrentValue.Add(itValue.AvgCurrentValue[i]);
                    result.CurrentValue.Add(itValue.CurrentValue[i]);
                }
                results.Add(result);
            }
            return results;
        }

        /// <summary>
        /// 从大量CV数据中抽取数据进行展示
        /// </summary>
        /// <param name="data"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public static List<CVValue> FilterCVData(List<CVValue> data, int Count)
        {
            if (data.Count == 0)
                return data;

            int total = data[0].CurrentValue.Count;
            int step = 1;
            if (Count != 0 && total > Count)
                step = total / Count;

            List<CVValue> results = new List<CVValue>();
            foreach (CVValue cvValue in data)
            {
                CVValue result = new CVValue();
                result.Index = cvValue.Index;
                result.Time = cvValue.Time;
                for (int i = 0; i < cvValue.CurrentValue.Count; i = i + step)
                {
                    result.CurrentValue.Add(cvValue.CurrentValue[i]);
                    result.AvgCurrentValue.Add(cvValue.AvgCurrentValue[i]);
                    result.VoltageValue.Add(cvValue.VoltageValue[i]);
                    result.AvgVoltageValue.Add(cvValue.AvgVoltageValue[i]);
                }
                results.Add(result);
            }
            return results;
        }

    }
}
