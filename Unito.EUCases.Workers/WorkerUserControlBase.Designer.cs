namespace Unito.EUCases.Workers
{
    partial class WorkerUserControlBase
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkerUserControlBase));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.startButton = new System.Windows.Forms.ToolStripButton();
            this.cancelButton = new System.Windows.Forms.ToolStripButton();
            this.resetButton = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.parametersAndResultsTabControl = new System.Windows.Forms.TabControl();
            this.parametersTabPage = new System.Windows.Forms.TabPage();
            this.parametersPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.parametersToolStrip = new System.Windows.Forms.ToolStrip();
            this.parametersOpenButton = new System.Windows.Forms.ToolStripButton();
            this.parametersSaveButton = new System.Windows.Forms.ToolStripButton();
            this.logTabPage = new System.Windows.Forms.TabPage();
            this.traceLogTextBox = new System.Windows.Forms.TextBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.clearTraceLogButton = new System.Windows.Forms.ToolStripButton();
            this.saveLogButton = new System.Windows.Forms.ToolStripButton();
            this.resultsTabPage = new System.Windows.Forms.TabPage();
            this.resultsPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.resultsToolStrip = new System.Windows.Forms.ToolStrip();
            this.resultsSaveButton = new System.Windows.Forms.ToolStripButton();
            this.usageTabPage = new System.Windows.Forms.TabPage();
            this.usageTextBox = new System.Windows.Forms.TextBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.logLevelDropDownList = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.customLogLevelsTextBox = new ToolStripSpringTextBox();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.parametersAndResultsTabControl.SuspendLayout();
            this.parametersTabPage.SuspendLayout();
            this.parametersToolStrip.SuspendLayout();
            this.logTabPage.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.resultsTabPage.SuspendLayout();
            this.resultsToolStrip.SuspendLayout();
            this.usageTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startButton,
            this.cancelButton,
            this.resetButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(620, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // startButton
            // 
            this.startButton.Image = ((System.Drawing.Image)(resources.GetObject("startButton.Image")));
            this.startButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(51, 22);
            this.startButton.Text = "Start";
            // 
            // cancelButton
            // 
            this.cancelButton.Image = global::Unito.EUCases.Workers.Properties.Resources.Stop;
            this.cancelButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(63, 22);
            this.cancelButton.Text = "Cancel";
            // 
            // resetButton
            // 
            this.resetButton.Image = global::Unito.EUCases.Workers.Properties.Resources.Reset;
            this.resetButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(55, 22);
            this.resetButton.Text = "Reset";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar,
            this.toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(620, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(73, 17);
            this.toolStripStatusLabel.Text = "<unknown>";
            // 
            // parametersAndResultsTabControl
            // 
            this.parametersAndResultsTabControl.Controls.Add(this.parametersTabPage);
            this.parametersAndResultsTabControl.Controls.Add(this.logTabPage);
            this.parametersAndResultsTabControl.Controls.Add(this.resultsTabPage);
            this.parametersAndResultsTabControl.Controls.Add(this.usageTabPage);
            this.parametersAndResultsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.parametersAndResultsTabControl.Location = new System.Drawing.Point(0, 25);
            this.parametersAndResultsTabControl.Name = "parametersAndResultsTabControl";
            this.parametersAndResultsTabControl.SelectedIndex = 0;
            this.parametersAndResultsTabControl.Size = new System.Drawing.Size(620, 403);
            this.parametersAndResultsTabControl.TabIndex = 2;
            // 
            // parametersTabPage
            // 
            this.parametersTabPage.Controls.Add(this.parametersPropertyGrid);
            this.parametersTabPage.Controls.Add(this.parametersToolStrip);
            this.parametersTabPage.Location = new System.Drawing.Point(4, 22);
            this.parametersTabPage.Name = "parametersTabPage";
            this.parametersTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.parametersTabPage.Size = new System.Drawing.Size(612, 377);
            this.parametersTabPage.TabIndex = 0;
            this.parametersTabPage.Text = "Parameters";
            this.parametersTabPage.UseVisualStyleBackColor = true;
            // 
            // parametersPropertyGrid
            // 
            this.parametersPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.parametersPropertyGrid.Location = new System.Drawing.Point(3, 28);
            this.parametersPropertyGrid.Name = "parametersPropertyGrid";
            this.parametersPropertyGrid.Size = new System.Drawing.Size(606, 346);
            this.parametersPropertyGrid.TabIndex = 2;
            // 
            // parametersToolStrip
            // 
            this.parametersToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.parametersOpenButton,
            this.parametersSaveButton});
            this.parametersToolStrip.Location = new System.Drawing.Point(3, 3);
            this.parametersToolStrip.Name = "parametersToolStrip";
            this.parametersToolStrip.Size = new System.Drawing.Size(606, 25);
            this.parametersToolStrip.TabIndex = 1;
            this.parametersToolStrip.Text = "toolStrip3";
            // 
            // parametersOpenButton
            // 
            this.parametersOpenButton.Image = global::Unito.EUCases.Workers.Properties.Resources.Open;
            this.parametersOpenButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.parametersOpenButton.Name = "parametersOpenButton";
            this.parametersOpenButton.Size = new System.Drawing.Size(56, 22);
            this.parametersOpenButton.Text = "Open";
            // 
            // parametersSaveButton
            // 
            this.parametersSaveButton.Image = global::Unito.EUCases.Workers.Properties.Resources.Save;
            this.parametersSaveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.parametersSaveButton.Name = "parametersSaveButton";
            this.parametersSaveButton.Size = new System.Drawing.Size(51, 22);
            this.parametersSaveButton.Text = "Save";
            // 
            // logTabPage
            // 
            this.logTabPage.Controls.Add(this.traceLogTextBox);
            this.logTabPage.Controls.Add(this.toolStrip2);
            this.logTabPage.Location = new System.Drawing.Point(4, 22);
            this.logTabPage.Name = "logTabPage";
            this.logTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.logTabPage.Size = new System.Drawing.Size(612, 377);
            this.logTabPage.TabIndex = 3;
            this.logTabPage.Text = "Trace";
            this.logTabPage.UseVisualStyleBackColor = true;
            // 
            // traceLogTextBox
            // 
            this.traceLogTextBox.BackColor = System.Drawing.Color.Black;
            this.traceLogTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.traceLogTextBox.ForeColor = System.Drawing.Color.White;
            this.traceLogTextBox.Location = new System.Drawing.Point(3, 28);
            this.traceLogTextBox.Multiline = true;
            this.traceLogTextBox.Name = "traceLogTextBox";
            this.traceLogTextBox.ReadOnly = true;
            this.traceLogTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.traceLogTextBox.Size = new System.Drawing.Size(606, 346);
            this.traceLogTextBox.TabIndex = 2;
            this.traceLogTextBox.WordWrap = false;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearTraceLogButton,
            this.saveLogButton,
            this.toolStripLabel1,
            this.logLevelDropDownList,
            this.toolStripLabel2,
            this.customLogLevelsTextBox});
            this.toolStrip2.Location = new System.Drawing.Point(3, 3);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(606, 25);
            this.toolStrip2.TabIndex = 1;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // clearTraceLogButton
            // 
            this.clearTraceLogButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.clearTraceLogButton.Image = ((System.Drawing.Image)(resources.GetObject("clearTraceLogButton.Image")));
            this.clearTraceLogButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.clearTraceLogButton.Name = "clearTraceLogButton";
            this.clearTraceLogButton.Size = new System.Drawing.Size(38, 22);
            this.clearTraceLogButton.Text = "Clear";
            this.clearTraceLogButton.Click += new System.EventHandler(this.clearTraceLogButton_Click);
            // 
            // saveLogButton
            // 
            this.saveLogButton.Image = global::Unito.EUCases.Workers.Properties.Resources.Save;
            this.saveLogButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveLogButton.Name = "saveLogButton";
            this.saveLogButton.Size = new System.Drawing.Size(51, 22);
            this.saveLogButton.Text = "Save";
            // 
            // resultsTabPage
            // 
            this.resultsTabPage.Controls.Add(this.resultsPropertyGrid);
            this.resultsTabPage.Controls.Add(this.resultsToolStrip);
            this.resultsTabPage.Location = new System.Drawing.Point(4, 22);
            this.resultsTabPage.Name = "resultsTabPage";
            this.resultsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.resultsTabPage.Size = new System.Drawing.Size(547, 377);
            this.resultsTabPage.TabIndex = 1;
            this.resultsTabPage.Text = "Results";
            this.resultsTabPage.UseVisualStyleBackColor = true;
            // 
            // resultsPropertyGrid
            // 
            this.resultsPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultsPropertyGrid.Location = new System.Drawing.Point(3, 28);
            this.resultsPropertyGrid.Name = "resultsPropertyGrid";
            this.resultsPropertyGrid.Size = new System.Drawing.Size(541, 346);
            this.resultsPropertyGrid.TabIndex = 3;
            // 
            // resultsToolStrip
            // 
            this.resultsToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resultsSaveButton});
            this.resultsToolStrip.Location = new System.Drawing.Point(3, 3);
            this.resultsToolStrip.Name = "resultsToolStrip";
            this.resultsToolStrip.Size = new System.Drawing.Size(541, 25);
            this.resultsToolStrip.TabIndex = 2;
            this.resultsToolStrip.Text = "resultsToolStrip";
            // 
            // resultsSaveButton
            // 
            this.resultsSaveButton.Image = global::Unito.EUCases.Workers.Properties.Resources.Save;
            this.resultsSaveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.resultsSaveButton.Name = "resultsSaveButton";
            this.resultsSaveButton.Size = new System.Drawing.Size(51, 22);
            this.resultsSaveButton.Text = "Save";
            // 
            // usageTabPage
            // 
            this.usageTabPage.Controls.Add(this.usageTextBox);
            this.usageTabPage.Location = new System.Drawing.Point(4, 22);
            this.usageTabPage.Name = "usageTabPage";
            this.usageTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.usageTabPage.Size = new System.Drawing.Size(547, 377);
            this.usageTabPage.TabIndex = 2;
            this.usageTabPage.Text = "Usage";
            this.usageTabPage.UseVisualStyleBackColor = true;
            // 
            // usageTextBox
            // 
            this.usageTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.usageTextBox.Location = new System.Drawing.Point(3, 3);
            this.usageTextBox.Multiline = true;
            this.usageTextBox.Name = "usageTextBox";
            this.usageTextBox.ReadOnly = true;
            this.usageTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.usageTextBox.Size = new System.Drawing.Size(541, 371);
            this.usageTextBox.TabIndex = 0;
            this.usageTextBox.Text = "<<usage>>";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(57, 22);
            this.toolStripLabel1.Text = "Log level:";
            // 
            // logLevelDropDownList
            // 
            this.logLevelDropDownList.Name = "logLevelDropDownList";
            this.logLevelDropDownList.Size = new System.Drawing.Size(121, 25);
            this.logLevelDropDownList.SelectedIndexChanged += new System.EventHandler(this.logLevelDropDownList_SelectedIndexChanged);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(81, 22);
            this.toolStripLabel2.Text = "Custom levels";
            // 
            // customLogLevelsTextBox
            // 
            this.customLogLevelsTextBox.Name = "customLogLevelsTextBox";
            this.customLogLevelsTextBox.Size = new System.Drawing.Size(100, 25);
            this.customLogLevelsTextBox.Leave += new System.EventHandler(this.customLogLevelsTextBox_Leave);
            // 
            // WorkerUserControlBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.parametersAndResultsTabControl);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "WorkerUserControlBase";
            this.Size = new System.Drawing.Size(620, 450);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.parametersAndResultsTabControl.ResumeLayout(false);
            this.parametersTabPage.ResumeLayout(false);
            this.parametersTabPage.PerformLayout();
            this.parametersToolStrip.ResumeLayout(false);
            this.parametersToolStrip.PerformLayout();
            this.logTabPage.ResumeLayout(false);
            this.logTabPage.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.resultsTabPage.ResumeLayout(false);
            this.resultsTabPage.PerformLayout();
            this.resultsToolStrip.ResumeLayout(false);
            this.resultsToolStrip.PerformLayout();
            this.usageTabPage.ResumeLayout(false);
            this.usageTabPage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        protected System.Windows.Forms.ToolStripButton startButton;
        protected System.Windows.Forms.ToolStripButton cancelButton;
        protected System.Windows.Forms.ToolStripButton resetButton;
        protected System.Windows.Forms.TabControl parametersAndResultsTabControl;
        private System.Windows.Forms.TabPage parametersTabPage;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton clearTraceLogButton;
        protected System.Windows.Forms.TabPage resultsTabPage;
        private System.Windows.Forms.TabPage usageTabPage;
        protected System.Windows.Forms.TextBox usageTextBox;
        protected System.Windows.Forms.TabPage logTabPage;
        protected System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        protected System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStrip parametersToolStrip;
        protected System.Windows.Forms.ToolStripButton parametersOpenButton;
        protected System.Windows.Forms.ToolStripButton parametersSaveButton;
        protected System.Windows.Forms.PropertyGrid parametersPropertyGrid;
        protected System.Windows.Forms.PropertyGrid resultsPropertyGrid;
        private System.Windows.Forms.ToolStrip resultsToolStrip;
        protected System.Windows.Forms.ToolStripButton resultsSaveButton;
        protected internal System.Windows.Forms.TextBox traceLogTextBox;
        protected System.Windows.Forms.ToolStripButton saveLogButton;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox logLevelDropDownList;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private ToolStripSpringTextBox customLogLevelsTextBox;
    }
}
