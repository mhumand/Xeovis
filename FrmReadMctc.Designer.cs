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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabpageDetailed = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbxModelVariable = new System.Windows.Forms.ComboBox();
            this.ccMctcPlot = new DatabaseReader.CustomControls.ChartControl();
            this.tcBatchDetails = new DatabaseReader.CustomControls.TableControl();
            this.label3 = new System.Windows.Forms.Label();
            this.tabpageOverview = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ckbxExcludeIrrelevantBatches = new System.Windows.Forms.CheckBox();
            this.nudMctcRunNumber = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.tbMctcRunNumber = new System.Windows.Forms.TrackBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tcMctcDetails = new DatabaseReader.CustomControls.TableControl();
            this.menuStrip.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabpageDetailed.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabpageOverview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMctcRunNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMctcRunNumber)).BeginInit();
            this.groupBox1.SuspendLayout();
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
            this.tvCaseSelection.Size = new System.Drawing.Size(164, 698);
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
            this.tsmSettings});
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
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabpageDetailed);
            this.tabControl1.Controls.Add(this.tabpageOverview);
            this.tabControl1.Location = new System.Drawing.Point(182, 120);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1169, 634);
            this.tabControl1.TabIndex = 12;
            // 
            // tabpageDetailed
            // 
            this.tabpageDetailed.Controls.Add(this.splitContainer1);
            this.tabpageDetailed.Location = new System.Drawing.Point(4, 22);
            this.tabpageDetailed.Name = "tabpageDetailed";
            this.tabpageDetailed.Padding = new System.Windows.Forms.Padding(3);
            this.tabpageDetailed.Size = new System.Drawing.Size(1161, 608);
            this.tabpageDetailed.TabIndex = 0;
            this.tabpageDetailed.Text = "Detailed";
            this.tabpageDetailed.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.cmbxModelVariable);
            this.splitContainer1.Panel1.Controls.Add(this.ccMctcPlot);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tcBatchDetails);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Size = new System.Drawing.Size(1155, 602);
            this.splitContainer1.SplitterDistance = 602;
            this.splitContainer1.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Model Variable";
            // 
            // cmbxModelVariable
            // 
            this.cmbxModelVariable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbxModelVariable.FormattingEnabled = true;
            this.cmbxModelVariable.Location = new System.Drawing.Point(93, 6);
            this.cmbxModelVariable.Name = "cmbxModelVariable";
            this.cmbxModelVariable.Size = new System.Drawing.Size(121, 21);
            this.cmbxModelVariable.TabIndex = 5;
            this.cmbxModelVariable.SelectedIndexChanged += new System.EventHandler(this.cmbxModelVariable_SelectedIndexChanged);
            // 
            // ccMctcPlot
            // 
            this.ccMctcPlot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ccMctcPlot.Location = new System.Drawing.Point(3, 32);
            this.ccMctcPlot.Name = "ccMctcPlot";
            this.ccMctcPlot.Size = new System.Drawing.Size(596, 593);
            this.ccMctcPlot.TabIndex = 4;
            this.ccMctcPlot.UseLabelFiltering = false;
            // 
            // tcBatchDetails
            // 
            this.tcBatchDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcBatchDetails.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcBatchDetails.Location = new System.Drawing.Point(12, 25);
            this.tcBatchDetails.MultiSelect = true;
            this.tcBatchDetails.Name = "tcBatchDetails";
            this.tcBatchDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect;
            this.tcBatchDetails.Size = new System.Drawing.Size(524, 537);
            this.tcBatchDetails.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Batch Details";
            // 
            // tabpageOverview
            // 
            this.tabpageOverview.Controls.Add(this.tableLayoutPanel);
            this.tabpageOverview.Location = new System.Drawing.Point(4, 22);
            this.tabpageOverview.Name = "tabpageOverview";
            this.tabpageOverview.Padding = new System.Windows.Forms.Padding(3);
            this.tabpageOverview.Size = new System.Drawing.Size(1161, 623);
            this.tabpageOverview.TabIndex = 1;
            this.tabpageOverview.Text = "Overview";
            this.tabpageOverview.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.AutoScroll = true;
            this.tableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
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
            this.tableLayoutPanel.Size = new System.Drawing.Size(1155, 617);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // ckbxExcludeIrrelevantBatches
            // 
            this.ckbxExcludeIrrelevantBatches.AutoSize = true;
            this.ckbxExcludeIrrelevantBatches.Checked = true;
            this.ckbxExcludeIrrelevantBatches.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbxExcludeIrrelevantBatches.Location = new System.Drawing.Point(13, 64);
            this.ckbxExcludeIrrelevantBatches.Name = "ckbxExcludeIrrelevantBatches";
            this.ckbxExcludeIrrelevantBatches.Size = new System.Drawing.Size(158, 17);
            this.ckbxExcludeIrrelevantBatches.TabIndex = 7;
            this.ckbxExcludeIrrelevantBatches.Text = "Exclude Irrelevant Batches";
            this.ckbxExcludeIrrelevantBatches.UseVisualStyleBackColor = true;
            this.ckbxExcludeIrrelevantBatches.CheckedChanged += new System.EventHandler(this.ckbxExcludeIrrelevantBatches_CheckedChanged);
            // 
            // nudMctcRunNumber
            // 
            this.nudMctcRunNumber.Location = new System.Drawing.Point(92, 27);
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
            this.label2.Location = new System.Drawing.Point(10, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Mctc Run No.";
            // 
            // tbMctcRunNumber
            // 
            this.tbMctcRunNumber.AutoSize = false;
            this.tbMctcRunNumber.Location = new System.Drawing.Point(152, 28);
            this.tbMctcRunNumber.Minimum = 1;
            this.tbMctcRunNumber.Name = "tbMctcRunNumber";
            this.tbMctcRunNumber.Size = new System.Drawing.Size(212, 21);
            this.tbMctcRunNumber.TabIndex = 1;
            this.tbMctcRunNumber.Value = 1;
            this.tbMctcRunNumber.Scroll += new System.EventHandler(this.tbMctcRunNumber_Scroll);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.ckbxExcludeIrrelevantBatches);
            this.groupBox1.Controls.Add(this.tcMctcDetails);
            this.groupBox1.Controls.Add(this.tbMctcRunNumber);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.nudMctcRunNumber);
            this.groupBox1.Location = new System.Drawing.Point(182, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1169, 87);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "MCTC";
            // 
            // tcMctcDetails
            // 
            this.tcMctcDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcMctcDetails.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcMctcDetails.Location = new System.Drawing.Point(370, 12);
            this.tcMctcDetails.MultiSelect = true;
            this.tcMctcDetails.Name = "tcMctcDetails";
            this.tcMctcDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect;
            this.tcMctcDetails.Size = new System.Drawing.Size(792, 69);
            this.tcMctcDetails.TabIndex = 9;
            // 
            // FrmReadMctc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1363, 766);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnPlotAndTabulate);
            this.Controls.Add(this.tvCaseSelection);
            this.Controls.Add(this.menuStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "FrmReadMctc";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Read MCTC";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabpageDetailed.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabpageOverview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudMctcRunNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMctcRunNumber)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private System.Windows.Forms.SplitContainer splitContainer1;
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
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabpageDetailed;
        private System.Windows.Forms.TabPage tabpageOverview;
        private System.Windows.Forms.TrackBar tbMctcRunNumber;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.NumericUpDown nudMctcRunNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private CustomControls.TableControl tcMctcDetails;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}