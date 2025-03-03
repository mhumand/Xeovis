namespace DatabaseReader
{
    partial class FrmVariableDefinitions
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
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowseAndRead = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.cmbxPhase = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cklbOxides = new System.Windows.Forms.CheckedListBox();
            this.lblPhaseFullName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(91, 12);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(572, 22);
            this.txtFilePath.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "ax-File";
            // 
            // btnBrowseAndRead
            // 
            this.btnBrowseAndRead.Location = new System.Drawing.Point(669, 12);
            this.btnBrowseAndRead.Name = "btnBrowseAndRead";
            this.btnBrowseAndRead.Size = new System.Drawing.Size(106, 22);
            this.btnBrowseAndRead.TabIndex = 2;
            this.btnBrowseAndRead.Text = "Browse and Read";
            this.btnBrowseAndRead.UseVisualStyleBackColor = true;
            this.btnBrowseAndRead.Click += new System.EventHandler(this.btnBrowseAndRead_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(619, 438);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(700, 438);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cmbxPhase
            // 
            this.cmbxPhase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbxPhase.FormattingEnabled = true;
            this.cmbxPhase.Location = new System.Drawing.Point(91, 50);
            this.cmbxPhase.Name = "cmbxPhase";
            this.cmbxPhase.Size = new System.Drawing.Size(123, 21);
            this.cmbxPhase.TabIndex = 4;
            this.cmbxPhase.SelectedIndexChanged += new System.EventHandler(this.cmbxPhase_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Phase";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Composition";
            // 
            // cklbOxides
            // 
            this.cklbOxides.ColumnWidth = 60;
            this.cklbOxides.FormattingEnabled = true;
            this.cklbOxides.Location = new System.Drawing.Point(91, 77);
            this.cklbOxides.MultiColumn = true;
            this.cklbOxides.Name = "cklbOxides";
            this.cklbOxides.Size = new System.Drawing.Size(683, 21);
            this.cklbOxides.TabIndex = 5;
            // 
            // lblPhaseFullName
            // 
            this.lblPhaseFullName.AutoSize = true;
            this.lblPhaseFullName.Location = new System.Drawing.Point(220, 53);
            this.lblPhaseFullName.Name = "lblPhaseFullName";
            this.lblPhaseFullName.Size = new System.Drawing.Size(16, 13);
            this.lblPhaseFullName.TabIndex = 1;
            this.lblPhaseFullName.Text = "...";
            // 
            // FrmVariableDefinitions
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(787, 473);
            this.Controls.Add(this.cklbOxides);
            this.Controls.Add(this.cmbxPhase);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnBrowseAndRead);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblPhaseFullName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFilePath);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmVariableDefinitions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Variable Definitions";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowseAndRead;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ComboBox cmbxPhase;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckedListBox cklbOxides;
        private System.Windows.Forms.Label lblPhaseFullName;
    }
}