namespace DatabaseReader
{
    partial class FrmMain
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
            this.btnReadExperiments = new System.Windows.Forms.Button();
            this.cklbxExperiments = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnScanDirectory = new System.Windows.Forms.Button();
            this.cmbxExperiments = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbxManualSelection = new System.Windows.Forms.ComboBox();
            this.ckbxManualSelection = new System.Windows.Forms.CheckBox();
            this.cmbxPhases = new System.Windows.Forms.ComboBox();
            this.ckbxSubDirectory = new System.Windows.Forms.CheckBox();
            this.txtSubDirectory = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tcExperiment = new DatabaseReader.CustomControls.TableControl();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tcPhases = new DatabaseReader.CustomControls.TableControl();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageDataManager = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnSubselectRuns = new System.Windows.Forms.Button();
            this.tabPageSubselectRuns = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cmbxRunFilterType = new System.Windows.Forms.ComboBox();
            this.btnGenerateRuns = new System.Windows.Forms.Button();
            this.flpFilters = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddRunFilterType = new System.Windows.Forms.Button();
            this.tcFilteredRuns = new DatabaseReader.CustomControls.TableControl();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageDataManager.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabPageSubselectRuns.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnReadExperiments
            // 
            this.btnReadExperiments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReadExperiments.Location = new System.Drawing.Point(5, 559);
            this.btnReadExperiments.Name = "btnReadExperiments";
            this.btnReadExperiments.Size = new System.Drawing.Size(181, 23);
            this.btnReadExperiments.TabIndex = 2;
            this.btnReadExperiments.Text = "Read Selected Experiments";
            this.btnReadExperiments.UseVisualStyleBackColor = true;
            this.btnReadExperiments.Click += new System.EventHandler(this.btnReadExperiments_Click);
            // 
            // cklbxExperiments
            // 
            this.cklbxExperiments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.cklbxExperiments.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.cklbxExperiments.CheckOnClick = true;
            this.cklbxExperiments.FormattingEnabled = true;
            this.cklbxExperiments.HorizontalScrollbar = true;
            this.cklbxExperiments.Location = new System.Drawing.Point(6, 22);
            this.cklbxExperiments.Name = "cklbxExperiments";
            this.cklbxExperiments.Size = new System.Drawing.Size(181, 476);
            this.cklbxExperiments.TabIndex = 3;
            this.cklbxExperiments.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cklbxExperiments_MouseDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Directory";
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(87, 12);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(629, 22);
            this.txtPath.TabIndex = 1;
            // 
            // btnScanDirectory
            // 
            this.btnScanDirectory.Location = new System.Drawing.Point(722, 12);
            this.btnScanDirectory.Name = "btnScanDirectory";
            this.btnScanDirectory.Size = new System.Drawing.Size(75, 23);
            this.btnScanDirectory.TabIndex = 2;
            this.btnScanDirectory.Text = "Scan";
            this.btnScanDirectory.UseVisualStyleBackColor = true;
            this.btnScanDirectory.Click += new System.EventHandler(this.btnScanDirectory_Click);
            // 
            // cmbxExperiments
            // 
            this.cmbxExperiments.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbxExperiments.FormattingEnabled = true;
            this.cmbxExperiments.Location = new System.Drawing.Point(6, 22);
            this.cmbxExperiments.Name = "cmbxExperiments";
            this.cmbxExperiments.Size = new System.Drawing.Size(201, 21);
            this.cmbxExperiments.TabIndex = 4;
            this.cmbxExperiments.SelectedIndexChanged += new System.EventHandler(this.cmbxExperiments_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.cmbxManualSelection);
            this.groupBox1.Controls.Add(this.ckbxManualSelection);
            this.groupBox1.Controls.Add(this.btnReadExperiments);
            this.groupBox1.Controls.Add(this.cklbxExperiments);
            this.groupBox1.Location = new System.Drawing.Point(3, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(195, 588);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Studies with data";
            // 
            // cmbxManualSelection
            // 
            this.cmbxManualSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbxManualSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbxManualSelection.FormattingEnabled = true;
            this.cmbxManualSelection.Location = new System.Drawing.Point(5, 532);
            this.cmbxManualSelection.Name = "cmbxManualSelection";
            this.cmbxManualSelection.Size = new System.Drawing.Size(181, 21);
            this.cmbxManualSelection.TabIndex = 7;
            this.cmbxManualSelection.SelectedIndexChanged += new System.EventHandler(this.cmbxManualSelection_SelectedIndexChanged);
            // 
            // ckbxManualSelection
            // 
            this.ckbxManualSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ckbxManualSelection.AutoSize = true;
            this.ckbxManualSelection.Location = new System.Drawing.Point(7, 509);
            this.ckbxManualSelection.Name = "ckbxManualSelection";
            this.ckbxManualSelection.Size = new System.Drawing.Size(115, 17);
            this.ckbxManualSelection.TabIndex = 7;
            this.ckbxManualSelection.Text = "Manual Selection";
            this.ckbxManualSelection.UseVisualStyleBackColor = true;
            this.ckbxManualSelection.CheckedChanged += new System.EventHandler(this.ckbxManualSelection_CheckedChanged);
            // 
            // cmbxPhases
            // 
            this.cmbxPhases.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbxPhases.FormattingEnabled = true;
            this.cmbxPhases.Location = new System.Drawing.Point(6, 21);
            this.cmbxPhases.Name = "cmbxPhases";
            this.cmbxPhases.Size = new System.Drawing.Size(201, 21);
            this.cmbxPhases.TabIndex = 4;
            this.cmbxPhases.SelectedIndexChanged += new System.EventHandler(this.cmbxPhases_SelectedIndexChanged);
            // 
            // ckbxSubDirectory
            // 
            this.ckbxSubDirectory.AutoSize = true;
            this.ckbxSubDirectory.Checked = true;
            this.ckbxSubDirectory.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbxSubDirectory.Location = new System.Drawing.Point(803, 16);
            this.ckbxSubDirectory.Name = "ckbxSubDirectory";
            this.ckbxSubDirectory.Size = new System.Drawing.Size(132, 17);
            this.ckbxSubDirectory.TabIndex = 8;
            this.ckbxSubDirectory.Text = "Include Subdirectory";
            this.ckbxSubDirectory.UseVisualStyleBackColor = true;
            this.ckbxSubDirectory.CheckedChanged += new System.EventHandler(this.ckbxSubDirectory_CheckedChanged);
            // 
            // txtSubDirectory
            // 
            this.txtSubDirectory.Location = new System.Drawing.Point(932, 13);
            this.txtSubDirectory.Name = "txtSubDirectory";
            this.txtSubDirectory.Size = new System.Drawing.Size(100, 22);
            this.txtSubDirectory.TabIndex = 9;
            this.txtSubDirectory.Text = "MH";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmbxExperiments);
            this.groupBox2.Controls.Add(this.tcExperiment);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1039, 279);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Overview";
            // 
            // tcExperiment
            // 
            this.tcExperiment.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcExperiment.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcExperiment.Location = new System.Drawing.Point(6, 49);
            this.tcExperiment.Name = "tcExperiment";
            this.tcExperiment.Size = new System.Drawing.Size(1027, 224);
            this.tcExperiment.TabIndex = 5;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tcPhases);
            this.groupBox3.Controls.Add(this.cmbxPhases);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1039, 276);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Phases";
            // 
            // tcPhases
            // 
            this.tcPhases.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcPhases.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcPhases.Location = new System.Drawing.Point(6, 48);
            this.tcPhases.Name = "tcPhases";
            this.tcPhases.Size = new System.Drawing.Size(1027, 222);
            this.tcPhases.TabIndex = 5;
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPageDataManager);
            this.tabControl.Controls.Add(this.tabPageSubselectRuns);
            this.tabControl.Location = new System.Drawing.Point(16, 41);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1257, 632);
            this.tabControl.TabIndex = 10;
            this.tabControl.Visible = false;
            // 
            // tabPageDataManager
            // 
            this.tabPageDataManager.Controls.Add(this.splitContainer1);
            this.tabPageDataManager.Controls.Add(this.btnSubselectRuns);
            this.tabPageDataManager.Controls.Add(this.groupBox1);
            this.tabPageDataManager.Location = new System.Drawing.Point(4, 22);
            this.tabPageDataManager.Name = "tabPageDataManager";
            this.tabPageDataManager.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDataManager.Size = new System.Drawing.Size(1249, 606);
            this.tabPageDataManager.TabIndex = 0;
            this.tabPageDataManager.Text = "Data Manager";
            this.tabPageDataManager.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(204, 6);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer1.Size = new System.Drawing.Size(1039, 559);
            this.splitContainer1.SplitterDistance = 279;
            this.splitContainer1.TabIndex = 12;
            // 
            // btnSubselectRuns
            // 
            this.btnSubselectRuns.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSubselectRuns.Location = new System.Drawing.Point(1107, 571);
            this.btnSubselectRuns.Name = "btnSubselectRuns";
            this.btnSubselectRuns.Size = new System.Drawing.Size(136, 23);
            this.btnSubselectRuns.TabIndex = 11;
            this.btnSubselectRuns.Text = "Subselect Runs >>";
            this.btnSubselectRuns.UseVisualStyleBackColor = true;
            this.btnSubselectRuns.Click += new System.EventHandler(this.btnSubselectRuns_Click);
            // 
            // tabPageSubselectRuns
            // 
            this.tabPageSubselectRuns.Controls.Add(this.groupBox4);
            this.tabPageSubselectRuns.Controls.Add(this.tcFilteredRuns);
            this.tabPageSubselectRuns.Location = new System.Drawing.Point(4, 22);
            this.tabPageSubselectRuns.Name = "tabPageSubselectRuns";
            this.tabPageSubselectRuns.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSubselectRuns.Size = new System.Drawing.Size(1249, 606);
            this.tabPageSubselectRuns.TabIndex = 1;
            this.tabPageSubselectRuns.Text = "Subselect Runs";
            this.tabPageSubselectRuns.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox4.Controls.Add(this.cmbxRunFilterType);
            this.groupBox4.Controls.Add(this.btnGenerateRuns);
            this.groupBox4.Controls.Add(this.flpFilters);
            this.groupBox4.Controls.Add(this.btnAddRunFilterType);
            this.groupBox4.Location = new System.Drawing.Point(6, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(213, 594);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Add Filters";
            // 
            // cmbxRunFilterType
            // 
            this.cmbxRunFilterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbxRunFilterType.FormattingEnabled = true;
            this.cmbxRunFilterType.Location = new System.Drawing.Point(6, 21);
            this.cmbxRunFilterType.Name = "cmbxRunFilterType";
            this.cmbxRunFilterType.Size = new System.Drawing.Size(153, 21);
            this.cmbxRunFilterType.TabIndex = 3;
            // 
            // btnGenerateRuns
            // 
            this.btnGenerateRuns.Location = new System.Drawing.Point(6, 565);
            this.btnGenerateRuns.Name = "btnGenerateRuns";
            this.btnGenerateRuns.Size = new System.Drawing.Size(200, 23);
            this.btnGenerateRuns.TabIndex = 5;
            this.btnGenerateRuns.Text = "Apply and Generate";
            this.btnGenerateRuns.UseVisualStyleBackColor = true;
            this.btnGenerateRuns.Click += new System.EventHandler(this.btnGenerateRuns_Click);
            // 
            // flpFilters
            // 
            this.flpFilters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpFilters.Location = new System.Drawing.Point(6, 47);
            this.flpFilters.Name = "flpFilters";
            this.flpFilters.Size = new System.Drawing.Size(200, 512);
            this.flpFilters.TabIndex = 0;
            // 
            // btnAddRunFilterType
            // 
            this.btnAddRunFilterType.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddRunFilterType.Location = new System.Drawing.Point(162, 20);
            this.btnAddRunFilterType.Name = "btnAddRunFilterType";
            this.btnAddRunFilterType.Size = new System.Drawing.Size(44, 23);
            this.btnAddRunFilterType.TabIndex = 4;
            this.btnAddRunFilterType.Text = "Add";
            this.btnAddRunFilterType.UseVisualStyleBackColor = true;
            this.btnAddRunFilterType.Click += new System.EventHandler(this.btnAddRunFilterType_Click);
            // 
            // tcFilteredRuns
            // 
            this.tcFilteredRuns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcFilteredRuns.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tcFilteredRuns.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcFilteredRuns.Location = new System.Drawing.Point(225, 26);
            this.tcFilteredRuns.Name = "tcFilteredRuns";
            this.tcFilteredRuns.Size = new System.Drawing.Size(1018, 539);
            this.tcFilteredRuns.TabIndex = 1;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1285, 686);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.txtSubDirectory);
            this.Controls.Add(this.ckbxSubDirectory);
            this.Controls.Add(this.btnScanDirectory);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.label2);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DatabaseReader";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPageDataManager.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabPageSubselectRuns.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnReadExperiments;
        private System.Windows.Forms.CheckedListBox cklbxExperiments;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnScanDirectory;
        private System.Windows.Forms.ComboBox cmbxExperiments;
        private CustomControls.TableControl tcExperiment;
        private System.Windows.Forms.CheckBox ckbxSubDirectory;
        private System.Windows.Forms.TextBox txtSubDirectory;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbxPhases;
        private CustomControls.TableControl tcPhases;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageDataManager;
        private System.Windows.Forms.ComboBox cmbxManualSelection;
        private System.Windows.Forms.CheckBox ckbxManualSelection;
        private System.Windows.Forms.TabPage tabPageSubselectRuns;
        private System.Windows.Forms.Button btnSubselectRuns;
        private System.Windows.Forms.Button btnAddRunFilterType;
        private System.Windows.Forms.ComboBox cmbxRunFilterType;
        private CustomControls.TableControl tcFilteredRuns;
        private System.Windows.Forms.FlowLayoutPanel flpFilters;
        private System.Windows.Forms.Button btnGenerateRuns;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}

