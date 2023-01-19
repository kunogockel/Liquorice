namespace Liquorice
{
    partial class MainDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainDlg));
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblFileName = new System.Windows.Forms.Label();
            this.txtHelp = new System.Windows.Forms.TextBox();
            this.txtEditor = new Liquorice.Editor();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel1.Controls.Add(this.lblFileName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.ForeColor = System.Drawing.Color.Black;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(670, 22);
            this.panel1.TabIndex = 2;
            // 
            // lblFileName
            // 
            this.lblFileName.Font = new System.Drawing.Font("Cascadia Code", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFileName.ForeColor = System.Drawing.Color.DimGray;
            this.lblFileName.Location = new System.Drawing.Point(0, 0);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(261, 21);
            this.lblFileName.TabIndex = 0;
            this.lblFileName.Text = "label1";
            this.lblFileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtHelp
            // 
            this.txtHelp.BackColor = System.Drawing.SystemColors.Control;
            this.txtHelp.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtHelp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHelp.Font = new System.Drawing.Font("Cascadia Code", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHelp.ForeColor = System.Drawing.Color.Black;
            this.txtHelp.Location = new System.Drawing.Point(0, 22);
            this.txtHelp.Multiline = true;
            this.txtHelp.Name = "txtHelp";
            this.txtHelp.ReadOnly = true;
            this.txtHelp.Size = new System.Drawing.Size(670, 421);
            this.txtHelp.TabIndex = 3;
            this.txtHelp.Visible = false;
            this.txtHelp.WordWrap = false;
            // 
            // txtEditor
            // 
            this.txtEditor.BackColor = System.Drawing.SystemColors.Control;
            this.txtEditor.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEditor.Font = new System.Drawing.Font("Cascadia Code", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEditor.ForeColor = System.Drawing.Color.Black;
            this.txtEditor.Location = new System.Drawing.Point(0, 22);
            this.txtEditor.Multiline = true;
            this.txtEditor.Name = "txtEditor";
            this.txtEditor.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtEditor.Size = new System.Drawing.Size(670, 421);
            this.txtEditor.TabIndex = 1;
            this.txtEditor.WordWrap = false;
            // 
            // MainDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(670, 443);
            this.Controls.Add(this.txtHelp);
            this.Controls.Add(this.txtEditor);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainDlg";
            this.Text = "Liquorice";
            this.Shown += new System.EventHandler(this.MainDlg_Shown);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Editor txtEditor;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.TextBox txtHelp;
    }
}

