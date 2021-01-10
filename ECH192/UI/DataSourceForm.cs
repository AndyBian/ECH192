using CommunicationServer.CommonEntity;
using CommunicationServer.CommunicationDriver;
using CommunicationServer.Protocol;
using ECH192.Entity;
using ECH192.SysControl;
using MetroFramework.Forms;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ECH192.UI
{
    /// <summary>
    /// 展示最新的一条数据信息，包括CV数据和IT数据结果
    /// </summary>
    public partial class DataSourceForm : MetroForm
    {
        public DataSourceForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 动态添加数据信息栏中字段数据
        /// </summary>
        private void InitTestResultMessage()
        {
            //绑定数据源时不自动添加列
            this.gridviewTestResult.AutoGenerateColumns = false;

            //序号
            DataGridViewColumn dgc = UtilHelp.CreateColumn("SerialNum", "SerialNum", "NO.", true, true, 50, 1);
            this.gridviewTestResult.Columns.Add(dgc);

            //通道名称
            dgc = UtilHelp.CreateColumn("ChannelName", "ChannelName", "Channel", true, true, 100, 2);
            this.gridviewTestResult.Columns.Add(dgc);

            //IT值
            dgc = UtilHelp.CreateColumn("ITValue", "ITValue", "IT(nA)", true, true, 100, 3);
            this.gridviewTestResult.Columns.Add(dgc);

            //CV法X1值
            dgc = UtilHelp.CreateColumn("CVX1Value", "CVX1Value", "CV-X1(nA)", true, true, 100, 4);
            this.gridviewTestResult.Columns.Add(dgc);

            //CV法Y1值
            dgc = UtilHelp.CreateColumn("CVY1Value", "CVY1Value", "CV-Y1(mV)", true, true, 100, 5);
            this.gridviewTestResult.Columns.Add(dgc);

            //CV法X2值
            dgc = UtilHelp.CreateColumn("CVX2Value", "CVX2Value", "CV-X2(nA)", true, true, 100, 6);
            this.gridviewTestResult.Columns.Add(dgc);

            //CV法Y2值
            dgc = UtilHelp.CreateColumn("CVY2Value", "CVY2Value", "CV-Y2(mV)", true, true, 100, 7);
            this.gridviewTestResult.Columns.Add(dgc);
        }

        private void DataSourceForm_Load(object sender, System.EventArgs e)
        {
            InitTestResultMessage();

            this.SetData(CommunicationManager.GetDataSource());
        }

        /// <summary>
        /// 更新数据展示
        /// </summary>
        /// <param name="data"></param>
        public void SetData(List<DataSource> datas)
        {
            int index = this.gridviewTestResult.FirstDisplayedScrollingRowIndex;
            List<DataSource> temp = new List<DataSource>();
            gridviewTestResult.DataSource = temp;
            gridviewTestResult.DataSource = datas;
            if (index > 0 && index <= this.gridviewTestResult.Rows.Count)
                this.gridviewTestResult.FirstDisplayedScrollingRowIndex = index;
        }
    }
}
