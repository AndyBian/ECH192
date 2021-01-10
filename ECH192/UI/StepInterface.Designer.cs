namespace ECH192.UI
{
    partial class StepInterface
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StepInterface));
            this.cbStep = new System.Windows.Forms.ComboBox();
            this.lbStep = new System.Windows.Forms.Label();
            this.txtTime = new System.Windows.Forms.TextBox();
            this.lbtime = new System.Windows.Forms.Label();
            this.CheckStatus = new System.Windows.Forms.CheckBox();
            this.btnRealTest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbStep
            // 
            this.cbStep.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStep.FormattingEnabled = true;
            this.cbStep.Location = new System.Drawing.Point(126, 7);
            this.cbStep.Margin = new System.Windows.Forms.Padding(5);
            this.cbStep.Name = "cbStep";
            this.cbStep.Size = new System.Drawing.Size(130, 29);
            this.cbStep.TabIndex = 0;
            this.cbStep.SelectedIndexChanged += new System.EventHandler(this.cbStep_SelectedIndexChanged);
            // 
            // lbStep
            // 
            this.lbStep.AutoSize = true;
            this.lbStep.Location = new System.Drawing.Point(56, 10);
            this.lbStep.Name = "lbStep";
            this.lbStep.Size = new System.Drawing.Size(58, 21);
            this.lbStep.TabIndex = 1;
            this.lbStep.Text = "步骤：";
            // 
            // txtTime
            // 
            this.txtTime.Location = new System.Drawing.Point(261, 7);
            this.txtTime.MaxLength = 5;
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(80, 29);
            this.txtTime.TabIndex = 2;
            this.txtTime.Text = "100";
            this.txtTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTime.TextChanged += new System.EventHandler(this.txtTime_TextChanged);
            // 
            // lbtime
            // 
            this.lbtime.AutoSize = true;
            this.lbtime.Location = new System.Drawing.Point(344, 12);
            this.lbtime.Name = "lbtime";
            this.lbtime.Size = new System.Drawing.Size(17, 21);
            this.lbtime.TabIndex = 3;
            this.lbtime.Text = "s";
            // 
            // CheckStatus
            // 
            this.CheckStatus.AutoSize = true;
            this.CheckStatus.Location = new System.Drawing.Point(38, 14);
            this.CheckStatus.Name = "CheckStatus";
            this.CheckStatus.Size = new System.Drawing.Size(15, 14);
            this.CheckStatus.TabIndex = 5;
            this.CheckStatus.UseVisualStyleBackColor = true;
            this.CheckStatus.CheckedChanged += new System.EventHandler(this.CheckStatus_CheckedChanged);
            // 
            // btnRealTest
            // 
            this.btnRealTest.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRealTest.BackgroundImage")));
            this.btnRealTest.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRealTest.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnRealTest.FlatAppearance.BorderSize = 0;
            this.btnRealTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRealTest.ForeColor = System.Drawing.SystemColors.Control;
            this.btnRealTest.Location = new System.Drawing.Point(1, 6);
            this.btnRealTest.Name = "btnRealTest";
            this.btnRealTest.Size = new System.Drawing.Size(30, 30);
            this.btnRealTest.TabIndex = 4;
            this.btnRealTest.UseVisualStyleBackColor = true;
            this.btnRealTest.Click += new System.EventHandler(this.btnRealTest_Click);
            // 
            // StepInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CheckStatus);
            this.Controls.Add(this.btnRealTest);
            this.Controls.Add(this.lbtime);
            this.Controls.Add(this.txtTime);
            this.Controls.Add(this.lbStep);
            this.Controls.Add(this.cbStep);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(169)))), ((int)(((byte)(206)))));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "StepInterface";
            this.Size = new System.Drawing.Size(368, 43);
            this.Load += new System.EventHandler(this.StepInterface_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.StepInterface_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbStep;
        private System.Windows.Forms.Label lbStep;
        private System.Windows.Forms.TextBox txtTime;
        private System.Windows.Forms.Label lbtime;
        private System.Windows.Forms.Button btnRealTest;
        private System.Windows.Forms.CheckBox CheckStatus;
    }
}
