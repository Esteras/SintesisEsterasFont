namespace ProjecteSintesis
{
    partial class FrmClient
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
            System.Windows.Forms.Label nALBARALabel;
            System.Windows.Forms.Label dATAALBARALabel;
            System.Windows.Forms.Label cODICLIENTLabel;
            System.Windows.Forms.Label nIFLabel;
            System.Windows.Forms.Label nOMLabel;
            System.Windows.Forms.Label dIRECCIOLabel;
            System.Windows.Forms.Label pOBLACIOLabel;
            this.nALBARATextBox = new System.Windows.Forms.TextBox();
            nALBARALabel = new System.Windows.Forms.Label();
            dATAALBARALabel = new System.Windows.Forms.Label();
            cODICLIENTLabel = new System.Windows.Forms.Label();
            nIFLabel = new System.Windows.Forms.Label();
            nOMLabel = new System.Windows.Forms.Label();
            dIRECCIOLabel = new System.Windows.Forms.Label();
            pOBLACIOLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // nALBARALabel
            // 
            nALBARALabel.AutoSize = true;
            nALBARALabel.Location = new System.Drawing.Point(-154, -70);
            nALBARALabel.Name = "nALBARALabel";
            nALBARALabel.Size = new System.Drawing.Size(60, 13);
            nALBARALabel.TabIndex = 55;
            nALBARALabel.Text = "NALBARA:";
            // 
            // dATAALBARALabel
            // 
            dATAALBARALabel.AutoSize = true;
            dATAALBARALabel.Location = new System.Drawing.Point(-154, -43);
            dATAALBARALabel.Name = "dATAALBARALabel";
            dATAALBARALabel.Size = new System.Drawing.Size(81, 13);
            dATAALBARALabel.TabIndex = 56;
            dATAALBARALabel.Text = "DATAALBARA:";
            // 
            // cODICLIENTLabel
            // 
            cODICLIENTLabel.AutoSize = true;
            cODICLIENTLabel.Location = new System.Drawing.Point(-154, -18);
            cODICLIENTLabel.Name = "cODICLIENTLabel";
            cODICLIENTLabel.Size = new System.Drawing.Size(74, 13);
            cODICLIENTLabel.TabIndex = 57;
            cODICLIENTLabel.Text = "CODICLIENT:";
            // 
            // nIFLabel
            // 
            nIFLabel.AutoSize = true;
            nIFLabel.Location = new System.Drawing.Point(-154, 8);
            nIFLabel.Name = "nIFLabel";
            nIFLabel.Size = new System.Drawing.Size(27, 13);
            nIFLabel.TabIndex = 58;
            nIFLabel.Text = "NIF:";
            // 
            // nOMLabel
            // 
            nOMLabel.AutoSize = true;
            nOMLabel.Location = new System.Drawing.Point(-154, 34);
            nOMLabel.Name = "nOMLabel";
            nOMLabel.Size = new System.Drawing.Size(35, 13);
            nOMLabel.TabIndex = 59;
            nOMLabel.Text = "NOM:";
            // 
            // dIRECCIOLabel
            // 
            dIRECCIOLabel.AutoSize = true;
            dIRECCIOLabel.Location = new System.Drawing.Point(-154, 60);
            dIRECCIOLabel.Name = "dIRECCIOLabel";
            dIRECCIOLabel.Size = new System.Drawing.Size(61, 13);
            dIRECCIOLabel.TabIndex = 60;
            dIRECCIOLabel.Text = "DIRECCIO:";
            // 
            // pOBLACIOLabel
            // 
            pOBLACIOLabel.AutoSize = true;
            pOBLACIOLabel.Location = new System.Drawing.Point(-154, 86);
            pOBLACIOLabel.Name = "pOBLACIOLabel";
            pOBLACIOLabel.Size = new System.Drawing.Size(63, 13);
            pOBLACIOLabel.TabIndex = 61;
            pOBLACIOLabel.Text = "POBLACIO:";
            // 
            // nALBARATextBox
            // 
            this.nALBARATextBox.Location = new System.Drawing.Point(-67, -73);
            this.nALBARATextBox.Name = "nALBARATextBox";
            this.nALBARATextBox.Size = new System.Drawing.Size(173, 20);
            this.nALBARATextBox.TabIndex = 46;
            // 
            // FrmClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 366);
            this.Controls.Add(nALBARALabel);
            this.Controls.Add(this.nALBARATextBox);
            this.Controls.Add(dATAALBARALabel);
            this.Controls.Add(cODICLIENTLabel);
            this.Controls.Add(nIFLabel);
            this.Controls.Add(nOMLabel);
            this.Controls.Add(dIRECCIOLabel);
            this.Controls.Add(pOBLACIOLabel);
            this.Name = "FrmClient";
            this.Text = "FrmClient";
            this.Load += new System.EventHandler(this.FrmClient_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox nALBARATextBox;
    }
}