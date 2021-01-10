using CommunicationServer.CommonEntity;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ECH192.UI
{
    public partial class ZoomITRealForm : MetroForm
    {
        // 实时展示的序号(96个通道分为6组)
        public int Index;

        // IT采集数据实时展示界面
        ITRealTest ItRealTest;

        // 更新步骤信息
        public delegate void CloseZoomShowDelegate();
        public CloseZoomShowDelegate closezoomshow;

        public ZoomITRealForm(int index)
        {
            this.Index = index;
            InitializeComponent();
            this.Text = "Group" + index;

            ItRealTest = new ITRealTest();
            ItRealTest.Dock = DockStyle.Fill;
            ItRealTest.Init();
            panelFill.Controls.Add(ItRealTest);
        }

        /// <summary>
        /// 设置IT法结果的统计图谱值
        /// </summary>
        /// <param name="result"></param>
        public void SetData(List<ITValue> itvalues)
        {
            if (itvalues != null && itvalues.Count > 0)
                ItRealTest.SetData(itvalues);
        }

        /// <summary>
        /// 关闭IT采集结果放大界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZoomITRealForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (closezoomshow != null)
                closezoomshow();
        }

        private void ZoomITRealForm_Load(object sender, EventArgs e)
        {

        }
    }
}
