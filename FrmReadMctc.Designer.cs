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
            this.btnReadMctc = new System.Windows.Forms.Button();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // btnReadMctc
            // 
            this.btnReadMctc.Location = new System.Drawing.Point(25, 57);
            this.btnReadMctc.Name = "btnReadMctc";
            this.btnReadMctc.Size = new System.Drawing.Size(95, 23);
            this.btnReadMctc.TabIndex = 0;
            this.btnReadMctc.Text = "Read Mctc";
            this.btnReadMctc.UseVisualStyleBackColor = true;
            this.btnReadMctc.Click += new System.EventHandler(this.ReadMctc_Click);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(25, 101);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(204, 446);
            this.checkedListBox1.TabIndex = 1;
            // 
            // FrmReadMctc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1242, 769);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.btnReadMctc);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FrmReadMctc";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Read Mctc";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnReadMctc;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
    }
}