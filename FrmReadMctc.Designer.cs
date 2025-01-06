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
            this.tabpageOverview = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.nudMctcRunNumber = new System.Windows.Forms.NumericUpDown();
            this.ckbxExcludeIrrelevantBatches = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbxModelVariable = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ccMctcPlot = new DatabaseReader.CustomControls.ChartControl();
            this.tcMctcDetails = new DatabaseReader.CustomControls.TableControl();
            this.tcBatchDetails = new DatabaseReader.CustomControls.TableControl();
            this.menuStrip.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabpageDetailed.SuspendLayout();
            this.tabpageOverview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMctcRunNumber)).BeginInit();
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
            this.tvCaseSelection.Size = new System.Drawing.Size(181, 698);
            this.tvCaseSelection.TabIndex = 1;
            this.tvCaseSelection.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvCaseSelection_AfterCheck);
            this.tvCaseSelection.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tvCaseSelection_MouseClick);
            // 
            // btnPlotAndTabulate
            // 
            this.btnPlotAndTabulate.Location = new System.Drawing.Point(12, 27);
            this.btnPlotAndTabulate.Name = "btnPlotAndTabulate";
            this.btnPlotAndTabulate.Size = new System.Drawing.Size(181, 23);
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
            this.tabControl1.Location = new System.Drawing.Point(202, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1149, 727);
            this.tabControl1.TabIndex = 12;
            // 
            // tabpageDetailed
            // 
            this.tabpageDetailed.Controls.Add(this.splitContainer1);
            this.tabpageDetailed.Location = new System.Drawing.Point(4, 22);
            this.tabpageDetailed.Name = "tabpageDetailed";
            this.tabpageDetailed.Padding = new System.Windows.Forms.Padding(3);
            this.tabpageDetailed.Size = new System.Drawing.Size(1141, 701);
            this.tabpageDetailed.TabIndex = 0;
            this.tabpageDetailed.Text = "Detailed";
            this.tabpageDetailed.UseVisualStyleBackColor = true;
            // 
            // tabpageOverview
            // 
            this.tabpageOverview.Controls.Add(this.trackBar1);
            this.tabpageOverview.Controls.Add(this.tableLayoutPanel1);
            this.tabpageOverview.Location = new System.Drawing.Point(4, 22);
            this.tabpageOverview.Name = "tabpageOverview";
            this.tabpageOverview.Padding = new System.Windows.Forms.Padding(3);
            this.tabpageOverview.Size = new System.Drawing.Size(1141, 701);
            this.tabpageOverview.TabIndex = 1;
            this.tabpageOverview.Text = "Overview";
            this.tabpageOverview.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 57);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1129, 638);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(84, 6);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(331, 45);
            this.trackBar1.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.nudMctcRunNumber);
            this.splitContainer1.Panel1.Controls.Add(this.ckbxExcludeIrrelevantBatches);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.cmbxModelVariable);
            this.splitContainer1.Panel1.Controls.Add(this.ccMctcPlot);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tcMctcDetails);
            this.splitContainer1.Panel2.Controls.Add(this.tcBatchDetails);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Size = new System.Drawing.Size(1135, 695);
            this.splitContainer1.SplitterDistance = 621;
            this.splitContainer1.TabIndex = 9;
            // 
            // nudMctcRunNumber
            // 
            this.nudMctcRunNumber.Location = new System.Drawing.Point(105, 7);
            this.nudMctcRunNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMctcRunNumber.Name = "nudMctcRunNumber";
            this.nudMctcRunNumber.Size = new System.Drawing.Size(54, 22);
            this.nudMctcRunNumber.TabIndex = 8;
            this.nudMctcRunNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMctcRunNumber.ValueChanged += new System.EventHandler(this.nudMctcRunNumber_ValueChanged);
            // 
            // ckbxExcludeIrrelevantBatches
            // 
            this.ckbxExcludeIrrelevantBatches.AutoSize = true;
            this.ckbxExcludeIrrelevantBatches.Checked = true;
            this.ckbxExcludeIrrelevantBatches.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbxExcludeIrrelevantBatches.Location = new System.Drawing.Point(232, 38);
            this.ckbxExcludeIrrelevantBatches.Name = "ckbxExcludeIrrelevantBatches";
            this.ckbxExcludeIrrelevantBatches.Size = new System.Drawing.Size(158, 17);
            this.ckbxExcludeIrrelevantBatches.TabIndex = 7;
            this.ckbxExcludeIrrelevantBatches.Text = "Exclude Irrelevant Batches";
            this.ckbxExcludeIrrelevantBatches.UseVisualStyleBackColor = true;
            this.ckbxExcludeIrrelevantBatches.CheckedChanged += new System.EventHandler(this.ckbxExcludeIrrelevantBatches_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Mctc Run No.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Model Variable";
            // 
            // cmbxModelVariable
            // 
            this.cmbxModelVariable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbxModelVariable.FormattingEnabled = true;
            this.cmbxModelVariable.Location = new System.Drawing.Point(105, 35);
            this.cmbxModelVariable.Name = "cmbxModelVariable";
            this.cmbxModelVariable.Size = new System.Drawing.Size(121, 21);
            this.cmbxModelVariable.TabIndex = 5;
            this.cmbxModelVariable.SelectedIndexChanged += new System.EventHandler(this.cmbxModelVariable_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Batch Details";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Mctc Details";
            // 
            // ccMctcPlot
            // 
            this.ccMctcPlot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ccMctcPlot.Location = new System.Drawing.Point(3, 62);
            this.ccMctcPlot.Name = "ccMctcPlot";
            this.ccMctcPlot.Size = new System.Drawing.Size(615, 593);
            this.ccMctcPlot.TabIndex = 4;
            this.ccMctcPlot.UseLabelFiltering = false;
            // 
            // tcMctcDetails
            // 
            this.tcMctcDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcMctcDetails.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcMctcDetails.Location = new System.Drawing.Point(12, 21);
            this.tcMctcDetails.MultiSelect = true;
            this.tcMctcDetails.Name = "tcMctcDetails";
            this.tcMctcDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect;
            this.tcMctcDetails.Size = new System.Drawing.Size(485, 53);
            this.tcMctcDetails.TabIndex = 9;
            // 
            // tcBatchDetails
            // 
            this.tcBatchDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcBatchDetails.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcBatchDetails.Location = new System.Drawing.Point(12, 93);
            this.tcBatchDetails.MultiSelect = true;
            this.tcBatchDetails.Name = "tcBatchDetails";
            this.tcBatchDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect;
            this.tcBatchDetails.Size = new System.Drawing.Size(485, 562);
            this.tcBatchDetails.TabIndex = 8;
            // 
            // FrmReadMctc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1363, 766);
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
            this.tabpageOverview.ResumeLayout(false);
            this.tabpageOverview.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudMctcRunNumber)).EndInit();
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
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.NumericUpDown nudMctcRunNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private CustomControls.TableControl tcMctcDetails;
        private System.Windows.Forms.Label label4;
    }
}