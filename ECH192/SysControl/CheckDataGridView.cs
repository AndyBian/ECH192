using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace ECH192.SysControl
{
    /// <summary>
    /// 带有选择框的datagridview控件
    /// </summary>
    //实现全选的事件参数类
    public class DataGridviewCheckboxHeaderEventHander : EventArgs
    {
        //选择框属性
        private bool checkedState = false;

        public bool CheckedState
        {
            get { return checkedState; }
            set { checkedState = value; }
        }
    }

    //与事件关联的委托
    public delegate void DataGridviewCheckboxHeaderCellEventHander(object sender, DataGridviewCheckboxHeaderEventHander e, DataGridView dgv);

    //重写 DataGridViewColumnHeaderCell
    public class DataGridviewCheckboxHeaderCell : DataGridViewColumnHeaderCell
    {

        private Point checkBoxLocation;
        private Size checkBoxSize;
        public bool isChecked = false;
        private Point cellLocation = new Point();
        private CheckBoxState cbState = CheckBoxState.UncheckedNormal;

        public event DataGridviewCheckboxHeaderCellEventHander OnCheckBoxClicked;


        //绘制列头checkbox
        protected override void Paint(Graphics g,
                                      Rectangle clipBounds,
                                      Rectangle cellBounds,
                                      int rowIndex,
                                      DataGridViewElementStates dataGridViewElementState,
                                      object value,
                                      object formattedValue,
                                      string errorText,
                                      DataGridViewCellStyle cellStyle,
                                      DataGridViewAdvancedBorderStyle advancedBorderStyle,
                                      DataGridViewPaintParts paintParts)
        {
            base.Paint(g, clipBounds, cellBounds, rowIndex, dataGridViewElementState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
            Point p = new Point();
            Size s = CheckBoxRenderer.GetGlyphSize(g, CheckBoxState.UncheckedNormal);
            //列头checkbox的X坐标
            //p.X = cellBounds.Location.X + (int)(cellBounds.Width/1.2) - (s.Width / 2) - 1;
            p.X = cellBounds.Location.X + (int)(cellBounds.Width / 2) - (s.Width / 2) - 1;
            //列头checkbox的Y坐标
            p.Y = cellBounds.Location.Y + (cellBounds.Height / 2) - (s.Height / 2);
            cellLocation = cellBounds.Location;
            checkBoxLocation = p;
            checkBoxSize = s;
            if (isChecked)
                cbState = CheckBoxState.CheckedNormal;
            else
                cbState = CheckBoxState.UncheckedNormal;
            //绘制复选框
            CheckBoxRenderer.DrawCheckBox(g, checkBoxLocation, cbState);
        }

        /// <summary>
        /// 响应点击列头checkbox单击事件
        /// </summary>
        protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
        {
            Point p = new Point(e.X + cellLocation.X, e.Y + cellLocation.Y);
            if (p.X >= checkBoxLocation.X && p.X <= checkBoxLocation.X + checkBoxSize.Width && p.Y >= checkBoxLocation.Y && p.Y <= checkBoxLocation.Y + checkBoxSize.Height)
            {
                isChecked = !isChecked;

                //获取列头checkbox的选择状态
                DataGridviewCheckboxHeaderEventHander ex = new DataGridviewCheckboxHeaderEventHander();
                ex.CheckedState = isChecked;

                //此处不代表选择的列头checkbox，只是作为参数传递。应该列头checkbox是绘制出来的，无法获得它的实例
                object sender = new object();

                if (OnCheckBoxClicked != null)
                {
                    //触发单击事件
                    OnCheckBoxClicked(sender, ex, this.DataGridView);
                    this.DataGridView.InvalidateCell(this);
                }
            }
            base.OnMouseClick(e);
        }

    }
}
