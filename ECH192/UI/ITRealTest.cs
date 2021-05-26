using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ECH192.Entity;
using ECH192.SysControl;
using CommunicationServer.CommonEntity;
using log4net;

namespace ECH192.UI
{
    public partial class ITRealTest : UserControl
    {
        // 实时展示的序号(96个通道分为6组)
        public int Index = -1;

        public static ILog logger = LogManager.GetLogger(typeof(ITRealTest));

        // 放大显示
        public delegate void ZoomShowDelegate(int index);
        public ZoomShowDelegate zoomshow;

        public ITRealTest(int index)
        {
            this.Index = index;
            InitializeComponent();
        }

        public ITRealTest()
        {
            InitializeComponent();
        }

        public void Init()
        {
            InitChart();
        }

        // 图表，用于显示IT法的测试结果
        public System.Windows.Forms.DataVisualization.Charting.Chart chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();

        // IT法只展示1个电流结果的柱状图
        ChartArea chartArea1 = new ChartArea();

        private Color colData = Color.FromArgb(65, 165, 202);

        private Color[] Colors = new Color[] {
            Color.Red ,Color.Green, Color.Fuchsia, Color.DodgerBlue, Color.Orange ,
            Color.WhiteSmoke, Color.Gray, Color.Yellow, Color.Cyan ,Color.SkyBlue,
            Color.AliceBlue ,Color.Black, Color.DarkBlue, Color.DarkGreen, Color.DarkOrange ,
            Color.DarkRed};

        /// <summary>
        /// 图谱初始化
        /// </summary>
        public void InitChart()
        {
            Title ITTitle = new Title();

            ITTitle.Docking = Docking.Top;
            ITTitle.Text = Index == -1 ? "IT法实时数据" : "Group" + Index;
            ITTitle.Alignment = ContentAlignment.TopCenter;
            ITTitle.Font = new Font(ITTitle.Font.FontFamily, 13, ITTitle.Font.Style);

            chart1.Titles.Add(ITTitle);

            this.Controls.Add(chart1);
            chart1.Dock = DockStyle.Fill;
            chart1.BackColor = Color.FromName("Contorl");

            if (Index != -1)
            {
                chart1.MouseEnter += new EventHandler(MouseEnterEvent);
                chart1.MouseLeave += new EventHandler(MouseLeaveEvent);
            }
            else
                this.btnzoom.Visible = false;

            ITInit();
        }

        /// <summary>
        /// 初始化IT法结果统计图
        /// </summary>
        private void ITInit()
        {
            chartArea1.Name = "IT";

            // 初始化X轴显示信息
            // 初始值是0-1000nA，每个间隔100nA
            chartArea1.AxisX.Minimum = 0;
            chartArea1.AxisX.Maximum = 100;
            chartArea1.AxisX.Interval = 20;
            //chartArea1.AxisX.MajorGrid.Interval = 10;
            //chartArea1.AxisX.MajorTickMark.Interval = 10;
            chartArea1.AxisX.IsMarginVisible = false;
            chartArea1.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea1.AxisX.MinorGrid.LineColor = Color.LightGray;
            chartArea1.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            chartArea1.AxisX.MinorGrid.LineDashStyle = ChartDashStyle.Dot;
            chartArea1.AxisX.Title = "时间(s)";
            chartArea1.AxisX.TitleFont = new Font("微软雅黑", 12);

            // 初始化Y轴显示信息
            // 初始值是0-100，每个间隔10个
            chartArea1.AxisY.TextOrientation = TextOrientation.Rotated270;
            chartArea1.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea1.AxisY.MinorGrid.LineColor = Color.LightGray;
            chartArea1.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            chartArea1.AxisY.MinorGrid.LineDashStyle = ChartDashStyle.Dot;
            chartArea1.AxisY.Title = "电流(nA)";
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
            chartArea1.AxisX.ScaleView.Zoomable = true;
            chartArea1.BorderColor = Color.Black;
            chartArea1.BorderWidth = 2;
            chartArea1.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;

            for (int i = 0; i < 16; i++)
            {
                // 数据曲线
                System.Windows.Forms.DataVisualization.Charting.Series series = new System.Windows.Forms.DataVisualization.Charting.Series();
                series.ChartType = SeriesChartType.Line;
                series.IsVisibleInLegend = true;
                series.ToolTip = "(#VALX,#VALY{f0})";
                series.Color = Colors[i] ;
                series.Name = "CH" + (i + 1);
                chart1.Series.Add(series);
            }

            string[] EventLegendTexts = new string[] { "", "#LEGENDTEXT" };
            System.Windows.Forms.DataVisualization.Charting.Legend legend = new System.Windows.Forms.DataVisualization.Charting.Legend();
            legend.BackColor = System.Drawing.Color.Transparent;
            legend.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Right;
            System.Windows.Forms.DataVisualization.Charting.LegendCellColumn[] columns = new System.Windows.Forms.DataVisualization.Charting.LegendCellColumn[2];
            for (int i = 0; i < columns.Length; i++)
            {
                columns[i] = new System.Windows.Forms.DataVisualization.Charting.LegendCellColumn();
                if (i == 0)
                { columns[i].ColumnType = System.Windows.Forms.DataVisualization.Charting.LegendCellColumnType.SeriesSymbol; }
                else
                {
                    columns[i].ColumnType = System.Windows.Forms.DataVisualization.Charting.LegendCellColumnType.Text;
                    columns[i].Text = EventLegendTexts[i];
                }
                columns[i].Alignment = ContentAlignment.TopRight;
                legend.CellColumns.Add(columns[i]);
            }
            chart1.Legends.Add(legend);

            chart1.ChartAreas.Add(chartArea1);
            chart1.Titles[0].IsDockedInsideChartArea = false;
            chart1.Titles[0].DockedToChartArea = chartArea1.Name;

            //初始化
            InitData();
        }

