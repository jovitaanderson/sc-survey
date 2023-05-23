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
            this.labelYAxis = new System.Windows.Forms.Label();
            this.labelXAxis = new System.Windows.Forms.Label();
            this.btnAdmin = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.labelTitle = new System.Windows.Forms.Label();
            this.lbl_clickMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelYAxis
            // 
            this.labelYAxis.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelYAxis.AutoSize = true;
            this.labelYAxis.BackColor = System.Drawing.Color.Transparent;
            this.labelYAxis.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelYAxis.Location = new System.Drawing.Point(24, 162);
            this.labelYAxis.Name = "labelYAxis";
            this.labelYAxis.Size = new System.Drawing.Size(79, 29);
            this.labelYAxis.TabIndex = 3;
            this.labelYAxis.Text = "Text 1";
            // 
            // labelXAxis
            // 
            this.labelXAxis.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelXAxis.AutoSize = true;
            this.labelXAxis.BackColor = System.Drawing.Color.Transparent;
            this.labelXAxis.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelXAxis.Location = new System.Drawing.Point(212, 297);
            this.labelXAxis.Name = "labelXAxis";
            this.labelXAxis.Size = new System.Drawing.Size(79, 29);
            this.labelXAxis.TabIndex = 4;
            this.labelXAxis.Text = "Text 2";
            // 
            // btnAdmin
            // 
            this.btnAdmin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdmin.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdmin.Location = new System.Drawing.Point(571, 307);
            this.btnAdmin.Name = "btnAdmin";
            this.btnAdmin.Size = new System.Drawing.Size(106, 41);
            this.btnAdmin.TabIndex = 5;
            this.btnAdmin.Text = "Admin";
            this.btnAdmin.UseVisualStyleBackColor = true;
            this.btnAdmin.Click += new System.EventHandler(this.btnAdmin_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(571, 279);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(106, 22);
            this.textBox1.TabIndex = 6;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.Location = new System.Drawing.Point(212, 30);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(79, 29);
            this.labelTitle.TabIndex = 7;
            this.labelTitle.Text = "label1";
            // 
            // lbl_clickMessage
            // 
            this.lbl_clickMessage.AutoSize = true;
            this.lbl_clickMessage.BackColor = System.Drawing.Color.Transparent;
            this.lbl_clickMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_clickMessage.Location = new System.Drawing.Point(592, 56);
            this.lbl_clickMessage.Name = "lbl_clickMessage";
            this.lbl_clickMessage.Size = new System.Drawing.Size(79, 29);
            this.lbl_clickMessage.TabIndex = 8;
            this.lbl_clickMessage.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 360);
            this.Controls.Add(this.lbl_clickMessage);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnAdmin);
            this.Controls.Add(this.labelXAxis);
            this.Controls.Add(this.labelYAxis);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelYAxis;
        private System.Windows.Forms.Label labelXAxis;
        private System.Windows.Forms.Button btnAdmin;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label lbl_clickMessage;
    }
}

