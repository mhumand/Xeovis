namespace DatabaseReader
{
    partial class FrmReadMctc
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tvCaseSelection = new System.Windows.Forms.TreeView();
            this.btnPlotAndTabulate = new System.Windows.Forms.Button();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.tsmFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmSave = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmReadMCTC = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmResetMCTCDirectory = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmVariableDefinitions = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmPlotSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmFixedUIOnReading = new System.Windows.Forms.ToolStripMenuItem();
            this.calibrationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmCalibration = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControlResults = new System.Windows.Forms.TabControl();
            this.tabpageDetailed = new System.Windows.Forms.TabPage();
            this.ckbxShowBatchDetails = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainerDetailedTab = new System.Windows.Forms.SplitContainer();
            this.lblBatchDetails = new System.Windows.Forms.Label();
            this.cmbxModelVariable = new System.Windows.Forms.ComboBox();
            this.tabpageOverview = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ckbxExcludeIrrelevantBatches = new System.Windows.Forms.CheckBox();
            this.nudMctcRunNumber = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.tbMctcRunNumber = new System.Windows.Forms.TrackBar();
            this.tabControlMCTC = new System.Windows.Forms.TabControl();
            this.tabPageMCTC = new System.Windows.Forms.TabPage();
            this.nudLayoutColumnCount = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbxPlotPointsGrouping = new System.Windows.Forms.ComboBox();
            this.tcMctcDetails = new DatabaseReader.CustomControls.TableControl();
            this.ccMctcPlot = new DatabaseReader.CustomControls.ChartControl();
            this.tcBatchDetails = new DatabaseReader.CustomControls.TableControl();
            this.menuStrip.SuspendLayout();
            this.tabControlResults.SuspendLayout();
            this.tabpageDetailed.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDetailedTab)).BeginInit();
            this.splitContainerDetailedTab.Panel1.SuspendLayout();
            this.splitContainerDetailedTab.Panel2.SuspendLayout();
            this.splitContainerDetailedTab.SuspendLayout();
            this.tabpageOverview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMctcRunNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMctcRunNumber)).BeginInit();
            this.tabControlMCTC.SuspendLayout();
            this.tabPageMCTC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLayoutColumnCount)).BeginInit();
            this.SuspendLayout();
            // 
            // tvCaseSelection
            // 
            this.tvCaseSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tvCaseSelection.CheckBoxes = true;
            this.tvCaseSelection.FullRowSelect = true;
            this.tvCaseSelection.Location = new System.Drawing.Point(12, 56);
            this.tvCaseSelection.Name = "tvCaseSelection";
            this.tvCaseSelection.Size = new System.Drawing.Size(164, 716);
            this.tvCaseSelection.TabIndex = 1;
            this.tvCaseSelection.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvCaseSelection_AfterCheck);
            this.tvCaseSelection.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tvCaseSelection_MouseClick);
            // 
            // btnPlotAndTabulate
            // 
            this.btnPlotAndTabulate.Location = new System.Drawing.Point(12, 27);
            this.btnPlotAndTabulate.Name = "btnPlotAndTabulate";
            this.btnPlotAndTabulate.Size = new System.Drawing.Size(164, 23);
            this.btnPlotAndTabulate.TabIndex = 2;
            this.btnPlotAndTabulate.Text = "Plot and Tabulate";
            this.btnPlotAndTabulate.UseVisualStyleBackColor = true;
            this.btnPlotAndTabulate.Click += new System.EventHandler(this.btnPlotAndTabulate_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmFile,
            this.tsmSettings,
            this.calibrationToolStripMenuItem});
            this.menuStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1363, 24);
            this.menuStrip.TabIndex = 11;
            this.menuStrip.Text = "menuStrip1";
            // 
            // tsmFile
            // 
            this.tsmFile.BackColor = System.Drawing.SystemColors.Control;
            this.tsmFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmOpen,
            this.tsmSave,
            this.tsmReadMCTC,
            this.tsmResetMCTCDirectory});
            this.tsmFile.Name = "tsmFile";
            this.tsmFile.Size = new System.Drawing.Size(37, 20);
            this.tsmFile.Text = "File";
            this.tsmFile.DropDownOpening += new System.EventHandler(this.tsmFile_DropDownOpening);
            // 
            // tsmOpen
            // 
            this.tsmOpen.Name = "tsmOpen";
            this.tsmOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.tsmOpen.Size = new System.Drawing.Size(216, 22);
            this.tsmOpen.Text = "Open";
            this.tsmOpen.Click += new System.EventHandler(this.tsmOpen_Click);
            // 
            // tsmSave
            // 
            this.tsmSave.Name = "tsmSave";
            this.tsmSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.tsmSave.Size = new System.Drawing.Size(216, 22);
            this.tsmSave.Text = "Save";
            this.tsmSave.Click += new System.EventHandler(this.tsmSave_Click);
            // 
            // tsmReadMCTC
            // 
            this.tsmReadMCTC.Name = "tsmReadMCTC";
            this.tsmReadMCTC.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.tsmReadMCTC.Size = new System.Drawing.Size(216, 22);
            this.tsmReadMCTC.Text = "Read MCTC Results";
            this.tsmReadMCTC.Click += new System.EventHandler(this.tsmReadMCTC_Click);
            // 
            // tsmResetMCTCDirectory
            // 
            this.tsmResetMCTCDirectory.Name = "tsmResetMCTCDirectory";
            this.tsmResetMCTCDirectory.Size = new System.Drawing.Size(216, 22);
            this.tsmResetMCTCDirectory.Text = "Reset MCTC Directory";
            this.tsmResetMCTCDirectory.Click += new System.EventHandler(this.tsmResetMCTCDirectory_Click);
            // 
            // tsmSettings
            // 
            this.tsmSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmVariableDefinitions,
            this.tsmPlotSettings,
            this.tsmFixedUIOnReading});
            this.tsmSettings.Name = "tsmSettings";
            this.tsmSettings.Size = new System.Drawing.Size(61, 20);
            this.tsmSettings.Text = "Settings";
            // 
            // tsmVariableDefinitions
            // 
            this.tsmVariableDefinitions.Name = "tsmVariableDefinitions";
            this.tsmVariableDefinitions.Size = new System.Drawing.Size(176, 22);
            this.tsmVariableDefinitions.Text = "Variables Definition";
            this.tsmVariableDefinitions.Click += new System.EventHandler(this.tsmVariableDefinitions_Click);
            // 
            // tsmPlotSettings
            // 
            this.tsmPlotSettings.Name = "tsmPlotSettings";
            this.tsmPlotSettings.Size = new System.Drawing.Size(176, 22);
            this.tsmPlotSettings.Text = "Plot Settings";
            this.tsmPlotSettings.Click += new System.EventHandler(this.tsmPlotSettings_Click);
            // 
            // tsmFixedUIOnReading
            // 
            this.tsmFixedUIOnReading.CheckOnClick = true;
            this.tsmFixedUIOnReading.Name = "tsmFixedUIOnReading";
            this.tsmFixedUIOnReading.Size = new System.Drawing.Size(176, 22);
            this.tsmFixedUIOnReading.Text = "Fixed UI on reading";
            // 
            // calibrationToolStripMenuItem
            // 
            this.calibrationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmCalibration});
            this.calibrationToolStripMenuItem.Name = "calibrationToolStripMenuItem";
            this.calibrationToolStripMenuItem.Size = new System.Drawing.Size(97, 20);
            this.calibrationToolStripMenuItem.Text = "THERMOCALC";
            // 
            // tsmCalibration
            // 
            this.tsmCalibration.Name = "tsmCalibration";
            this.tsmCalibration.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
            this.tsmCalibration.Size = new System.Drawing.Size(206, 22);
            this.tsmCalibration.Text = "Calibration";
            this.tsmCalibration.Click += new System.EventHandler(this.tsmCalibration_Click);
            // 
            // tabControlResults
            // 
            this.tabControlResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlResults.Controls.Add(this.tabpageDetailed);
            this.tabControlResults.Controls.Add(this.tabpageOverview);
            this.tabControlResults.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControlResults.Location = new System.Drawing.Point(182, 138);
            this.tabControlResults.Name = "tabControlResults";
            this.tabControlResults.SelectedIndex = 0;
            this.tabControlResults.Size = new System.Drawing.Size(1169, 634);
            this.tabControlResults.TabIndex = 12;
            // 
            // tabpageDetailed
            // 
            this.tabpageDetailed.Controls.Add(this.cmbxPlotPointsGrouping);
            this.tabpageDetailed.Controls.Add(this.ckbxShowBatchDetails);
            this.tabpageDetailed.Controls.Add(this.label3);
            this.tabpageDetailed.Controls.Add(this.label1);
            this.tabpageDetailed.Controls.Add(this.splitContainerDetailedTab);
            this.tabpageDetailed.Controls.Add(this.cmbxModelVariable);
            this.tabpageDetailed.Location = new System.Drawing.Point(4, 22);
            this.tabpageDetailed.Name = "tabpageDetailed";
            this.tabpageDetailed.Padding = new System.Windows.Forms.Padding(3);
            this.tabpageDetailed.Size = new System.Drawing.Size(1161, 608);
            this.tabpageDetailed.TabIndex = 0;
            this.tabpageDetailed.Text = "Detailed";
            this.tabpageDetailed.UseVisualStyleBackColor = true;
            // 
            // ckbxShowBatchDetails
            // 
            this.ckbxShowBatchDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ckbxShowBatchDetails.AutoSize = true;
            this.ckbxShowBatchDetails.Checked = true;
            this.ckbxShowBatchDetails.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbxShowBatchDetails.Location = new System.Drawing.Point(1034, 10);
            this.ckbxShowBatchDetails.Name = "ckbxShowBatchDetails";
            this.ckbxShowBatchDetails.Size = new System.Drawing.Size(121, 17);
            this.ckbxShowBatchDetails.TabIndex = 10;
            this.ckbxShowBatchDetails.Text = "Show details table";
            this.ckbxShowBatchDetails.UseVisualStyleBackColor = true;
            this.ckbxShowBatchDetails.CheckedChanged += new System.EventHandler(this.ckbxShowBatchDetails_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Model Variable";
            // 
            // splitContainerDetailedTab
            // 
            this.splitContainerDetailedTab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerDetailedTab.Location = new System.Drawing.Point(3, 33);
            this.splitContainerDetailedTab.Name = "splitContainerDetailedTab";
            // 
            // splitContainerDetailedTab.Panel1
            // 
            this.splitContainerDetailedTab.Panel1.AutoScroll = true;
            this.splitContainerDetailedTab.Panel1.Controls.Add(this.ccMctcPlot);
            this.splitContainerDetailedTab.Panel1MinSize = 100;
            // 
            // splitContainerDetailedTab.Panel2
            // 
            this.splitContainerDetailedTab.Panel2.AutoScroll = true;
            this.splitContainerDetailedTab.Panel2.Controls.Add(this.tcBatchDetails);
            this.splitContainerDetailedTab.Panel2.Controls.Add(this.lblBatchDetails);
            this.splitContainerDetailedTab.Panel2MinSize = 100;
            this.splitContainerDetailedTab.Size = new System.Drawing.Size(1155, 572);
            this.splitContainerDetailedTab.SplitterDistance = 540;
            this.splitContainerDetailedTab.TabIndex = 9;
            // 
            // lblBatchDetails
            // 
            this.lblBatchDetails.AutoSize = true;
            this.lblBatchDetails.Location = new System.Drawing.Point(9, 9);
            this.lblBatchDetails.Name = "lblBatchDetails";
            this.lblBatchDetails.Size = new System.Drawing.Size(76, 13);
            this.lblBatchDetails.TabIndex = 6;
            this.lblBatchDetails.Text = "Batch Details:";
            // 
            // cmbxModelVariable
            // 
            this.cmbxModelVariable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbxModelVariable.FormattingEnabled = true;
            this.cmbxModelVariable.Location = new System.Drawing.Point(96, 6);
            this.cmbxModelVariable.Name = "cmbxModelVariable";
            this.cmbxModelVariable.Size = new System.Drawing.Size(121, 21);
            this.cmbxModelVariable.TabIndex = 5;
            this.cmbxModelVariable.SelectedIndexChanged += new System.EventHandler(this.cmbxModelVariable_SelectedIndexChanged);
            // 
            // tabpageOverview
            // 
            this.tabpageOverview.Controls.Add(this.tableLayoutPanel);
            this.tabpageOverview.Location = new System.Drawing.Point(4, 22);
            this.tabpageOverview.Name = "tabpageOverview";
            this.tabpageOverview.Padding = new System.Windows.Forms.Padding(3);
            this.tabpageOverview.Size = new System.Drawing.Size(1161, 608);
            this.tabpageOverview.TabIndex = 1;
            this.tabpageOverview.Text = "Overview";
            this.tabpageOverview.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.AutoScroll = true;
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(1155, 602);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // ckbxExcludeIrrelevantBatches
            // 
            this.ckbxExcludeIrrelevantBatches.AutoSize = true;
            this.ckbxExcludeIrrelevantBatches.Checked = true;
            this.ckbxExcludeIrrelevantBatches.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbxExcludeIrrelevantBatches.Location = new System.Drawing.Point(147, 57);
            this.ckbxExcludeIrrelevantBatches.Name = "ckbxExcludeIrrelevantBatches";
            this.ckbxExcludeIrrelevantBatches.Size = new System.Drawing.Size(158, 17);
            this.ckbxExcludeIrrelevantBatches.TabIndex = 7;
            this.ckbxExcludeIrrelevantBatches.Text = "Exclude Irrelevant Batches";
            this.ckbxExcludeIrrelevantBatches.UseVisualStyleBackColor = true;
            this.ckbxExcludeIrrelevantBatches.CheckedChanged += new System.EventHandler(this.ckbxExcludeIrrelevantBatches_CheckedChanged);
            // 
            // nudMctcRunNumber
            // 
            this.nudMctcRunNumber.Location = new System.Drawing.Point(85, 21);
            this.nudMctcRunNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMctcRunNumber.Name = "nudMctcRunNumber";
            this.nudMctcRunNumber.Size = new System.Drawing.Size(54, 22);
            this.nudMctcRunNumber.TabIndex = 8;
            this.nudMctcRunNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudMctcRunNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMctcRunNumber.ValueChanged += new System.EventHandler(this.nudMctcRunNumber_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Mctc Run No.";
            // 
            // tbMctcRunNumber
            // 
            this.tbMctcRunNumber.AutoSize = false;
            this.tbMctcRunNumber.Location = new System.Drawing.Point(147, 22);
            this.tbMctcRunNumber.Minimum = 1;
            this.tbMctcRunNumber.Name = "tbMctcRunNumber";
            this.tbMctcRunNumber.Size = new System.Drawing.Size(212, 21);
            this.tbMctcRunNumber.TabIndex = 1;
            this.tbMctcRunNumber.Value = 1;
            this.tbMctcRunNumber.Scroll += new System.EventHandler(this.tbMctcRunNumber_Scroll);
            // 
            // tabControlMCTC
            // 
            this.tabControlMCTC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlMCTC.Controls.Add(this.tabPageMCTC);
            this.tabControlMCTC.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControlMCTC.Location = new System.Drawing.Point(182, 27);
            this.tabControlMCTC.Name = "tabControlMCTC";
            this.tabControlMCTC.SelectedIndex = 0;
            this.tabControlMCTC.Size = new System.Drawing.Size(1169, 109);
            this.tabControlMCTC.TabIndex = 14;
            // 
            // tabPageMCTC
            // 
            this.tabPageMCTC.Controls.Add(this.ckbxExcludeIrrelevantBatches);
            this.tabPageMCTC.Controls.Add(this.tcMctcDetails);
            this.tabPageMCTC.Controls.Add(this.nudLayoutColumnCount);
            this.tabPageMCTC.Controls.Add(this.nudMctcRunNumber);
            this.tabPageMCTC.Controls.Add(this.tbMctcRunNumber);
            this.tabPageMCTC.Controls.Add(this.label4);
            this.tabPageMCTC.Controls.Add(this.label2);
            this.tabPageMCTC.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPageMCTC.Location = new System.Drawing.Point(4, 22);
            this.tabPageMCTC.Name = "tabPageMCTC";
            this.tabPageMCTC.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMCTC.Size = new System.Drawing.Size(1161, 83);
            this.tabPageMCTC.TabIndex = 0;
            this.tabPageMCTC.Text = "MCTC";
            this.tabPageMCTC.UseVisualStyleBackColor = true;
            // 
            // nudLayoutColumnCount
            // 
            this.nudLayoutColumnCount.Location = new System.Drawing.Point(85, 55);
            this.nudLayoutColumnCount.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudLayoutColumnCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudLayoutColumnCount.Name = "nudLayoutColumnCount";
            this.nudLayoutColumnCount.Size = new System.Drawing.Size(54, 22);
            this.nudLayoutColumnCount.TabIndex = 8;
            this.nudLayoutColumnCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudLayoutColumnCount.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nudLayoutColumnCount.ValueChanged += new System.EventHandler(this.nudLayoutColumnCount_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Columns No.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(250, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Group by";
            // 
            // cmbxPlotPointsGrouping
            // 
            this.cmbxPlotPointsGrouping.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbxPlotPointsGrouping.Enabled = false;
            this.cmbxPlotPointsGrouping.FormattingEnabled = true;
            this.cmbxPlotPointsGrouping.Location = new System.Drawing.Point(311, 6);
            this.cmbxPlotPointsGrouping.Name = "cmbxPlotPointsGrouping";
            this.cmbxPlotPointsGrouping.Size = new System.Drawing.Size(121, 21);
            this.cmbxPlotPointsGrouping.TabIndex = 11;
            // 
            // tcMctcDetails
            // 
            this.tcMctcDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcMctcDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tcMctcDetails.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcMctcDetails.Location = new System.Drawing.Point(365, 3);
            this.tcMctcDetails.MultiSelect = true;
            this.tcMctcDetails.Name = "tcMctcDetails";
            this.tcMctcDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect;
            this.tcMctcDetails.Size = new System.Drawing.Size(790, 77);
            this.tcMctcDetails.TabIndex = 9;
            // 
            // ccMctcPlot
            // 
            this.ccMctcPlot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ccMctcPlot.Location = new System.Drawing.Point(3, 9);
            this.ccMctcPlot.Name = "ccMctcPlot";
            this.ccMctcPlot.Size = new System.Drawing.Size(534, 558);
            this.ccMctcPlot.TabIndex = 4;
            this.ccMctcPlot.UseLabelFiltering = false;
            // 
            // tcBatchDetails
            // 
            this.tcBatchDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcBatchDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tcBatchDetails.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcBatchDetails.Location = new System.Drawing.Point(12, 32);
            this.tcBatchDetails.MultiSelect = true;
            this.tcBatchDetails.Name = "tcBatchDetails";
            this.tcBatchDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect;
            this.tcBatchDetails.Size = new System.Drawing.Size(586, 534);
            this.tcBatchDetails.TabIndex = 8;
            // 
            // FrmReadMctc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1363, 784);
            this.Controls.Add(this.tabControlMCTC);
            this.Controls.Add(this.tabControlResults);
            this.Controls.Add(this.btnPlotAndTabulate);
            this.Controls.Add(this.tvCaseSelection);
            this.Controls.Add(this.menuStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "FrmReadMctc";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Read MCTC";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.tabControlResults.ResumeLayout(false);
            this.tabpageDetailed.ResumeLayout(false);
            this.tabpageDetailed.PerformLayout();
            this.splitContainerDetailedTab.Panel1.ResumeLayout(false);
            this.splitContainerDetailedTab.Panel2.ResumeLayout(false);
            this.splitContainerDetailedTab.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDetailedTab)).EndInit();
            this.splitContainerDetailedTab.ResumeLayout(false);
            this.tabpageOverview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudMctcRunNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMctcRunNumber)).EndInit();
            this.tabControlMCTC.ResumeLayout(false);
            this.tabPageMCTC.ResumeLayout(false);
            this.tabPageMCTC.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLayoutColumnCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TreeView tvCaseSelection;
        private System.Windows.Forms.Button btnPlotAndTabulate;
        private CustomControls.ChartControl ccMctcPlot;
        private System.Windows.Forms.ComboBox cmbxModelVariable;
        private System.Windows.Forms.Label label1;
        private CustomControls.TableControl tcBatchDetails;
        private System.Windows.Forms.SplitContainer splitContainerDetailedTab;
        private System.Windows.Forms.CheckBox ckbxExcludeIrrelevantBatches;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem tsmFile;
        private System.Windows.Forms.ToolStripMenuItem tsmOpen;
        private System.Windows.Forms.ToolStripMenuItem tsmSave;
        private System.Windows.Forms.ToolStripMenuItem tsmSettings;
        private System.Windows.Forms.ToolStripMenuItem tsmVariableDefinitions;
        private System.Windows.Forms.ToolStripMenuItem tsmPlotSettings;
        private System.Windows.Forms.ToolStripMenuItem tsmReadMCTC;
        private System.Windows.Forms.ToolStripMenuItem tsmResetMCTCDirectory;
        private System.Windows.Forms.ToolStripMenuItem tsmFixedUIOnReading;
        private System.Windows.Forms.TabControl tabControlResults;
        private System.Windows.Forms.TabPage tabpageDetailed;
        private System.Windows.Forms.TabPage tabpageOverview;
        private System.Windows.Forms.TrackBar tbMctcRunNumber;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.NumericUpDown nudMctcRunNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblBatchDetails;
        private CustomControls.TableControl tcMctcDetails;
        private System.Windows.Forms.TabControl tabControlMCTC;
        private System.Windows.Forms.TabPage tabPageMCTC;
        private System.Windows.Forms.NumericUpDown nudLayoutColumnCount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripMenuItem calibrationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmCalibration;
        private System.Windows.Forms.CheckBox ckbxShowBatchDetails;
        private System.Windows.Forms.ComboBox cmbxPlotPointsGrouping;
        private System.Windows.Forms.Label label3;
    }
}