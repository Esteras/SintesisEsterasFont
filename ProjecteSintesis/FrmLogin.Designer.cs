namespace ProjecteSintesis
{
    partial class FrmLogin
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblLogin = new System.Windows.Forms.Label();
            this.lblUsr = new System.Windows.Forms.Label();
            this.lblPwd = new System.Windows.Forms.Label();
            this.txtUsr = new System.Windows.Forms.TextBox();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.btnAccedir = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblLogin
            // 
            this.lblLogin.AutoSize = true;
            this.lblLogin.Location = new System.Drawing.Point(12, 9);
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.Size = new System.Drawing.Size(40, 13);
            this.lblLogin.TabIndex = 0;
            this.lblLogin.Text = "LOGIN";
            // 
            // lblUsr
            // 
            this.lblUsr.AutoSize = true;
            this.lblUsr.Location = new System.Drawing.Point(28, 43);
            this.lblUsr.Name = "lblUsr";
            this.lblUsr.Size = new System.Drawing.Size(43, 13);
            this.lblUsr.TabIndex = 1;
            this.lblUsr.Text = "Usuari: ";
            // 
            // lblPwd
            // 
            this.lblPwd.AutoSize = true;
            this.lblPwd.Location = new System.Drawing.Point(12, 71);
            this.lblPwd.Name = "lblPwd";
            this.lblPwd.Size = new System.Drawing.Size(59, 13);
            this.lblPwd.TabIndex = 3;
            this.lblPwd.Text = "Password: ";
            // 
            // txtUsr
            // 
            this.txtUsr.Location = new System.Drawing.Point(77, 40);
            this.txtUsr.Name = "txtUsr";
            this.txtUsr.Size = new System.Drawing.Size(118, 20);
            this.txtUsr.TabIndex = 4;
            this.txtUsr.TextChanged += new System.EventHandler(this.txtUsr_TextChanged);
            // 
            // txtPwd
            // 
            this.txtPwd.Location = new System.Drawing.Point(77, 68);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.Size = new System.Drawing.Size(118, 20);
            this.txtPwd.TabIndex = 5;
            this.txtPwd.TextChanged += new System.EventHandler(this.txtPwd_TextChanged);
            // 
            // btnAccedir
            // 
            this.btnAccedir.Location = new System.Drawing.Point(12, 112);
            this.btnAccedir.Name = "btnAccedir";
            this.btnAccedir.Size = new System.Drawing.Size(75, 23);
            this.btnAccedir.TabIndex = 6;
            this.btnAccedir.Text = "Accedeix";
            this.btnAccedir.UseVisualStyleBackColor = true;
            this.btnAccedir.Click += new System.EventHandler(this.btnAccedir_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(130, 112);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Cancel·la";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FrmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(217, 161);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnAccedir);
            this.Controls.Add(this.txtPwd);
            this.Controls.Add(this.txtUsr);
            this.Controls.Add(this.lblPwd);
            this.Controls.Add(this.lblUsr);
            this.Controls.Add(this.lblLogin);
            this.Name = "FrmLogin";
            this.Text = "Validació";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmLogin_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmLogin_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLogin;
        private System.Windows.Forms.Label lblUsr;
        private System.Windows.Forms.Label lblPwd;
        private System.Windows.Forms.TextBox txtUsr;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.Button btnAccedir;
        private System.Windows.Forms.Button button2;
    }
}

