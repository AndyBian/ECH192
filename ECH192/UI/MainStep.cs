using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ECH192.UI
{
    public partial class MainStep : UserControl
    {
        // 头部标签信息
        public string message;

        // 开始测试
        public delegate void StartTest(string message);
        public StartTest starttest;

        public MainStep()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 更新头部信息
        /// </summary>
        public void UpdateLabelMessage()
        {
            this.lbMessage.Text = message;
        }

        public void UpdatePlayBackImage(Image image, bool enable, int progress = -1)
        {
            if (this.InvokeRequired)
            {
                btnRealTest.Invoke(new Action(() => btnRealTest.BackgroundImage = image));  // 跨线程访问UI控件
                btnRealTest.Invoke(new Action(() => btnRealTest.Enabled = enable));  // 跨线程访问UI控件
                if (progress != -1)
                    progressBar.Invoke(new Action(() => progressBar.Value = progress > 100 ? 100 : progress));
            }
            else
            {
                btnRealTest.BackgroundImage = image;
                btnRealTest.Enabled = enable;
                if (progress != -1)
                    progressBar.Value = progress;
            }
        }

        /// <summary>
        /// 开始测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRealTest_Click(object sender, EventArgs e)
        {
            if (starttest != null)
                starttest(message);
        }
    }
}