        /// <summary>
        /// 设置IT法结果的统计图谱值
        /// </summary>
        /// <param name="result"></param>
        public void InitData()
        {
            // 初始化标签坐标
            chartArea1.AxisX.Minimum = 0;
            chartArea1.AxisX.Maximum = 100;
            chartArea1.AxisX.Interval = 20;
            chartArea1.AxisX.MajorGrid.Interval = 10;
            chartArea1.AxisX.MajorTickMark.Interval = 10;

            chartArea1.AxisY.Minimum = 0;
            chartArea1.AxisY.Maximum = 100;
            chartArea1.AxisY.Interval = 10;
            chartArea1.AxisY.MajorGrid.Interval = 10;
            chartArea1.AxisY.MajorTickMark.Interval = 10;

            // 默认赋值
            List<double> currentvalue = new List<double>();
            currentvalue.Add(0);
            List<double> avgcurrentvalue = new List<double>();
            avgcurrentvalue.Add(0);
            List<float> times = new List<float>();
            times.Add(0);
            ITValue itvalue = new ITValue(0, currentvalue, avgcurrentvalue);
            itvalue.Time = times;

            for (int i = 0; i < 16; i++)
                chart1.Series[i].Points.Clear();

            chartArea1.AxisY.CustomLabels.Clear();

            chartArea1.AxisY.LabelAutoFitStyle = LabelAutoFitStyles.IncreaseFont;

            for (int i = 0; i < itvalue.AvgCurrentValue.Count; i++)
            {
                // 逐条数据绘制
                for (int j = 0; j < 16; j++)
                    chart1.Series[j].Points.AddXY(itvalue.Time[i], itvalue.AvgCurrentValue[i]);
            }
        }

        /// <summary>
        /// 设置IT法结果的统计图谱值
        /// </summary>
        /// <param name="result"></param>
        public void SetData(List<ITValue> itvalues)
        {
            if (itvalues == null || itvalues.Count == 0)
                return;

            for (int i = 0; i < 16; i++)
                chart1.Series[i].Points.Clear();
        
            chartArea1.AxisY.LabelAutoFitStyle = LabelAutoFitStyles.IncreaseFont;

            List<double> maxvalues = new List<double>();
            List<double> minvalues = new List<double>();
            for(int i = 0; i < itvalues.Count; i++)
            {
                maxvalues.Add(itvalues[i].AvgCurrentValue.Max());
                minvalues.Add(itvalues[i].AvgCurrentValue.Min());
            }

            int Interval = (int)(Math.Abs(maxvalues.Max() - minvalues.Min()) * 0.1);

            if (Interval == 0)
                Interval = 1000;

            chartArea1.AxisY.Maximum = maxvalues.Max() + Interval;
            chartArea1.AxisY.Minimum = minvalues.Min() - Interval;
            chartArea1.AxisY.Interval = (chartArea1.AxisY.Maximum - chartArea1.AxisY.Minimum) / 10;
            chartArea1.AxisY.MajorGrid.Interval = (chartArea1.AxisY.Maximum - chartArea1.AxisY.Minimum) / 10;
            chartArea1.AxisY.MajorTickMark.Interval = (chartArea1.AxisY.Maximum - chartArea1.AxisY.Minimum) / 10;

            chartArea1.AxisX.Maximum = (int)(itvalues[0].Time.Max() + 1);
            chartArea1.AxisX.Interval = (int)(itvalues[0].Time.Max() / 5);
            chartArea1.AxisX.MajorGrid.Interval = (int)(itvalues[0].Time.Max() / 5);
            chartArea1.AxisX.MajorTickMark.Interval = (int)(itvalues[0].Time.Max() / 5);

            try
            {
                // 逐条数据绘制
                for (int i = 0; i < 16; i++)
                {
                    //for (int k = 0; k < itvalues[i * 2].AvgCurrentValue.Count; k++)
                    //{
                    //    chart1.Series[i].Points.AddXY(itvalues[i * 2].Time[k], itvalues[i * 2].AvgCurrentValue[k]);
                    //}
                    chart1.Series[i].Points.DataBindXY(itvalues[i * 2].Time, itvalues[i * 2].AvgCurrentValue);
                }
            }
            catch (Exception ex)
            {
                logger.Error("ITReal Test error:" + ex.ToString());
            }
        }

        private void MouseLeaveEvent(object sender, EventArgs e)
        {
            btnzoom.Visible = true;
        }

        private void MouseEnterEvent(object sender, EventArgs e)
        {
            btnzoom.Visible = true;
        }

        private void ITRealTest_Resize(object sender, EventArgs e)
        {
            btnzoom.Location = new Point(this.Location.X + this.Width - 20, this.Location.Y);
        }

        /// <summary>
        /// 放大显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnzoom_Click(object sender, EventArgs e)
        {
            if (zoomshow != null)
                zoomshow(Index);
        }
    }
}
