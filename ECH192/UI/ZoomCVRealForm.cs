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
    public partial class ZoomCVRealForm : MetroForm
    {
        // 实时展示的序号(96个通道分为6组)
        public int Index;

        // CV采集数据实时展示界面
        CVRealTest CvRealTest;

        // 更新步骤信息
        public delegate void CloseZoomShowDelegate();
        public CloseZoomShowDelegate closezoomshow;

        public ZoomCVRealForm(int index)
        {
            this.Index = index;
            InitializeComponent();
            this.Text = "Group" + index;

            CvRealTest = new CVRealTest();
            CvRealTest.Dock = DockStyle.Fill;
            CvRealTest.Init();
            panelFill.Controls.Add(CvRealTest);
        }

        /// <summary>
        /// 设置CV法结果的统计图谱值
        /// </summary>
        /// <param name="result"></param>
        public void SetData(List<CVValue> cvvalues)
        {
            CvRealTest.SetData(cvvalues);
        }

        private void ZoomCVRealForm_Load(object sender, EventArgs e)
        {

        }

        private void ZoomCVRealForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (closezoomshow != null)
                closezoomshow();
        }
    }
}
