namespace TestingWinForms
{
    partial class Form1
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
            this.lbl_clickMessage = new System.Windows.Forms.Label();
            this.btn_click = new System.Windows.Forms.Button();
            this.labelYAxis = new System.Windows.Forms.Label();
            this.labelXAxis = new System.Windows.Forms.Label();
            this.btnAdmin = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.labelTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_clickMessage
            // 
            this.lbl_clickMessage.AutoSize = true;
            this.lbl_clickMessage.Location = new System.Drawing.Point(587, 82);
            this.lbl_clickMessage.Name = "lbl_clickMessage";
            this.lbl_clickMessage.Size = new System.Drawing.Size(46, 17);
            this.lbl_clickMessage.TabIndex = 2;
            this.lbl_clickMessage.Text = "label1";
            // 
            // btn_click
            // 
            this.btn_click.Location = new System.Drawing.Point(541, 110);
            this.btn_click.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_click.Name = "btn_click";
            this.btn_click.Size = new System.Drawing.Size(137, 54);
            this.btn_click.TabIndex = 0;
            this.btn_click.Text = "Test btn";
            this.btn_click.UseVisualStyleBackColor = true;
            this.btn_click.Click += new System.EventHandler(this.button1_Click);
            // 
            // labelYAxis
            // 
            this.labelYAxis.AutoSize = true;
            this.labelYAxis.Location = new System.Drawing.Point(48, 163);
            this.labelYAxis.Name = "labelYAxis";
            this.labelYAxis.Size = new System.Drawing.Size(47, 17);
            this.labelYAxis.TabIndex = 3;
            this.labelYAxis.Text = "Text 1";
            // 
            // labelXAxis
            // 
            this.labelXAxis.AutoSize = true;
            this.labelXAxis.Location = new System.Drawing.Point(209, 321);
            this.labelXAxis.Name = "labelXAxis";
            this.labelXAxis.Size = new System.Drawing.Size(47, 17);
            this.labelXAxis.TabIndex = 4;
            this.labelXAxis.Text = "Text 2";
            // 
            // btnAdmin
            // 
            this.btnAdmin.Location = new System.Drawing.Point(548, 284);
            this.btnAdmin.Name = "btnAdmin";
            this.btnAdmin.Size = new System.Drawing.Size(129, 53);
            this.btnAdmin.TabIndex = 5;
            this.btnAdmin.Text = "Admin";
            this.btnAdmin.UseVisualStyleBackColor = true;
            this.btnAdmin.Click += new System.EventHandler(this.btnAdmin_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(558, 256);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 22);
            this.textBox1.TabIndex = 6;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Location = new System.Drawing.Point(212, 30);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(46, 17);
            this.labelTitle.TabIndex = 7;
            this.labelTitle.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 360);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnAdmin);
            this.Controls.Add(this.labelXAxis);
            this.Controls.Add(this.labelYAxis);
            this.Controls.Add(this.lbl_clickMessage);
            this.Controls.Add(this.btn_click);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lbl_clickMessage;
        private System.Windows.Forms.Button btn_click;
        private System.Windows.Forms.Label labelYAxis;
        private System.Windows.Forms.Label labelXAxis;
        private System.Windows.Forms.Button btnAdmin;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label labelTitle;
    }
}

