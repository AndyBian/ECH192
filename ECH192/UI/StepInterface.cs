using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ECH192.Entity.Enum;
using ECH192.SysControl;

namespace ECH192.UI
{
    public partial class StepInterface : UserControl
    {
        //表示当前是第几步
        public int stepNum { get; set; }

        // 表示当前是否处于选中
        public bool CheckState;

        //步骤名称
        public string stepname { get; set; }

        //步骤耗时
        public int steptime { get; set; } = 100;

        // 更新步骤信息
        public delegate void UpdateStepInfo();
        public UpdateStepInfo updatestepinfo;

        // 测试步骤进行测试
        public delegate void TestStep(StepEnum stepEnum,int stepindex);
        public TestStep teststep;

        //是否需要更新界面
        private bool updateselectindex = false;

        public StepInterface()
        {
            InitializeComponent();
        }

        private void StepInterface_Load(object sender, EventArgs e)
        {
            string[] steps = Enum.GetNames(typeof(StepEnum));
            cbStep.Items.Clear();
            cbStep.Items.AddRange(steps);
            if (steps.Length > 0 && this.stepname == null)
                cbStep.SelectedIndex = 0;
            else
                cbStep.SelectedIndex = (int)(StepEnum)Enum.Parse(typeof(StepEnum), stepname);
            lbStep.Text = "步骤" + stepNum + ":";
        }

        /// <summary>
        /// 更新步骤的名称
        /// </summary>
        public void SetLbStep()
        {
            lbStep.Text = "步骤" + stepNum + ":";
        }

        /// <summary>
        /// 获取保存的步骤之后更新界面显示
        /// </summary>
        public void update()
        {
            txtTime.Text = steptime.ToString();
            cbStep.Text = stepname;
            string[] steps = Enum.GetNames(typeof(StepEnum));
            cbStep.Items.Clear();
            cbStep.Items.AddRange(steps);
            cbStep.SelectedIndex = (int)(StepEnum)Enum.Parse(typeof(StepEnum), stepname);
        }

        /// <summary>
        /// 不同的步骤，确认是否含有时间参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbStep_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowTimes();

            stepname = cbStep.Text;
            if (updatestepinfo != null && updateselectindex)
                updatestepinfo();
        }

        /// <summary>
        /// 动态显示步骤和时间的显示
        /// </summary>
        public void ShowTimes()
        {
            if (cbStep.SelectedIndex >= 1 && cbStep.SelectedIndex <= 4)
            {
                txtTime.Visible = true;
                lbtime.Visible = true;
            }
            else
            {
                txtTime.Visible = false;
                lbtime.Visible = false;
            }
        }

        /// <summary>
        /// 实时测试步骤
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRealTest_Click(object sender, EventArgs e)
        {
            //TODO
            if (teststep != null)
                teststep((StepEnum)cbStep.SelectedIndex, stepNum - 1);
        }

        /// <summary>
        /// 修改步骤的选择状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckStatus_CheckedChanged(object sender, EventArgs e)
        {
            this.CheckState = CheckStatus.Checked;
        }

        /// <summary>
        /// 实时修改测试时间间隔
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTime_TextChanged(object sender, EventArgs e)
        {
            if (!UtilHelp.CheckInput(txtTime.Text))
            {
                MsgForm msg = new MsgForm("提示", "数值输入错误");
                txtTime.Text = "100";
                msg.ShowDialog();
                return;
            }
            int interval = 0;
            int.TryParse(txtTime.Text, out interval);
            this.steptime = interval;
            if (updatestepinfo != null && updateselectindex)
                updatestepinfo();
        }

        private void StepInterface_Paint(object sender, PaintEventArgs e)
        {
            updateselectindex = true;
        }
    }
}
