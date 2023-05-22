namespace TestingWinForms
{
    partial class Form2
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
            this.questionLabel = new System.Windows.Forms.Label();
            this.submitButton = new System.Windows.Forms.Button();
            this.optionACheckBox = new System.Windows.Forms.CheckBox();
            this.optionBCheckBox = new System.Windows.Forms.CheckBox();
            this.optionCCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // questionLabel
            // 
            this.questionLabel.AutoSize = true;
            this.questionLabel.Location = new System.Drawing.Point(226, 130);
            this.questionLabel.Name = "questionLabel";
            this.questionLabel.Size = new System.Drawing.Size(70, 20);
            this.questionLabel.TabIndex = 4;
            this.questionLabel.Text = "question";
            // 
            // submitButton
            // 
            this.submitButton.Location = new System.Drawing.Point(603, 335);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(92, 46);
            this.submitButton.TabIndex = 5;
            this.submitButton.Text = "Submit";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.submitButton_Click);
            // 
            // optionACheckBox
            // 
            this.optionACheckBox.AutoSize = true;
            this.optionACheckBox.Location = new System.Drawing.Point(230, 174);
            this.optionACheckBox.Name = "optionACheckBox";
            this.optionACheckBox.Size = new System.Drawing.Size(113, 24);
            this.optionACheckBox.TabIndex = 6;
            this.optionACheckBox.Text = "checkBox1";
            this.optionACheckBox.UseVisualStyleBackColor = true;
            // 
            // optionBCheckBox
            // 
            this.optionBCheckBox.AutoSize = true;
            this.optionBCheckBox.Location = new System.Drawing.Point(230, 206);
            this.optionBCheckBox.Name = "optionBCheckBox";
            this.optionBCheckBox.Size = new System.Drawing.Size(113, 24);
            this.optionBCheckBox.TabIndex = 7;
            this.optionBCheckBox.Text = "checkBox2";
            this.optionBCheckBox.UseVisualStyleBackColor = true;
            // 
            // optionCCheckBox
            // 
            this.optionCCheckBox.AutoSize = true;
            this.optionCCheckBox.Location = new System.Drawing.Point(230, 236);
            this.optionCCheckBox.Name = "optionCCheckBox";
            this.optionCCheckBox.Size = new System.Drawing.Size(113, 24);
            this.optionCCheckBox.TabIndex = 8;
            this.optionCCheckBox.Text = "checkBox3";
            this.optionCCheckBox.UseVisualStyleBackColor = true;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.optionCCheckBox);
            this.Controls.Add(this.optionBCheckBox);
            this.Controls.Add(this.optionACheckBox);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.questionLabel);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label questionLabel;
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.CheckBox optionACheckBox;
        private System.Windows.Forms.CheckBox optionBCheckBox;
        private System.Windows.Forms.CheckBox optionCCheckBox;
    }
}