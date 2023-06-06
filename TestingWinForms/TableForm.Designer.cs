namespace TestingWinForms
{
    partial class TableForm
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
            this.labelTitle = new System.Windows.Forms.Label();
            this.nextButton = new System.Windows.Forms.Button();
            this.labelYAxis2 = new System.Windows.Forms.Label();
            this.labelXAxis2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelYAxis
            // 
            this.labelYAxis.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelYAxis.AutoSize = true;
            this.labelYAxis.BackColor = System.Drawing.Color.Transparent;
            this.labelYAxis.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelYAxis.Location = new System.Drawing.Point(50, 163);
            this.labelYAxis.Margin = new System.Windows.Forms.Padding(10, 0, 3, 0);
            this.labelYAxis.Name = "labelYAxis";
            this.labelYAxis.Size = new System.Drawing.Size(97, 36);
            this.labelYAxis.TabIndex = 3;
            this.labelYAxis.Text = "Text 1";
            this.labelYAxis.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelXAxis
            // 
            this.labelXAxis.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.labelXAxis.AutoSize = true;
            this.labelXAxis.BackColor = System.Drawing.Color.Transparent;
            this.labelXAxis.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelXAxis.Location = new System.Drawing.Point(253, 242);
            this.labelXAxis.Margin = new System.Windows.Forms.Padding(3, 0, 3, 25);
            this.labelXAxis.Name = "labelXAxis";
            this.labelXAxis.Size = new System.Drawing.Size(97, 36);
            this.labelXAxis.TabIndex = 4;
            this.labelXAxis.Text = "Text 2";
            this.labelXAxis.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.Location = new System.Drawing.Point(53, 48);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(3, 25, 3, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(97, 38);
            this.labelTitle.TabIndex = 7;
            this.labelTitle.Text = "label1";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nextButton
            // 
            this.nextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nextButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextButton.Location = new System.Drawing.Point(517, 264);
            this.nextButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(133, 46);
            this.nextButton.TabIndex = 8;
            this.nextButton.Text = "Next";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // labelYAxis2
            // 
            this.labelYAxis2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelYAxis2.AutoSize = true;
            this.labelYAxis2.BackColor = System.Drawing.Color.Transparent;
            this.labelYAxis2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelYAxis2.Location = new System.Drawing.Point(228, 163);
            this.labelYAxis2.Margin = new System.Windows.Forms.Padding(10, 0, 3, 0);
            this.labelYAxis2.Name = "labelYAxis2";
            this.labelYAxis2.Size = new System.Drawing.Size(173, 36);
            this.labelYAxis2.TabIndex = 9;
            this.labelYAxis2.Text = "labelYAxis2";
            this.labelYAxis2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelXAxis2
            // 
            this.labelXAxis2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.labelXAxis2.AutoSize = true;
            this.labelXAxis2.BackColor = System.Drawing.Color.Transparent;
            this.labelXAxis2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelXAxis2.Location = new System.Drawing.Point(305, 34);
            this.labelXAxis2.Margin = new System.Windows.Forms.Padding(3, 0, 3, 25);
            this.labelXAxis2.Name = "labelXAxis2";
            this.labelXAxis2.Size = new System.Drawing.Size(174, 36);
            this.labelXAxis2.TabIndex = 10;
            this.labelXAxis2.Text = "labelXAxis2";
            this.labelXAxis2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 360);
            this.Controls.Add(this.labelXAxis2);
            this.Controls.Add(this.labelYAxis2);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.labelXAxis);
            this.Controls.Add(this.labelYAxis);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "TableForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelYAxis;
        private System.Windows.Forms.Label labelXAxis;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.Label labelYAxis2;
        private System.Windows.Forms.Label labelXAxis2;
    }
}

