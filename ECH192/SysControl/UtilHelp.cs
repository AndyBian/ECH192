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
                if (result < 0 || resultf < 0)
                    return false;
                return true;
            }
            return false;
        }
    }
}
