using CommunicationServer.CommonEntity;
using ECH192.Entity;
using ECH192.SysControl;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ECH192.UI
{
    public partial class CVTest : UserControl
    {
        private string Hname = "电压(mV)";

        public CVTest(string hname)
        {
            this.Hname = hname;
            InitializeComponent();
        }

        #region 初始化

        // 图表，用于显示IT法的测试结果
        public System.Windows.Forms.DataVisualization.Charting.Chart chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();

        // IT法只展示1个电流结果的柱状图
        ChartArea chartArea1 = new ChartArea();

        private Color colData = Color.FromArgb(65, 165, 202);

        /// <summary>
        /// 图谱初始化
        /// </summary>
        public void InitChart(string title)
        {
            Title ITTitle = new Title();

            ITTitle.Docking = Docking.Top;
            ITTitle.Text = title;
            ITTitle.Alignment = ContentAlignment.TopCenter;
            ITTitle.Font = new Font(ITTitle.Font.FontFamily, 13, ITTitle.Font.Style);

            chart1.Titles.Add(ITTitle);

            this.Controls.Add(chart1);
            chart1.Dock = DockStyle.Fill;
            chart1.BackColor = Color.FromName("Contorl");

            ITInit();
        }

        /// <summary>
        /// 初始化IT法结果统计图
        /// </summary>
        private void ITInit()
        {
            chartArea1.Name = "CV";

            // 初始化X轴显示信息
            // 初始值是0-100nA，每个间隔10nA
            chartArea1.AxisX.Minimum = 0;
            chartArea1.AxisX.Maximum = 100;
            chartArea1.AxisX.Interval = 10;
            //chartArea1.AxisX.MajorGrid.Interval = 10;
            //chartArea1.AxisX.MajorTickMark.Interval = 10;
            chartArea1.AxisX.IsMarginVisible = false;
            chartArea1.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea1.AxisX.MinorGrid.LineColor = Color.LightGray;
            chartArea1.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            chartArea1.AxisX.MinorGrid.LineDashStyle = ChartDashStyle.Dot;
            chartArea1.AxisX.Title = Hname;
            chartArea1.AxisX.TitleFont = new Font("微软雅黑", 12);

            // 初始化Y轴显示信息
            // 初始值是0-100，每个间隔10个
            chartArea1.AxisY.TextOrientation = TextOrientation.Rotated270;
            chartArea1.AxisY.Minimum = 0;
            chartArea1.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea1.AxisY.MinorGrid.LineColor = Color.LightGray;
            chartArea1.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            chartArea1.AxisY.MinorGrid.LineDashStyle = ChartDashStyle.Dot;
            chartArea1.AxisY.Title = "个数";
            chartArea1.AxisY.TitleFont = new Font("微软雅黑", 12);
            chartArea1.AxisY.Minimum = 0;
            chartArea1.AxisY.Maximum = 100;
            chartArea1.AxisY.Interval = 10;
            //chartArea1.AxisY.MajorGrid.Interval = 10;
            //chartArea1.AxisY.MajorTickMark.Interval = 10;

            chartArea1.AxisY.LabelAutoFitStyle = LabelAutoFitStyles.IncreaseFont;
            chartArea1.AxisX.LabelStyle.Font = new Font("微软雅黑", 12);
            //chartArea1.AxisX.LabelStyle.ForeColor = Color.FromArgb(65, 165, 202);

            chartArea1.CursorX.IsUserEnabled = true;
            chartArea1.CursorX.IsUserSelectionEnabled = true;
            chartArea1.AxisX.ScaleView.Zoomable = false;
            chartArea1.BorderColor = Color.Black;
            chartArea1.BorderWidth = 2;
            chartArea1.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;

            // 数据曲线
            System.Windows.Forms.DataVisualization.Charting.Series series = new System.Windows.Forms.DataVisualization.Charting.Series();
            series.ChartType = SeriesChartType.Column;
            series.IsVisibleInLegend = false;
            series.ToolTip = "(#VALX,#VALY{f0})";
            series.Color = colData;

            chart1.Series.Add(series);
            chart1.ChartAreas.Add(chartArea1);
            chart1.Titles[0].IsDockedInsideChartArea = false;
            chart1.Titles[0].DockedToChartArea = chartArea1.Name;

            // 默认赋值
            InitData();
        }

        /// <summary>
        /// 初始化CV法结果的统计图谱值
        /// </summary>
        /// <param name="result"></param>
        public void InitData()
        {
            chartArea1.AxisX.Minimum = 0;
            chartArea1.AxisX.Maximum = 100;
            chartArea1.AxisX.Interval = 10;

            chartArea1.AxisY.Minimum = 0;
            chartArea1.AxisY.Maximum = 100;
            chartArea1.AxisY.Interval = 10;

            // 默认赋值
            CVStatisticResult result = new CVStatisticResult(100, 100);
            result.CVValues.Add(0);
            result.Counts.Add(0);
            SetData(result);
        }

        /// <summary>
        /// 设置CV法结果的统计图谱值
        /// </summary>
        /// <param name="result"></param>
        public void SetData(CVStatisticResult result)
        {
            if (result == null || result.CVValues.Count == 0)
                return;

            //清空数据
            chart1.Series[0].Points.Clear();
            chartArea1.AxisY.Maximum = SystemParam.NodeNum + 1;
            chartArea1.AxisY.LabelAutoFitStyle = LabelAutoFitStyles.IncreaseFont;

            chartArea1.AxisX.Maximum = result.CVValues.Max();
            chartArea1.AxisX.Minimum = result.CVValues.Min();
            chartArea1.AxisX.Interval = (result.CVValues.Max() - result.CVValues.Min()) / 10;

            // 逐条数据绘制
            for (int i = 0; i < result.CVValues.Count; i++)
            {
                chart1.Series[0].Points.AddXY(result.CVValues[i], result.Counts[i]);
            }
        }

        #endregion
    }
}
